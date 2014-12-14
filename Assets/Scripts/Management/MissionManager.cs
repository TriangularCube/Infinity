using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour {

#pragma warning disable 0649
	[SerializeField]
	Ship startingShip;
#pragma warning restore 0649

	// Use this for initialization
	void Start () {

		if( startingShip is Carrier )
			((Carrier)startingShip).AssignDefault( TNManager.player );
        else if( startingShip is Terminal ) {
			((Terminal)startingShip).AssignPilot( TNManager.player, "" );
            EventManager.instance.QueueEvent( new AllyLaunched( (Terminal)startingShip, null ) );
        }
	}

	void Update(){

		//DEBUG
		if( Input.GetKeyDown( KeyCode.LeftControl ) ){
			Screen.lockCursor = !Screen.lockCursor;
		}

	}

}
