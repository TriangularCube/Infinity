using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UITexture) )]
public class AllyIndicator : MonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
	private UITexture texture;

	[SerializeField]
	private Texture box;
	[SerializeField]
	private Texture arrow;
#pragma warning restore 0649

    #region Transform and GameObject Cache
    private GameObject thisGameObject;
	public new GameObject gameObject{
		get{
			return thisGameObject;
		}
	}

	private Transform thisTransform;
	public new Transform transform{
		get{
			return thisTransform;
		}
	}

	void Awake(){
		thisTransform = base.transform;
		thisGameObject = base.gameObject;
	}

	public void Activate(){
		base.gameObject.SetActive( true );
	}
	#endregion

	public void SetArrow(){
		texture.mainTexture = arrow;
	}

	public void SetBox(){
		texture.mainTexture = box;
	}
}
