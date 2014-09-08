using UnityEngine;
using System.Collections;

public class UnitTest : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.A)) {
			transform.Translate( new Vector3( 0, 0, 1 ) );
		}
	}
}
