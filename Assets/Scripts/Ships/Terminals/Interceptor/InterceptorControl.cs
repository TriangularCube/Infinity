using UnityEngine;
using System.Collections;

public class InterceptorControl : TerminalControl {

	void Update(){

		if ( Screen.lockCursor ) {
			
			//Get input to update lookVector
			shipCore.UpdateLookVector( Quaternion.Euler( new Vector3( -Input.GetAxis( "Mouse Y" ), Input.GetAxis( "Mouse X" ), Input.GetAxis( "Roll" ) ) ) );

			if ( Input.GetButtonDown( "Boost" ) ) {
				
				shipCore.InitiateHyperBurst();
				
			}

		}

	}

}
