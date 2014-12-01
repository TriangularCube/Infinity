using UnityEngine;
using System.Collections;

public class TerminalWeapon : PimpedMonoBehaviour {

	[SerializeField]
	private GameObject Ammo;
	
	[SerializeField]
	private float coolDown = 0.1f;
	private bool isOnCooldown = false;

	public void Fire(){
		//TODO Fire stuff
		
		if (!isOnCooldown) {
			/* GameObject obj = (GameObject) */ Instantiate( Ammo, transform.position, transform.rotation );
			//obj.SendMessage( "PassFiringVector", rootRigidbody.velocity, SendMessageOptions.RequireReceiver );
			StartCoroutine( Cooldown() );
		}
	}

	IEnumerator Cooldown(){
		isOnCooldown = true;
		yield return new WaitForSeconds( coolDown );
		isOnCooldown = false;
	}
}
