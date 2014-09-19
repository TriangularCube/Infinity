using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NetPlayer = TNet.Player;

public abstract class ShipControl : MonoBehaviour {

	public Transform cameraPoint;

	protected GameObject playerCamera;

	//This method is accessed when whatever else is controlling the camera relinquishes control, transferring control here
	public virtual void TransferControl( GameObject cam ){
		playerCamera = cam;
		
		playerCamera.transform.position = cameraPoint.position;
		playerCamera.transform.rotation = cameraPoint.rotation;
		
		playerCamera.transform.parent = transform;

		Screen.lockCursor = true;
		
		enabled = true;
	}

	//Do this to clean up the object in preparation for deactivation or destruction
	public void CleanUp(){
		Debug.Log ("Yo!");

		if( playerCamera != null ){
			playerCamera.transform.parent = null;
			
			playerCamera = null;
		}

		enabled = false;
	}

}
