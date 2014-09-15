using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LaunchMenuManager : MonoBehaviour {

	//Editor references
	public GameObject shipSelectButton;
	public UIGrid grid;

	//Our list of docked Terminals
	private List<GameObject> dockedTerminals;

	public void Test(){
		NGUITools.AddChild (grid.gameObject, shipSelectButton);
		grid.Reposition ();
	}

	//HACK
//	void Update(){
		//If the menu is active and the cursor is locked (which shouldn't ever happen), turn the menu off
//		if (gameObject.activeInHierarchy && Screen.lockCursor) {
//			gameObject.SetActive( false );
//		}
//	}
}
