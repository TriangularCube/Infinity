using UnityEngine;

[System.Serializable]
public class ControlProfile {

    public enum BindType { AXIS, BUTTON }

    public BindType thrustX = BindType.BUTTON;
    public string thrustXAxis = null;
    public KeyCode thrustXPositive = KeyCode.D, thrustXNegative = KeyCode.A;

    public BindType thrustY = BindType.BUTTON;
    public string thrustYAxis = null;
    public KeyCode thrustYPositive = KeyCode.C, thrustYNegative = KeyCode.Z;

    public BindType thrustZ = BindType.BUTTON;
    public string thrustZAxis = null;
    public KeyCode thrustZPositive = KeyCode.W, thrustZNegative = KeyCode.S;

    public BindType roll = BindType.BUTTON;
    public KeyCode rollPositive = KeyCode.E, rollNegative = KeyCode.Q;

    public KeyCode breakButton = KeyCode.LeftShift;
    public KeyCode boost = KeyCode.Space;

    public KeyCode fire = KeyCode.Mouse0;

    public KeyCode look = KeyCode.Mouse1;

    public KeyCode dock = KeyCode.F;

    public KeyCode launchMenu = KeyCode.L;

    public KeyCode select1 = KeyCode.Alpha1, select2 = KeyCode.Alpha2, select3 = KeyCode.Alpha3;
}

public class GetInput {

    //TOOD, HACK
    static GetInput(){
        activeProfile = new ControlProfile();
    }

    private static ControlProfile activeProfile = null;

    //Convenience getter
    private static bool notMouseLocked { get { return !HUD.instance.mouseLocked; } }

    #region Mouse Accessors
    public static float MouseX(){
        if( notMouseLocked ) return 0f;
        return Input.GetAxis( "Mouse X" );
    }

    public static float MouseY() {
        if( notMouseLocked ) return 0f;
        return Input.GetAxis( "Mouse Y" );
    }
    #endregion Mouse Accessors

    public static float ThrustX() {
        if( notMouseLocked ) return 0f;
        switch( activeProfile.thrustX ) {
            case ControlProfile.BindType.BUTTON:
                if( Input.GetKey( activeProfile.thrustXPositive ) && Input.GetKey( activeProfile.thrustXNegative ) ) return 0f;
                if( Input.GetKey( activeProfile.thrustXPositive ) ) return 1f;
                if( Input.GetKey( activeProfile.thrustXNegative ) ) return -1f;
                return 0f;
            case ControlProfile.BindType.AXIS:
                if( activeProfile.thrustXAxis == null ) throw new System.Exception( "Thrust X Axis is not defined, but is called on" );
                return Input.GetAxis( activeProfile.thrustXAxis );
            default:
                throw new System.Exception( "Thrust X has neither Axis nor Buttons defined" );
        }
    }

    public static float ThrustY() {
        if( notMouseLocked ) return 0f;
        switch( activeProfile.thrustY ) {
            case ControlProfile.BindType.BUTTON:
                if( Input.GetKey( activeProfile.thrustYPositive ) && Input.GetKey( activeProfile.thrustYNegative ) ) return 0f;
                if( Input.GetKey( activeProfile.thrustYPositive ) ) return 1f;
                if( Input.GetKey( activeProfile.thrustYNegative ) ) return -1f;
                return 0f;
            case ControlProfile.BindType.AXIS:
                if( activeProfile.thrustYAxis == null ) throw new System.Exception( "Thrust Y Axis is not defined, but is called on" );
                return Input.GetAxis( activeProfile.thrustYAxis );
            default:
                throw new System.Exception( "Thrust Y has neither Axis nor Buttons defined" );
        }
    }

    public static float ThrustZ() {
        if( notMouseLocked ) return 0f;
        switch( activeProfile.thrustZ ) {
            case ControlProfile.BindType.BUTTON:
                if( Input.GetKey( activeProfile.thrustZPositive ) && Input.GetKey( activeProfile.thrustZNegative ) ) return 0f;
                if( Input.GetKey( activeProfile.thrustZPositive ) ) return 1f;
                if( Input.GetKey( activeProfile.thrustZNegative ) ) return -1f;
                return 0f;
            case ControlProfile.BindType.AXIS:
                if( activeProfile.thrustZAxis == null ) throw new System.Exception( "Thrust Z Axis is not defined, but is called on" );
                return Input.GetAxis( activeProfile.thrustZAxis );
            default:
                throw new System.Exception( "Thrust Z has neither Axis nor Buttons defined" );
        }
    }

    public static float Roll() {
        if( notMouseLocked ) return 0f;
        switch( activeProfile.roll ) {
            case ControlProfile.BindType.BUTTON:
                if( Input.GetKey( activeProfile.rollPositive ) && Input.GetKey( activeProfile.rollNegative) ) return 0f;
                if( Input.GetKey( activeProfile.rollPositive ) ) return 1f;
                if( Input.GetKey( activeProfile.rollNegative) ) return -1f;
                return 0f;
            case ControlProfile.BindType.AXIS:
                throw new System.Exception( "Roll Axis isn't implemented" );//TODO
            default:
                throw new System.Exception( "Roll has neither Axis nor Buttons defined" );
        }
    }

    public static bool Break() {
        if( notMouseLocked ) return false;
        return Input.GetKey( activeProfile.breakButton );
    }

    public static bool Boost() {
        if( notMouseLocked ) return false;
        return Input.GetKey( activeProfile.boost );
    }

    public static bool SelectWeaponKeyDown( int sel ) {
        if( notMouseLocked ) return false;
        switch( sel ) {
            case 1:
                return Input.GetKeyDown( activeProfile.select1 );
            case 2:
                return Input.GetKeyDown( activeProfile.select2 );
            case 3:
                return Input.GetKeyDown( activeProfile.select3 );
            default:
                throw new System.Exception( "A Weapon not 1 - 3 has been selected" );
        }
        
    }

    public static bool SelectWeaponKeyUp( int sel ) {
        if( notMouseLocked ) return false;
        switch( sel ) {
            case 1:
                return Input.GetKeyUp( activeProfile.select1 );
            case 2:
                return Input.GetKeyUp( activeProfile.select2 );
            case 3:
                return Input.GetKeyUp( activeProfile.select3 );
            default:
                throw new System.Exception( "A Weapon not 1 - 3 has been deselected" );
        }

    }

    public static bool SelectWeaponKeyHeld( int sel ) {
        if( notMouseLocked ) return false;
        switch( sel ) {
            case 1:
                return Input.GetKey( activeProfile.select1 );
            case 2:
                return Input.GetKey( activeProfile.select2 );
            case 3:
                return Input.GetKey( activeProfile.select3 );
            default:
                throw new System.Exception( "A Weapon not 1 - 3 has been deselected" );
        }
    }

    public static bool Fire() {
        if( notMouseLocked ) return false;
        return Input.GetKey( activeProfile.fire );
    }

    public static bool Look() {
        if( notMouseLocked ) return false;
        return Input.GetKey( activeProfile.look );
    }

    public static bool LookUp() {
        if( notMouseLocked ) return false;
        return Input.GetKeyUp( activeProfile.look );
    }

    public static bool Dock() {
        return Input.GetKeyDown( activeProfile.dock );
    }

    public static bool LaunchMenu() {
        return Input.GetKeyDown( activeProfile.launchMenu );
    }
}
