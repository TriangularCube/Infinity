using UnityEngine;
using System.Collections;
using TNet;

using Netplayer = TNet.Player;

public abstract class Terminal : Ship {

	public Netplayer pilot{ get; private set; }
	[SerializeField]
	protected TerminalControl control;

	protected override void Awake () {
		base.Awake ();

		//SetupDockingAndLaunching();

        isUnderRepair = false; //HACK, TODO

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

        if( fireWeapon ) {
            switch( currentWeapon ) {
                case 1:
                    weapon1.Fire();
                    break;
                case 2:
                    weapon2.Fire();
                    break;
                case 3:
                    weapon3.Fire();
                    break;
            }
        }
    }


    //Our vector to rotate towards, which happens to also be our free look vector
    protected Quaternion targetLookDirection = Quaternion.identity;
    private Quaternion targetLookDirectionToSync = Quaternion.identity;
    public void UpdateLookVector( Quaternion newQuat ) {
        targetLookDirectionToSync = newQuat;
    }

    protected bool isBoostActive = false;
    private bool boostSync = false;
    public void UpdateBurst( bool burst ) {
        boostSync = burst;
    }

    protected Vector3 inputDirection = Vector3.zero; //A normalized input vector
    protected bool breakButton = false;

    private Vector3 inputDirectionSync = Vector3.zero;
    private bool breakButtonSync = false;

    public void UpdateInputAndBreak( Vector3 newInput, bool newBreak ) {
        inputDirectionSync = newInput;
        breakButtonSync = newBreak;
    }
    #endregion

    #region Weapons
    private bool weaponSwitchCooldown = false;
    [SerializeField]
    //Weapon Switch cooldown time
    private float weaponSwitchCooldownTime = 0.3f;

    private IEnumerator WeaponSwitchCooldown() {
        weaponSwitchCooldown = true;
        yield return new WaitForSeconds( weaponSwitchCooldownTime );
        weaponSwitchCooldown = false;
    }

    protected TerminalWeapon weapon1, weapon2, weapon3;

    protected abstract void AssignWeapons( string weaponSelection );

    protected int currentWeapon = 1;
    protected bool fireWeapon; //The variables synced from the Host
    private bool fireWeaponToSync; //The variables to sync to the Host

    public void UpdateFireControl( bool switchToNextWeapon, bool switchToPrevWeapon, bool fireCurrentWeapon ) {

        if( !weaponSwitchCooldown ) {
            if( switchToNextWeapon ) {
                tno.Send( "SwitchWeapon", Target.Host, true );
            }

            if( switchToPrevWeapon ) {
                tno.Send( "SwitchWeapon", Target.Host, false );
            }

            if( switchToPrevWeapon || switchToNextWeapon ) {
                StartCoroutine( WeaponSwitchCooldown() );
            }
        }
        
        fireWeaponToSync = fireCurrentWeapon;

    }

    [RFC]
    protected void SwitchWeapon( bool direction ) {

        //Debug.Log( ++currentWeapon > 3 );

        if( direction ) {
            if( ++currentWeapon > 3 ) currentWeapon = 1;
        } else {
            if( --currentWeapon < 1 ) currentWeapon = 3;
        }

        if( !TNManager.isHosting ) StartCoroutine( WeaponSwitchCooldown() );

    }
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
        targetLookDirection = transform.rotation;
        targetLookDirectionToSync = targetLookDirection;
        inputDirection = Vector3.zero;
        inputDirectionSync = Vector3.zero;

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
        if( inRange != inDockingRange ) {
            if( pilot != TNManager.player && TNManager.isHosting ) {
                tno.Send( "IsInRangeToDock", pilot, inRange );
            }

            inDockingRange = inRange;

            HUD.instance.DockingRangeChange( inRange );
        }
    }
	
	public void AttemptRequestDock(){

		tno.Send( "RequestDock", Target.Host );

	}

	[RFC]
	protected void RequestDock(){
		
		//If we're somehow no longer in docking range in the time it took us to request docking, do nothing
		if (!inDockingRange) return;
		
		Flagship.instance.RequestDock( tno.uid );
		
	}

	public void DockPrep(){

		//Cleanup the control
		control.CleanUp ();

		//Unseat the pilot
		pilot = null;

		//Zero the velocity
		rigidBody.velocity = Vector3.zero;

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

    public int selectedWeapon {
        get {
            return currentWeapon;
        }
    }

    public bool isUnderRepair{ get; private set; }

    private ShipSelectButton button;
    #endregion HUD Hooks


    #region Sync To Host
    protected override void OnEnable() {
        base.OnEnable();

        if( TNManager.player == pilot ) StartCoroutine( SyncToHost() );
        //Debug.Break();
    }

    private IEnumerator SyncToHost() {

        while( true ) {
            SendDataToHost();
            yield return new WaitForSeconds( 1f / SessionManager.instance.maxNetworkUpdatesPerSecond );
        }

    }

    private void SendDataToHost() {

        tno.SendQuickly( 2, Target.Host, targetLookDirectionToSync, inputDirectionSync, breakButtonSync, boostSync, fireWeaponToSync );

    }

    [RFC(2)]
    protected void RecieveSyncOnHost( Quaternion lookDirection, Vector3 input, bool onBreak, bool boost, bool fireCurrentWeapon ){

        targetLookDirection = lookDirection;
        inputDirection = input;
        breakButton = onBreak;
        isBoostActive = boost;

        fireWeapon = fireCurrentWeapon;

    }
    #endregion Sync To Host
}
