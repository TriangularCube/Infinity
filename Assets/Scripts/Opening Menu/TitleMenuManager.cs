using UnityEngine;
using System.Collections;

public class TitleMenuManager : Singleton<TitleMenuManager> {

    [SerializeField]
    private Splash splash;

    private void Start() {
        splash.StartSplash();

        //DEBUG Functionality
        Debug.Log( TNServerInstance.isActive );
    }

    [SerializeField]
    private ProfileScreen profileScreen;

    public void SplashComplete() {

        if( PlayerSettings.currentProfile == null ) {
            StartProfilePage();
        } else {
            StartMainMenu();
        }

    }

    public void StartProfilePage() {
        profileScreen.StartProfileSelect();
    }

    [SerializeField]
    private MainMenu mainMenu;

    public void StartMainMenu() {
        mainMenu.EngageMainMenu();
    }

    [SerializeField]
    private MultiplayerScreen multiplayer;

    public void StartMultiplayer() {
        multiplayer.StartMultiplayerScreen();
    }

    [SerializeField]
    private Connect connect;

    public void StartConnect() {
        connect.StartConnect();
    }


    //On Connect to something
    void OnNetworkConnect( bool success, string msg ) {
        if( success ) {
            TNManager.JoinChannel( 1, "Lobby", false, 6, "" );
        } else {
            Debug.LogError( msg );
        }
    }



}
