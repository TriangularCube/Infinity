using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NetPlayer = TNet.Player;

public abstract class ShipControl : MonoBehaviour {

	public Transform cameraPoint;
	public GameObject playerCamera;
	

	public abstract void TransferControl( GameObject cam );

	//Do this to clean up the object in preparation for deactivation or destruction
	public void CleanUp(){
		Debug.Log ("Yo!");

		if( playerCamera != null ){
			playerCamera.transform.parent = null;
			
			playerCamera = null;
		}
	}

}
