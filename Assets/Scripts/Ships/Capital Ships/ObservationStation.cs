using UnityEngine;
using System.Collections;

public class ObservationStation : CapitalShipStation {

	Quaternion storedRotation = Quaternion.identity;

	void Update(){

		if ( Cursor.lockState == CursorLockMode.Locked ) {
			//This is observation, so we're just going to let the player move the camera around
			
			//Camera Changes
			storedRotation = Quaternion.AngleAxis( Input.GetAxis( "Mouse X" ), Vector3.up) * storedRotation;
			storedRotation = storedRotation * Quaternion.Euler( Input.GetAxis( "Mouse Y" ), 0f, 0f);

		}

		//TODO Optimize this
		lookRotation = _cameraPoint.rotation * storedRotation;

		UpdateCamera();

	}

}

