using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Netplayer = TNet.Player;

public class HUD : Singleton<HUD> {
	
	void Start(){
		SetupHUD();
        SetupTerminalHUD();
	}

	void Update(){

		SuppressHUD();

		DrawIndicators();

        if( activeTerminal )  UpdateTerminalHUD();

        if( activeCarrier ) UpdateCarrierHUD();

        if( launchMenuPanel ) UpdateLaunchMenu();

	}

    #region HUD Listeners
    private bool AllyDocked( IEvent evt ) {

        AllyDocked dockedEvent = (AllyDocked)evt;

        //If this is us
        if( dockedEvent.pilot == TNManager.player ) {

            if( dockedEvent.carrier == flagship ) {

                //Disable the flagship's display box since we landed on it
                flagshipIndicator.gameObject.SetActive( false );

            } else {

                // TODO Disable display box for whatever ship we landed on.

            }

            //Turn off the Docking Range indicator
            dockingRangeIndicator.SetActive( false );

            DisableTerminalHUD();
            EnableCarrierHUD( dockedEvent.carrier );

        } else {

            //Else disable the display box for that ally
            allyIndicatorList[dockedEvent.pilot].gameObject.SetActive( false );

        }

        // TODO Add this guy's name to the carrier's name list?

        return false;

    }

    private bool AllyLaunched( IEvent evt ) {

        AllyLaunched launchEvent = (AllyLaunched)evt;

        //If this is us
        if( launchEvent.terminal.pilot == TNManager.player ) {

            if( launchEvent.carrier == flagship ) {

                //Enable the flagship's display box since we just launched from it
                flagshipIndicator.gameObject.SetActive( true );

            } else {

                //TODO Enable display box for whatever ship we Lauched From

            }

            EnableTerminalHUD( launchEvent.terminal );

        } else {

            //Else enable the display box for that ally
            allyIndicatorList[launchEvent.terminal.pilot].gameObject.SetActive( true );

        }

        return false;

    }
    #endregion HUD Listeners

	#region General HUD
#pragma warning disable 0649
    private bool suppressHUD = false;
	[SerializeField, Group( "General HUD" )]
	private float SuppressedAlpha = 0.2f;
    [SerializeField, Group( "General HUD" )]
	private float UnsupressedAlpha = 1f;

    [SerializeField, Group( "General HUD" )]
	private UIPanel HUDPanel;
    [SerializeField, Group( "General HUD" )]
	private float screenPadding = 0.485f;

    [SerializeField, Group( "General HUD" )]
	private Camera playerCamera;


    [SerializeField, Group( "General HUD" )]
	private Carrier flagship;
    [SerializeField, Group( "General HUD" )]
	private AllyIndicator flagshipIndicator;


    [SerializeField, Group( "General HUD" )]
	private GameObject allyTargetingPrefab;
#pragma warning restore 0649
    private Dictionary<Netplayer, AllyIndicator> allyIndicatorList = new Dictionary<Netplayer, AllyIndicator>();
	private Dictionary<Netplayer, Transform> allyTransformList = new Dictionary<Netplayer, Transform>();

	//On Start
	private void SetupHUD(){

		//TODO Initialize the players' terminal display boxes
		if( !flagship.ContainsPlayer( TNManager.player ) ){
			flagshipIndicator.Activate();
		}

        //Register our listeners
        EventManager.instance.AddListener( "AllyDocked", AllyDocked );
        EventManager.instance.AddListener( "AllyLaunched", AllyLaunched );

	}

    private void SuppressHUD(){

		if( suppressHUD )
			HUDPanel.alpha = SuppressedAlpha;
		else 
			HUDPanel.alpha = UnsupressedAlpha;

	}
	
