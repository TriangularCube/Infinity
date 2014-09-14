using UnityEngine;
using System.Collections;

public abstract class Terminal : Ship {

	public ShipControl control;
	public TNet.Player pilot{ get; set; }

	public override bool ContainsPlayer (TNet.Player check)
	{
		return pilot == check;
	}

	//TODO
	public override void AssignDefault (TNet.Player pilot)
	{
		if (TNManager.isHosting) {
			tno.Send( "AssignDefault", TNet.Target.Others, pilot );

			PlayersManager.Instance.ApplyFocusChange ( pilot, tno.uid, "Pilot" );
		}

		if (pilot == TNManager.player) {
			PlayersManager.Instance.playerCamControl.SetTarget( transform, control );
		}

		this.pilot = pilot;
	}

}
