using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour {

#pragma warning disable 0649
	[SerializeField]
	Ship startingShip;
#pragma warning restore 0649

    //DEBUG Initialization
	void Start () {

		if( startingShip is Carrier ){
            Carrier startShip = (Carrier)startingShip;
            
            startShip.AssignDefault( TNManager.player );
            EventManager.instance.QueueEvent( new AllyDocked( TNManager.player, startShip ) );

        } else if( startingShip is Terminal ) {
            Terminal startShip = (Terminal)startingShip;

            startShip.gameObject.SetActive( false ); //Not turning the object will cause OnEnable to fire before we've assigned a pilot, thereby not turning on the Sync coroutine. This ends up not updating any variables.
			startShip.OnLaunch( TNManager.player, "" );
            EventManager.instance.QueueEvent( new AllyLaunched( startShip, null ) );
            //Screen.lockCursor = true;
        }
	}

	void Update(){

		//DEBUG
		if( Input.GetKeyDown( KeyCode.LeftControl ) ){
            if( Cursor.lockState == CursorLockMode.Locked ) {
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
		}

	}

}
