using UnityEngine;
using System.Collections;
using TNet;

using Netplayer = TNet.Player;

public abstract class Terminal : Ship {

	public Netplayer pilot{ get; private set; }
	[SerializeField]
	private TerminalControl control;

	//Reference of the carrier we're in range to dock into
	private Carrier carrierToDockInto = null;
	//A boolean to transfer across network
	private bool inCarrierRange = false;

	#region Override
	public override bool ContainsPlayer ( Netplayer check ){
		return pilot == check ? true : false;
	}

	public override void AssignDefault ( Netplayer player ){
		pilot = player;

		if (pilot == TNManager.player) {
			//CameraControls.instance.SetTarget( transform, control );

			//Activate the controls
			control.enabled = true;
			
			Screen.lockCursor = true;
		}
	}

	protected override void Awake ()
	{
		base.Awake ();

		if (TNManager.isHosting) {

			//Register Listeners
			EventManager.instance.AddListener ("EnteringDockingRange", EnteringDockingZone);
			EventManager.instance.AddListener ("LeavingDockingRange", LeavingDockingZone);
		
		}

	}
	#endregion

	//These listeners are only called on Host
	#region Listeners
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
	#endregion

	#region Launching and Docking
	public void OnLaunch( Netplayer toBeSeated ){
		//Unparent ourself
		transform.parent = null;
		
		//Reset our Scale (because for some reason the scale gets messed up when we parent)
		transform.localScale = Vector3.one;

		//Set ourself to active
		gameObject.SetActive (true);

		//Assign the player to the pilot position
		AssignDefault (toBeSeated);

		//TODO Add some forwards momentum
		rigidbody.AddRelativeForce( Vector3.forward * 10, ForceMode.Impulse );

	}
	
	public void AttemptRequestDock(){

		//If we're not in docking range, do nothing
		if (!inCarrierRange) return;

		tno.Send ("RequestDock", Target.Host);

	}

	[RFC]
	private void RequestDock(){

		//If we're somehow no longer in docking range in the time it took us to request docking, do nothing
		if (!inCarrierRange) return;

		EventManager.instance.QueueEvent (new RequestDock (this, carrierToDockInto));

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

}
