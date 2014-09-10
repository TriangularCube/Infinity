using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NetPlayer = TNet.Player;

public abstract class ShipControl : MonoBehaviour {

	public Transform cameraPoint;
	public GameObject playerCamera;
	

	public abstract void TransferControl( GameObject cam );

	//Do this to clean up the object in preperation for deactivation or destruction
	public void CleanUp(){
		Debug.Log ("Yo!");

		if( playerCamera != null ){
			//TODO Cannot change object hierarchy while its parent is being deactivated. This means if I deactivate the ship this camera's on, I can't deparent it here.
			playerCamera.transform.parent = null;
			
			playerCamera = null;
		}
	}

}
