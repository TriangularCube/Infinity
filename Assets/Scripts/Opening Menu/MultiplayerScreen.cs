using UnityEngine;
using System.Collections;

public class MultiplayerScreen : MonoBehaviour {

    public void StartMultiplayerScreen() {
        gameObject.SetActive( true );
    }

    public void Cancel() {
        //TODO Animation Stuff
        gameObject.SetActive( false );
        TitleMenuManager.instance.StartMainMenu();
    }

    public void Host() {
        if( TNServerInstance.Start( 4400 ) ) TNManager.Connect( "127.0.0.1", 4400 );
    }

    public void Connect() {
        //TODO Animation Stuff
        gameObject.SetActive( false );
        TitleMenuManager.instance.StartConnect();
    }

}
