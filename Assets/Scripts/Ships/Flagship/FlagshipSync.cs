using UnityEngine;
using System.Collections;
using TNet;

public class FlagshipSync : ShipSync {

    #region Sync
    protected override void SendData() {
        //TODO
        tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation );
    }

    [RFC( 2 )]
    public void UpdateNavigationControl( Vector3 moveVector, Quaternion lookVector ) {
        targetLookDirection = lookVector;
        targetAccelDirection = moveVector;
    }
    #endregion Sync

}
