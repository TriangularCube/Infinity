using UnityEngine;
using System.Collections;

public class FlagshipNavigation : ShipControl {

	private Quaternion freeLookVector = Quaternion.identity;
	private Quaternion headingVector = Quaternion.identity;

	private Quaternion fixedFreeLookVector = Quaternion.identity;

	private bool isFreeLook = true;

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

	void FreeLook(){

		fixedFreeLookVector *= Quaternion.Euler (0f, Input.GetAxis ("Mouse X"), 0f);
		freeLookVector = Quaternion.AngleAxis (-Input.GetAxis ("Mouse Y"), fixedFreeLookVector * Vector3.right ) * Quaternion.AngleAxis (Input.GetAxis ("Mouse X"), Vector3.up ) * freeLookVector;

	}

	void HeadingChange(){

		headingVector *= Quaternion.Euler (-Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), Input.GetAxis ("Roll"));

	}

	void FixedUpdate(){

		StationControl ();

		AttitudeControl ();
		
	}

	void StationControl(){



	}

	void AttitudeControl(){

		rigidbody.MoveRotation (Quaternion.RotateTowards (rigidbody.rotation, headingVector, 1f));

		if (!isFreeLook) {

			playerCamera.transform.rotation = headingVector;
			playerCamera.transform.position = headingVector * ( cameraPoint.localPosition + transform.position );

		} else {

			playerCamera.transform.localRotation = freeLookVector;
			playerCamera.transform.localPosition =  freeLookVector * cameraPoint.localPosition;

		}
	}
}
