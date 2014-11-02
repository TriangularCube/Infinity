using UnityEngine;
using System.Collections;

public class PlayersManagerFocus : MonoBehaviour {

	public Camera playerCamera;
	public GameObject playerFocus;

	//DEBUG
	public GameObject Fighter;

	//TODO Have a list of all the player focuses

	// Use this for initialization
	void Start () {
		//DEBUG FEATURE
		if (Network.peerType == NetworkPeerType.Disconnected) {
			DebugStart (Network.player, Instantiate (Fighter, new Vector3 (0, 0, 0), Quaternion.identity).name);
		} 
		Screen.lockCursor = true;
	}

	//DEBUG FUNCTION
	void DebugStart( NetworkPlayer player, string focusName ){
		playerFocus = GameObject.Find( focusName );
		
		//Do some extra stuff if that player happens to be us
		if (Network.player == player) {
			//Set the new camera target
			playerCamera.GetComponent<CameraFollow>().Target = playerFocus.transform.FindChild ("CameraPoint").transform;
		}

		playerFocus.GetComponent<MechStationAndAttitudeControl>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		//DEBUG FEATURE
		if( Input.GetKeyUp( KeyCode.LeftControl ) ){
			Screen.lockCursor = !Screen.lockCursor;
		}
	}
	
	public void ApplyFocusChange( GameObject focus ){
		if (Network.isClient) {
			networkView.RPC ("ChangedFocus", RPCMode.Server, Network.player, focus.name);
		} else if (Network.isServer) {
			ChangedFocus( Network.player, focus.name );
		}
	}

	[RPC]
	//This should be used for the player doing the changing
	public void ChangedFocus( NetworkPlayer player, string focusName ){
		//TODO Check if the target focus is not already occupied by another player

		//DEBUG
		if ( Network.isServer  || Network.peerType == NetworkPeerType.Disconnected ) {

			networkView.RPC( "ChangedFocus", RPCMode.Others, focusName );

			//NOTE!!! DO THIS STUFF ON THE SERVER ONLY!!!

			//Disable control of current craft, and enable AI scripts on current craft
			//TODO Get player's current focus
			//TODO If it's a Turret, turn off Attitude Control, turn on AI
			//TODO Otherwise (it's a 

			//On the target focus
			//TODO Disable AI scripts
			//TODO Set the controller of the target to this player
			playerFocus.GetComponent<MechStationAndAttitudeControl>().enabled = true;
		}

		//Update the fact that this player has changed his focus
		playerFocus = GameObject.Find( focusName );

		//Do some extra stuff if that player happens to be us
		if (Network.player == player) {
			//Set the new camera target
			playerCamera.GetComponent<CameraFollow>().Target = playerFocus.transform.FindChild ("CameraPoint").transform;
		}

	}

}
