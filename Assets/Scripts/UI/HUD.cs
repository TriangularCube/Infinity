using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : Singlton<HUD> {

	[SerializeField]
	GameObject panel;

	[SerializeField]
	GameObject targetingPrefab;

	//That's {Target Ship, UI Element}
	private Dictionary<GameObject, GameObject> targetDisplayList = new Dictionary<GameObject, GameObject>();

	public void AddTarget( GameObject newTarget ){
		//TODO Instantiate something here and add it
		targetDisplayList [newTarget] = null;
	}

	// Update is called once per frame
	void Update () {
		Paint ();
	}

	void Paint(){

	}
}