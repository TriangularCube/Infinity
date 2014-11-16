using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : Singleton<HUD> {

	protected override void Awake(){
		base.Awake ();
		
		//Initializing the Flagship's dedicated box
		flagshipIndicator = ((GameObject)Instantiate(allyTargetingPrefab)).GetComponent<AllyIndicator>();
		flagshipIndicator.transform.parent = HUDPanel.transform;
		flagshipIndicator.transform.localScale = Vector3.one;
		
		//TODO Initialize the players' terminal display boxes
		
		//Register our listeners
		EventManager.instance.AddListener( "AllyDocked", AllyDocked );
		EventManager.instance.AddListener( "AllyLaunched", AllyLaunched );

		EventManager.instance.AddListener( "EnteringDockingRange", TerminalEnteringDockingRange );
		EventManager.instance.AddListener( "LeavingDockingRange", TerminalLeavingDockingRange );

	}

	#region HUD
	[SerializeField]
	private GameObject HUDPanel;
	
	[SerializeField]
	private GameObject allyTargetingPrefab;
	[SerializeField]
	private float screenPadding = 0.485f;

	[SerializeField]
	private Carrier flagship;
	private AllyIndicator flagshipIndicator;

	[SerializeField]
	private Dictionary<Transform, GameObject> playerList = new Dictionary<Transform, GameObject>();

	[SerializeField]
	private Camera playerCamera;

	private bool TerminalEnteringDockingRange( IEvent evt ){

		EnteringDockingRange edr = (EnteringDockingRange)evt;

		//If it's not us, we don't care
		if( edr.terminal.pilot != TNManager.player ) return false;

		//TODO Turn on Within Range notification

		return false;
	}

	private bool TerminalLeavingDockingRange( IEvent evt ){

		LeavingDockingRange ldr = (LeavingDockingRange)evt;

		//If it's not us, we don't care
		if( ldr.terminal.pilot != TNManager.player ) return false; 

		//TODO Turn off Within Range notification

		return false;
	}

	private bool AllyDocked( IEvent evt ){

		AllyDocked dockedEvent = (AllyDocked) evt;

		//If this is us
		if (dockedEvent.terminal.pilot == TNManager.player) {

			if( dockedEvent.carrier == flagship ){

				//Disable the flagship's display box since we landed on it
				flagshipIndicator.gameObject.SetActive( false );
			
			} else {

				//TODO Disable display box for whatever ship we landed on

			}

		} else {

			//Else disable the display box for that ally
			playerList [dockedEvent.terminal.transform].SetActive (false);

		}

		//TODO Add this guy's name to the carrier's name list?

		return false;

	}

	private bool AllyLaunched( IEvent evt ){

		AllyLaunched launchEvent = (AllyLaunched)evt;

		//If this is us
		if (launchEvent.terminal.pilot == TNManager.player) {
						
			if( launchEvent.carrier == flagship ){
				
				//Enable the flagship's display box since we just launched from it
				flagshipIndicator.gameObject.SetActive( true );
				
			} else {
				
				//TODO Enable display box for whatever ship we landed on
				
			}
			
		} else {
			
			//Else enable the display box for that ally
			playerList [launchEvent.terminal.transform].SetActive (true);
			
		}

		return false;

	}
	#endregion

	#region Launch Menu
	[SerializeField]
	private GameObject launchMenuPanel;
	[SerializeField]
	private UIGrid launchMenuGrid;
	#endregion
}
