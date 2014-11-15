using UnityEngine;
using System.Collections;

public enum InterceptorLookMode{ Locked, Free };

public class Profile{
	
	InterceptorLookMode interceptorLookMode = InterceptorLookMode.Free;
	
	public InterceptorLookMode GetInterceptorLookMode(){
		return interceptorLookMode;
	}
	
}

public class PlayerSettings {

	#region Profile Settings
	public static InterceptorLookMode GetInterceptorLookMode(){
		return currentProfile.GetInterceptorLookMode ();
	}
	#endregion

	static Profile currentProfile;

	static PlayerSettings(){
		//TODO Load a list of player profiles
		currentProfile = new Profile ();
	}

	public static void SelectProfile(){

	}
}
