using UnityEngine;
using System.Collections;

[RequireComponent( typeof ( Rigidbody ) )]

public abstract class Ship : TNBehaviour {

	//Madatory check for if a player is on this ship
	public abstract bool ContainsPlayer( TNet.Player check );

	//A list of enemy ships this ship can see
	protected TNet.List<GameObject> trackingList = new TNet.List<GameObject>();

	//A convinience method, for the future if the game wants to directly assign a player to a ship
	public abstract void AssignDefault( TNet.Player player );

	#region Cache Transform
	private Transform thisTransform;

	protected virtual void Awake(){
		thisTransform = base.transform;
	}

	public new Transform transform{
		get{
			return thisTransform;
		}
	}
	#endregion

	#region Ship Movement
	protected Vector3 velocity = Vector3.zero;
	[SerializeField]
	protected float maxSpeed = 20f;
	[SerializeField]
	protected float maxBurstSpeed = 45f;

	[SerializeField]
	protected float forwardAcceleration, backwardAcceleration, sideAcceleration, verticalAcceration;
	#endregion

	#region Sync
	protected override void OnEnable(){
		base.OnEnable ();

		if( TNManager.isHosting) StartCoroutine (Sync ());
	}

	private IEnumerator Sync(){
		while (true) {
			SendData();

			yield return new WaitForSeconds (1f / SessionManager.instance.maxNetworkUpdatesPerSecond);
		}
	}

	protected abstract void SendData ();
	#endregion
}
