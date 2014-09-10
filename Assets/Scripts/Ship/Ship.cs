using UnityEngine;
using System.Collections;

public abstract class Ship : TNBehaviour {

	public abstract bool ContainsFocusedPlayer( TNet.Player check );
}
