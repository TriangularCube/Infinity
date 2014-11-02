using UnityEngine;
using System.Collections;

public static class PlayerSettings{

	static PlayerSettings(){
		profile = ProfileLoader.loadDefault();
//		Debug.Log ("RUN!");
	}

	public static PlayerProfile profile { get; set; }

	//Network statuses and stuff
}
