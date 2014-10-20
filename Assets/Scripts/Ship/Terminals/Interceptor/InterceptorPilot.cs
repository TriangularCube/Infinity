using UnityEngine;
using System.Collections;

public class InterceptorPilot : ShipControl {

	//Our Looking point
	private Quaternion lookVector = Quaternion.identity;


	void Update(){
		if ( playerCamera != null && Screen.lockCursor == true ) {
			lookVector *= Quaternion.Euler( new Vector3( -Input.GetAxis( "Mouse Y" ), Input.GetAxis( "Mouse X" ), Input.GetAxis( "Roll" ) ) );
		}

		if ( !hyperThrustButton && !hyperThurst ) {
			hyperThrustButton = Input.GetButtonDown( "Boost" );
		}
	}
	
	void FixedUpdate(){

		if (hyperThurst) {
			if (++hyperThrustTickCount >= ticksInHyperThrust) {
				hyperThrustTickCount = 0;
				hyperThurst = false;
			}
		} else {

			ApplyVector ();

		}

		ApplyAttitude ();

	}
	#endregion

	#region Station and Attitude Control
	[SerializeField]
	private int ticksInHyperThrust = 30;
	private int hyperThrustTickCount = 0;
	private bool hyperThrustButton = false;

	bool hyperThurst{ 
		get{
			return stats.hyperThurst;
		}
		
		set{
			stats.hyperThurst = value;
		}
	}

	private void ApplyAttitude(){

		//Find the look vector
		Quaternion oldLookVector = lookVector;

		//TODO Do some fancy attitude controls later.
		transform.rotation = Quaternion.RotateTowards( transform.rotation, lookVector, 5f );
		playerCamera.transform.rotation = oldLookVector;
		
		//The offset in the direction of travel. This is not working out currently. Should not need when I implement Movement indicators.
		//		Vector3 offset = transform.InverseTransformDirection( rigidbody.velocity * 0.05f );
		
		playerCamera.transform.position = ( oldLookVector * ( cameraPoint.localPosition /* - offset */ ) ) + transform.position;
	}

	private void ApplyVector(){
		//TODO Maybe deal with this if I ever want to implement Warp
		//If drifting faster than max velocity, set back to max velocity
		if( rigidbody.velocity.sqrMagnitude > stats.maxSpeedSqr ){
			rigidbody.velocity = Vector3.ClampMagnitude( rigidbody.velocity, stats.maxSpeed );
		}
		
		Debug.DrawRay( rigidbody.position, rigidbody.velocity * 10 );
		
		//TODO Change this away from the input manager
		if ( Input.GetButton( "Break" ) ) {
			//MAYBE!! TODO Think of some way to still accelerate towards the keyed vectors while breaking the others
			
			//TODO Fine, use Hyper Thrust meter to emergency break
			rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, Vector3.zero, stats.breakForce * Time.deltaTime );
			
		} else {
			//OTHERWISE accelerate towards a vector
			

			Vector3 inputVector = new Vector3( Input.GetAxis( "Thrust X" ), Input.GetAxis( "Thrust Y" ), Input.GetAxis( "Thrust Z" ) );


			//Boosting!
			if( inputVector != Vector3.zero && hyperThrustButton ){
				rigidbody.velocity = lookVector * ( inputVector.normalized * stats.maxSpeed );
				transform.rotation = lookVector;
				playerCamera.transform.position = cameraPoint.position;

				hyperThurst = true;
				hyperThrustButton = false;

				return;
			}
			
			Vector3 targetVector = Vector3.zero;
			
			//Process the left and right thrust
			targetVector.x = inputVector.x * stats.otherForce;
			
			//Process the up and down thrust
			targetVector.y = inputVector.y * stats.otherForce;
			
			if( inputVector.z > 0f ){
				
				//Process the forward thrust
				targetVector.z = inputVector.z * stats.forwordForce;
				
			} else {
				
				//Process the backward thrust
				targetVector.z = inputVector.z * stats.backwardForce;
				
			}
			
			rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, transform.TransformDirection( inputVector.normalized * stats.maxSpeed ), targetVector.magnitude * Time.deltaTime );
		}
	}
	#endregion

	public override void CleanUp ()
	{
		base.CleanUp ();
		lookVector = Quaternion.identity;
	}
}
