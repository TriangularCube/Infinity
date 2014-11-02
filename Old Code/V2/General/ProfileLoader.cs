using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ProfileLoader
{
	public static PlayerProfile loadDefault(){
		PlayerProfile profile = new PlayerProfile();

		//Generate Name
		profile.playerName = "New001";

		//Generate default keys
		Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
		keys.Add ("up", KeyCode.Z);
		keys.Add ("down", KeyCode.C);
		keys.Add ("forward", KeyCode.W);
		keys.Add ("backward", KeyCode.S);
		keys.Add ("left", KeyCode.A);
		keys.Add ("right", KeyCode.D);
		//TODO Attitude controls (Pitch, Yaw, and Roll)
		keys.Add ("break", KeyCode.Space);

		profile.keycodes = keys;
		
		//Generate default mouse sensitivity
		profile.mouseSensitivityX = 1f;
		profile.mouseSensitivityY = 1f;

		//Toss back the profile
		return profile;
	}
}

