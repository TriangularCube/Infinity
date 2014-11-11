using UnityEngine;
using System.Collections;

public class CameraControls : Singleton<CameraControls> {

	private Transform thisTransform;
	public new Transform transform{
		get{
			return thisTransform;
		}
	}

	protected override void Awake ()
	{
		base.Awake ();
		thisTransform = base.transform;
	}

	private ShipControl target;
	private Transform targetCameraPoint{ get{ return target.cameraPoint; } }

	public float camTransferSpeed = 600f;
	public float camTransferRotation = 40f;



	public void SetTarget( Transform parent, ShipControl controller ){
		transform.parent = parent;

		target = controller;

		enabled = true;
	}

	void Update(){
		//If this script is enabled, but we don't have a target, turn ourselves off.
		if (!target) {
			enabled = false;
			return;
		}

		//We can actually do a fancy Lerp here
		transform.position = Vector3.MoveTowards (transform.position, targetCameraPoint.position, camTransferSpeed * Time.deltaTime);
		transform.rotation = Quaternion.RotateTowards( transform.rotation, targetCameraPoint.rotation, camTransferRotation * Time.deltaTime);

		if (Vector3.Distance (transform.position, targetCameraPoint.position) < 1f) {
			target.TransferControl (this);

			target = null;
			enabled = false;
		}
	}
	
}