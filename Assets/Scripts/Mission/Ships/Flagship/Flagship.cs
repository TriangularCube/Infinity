﻿using UnityEngine;
using TNet;

using Netplayer = TNet.Player;

public class Flagship : Ship {

    public static Flagship instance { get { return _instance;  } }
    private static Flagship _instance;

	protected override void Awake(){
        _instance = this;
		base.Awake ();
	}

    void Start() {
        //Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
        foreach( Transform child in dock ) {
            dockedTerminals.Add( child.gameObject.GetComponent<Terminal>() );
            //child.gameObject.SetActive( false );
        }
        //TODO Sort the Terminals we just added
    }

#pragma warning disable 0649
    [SerializeField]
    private Transform directionIndicator;

    [SerializeField]
    private FlagshipSync status;
#pragma warning restore 0649

    #region Ship Operations
    private void FixedUpdate() {
        //Station Control
        StationControl();

        //Attitude Control
        AttitudeControl();
    }

    private void Update() {
        //Fire Control...?

        //Change the direction control indicator
        if( directionIndicator && rigidbody.velocity != Vector3.zero ) {
            directionIndicator.rotation = Quaternion.LookRotation( rigidbody.velocity );
        }

    }

    #region Station and Attitude Control
    private void StationControl() {
        //NOTE: Using Forward Acceleration speed here as a placeholder. The Acceleration direction mechanism needs a serious overhaul. TODO
        rigidbody.velocity = Vector3.MoveTowards( rigidbody.velocity, status.targetAccelDirection * maxSpeed, forwardAcceleration * Time.deltaTime );
    }

    private void AttitudeControl() {
        //TODO Do some fancy attitude controls later.
        rigidbody.MoveRotation( Quaternion.RotateTowards( transform.rotation, status.targetLookDirection, 2f ) );
    }

    #endregion Station and attitude Control

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

            //Turn the direction indicator on (This is really a HUD operation, but for sake of simplicity of code it is here)
            directionIndicator.gameObject.SetActive( true );
        } else {
            HUD.instance.AllyShipDocked( terminal );
        }

		//Assign the pilot to Default
		AssignDefault( pilot );
	}
	#endregion

	#region Launching Terminal 
    public void RequestLaunchTerminal( uint termID ) {
        tno.Send( 4, Target.Host, termID, TNManager.player );
    }

	[RFC(4)]
    public virtual void AttemptToLaunchTerminal( uint terminalID, Netplayer player ) {


        //Debug.Log( TNObject.Find( terminalID ) );
		//Find the Terminal
		Terminal terminal = TNObject.Find (terminalID).gameObject.GetComponent<Terminal>();



		//If the terminal is no longer docked (as in, someone else requested it first), just do nothing
		if ( terminal && !dockedTerminals.Contains( terminal ) ){
			
			//TODO Send some sort of message back to the player requesting informing him the terminal is no longer here
			return;
			
		}
		
		tno.Send ( 5, Target.All, terminalID, player );

	}
	
	[RFC(5)]
	protected virtual void LaunchTerminal( uint terminalID, Netplayer player ){

        Debug.Log( "Here" );
		
		//Find the Terminal...again
		Terminal terminal = TNObject.Find (terminalID).gameObject.GetComponent<Terminal>();
		
		//If this is us
		if (player == TNManager.player) {
			//Reset all controls
			ResetControls();

            directionIndicator.gameObject.SetActive( false );
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
		AssignObservation( pilot );
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
        observation.enabled = false;
        navigation.enabled = false;
	}
    #endregion Assignment
}
