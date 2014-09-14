using UnityEngine;
using System.Collections;

public abstract class Ship : TNBehaviour {

	public abstract bool ContainsPlayer( TNet.Player check );

	public abstract void AssignDefault( TNet.Player player );
}
