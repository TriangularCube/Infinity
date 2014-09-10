using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	private ShipControl target;
	private Transform targetCameraPoint{ get{ return target.cameraPoint; } }

	public float camTransferSpeed = 600f;

	public void SetTarget( Transform parent, ShipControl controller ){
		transform.parent = parent;

		target = controller;

		enabled = true;
	}

	void Update(){
		if (target) {
			transform.position = Vector3.MoveTowards (transform.position, targetCameraPoint.position, camTransferSpeed * Time.deltaTime);

			if (Vector3.Distance (transform.position, targetCameraPoint.position) < 1f) {
				target.TransferControl (gameObject);

				target = null;
				enabled = false;
			}
		}
	}
}
