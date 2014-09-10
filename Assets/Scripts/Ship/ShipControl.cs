using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NetPlayer = TNet.Player;

public abstract class ShipControl : MonoBehaviour {

	public Transform cameraPoint;
	public GameObject playerCamera;
	

	public abstract void TransferControl( GameObject cam );

}
