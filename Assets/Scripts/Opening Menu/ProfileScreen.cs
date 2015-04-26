using UnityEngine;
using System.Collections;

public class ProfileScreen : MonoBehaviour {

    public void StartProfileSelect() {
        gameObject.SetActive( true );
    }

    public void AcceptProfile() {
        //Play various animations
        
        //DEBUG FUNCTIONALITY AT THE MOMENT
        gameObject.SetActive( false );
        //Reset Profile Page
        TitleMenuManager.instance.StartMainMenu();
    }

}
