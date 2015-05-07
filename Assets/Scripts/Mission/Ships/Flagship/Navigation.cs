using UnityEngine;
using System.Collections;

public class Navigation : ShipControl {

    private bool isFreelook = true;
    private Quaternion targetShipFacing = Quaternion.identity;

    void Update() {

        if( GetInput.Look() && isFreelook ) {

            //TODO Take into account explosion forces that auto-move the ship

            //TODO Snap the camera to targetShipFacing
            lookRotation = targetShipFacing;//DEBUG

            isFreelook = false;
        } else if( GetInput.LookUp() && !isFreelook ) {
            isFreelook = true;
        }

        if( HUD.instance.mouseLocked ) {
            if( isFreelook ) {

                //BUG The first time after assigning to navigation, lookRotation will not update
                //GetComponent<Rigidbody>().MoveRotation( transform.rotation * Quaternion.AngleAxis( Time.deltaTime * 1f, transform.right ) );

                //Camera Changes
                lookRotation = Quaternion.AngleAxis( Input.GetAxis( "Mouse X" ), transform.up ) * lookRotation;
                lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Mouse Y" ), Vector3.right );
                lookRotation = lookRotation * Quaternion.AngleAxis( Input.GetAxis( "Roll" ), Vector3.forward );

                //Debug.Log( lookRotation );
            } else {
                targetShipFacing = Quaternion.AngleAxis( Input.GetAxis( "Mouse X" ), transform.up ) * targetShipFacing;
                targetShipFacing = targetShipFacing * Quaternion.AngleAxis( Input.GetAxis( "Mouse Y" ), Vector3.right );
                targetShipFacing = targetShipFacing * Quaternion.AngleAxis( Input.GetAxis( "Roll" ), Vector3.forward );

                lookRotation = targetShipFacing;//DEBUG...?
            }

            UpdateCamera();

            //Movement updates
        }



    }

    #region Sync
    protected override void OnEnable() {
        base.OnEnable();

        targetShipFacing = transform.rotation;
        StartCoroutine( SyncToHost() );
    }

    private IEnumerator SyncToHost() {
        while( true ) {
            //Do Stuff
            tno.SendQuickly( 2, TNet.Target.Host, Vector3.zero, targetShipFacing );//Sent to FlagshipSync

            yield return new WaitForSeconds( 1f / SessionManager.instance.maxNetworkUpdatesPerSecond );
        }
    }
    #endregion
}
