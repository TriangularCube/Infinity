using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

	public float maxSpeed;
	public float acceleration;
	public float breakSmooth;
	public bool isCarrier;

	public float maxSpeedSqr{ get { return maxSpeed * maxSpeed; } }

}
