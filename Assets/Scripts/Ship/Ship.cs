using UnityEngine;
using System.Collections;

public abstract class Ship : TNBehaviour {

	public abstract bool ContainsPlayer( TNet.Player check );
}
