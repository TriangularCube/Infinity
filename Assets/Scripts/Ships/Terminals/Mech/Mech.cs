using UnityEngine;
using System.Collections;
using TNet;

public class Mech : Terminal {

	#region Piloting
	//Our vector to rotate towards, which happens to also be our free look vector
	private Quaternion targetTravelDirection = Quaternion.identity;
	
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
		
		ApplyAttitudeControl ();
		
		//If in Hyper Burst cooldown
		if ( inHyperBurstCooldown ) {
			
			hyperBurstCooldownTickCurrent--;
			return;
			
		}
		
		//TODO Apply Station Control
	}

	#region Attitude Control
	public override void UpdateLookVector( Quaternion newQuat ){
		
		if (!TNManager.isHosting)
			tno.SendQuickly ("UpdateLookVector", Target.Host, newQuat);
		
		targetTravelDirection = newQuat;
		
	}

	private void ApplyAttitudeControl(){
		
		//TODO Do some fancy attitude controls later.
		rigidbody.MoveRotation( Quaternion.RotateTowards( transform.rotation, targetTravelDirection, 5f ) );
		
	}
	#endregion

	#region Station Control
	public override void UpdateInputAndBreak( Vector3 input, bool breakButton ){
		throw new System.NotImplementedException ();//TODO
	}
	#endregion

	#region Hyper Burst
	public override void UpdateBurst (bool burst)
	{
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
		//TODO Implement out of order handling
		//TODO Sync information
		
		tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation );
		
	}
	
	[RFC(1)]
	private void RecieveSync( Vector3 position, Quaternion facing ){
		
		//TODO Do stuff with recieved sync data
		
	}
	#endregion

}
