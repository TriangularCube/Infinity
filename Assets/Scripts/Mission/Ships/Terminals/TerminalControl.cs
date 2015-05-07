using UnityEngine;
using System.Collections;
using TNet;

public class TerminalControl : ShipControl {

	[SerializeField]
	protected Terminal shipCore;



    void Update() {

        //Resolve Docking Request
        if( GetInput.Dock() ) {
            tno.Send( "RequestDock", Target.Host );
        }

        #region Attitude Control
        //Get input to update lookVector
        if( PlayerSettings.currentProfile.interceptorLookMode == InterceptorLookMode.Free ) {

            //Camera Changes
            lookRotation = RotationProcess( lookRotation, transform.up );

            targetLookDirectionToSync = lookRotation;

        } else {
            //TODO Flight input for Locked type

            //Probably something to do with Screen Spaces
        }
        #endregion

        #region Station Control
        //Input Direction
        inputDirectionSync = new Vector3( GetInput.ThrustX(), GetInput.ThrustY(), GetInput.ThrustZ() );

        //Boost and Break
        boostSync = GetInput.Boost();
        breakButtonSync = GetInput.Break();
        #endregion Station Control

        //TODO Weapon Selection needs to be overhauled
        WeaponSwitch();
        WeaponFire();

        UpdateCamera();
    }

    #region Weapon Switching and Firing

    private bool isSelectingWeapons = false;
    private TerminalWeaponStat weapon1 { get { return shipCore.status.weapon1; } }
    private TerminalWeaponStat weapon2 { get { return shipCore.status.weapon2; } }
    private TerminalWeaponStat weapon3 { get { return shipCore.status.weapon3; } }

    protected virtual void WeaponFire() {
        if( GetInput.Fire() ) {
            if( weapon1.selected ) weapon1.fire = true;
            if( weapon2.selected ) weapon2.fire = true;
            if( weapon3.selected ) weapon3.fire = true;
        } else {
            weapon1.fire = false;
            weapon2.fire = false;
            weapon3.fire = false;
        }
    }

    protected virtual void WeaponSwitch() {
        //If any of the select button is released, and none of the buttons are currently being pressed, we can assume the player's finished selecting
        if( GetInput.SelectWeaponKeyUp( 1 ) || GetInput.SelectWeaponKeyUp( 2 ) || GetInput.SelectWeaponKeyUp( 3 ) ) {
            if( !GetInput.SelectWeaponKeyHeld( 1 ) && !GetInput.SelectWeaponKeyHeld( 2 ) && !GetInput.SelectWeaponKeyHeld( 3 ) ) {
                isSelectingWeapons = false;
            }
        }

        if( GetInput.SelectWeaponKeyDown( 1 ) || GetInput.SelectWeaponKeyDown( 2 ) || GetInput.SelectWeaponKeyDown( 3 ) ) {
            if( !isSelectingWeapons ) {
                weapon1.selected = false;
                weapon2.selected = false;
                weapon3.selected = false;

                isSelectingWeapons = true;
            }

            if( GetInput.SelectWeaponKeyDown( 1 ) ) weapon1.selected = true;
            if( GetInput.SelectWeaponKeyDown( 2 ) ) weapon2.selected = true;
            if( GetInput.SelectWeaponKeyDown( 3 ) ) weapon3.selected = true;
        }
    }
    #endregion Weapon Switching and Firing

    #region Sync To Host
    protected override void OnEnable() {
        base.OnEnable();

        targetLookDirectionToSync = transform.rotation;

        StartCoroutine( SyncToHost() );
    }



    //Sync Variables
    private Quaternion targetLookDirectionToSync = Quaternion.identity; //Our vector to rotate towards, which happens to also be our free look vector
    private Vector3 inputDirectionSync = Vector3.zero;
    private bool breakButtonSync = false;
    private bool boostSync = false;
    private bool fireWeapon1ToSync = false, fireWeapon2ToSync = false, fireWeapon3ToSync = false;



    private IEnumerator SyncToHost() {

        while( true ) {
            tno.SendQuickly( 2, Target.Host, targetLookDirectionToSync, inputDirectionSync, breakButtonSync, boostSync, fireWeapon1ToSync, fireWeapon2ToSync, fireWeapon3ToSync );
            yield return new WaitForSeconds( 1f / SessionManager.instance.maxNetworkUpdatesPerSecond );
        }

    }

    protected override void OnDisable() {
        base.OnDisable();

        //Reset all variables on disable
        targetLookDirectionToSync = Quaternion.identity;
        inputDirectionSync = Vector3.zero;
        breakButtonSync = false;
        boostSync = false;
        fireWeapon1ToSync = false;
        fireWeapon2ToSync = false;
        fireWeapon3ToSync = false;
    }
    #endregion Sync To Host

}
