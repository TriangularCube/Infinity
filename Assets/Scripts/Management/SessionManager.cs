using UnityEngine;
using System.Collections;

using TNet;
using Netplayer = TNet.Player;

[RequireComponent ( typeof (TNObject) )]
/* This is where we are going deal with the session management.
 */
public class SessionManager : TNSingleton<SessionManager> {

	[SerializeField]
	private int _maxNetworkUpdatesPerSecond = 4;
	public int maxNetworkUpdatesPerSecond{ get{ return _maxNetworkUpdatesPerSecond; } }

	protected override void Awake(){

		//If we're not the only instance of this, that means we're didn't start on this scene. Destroy this instance. Purely for testing purposes
		if (instance != null) {

			Debug.Log( "Destroying this instance of Session Manager" );
			Destroy( this );
			return;

		}
		base.Awake ();
		DontDestroyOnLoad( this );

	}

    //Mission Selection, for persistence

	//TODO Saved game load



	//TODO After Mission end, do stuff
}
