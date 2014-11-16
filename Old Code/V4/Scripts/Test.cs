using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
		if (Input.GetButtonDown ("Dock")) {

			Debug.Log( "Load" );
			Application.LoadLevel( "Test Scene 1" );

		}

	}
}
