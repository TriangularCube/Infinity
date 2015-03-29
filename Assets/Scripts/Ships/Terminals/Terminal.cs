using UnityEngine;
using System.Collections;
using TNet;

using Netplayer = TNet.Player;

public abstract class Terminal : Ship {

	public Netplayer pilot{ get; private set; }
	[SerializeField]
	protected TerminalControl control;

    [SerializeField]
    protected TerminalSync status;

	protected override void Awake () {
		base.Awake ();

		//SetupDockingAndLaunching();

        button = HUD.instance.RequestNewShipButton( this );

		/*
		//Disable ourselves if we're parented to something
		if( transform.parent ){
			gameObject.SetActive( false );
            //Should probably do this in Carrier, and null the current owner
		}
		*/
	}

    void FixedUpdate() {

        AttitudeControl();

        StationControl();

        FireControl();

    }

    #region Station, Attitude, and Fire Controls
    protected abstract void AttitudeControl();

    protected abstract void StationControl();

    protected void FireControl() {

        if( status.fireWeapon1 ) weapon1.Fire();

        if( status.fireWeapon2 ) weapon2.Fire();

        if( status.fireWeapon3 ) weapon3.Fire();

    }
    #endregion

    #region Weapons
    protected TerminalWeapon weapon1, weapon2, weapon3;

    protected abstract void AssignWeapons( string weaponSelection );
    #endregion Weapons

    #region Launching and Docking
    #region Launching
    public void OnLaunch( Netplayer toBeSeated, string weaponSelection ){
		//Unparent ourself
		transform.parent = null;
		
		//Reset our Scale (because for some reason the scale gets messed up when we parent)
        transform.localScale = Vector3.one;

		//Assign the player to the pilot position
        pilot = toBeSeated;
        if( pilot == TNManager.player ) {
            //Activate the controls
            control.Assign();

            HUD.instance.mouseLocked = true;
        }
        //TODO, DEBUG
        AssignWeapons( weaponSelection );


        //Reset the target vectors and rotations
        status.targetLookDirection = transform.rotation;
        status.inputDirection = Vector3.zero;

        //Set ourself to active
        gameObject.SetActive( true );

        //TODO Add some forwards momentum
        //rigidBody.AddRelativeForce( Vector3.forward * 20, ForceMode.Impulse );

        if( pilot == TNManager.player ) {
            HUD.instance.PlayerShipLaunched( this );
        } else {
            HUD.instance.AllyShipLaunched( this );
        }
	}
    #endregion Launching

    #region Docking
    //A boolean to transfer across network
    private bool inDockingRange = false;//DEBUG

    [RFC]
    public void IsInRangeToDock( bool inRange ) {
        if( inRange != inDockingRange ) return;

        if( pilot != TNManager.player ) {
            tno.Send( "IsInRangeToDock", pilot, inRange );
            return;
        }

        inDockingRange = inRange;

        HUD.instance.DockingRangeChange( inRange );
    }

	[RFC]
	protected void RequestDock(){
		
		//If we're somehow no longer in docking range in the time it took us to request docking, do nothing
		if (!inDockingRange) return;
		
		Flagship.instance.RequestDock( tno.uid );
		
	}

	public void DockPrep(){

		//Unseat the pilot
		pilot = null;

		//Zero the velocity
		rigidbody.velocity = Vector3.zero;

		//Disable the Terminal
		gameObject.SetActive(false);

        //Enable the dock button
        button.SetButtonActive( true );

	}
    #endregion Docking
    #endregion Launching and Docking

    #region HUD Hooks
    public TerminalWeapon[] weaponSelection {
        get {
            return new TerminalWeapon[] { weapon1, weapon2, weapon3 };
        }
    }

    //DEBUG
    public int selectedWeapon { get { return control.selectedWeapon; } }

    private ShipSelectButton button;
    #endregion HUD Hooks


    
}
