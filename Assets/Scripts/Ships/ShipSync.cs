using UnityEngine;
using System.Collections;

public abstract class ShipSync : TNBehaviour {

    protected override void OnEnable() {
        base.OnEnable();
        if( TNManager.isHosting ) StartCoroutine( SyncData() );
    }

    private IEnumerator SyncData() {
        while( true ) {
            SendData();

            yield return new WaitForSeconds( 1f / SessionManager.instance.maxNetworkUpdatesPerSecond );
        }
    }

    protected abstract void SendData();

}
