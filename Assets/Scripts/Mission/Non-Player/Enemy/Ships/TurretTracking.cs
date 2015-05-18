using UnityEngine;
using System.Collections;

public class TurretTracking : MonoBehaviour {

    public GameObject go;
    public float rotateSpeed = 2f;

    void Update() {
        Quaternion newQuat = Quaternion.LookRotation( transform.root.InverseTransformDirection( go.transform.position - transform.position ) );
        if( newQuat.eulerAngles.x < 270f )
            newQuat = Quaternion.Euler( 0f, newQuat.eulerAngles.y, newQuat.eulerAngles.z );
        else
            newQuat = Quaternion.Euler( Mathf.Clamp( newQuat.eulerAngles.x, 270f, 359f ), newQuat.eulerAngles.y, newQuat.eulerAngles.z );
        transform.localRotation = Quaternion.RotateTowards( transform.localRotation, newQuat, rotateSpeed );
    }

}
