using UnityEngine;
using System.Collections;

public abstract class TerminalControl : ShipControl {

	public override void CleanUp ()
	{
		if (!enabled) return;
		base.CleanUp ();
	}


	}

}
