﻿using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using TNet;

using Netplayer = TNet.Player;

public abstract class Carrier : Ship {

	#region Dock and Terminals
	[SerializeField]
	private Transform dock;
	public Vector3 getDock{ get{ return dock.position; } }
	[SerializeField]
	private Transform launchPoint;

	//Our list of currently docked Terminals
	private List<Terminal> dockedTerminals = new List<Terminal>();
	#endregion

	#region Ship Roles
	[SerializeField]
	private ObservationStation observationStation;
	[SerializeField]
	private CapitalShipStation navigation;
	//Our list of players currently in our Observation Deck
	private TNet.List<Netplayer> playersInObservation = new List<Netplayer>();

	//Our currnet Navigator
	private Netplayer navigator;
	#endregion

	protected override void Awake(){
		base.Awake ();

		//Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
		foreach ( Transform child in transform.FindChild( "Dock" ) ) {
			dockedTerminals.Add( child.gameObject.GetComponent<Terminal>() );
		}
		//TODO Sort the Terminals we just added

		EventManager.instance.AddListener( "ReqeustDock", new DelegateEventHandler( RequestDock ) );
		EventManager.instance.AddListener( "RequestLaunch", new DelegateEventHandler ( RequestLaunch ) );
	}

	#region Docking Terminal
	private bool RequestDock ( IEvent evt ){

		RequestDock req = (RequestDock) evt;

		if (req.carrier != this) {
			return false;
		}

		tno.Send ("DockTerminal", Target.All, req.terminal.tno.uid);

		return true;
	}

	[RFC]
	void DockTerminal( uint terminalID ){

		//Find our GameObject
		Terminal terminal = TNObject.Find (terminalID).gameObject.GetComponent<Terminal>();

		//Pull out our pilot
		Netplayer pilot = terminal.pilot;

		//Initiate Docking Procedures
		terminal.DockPrep ();

		terminal.transform.parent = dock;
		terminal.transform.position = launchPoint.position;
		terminal.transform.rotation = launchPoint.rotation;
		dockedTerminals.Add (terminal);

		//Fire off an event telling relevant parties something's docked
		EventManager.instance.QueueEvent (new AllyDocked (terminal, this));

		//Assign the pilot to Default
		AssignDefault (pilot);

	}
	#endregion

	#region Launching Terminal
	public bool RequestLaunch( IEvent evt ){

		RequestLaunch req = (RequestLaunch)evt;

		if ( req.carrier != this ) return false;

		tno.Send ("AttemptToLaunchTerminal", Target.Host, req.terminal.tno.uid, TNManager.player);

		return true;

	}

	[RFC]
	protected virtual void AttemptToLaunchTerminal( uint terminalID, Netplayer player ){

		//Find the Terminal
		Terminal terminal = TNObject.Find (terminalID).gameObject.GetComponent<Terminal>();

		//If the terminal is no longer docked (as in, someone else requested it first), just do nothing
		if ( terminal && !dockedTerminals.Contains( terminal ) ){

			//TODO Send some sort of message back to the player requesting informing him the terminal is no longer here
			return;

		}

		tno.Send ("LaunchTerminal", Target.All, terminalID, player);
	}

	[RFC]
	protected virtual void LaunchTerminal( uint terminalID, Netplayer player ){

		//Find the Terminal...again
		Terminal terminal = TNObject.Find (terminalID).gameObject.GetComponent<Terminal>();
		
		//If this is us
		if (player == TNManager.player) {
			//Reset all controls
			ResetControls();
		}
		
		RemovePilot (player);
		
		//Remove the Terminal
		dockedTerminals.Remove (terminal);
		
		terminal.OnLaunch ( player );

	}

	//Removes a chosen pilot from any roles on the ship
	protected virtual void RemovePilot( Netplayer player ){

		if (playersInObservation.Contains (player)) {

			playersInObservation.Remove( player );
			return;

		}

		if (navigator == player) {

			navigator = null;
			return;

		}

	}
	#endregion

	#region Assignment
	public override void AssignDefault ( Netplayer pilot )
	{
		//AssignObservation (pilot);
		
		//Debug
		AssignNavigation (pilot);
	}

	protected void AssignObservation( Netplayer player ){
		
		//Add the player to the list
		playersInObservation.Add (player);

		if (player == TNManager.player) {

			ResetControls();
			observationStation.Assign();

		}
		
		EventManager.instance.QueueEvent (new AssignedShipRole (this, player, "Observation"));
	}

	private void AssignNavigation( Netplayer player ){
				
		//Do this stuff only if it pertains to us
		if (player == TNManager.player) {

			//If the player is already on the ship, meaning he is just changing roles
			ResetControls();
			navigation.Assign();

		}
		
		navigator = player;
		
		EventManager.instance.QueueEvent (new AssignedShipRole (this, player, "Navigation"));
	}
	#endregion

	protected virtual void ResetControls(){
		observationStation.CleanUp();
		navigation.CleanUp();
	}
}