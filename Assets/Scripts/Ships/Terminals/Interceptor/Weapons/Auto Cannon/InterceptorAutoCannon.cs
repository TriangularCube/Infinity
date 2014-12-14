using UnityEngine;
using System.Collections;
using TNet;

public class InterceptorAutoCannon : TerminalWeapon {

    public override int reserve {
        get { return reserveAmmunition; }
    }

    public int reserveAmmunition = 500;

#pragma warning disable 0649
    [SerializeField]
    private GameObject AmmoObject;
#pragma warning restore 0649

    [SerializeField]
    private float coolDown = 0.1f;
    private bool isOnCooldown = false;

    public override void Fire() {
        if( !_overHeated && reserveAmmunition > 0 && !isOnCooldown ) {

            Instantiate( AmmoObject, transform.position, transform.rotation );
            //obj.SendMessage( "PassFiringVector", rootRigidbody.velocity, SendMessageOptions.RequireReceiver );

            reserveAmmunition--;

            HeatHandling( ref _currentHeat, heatGeneratedPerTick, heatCapacity, ref _overHeated );
            StartCoroutine( Cooldown() );
        }
    }

    IEnumerator Cooldown() {
        isOnCooldown = true;
        yield return new WaitForSeconds( coolDown );
        isOnCooldown = false;
    }

    void FixedUpdate() {
        if( !isOnCooldown ) HeatSink( ref _currentHeat, heatSinkPerTick, ref _overHeated );
    }

    protected override void SendData() {
        tno.SendQuickly( 10, terminal.pilot, _currentHeat, _overHeated );
    }

    [RFC(10)]
    protected void RecieveData( float heat, bool over ) {
        _currentHeat = heat;
        _overHeated = over;
    }

}
