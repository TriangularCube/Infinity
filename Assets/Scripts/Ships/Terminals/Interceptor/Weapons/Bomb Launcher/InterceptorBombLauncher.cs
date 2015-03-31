using UnityEngine;
using System.Collections;

public class InterceptorBombLauncher : TerminalWeapon {

    //THIS WHOLE CLASS IS ONLY IMPLEMENTED WITH TEMPORARY FUNCTIONALITY

#pragma warning disable 0649
    [SerializeField]
    private GameObject AmmoObject;

    [SerializeField]
    private float heatGeneratedPerShot;
#pragma warning restore 0649

    [SerializeField]
    private float coolDown = 0.1f;
    private bool isOnCooldown = false;

    private void Fire() {
        if( !stat.overheat && stat.ammo > 0 && !isOnCooldown ) {

            Instantiate( AmmoObject, transform.position, transform.rotation );
            //obj.SendMessage( "PassFiringVector", rootRigidbody.velocity, SendMessageOptions.RequireReceiver );

            stat.ammo--;

            HeatHandling( ref currentHeat, heatGeneratedPerShot, maxHeat, ref stat.overheat );
            StartCoroutine( Cooldown() );
        }
    }

    IEnumerator Cooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds( coolDown );
        isOnCooldown = false;
    }

    void FixedUpdate() {
        if( stat.overheat || ( !stat.fire && !isOnCooldown ) ) HeatSink( ref currentHeat, heatSinkPerTick, ref stat.overheat );
    }

}
