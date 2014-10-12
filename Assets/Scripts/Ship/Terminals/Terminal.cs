using UnityEngine;
using System.Collections;
using TNet;

public abstract class Terminal : Ship {

	public ShipControl control;
	public Player pilot{ get; set; }

	public override bool ContainsPlayer (Player check)
	{
		return pilot == check;
	}

	//TODO
	[RFC]
	public override void AssignDefault (Player pilot)
	{
		if (TNManager.isHosting) {
			//What should be a redundent check
			if( !gameObject.activeSelf ){
				Debug.LogException( new UnityException( "Seating a pilot onto an inactive Terminal" ) );
			}

			tno.Send( "AssignDefault", Target.Others, pilot );

			PlayersManager.instance.UpdateFocusChange ( pilot, tno.uid, "Pilot" );
		}

		if (pilot == TNManager.player) {
			CameraControls.instance.SetTarget( transform, control );
		}

		this.pilot = pilot;
	}

	public void CleanUp(){
		pilot = null;

		rigidbody.velocity = Vector3.zero;
		gameObject.SetActive( false );
	}

}
