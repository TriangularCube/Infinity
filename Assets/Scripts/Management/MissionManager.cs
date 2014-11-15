using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour {

	[SerializeField]
	Ship startingShip;

	// Use this for initialization
	void Start () {

		startingShip.AssignDefault( TNManager.player );

	}

	void Update(){

		//DEBUG
		if( Input.GetKeyDown( KeyCode.LeftControl ) ){
			Screen.lockCursor = !Screen.lockCursor;
		}

	}

}
