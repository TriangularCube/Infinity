using UnityEngine;
using System.Collections;

public class TerminalWeapon : MonoBehaviour {

	void Awake(){
		transform.root.GetComponent<TerminalFireControl> ().RegisterWeapon (this);
	}

	[SerializeField]
	private GameObject Ammo;

	[SerializeField]
	private float coolDown = 0.1f;
	private bool isOnCooldown = false;

	public void Fire(){
		//TODO Fire stuff

		if (!isOnCooldown) {
			Instantiate( Ammo, transform.position, transform.rotation );

			StartCoroutine( Cooldown() );
		}
	}

	IEnumerator Cooldown(){
		isOnCooldown = true;
		yield return new WaitForSeconds( coolDown );
		isOnCooldown = false;
	}

}
