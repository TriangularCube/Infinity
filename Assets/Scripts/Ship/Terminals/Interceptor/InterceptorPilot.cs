using UnityEngine;
using System.Collections;

public class InterceptorPilot : ShipControl {

	//should be in local coordinates
	private Vector3 accelVector;

	//Our Looking point
	private Vector3 lookVector;

	void Update(){
		if ( playerCamera != null && Screen.lockCursor == true ) {
			//This is observation, so we're just going to let the player move the camera around
			
			playerCamera.transform.RotateAround( transform.position, transform.TransformDirection( Vector3.up ), Input.GetAxis( "Yaw" ) );
			playerCamera.transform.RotateAround( transform.position, playerCamera.transform.TransformDirection( Vector3.left ), Input.GetAxis( "Pitch" ) );
		}
	}
	
	void FixedUpdate(){

		//Find the look vector
		lookVector = transform.TransformDirection (playerCamera.transform.forward);

		
		#region Applying Velocity
		
		//TODO Maybe deal with this if I ever want to implement Warp
		//If drifting faster than max velocity, set back to max velocity
		if( rigidbody.velocity.sqrMagnitude > stats.maxSpeedSqr ){
			rigidbody.velocity *= 0.99f;
		}
		
		Debug.DrawRay( rigidbody.position, rigidbody.velocity * 10 );
		
		//TODO Change this away from the input manager
		if ( Input.GetAxis( "Break" ) > 0 ) {
			//MAYBE!! TODO Think of some way to still accelerate towards the keyed vectors while breaking the others
			
			//TODO Fine, use Hyper Thrust meter to emergency break
			//IF player wants to use the break, scale back the velocity
			rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, Vector3.zero, stats.breakSmooth * Time.deltaTime );
			
		} else {
			//OTHERWISE accelerate towards a vector
			
			
			//TODO Implement Hyper Thursting if double tap
			//If the speed isn't already at max
			if( rigidbody.velocity.sqrMagnitude < stats.maxSpeedSqr ){
				accelVector.x = Input.GetAxis( "Thrust X" );
				accelVector.y = Input.GetAxis( "Thrust Y" );
				accelVector.z = Input.GetAxis( "Thrust Z" );
				accelVector = accelVector.normalized * Time.deltaTime * stats.acceleration;
				
				rigidbody.AddRelativeForce( accelVector );
			}
		}
		
		#endregion
		
		#region Applying Attitude Control
		
		//TODO Do some fancy attitude controls later.
		

		#endregion
	}
}
