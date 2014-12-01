using UnityEngine;
using System.Collections;

public class MechControl : TerminalControl {

	void Update(){
		
		if ( Screen.lockCursor ) {
			
			//Get input to update lookVector
			if( PlayerSettings.GetMechLookMode() == MechLookMode.Free ){
				
				//Camera Changes
				lookRotation = Quaternion.AngleAxis( Input.GetAxis( "Mouse X" ), transform.up ) * lookRotation;
				lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Mouse Y" ), Vector3.right );
				lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Roll" ), Vector3.forward );
				
				shipCore.UpdateLookVector ( lookRotation );

				
			} else {
				//TODO Flight input for Locked type
				
				/* Probably something to do with Screen Spaces
				 */
			}
			
			if ( Input.GetButtonDown( "Boost" ) ) {
				
				shipCore.UpdateBurst(true);
				
			}

			ResolveDockingRequest();
			
			//If FixedUpdate ran this frame
			if (updateCamera) {
				
				//Update the camera. Since Update runs after internal physics updates, this means all movement would have been done by this time
				playerCamera.transform.rotation = lookRotation;
				playerCamera.transform.position = lookRotation * _cameraPoint.localPosition + transform.position;
				
				updateCamera = false;
				
			}
			
		}
		
		//Debug.DrawRay( savedPosition, (savedRotation * Vector3.forward) * 10 );
		
	}

}
