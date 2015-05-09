using UnityEngine;
using System.Collections;

public class Mission : MonoBehaviour {

    public string name = "Default Mission Name";

    void OnClick() {
        LobbyManager.instance.SelectMission( this );
    }

}
