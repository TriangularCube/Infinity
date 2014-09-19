using UnityEngine;
using System.Collections;

public class ShipSelectButton : MonoBehaviour {

	public GameObject terminal { get; set; }

	void OnClick(){
		Debug.Log ("Button pressed for launching terminal!");
		Debug.Log (terminal);
		LaunchMenuManager.instance.carrier.ApplyForLaunch( TNManager.player, terminal );
	}
}
