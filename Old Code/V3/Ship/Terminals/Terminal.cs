using UnityEngine;
using System.Collections;
using TNet;

public abstract class Terminal : Ship {

	[SerializeField]
	private TerminalPilot control;

	private Player pilot;

	private bool readyToDock = false;
	private Carrier receivingCarrier = null;

	public override bool ContainsPlayer (Player check)
	{
		return pilot == check;
	}

	[RFC]
	public override void AssignDefault (Player pilot)
	{
		if (TNManager.isHosting) {
			//What should be a redundent check
			if( !gameObject.activeSelf ){
				Debug.LogException( new UnityException( "Seating a pilot onto an inactive Terminal" ) );
			}

			tno.Send( "AssignDefault", Target.Others, pilot );

			PlayersManager.instance.UpdateFocusChange ( pilot, tno.uid, "Pilot" );
		}

		if (pilot == TNManager.player) {
			CameraControls.instance.SetTarget( transform, control );

			LaunchMenuManager.instance.Launched ();
			
			Screen.lockCursor = true;
		}

		this.pilot = pilot;
	}

	public void CleanUp(){
		pilot = null;

		rigidbody.velocity = Vector3.zero;
		gameObject.SetActive( false );
	}

	#region Carrier Interaction
	[RFC]
	//Called when launched from a carrier
	public virtual void OnLaunch ( Quaternion facing, Player toBeSeated ){

		//Unparent ourself
		transform.parent = null;

		//Reset our Scale (because for some reason the scale gets messed up when we parent
		transform.localScale = Vector3.one;

		control.OnLaunch (facing);

		//Set ourself to active
		gameObject.SetActive (true);

		if (TNManager.isHosting) {
			rigidbody.AddRelativeForce( Vector3.forward * 10, ForceMode.Impulse );

			AssignDefault (toBeSeated);
		}

	}


	//Called when entering a docking area for a carrier
	public void ReadyForDocking( Carrier carrier ){

		receivingCarrier = carrier;
		readyToDock = true;

	}

	//Called when leaving the docking area
	public void LeavingDockingArea(){

		readyToDock = false;
		receivingCarrier = null;

	}

	[RFC]
	public void InitiateDocking(){

		if ( !TNManager.isHosting ) {
			tno.Send( "InitiateDocking", Target.Host );
			return;
		}

		if ( !readyToDock ) return;

		receivingCarrier.ApplyDock( pilot, tno.uid );

	}
	#endregion

}
