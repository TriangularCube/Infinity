using UnityEngine;
using System.Collections;
using TNet;

public class Interceptor : Terminal {

	#region Piloting

	#region Station Control
#pragma warning disable 0649
    [SerializeField, Group("Boost")]
	private float boostAcceleration;
#pragma warning restore 0649

	protected override void ApplyStationControl(){

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
	protected override void ApplyAttitudeControl(){

		//TODO Do some fancy attitude controls later.
		rigidbody.MoveRotation( Quaternion.RotateTowards( transform.rotation, targetLookDirection, 5f ) );

	}
	#endregion Attitude Control

	#region Boost
	[SerializeField, Group("Boost")]
	private int boostCharge = 360; //1 Boost unit is used per physics tick
	#endregion Boost

	#region Fire Control
#pragma warning disable 0649
    [SerializeField, Group("Weapons")]
	private TerminalWeapon autoCannon, plasmaGun, bombLauncher;
#pragma warning restore 0649

    protected override void AssignWeapons( string weaponSelection ){

		//The following are DEBUG functionality TODO, HACK

		weapon1 = autoCannon;
		weapon1.gameObject.SetActive( true );

		weapon2 = plasmaGun;
		weapon2.gameObject.SetActive( true );

		weapon3 = bombLauncher;
		weapon3.gameObject.SetActive( true );

	}

	#endregion Fire Control
	#endregion Piloting

	#region Sync
	protected override void SendData () {
		//TODO Sync information

		tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation, currentWeapon, fireWeapon );
		//TODO Sync Boost and Boost Charge
	}

	[RFC(1)]
	private void RecieveSync( Vector3 position, Quaternion facing, int selectedWeapon, bool fire ){

        //TODO Implement out of order handling
		// TODO Do stuff with recieved sync data

        // TODO Facing and Flight Input

        if( selectedWeapon != currentWeapon ) {

            currentWeapon = selectedWeapon;

            if( TNManager.player == pilot ) EventManager.instance.QueueEvent( new WeaponSwitch( currentWeapon ) );

        }

        // TODO Fire Weapon
        
	}
	#endregion
}