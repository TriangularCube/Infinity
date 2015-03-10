using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Netplayer = TNet.Player;

public class HUD : Singleton<HUD> {
	
	void Start(){
		//SetupHUD();
        SetupTerminalHUD();
	}

	void Update(){

		SuppressHUD();

		DrawIndicators();

        if( activeTerminal ) UpdateTerminalHUD();
        else if( onFlagship ) UpdateFlagshipHUD();
        //else throw new UnityException( "Neither Active Terminal nor Carrier is defined in HUD" );

        if( Input.GetButtonDown( "Launch Panel" ) && onFlagship ) {
            launchMenuPanel.SetActive( !launchMenuPanel.activeSelf );
            Cursor.visible = launchMenuPanel;//Debug
            Cursor.lockState = launchMenuPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }
        //if( launchMenuPanel ) UpdateLaunchMenu();

	}

    /*
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

            //TODO Populate the Ship Lists
            UpdateList = true;

        } else {

            //Else disable the display box for that ally
            allyIndicatorList[dockedEvent.pilot].gameObject.SetActive( false );

            //TODO Add newly docked terminal into list

        }

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

            launchMenuPanel.SetActive( false );
            DisableCarrierHUD();
            EnableTerminalHUD( launchEvent.terminal );

            //TODO Wipe the ship lists

        } else {

            //Else enable the display box for that ally
            allyIndicatorList[launchEvent.terminal.pilot].gameObject.SetActive( true );

            //TODO Remove the launched terminal from lists

        }

        return false;

    }
    #endregion HUD Listeners
    */

    #region HUD Switch Interaction
    public void AllyShipLaunched( Terminal term ) {
        //Turn indicator on
        GameObject indicator = Instantiate<GameObject>( allyTargetingPrefab );
        indicator.transform.parent = HUDPanel.transform.FindChild( "Indicators" );
        indicator.transform.localScale = Vector3.one;
        indicator.transform.localPosition = Vector3.zero;

        allyIndicatorList.Add( term.transform, indicator.GetComponent<AllyIndicator>() );
    }

    public void AllyShipDocked( Terminal term ) {
        //Turn indicator off
    }

    public void PlayerShipLaunched( Terminal term ) {
        //Turn all Flagship HUD off
        launchMenuPanel.gameObject.SetActive( false );

        //Turn Terminal HUD on
        EnableTerminalHUD( term );
    }

    public void PlayerShipDocked() {
        //Turn Terminal HUD off
        DisableTerminalHUD();
    }

