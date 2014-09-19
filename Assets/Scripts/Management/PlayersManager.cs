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

public class PlayersManager : TNBehaviour {

	private static PlayersManager _instance;

	public static PlayersManager instance{
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<PlayersManager> ();
			}

			return _instance;
		}
	}

	//A list of the focuses of all players
	private Dictionary< NetPlayer, PlayerFocus > ListOfPlayerFocus = new Dictionary< NetPlayer, PlayerFocus >();

	//Our constant reference to our Flagship
	public GameObject Flagship;

	//Our players camera
	public CameraControls playerCamControl;

	//DEBUG Our starting object
	public GameObject startingObject;

	void Awake(){
		if (_instance)
			throw new UnityException( "OK, this is the second instance of PlayerManager. Something is seriously wrong." );
		

		//Register ourselves with the instance
		_instance = this;
	}

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
