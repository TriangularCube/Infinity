using UnityEngine;
using System.Collections;

public class CheckStuff : MonoBehaviour {

    void OnNetworkJoinChannel( bool sucess, string msg ) {
        if( sucess ) {
            Debug.Log( "Successfully joined channel " + TNManager.channelID );
        } else {
            Debug.LogError( msg );
        }
    }

    public void Check() {
        TNManager.Disconnect();
        Application.LoadLevel( "Opening" );
    }

}
