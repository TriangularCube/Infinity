using UnityEngine;
using System.Collections;

public class TurretTracking : MonoBehaviour {

    public GameObject go;

    void Update() {
        transform.localRotation = Quaternion.LookRotation( transform.root.InverseTransformDirection( go.transform.position - transform.position ) );
    }

}
