using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour {

#pragma warning disable 0649
	[SerializeField]
	Ship startingShip;
#pragma warning restore 0649

    //DEBUG Initialization
	void Start () {

		if( startingShip is Flagship ){
            Flagship startShip = (Flagship)startingShip;
            
            startShip.AssignDefault( TNManager.player );
            HUD.instance.PlayerShipDocked();

        } else if( startingShip is Terminal ) {
            Terminal startShip = (Terminal)startingShip;

            startShip.gameObject.SetActive( false ); //Not turning off the object will cause OnEnable to fire before we've assigned a pilot, thereby not turning on the Sync coroutine. This ends up not updating any variables.
			startShip.OnLaunch( TNManager.player, "" );
            
            //DEBUG
            //HUD.instance.AllyShipLaunched( startShip );

            //EventManager.instance.QueueEvent( new AllyLaunched( startShip, null ) );
            //Screen.lockCursor = true;
        }
	}

	void Update(){

		//DEBUG
		if( Input.GetKeyDown( KeyCode.LeftControl ) ){
            HUD.instance.mouseLocked = !HUD.instance.mouseLocked;
		}

	}

}
