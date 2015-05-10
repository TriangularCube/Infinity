using UnityEngine;
using System.Collections.Generic;

using TNet;
using Netplayer = TNet.Player;

public class LobbyManager : TNSingleton<LobbyManager> {

    [SerializeField]
    private UILabel hostIPLabel;

    protected override void Awake() {
        base.Awake();


        if( TNManager.isHosting ) {
            hostIPLabel.gameObject.SetActive( true );
            hostIPLabel.text = TNet.Tools.localAddress.ToString();
        }
    }

    public void Exit() {
        //Play Animations and stuff
        TNManager.Disconnect();
        //Debug.Log( TNServerInstance.isActive );
        if( TNServerInstance.isActive ) TNServerInstance.Stop();
        Destroy( SessionManager.instance.gameObject );
        Application.LoadLevel( 2 );
    }

    //TODO Deal with players connecting and disconnecting.
    [SerializeField, Group( "Players" )]
    private GameObject playerPrefab;

    [SerializeField, Group( "Players" )]
    private GameObject playerTable;

    private Dictionary<Netplayer, GameObject> playerList = new Dictionary<Netplayer, GameObject>();

    public void OnNetworkPlayerJoin( Netplayer newPlayer ) {
        playerList.Add( newPlayer, NGUITools.AddChild( playerTable, playerPrefab ) );
        playerTable.GetComponent<UITable>().repositionNow = true;
    }

    //TODO Mission Select
    private Mission selectedMission = null;

    public void SelectMission( Mission mission ) {
        Debug.Log( "Mission " + mission.name + " selected" );
        if( !TNManager.isHosting ) return; //Do nothing if we're not hosting
        //Do Stuff

        selectedMission = mission;
    }

    //TODO Ready notificaiton, Start Mission

    [SerializeField]
    private UISprite startButton;

    public void StartMission() {
        //All sorts of checks and stuff should go here
        TNManager.LoadLevel( "Test Scene 1" );
    }


}
