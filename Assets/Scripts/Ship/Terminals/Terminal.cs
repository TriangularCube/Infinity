using UnityEngine;
using System.Collections;
using TNet;

public abstract class Terminal : Ship {

	public ShipControl control;
	public TNet.Player pilot{ get; set; }

	public override bool ContainsPlayer (TNet.Player check)
	{
		return pilot == check;
	}

	//TODO
	[RFC]
	public override void AssignDefault (TNet.Player pilot)
	{
		if (TNManager.isHosting) {
			tno.Send( "AssignDefault", TNet.Target.Others, pilot );

			PlayersManager.Instance.UpdateFocusChange ( pilot, tno.uid, "Pilot" );
		}

		if (pilot == TNManager.player) {
			PlayersManager.Instance.playerCamControl.SetTarget( transform, control );
		}

		this.pilot = pilot;
	}

	public void CleanUp(){
		pilot = null;

		gameObject.SetActive( false );
	}

}
