using UnityEngine;
using System.Collections;

public class FlagshipNavigation : ShipControl {

	private bool isFreeLook = true;
	private Quaternion freeLookVector = Quaternion.identity;
	private Quaternion fixedFreeLookVector = Quaternion.identity;

	private Quaternion headingVector = Quaternion.identity;
	
	void Update(){

		if (!Screen.lockCursor) return;

		if( Input.GetButtonDown( "Look" ) ) isFreeLook = false;
		if (Input.GetButtonUp ("Look"))	isFreeLook = true;


		if ( isFreeLook ) {

			FreeLook();

		} else {

			HeadingChange();

		}

	}

	void FixedUpdate(){
		
		StationControl ();
		
		AttitudeControl ();
		
	}

	void FreeLook(){

		/*
		fixedFreeLookVector *= Quaternion.Euler (0f, Input.GetAxis ("Mouse X"), 0f);
		freeLookVector = Quaternion.AngleAxis (-Input.GetAxis ("Mouse Y"), fixedFreeLookVector * Vector3.right ) * Quaternion.AngleAxis (Input.GetAxis ("Mouse X"), Vector3.up ) * freeLookVector;
		*/

		_cameraPoint.RotateAround (transform.position, transform.up, Input.GetAxis ("Mouse X"));
		_cameraPoint.RotateAround (transform.position, _cameraPoint.right, -Input.GetAxis ("Mouse Y"));
		
		//freeLookVector = freeLookVector * Quaternion.AngleAxis ( Input.GetAxis ("Mouse X"), transform.up );
	}

	void HeadingChange(){

		headingVector *= Quaternion.Euler (-Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), Input.GetAxis ("Roll"));
		freeLookVector *= Quaternion.Euler (0f, 0f, Input.GetAxis ("Roll"));

	}

	void StationControl(){

		Vector3 inputControl = new Vector3 (Input.GetAxis ("Thrust X"), Input.GetAxis ("Thrust Y"), Input.GetAxis ("Thrust Z"));



	}

	void AttitudeControl(){

		rigidbody.MoveRotation (Quaternion.RotateTowards (rigidbody.rotation, headingVector, 1f));

		if (!isFreeLook) {

			playerCamera.transform.rotation = headingVector;
			playerCamera.transform.position = headingVector * ( localPosition + transform.position );

		} else {

			/*
			playerCamera.transform.rotation = freeLookVector;
			playerCamera.transform.position = freeLookVector * ( cameraPoint.localPosition + transform.position );
			*/

			playerCamera.transform.position = _cameraPoint.position;
			playerCamera.transform.rotation = _cameraPoint.rotation;

		}
	}
}