	private void DrawIndicators(){
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
					viewportPoint.x = -( tan * screenPadding );
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
					viewportPoint.y = -( screenPadding / tan );
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
	#endregion General HUD

    #region Terminal HUD
    private void SetupTerminalHUD() {

        //Register Listeners
        EventManager.instance.AddListener( "EnteringDockingRange", TerminalEnteringDockingRange );
        EventManager.instance.AddListener( "LeavingDockingRange", TerminalLeavingDockingRange );

        //Setup Position Refrences for Weapon Labels
        firstWeaponPosition = firstWeaponLabel.transform.position;
        secondWeaponPosition = secondWeaponLabel.transform.position;
        thirdWeaponPosition = thirdWeaponLabel.transform.position;

    }

    private Terminal activeTerminal = null;

#pragma warning disable 0649
    [SerializeField, Group( "Terminal HUD" )]
    private GameObject TerminalHUD;

    [SerializeField, Group( "Terminal HUD" )]
    private GameObject dockingRangeIndicator;

    [SerializeField, Group( "Terminal HUD" )]
    private Transform WeaponSelectionLabel;

    [SerializeField, Group( "Terminal HUD" )]
    private UILabel firstWeaponLabel, secondWeaponLabel, thirdWeaponLabel;
    [SerializeField, Group( "Terminal HUD" )]
    private UILabel firstWeaponAmmoCounter, secondWeaponAmmoCounter, thirdWeaponAmmoCounter;
    [SerializeField, Group( "Terminal HUD" )]
    private UITexture firstWeaponHeatDisplay, secondWeaponHeatDisplay, thirdWeaponHeatDisplay;
    [SerializeField, Group( "Terminal HUD" )]
    private Color32 notOverheated, overheated;

    private Vector3 firstWeaponPosition;
    private Vector3 secondWeaponPosition;
    private Vector3 thirdWeaponPosition;
#pragma warning restore 0649

    private void UpdateTerminalHUD() {
        UpdateTerminalWeaponStatus();
    }

    #region Terminal Listeners
    private bool TerminalEnteringDockingRange( IEvent evt ) {

        EnteringDockingRange edr = (EnteringDockingRange)evt;

        //If it's not us, we don't care
        if( edr.terminal != activeTerminal ) return false;

        //TODO Turn on Within Range notification
        dockingRangeIndicator.SetActive( true );

        return false;
    }

    private bool TerminalLeavingDockingRange( IEvent evt ) {

        LeavingDockingRange ldr = (LeavingDockingRange)evt;

        //If it's not us, we don't care
        if( ldr.terminal != activeTerminal ) return false;

        //TODO Turn off Within Range notification
        dockingRangeIndicator.SetActive( false );

        return false;
    }
    #endregion Terminal Listeners

    #region Terminal Weapons
    private TerminalWeapon[] weapons;
    private int selectedWeapon = 1;

    private void UpdateTerminalWeaponStatus() {

        if( selectedWeapon != activeTerminal.selectedWeapon ) {

            selectedWeapon = activeTerminal.selectedWeapon;

            switch( selectedWeapon ) {
                case 1:
                    WeaponSelectionLabel.position = firstWeaponPosition;
                    break;
                case 2:
                    WeaponSelectionLabel.position = secondWeaponPosition;
                    break;
                case 3:
                    WeaponSelectionLabel.position = thirdWeaponPosition;
                    break;
                default:
                    throw new UnityException( "Weapon Selection is not the three weapons" );
            }

        }

        firstWeaponAmmoCounter.text = weapons[0].reserve.ToString();
        secondWeaponAmmoCounter.text = weapons[1].reserve.ToString();
        thirdWeaponAmmoCounter.text = weapons[2].reserve.ToString();

        if( weapons[0].isOverHeated )
            firstWeaponHeatDisplay.color = overheated;
        else
            firstWeaponHeatDisplay.color = notOverheated;
        firstWeaponHeatDisplay.fillAmount = weapons[0].currentHeat;

        if( weapons[1].isOverHeated )
            secondWeaponHeatDisplay.color = overheated;
        else
            secondWeaponHeatDisplay.color = notOverheated;
        secondWeaponHeatDisplay.fillAmount = weapons[1].currentHeat;

        if( weapons[2].isOverHeated )
            thirdWeaponHeatDisplay.color = overheated;
        else
            thirdWeaponHeatDisplay.color = notOverheated;
        thirdWeaponHeatDisplay.fillAmount = weapons[2].currentHeat;



    }
    #endregion Terminal Weapons

    private void EnableTerminalHUD( Terminal term ) {

        activeTerminal = term;

        weapons = activeTerminal.weaponSelection;

        //Set the weapon name labels
        firstWeaponLabel.text = weapons[0].weaponName;
        secondWeaponLabel.text = weapons[1].weaponName;
        thirdWeaponLabel.text = weapons[2].weaponName;

    }
    private void DisableTerminalHUD() {

        TerminalHUD.SetActive( false );

        activeTerminal = null;
        weapons = null;

        selectedWeapon = 1;

        firstWeaponLabel.text = "Weapon 1";
        secondWeaponLabel.text = "Weapon 2";
        thirdWeaponLabel.text = "Weapon 3";

    }
    #endregion Terminal HUD

    #region Carrier HUD
    private Carrier activeCarrier = null;

    private void UpdateCarrierHUD() {
        //TODO
    }

    private void EnableCarrierHUD( Carrier car ) {

        activeCarrier = car;

    }
    private void DisableCarrierHUD() {

        activeCarrier = null;

    }
    #endregion Carrier HUD

    #region Launch Menu
#pragma warning disable 0649
    [SerializeField, Group( "Launch Menu" )]
	private GameObject launchMenuPanel;
    [SerializeField, Group( "Launch Menu" )]
	private UITable launchMenuTable;
    [SerializeField, Group( "Launch Menu" )]
    private UIButton RequestButton, LaunchButton;
    [SerializeField, Group( "Launch Menu" )]
    private GameObject InterceptorList, BomberList, MobileFrameList, DroneList, DroneProgress;
    [SerializeField, Group( "Launch Menu" )]
    private UITexture DroneProgressBackground, DroneProgressBar;
#pragma warning restore 0649

    void UpdateLaunchMenu() {
        //TODO
    }
    #endregion Launch Menu
}
