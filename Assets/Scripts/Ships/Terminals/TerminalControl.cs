using UnityEngine;
using System.Collections;

public abstract class TerminalControl : ShipControl {

	[SerializeField]
	protected Interceptor shipCore;

	public override void CleanUp ()
	{
		if (!enabled) return;
		base.CleanUp ();
	}

}
