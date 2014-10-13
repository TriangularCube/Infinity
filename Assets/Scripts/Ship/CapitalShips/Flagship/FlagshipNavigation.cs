using UnityEngine;
using System.Collections;

public class FlagshipNavigation : ShipControl {

	//should be in local coordinates
	private Vector3 accelVector;

	void FixedUpdate(){

		#region Applying Velocity

		//TODO Maybe deal with this if I ever want to implement Warp
		//If drifting faster than max velocity, set back to max velocity


		//TODO Change this away from the input manager
		if ( Input.GetAxis( "Break" ) > 0 ) {
			//MAYBE!! TODO Think of some way to still accelerate towards the keyed vectors while breaking the others
			
			//TODO Fine, use Hyper Thrust meter to emergency break
			//IF player wants to use the break, scale back the velocity
			rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, Vector3.zero, stats.breakForce * Time.deltaTime );
			
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
		
		//TODO This is pretty basic. Do some fancy attitude controls later.
		if( Screen.lockCursor ){
			rigidbody.rotation = rigidbody.rotation * Quaternion.Euler( -Input.GetAxis( "Pitch" ), Input.GetAxis( "Yaw" ), Input.GetAxis( "Roll" ) );
		}
		
		#endregion
	}
}
