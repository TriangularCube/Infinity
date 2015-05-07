﻿using UnityEngine;

public abstract class ShipControl : TNBehaviour {

	[SerializeField]
	protected Transform cameraPoint;

	protected Camera playerCamera = null;

    //A flag to make sure camera only updates after physics updates, and only once between physics updates
	private bool updateCamera = false;

    //The rotation the Camera will reference when updating after physics update
	protected Quaternion lookRotation;

	protected override void OnEnable(){
        base.OnEnable();
		lookRotation = cameraPoint.rotation;
	}

	void FixedUpdate(){
		
		//Update the camera this frame
		updateCamera = true;
		
	}

	protected void UpdateCamera(){
		//If FixedUpdate ran this frame
        if( !updateCamera ) return;
			
		//Update the camera. Since Update runs after internal physics updates, this means all movement would have been done by this time
		playerCamera.transform.rotation = lookRotation;
		playerCamera.transform.position = lookRotation * cameraPoint.localPosition + transform.position;
			
		updateCamera = false;
	}



	public virtual void Assign(){

		playerCamera = Camera.main;//HACK

		enabled = true;

		//TODO Do additional stuff to pull camera towards camera point

	}

	//Do this to clean up the object in preparation for deactivation or destruction
	protected virtual void OnDisable(){

        updateCamera = false;
		playerCamera = null;
		enabled = false;

	}

    protected static Quaternion RotationProcess( Quaternion input, Vector3 up ) {
        input = Quaternion.AngleAxis( GetInput.MouseX(), up ) * input;
        input = input * Quaternion.AngleAxis( GetInput.MouseY(), Vector3.right );
        input = input * Quaternion.AngleAxis( GetInput.Roll(), Vector3.forward );

        return input;
    }

}
