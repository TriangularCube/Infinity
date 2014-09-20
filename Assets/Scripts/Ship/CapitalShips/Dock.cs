using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

	public Carrier carrier;

	void OnTriggerEnter( Collider other ){
		if (other.transform.root.gameObject.GetComponent<Terminal> () != null && other.transform.root.gameObject.activeSelf) {
			carrier.ApplyDock(other.transform.root.gameObject);
		}
	}
}
