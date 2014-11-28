using UnityEngine;
using TNet;

public class Interceptor : Terminal {

	#region Piloting

	void FixedUpdate(){

		ApplyAttitudeControl ();

		ApplyStationControl ();

	}

	#region Station Control
	[SerializeField]
	private float boostAcceleration;

	//A normalized input vector
	private Vector3 inputDirection = Vector3.zero;
	private bool breakButton = false;

	[RFC]
	public override void UpdateInputAndBreak( Vector3 newInput, bool newBreak ){

		if (!TNManager.isHosting) tno.SendQuickly( "UpdateInputAndBreak", Target.Host, newInput, newBreak );

		inputDirection = newInput;
		breakButton = newBreak;

	}

	private void ApplyStationControl(){

		//Calculate max magnitude change
		float mag = 0f;

		if (!isBoostActive) {
			Vector3 forceVector = Vector3.zero;

			//Process the left and right thrust
			forceVector.x = inputDirection.x * sideAcceleration;

			//Process the up and down thrust
			forceVector.y = inputDirection.y * verticalAcceration;

			if (inputDirection.z > 0f) {

				//Process the forward thrust
				forceVector.z = inputDirection.z * forwardAcceleration;

			} else {

				//Process the backward thrust
				forceVector.z = inputDirection.z * backwardAcceleration;

			}

			mag = forceVector.magnitude;

		} else {
			mag = boostAcceleration;
		}

		float currentMaxSpeed = isBoostActive ? maxBurstSpeed : maxSpeed;

		rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, targetLookDirection * inputDirection.normalized * currentMaxSpeed, mag * Time.deltaTime );

	}
	#endregion Station Control

	#region Attitude Control
	//Our vector to rotate towards, which happens to also be our free look vector
	private Quaternion targetLookDirection = Quaternion.identity;

	private void ApplyAttitudeControl(){

		//TODO Do some fancy attitude controls later.
		rigidbody.MoveRotation( Quaternion.RotateTowards( transform.rotation, targetLookDirection, 5f ) );

	}

	[RFC]
	public override void UpdateLookVector( Quaternion newQuat ){

		//TODO This is sending every frame, which is dumb. Find some way to filter this
		if( !TNManager.isHosting && newQuat != rigidbody.rotation )
						tno.SendQuickly ("UpdateLookVector", Target.Host, newQuat);

		targetLookDirection = newQuat;

	}
	#endregion Attitude Control

	#region Boost
	[SerializeField]
	private int boostCharge = 360; //1 Boost unit is used per physics tick
	private bool isBoostActive = false;

	public override void UpdateBurst( bool burstStatus ){

		if( isBoostActive != burstStatus ) {

			if( !TNManager.isHosting ) tno.Send( "UpdateBoostOnServer", Target.Host, burstStatus );
		
			isBoostActive = burstStatus;
		}

	}

	[RFC]
	void UpdateBoostOnServer( bool boostStatus ){

		isBoostActive = boostStatus;
		
	}
	#endregion Boost

	#region Fire Control
	public override void UpdateFireControl (bool nextWeapon, bool prevWeapon, bool fireWeapon){
		throw new System.NotImplementedException ();
	}
	#endregion
	#endregion Piloting

	#region Sync
	protected override void SendData ()
	{
		//TODO Implement out of order handling
		//TODO Sync information

		tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation );
		//TODO Sync Boost and Boost Charge
	}

	[RFC(1)]
	private void RecieveSync( Vector3 position, Quaternion facing ){

		//TODO Do stuff with recieved sync data

	}
	#endregion
}