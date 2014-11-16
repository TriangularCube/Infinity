using UnityEngine;
using System.Collections;

public abstract class CapitalShipStation : ShipControl {

	protected Vector3 defaultLocalPosition;
	protected Quaternion defaultLocalRotation;
	
	void Awake(){
		defaultLocalPosition = _cameraPoint.localPosition;
		defaultLocalRotation = _cameraPoint.localRotation;
	}

	public override void CleanUp ()
	{

		_cameraPoint.localPosition = defaultLocalPosition;
		_cameraPoint.localRotation = defaultLocalRotation;

		base.CleanUp ();

	}

}
