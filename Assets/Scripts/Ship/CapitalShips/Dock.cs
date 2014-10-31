using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

	[SerializeField]
	private Carrier carrier;

	void Awake(){
		Debug.Log ("Awake on Dock");
		if (TNManager.isHosting) {
			gameObject.SetActive( false );
		}
	}

	void OnTriggerEnter( Collider other ){

		other.transform.root.gameObject.GetComponent<Terminal>().ReadyForDocking( carrier );

	}

	void OnTriggerExit( Collider other ){

		other.transform.root.gameObject.GetComponent<Terminal>().LeavingDockingArea ();

	}
}
