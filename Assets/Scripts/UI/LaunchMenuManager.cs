using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LaunchMenuManager : MonoBehaviour {

	//Editor references
	public GameObject shipSelectButton;
	public UIGrid grid;
	public GameObject NoShipsDocked;

	//Our list of docked Terminals
	private Dictionary< GameObject, GameObject > dockedTerminals;

	void Awake(){
		dockedTerminals = new Dictionary<GameObject, GameObject>(1);
	}

	//When we change ships
	public void Reset(){
		dockedTerminals.Clear ();
		NoShipsDocked.SetActive (true);
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

	}

	public void TerminalLaunched( GameObject terminal ){

		//HACK This probably won't happen...?

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
