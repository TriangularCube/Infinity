using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TNet;
using NetPlayer = TNet.Player;

[RequireComponent( typeof( ObservationStation ) )]
[RequireComponent( typeof( FlagshipNavigation ) )]

public class Flagship : Carrier {

	public override void AssignDefault (NetPlayer pilot)
	{
		base.AssignDefault (pilot);
		HUD.instance.DockedIntoFlagship ();
	}

	protected override void LaunchTerminal (NetPlayer pilot, uint terminalID)
	{
		base.LaunchTerminal (pilot, terminalID);
		HUD.instance.LaunchedFromFlagship ();
	}
	
	public void OnDestroy(){
		//TODO Figure out what to do when the thing we're on is destroyed.
	}
}
