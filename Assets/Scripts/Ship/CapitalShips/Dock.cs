using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

	public Carrier carrier;

	void OnTriggerEnter( Collider other ){
		if (other.transform.root.gameObject.GetComponent<Terminal> () != null && other.transform.root.gameObject.activeSelf) {
//			Debug.Log ("Docking ship is, in fact, a Terminal");
//			Debug.Log( other.transform.root.gameObject );
			carrier.ApplyDock(other.transform.root.gameObject);
		}
	}
}
