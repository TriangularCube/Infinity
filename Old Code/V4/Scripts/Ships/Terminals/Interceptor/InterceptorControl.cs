﻿using UnityEngine;
using System.Collections;

public class InterceptorControl : TerminalControl {

	[SerializeField]
	Interceptor shipCore;

	void Update(){

		//Reset the Camera's posiiont and rotaiton if it was changed between last frame and this one (i.e. by ship rotation)
		if (PlayerSettings.GetInterceptorLookMode () == InterceptorLookMode.Free) {
			
			_cameraPoint.rotation = savedRotation;
			_cameraPoint.position = savedPosition;

		}

		if (Screen.lockCursor) {

			#region Rotation
			//Get input to update lookVector
			if (PlayerSettings.GetInterceptorLookMode () == InterceptorLookMode.Free) {

				//Camera Changes
				_cameraPoint.RotateAround (transform.position, _cameraPoint.up, Input.GetAxis ("Mouse X"));
				_cameraPoint.RotateAround (transform.position, _cameraPoint.right, Input.GetAxis ("Mouse Y"));
				_cameraPoint.RotateAround (transform.position, _cameraPoint.forward, Input.GetAxis ("Roll"));

				shipCore.UpdateLookVector (_cameraPoint.rotation);

				//Save our camera point's position and rotaiton
				savedRotation = _cameraPoint.rotation;
				savedPosition = _cameraPoint.position;

			} else {
				//TODO Flight input for Locked type

				//Probably something to do with Screen Spaces
			}

			shipCore.UpdateBoost( Input.GetButton ("Boost") );
			#endregion

			#region Translation

			shipCore.UpdateInputVectorAndBreak( new Vector3( Input.GetAxis( "Thrust X" ), Input.GetAxis( "Thrust Y" ), Input.GetAxis( "Thrust Z") ), Input.GetButton( "Break" ) );

			#endregion

		} else {

			shipCore.UpdateBoost( false );
			shipCore.UpdateInputVectorAndBreak( Vector3.zero, false );

		}


	}

	void FixedUpdate(){

		playerCamera.transform.position = _cameraPoint.position;
		playerCamera.transform.rotation = _cameraPoint.rotation;

	}

}