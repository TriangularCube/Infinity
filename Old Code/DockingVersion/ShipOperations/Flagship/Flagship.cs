using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Netplayer = TNet.Player;


public class Flagship : ShipControl {

	public FlagshipObservationControl flagshipObservationControl;
	
	private Dictionary<Netplayer, string> playerRoles = new Dictionary<Netplayer, string>();

	private string[] roles = { "Observation", "Navigation" };

	public override void AssignDefault( Netplayer player )
	{
		AssignObservation( player );
	}

	public override string[] GetAvailableRoles ()
	{
		return roles;
	}

	//HACK Might replace with an enum or something
	public override void AssignRole( Netplayer player, string role ){
		switch (role) {
		case "Observation":
			AssignObservation( player );
			break;
		case "Navigation":
			AssignNavigation( player );
			break;
		default:
			Debug.LogError( "Weird, no applicable role chosen" );
			break;
		}
	}

	void AssignObservation( Netplayer player ){
		//Add the player to the list
		playerRoles.Add (player, "Observation");

		//TODO Turn on the Observation Controls
		if (player == TNManager.player) {
			flagshipObservationControl.enabled = true;
		}

		Debug.Log ("Assigned Observation");
	}

	void AssignNavigation( Netplayer player ){
		//Throw an error if there's already someone on Navigation
		if ( playerRoles.ContainsValue( "Navigation" ) ) {
			Debug.LogError( "Navigation is already full" );
			return;
		}

		//Add the player to the role of Navigation
		playerRoles.Add (player, "Navigation");

		//TODO Turn on Navigation controls
	}

	public override void Reset( Netplayer player ){
		//Remove the player from the list
		playerRoles.Remove (player);

		//TODO Turn off all scripts
		flagshipObservationControl.enabled = false;
	}
}
