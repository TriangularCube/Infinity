using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

using NetPlayer = TNet.Player;

public abstract class Carrier : Ship {

	[SerializeField]
	private ObservationStation observationStation;
	[SerializeField]
	private ShipControl navigation;
	[SerializeField]
	private Transform dock;
	[SerializeField]
	private Transform launchPoint;

	public TNet.List<GameObject> dockedTerminals{ get; set; }

	private Dictionary<NetPlayer, string> playerRoles = new Dictionary<NetPlayer, string>();

	//Check if this player in on this ship
	public override bool ContainsPlayer (NetPlayer check)
	{
		if (playerRoles.ContainsKey (check)) {
			return true;
		} else {
			return false;
		}
	}

	protected override void Awake(){
		base.Awake ();
		dockedTerminals = new TNet.List<GameObject> ();

		//Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
		foreach (Transform child in transform.GetChild( 1 )) {
			dockedTerminals.Add( child.gameObject );
		}
	}

	#region Docking
	public virtual void ApplyDock ( NetPlayer pilot, uint terminalUID ){
//		Debug.Log ("Requested Docking");

		//Dock the incoming terminal
		DockTerminal (terminalUID, pilot );
		
		//The default role is Observation, so we automatically assign the pilot to observation
		AssignDefault (pilot);
	}

	[RFC]
	void DockTerminal( uint terminalID, NetPlayer pilot ){
		
		if (TNManager.isHosting) {
			tno.Send( "DockTerminal", Target.Others, terminalID );
		}
		
		//Find our GameObject
		GameObject terminal = TNObject.Find (terminalID).gameObject;
//		Debug.Log ("Found Terminal " + terminalID + " in DockTerminal - " + terminal);
		
		//If this is us
		if (pilot == TNManager.player) {
			ShipControl control = terminal.GetComponent<TerminalPilot>();
			
			//Do cleanup operations
			control.CleanUp ();

			LaunchMenuManager.instance.Docked( this );
		}

		//Call cleanup on the terminal
		terminal.GetComponent<Terminal> ().CleanUp ();
		
		//Physically dock the ship
		terminal.transform.parent = dock;
		terminal.transform.position = launchPoint.position;
		terminal.transform.rotation = launchPoint.rotation;
		dockedTerminals.Add (terminal);

		if (LaunchMenuManager.instance.carrier == this) {
			LaunchMenuManager.instance.PopulateList();
		}
	}
	#endregion

	#region Launch
	public void ApplyForLaunch( NetPlayer pilot, GameObject terminal ){
		tno.Send ("LaunchTerminal", Target.Host, pilot, terminal.GetComponent<TNObject> ().uid);
	}

	[RFC]
	protected virtual void LaunchTerminal( NetPlayer pilot, uint terminalID ){
		if (TNManager.isHosting) {
			tno.Send( "LaunchTerminal", Target.Others, pilot, terminalID );
		}

		//Find the Terminal
		GameObject terminal = TNObject.Find (terminalID).gameObject;

		if (pilot == TNManager.player) {
			//Reset all controls
			ResetControls();
		}

		//Remove the pilot from the carrier
		playerRoles.Remove (pilot);

		//Remove the Terminal
		dockedTerminals.Remove (terminal);

		//Debug
		terminal.transform.position = launchPoint.position;

		terminal.GetComponent<Terminal>().OnLaunch ( launchPoint.rotation, pilot );
		
	}
	#endregion

	#region Assignment
	public override void AssignDefault ( NetPlayer pilot )
	{
//		AssignObservation (pilot);

		//Debug
		AssignNavigation (pilot);
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
			
			CameraControls.instance.SetTarget( transform, observationStation );
		}
		
		//Add the player to the list
		playerRoles[player] = "Observation";
		
		Debug.Log ("Assigned Observation");
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
			
			CameraControls.instance.SetTarget( transform, navigation );
		}
		
		//Add the player to the list
		playerRoles[player] = "Navigation";
		
		Debug.Log ("Assigned Navigation");
	}
	#endregion

	protected virtual void ResetControls(){
		observationStation.CleanUp();
		navigation.CleanUp();
	}
}
