using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UITexture) )]
public class Indicator : MonoBehaviour {

	[SerializeField]
	private UITexture texture;

	[SerializeField]
	private Texture box;
	[SerializeField]
	private Texture arrow;

	#region Transform Cache
	private Transform thisTransform;
	public new Transform transform{
		get{
			return thisTransform;
		}
	}

	void Awake(){
		thisTransform = base.transform;
	}
	#endregion

	public void SetArrow(){
		texture.mainTexture = arrow;
	}

	public void SetBox(){
		texture.mainTexture = box;
	}
}
