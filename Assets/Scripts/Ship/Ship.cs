using UnityEngine;
using System.Collections;

public abstract class Ship : TNBehaviour {

	//HACK We shouldn't need this
	protected CameraControls playerCameraControl;

	void Start(){
		playerCameraControl = PlayersManager.Instance.playerCamControl;
	}

	public abstract bool ContainsPlayer( TNet.Player check );

	public abstract void AssignDefault( TNet.Player player );
}
