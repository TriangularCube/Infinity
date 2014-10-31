using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

	public Carrier carrier;

		}
	}

	void OnTriggerEnter( Collider other ){

		other.transform.root.gameObject.GetComponent<Terminal>().ReadyForDocking( carrier );

	}

	void OnTriggerExit( Collider other ){

		other.transform.root.gameObject.GetComponent<Terminal>().LeavingDockingArea ();

	}
}
