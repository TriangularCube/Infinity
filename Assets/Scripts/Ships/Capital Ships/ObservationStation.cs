using UnityEngine;
using System.Collections;

public class ObservationStation : CapitalShipStation {

	void Update(){
		if ( Screen.lockCursor ) {
			//This is observation, so we're just going to let the player move the camera around
			
			_cameraPoint.transform.RotateAround( transform.position, transform.TransformDirection( Vector3.up ), Input.GetAxis( "Yaw" ) );
			_cameraPoint.transform.RotateAround( transform.position, playerCamera.transform.TransformDirection( Vector3.left ), Input.GetAxis( "Pitch" ) );
		}

	}

	void FixedUpdate(){

		playerCamera.transform.position = _cameraPoint.position;
		playerCamera.transform.rotation = _cameraPoint.rotation;

	}

}

