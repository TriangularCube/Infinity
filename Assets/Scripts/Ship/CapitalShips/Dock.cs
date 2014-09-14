using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

	public Carrier carrier;

	void OnTriggerEnter( Collider other ){
		if (other.transform.root.gameObject.GetComponent<Terminal> () != null) {

			Debug.Log ("Docking ship is, in fact, a Terminal");
			carrier.Dock( other.transform.root.gameObject);
		}
	}
}
