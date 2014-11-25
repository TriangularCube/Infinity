using UnityEngine;

public abstract class ShipControl : MonoBehaviour {

	[SerializeField]
	protected Transform _cameraPoint;
	public Transform cameraPoint{
		get{
			return _cameraPoint;
		}
	}

	protected bool updateCamera = false;
	protected Quaternion lookRotation;

	void OnEnable(){
		lookRotation = _cameraPoint.rotation;
	}

	void FixedUpdate(){
		
		//Update the camera this frame
		updateCamera = true;
		
	}

	protected void UpdateCamera(){
		//If FixedUpdate ran this frame
		if (updateCamera) {
			
			//Update the camera. Since Update runs after internal physics updates, this means all movement would have been done by this time
			playerCamera.transform.rotation = lookRotation;
			playerCamera.transform.position = lookRotation * _cameraPoint.localPosition + transform.position;
			
			updateCamera = false;
			
		}
	}

	protected Camera playerCamera = null;

	public virtual void Assign(){

		playerCamera = Camera.main;//HACK

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
