using UnityEngine;
using System.Collections;

public class InterceptorControl : TerminalControl {

	void Update(){

		if (Screen.lockCursor) {

			#region Rotation
			//Get input to update lookVector
			if (PlayerSettings.GetInterceptorLookMode () == InterceptorLookMode.Free) {

				//Camera Changes
				lookRotation = Quaternion.AngleAxis( Input.GetAxis( "Mouse X" ), transform.up ) * lookRotation;
				lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Mouse Y" ), Vector3.right );
				lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Roll" ), Vector3.forward );

				shipCore.UpdateLookVector ( lookRotation );

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

			//If mouse is not locked pass through zeroed values
			shipCore.UpdateBoost( false );
			shipCore.UpdateInputVectorAndBreak( Vector3.zero, false );

		}

		CheckForDocking();

		UpdateCamera();
	}



}
