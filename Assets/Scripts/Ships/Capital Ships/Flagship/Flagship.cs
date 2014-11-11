using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TNet;
using Netplayer = TNet.Player;

[RequireComponent( typeof( ObservationStation ) )]

public class Flagship : Carrier {

	public override bool ContainsPlayer (Netplayer check)
	{
		throw new System.NotImplementedException ();
	}
	
	public void OnDestroy(){
		//TODO Figure out what to do when the thing we're on is destroyed.
	}

	protected override IEnumerator Sync ()
	{
		throw new System.NotImplementedException ();
	}
}
