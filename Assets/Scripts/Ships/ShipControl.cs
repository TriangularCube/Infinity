using UnityEngine;

public abstract class ShipControl : MonoBehaviour {

	#region Camera Controls
	[SerializeField]
	protected Transform _cameraPoint;
	public Transform cameraPoint{
		get{
			return _cameraPoint;
		}
	}
	#endregion

	protected Camera playerCamera = null;

	public virtual void Assign(){

		playerCamera = Camera.main;
//		playerCamera.transform.parent = transform;
//		playerCamera.transform.position = _cameraPoint.position;
//		playerCamera.transform.rotation = _cameraPoint.rotation;

		enabled = true;

		//TODO Do additional stuff to pull camera towards camera point

	}

	//Do this to clean up the object in preparation for deactivation or destruction
	public virtual void CleanUp(){

		if (!enabled) return;

		playerCamera = null;

		enabled = false;

	}

}
