using UnityEngine;
using System.Collections;

public abstract class TerminalControl : ShipControl {

	[SerializeField]
	protected Terminal shipCore;

	public override void CleanUp ()
	{
		if (!enabled) return;
		base.CleanUp ();
	}

	protected virtual void CheckForDocking(){

		if( Input.GetButtonDown( "Dock" ) ){
			shipCore.AttemptRequestDock();
			updateCamera = false;
		}

	}

}
