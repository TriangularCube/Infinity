using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

	[SerializeField]
	private Carrier carrier;

	void Awake(){
		Debug.Log ("Awake on Dock");
		if (!TNManager.isHosting) {
			gameObject.SetActive( false );
		}
	}

	void OnTriggerEnter( Collider other ){

		//other.transform.root.gameObject.GetComponent<Terminal>().ReadyForDocking( carrier );
		EventManager.instance.QueueEvent ( new EnteringDockingRange( other.transform.root.GetComponent<Terminal>(), carrier ) );

	}

	void OnTriggerExit( Collider other ){

		EventManager.instance.QueueEvent( new LeavingDockingRange( other.transform.root.GetComponent<Terminal>() ) );

	}
}
