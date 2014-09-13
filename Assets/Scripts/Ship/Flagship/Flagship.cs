using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TNet;
using NetPlayer = TNet.Player;

public class Flagship : Carrier {

	public FlagshipObservation flagshipObservation;
	public Transform dock;

	private CameraControls playerCameraControl;
	private Dictionary<NetPlayer, string> playerRoles = new Dictionary<NetPlayer, string>();
	public override bool ContainsPlayer (NetPlayer check)
	{
		throw new System.NotImplementedException ();
	}

	
	void Start(){
		playerCameraControl = PlayersManager.Instance.playerCam.GetComponent<CameraControls>();
	}

	public void Dock( GameObject terminal ){

		if (terminal.GetComponent<Terminal> () == null) {
			throw new UnityException ("Dock is called on a non-terminal object");
		}

		NetPlayer pilot = terminal.GetComponent<Terminal> ().pilot;

		//Dock the incoming terminal
		DockTerminal( terminal.GetComponent<TNObject> ().uid );

		//The default role is Observation, so we automatically assign the pilot to observation
		AssignObservation( pilot );
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

			playerCameraControl.SetTarget( transform, flagshipObservation );
		}

		//Add the player to the list
		playerRoles[player] = "Observation";
		
		Debug.Log ("Assigned Observation");
	}

	[RFC(2)]
	void DockTerminal( uint terminalID ){
		
		if (TNManager.isHosting) {
			tno.Send( 2, Target.Others, terminalID );
		}
		
		//TODO Actually dock the terminal
		GameObject terminal = TNObject.Find (terminalID).gameObject;
		
		//If this is us
		if (GetComponent<Terminal> ().pilot == TNManager.player) {
			ShipControl control = terminal.GetComponent<ShipControl>();
			
			//Do cleanup operations
			control.CleanUp ();
		}

		//Unseat the pilot
		terminal.GetComponent<Terminal> ().pilot = null;
		
		//TODO This probably needs more work
		terminal.SetActive (false);
		terminal.transform.parent = dock;
		terminal.transform.position = dock.position;
		dockedTerminals.Add (terminal);
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
