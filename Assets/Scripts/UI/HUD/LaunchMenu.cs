using UnityEngine;
using System.Collections;

public class LaunchMenu : PimpedMonoBehaviour {

    void Update() {
        if( Input.GetButtonDown( "Launch Panel" ) ) {
            launchMenuPanel.SetActive( !launchMenuPanel.activeSelf );
            Cursor.visible = launchMenuPanel;//Debug (This feature apparently isn't working at the moment
            Cursor.lockState = launchMenuPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    #region Variables
#pragma warning disable 0649
    //The Launch Menu panel we can enable/disable
    [SerializeField]
    private GameObject launchMenuPanel;

    //The Table for the lists of terminals. Need to reposition once in a while
    [SerializeField]
    private UITable launchMenuTable;

    [SerializeField, Group( "Launch Button" )]
    private Collider launchButtonCollider;

    [SerializeField, Group( "Launch Button" )]
    private UISprite launchButtonSprite;

    //The UI Grid container for above lists
    [SerializeField, Group( "Terminal Lists" )]
    private UIGrid interceptorGrid, bomberGrid, mobileFrameGrid, droneGrid;

    //The lists of terminals, as well as the Drone build display
    [SerializeField, Group( "Terminal Lists" )]
    private GameObject droneProgress;

    //The Drone progress bar
    [SerializeField, Group( "Terminal Lists" )]
    private UITexture droneProgressBar;
#pragma warning restore 0649
    #endregion Variables

    public void TurnMenuOff() {

        //Deactivate panel
        launchMenuPanel.SetActive( false );
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Deselect Ship
        if( selectedTerminal ) {
            selectedTerminal.Select( false );
            selectedTerminal = null;
        }

        if( launchButtonCollider ) launchButtonCollider.enabled = false;
        launchButtonSprite.color = Color.gray;//DEBUG
    }


    /* Every ship will request a button be instantiated for it. The button will be disabled until the ship docks.
     * Each ship will keep a reference to its own button, so that the Flagship will be able to iterate
     * through it and turn on all buttons belonging ships currently docked.
     */
    public ShipSelectButton RequestNewShipButton( Terminal term ) {

        //Instantiate the button
        GameObject button = Instantiate<GameObject>( shipSelectPrefab );
        GameObject list = null;

        //Add the button to the correct Grid
        if( term is Interceptor ) {
            list = interceptorGrid.gameObject;
        } //Else add other ship types

        button.transform.parent = list.transform;
        button.transform.localScale = Vector3.one;
        button.transform.localPosition = Vector3.zero;
        button.SetActive( false );

        ShipSelectButton shipButton = button.GetComponent<ShipSelectButton>();
        shipButton.terminal = term;

        return shipButton;

    }

    //Reorganizes the lists of Terminals when new buttons have been enabled/disabled.
    private void ReorganizeLists() {
        interceptorGrid.repositionNow = true;
        bomberGrid.repositionNow = true;
        //Other lists as necessary

        launchMenuTable.repositionNow = true;
    }


    #region Ship Selection
#pragma warning disable 0649
    //Prefab for ship selection button
    [SerializeField, Group( "Ship Selection" )]
    private GameObject shipSelectPrefab;

    //Colors for ship button selections
    [Group( "Ship Selection" )]
    public Color selectedColor, deselectedColor, disabledColor;
#pragma warning restore 0649

    private ShipSelectButton selectedTerminal;

    public void SelectTerminal( ShipSelectButton term ) {

        if( selectedTerminal ) {
            bool quit = selectedTerminal == term;
            selectedTerminal.Select( false );
            if( quit ) {
                selectedTerminal = null;
                launchButtonCollider.enabled = false;
                launchButtonSprite.color = Color.gray;//DEBUG
                return;
            }
        }

        selectedTerminal = term;
        selectedTerminal.Select( true );
        if( !launchButtonCollider.enabled ) {
            launchButtonCollider.enabled = true;
            launchButtonSprite.color = Color.blue;//DEBUG
        }

    }

    #endregion Ship Selection

    public void LaunchTerminal() {
        Debug.Log( "Launch Button Clicked" );
        //Fire off message to launch
        Flagship.instance.RequestLaunchTerminal( selectedTerminal.terminal.tno.uid );
    }

}
