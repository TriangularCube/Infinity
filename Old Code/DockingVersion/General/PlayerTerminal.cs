using UnityEngine;
using System.Collections;

public class PlayerTerminal : MonoBehaviour {

	//So I can start passing information around
	public PlayersManagerOld playersManager;

	//TODO Stuff
	public GameObject launchMenu;

	private PlayerFocus playerFocus;
	private ShipControl currentShipControl;
	
	//DEBUG
	public TNObject StartingFocus;
	public Camera playerCamera;

	// Use this for initialization
	void Start () {
		
		//DEBUG FEATURE
		playersManager.ApplyFocusChange( StartingFocus.uid, "Observation", TNManager.player );

		Screen.lockCursor = true;
	}

	// Resolving key presses
	void Update () {
		//DEBUG FEATURE
		if( Input.GetKeyUp( KeyCode.LeftControl ) ){
			Screen.lockCursor = !Screen.lockCursor;
		}

		//HACK: HO! Not using Unity's input manager! Not cool!
		if( Input.GetKeyUp( KeyCode.L ) ){
			if (playerFocus.Focus.GetComponent<Stats> ().isCarrier ) {

				launchMenu.SetActive( !launchMenu.activeSelf );
				Screen.lockCursor = !launchMenu.activeSelf;
			}
		}
	}

	public void ChangeFocus( PlayerFocus focus ){

		//TODO Disable stuff on current focus
		if (playerFocus != null && playerFocus != focus) {
			playerFocus.Focus.GetComponent<ShipControl>().Reset( TNManager.player );
			//Reset the controls? EDIT: OnDisable will reset stuff
		}

		//Change focus
		playerFocus = focus;

		#region Camera Controls
		//Enable stuff on new focus
		//TODO Wait for camera to be in place before doing this stuff
//		ShipControl control = playerFocus.Role.GetComponent<ShipControl> ();

//		control.TransferControl (playerCamera);
//		control.enabled = true;
		#endregion

		//Update the Launch Menu if the ship is a carrier
		if (focus.Focus.GetComponent<Stats> ().isCarrier) {
			launchMenu.GetComponent<LaunchMenu> ().UpdateListOfShips (focus.Focus.GetComponent<CarrierManagement> ().GetListOfShips);
		} else {
			launchMenu.GetComponent<LaunchMenu> ().UpdateListOfShips( null );
		}

		currentShipControl = focus.Focus.GetComponent<ShipControl>();

		currentShipControl.AssignDefault(TNManager.player);
	}
}