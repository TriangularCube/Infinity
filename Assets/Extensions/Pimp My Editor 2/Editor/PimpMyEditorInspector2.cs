using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CanEditMultipleObjects()]
[CustomEditor(typeof(PimpedMonoBehaviour), true)]
public partial class PimpMyEditorInspector2 : Editor {

    protected static PimpMyEditorInspector2 instance;

    [System.AttributeUsage(System.AttributeTargets.Method)]
    private class RunOnEnableAttribute : System.Attribute { }
    
    [System.AttributeUsage(System.AttributeTargets.Method)]
    private class RunOnDisableAttribute : System.Attribute { }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    private class RunBeforeOnInspectorGUIAttribute : System.Attribute {
        public readonly int priority;
        public RunBeforeOnInspectorGUIAttribute()
        {
            priority = 0;
        }
        public RunBeforeOnInspectorGUIAttribute(int priority)
        {
            this.priority = priority;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    private class RunAfterOnInspectorGUIAttribute : System.Attribute { }
    
    List<System.Reflection.MethodInfo> runOnEnable;
    List<System.Reflection.MethodInfo> runOnDisable;    
    List<System.Reflection.MethodInfo> runBeforeOnInspectorGUI;
    List<System.Reflection.MethodInfo> runAfterOnInspectorGUI;

    void OnEnable()
    {
        PimpMyEditorInspector2.instance = this;

        runOnEnable = new List<System.Reflection.MethodInfo>();
        runOnDisable = new List<System.Reflection.MethodInfo>();        
        runBeforeOnInspectorGUI = new List<System.Reflection.MethodInfo>();
        runAfterOnInspectorGUI = new List<System.Reflection.MethodInfo>();

        System.Reflection.MethodInfo[] m = typeof(PimpMyEditorInspector2).GetMethods(
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Instance
        );

        for (int i = 0; i < m.Length; i++)
        {
            object[] att = m[i].GetCustomAttributes(true);
            for (int a = 0; a < att.Length; a++)
            {
                System.Type t = att[a].GetType();
                if (t == typeof(RunOnEnableAttribute))
                    runOnEnable.Add(m[i]);
                if (t == typeof(RunOnDisableAttribute))
                    runOnDisable.Add(m[i]);
                if (t == typeof(RunBeforeOnInspectorGUIAttribute))
                    runBeforeOnInspectorGUI.Add(m[i]);
                if (t == typeof(RunAfterOnInspectorGUIAttribute))
                    runAfterOnInspectorGUI.Add(m[i]);                
            }
        }

        for (int i = 0; i < runOnEnable.Count; i++)
        {
            runOnEnable[i].Invoke(this, new object[0]);
        }

        for (int i = 0; i < runBeforeOnInspectorGUI.Count; i++)
        {
            int pi = ((RunBeforeOnInspectorGUIAttribute)(runBeforeOnInspectorGUI[i].GetCustomAttributes(typeof(RunBeforeOnInspectorGUIAttribute), true)[0])).priority;
            for (int j = i + 1; j < runBeforeOnInspectorGUI.Count; j++)
            {
                int pj = ((RunBeforeOnInspectorGUIAttribute)(runBeforeOnInspectorGUI[j].GetCustomAttributes(typeof(RunBeforeOnInspectorGUIAttribute), true)[0])).priority;
                if (pj < pi)
                {
                    System.Reflection.MethodInfo tmp = runBeforeOnInspectorGUI[i];
                    runBeforeOnInspectorGUI[i] = runBeforeOnInspectorGUI[j];
                    runBeforeOnInspectorGUI[j] = tmp;
                    pi = pj;
                }
            }           
        }
    }

    void OnDisable()
    {        
        for (int i = 0; i < runOnDisable.Count; i++)
        {
            runOnDisable[i].Invoke(this, new object[0]);
        }
    }

    protected abstract class SP
    {
        abstract public void DrawEditorGUI(SerializedObject serializedObject, int indentBonus);
        abstract public string toStr(SerializedObject serializedObject, string indent);

        public string path;
        public List<SP> children;
        public SP parent;
        public int depth;

        public string tt;
        public string ht;
        public string gr;
        public string gt;
    }

    protected class SP_SerializedProperty : SP
    {
        public SP_SerializedProperty(SerializedProperty p)
        {
            // this.p = p.Copy();
            this.path = p.propertyPath;
            this.depth = p.depth;
            this.children = new List<SP>();
        }

        public override void DrawEditorGUI(SerializedObject serializedObject, int indentBonus)
        {
            SerializedProperty p = serializedObject.FindProperty(path);
            bool drawChildren = true;
			bool didStartTooltip = false;
            if (p != null)
            {
				// if(!string.IsNullOrEmpty(gr))
				// 	indentBonus++;
                EditorGUI.indentLevel = p.depth + indentBonus;				
				// if(p.name == "stuff" || p.name == "aaa" || p.name == "bar")
				// 	Debug.Log(p.name + " => " + EditorGUI.indentLevel);
                if (tt != null)
                {
					didStartTooltip = true;
					EditorGUILayout.BeginVertical(GUI.skin.box);
					EditorGUILayout.HelpBox(tt, MessageType.Info);
					
                    // // EditorGUILayout.BeginVertical(GUI.skin.box);
                    // // if ((PimpMyEditorInspector2.instance.showTooltipAbove && p.isArray) || !p.hasChildren)
                    // {
                    //     EditorGUILayout.Separator();
                    //     EditorGUILayout.HelpBox(tt, MessageType.Info);
                    // }
                    drawChildren = EditorGUILayout.PropertyField(
                        p,
                        new GUIContent(ObjectNames.NicifyVariableName(p.name) + (ht != null ? " *":""), ht != null ? ht : "")
                    );

                    // if (drawChildren && !PimpMyEditorInspector2.instance.showTooltipAbove)
                    // {
                    //     EditorGUILayout.HelpBox(tt, MessageType.Info);
                    // }
                    // EditorGUILayout.EndVertical();
                }
                else if (ht != null)
                {
                    drawChildren = EditorGUILayout.PropertyField(
                        p,
                        new GUIContent(ObjectNames.NicifyVariableName(p.name) + " *", ht)
                    );
                }
                else
                    drawChildren = EditorGUILayout.PropertyField(p);                
            }

            if (!drawChildren)
			{
				if(didStartTooltip)
					EditorGUILayout.EndVertical();
				return;
			}

            string cgr = null;
            bool indented = false;
            for (int i = 0; i < children.Count; i++)
            {
                
                if (children[i].gr != cgr)
                {
                    if (indented)
                    {
                        indentBonus--;
                        indented = false;
                        //EditorGUILayout.EndVertical();                        
                    }
                    cgr = children[i].gr;
                    if (children[i].gr != null)
                    {                        
                        EditorGUI.indentLevel = (p!=null?p.depth+1:0) + indentBonus;						
                        bool isFoldout = EditorPrefs.GetBool("PimpMyEditor-Foldout-" + path + "-" + i, true);
                        bool isStillFoldout = EditorGUILayout.Foldout(isFoldout, children[i].gr);
                        if (isStillFoldout != isFoldout)
                            EditorPrefs.SetBool("PimpMyEditor-Foldout-" + path + "-" + i, isStillFoldout);

                        if (isFoldout && children[i].gt != null)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.HelpBox(children[i].gt, MessageType.Info);
                            EditorGUI.indentLevel--;
                        }

                        
                        if (!isFoldout)
                        {
                            int j = i;
                            while (j < children.Count && children[j].gr == cgr) j++;
                            if (j != i)
                                i = j - 1;
                            cgr = null;                            
                            continue;
                        }
                        indentBonus++;
                        indented = true;
                    }
                }
                
                children[i].DrawEditorGUI(serializedObject, indentBonus);
            }
			
			if(didStartTooltip)
				EditorGUILayout.EndVertical();
           // if (indented)
            //    EditorGUILayout.EndVertical();
        }

        public override string toStr(SerializedObject serializedObject, string indent)
        {
            SerializedProperty p = serializedObject.FindProperty(path);

            string s = indent + (p == null ? "null\n" : p.name + " (" + p.hasChildren + ", " + p.hasVisibleChildren + ") " + "(" + (this.gr != null ? this.gr : "no group") + ")\n");
            for (int i = 0; i < children.Count; i++)
            {
                s += children[i].toStr(serializedObject, indent + "\t");
            }
            return s;
        }
    }
    private SP first;

