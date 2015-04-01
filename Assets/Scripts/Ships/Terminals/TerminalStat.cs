using UnityEngine;
using System.Collections;
using TNet;

public class TerminalWeaponStat {
    public bool selected = false;

    public string name = "Generic Weapon";
    public bool fire = false;
    public int ammo = -1;
    public string ammoDisplay {
        get {
            if( ammo == -1 )
                return "-";
            else
                return ammo.ToString();
        }
    }
    public float heatPercent = 0f;
    public bool overheat = false;

    public void Reset() {
        selected = false;
        name = "Generic Weapon";
        fire = false;
        ammo = -1;
        heatPercent = 0f;
        overheat = false;
    }
}

public class TerminalStat : ShipStat {

    [SerializeField]
    private Terminal core;

    #region Station and Attitude Control Vairables
    //Our vector to rotate towards, which happens to also be our free look vector
    public Quaternion targetLookDirection = Quaternion.identity;

    //A normalized input vector
    public Vector3 inputDirection = Vector3.zero;

    //Whether we are applying Breaks
    public bool breakButton = false;

    //Boost Status
    public bool isBoostActive = false;
    #endregion Station and Attitude Control Vairables

    #region Weapon Variables
    //TODO: Possibly the Weapon Selection here too

    public TerminalWeaponStat weapon1 = new TerminalWeaponStat(), weapon2 = new TerminalWeaponStat(), weapon3 = new TerminalWeaponStat();
    #endregion Weapon Variables

    //Whether this Terminal is currently under repairs
    public bool isUnderRepair = false;

    #region Sync From Host to Clients
    protected override void SendData() {
        tno.SendQuickly( 1, Target.Others, transform.position, transform.rotation,
                         weapon1.fire, weapon1.ammo, weapon1.overheat, 
                         weapon2.fire, weapon2.ammo, weapon2.overheat,
                         weapon3.fire, weapon3.ammo, weapon3.overheat );
    }

    [RFC(1)] //TODO Seriously incomplete
    private void RecieveSync( Vector3 position, Quaternion facing, bool fire1, bool fire2, bool fire3 ) {

        //TODO Implement out of order handling
        // TODO Do stuff with recieved sync data

        // TODO Facing and Flight Input

        // TODO Fire Weapon

    }
    #endregion Sync From Host to Clients

    //Sync from Pilot to Host
    [RFC(2)] //The rest of this is in TerminalControl
    protected void RecieveSyncOnHost( Quaternion lookDirection, Vector3 input, bool onBreak, bool boost, bool weapon1Fire, bool weapon2Fire, bool weapon3Fire ) {

        if( !gameObject.activeSelf ) return; //Don't process if we're not active (probably due to being docked)

        //Out of order handling

        targetLookDirection = lookDirection;
        inputDirection = input;
        breakButton = onBreak;
        isBoostActive = boost;
        
        weapon1.fire = weapon1Fire;
        weapon2.fire = weapon2Fire;
        weapon3.fire = weapon3Fire;

    }

    //Reset all variable when this script (more importantly, this GameObject) is disabled, most likely through docking.
    void OnDisable() {
        targetLookDirection = Quaternion.identity;
        inputDirection = Vector3.zero;
        breakButton = false;
        isBoostActive = false;

        weapon1.Reset();
        weapon2.Reset();
        weapon3.Reset();

        weapon1.selected = true;
    }

}
