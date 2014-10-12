using UnityEngine;
using System.Collections;

public class InterceptorAutoCannonRound : MonoBehaviour {

	[SerializeField]
	float speed = 150f;

	void Awake(){
		Invoke ("Expired", 3f);
	}

	void Update(){
		//TODO Do a spherecast here

		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}

	void Expired(){
		Destroy (gameObject);
	}

}
