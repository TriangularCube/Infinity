using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Netplayer = TNet.Player;

public class HUD : Singleton<HUD> {
	
	protected override void Awake(){
		base.Awake ();

		//Register our listeners
		EventManager.instance.AddListener( "AllyDocked", AllyDocked );
		EventManager.instance.AddListener( "AllyLaunched", AllyLaunched );
		
		//Only add these events on the Host, since only the host runs the "correct" physics simulation
		EventManager.instance.AddListener( "EnteringDockingRange", TerminalEnteringDockingRange );
		EventManager.instance.AddListener( "LeavingDockingRange", TerminalLeavingDockingRange );
	}

	void Start(){
		SetupHUD ();
	}

	void Update(){

		SuppressHUD();

		DrawIndicators();
	
	}

	#region Listeners
	private bool TerminalEnteringDockingRange( IEvent evt ){
		
		EnteringDockingRange edr = (EnteringDockingRange)evt;
		
		//If it's not us, we don't care
		if( edr.terminal.pilot != TNManager.player ) return false;
		
		//TODO Turn on Within Range notification
		dockingRangeIndicator.SetActive( true );
		
		return false;
	}
	
	private bool TerminalLeavingDockingRange( IEvent evt ){
		
		LeavingDockingRange ldr = (LeavingDockingRange)evt;
		
		//If it's not us, we don't care
		if( ldr.terminal.pilot != TNManager.player ) return false; 
		
		//TODO Turn off Within Range notification
		dockingRangeIndicator.SetActive( false );
		
		return false;
	}
	
	private bool AllyDocked( IEvent evt ){
		
		AllyDocked dockedEvent = (AllyDocked) evt;
		
		//If this is us
		if( dockedEvent.pilot == TNManager.player ){
			
			if( dockedEvent.carrier == flagship ){
				
				//Disable the flagship's display box since we landed on it
				flagshipIndicator.gameObject.SetActive( false );
				
			} else {
				
				//TODO Disable display box for whatever ship we landed on
				
			}

			dockingRangeIndicator.SetActive( false );
			
		} else {
			
			//Else disable the display box for that ally
			allyIndicatorList[dockedEvent.pilot].gameObject.SetActive( false );
			
		}
		
		//TODO Add this guy's name to the carrier's name list?
		
		return false;
		
	}
	
	private bool AllyLaunched( IEvent evt ){
		
		AllyLaunched launchEvent = (AllyLaunched)evt;
		
		//If this is us
		if ( launchEvent.terminal.pilot == TNManager.player ) {

			if( launchEvent.carrier == flagship ){
				
				//Enable the flagship's display box since we just launched from it
				flagshipIndicator.gameObject.SetActive( true );
				
			} else {
				
				//TODO Enable display box for whatever ship we landed on
				
			}
			
		} else {
			
			//Else enable the display box for that ally
			allyIndicatorList[launchEvent.terminal.pilot].gameObject.SetActive( true );
			
		}
		
		return false;
		
	}
	#endregion

	#region HUD
	private bool suppressHUD = false;
	[SerializeField]
	private float SuppressedAlpha = 0.2f;
	[SerializeField]
	private float UnsupressedAlpha = 1f;

	[SerializeField]
	private UIPanel HUDPanel;
	[SerializeField]
	private float screenPadding = 0.485f;
	
	[SerializeField]
	private Camera playerCamera;
	
	
	[SerializeField]
	private Carrier flagship;
	[SerializeField]
	private AllyIndicator flagshipIndicator;
	
	
	[SerializeField]
	private GameObject allyTargetingPrefab;
	private Dictionary<Netplayer, AllyIndicator> allyIndicatorList = new Dictionary<Netplayer, AllyIndicator>();
	private Dictionary<Netplayer, Transform> allyTransformList = new Dictionary<Netplayer, Transform>();

	[SerializeField]
	private GameObject dockingRangeIndicator;

	//On Start
	private void SetupHUD(){
		
		//TODO Initialize the players' terminal display boxes
		if( !flagship.ContainsPlayer( TNManager.player ) ){
			flagshipIndicator.gameObject.SetActive( true );
		}

	}

	private void SuppressHUD(){

		if( suppressHUD )
			HUDPanel.alpha = SuppressedAlpha;
		else 
			HUDPanel.alpha = UnsupressedAlpha;

	}
	
	void DrawIndicators(){
		//Update Flagship Indicator
		if( flagship.gameObject.activeSelf ){
			DrawIndicatorOnScreen( flagship.transform.position, flagshipIndicator );
		}
	}

	//This is the main process of determining if the target is off the screen, and draws the appropriate indicator at the proper location
	void DrawIndicatorOnScreen( Vector3 targetPosition, AllyIndicator indicator ){
		
		//Find the viewport position of the target
		Vector3 viewportPoint = playerCamera.WorldToViewportPoint (targetPosition);
		
		//Center the origin point
		viewportPoint.x -= 0.5f;
		viewportPoint.y -= 0.5f;
		
		
		if( viewportPoint.z > 0f && viewportPoint.y > -screenPadding && viewportPoint.y < screenPadding && viewportPoint.x > -screenPadding && viewportPoint.x < screenPadding ){
			
			//The target is within the screen
			
			//Set the indicator to Box
			indicator.SetBox ();
			
			//Set the position and rotation of the indicator
			indicator.transform.rotation = Quaternion.identity;
			
		} else {
			//The target is off the screen
			
			//Set the indicator to arrow
			indicator.SetArrow ();
			
			//Flip the viewport coordinates if the enemy is behind us
			if( viewportPoint.z < 0f ) viewportPoint *= -1;
			
			//Find the wide angle
			float rotationAngle = Vector3.Angle( Vector3.up, new Vector3( viewportPoint.x, viewportPoint.y, 0f ) );
			if( viewportPoint.x > 0f ){
				rotationAngle *= -1;
			}
			
			//Set our rotation
			indicator.transform.rotation = Quaternion.Euler( 0f, 0f, rotationAngle );
			
			float sin = Mathf.Sin( rotationAngle * Mathf.Deg2Rad );
			float cos = Mathf.Cos( rotationAngle * Mathf.Deg2Rad );
			float tan = Mathf.Tan( rotationAngle * Mathf.Deg2Rad );
			
			if( Mathf.Abs( cos ) > Mathf.Abs( sin ) ){
				
				//We're touching a vertical bound
				
				if( cos > 0f ){
					//If we're on the top half of screen
					viewportPoint.y = screenPadding;
					viewportPoint.x = - ( tan * screenPadding );
				} else {
					//If we're on the bottom half of the screen
					viewportPoint.y = -screenPadding;
					viewportPoint.x = tan * screenPadding;
				}
				
			} else {
				//We're touching a horizontal bound
				
				if( sin < 0f ){
					//If we're on the right side
					viewportPoint.x = screenPadding;
					viewportPoint.y = - ( screenPadding / tan );
				} else {
					//If we're on the left side
					viewportPoint.x = -screenPadding;
					viewportPoint.y = screenPadding / tan;
				}
			}
			
		}
		
		//Zero out the Z
		viewportPoint.z = 0f;
		
		viewportPoint.x *= Screen.width;
		viewportPoint.y *= Screen.height;
		
		//Set the position
		indicator.transform.localPosition = viewportPoint;
		
	}
	#endregion
	
	#region Launch Menu
	[SerializeField]
	private GameObject launchMenuPanel;
	[SerializeField]
	private UIGrid launchMenuGrid;
	#endregion
}
