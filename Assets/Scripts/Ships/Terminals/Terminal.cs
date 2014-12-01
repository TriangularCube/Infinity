using UnityEngine;
using System.Collections;
using TNet;

using Netplayer = TNet.Player;

public abstract class Terminal : Ship {

	public Netplayer pilot{ get; private set; }
	[SerializeField]
	protected TerminalControl control;

	#region Override
	/*
	public bool ContainsPlayer ( Netplayer check ){
		return pilot == check ? true : false;
	}
	*/

	public void AssignPilot( Netplayer player, string weaponSelection ){
		pilot = player;

		if (pilot == TNManager.player) {
			//CameraControls.instance.SetTarget( transform, control );

			//Activate the controls
			control.Assign();
			
			Screen.lockCursor = true;
		}

		//TODO, DEBUG
		AssignWeapons( weaponSelection );
	}

	protected abstract void AssignWeapons( string weaponSelection );

	protected override void Awake ()
	{
		base.Awake ();

		SetupDockingAndLaunching();

		/*
		//Disable ourselves if we're parented to something
		if( transform.parent ){
			gameObject.SetActive( false );
		}
		*/
	}
	#endregion

	#region Launching and Docking
	//Reference of the carrier we're in range to dock into
	private Carrier carrierToDockInto = null;
	//A boolean to transfer across network
	private bool inCarrierRange = false;

	private void SetupDockingAndLaunching(){
		if (TNManager.isHosting) {
			
			//Register Listeners
			EventManager.instance.AddListener ("EnteringDockingRange", EnteringDockingZone);
			EventManager.instance.AddListener ("LeavingDockingRange", LeavingDockingZone);
			
		}
	}

	//These listeners are only called on Host
	private bool EnteringDockingZone( IEvent evt ){

		EnteringDockingRange edr = (EnteringDockingRange)evt;

		if (edr.terminal != this) return false;

		carrierToDockInto = edr.carrier;
		inCarrierRange = true;

		return false;

	}

	private bool LeavingDockingZone( IEvent evt ){

		LeavingDockingRange ldr = (LeavingDockingRange)evt;

		if (ldr.terminal != this) return false;

		carrierToDockInto = null;
		inCarrierRange = false;

		return false;

	}
	
	public void OnLaunch( Netplayer toBeSeated, string weaponSelection ){
		//Unparent ourself
		transform.parent = null;
		
		//Reset our Scale (because for some reason the scale gets messed up when we parent)
		transform.localScale = Vector3.one;

		//Set ourself to active
		gameObject.SetActive (true);

		//Assign the player to the pilot position
		AssignPilot( toBeSeated, weaponSelection );

		//TODO Add some forwards momentum
		rigidbody.AddRelativeForce( Vector3.forward * 10, ForceMode.Impulse );

	}
	
	public void AttemptRequestDock(){

		//If we're not in docking range, do nothing
		if (!inCarrierRange) return;

		tno.Send( "RequestDock", Target.Host );

	}

	[RFC]
	protected void RequestDock(){
		
		//If we're somehow no longer in docking range in the time it took us to request docking, do nothing
		if (!inCarrierRange) return;
		
		carrierToDockInto.RequestDock (this);
		
	}

	public void DockPrep(){

		//Cleanup the control
		control.CleanUp ();

		//Unseat the pilot
		pilot = null;

		//Zero the velocity
		rigidbody.velocity = Vector3.zero;

		//Disable the Terminal
		gameObject.SetActive(false);

	}
	#endregion

	#region Station and Attitude Controls
	public abstract void UpdateLookVector( Quaternion newQuat );
	public abstract void UpdateBurst( bool burst );
	public abstract void UpdateInputAndBreak( Vector3 input, bool breakButton );
	public abstract void UpdateFireControl( bool nextWeapon, bool prevWeapon, bool fireWeapon );
	#endregion
}
