using UnityEngine;
using System.Collections;

public class FlagshipObservation : ShipControl {

	//This method is accessed when whatever else is controlling the camera relinquishes control, transferring control here
	public override void TransferControl( GameObject cam ){
		playerCamera = cam;

		playerCamera.transform.position = cameraPoint.position;
		playerCamera.transform.rotation = cameraPoint.rotation;

		playerCamera.transform.parent = transform;
	}


	void Update(){
		if ( playerCamera != null && Screen.lockCursor == true) {
			//This is observation, so we're just going to let the player move the camera around
			
			playerCamera.transform.RotateAround( transform.position, transform.TransformDirection( Vector3.up ), Input.GetAxis( "Yaw" ) );
			playerCamera.transform.RotateAround( transform.position, playerCamera.transform.TransformDirection( Vector3.left ), Input.GetAxis( "Pitch" ) );
		}

	}

	void OnDisable(){
		if( playerCamera != null ){
			playerCamera.transform.parent = null;

			playerCamera = null;
		} else Debug.Log( "A little weird that Flagship Observation got disabled without a camera being set first." );
	}
}

