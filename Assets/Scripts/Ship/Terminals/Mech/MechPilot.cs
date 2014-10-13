using UnityEngine;
using System.Collections;

public class MechPilot : ShipControl {

	//Our Looking point
	private Quaternion lookVector = Quaternion.identity;
	
	void Update(){
		if ( playerCamera != null && Screen.lockCursor == true ) {
			lookVector *= Quaternion.Euler( new Vector3( -Input.GetAxis( "Pitch" ), Input.GetAxis( "Yaw" ), Input.GetAxis( "Roll" ) ) );
		}
	}
	
	void FixedUpdate(){
		
		//Find the look vector
		Quaternion oldLookVector = lookVector;
		Vector3 accelVector = Vector3.zero;
		
		#region Applying Velocity
		
		//TODO Maybe deal with this if I ever want to implement Warp
		//If drifting faster than max velocity, set back to max velocity
		if( rigidbody.velocity.sqrMagnitude > stats.maxSpeedSqr ){
			rigidbody.velocity *= 0.99f;
		}
		
		Debug.DrawRay( rigidbody.position, rigidbody.velocity * 10 );
		
		//TODO Change this away from the input manager
		if ( Input.GetButton( "Break" ) ) {
			//MAYBE!! TODO Think of some way to still accelerate towards the keyed vectors while breaking the others
			
			//TODO Fine, use Hyper Thrust meter to emergency break
			//IF player wants to use the break, scale back the velocity
//			rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, Vector3.zero, stats.breakSmooth * Time.deltaTime );
			
		} else {
			//OTHERWISE accelerate towards a vector
			
			
			//TODO Implement Hyper Thursting if double tap
			//If the speed isn't already at max
			if( rigidbody.velocity.sqrMagnitude < stats.maxSpeedSqr ){
				accelVector.x = Input.GetAxis( "Thrust X" );
				accelVector.y = Input.GetAxis( "Thrust Y" );
				accelVector.z = Input.GetAxis( "Thrust Z" );
//				accelVector = accelVector.normalized * Time.deltaTime * stats.acceleration;
				
				rigidbody.AddRelativeForce( accelVector );
			}
		}
		
		#endregion
		
		#region Applying Attitude Control
		
		//TODO Do some fancy attitude controls later.
		transform.rotation = Quaternion.RotateTowards( transform.rotation, lookVector, 5f );
		playerCamera.transform.rotation = oldLookVector;
		
		//The offset in the direction of travel. This is not working out currently. Should not need when I implement Movement indicators.
		//		Vector3 offset = transform.InverseTransformDirection( rigidbody.velocity * 0.05f );
		
		playerCamera.transform.position = ( oldLookVector * ( cameraPoint.localPosition /* - offset */ ) ) + transform.position;
		
		#endregion
	}
}
