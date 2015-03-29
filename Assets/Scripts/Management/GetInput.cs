using UnityEngine;
using System.Collections;

public class GetInput : Singleton<GetInput> {

    void Update() {

    }

    private float mouseX = 0f;
    public static float GetMouseX(){
        return instance.mouseX;
    }

}
