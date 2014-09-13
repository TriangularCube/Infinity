using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Carrier : Ship {

	public List<GameObject> dockedTerminals{ get; set; }

	void Awake(){
		dockedTerminals = new List<GameObject> ();

		//Iterate through each of the children of the "Dock", and add it to the list of "Docked Ships"
		foreach (Transform child in transform.FindChild( "Dock" )) {
			dockedTerminals.Add( child.gameObject );
		}
	}

	public abstract void AssignDefault ( TNet.Player pilot );
}
