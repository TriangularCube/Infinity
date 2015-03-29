using UnityEngine;
using System.Collections;

public abstract class ShipSync : PimpedMonoBehaviour {

    protected virtual void OnEnable() {
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