    public override void OnInspectorGUI()
    {        
        serializedObject.Update();

        SerializedProperty p = serializedObject.GetIterator();
        first = new SP_SerializedProperty(p);
        SP current = first;
        SP last = first;

        bool showChildren = true;
        while (p.NextVisible(showChildren))
        {
            SP next = new SP_SerializedProperty(p);
            if (next.depth > last.depth)
            {
                next.parent = last;
                last.children.Add(next);
                current = last;
            }
            else
            {
                while (next.depth <= current.depth)
                    current = current.parent;
                next.parent = current;
                current.children.Add(next);
            }
            last = next;
        }

        // Run all required before inspector gui
        for (int i = 0; i < runBeforeOnInspectorGUI.Count; i++)
        {
            runBeforeOnInspectorGUI[i].Invoke(this, new object[0]);
        }

        // Debug.Log(first.toStr(serializedObject, ""));

        // EditorGUIUtility.LookLikeInspector();
        first.DrawEditorGUI(serializedObject, 1);
        serializedObject.ApplyModifiedProperties();

        if (runAfterOnInspectorGUI.Count > 0)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical(GUI.skin.box);            
            EditorGUI.indentLevel = 0;
            bool showOptions = EditorPrefs.GetBool("PimpMyEditor-ShowOptions-" + target.GetType(), false);
            bool so = EditorGUILayout.Foldout(showOptions, "Pimp Options");
            if (so != showOptions)
                EditorPrefs.SetBool("PimpMyEditor-ShowOptions-" + target.GetType(), so);
            if (so)
            {
                EditorGUI.indentLevel = 1;
                // Run all required after inspector gui
                for (int i = 0; i < runAfterOnInspectorGUI.Count; i++)
                {
                    runAfterOnInspectorGUI[i].Invoke(this, new object[0]);
                }
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUIUtility.LookLikeControls();
    }

    protected System.Reflection.FieldInfo GetSerializedPropertyFieldInfo(SerializedProperty p)
    {
        string[] path = p.propertyPath.Split('.');
        System.Reflection.FieldInfo info = null;
        System.Reflection.FieldInfo _info = null;
        System.Type type = target.GetType();
        for (int i = 0; i < path.Length; i++)
        {
            System.Type searchType = type;
            do
            {
                info = searchType.GetField(
                    path[i],
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic
                );
                searchType = searchType.BaseType;
            } while (searchType != null && info == null);
            
            _info = info == null ? _info : info;
			
			if (info == null && path[i] == "Array")
            {
                i++;
                if (path.Length > i + 1 && path[i].IndexOf("data[") == 0)
                {
                    // get the type of the array
                    string typeName = type.ToString();
                    typeName = typeName.Replace("[]", "");

                    foreach (System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (assembly.FullName.IndexOf("Assembly-CSharp,") == 0)
                        {
                            foreach (System.Type atype in assembly.GetTypes())
                            {
                                if (atype.FullName == typeName)
                                {
                                    // This is my type
                                    type = atype;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    continue;
                }
                else if (path[i].IndexOf("data[") == 0)
                    return _info;

                return null;
            }

            if (info == null)
            {
                return null;
            }

            type = info.FieldType;
        }

        return info;
    }

    protected object GetSerializedPropertyFieldValue(SerializedProperty p)
    {
        string[] path = p.propertyPath.Split('.');
        System.Reflection.FieldInfo info = null;
        System.Type type = target.GetType();
        object currentValue = p.serializedObject.targetObject;

        for (int i = 0; i < path.Length; i++)
        {

            info = type.GetField(path[i]);

            if (info == null && path[i] == "Array")
            {
                i++;
                if (path.Length > i + 1 && path[i].IndexOf("data[") == 0)
                {
                    int index = int.Parse(path[i].Replace("data[", "").Replace("]", ""));
                    object[] objArr = ((object[])currentValue);
                    if (objArr == null || objArr.Length <= index)
                        return null;

                    currentValue = objArr[index];

                    // get the type of the array
                    string typeName = type.ToString();
                    typeName = typeName.Replace("[]", "");

                    foreach (System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (assembly.FullName.IndexOf("Assembly-CSharp,") == 0)
                        {
                            foreach (System.Type atype in assembly.GetTypes())
                            {
                                if (atype.FullName == typeName)
                                {
                                    // This is my type
                                    type = atype;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    continue;
                }
                else if (path[i].IndexOf("data[") == 0 && currentValue != null)
                {
                    int index = int.Parse(path[i].Replace("data[", "").Replace("]", ""));
	                try 
					{
						object[] objArr = ((object[])currentValue);
	                    if (objArr == null || objArr.Length <= index)
	                        return null;
	
	                    return objArr[index];
					}
					catch(System.Exception e0)
					{
						try 
						{
							System.Array array = (System.Array) currentValue;
							if(array == null || array.Length <= index)
								return null;
							
							return array.GetValue(index);
						}
						catch(System.Exception e1)
						{
                            if (currentValue.GetType().IsGenericType)
                            {
                                try
                                {                                    
                                    IList list = (IList)currentValue;
                                    return list[index];
                                }
                                catch (System.Exception e2)
                                {
                                    Debug.LogWarning(
                                        "Pimp My Editor: Unable to get value of element in array. Email info@biometricgames.com for support if the problem does not go away magically all by itself. Did you try rebooting? :-p" +
                                        "\n\n" + e0.ToString() + "\n=====\n" + e1.ToString()
                                        + "\n======\n" + e2.ToString()
                                    );
                                }
                            }
                            else
                            {
                                Debug.LogWarning(
                                        "Pimp My Editor: Unable to get value of element in array. Email info@biometricgames.com for support if the problem does not go away magically all by itself. Did you try rebooting? :-p" +
                                        "\n\n" + e0.ToString() + "\n=====\n" + e1.ToString()
                                    );
                            }
						}
					}
                }
                return null;
            }

            if (info == null)
            {
                return null;
            }

            currentValue = info.GetValue(currentValue);
            type = info.FieldType;
        }

        return currentValue;

    }

    protected bool showTooltipAbove;
}
