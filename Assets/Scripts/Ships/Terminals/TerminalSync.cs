using UnityEngine;
using System.Collections;
using TNet;

public abstract class TerminalSync : ShipSync {

    [SerializeField]
    private Terminal core;

    //The current weapon this terminal has selected
    public int currentWeapon = 1;

    //The state of the trigger
    public bool fireWeapon = false;

    //Whether this Terminal is currently under repairs
    public bool isUnderRepair { get; private set; }

    void Awake() {
        isUnderRepair = false; //HACK, TODO
    }

    [RFC( 2 )]
    protected void RecieveSyncOnHost( Quaternion lookDirection, Vector3 input, bool onBreak, bool boost, bool fireCurrentWeapon ) {

        targetLookDirection = lookDirection;
        inputDirection = input;
        breakButton = onBreak;
        isBoostActive = boost;

        fireWeapon = fireCurrentWeapon;

    }
}
