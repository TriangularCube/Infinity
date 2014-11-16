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

	#region Sync
	protected override void SendData ()
	{
		//Debug.Log ("Sync Flagship");
		//TODO Sync information
		
		tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation );
		
	}
	
	[RFC(1)]
	private void RecieveSync( Vector3 position, Quaternion facing ){
		
		//TODO Do stuff with recieved sync data
		
	}
	#endregion
}
