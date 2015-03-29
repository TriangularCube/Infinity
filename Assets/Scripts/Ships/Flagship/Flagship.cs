using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

using Netplayer = TNet.Player;

public class Flagship : Ship {

    public static Flagship instance { get { return _instance;  } }
    private static Flagship _instance;

	protected override void Awake(){
        _instance = this;
		base.Awake ();

		//Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
		foreach ( Transform child in dock ) {
			dockedTerminals.Add( child.gameObject.GetComponent<Terminal>() );
            child.gameObject.SetActive( false );
		}
		//TODO Sort the Terminals we just added
	}

    [SerializeField]
    private Transform directionIndicator;

    #region Ship Operations
    private void FixedUpdate() {
        //Station Control

        //Attitude Control
        AttitudeContrl();
    }

    private void Update() {
        //Fire Control...?

    }

    #region Station Control
    private Quaternion targetLookDirection = Quaternion.identity;
    private Vector3 targetAccelDirection = Vector3.zero;

    private void StationControl() {
        //NOTE: Using Forward Acceleration speed here as a placeholder. The Acceleration direction mechanism needs a serious overhaul.
        rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, targetAccelDirection * maxSpeed, forwardAcceleration * Time.deltaTime );
    }

    private void AttitudeContrl() {
        //TODO Do some fancy attitude controls later.
        rigidbody.MoveRotation( Quaternion.RotateTowards( transform.rotation, targetLookDirection, 2f ) );
    }

    #endregion Station Control

    #endregion Ship Operations

    #region Terminals
#pragma warning disable 0649
    [SerializeField]
	private Transform dock;
	[SerializeField]
	private Transform launchPoint;
#pragma warning restore 0649

    //Our list of currently docked Terminals
	private TNet.List<Terminal> dockedTerminals = new TNet.List<Terminal>();
    public TNet.List<Terminal> getDockedTerminals() { return dockedTerminals; }


	#region Docking Terminal
	public void RequestDock( uint termID ){
		
		Debug.Log( "Requested to Dock" );
		tno.Send( "DockTerminal", Target.All, termID );
		
	}

	[RFC]
	protected void DockTerminal( uint terminalID ){

		//Find our GameObject
		Terminal terminal = TNObject.Find( terminalID ).gameObject.GetComponent<Terminal>();

		//Pull out our pilot
		Netplayer pilot = terminal.pilot;

		//Initiate Docking Procedures
		terminal.DockPrep ();

		terminal.transform.parent = dock;
		terminal.transform.position = launchPoint.position;
		terminal.transform.rotation = launchPoint.rotation;

        dockedTerminals.Add( terminal );

		//Tell all relevant parties something's docked
        if( pilot == TNManager.player ) {
            HUD.instance.PlayerShipDocked();
        } else {
            HUD.instance.AllyShipDocked( terminal );
        }

		//Assign the pilot to Default
		AssignDefault( pilot );

        if( pilot == TNManager.player ) Debug.Log( "Docking Complete" );
	}
	#endregion

	#region Launching Terminal 
    public void RequestLaunchTerminal( uint termID ) {
        tno.Send( "AttemptToLaunchTerminal", Target.Host, termID, TNManager.player );
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
        dockedTerminals.Remove( terminal );
		
		terminal.OnLaunch( player, "" );//HACK, TODO
		
	}
	#endregion
    #endregion terminals

    #region Assignment
#pragma warning disable 0649
    [SerializeField]
	private Observation observation;
	[SerializeField]
	private Navigation navigation;
#pragma warning restore 0649


    //Our list of players currently in our Observation Deck
	private TNet.List<Netplayer> playersInObservation = new TNet.List<Netplayer>();//Is this list even useful?
	
	//Our currnet Navigator
	private Netplayer navigator;


	public void AssignDefault ( Netplayer pilot )
	{
		//AssignObservation( pilot );
		
		//Debug
		AssignNavigation( pilot );
	}

	protected void AssignObservation( Netplayer player ){
		
		//Add the player to the list
		playersInObservation.Add( player );

		if( player == TNManager.player ) {

			ResetControls();
			observation.Assign();
            HUD.instance.RoleAssigned( /*Observation*/ ); //DEBUG, TODO

		}
		
		//EventManager.instance.QueueEvent (new AssignedShipRole (this, player, "Observation"));
	}

	private void AssignNavigation( Netplayer player ){
				
		//Do this stuff only if it pertains to us
		if (player == TNManager.player) {

			//If the player is already on the ship, meaning he is just changing roles
			ResetControls();
			navigation.Assign();
            HUD.instance.RoleAssigned( /*Navigation*/ );

		}
		
		navigator = player;
		
		//EventManager.instance.QueueEvent (new AssignedShipRole (this, player, "Navigation"));
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
		
        //Plus any other roles as applicable
	}

    protected virtual void ResetControls(){
		observation.CleanUp();
		//navigation.CleanUp();
	}
    #endregion Assignment

    #region Sync
    protected override void SendData() {
        //TODO
        tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation );
    }

    [RFC(2)]
    public void UpdateNavigationControl( Vector3 moveVector, Quaternion lookVector ) {
        targetLookDirection = lookVector;
        targetAccelDirection = moveVector;
    }
    #endregion Sync
}
