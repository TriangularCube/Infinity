using UnityEngine;
using System.Collections;

public enum InterceptorLookMode{ Locked, Free }
public enum MechLookMode{ Locked, Free }

public class Profile{

	#region Interceptor
	InterceptorLookMode interceptorLookMode = InterceptorLookMode.Free;
	
	internal InterceptorLookMode GetInterceptorLookMode(){
		return interceptorLookMode;
	}
	#endregion

	#region Mech
	MechLookMode mechLookMode = MechLookMode.Free;

	internal MechLookMode GetMechLookMode(){
		return mechLookMode;
	}
	#endregion
	
}

public class PlayerSettings {

	#region Profile Settings
	public static InterceptorLookMode GetInterceptorLookMode(){
		return currentProfile.GetInterceptorLookMode ();
	}

	public static MechLookMode GetMechLookMode(){
		return currentProfile.GetMechLookMode ();
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
