using UnityEngine;
using System.Collections;

public class InterceptorAutoCannon : TerminalWeapon {

    public override int reserve {
        get { return reserveAmmunition; }
    }

    public int reserveAmmunition = 500;

    [SerializeField]
    private GameObject AmmoObject;

    [SerializeField]
    private float coolDown = 0.1f;
    private bool isOnCooldown = false;

    public override void Fire() {
        if( !isOnCooldown ) {
            /* GameObject obj = (GameObject) */
            Instantiate( AmmoObject, transform.position, transform.rotation );
            //obj.SendMessage( "PassFiringVector", rootRigidbody.velocity, SendMessageOptions.RequireReceiver );

            reserveAmmunition--;

            StartCoroutine( Cooldown() );
        }
    }

    IEnumerator Cooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds( coolDown );
        isOnCooldown = false;
    }

}