    //TODO
    public void RoleAssigned( /*Some Role*/ ) {

        //DEBUG
        onFlagship = true;
        flagshipIndicator.gameObject.SetActive( false );

    }
    #endregion HUD Switch Interaction

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
	private Flagship flagship;
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
        //EventManager.instance.AddListener( "AllyDocked", AllyDocked );
        //EventManager.instance.AddListener( "AllyLaunched", AllyLaunched );

	}
    */

    private void SuppressHUD(){

		if( suppressHUD )
			HUDPanel.alpha = SuppressedAlpha;
		else 
			HUDPanel.alpha = UnsupressedAlpha;

	}
	
	private void DrawIndicators(){
		//Update Flagship Indicator
		if( flagshipIndicator.gameObject.activeSelf ){
			DrawIndicatorOnScreen( flagship.transform.position, flagshipIndicator );
		}

        //Draw Ally indicators
        foreach( Transform ally in allyIndicatorList.Keys ){
            //DrawIndicatorOnScreen( ally.position, allyIndicatorList[ally] );
        }

        //Draw other indicators
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

    /*
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
    */

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

    public void DockingRangeChange( bool inRange ) {
        dockingRangeIndicator.SetActive( inRange );
    }

    private void EnableTerminalHUD( Terminal term ) {

        activeTerminal = term;

        weapons = activeTerminal.weaponSelection;

        //Set the weapon name labels
        firstWeaponLabel.text = weapons[0].weaponName;
        secondWeaponLabel.text = weapons[1].weaponName;
        thirdWeaponLabel.text = weapons[2].weaponName;

        TerminalHUD.SetActive( true );

    }
    private void DisableTerminalHUD() {

        TerminalHUD.SetActive( false );

        activeTerminal = null;
        weapons = null;

        selectedWeapon = 1;

        firstWeaponLabel.text = "Weapon 1";
        secondWeaponLabel.text = "Weapon 2";
        thirdWeaponLabel.text = "Weapon 3";

        dockingRangeIndicator.SetActive( false );

    }
    #endregion Terminal HUD

    #region Carrier HUD
    bool onFlagship = false;

    private void UpdateFlagshipHUD() {
        //TODO
    }

    private void EnableFlagshipHUD() {

    }
    private void DisableFlagshipHUD() {

    }
    #endregion Carrier HUD

    #region Launch Menu
#pragma warning disable 0649
    //The Launch Menu panel we can enable/disable
    [SerializeField, Group( "Launch Menu" )]
	private GameObject launchMenuPanel;

    //The Table for the lists of terminals. Need to reposition once in a while
    [SerializeField, Group( "Launch Menu" )]
	private UITable launchMenuTable;

    //The two buttons...although I'm not sure we really need a reference to them
    [SerializeField, Group( "Launch Menu" )]
    private UIButton RequestButton, LaunchButton;

    //The lists of terminals, as well as the Drone build display
    [SerializeField, Group( "Launch Menu" )]
    private GameObject InterceptorList, BomberList, MobileFrameList, DroneList, DroneProgress;

    //The Drone progress bar
    [SerializeField, Group( "Launch Menu" )]
    private UITexture DroneProgressBar;

    [SerializeField, Group( "Launch Menu" )]
    private GameObject ShipSelectPrefab;
#pragma warning restore 0649

    private ShipSelectButton selectedTerminal;
    private bool UpdateList = false;

    public ShipSelectButton RequestNewShipButton( Terminal term ) {
        
        //Instantiate the button
        GameObject button = Instantiate<GameObject>( ShipSelectPrefab );
        GameObject list = null;

        //Add the button to the correct Grid
        if( term is Interceptor ) {
            list = InterceptorList;
        } //Else add other ship types

        button.transform.parent = list.transform;
        button.transform.localScale = Vector3.one;
        button.transform.localPosition = Vector3.zero;
        list.GetComponent<UIGrid>().repositionNow = true;

        ShipSelectButton shipButton = button.GetComponent<ShipSelectButton>();
        shipButton.terminal = term;

        return shipButton;

    }

    private void ReorganizeLists() {
        InterceptorList.GetComponent<UIGrid>().Reposition();
        BomberList.GetComponent<UIGrid>().Reposition();
        //Other lists as necessary
    }

    /*
    private void PopulateList() {
            Dictionary<Terminal, Netplayer> reserveList = activeCarrier.getReserveList();

            foreach( Terminal term in activeCarrier.getDockedTerminals() ) {
                if( term is Interceptor ) {
                    InsertSelection( InterceptorList, term, reserveList[term] != null );
                }
                //Else if Bomber, and FRAME, and Drones

                Debug.Log( "Populated Once" );
            }

            PopulateShipList = false;
            launchMenuTable.repositionNow = true;
        //TODO update all terminals
    }


    private void InsertSelection( GameObject list, Terminal term, bool reserved ) {
        GameObject obj = (GameObject)Instantiate( ShipSelectPrefab );
        obj.GetComponentInChildren<ShipSelectButton>().terminal = term;

        obj.transform.parent = list.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;

        if( reserved ) obj.GetComponentInChildren<ShipSelectButton>().Disable();
    }
     * 
     * */

    public void SelectTerminal( ShipSelectButton term ) {

        if( selectedTerminal ) {
            bool quit = selectedTerminal == term;
            selectedTerminal.Select( false );
            if ( quit ){
                selectedTerminal = null;
                return;
            }
        }

        selectedTerminal = term;
        selectedTerminal.Select( true );

    }

    public void RequestTerminal() {
        if( !selectedTerminal ) return;

        //activeCarrier.RequestReserveTerminal( selectedTerminal.terminal );
    }
    #endregion Launch Menu
}
