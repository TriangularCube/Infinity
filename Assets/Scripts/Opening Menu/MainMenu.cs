using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public void EngageMainMenu() {
        gameObject.SetActive( true );
    }

    public void SwitchProfile() {
        //TODO Animation stuff
        gameObject.SetActive( false );
        TitleMenuManager.instance.StartProfilePage();
    }

    public void StartMultiplayer() {
        //TODO Animation Stuff
        gameObject.SetActive( false );
        TitleMenuManager.instance.StartMultiplayer();
    }

}
