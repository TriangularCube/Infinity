using UnityEngine;
using System.Collections;

public class InterceptorPilot : TerminalPilot {

	//Our Looking point
	private Quaternion lookVector = Quaternion.identity;

	#region Hyper Thrust Variables
	[SerializeField]
	private int ticksInHyperThrust = 30;
	private int hyperThrustTickCount = 0;
	private bool hyperThrustButton = false;
	
	private bool inHyperThrust{ 
		get{
			return stats.hyperThurst;
		}
		
		set{
			stats.hyperThurst = value;
		}
	}
	#endregion

	#region Main Loop Functions
	protected override void Update(){
		base.Update();

		if ( playerCamera != null && Screen.lockCursor == true ) {

			//Get input to update lookVector
			lookVector *= Quaternion.Euler( new Vector3( -Input.GetAxis( "Mouse Y" ), Input.GetAxis( "Mouse X" ), Input.GetAxis( "Roll" ) ) );
		
		}

		if ( !hyperThrustButton && !inHyperThrust ) {

			//Get input for Hyper Thrusting
			hyperThrustButton = Input.GetButtonDown( "Boost" );
		
		}
	}
	
	void FixedUpdate(){

		ApplyAttitudeControl ();

		if (inHyperThrust) {

				//Tick up Hyper Thrust, and if finished, reset Hyper Thurst state
			if (++hyperThrustTickCount >= ticksInHyperThrust) {
				hyperThrustTickCount = 0;
				inHyperThrust = false;
			}

		} else {

			//If drifting faster than max velocity, set back to max velocity
			if( rigidbody.velocity.sqrMagnitude > stats.maxSpeedSqr ){
				rigidbody.velocity = Vector3.ClampMagnitude( rigidbody.velocity, stats.maxSpeed );
			}

			if (Input.GetButton ("Break")) {

				ApplyBreaks ();

			} else {

				//Grab the input axes
				Vector3 inputVector = new Vector3 (Input.GetAxis ("Thrust X"), Input.GetAxis ("Thrust Y"), Input.GetAxis ("Thrust Z"));

				if (hyperThrustButton && inputVector != Vector3.zero) {

					ApplyThrustControl (inputVector);

				} else {

					hyperThrustButton = false;

					ApplyStationControl (inputVector);

				}

			}
		}

	}
	#endregion

	#region Ship Controls
	private void ApplyAttitudeControl(){

		//Find the look vector
		Quaternion oldLookVector = lookVector;

		//TODO Do some fancy attitude controls later.
		rigidbody.MoveRotation( Quaternion.RotateTowards( transform.rotation, lookVector, 5f ) );

		//Reset the look vector
		playerCamera.transform.rotation = oldLookVector;
		
		//The offset in the direction of travel. This is not working out currently. Should not need when I implement Movement indicators.
		//Vector3 offset = transform.InverseTransformDirection( rigidbody.velocity * 0.05f );
		
		playerCamera.transform.position = ( oldLookVector * ( cameraPoint.localPosition /* - offset */ ) ) + transform.position;
	}

	private void ApplyBreaks(){
		//MAYBE!! TODO Think of some way to still accelerate towards the keyed vectors while breaking the others
		
		//TODO Fine, use Hyper Thrust meter to emergency break
		rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, Vector3.zero, stats.breakForce * Time.deltaTime );
	}

	private void ApplyStationControl( Vector3 inputVector ){

		Debug.DrawRay( rigidbody.position, rigidbody.velocity * 10 );

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

		//Move the velocity towards the input vector, to a max magnitude of the vector forces
		rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, transform.TransformDirection( inputVector.normalized * stats.maxSpeed ), targetVector.magnitude * Time.deltaTime );

	}

	private void ApplyThrustControl( Vector3 inputVector ){

		//Set velocity to max speed in the boosted direction, corrected by look direction
		rigidbody.velocity = lookVector * ( inputVector.normalized * stats.maxSpeed );

		//Instantly turn to the look vector
		transform.rotation = lookVector;

		//Reset camera
		playerCamera.transform.position = cameraPoint.position;

		//Process HyperThrust inputs
		inHyperThrust = true;
		hyperThrustButton = false;

	}
	#endregion

	public override void OnLaunch (Quaternion facing)
	{
		lookVector = facing;
	}


	public override void CleanUp ()
	{
		base.CleanUp ();
		lookVector = Quaternion.identity;
	}
}
