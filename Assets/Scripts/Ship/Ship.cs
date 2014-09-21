using UnityEngine;
using System.Collections;

[RequireComponent( typeof ( Rigidbody ) )]
[RequireComponent( typeof ( Stats ) )]

public abstract class Ship : TNBehaviour {

	public abstract bool ContainsPlayer( TNet.Player check );

	//A convinience method, for the future if the game wants to directly assign a player to a ship
	public abstract void AssignDefault( TNet.Player player );
}
