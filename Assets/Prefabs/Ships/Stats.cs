using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

	#region Velocity variables
	public float maxSpeed;
	public float breakForce;

	public float forwordForce, backwardForce, otherForce;
	#endregion

	public float maxSpeedSqr{ get { return maxSpeed * maxSpeed; } }

	public bool hyperThurst = false;

}
