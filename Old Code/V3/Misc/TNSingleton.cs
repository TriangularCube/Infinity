using UnityEngine;
using TNet;

/* 
 * This class reimplements TNBehavior under the Singleton pattern
 */

[RequireComponent(typeof(TNObject))]
public abstract class TNSingleton<T> : Singleton<T> where T : MonoBehaviour {

	TNObject mTNO;
	
	public TNObject tno
	{
		get
		{
			if (mTNO == null) mTNO = GetComponent<TNObject>();
			return mTNO;
		}
	}
	
	protected virtual void OnEnable ()
	{
		if (Application.isPlaying)
		{
			tno.rebuildMethodList = true;
		}
	}
	
	/// <summary>
	/// Destroy this game object.
	/// </summary>
	
	public void DestroySelf () { TNManager.Destroy(gameObject); }
}
