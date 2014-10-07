using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NetPlayer = TNet.Player;

public abstract class ShipControl : MonoBehaviour {

	[SerializeField]
	protected Transform _cameraPoint;
	public Transform cameraPoint{
		get{
			return _cameraPoint;
		}
	}

	[SerializeField]
	protected Stats stats;

	protected CameraControls playerCamera;

	//This method is accessed when whatever else is controlling the camera relinquishes control, transferring control here
	public virtual void TransferControl( CameraControls cam ){
		playerCamera = cam;

		playerCamera.transform.position = cameraPoint.position;
		playerCamera.transform.rotation = cameraPoint.rotation;

		Screen.lockCursor = true;

		enabled = true;

	}

	//Do this to clean up the object in preparation for deactivation or destruction
	public void CleanUp(){
		if( playerCamera != null ){
			playerCamera.transform.parent = null;
			
			playerCamera = null;
		}

		enabled = false;
	}

}
