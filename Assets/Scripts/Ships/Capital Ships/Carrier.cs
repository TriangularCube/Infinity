using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

using Netplayer = TNet.Player;

public abstract class Carrier : Ship {

	protected override void Awake(){
		base.Awake ();

		//Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
		foreach ( Transform child in transform.FindChild( "Dock" ) ) {
			dockedTerminals.Add( child.gameObject.GetComponent<Terminal>() );
		}
		//TODO Sort the Terminals we just added

		EventManager.instance.AddListener( "RequestLaunch", new DelegateEventHandler ( RequestLaunch ) );
	}

	public bool ContainsPlayer( Netplayer check ){
		//throw new System.NotImplementedException ();
		//DEBUG, TODO
		return false;
	}

#pragma warning disable 0649
    [SerializeField]
	private Transform dock;
	[SerializeField]
	private Transform launchPoint;
#pragma warning restore 0649

    //Our list of currently docked Terminals
	private TNet.List<Terminal> dockedTerminals = new TNet.List<Terminal>();
    //List of requests for terminals
    private Dictionary<Terminal, Netplayer> terminalRequests = new Dictionary<Terminal, Netplayer>();

    private void AddTerminal( Terminal toAdd ) {
        dockedTerminals.Add( toAdd );
        terminalRequests.Add( toAdd, null );
    }

    private void RemoveTerminal( Terminal toRemove ) {
        dockedTerminals.Remove( toRemove );
        terminalRequests.Remove( toRemove );
    }


	#region Docking Terminal
	public void RequestDock( Terminal terminal ){
		
		Debug.Log( "Requested to Dock" );
		tno.Send( "DockTerminal", Target.All, terminal.tno.uid );
		
	}

	[RFC]
	protected void DockTerminal( uint terminalID ){

		//Find our GameObject
		Terminal terminal = TNObject.Find( terminalID ).gameObject.GetComponent<Terminal>();
		Debug.Log( "Found Object" );

		//Pull out our pilot
		Netplayer pilot = terminal.pilot;

		//Initiate Docking Procedures
		terminal.DockPrep ();

		terminal.transform.parent = dock;
		terminal.transform.position = launchPoint.position;
		terminal.transform.rotation = launchPoint.rotation;

        AddTerminal( terminal );

		//Fire off an event telling relevant parties something's docked
		EventManager.instance.QueueEvent( new AllyDocked( pilot, this ) );

		//Assign the pilot to Default
		AssignDefault( pilot );

	}
	#endregion


	#region Assignment
#pragma warning disable 0649
    [SerializeField]
	private ObservationStation observationStation;
	[SerializeField]
	private CapitalShipStation navigation;
#pragma warning restore 0649


    //Our list of players currently in our Observation Deck
	private TNet.List<Netplayer> playersInObservation = new TNet.List<Netplayer>();
	
	//Our currnet Navigator
	private Netplayer navigator;


	public void AssignDefault ( Netplayer pilot )
	{
		AssignObservation (pilot);
		
		//Debug
		//AssignNavigation (pilot);
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

	//Removes a chosen pilot from any roles on the ship
	protected virtual void RemovePilot( Netplayer player ){
		
		if( playersInObservation.Contains( player ) ) {
			
			playersInObservation.Remove( player );
			return;
			
		}
		
		if( navigator == player ) {
			
			navigator = null;
			return;
			
		}
		
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

        if( terminalRequests[ terminal ] != player ) {

            //The player who is currently requesting to launch the terminal is not the person who reserved it
            return;

        }
		
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
        RemoveTerminal( terminal );
		
		terminal.OnLaunch( player, "" );//HACK, TODO

        EventManager.instance.QueueEvent( new AllyLaunched( terminal, this ) );
		
	}
	#endregion


	protected virtual void ResetControls(){
		observationStation.CleanUp();
		//navigation.CleanUp();
	}
}
