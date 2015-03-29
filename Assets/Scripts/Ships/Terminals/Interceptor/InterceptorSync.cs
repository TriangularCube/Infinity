using UnityEngine;
using System.Collections;
using TNet;

public class InterceptorSync : TerminalSync {

    protected override void SendData() {
        Debug.Log( "Sync Send" );
    }

    [RFC(1)]
    private void RecieveSync( Vector3 position, Quaternion facing, int selectedWeapon, bool fire ) {

        //TODO Implement out of order handling
        // TODO Do stuff with recieved sync data

        // TODO Facing and Flight Input

        currentWeapon = selectedWeapon;

        // TODO Fire Weapon

    }

}
