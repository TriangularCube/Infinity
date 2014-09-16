using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

using NetPlayer = TNet.Player;

public abstract class Carrier : Ship {
	
	public ObservationStation observationStation;
	public Transform dock;
	public TNet.List<GameObject> dockedTerminals{ get; set; }

	protected Dictionary<NetPlayer, string> playerRoles = new Dictionary<NetPlayer, string>();

	//Check if this player in on this ship
	public override bool ContainsPlayer (NetPlayer check)
	{
		throw new System.NotImplementedException ();
	}

	void Awake(){
		dockedTerminals = new TNet.List<GameObject> ();

		//Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
		foreach (Transform child in transform.FindChild( "Dock" )) {
			dockedTerminals.Add( child.gameObject );
		}
	}

	public virtual void Dock (GameObject terminal){
		Debug.Log ("Requested Docking");
		
		if (terminal.GetComponent<Terminal> () == null) {
			throw new UnityException ("Dock is called on a non-terminal object");
		}
		
		NetPlayer pilot = terminal.GetComponent<Terminal> ().pilot;
		
		//Dock the incoming terminal
		DockTerminal( terminal.GetComponent<TNObject> ().uid );
		
		//The default role is Observation, so we automatically assign the pilot to observation
		AssignDefault (pilot);
	}

	[RFC]
	void DockTerminal( uint terminalID ){
		
		if (TNManager.isHosting) {
			tno.Send( "DockTerminal", Target.Others, terminalID );
		}
		
		//Find our GameObject
		GameObject terminal = TNObject.Find (terminalID).gameObject;
		
		//If this is us
		if (terminal.GetComponent<Terminal> ().pilot == TNManager.player) {
			ShipControl control = terminal.GetComponent<ShipControl>();
			
			//Do cleanup operations
			control.CleanUp ();

			LaunchMenuManager.instance.Docked( this );
		}
		
		//Call cleanup on the terminal
		terminal.GetComponent<Terminal> ().CleanUp ();
		
		//Physically dock the ship
		terminal.transform.parent = dock;
		terminal.transform.position = dock.position;
		dockedTerminals.Add (terminal);
	}

	[RFC]
	protected void AssignObservation( NetPlayer player ){
		
		if (TNManager.isHosting) {
			tno.Send ("AssignObservation", Target.Others, player);
			
			//Request Focus change from PlayerManager
			PlayersManager.instance.UpdateFocusChange ( player, tno.uid, "Observation" );
		}
		
		//Do this stuff only if it pertains to us
		if (player == TNManager.player) {
			//If the player is already on the ship, meaning he is just changing roles
			if (playerRoles.ContainsKey (player)) {
				//Reset all controls
				ResetControls();
			}
			
			PlayersManager.instance.playerCamControl.SetTarget( transform, observationStation );
		}
		
		//Add the player to the list
		playerRoles[player] = "Observation";
		
		Debug.Log ("Assigned Observation");
	}

	public override void AssignDefault ( NetPlayer pilot )
	{
		AssignObservation (pilot);
	}

	protected virtual void ResetControls(){
		observationStation.enabled = false;
	}
}
