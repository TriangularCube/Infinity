using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TNet;
using NetPlayer = TNet.Player;

public class Flagship : Carrier {

	public FlagshipObservation flagshipObservation;
	public Transform dock;

	private Dictionary<NetPlayer, string> playerRoles = new Dictionary<NetPlayer, string>();
	public override bool ContainsPlayer (NetPlayer check)
	{
		throw new System.NotImplementedException ();
	}

	//DEBUG
	void Start(){
		AssignObservation (TNManager.player);
	}

	public void Dock( GameObject terminal ){

		if (terminal.GetComponent<Terminal> () == null) {
			throw new UnityException ("Dock is called on a non-terminal object");
			return;
		}

		//The default role is Observation, so we automatically assign the pilot to observation
		tno.Send (1, Target.Host, terminal.GetComponent<Terminal>().pilot );

		//Dock the incoming terminal
		tno.Send (2, Target.Host, terminal.GetComponent<TNObject> ().uid);
	}

	[RFC(2)]
	void DockTerminal( uint terminalID ){
		//TODO Actually dock the terminal
		GameObject terminal = TNObject.Find (terminalID).gameObject;
		ShipControl control = terminal.GetComponent<ShipControl>();

		//HACK This may have problems
		control.CleanUp ();

		//TODO This needs more work
		terminal.SetActive (false);
		terminal.transform.parent = dock;
		terminal.transform.position = dock.position;
	}

	[RFC(1)]
	void AssignObservation( NetPlayer player ){

		if (TNManager.isHosting) {
			tno.Send (1, Target.Others, player);

			//TODO Request Focus change from PlayerManager
			PlayersManager.Instance.ApplyFocusChange ( TNManager.player, tno.uid, "Observation" );
		}

		//Do this stuff only if it pertains to us
		if (player == TNManager.player) {
			//If the player is already on the ship, meaning he is just changing roles
			if (playerRoles.ContainsKey (player)) {
				//Reset all controls
				ResetControls();
			}
			
			//TODO Turn on the Observation Controls
			flagshipObservation.enabled = true;

			PlayersManager.Instance.playerCam.GetComponent<CameraControls>().SetTarget( transform, flagshipObservation );
		}

		//Add the player to the list
		playerRoles[player] = "Observation";

		
		Debug.Log ("Assigned Observation");
	}

	void ResetControls(){
		//TODO There's more to do here
		flagshipObservation.enabled = false;
	}

	//TODO Launch a terminal

	public void OnDestroy(){
		//TODO Figure out what to do when the thing we're on is destroyed.
	}
}
