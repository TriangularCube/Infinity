using UnityEngine;
using System.Collections;

[RequireComponent( typeof ( Rigidbody ) )]
[RequireComponent( typeof ( Stats ) )]

public abstract class Ship : TNBehaviour {

	//Cached reference to our transform
	protected Transform thisTransform;

	//Madatory check for if a player is on this ship
	public abstract bool ContainsPlayer( TNet.Player check );

	//A list of enemy ships this ship can see
	public TNet.List<GameObject> trackingList = new TNet.List<GameObject>();

	//A convinience method, for the future if the game wants to directly assign a player to a ship
	public abstract void AssignDefault( TNet.Player player );

	protected virtual void Awake(){
		thisTransform = base.transform;
	}

	public new Transform transform{
		get{
			return thisTransform;
		}
	}
}
