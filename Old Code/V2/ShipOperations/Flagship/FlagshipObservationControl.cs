using UnityEngine;
using System.Collections;

public class FlagshipObservationControl : MonoBehaviour {

	private Camera playerCam;
	private Transform cameraPoint;

	//HACK
	void OnEnable(){
		playerCam = Camera.main;
	
		cameraPoint = transform.FindChild( "CameraPoints/FlagshipObservation" );

		playerCam.transform.position = cameraPoint.position;
		playerCam.transform.rotation = cameraPoint.rotation;

		playerCam.transform.parent = transform;
	}

	void Update () {

		if ( playerCam != null && Screen.lockCursor == true) {
			//This is observation, so we're just going to let the player move the camera around
			
			playerCam.transform.RotateAround( transform.position, transform.TransformDirection( Vector3.up ), Input.GetAxis( "Yaw" ) );
			playerCam.transform.RotateAround( transform.position, playerCam.transform.TransformDirection( Vector3.left ), Input.GetAxis( "Pitch" ) );
		}

	}

}
