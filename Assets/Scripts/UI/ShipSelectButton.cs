using UnityEngine;
using System.Collections;

public class ShipSelectButton : PimpedMonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
    private Color selectedColor, deselectedColor, disabledColor;
    [SerializeField]
    private UISprite sprite;
    [SerializeField]
    private UITexture underRepair;
#pragma warning restore 0649

    private Terminal terminal;

    void OnClick() {
        HUD.instance.SelectTerminal( this );
    }

    public void Select( bool isSelected ) {
        if( isSelected ) {
            sprite.color = selectedColor;
        } else {
            sprite.color = deselectedColor;
        }
    }

    void Update() {

        //Update the display of this button based on the Terminal we're representing
        

    }

}
