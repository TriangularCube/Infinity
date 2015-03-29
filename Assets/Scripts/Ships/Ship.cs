using UnityEngine;
using System.Collections;

[RequireComponent( typeof ( Rigidbody ) )]

public abstract class Ship : TNBehaviour {

	//Madatory check for if a player is on this ship
	//public abstract bool ContainsPlayer( TNet.Player check );

	//A list of enemy ships this ship can see
	protected TNet.List<GameObject> trackingList = new TNet.List<GameObject>();
	

	#region Cache
	private Transform _transform;
	public new Transform transform{
		get{
			return _transform;
		}
	}

    private Rigidbody _rigidBody;
    public new Rigidbody rigidbody {
        get {
            return _rigidBody;
        }
    }

    protected virtual void Awake() {
		_transform = base.transform;
        _rigidBody = GetComponent<Rigidbody>();
	}
	#endregion Cache

	#region Ship Movement
	[SerializeField, Group("Movement")]
	protected float maxSpeed = 20f;
	[SerializeField, Group("Movement")]
	protected float maxBurstSpeed = 45f;

	[SerializeField, Group("Movement")]
	protected float forwardAcceleration, backwardAcceleration, sideAcceleration, verticalAcceration;
	#endregion

	#region Sync
	
	#endregion
}
