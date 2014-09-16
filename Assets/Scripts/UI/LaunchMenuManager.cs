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

	public void PopulateList( Carrier carrier ){

		//What should be a redundent check
		if (dockedTerminals.Count > 0)
						throw new UnityException ("Launch Menu's list is being populated while it's still filled");

		if (carrier.dockedTerminals.Count > 0) {
		//Fill the grid with a button for each Terminal in the carrier
			foreach (GameObject terminal in carrier.dockedTerminals) {
				dockedTerminals.Add (NGUITools.AddChild (grid.gameObject, shipSelectButton), terminal);
			}
			grid.Reposition ();
		} else {
			NoShipsDocked.SetActive( true );
		}
	}

	public void TerminalDocked( GameObject terminal ){

		NoShipsDocked.SetActive (false);

		dockedTerminals.Add (NGUITools.AddChild (grid.gameObject, shipSelectButton), terminal);
		grid.Reposition ();

	//Called if we're docked into a carrier
	public void Docked( Carrier dockedInto ){
		//Set our carrier reference
		carrier = dockedInto;
		//Populate our menu
		PopulateList ();
	}

	public void TerminalLaunched( GameObject terminal ){
	//Called if we're launched in a terminal from a carrier
	public void Launched(){
		//Reset our references
		carrier = null;
		dockedTerminals.Clear ();

		//HACK This probably won't happen...?
		//Disable the menu
		LaunchMenu.SetActive (false);
	}

		//If there's no values in the list of terminals
		if (dockedTerminals == null || dockedTerminals.Count == 0) {
			//Turn the label that says "No Terminals docked"
			NoShipsDocked.SetActive (true);
		} else {
			NoShipsDocked.SetActive (false);
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
