using UnityEngine;
using System.Collections;

public class InterceptorAutoCannonRound : MonoBehaviour {

	[SerializeField]
	float velocity = 100f;
	[SerializeField]
	float range = 300f;

	float distanceTraveled = 0f;

	// Update is called once per frame
	void FixedUpdate () {

		float moveDistance = velocity * Time.deltaTime;
		RaycastHit hit;

		if( Physics.SphereCast( transform.position, 0.22f, transform.forward, out hit, moveDistance ) ){

			Debug.Log( "Hit something with Interceptor Auto Cannon! " + hit.collider.gameObject.name );

			//TODO possibly make some particle explosion

			//TODO
			Destroy( gameObject );

		}

		transform.Translate( Vector3.forward * moveDistance );

		distanceTraveled += moveDistance;

		if( distanceTraveled > range ){

			Destroy( gameObject );

		}

	}
}
