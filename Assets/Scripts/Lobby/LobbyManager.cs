using UnityEngine;
using System.Collections.Generic;

using TNet;
using Netplayer = TNet.Player;

public class LobbyManager : TNSingleton<LobbyManager> {

    protected override void Awake() {
        base.Awake();
        //DEBUG FUNCTIONALITY
        PlayerSettings.currentProfile = PlayerProfile.newDefaultPlayerProfile();
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

    public void SelectMission( string missionName, Mission mission ) {
        Debug.Log( "Mission " + missionName + " selected" );
        if( !TNManager.isHosting ) return; //Do nothing if we're not hosting
        //Do Stuff

        selectedMission = mission;
    }

    //TODO Ready notificaiton, Start Mission

}
