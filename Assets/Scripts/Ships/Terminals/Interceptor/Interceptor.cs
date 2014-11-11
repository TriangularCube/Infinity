﻿using UnityEngine;
using TNet;

public class Interceptor : Terminal {

	#region Piloting
	//Our vector to rotate towards, which happens to also be our free look vector
	private Quaternion lookVector = Quaternion.identity;

	//Hyper Thrust
	[SerializeField]
	private int hyperBurstCooldownTicks = 30;
	private int hyperBurstCooldownTickCount = 0;
	private bool activateHyperBurst = false;

	void FixedUpdate(){

		//Applying Hyper Burst effect
		if (activateHyperBurst) {

			//TODO do Hyper Burst stuff

			hyperBurstCooldownTickCount = hyperBurstCooldownTicks;
			activateHyperBurst = false;
			return;

		}

		//TODO Apply Attitude Adjustments

		//If in Hyper Burst cooldown
		if (hyperBurstCooldownTickCount > 0) {

			hyperBurstCooldownTickCount--;
			return;

		}

		//TODO Apply Station Control

	}

	[RFC]
	public void UpdateLookVector( Quaternion newQuat ){

		if (!TNManager.isHosting)
						tno.SendQuickly ("UpdateLookVector", Target.Host, newQuat);

		lookVector *= newQuat;

	}

	[RFC]
	public void InitiateHyperBurst(){

		//TODO Send off request for Hyper Burst to server

		//Check if we're in cooldown
		if ( hyperBurstCooldownTickCount > 0 ) return;

		activateHyperBurst = true;

	}
	#endregion

	//Invoking our Syncing script
	protected override void Awake ()
	{
		base.Awake ();
		StartCoroutine (Sync ());
	}

	protected override System.Collections.IEnumerator Sync ()
	{
		throw new System.NotImplementedException ();
	}
}