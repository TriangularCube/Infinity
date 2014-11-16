using UnityEngine;
using System.Collections;

public abstract class TerminalControl : ShipControl {

	public override void CleanUp ()
	{
		if (!enabled) return;
		base.CleanUp ();
	}

	//protected Quaternion savedRotation;
	protected Vector3 initialPosition;
	protected Quaternion lookRotation;

	void Start(){
		initialPosition = _cameraPoint.localPosition;
		lookRotation = _cameraPoint.rotation;
	}

}
