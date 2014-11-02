using UnityEngine;
using System.Collections;

public class InterceptorAutoCannonRound : MonoBehaviour {

	[SerializeField]
	float speed = 150f;

	private Vector3 firingVector;

	void Awake(){
		Invoke ("Expired", 3f);
	}

	public void PassFiringVector( Vector3 vector ){
		firingVector = transform.InverseTransformDirection( vector );
	}

	void FixedUpdate(){
		//TODO Do a spherecast here

		transform.Translate ( ( ( Vector3.forward * speed ) + firingVector ) * Time.deltaTime);
	}

	void Expired(){
		Destroy (gameObject);
	}

}
