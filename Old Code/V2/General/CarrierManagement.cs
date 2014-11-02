using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarrierManagement : MonoBehaviour {

	private List<GameObject> DockedShips;
	public List<GameObject> GetListOfShips{ get{ return DockedShips; } }

	void Awake(){

		DockedShips = new List<GameObject>();

		//Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
		foreach (Transform child in transform.FindChild( "Dock" )) {
			DockedShips.Add( child.gameObject );
		}
	}

	public void Dock( GameObject terminal ){

	}

	public void Launch( GameObject terminal ){

	}
}
