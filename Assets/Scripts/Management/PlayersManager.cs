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

	private static PlayersManager managerInstance;

	public static PlayersManager Instance{
		get{
			if (managerInstance == null) {
				managerInstance = GameObject.FindObjectOfType<PlayersManager> ();
			}

			return managerInstance;
		}
	}

	//A list of the focuses of all players
	private Dictionary< NetPlayer, PlayerFocus > ListOfPlayerFocus = new Dictionary< NetPlayer, PlayerFocus >();

	//Our constant reference to our Flagship
	public GameObject Flagship;

	//Our players camera
	public Camera playerCam;

	void Awake(){
		if (managerInstance) {
			Debug.LogError( "OK, this is the second instance of PlayerManager. Something is seriously wrong." );
		}

		//Register ourselves with the instance
		managerInstance = this;

		if (!Screen.lockCursor) {
			Debug.Log( "Locking Cursor" );

			Screen.lockCursor = true;
		}
	}

	void Start(){
		//DEBUG
		Flagship.GetComponent<Flagship> ().AssignDefault (TNManager.player);
	}

	void Update(){
		if( Input.GetKeyUp( KeyCode.LeftControl ) ){
			Screen.lockCursor = !Screen.lockCursor;
		}
	}

	//TODO May not need this anymore
	//Applyig for docking across the network
	public void ApplyFocusChange( NetPlayer player, uint target, string role ){
		tno.Send( 1, Target.Host, player, target, role );
	}

	[RFC(1)]
	void UpdateFocusChange( NetPlayer player, uint target, string role ){
		
		if( TNManager.isHosting ){
			tno.Send( 1, Target.Others, player, target, role );
		}

		//Update the player's focus
		ListOfPlayerFocus [player] = new PlayerFocus (TNObject.Find (target).gameObject, role);

		Debug.Log (ListOfPlayerFocus [player].Role);
	}

	//TODO Resolve stuff if someone dies in a non-flagship

	//TODO Resolve the game if the Flagship is destroyed
}
