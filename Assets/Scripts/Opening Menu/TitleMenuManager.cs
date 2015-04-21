using UnityEngine;
using System.Collections;

public class TitleMenuManager : Singleton<TitleMenuManager> {

    [SerializeField]
    private Splash splash;

    private void Start() {
        splash.StartSplash();
    }

    [SerializeField]
    private ProfileScreen profileScreen;

    public void SplashComplete() {

        if( PlayerSettings.currentProfile == null ) {
            profileScreen.StartProfileSelect();
        } else {
            //Go to main menu
        }

    }

}
