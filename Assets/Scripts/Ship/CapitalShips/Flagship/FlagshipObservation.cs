using UnityEngine;
using System.Collections;

public class FlagshipObservation : ShipControl {

	void Update(){
		if ( playerCamera != null && Screen.lockCursor == true) {
			//This is observation, so we're just going to let the player move the camera around
			
			playerCamera.transform.RotateAround( transform.position, transform.TransformDirection( Vector3.up ), Input.GetAxis( "Yaw" ) );
			playerCamera.transform.RotateAround( transform.position, playerCamera.transform.TransformDirection( Vector3.left ), Input.GetAxis( "Pitch" ) );
		}

	}


}

