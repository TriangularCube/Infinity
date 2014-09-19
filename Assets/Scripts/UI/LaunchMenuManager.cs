using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LaunchMenuManager : MonoBehaviour {

	//Here is a private reference only this class can access
	private static LaunchMenuManager _instance;
	
	//This is the public reference that other classes will use
	public static LaunchMenuManager instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<LaunchMenuManager>();
			return _instance;
		}
	}

	//Editor references
	public GameObject shipSelectButton;
	public GameObject LaunchMenu;
	public UIGrid grid;
	public GameObject NoShipsDocked;

	//Our list of docked Terminals
	//That's { Button, Terminal }
	private Dictionary< GameObject, GameObject > dockedTerminals;

	//A reference to the carrier we're in
	public Carrier carrier{ get; private set; }

	void Awake(){
		dockedTerminals = new Dictionary<GameObject, GameObject>(1);
	}

	//Called when the state of the carried ships has changed
	public void PopulateList(){

		dockedTerminals.Clear ();

		if (carrier.dockedTerminals.Count > 0) {
		//Fill the grid with a button for each Terminal in the carrier
			foreach (GameObject terminal in carrier.dockedTerminals) {
				NoShipsDocked.SetActive( false );
				GameObject button = NGUITools.AddChild (grid.gameObject, shipSelectButton);
				button.GetComponent<ShipSelectButton>().terminal = terminal;
				dockedTerminals.Add ( button, terminal);
			}
			grid.Reposition ();
		} else {
			NoShipsDocked.SetActive( true );
		}
	}

	//Called if we're docked into a carrier
	public void Docked( Carrier dockedInto ){
		//Set our carrier reference
		carrier = dockedInto;
	}

	//Called if we're launched in a terminal from a carrier
	public void Launched(){
		//Reset our references
		carrier = null;
		dockedTerminals.Clear ();

		//Disable the menu
		LaunchMenu.SetActive (false);
	}

	void Update(){
		//Call up the menu and stuff
		if (Input.GetKeyUp (KeyCode.L)) {
			if( carrier ){
				LaunchMenu.SetActive( !LaunchMenu.activeSelf );
				Screen.lockCursor = !LaunchMenu.activeSelf;
			}
		}
	}

	//HACK
//	void Update(){
		//If the menu is active and the cursor is locked (which shouldn't ever happen), turn the menu off
//		if (gameObject.activeInHierarchy && Screen.lockCursor) {
//			gameObject.SetActive( false );
//		}
//	}
}
