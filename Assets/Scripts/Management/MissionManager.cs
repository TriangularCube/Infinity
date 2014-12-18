using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour {

#pragma warning disable 0649
	[SerializeField]
	Ship startingShip;
#pragma warning restore 0649

    //DEBUG Initialization
	void Start () {

		if( startingShip is Carrier )
			((Carrier)startingShip).AssignDefault( TNManager.player );
        else if( startingShip is Terminal ) {
            Terminal startShip;
            startShip = (Terminal)startingShip;

            startShip.gameObject.SetActive( false );
			startShip.OnLaunch( TNManager.player, "" );
            EventManager.instance.QueueEvent( new AllyLaunched( startShip, null ) );
            Screen.lockCursor = true;
        }
	}

	void Update(){

		//DEBUG
		if( Input.GetKeyDown( KeyCode.LeftControl ) ){
			Screen.lockCursor = !Screen.lockCursor;
		}

	}

}
