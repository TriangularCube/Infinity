using UnityEngine;
using System.Collections;

public abstract class ShipControl : MonoBehaviour {

	public Transform CameraPoint;
	public GameObject playerCamera;

	public abstract void TransferControl( GameObject cam );
	
}
