using UnityEngine;
using System.Collections;

/*
 * Test showing the Pimp system in action
 * */
 
[ExecuteInEditMode()]
public class PimpedTest : PimpedMonoBehaviour {
	
	[SerializeField]
	[Tooltip("Expose a private secret about the hero")]
	private string privateSecret = "The gold is in the basement under the rug";
	
    [System.Serializable]
    public class Ability
    {
        [Group("Physical properties")]
        public int agility;
        [Group("Physical properties")]
        public int strength;
        [Group("Physical properties")]
        public int speed;

        [Group("Mental properties")]
        public int iq;
        [Group("Mental properties")]
        public int persuasion;
        [Group("Mental properties")]
        public int focus;

        public float mentalAverage { get {
            return (iq + persuasion + focus) / 3.0f;
        }}

        public float physicalAverage { get {
            return (agility + strength + speed) / 3.0f;
        }}

        [GetSetVisible]
        public int level { get {
            return Mathf.FloorToInt((mentalAverage + physicalAverage) / 2.0f);
        } set {
            int toLevel = value < 0 ? 0 : value;                
            int increase = toLevel - level;
                
            iq += increase;
            persuasion += increase;
            focus += increase;
                
            agility += increase;
            strength += increase;
            speed += increase;
        }}
    }

    [Tooltip("The villains abilities. They are vicious creatures capable of no remorse. Also this text is bloated to show wrapping.")]
    public Ability[] villains;

    [Tooltip("This is the hero!")]
    public Ability hero;

    [Tooltip("Another")]
    public Ability subhero;

    [Tooltip("Another")]
    public string something;

    [Tooltip("Another")]
    public int someint;

    [Tooltip("Another")]
    public float somefloat;

    [Tooltip("Another")]
    public Vector3 somevector;

    [System.Serializable]
    public class Nestor
    {
        [System.Serializable]
        public class Nested
        {
            [System.Serializable]
            public class InnerNest
            {
                public string hey;
                public Vector3 you;
                public Quaternion quat;
            }

            public InnerNest nestor;
            public Nested subnest;
        }
        public Nested nested;
        public Nested other;
    }

    [Tooltip("Another")]
    public Nestor nestor1;
    [Tooltip("Another")]
    public Nestor nestor2;

    public Nestor nestor3;

	[System.Serializable]
	public class Link
	{
		public string url;
		public Texture2D texture;
	}
	
	[HideInInspector]
	public Link[] link;

    void Update()
    {
        hero.agility++;
    }
	
	void OnGUI()
	{
        // This is just an ad running if you open the Pimp My Editor Example Scene
        if (link == null || link.Length < 4) return;
		GUILayout.BeginArea(new Rect(Screen.width/2-300,Screen.height/2-300, 600, 600));
		{
			GUILayout.BeginVertical();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GuiLink(link[0]);
					GUILayout.Space(5);
					GuiLink(link[1]);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5);
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GuiLink(link[2]);
					GUILayout.Space(5);
					GuiLink(link[3]);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
	
	void GuiLink(Link link)
	{
		GUILayout.BeginVertical(GUI.skin.button);
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(link.texture);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Label(link.url);
		}
		GUILayout.EndVertical();
		
		if(Event.current.type == EventType.MouseDown)
		{
			Rect r = GUILayoutUtility.GetLastRect();
			if(r.Contains(Event.current.mousePosition))
			{
				Application.OpenURL(link.url);
			}
		}
	}

    void ThisIsHereJustToRemoveAWarning()
    {
        if (privateSecret == null)
            privateSecret = "How did that happen?";

    }
}
