using UnityEngine;
using System.Collections;

public class Connect : MonoBehaviour {

    public void StartConnect() {
        //TODO Play animations and stuff
        gameObject.SetActive( true );
    }

    [SerializeField]
    private UILabel ipAddress;

    public void ConnectToHost() {
        Debug.Log( "Connecting to: " + ipAddress.text );
        //TODO Validate IP Address
        TNManager.Connect( ipAddress.text, 4400 );
    }


    public void Cancel() {
        //TODO Play animation
        gameObject.SetActive( false );
        TitleMenuManager.instance.StartMultiplayer();
    }

}
