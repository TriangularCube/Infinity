using UnityEngine;
using System.Collections;
using TNet;

public class TerminalControl : ShipControl {

	[SerializeField]
	protected Terminal shipCore;

    private bool isSelectingWeapons = false;
    private TerminalWeaponStat weapon1 { get { return shipCore.status.weapon1; } }
    private TerminalWeaponStat weapon2 { get { return shipCore.status.weapon2; } }
    private TerminalWeaponStat weapon3 { get { return shipCore.status.weapon3; } }

    void Update() {

        if( HUD.instance.mouseLocked ) {

            //Resolve Docking Request
            if( Input.GetButtonDown( "Dock" ) ) {
                tno.Send( "RequestDock", Target.Host );
            }

            #region Rotation
            //Get input to update lookVector
            if( PlayerSettings.GetInterceptorLookMode() == InterceptorLookMode.Free ) {

                //Camera Changes
                lookRotation = Quaternion.AngleAxis( Input.GetAxis( "Mouse X" ), transform.up ) * lookRotation;
                lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Mouse Y" ), Vector3.right );
                lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Roll" ), Vector3.forward );

                targetLookDirectionToSync = lookRotation;

            } else {
                //TODO Flight input for Locked type

                //Probably something to do with Screen Spaces
            }
            #endregion

            #region Station
            //Input Direction
            inputDirectionSync = new Vector3( Input.GetAxis( "Thrust X" ), Input.GetAxis( "Thrust Y" ), Input.GetAxis( "Thrust Z" ) );

            //Boost and Break
            boostSync = Input.GetButton( "Boost" );
            breakButtonSync = Input.GetButton( "Break" );
            #endregion Station

            //TODO Weapon Selection needs to be overhauled
            #region Weapon Switching and Firing
            //If any of the select button is released, and none of the buttons are currently being pressed, we can assume the player's finished selecting
            if( Input.GetButtonUp( "Select 1" ) || Input.GetButtonUp( "Select 2" ) || Input.GetButtonUp( "Select 3" ) ) {
                if( !Input.GetButton( "Select 1" ) && !Input.GetButton( "Select 2" ) && !Input.GetButton( "Select 3" ) ) {
                    isSelectingWeapons = false;
                }
            }

            if( Input.GetButtonDown( "Select 1" ) || Input.GetButtonDown( "Select 2" ) || Input.GetButtonDown( "Select 3" ) ) {
                if( !isSelectingWeapons ) {
                    weapon1.selected = false;
                    weapon2.selected = false;
                    weapon3.selected = false;

                    isSelectingWeapons = true;
                }

                if( Input.GetButtonDown( "Select 1" ) ) weapon1.selected = true;
                if( Input.GetButtonDown( "Select 2" ) ) weapon2.selected = true;
                if( Input.GetButtonDown( "Select 3" ) ) weapon3.selected = true;
            }

            if( Input.GetButton( "Fire" ) ) {
                if( weapon1.selected ) weapon1.fire = true;
                if( weapon2.selected ) weapon2.fire = true;
                if( weapon3.selected ) weapon3.fire = true;
            } else {
                weapon1.fire = false;
                weapon2.fire = false;
                weapon3.fire = false;
            }
            #endregion Weapon Switching and Firing

        } else {

            //If mouse is not locked pass through zeroed values
            boostSync = false;
            breakButtonSync = false;
            inputDirectionSync = Vector3.zero;

        }

        UpdateCamera();
    }

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
