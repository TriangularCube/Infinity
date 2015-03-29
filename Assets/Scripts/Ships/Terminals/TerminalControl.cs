using UnityEngine;
using System.Collections;
using TNet;

public class TerminalControl : ShipControl {

	[SerializeField]
	protected Terminal shipCore;

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

            //Input Direction
            inputDirectionSync = new Vector3( Input.GetAxis( "Thrust X" ), Input.GetAxis( "Thrust Y" ), Input.GetAxis( "Thrust Z" ) );

            //Boost and Break
            boostSync = Input.GetButton( "Boost" );
            breakButtonSync = Input.GetButton( "Break" );

            #region Weapon Switching and Firing
            if( !weaponSwitchCooldown ) {
                if( Input.GetAxis( "Mouse ScrollWheel" ) > 0f ) {
                    if( ++currentWeapon > 3 ) currentWeapon = 1;
                } else if( Input.GetAxis( "Mouse ScrollWheel" ) < 0f ) {
                    if( --currentWeapon < 1 ) currentWeapon = 3;
                }

                StartCoroutine( WeaponSwitchCooldown() );
            }

            switch( currentWeapon ) {
                case 1:
                    fireWeapon1ToSync = Input.GetButton( "Fire" );
                    break;
                case 2:
                    fireWeapon2ToSync = Input.GetButton( "Fire" );
                    break;
                case 3:
                    fireWeapon3ToSync = Input.GetButton( "Fire" );
                    break;
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

    #region Weapon Switching Logic
    private bool weaponSwitchCooldown = false;
    [SerializeField]
    //Weapon Switch cooldown time
    private float weaponSwitchCooldownTime = 0.3f;

    private IEnumerator WeaponSwitchCooldown() {
        weaponSwitchCooldown = true;
        yield return new WaitForSeconds( weaponSwitchCooldownTime );
        weaponSwitchCooldown = false;
    }

    protected int currentWeapon = 1;
    #endregion Weapon Switching Logic

    #region HUD Hook
    public int selectedWeapon { get { return currentWeapon; } }
    #endregion HUD Hook

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
