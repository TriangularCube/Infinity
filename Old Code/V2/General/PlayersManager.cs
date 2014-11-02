using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

using NetPlayer = TNet.Player;



public class PlayersManager : TNBehaviour {

	public PlayerTerminal terminal;
	
	//TODO Have a list of all the player focuses, and roles on those focuses
	private Dictionary< NetPlayer, PlayerFocus > ListOfPlayerFocus = new Dictionary< NetPlayer, PlayerFocus >();

	public void ApplyFocusChange( uint targetID, string role, NetPlayer player ){

		tno.Send ( 1, Target.Host, targetID, role, player );
		
	}

	[RFC(1)]
	private void ChangeFocus( uint targetID, string role, NetPlayer player ){
		if (TNManager.isHosting) {
			tno.Send ( 1, Target.Others, targetID, role, player );
		}

		//TODO Log that he changed focus
		GameObject target = TNObject.Find( targetID ).gameObject;
		PlayerFocus newFocus = new PlayerFocus (target, role);
		ListOfPlayerFocus.Add (player, newFocus);

		//If this is ourselves, fire off a procedure
		if( player == TNManager.player ){
			//DEBUG
			terminal.ChangeFocus( newFocus );
		}

	}

	/*
	//Invoked when the player docks a strike craft
	//Bit of a joke with the naming scheme. Terminals are a reference to Xenogears.
	[RFC(1)]
	void Dock( int player, GameObject terminal ){

		if( TNManager.isHosting ){
			tno.Send( 1, Target.Others, player, terminal );
		}

		//Switch the player's focus to the carrier
		//TODO Switch the focus of the player on the terminal to the carrier

		//Loading the terminal onto the carrier
		//TODO Add the terminal to the list of ships being carried
		//TODO Disable the terminal

	}

	//Send off the application to launch in the selected terminal
	public void ApplyForLaunch( GameObject terminal ){
		Debug.Log ("Applied for Launch");
		tno.Send (2, Target.Host, TNManager.playerID, terminal.GetComponent<TNObject>().uid);
	}

	//Launch the player chosen strike craft from the carrier
	[RFC(2)]
	void Launch( int player, uint terminal ){

		if( TNManager.isHosting ){
			tno.Send( 2, Target.Others, player, terminal );
		}



		//Spawn the strike craft
		//TODO Enable the strike craft

		//TODO Switch the focus of the player to the terminal

		//TODO Launch the terminal

	}
	*/
}