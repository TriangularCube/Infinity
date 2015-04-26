using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	//TODO

    bool takeInput = false;

    public void StartSplash() {
        //TODO Start splash
        Invoke( "TakeInput", 3 );
    }

    [SerializeField]
    private UITweener alphaTween;

    void TakeInput() {
        alphaTween.enabled = true;
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
        //Reset Splash Animations
        TitleMenuManager.instance.SplashComplete();
    }

}
