using UnityEngine;
using System.Collections;

public class InterceptorPlasmaCannon : TerminalWeapon {

    public override int reserve {
        get { throw new System.NotImplementedException(); }
    }

    //THIS WHOLE CLASS IS ONLY IMPLEMENTED WITH TEMPORARY FUNCTIONALITY

#pragma warning disable 0649
    [SerializeField]
    private GameObject AmmoObject;
#pragma warning restore 0649

    [SerializeField]
    private float coolDown = 0.1f;
    private bool isOnCooldown = false;

    public override void Fire() {
        if( !isOnCooldown ) {
            /* GameObject obj = (GameObject) */
            Instantiate( AmmoObject, transform.position, transform.rotation );
            //obj.SendMessage( "PassFiringVector", rootRigidbody.velocity, SendMessageOptions.RequireReceiver );
            StartCoroutine( Cooldown() );
        }
    }

    IEnumerator Cooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds( coolDown );
        isOnCooldown = false;
    }

}
