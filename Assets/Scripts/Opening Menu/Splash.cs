using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	//TODO

    public bool takeInput = false;

    public void StartSplash() {
        //TODO Start splash
        takeInput = true;
    }

    void Update() {
        if( takeInput && Input.anyKeyDown ) {
            Debug.Log( "Any Key Pressed" );
            takeInput = false;
            //Start Splash Fadeout
            StartCoroutine( SplashFadeOut() );
        }
    }

    private IEnumerator SplashFadeOut() {
        //TODO
        //Play various animations
        yield return null;

        gameObject.SetActive( false );
        TitleMenuManager.instance.SplashComplete();
    }

}
