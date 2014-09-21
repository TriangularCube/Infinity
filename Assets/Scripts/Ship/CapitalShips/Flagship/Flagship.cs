using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TNet;
using NetPlayer = TNet.Player;

[RequireComponent( typeof( ObservationStation ) )]
[RequireComponent( typeof( FlagshipNavigation ) )]

public class Flagship : Carrier {

	public FlagshipNavigation navigation;

	protected override void ResetControls(){
		base.ResetControls ();
		navigation.enabled = false;
	}

	//DEBUG
	public override void AssignDefault (NetPlayer pilot)
	{
		AssignNavigation(pilot);
	}

	[RFC]
	private void AssignNavigation( NetPlayer player ){
		if (TNManager.isHosting) {
			tno.Send ("AssignNavigation", Target.Others, player);
			
			//Request Focus change from PlayerManager
			PlayersManager.instance.UpdateFocusChange ( player, tno.uid, "Navigation" );
		}
		
		//Do this stuff only if it pertains to us
		if (player == TNManager.player) {
			//If the player is already on the ship, meaning he is just changing roles
			if (playerRoles.ContainsKey (player)) {
				//Reset all controls
				ResetControls();
			}
			
			PlayersManager.instance.playerCamControl.SetTarget( transform, navigation );
		}
		
		//Add the player to the list
		playerRoles[player] = "Navigation";
		
		Debug.Log ("Assigned Navigation");
	}

	public void OnDestroy(){
		//TODO Figure out what to do when the thing we're on is destroyed.
	}
}
