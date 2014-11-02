using UnityEngine;
using System.Collections;

public abstract class ShipControl : TNBehaviour {
	
	public abstract void AssignDefault( TNet.Player player );

	public abstract void AssignRole( TNet.Player player, string role );

	public abstract string[] GetAvailableRoles ();

	public abstract void Reset( TNet.Player player );
}
