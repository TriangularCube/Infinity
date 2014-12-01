using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic; 

public partial class PimpMyEditorInspector2 : Editor
{
    [RunBeforeOnInspectorGUI]
    protected void GroupInspector()
    {
        GroupInspectorApply(first);
        GroupInspectorCascade(first);
    }

    protected void GroupInspectorApply(SP sp)
    {
        SerializedProperty p = serializedObject.FindProperty(sp.path);
        if (p != null)
        {
			if(!p.propertyPath.Contains("]") || (p.propertyPath.LastIndexOf("]") < p.propertyPath.LastIndexOf(".")))
			{	
	            System.Reflection.FieldInfo fi = GetSerializedPropertyFieldInfo(p);
	            if (fi != null)
	            {			
	                object[] att = fi.GetCustomAttributes(typeof(GroupAttribute), true);                
	                if (att.Length > 0)
	                {
	                    GroupAttribute g = (GroupAttribute)att[0];
	                    sp.gr = g.name == "" ? null : g.name;
	                    sp.gt = g.tooltip == "" ? null : g.tooltip;
	                }
	                else
	                {
	                    att = fi.GetCustomAttributes(typeof(Group), true);
	                    if (att.Length > 0)
	                    {
	                        Group g = (Group)att[0];
	                        sp.gr = g.name == "" ? null : g.name;
	                        sp.gt = g.tooltip == "" ? null : g.tooltip;
	                    }
	                    else
	                    {
	                        att = fi.GetCustomAttributes(typeof(GroupedAttribute), true);
	                        if (att.Length > 0)
	                        {
	                            GroupedAttribute g = (GroupedAttribute)att[0];
	                            sp.gr = g.name == "" ? null : g.name;
	                            sp.gt = g.tooltip == "" ? null : g.tooltip;
	                        }
	                    }
	                }
	            }
			}
        }

        for (int i = 0; i < sp.children.Count; i++)
        {
            GroupInspectorApply(sp.children[i]);
        }
    }

    protected void GroupInspectorCascade(SP sp)
    {        
        for (int i = 0; i < sp.children.Count; i++)
        {
            if (sp.children[i].gr != null)
            {
                List<string> help = new List<string>();
                if(sp.children[i].gt != null)
                    help.Add(sp.children[i].gt);

                int grouped = 0;
                for (int j = i+1; j < sp.children.Count; j++)
                {
                    if (sp.children[j].gr == sp.children[i].gr)
                    {
                        if (sp.children[j].gt != null && !help.Contains(sp.children[j].gt))
                            help.Add(sp.children[j].gt);

                        SP tmp = sp.children[j];
                        sp.children.RemoveAt(j);
                        sp.children.Insert(i + grouped + 1, tmp);
                        grouped++;
                    }
                }

                if (help.Count > 0)
                {
                    sp.children[i].gt = "";
                    for (int h = 0; h < help.Count; h++)
                    {
                        sp.children[i].gt += (i != 0 ? " " : "") + help[h];
                    }
                }

                i += grouped;
            }

            GroupInspectorCascade(sp.children[i]);
        }
    }
}
