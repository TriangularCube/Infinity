using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

	public Carrier carrier;

	void OnTriggerEnter( Collider other ){
		if (other.gameObject.GetComponent<Terminal> () != null) {
			carrier.Dock( other.gameObject );
		}
	}
}
