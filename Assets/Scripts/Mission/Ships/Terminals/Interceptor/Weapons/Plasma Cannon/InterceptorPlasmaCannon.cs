using UnityEngine;
using System.Collections;

public class InterceptorPlasmaCannon : TerminalWeapon {

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

    IEnumerator Cooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds( coolDown );
        isOnCooldown = false;
    }

    void FixedUpdate() {
        if( !stat.overheat && !isOnCooldown && stat.fire ) {

            Instantiate( AmmoObject, transform.position, transform.rotation );
            //obj.SendMessage( "PassFiringVector", rootRigidbody.velocity, SendMessageOptions.RequireReceiver );

            HeatHandling( ref currentHeat, heatGeneratedPerShot, maxHeat, ref stat.overheat );
            stat.heatPercent = currentHeat / maxHeat;
            StartCoroutine( Cooldown() );
        } else
            HeatSink( ref currentHeat, heatSinkPerSecond, ref stat.overheat );

        stat.heatPercent = currentHeat / maxHeat;
    }

}
