using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MechStationAndAttitudeControl : MonoBehaviour {

	//The square the of max magnitude the velocity should be
	private float maxSpeed;
	private float maxSpeedSqr;
	//The max accelration
	private float accelerationRate;
	private float breakSmooth;

	//should be in local coordinates
	private Vector3 accelVector;

	/*
	protected override void DoStart () {
		//If this is the server
		if ( Network.isServer || Network.peerType == NetworkPeerType.Disconnected ) {
			accelVector = Vector3.zero;

			Stats stats = shipRigidbody.gameObject.GetComponent<Stats>();
			
			maxSpeed = stats.maxSpeed;
			maxSpeedSqr = maxSpeed * maxSpeed;
			accelerationRate = stats.acceleration;
			//DEBUG
			breakSmooth = stats.breakSmooth;
		}

	}

	void FixedUpdate () {

		//TODO Check if we're the server (which means we get to move stuff), then check which player is controlling it and apply the correct controls
		//whether that is coming from the network or on the local machine. Then either send it off if we're a client, or process it if we're the server

		#region Applying Velocity

		Debug.DrawRay( shipRigidbody.position, shipRigidbody.velocity * 10 );


		//TODO Think of some way to still accelerate towards the keyed vectors while breaking the others
		//IF player wants to use the break, scale back the velocity
		if ( Input.GetKey ( keys ["break"] ) ) {
			
			//If the velocity is not already zero
			if( velocityVector != Vector3.zero ){
				velocityVector = Vector3.MoveTowards( velocityVector, Vector3.zero, accelerationRate * Time.deltaTime );
			}
			
		} else {
			//OTHERWISE accelerate towards a vector
			//Now using Unity's input manager

			//Final acceleration vector
			accelVector = new Vector3 (Input.GetAxis( "Thrust X"), Input.GetAxis( "Thrust Y"), Input.GetAxis( "Thrust Z" ) ).normalized  * accelerationRate * Time.deltaTime;

			//TODO Do some math magic here and fix the velocity vector to align to a proper axis when it is being clamped
			//Finding the movement offset, then clamping the speed
			velocityVector = Vector3.ClampMagnitude (velocityVector + transform.TransformDirection (accelVector), maxSpeed * Time.deltaTime);

		}

		//Moving the rigid-body
		shipRigidbody.MovePosition (shipRigidbody.position + velocityVector);


		//TODO Maybe deal with this if I ever want to implement Warp
		//If drifting faster than max velocity, set back to max velocity
		if( shipRigidbody.velocity.sqrMagnitude > maxSpeedSqr ){
			shipRigidbody.velocity *= 0.99f;
		}

		Debug.Log( shipRigidbody.velocity.magnitude );

		//TODO Change this away from the input manager
		if ( Input.GetAxis( "Break" ) > 0 ) {
			//MAYBE!! TODO Think of some way to still accelerate towards the keyed vectors while breaking the others

			//TODO Fine, use Hyper Thrust meter to emergency break
			//IF player wants to use the break, scale back the velocity
			shipRigidbody.velocity = Vector3.MoveTowards( shipRigidbody.velocity, Vector3.zero, breakSmooth * Time.deltaTime );

		} else {
			//OTHERWISE accelerate towards a vector


			//TODO Implement Hyper Thursting if double tap
			//If the speed isn't already at max
			if( shipRigidbody.velocity.sqrMagnitude < maxSpeedSqr ){
				accelVector.x = Input.GetAxis( "Thrust X" );
				accelVector.y = Input.GetAxis( "Thrust Y" );
				accelVector.z = Input.GetAxis( "Thrust Z" );
				accelVector = accelVector.normalized * Time.deltaTime * accelerationRate;

				shipRigidbody.AddRelativeForce( accelVector );
			}
		}

		#endregion

		#region Applying Attitude Control

		//TODO This is pretty basic. Do some fancy attitude controls later.
		if( Screen.lockCursor ){
			shipRigidbody.rotation = shipRigidbody.rotation * Quaternion.Euler( Input.GetAxis( "Pitch" ), Input.GetAxis( "Yaw" ), Input.GetAxis( "Roll" ) );
		}

		#endregion
	}
		*/
}
