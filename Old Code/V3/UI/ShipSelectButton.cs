using UnityEngine;
using System.Collections;

public class ShipSelectButton : MonoBehaviour {

	public GameObject terminal { get; set; }

	void OnClick(){
		LaunchMenuManager.instance.carrier.ApplyForLaunch( TNManager.player, terminal );
	}
}
