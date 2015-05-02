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

	//TODO Saved game load

	//TODO Deal with players connecting and disconnecting.
    [SerializeField, Group( "Players" )]
    private GameObject playerPrefab;

    [SerializeField, Group( "Players" )]
    private GameObject playerTable;

    public void OnNetworkPlayerJoin( Netplayer newPlayer ) {
        NGUITools.AddChild( playerTable, playerPrefab );
        playerTable.GetComponent<UITable>().repositionNow = true;
    }

	//TODO Mission Select

	//TODO Ready notificaiton, Start Mission

	//TODO After Mission end, do stuff
}
