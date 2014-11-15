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

	protected Quaternion savedRotation;
	protected Vector3 savedPosition;

	void OnEnable(){
		savedPosition = _cameraPoint.position;
		savedRotation = _cameraPoint.rotation;
	}

	public abstract void UpdateCamera();

}
