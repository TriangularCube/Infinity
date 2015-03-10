using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
	private Flagship flagship;
#pragma warning restore 0649

    void Awake(){
		//Debug.Log ("Awake on Dock");
		
        
        if (!TNManager.isHosting) {
			gameObject.SetActive( false );
		}
        
	}

	void OnTriggerEnter( Collider other ){

		other.transform.root.gameObject.GetComponent<Terminal>().IsInRangeToDock( true );
        //TODO Possibly create a list of Terminals in range to dock as an additional check

	}

	void OnTriggerExit( Collider other ){

        other.transform.root.gameObject.GetComponent<Terminal>().IsInRangeToDock( false );

	}
}