using UnityEngine;
using TNet;

public class Interceptor : Terminal {

	#region Piloting
	//Our vector to rotate towards, which happens to also be our free look vector
	private Quaternion lookVector = Quaternion.identity;

	//Hyper Thrust
	[SerializeField]
	private int hyperBurstCooldownTickTotal = 30;
	private int hyperBurstCooldownTickCurrent = 0;
	private bool activateHyperBurst = false;
	private bool inHyperBurstCooldown {
		get{ 
			return hyperBurstCooldownTickCurrent > 0 ? true : false ;
		}
	}

	void FixedUpdate(){

		//Applying Hyper Burst effect
		if (activateHyperBurst) {

			tno.Send( "ApplyHyperBurst", Target.All );

			return;

		}

		//TODO Apply Attitude Adjustments

		//If in Hyper Burst cooldown
		if ( inHyperBurstCooldown ) {

			hyperBurstCooldownTickCurrent--;
			return;

		}

		//TODO Apply Station Control

	}

	public void UpdateLookVector( Quaternion newQuat ){

		if (!TNManager.isHosting)
						tno.SendQuickly ("UpdateLookVector", Target.Host, newQuat);

		lookVector *= newQuat;

	}

	#region Hyper Burst
	public void RequestInitiateHyperBurst(){

		tno.Send ("InitiateHyperBurst", Target.Host);

	}

	[RFC]
	private void InitiateHyperBurst(){

		//Check if we're in cooldown
		if ( inHyperBurstCooldown ) return;
		
		activateHyperBurst = true;

	}

	[RFC]
	private void ApplyHyperBurst(){

		//TODO Hyper Burst effects

		hyperBurstCooldownTickCurrent = hyperBurstCooldownTickTotal;
		activateHyperBurst = false;

	}
	#endregion
	#endregion

	#region Sync
	protected override void SendData ()
	{
		Debug.Log ("Sync Interceptor");
		//TODO Sync information

		tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation );

	}

	[RFC(1)]
	private void RecieveSync( Vector3 position, Quaternion facing ){

		//TODO Do stuff with recieved sync data

	}
	#endregion
}