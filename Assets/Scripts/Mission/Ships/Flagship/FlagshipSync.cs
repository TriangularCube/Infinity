using UnityEngine;
using System.Collections;
using TNet;

public class FlagshipSync : ShipStat {

    //DATA
    public Quaternion targetLookDirection = Quaternion.identity;
    public Vector3 targetAccelDirection = Vector3.zero;

    protected override void SendData() {
        //TODO
        tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation );
    }

    [RFC(1)]//Called from Host to Everyone
    private void ReceiveSyncFromHost( Vector3 position, Quaternion rotation ) {
        //TODO
        throw new System.NotImplementedException();
    }

    [RFC(2)] //Called from Navigation
    public void UpdateNavigationControl( Vector3 moveVector, Quaternion lookVector ) {
        targetLookDirection = lookVector;
        targetAccelDirection = moveVector;
    }

}
