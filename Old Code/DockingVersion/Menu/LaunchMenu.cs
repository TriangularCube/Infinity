using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaunchMenu : MonoBehaviour {

	public PlayersManagerOld manager;

	//DEBUG
	public UILabel noOfShips;

	private List<GameObject> ListOfShips = null;


	public void UpdateListOfShips( List<GameObject> ships ){
		ListOfShips = ships;

		//DEBUG
		noOfShips.text = "" + ListOfShips.Count;
	}
	
	public void LaunchTerminal(){
//		manager.ApplyForLaunch( ListOfShips[0] );

		Debug.Log ("Launch!");
	}
}
