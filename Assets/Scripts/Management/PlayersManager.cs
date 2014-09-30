using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TNet;
using NetPlayer = TNet.Player;

//TODO  This needs to get bigger at some point, or it needs to be moved somewhere else.
//		Eventually we're going to need a menu that displays player information, and it'll come from here
public struct PlayerFocus{
	public GameObject Focus;
	public string Role;
	
	public PlayerFocus( GameObject focus, string role ){
		Focus = focus;
		Role = role;
	}
}

public class PlayersManager : Singlton<PlayersManager> {

	#region Simulating TNBehaviour
	TNObject mTNO;
	
	public TNObject tno
	{
		get
		{
			if (mTNO == null) mTNO = GetComponent<TNObject>();
			return mTNO;
		}
	}
	#endregion

	//A list of the focuses of all players
	private Dictionary< NetPlayer, PlayerFocus > ListOfPlayerFocus = new Dictionary< NetPlayer, PlayerFocus >();

	//Our constant reference to our Flagship
	public GameObject Flagship;

	//Our players camera
	public CameraControls playerCamControl;
	//DEBUG Our starting object
	public GameObject startingObject;

	void Start(){
		//DEBUG
		startingObject.GetComponent<Ship> ().AssignDefault (TNManager.player);
	}


	void Update(){
		//DEBUG
		if( Input.GetKeyUp( KeyCode.LeftControl ) ){
			Screen.lockCursor = !Screen.lockCursor;
		}
	}

	[RFC(1)]
	public void UpdateFocusChange( NetPlayer player, uint target, string role ){

		if( TNManager.isHosting ){
			tno.Send( 1, Target.Others, player, target, role );
		}

		//Update the player's focus
		ListOfPlayerFocus [player] = new PlayerFocus (TNObject.Find (target).gameObject, role);

//		Debug.Log (ListOfPlayerFocus [player].Role);
	}

	//TODO Resolve stuff if someone dies in a non-flagship

	//TODO Resolve the game if the Flagship is destroyed
}
