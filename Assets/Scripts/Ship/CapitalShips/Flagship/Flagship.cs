using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TNet;
using NetPlayer = TNet.Player;

[RequireComponent( typeof( ObservationStation ) )]

public class Flagship : Carrier {

	protected override void ResetControls(){
		base.ResetControls ();
	}

	//TODO Launch a terminal

	public void OnDestroy(){
		//TODO Figure out what to do when the thing we're on is destroyed.
	}
}
