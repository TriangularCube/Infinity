using UnityEngine;
using System.Collections;

public class ShipSelectButton : PimpedMonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
    LaunchMenu launchMenu;
    [SerializeField]
    private UISprite sprite;
    [SerializeField]
    private UITexture underRepair;
    [SerializeField]
    private new Collider collider;
#pragma warning restore 0649

    public Terminal terminal;

    void Awake() {
        launchMenu = HUD.instance.getLaunchMenu();
    }

    void OnClick() {
        launchMenu.SelectTerminal( this );
    }

    public void Select( bool isSelected ) {
        if( isSelected ) {
            sprite.color = launchMenu.selectedColor;
        } else {
            sprite.color = launchMenu.deselectedColor;
        }
    }

    public void Disable() {
        sprite.color = launchMenu.deselectedColor;
        collider.enabled = false;
    }

    public void SetButtonActive( bool active ) {
        if( active ) {
            gameObject.SetActive( true );
        } else {
            gameObject.SetActive( false );
        }
    }

    void Update() {

        //Update the display of this button based on the Terminal we're representing
        

    }

}
