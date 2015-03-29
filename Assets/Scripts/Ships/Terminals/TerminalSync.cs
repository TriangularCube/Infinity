using UnityEngine;
using System.Collections;
using TNet;

public class TerminalSync : ShipSync {

    [SerializeField]
    private Terminal core;

    //Our vector to rotate towards, which happens to also be our free look vector
    public Quaternion targetLookDirection = Quaternion.identity;

    //A normalized input vector
    public Vector3 inputDirection = Vector3.zero;

    //Whether we are applying Breaks
    public bool breakButton = false;

    //Boost Status
    public bool isBoostActive = false;

    //The state of the trigger
    public bool fireWeapon1 = false, fireWeapon2 = false, fireWeapon3 = false;

    //Whether this Terminal is currently under repairs
    public bool isUnderRepair { get; private set; }

    void Awake() {
        isUnderRepair = false; //HACK, TODO
    }

    protected override void SendData() {
        throw new System.NotImplementedException();
    }

    [RFC( 2 )]
    protected void RecieveSyncOnHost( Quaternion lookDirection, Vector3 input, bool onBreak, bool boost, bool weapon1Fire, bool weapon2Fire, bool weapon3Fire ) {

        targetLookDirection = lookDirection;
        inputDirection = input;
        breakButton = onBreak;
        isBoostActive = boost;
        
        fireWeapon1 = weapon1Fire;
        fireWeapon2 = weapon2Fire;
        fireWeapon3 = weapon3Fire;

    }

    void OnDisable() {
        targetLookDirection = Quaternion.identity;
        inputDirection = Vector3.zero;
        breakButton = false;
        isBoostActive = false;

        fireWeapon1 = false;
        fireWeapon2 = false;
        fireWeapon3 = false;
    }

    protected override void OnEnable() {
        base.OnEnable();

        targetLookDirection = transform.rotation;//Align the look rotation on launch
    }
}
