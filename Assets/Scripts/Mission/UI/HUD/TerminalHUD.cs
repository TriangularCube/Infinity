using UnityEngine;
using System.Collections;

public class TerminalHUD : PimpedMonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
    private GameObject terminalHUD;

    private TerminalStat activeTerminal = null;
#pragma warning restore 0649

    private void Update() {
        UpdateTerminalWeaponStatus();
    }

    #region Enable and Disable
    public void EnableTerminalHUD( TerminalStat term ) {

        activeTerminal = term;

        //Set the weapon name labels
        firstWeaponLabel.text = activeTerminal.weapon1.name;
        secondWeaponLabel.text = activeTerminal.weapon2.name;
        thirdWeaponLabel.text = activeTerminal.weapon3.name;

        terminalHUD.SetActive( true );

        //Enable this script
        enabled = true;

    }
    public void DisableTerminalHUD() {

        terminalHUD.SetActive( false );

        activeTerminal = null;

        firstWeaponLabel.text = "Weapon 1";
        secondWeaponLabel.text = "Weapon 2";
        thirdWeaponLabel.text = "Weapon 3";

        dockingRangeIndicator.SetActive( false );

        //Disable this script
        enabled = false;

    }
    #endregion Enable and Disable

    #region Docking Indicator
#pragma warning disable 0649
    [SerializeField]
    private GameObject dockingRangeIndicator;
#pragma warning restore 0649

    public void DockingRangeChange( bool inRange ) {
        dockingRangeIndicator.SetActive( inRange );
    }
    #endregion Docking Indicator


    #region Terminal Weapons
#pragma warning disable 0649
    [SerializeField, Group( "Weapons" )]
    private UILabel firstWeaponLabel, secondWeaponLabel, thirdWeaponLabel;
    [SerializeField, Group( "Weapons" )]
    private UILabel firstWeaponAmmoCounter, secondWeaponAmmoCounter, thirdWeaponAmmoCounter;
    [SerializeField, Group( "Weapons" )]
    private UITexture firstWeaponHeatDisplay, secondWeaponHeatDisplay, thirdWeaponHeatDisplay;
    [SerializeField, Group( "Weapons" )]
    private Color32 notOverheated, overheated;

    [SerializeField, Group( "Weapons" )]
    private Transform WeaponSelectionLabel;

    [SerializeField, Group( "Weapons" )]
    private GameObject selection1, selection2, selection3;
#pragma warning restore 0649

    //private TerminalWeapon[] weapons;
    //private int selectedWeapon = 1;

    private void UpdateTerminalWeaponStatus() {

        selection1.SetActive( activeTerminal.weapon1.selected );
        selection2.SetActive( activeTerminal.weapon2.selected );
        selection3.SetActive( activeTerminal.weapon3.selected );

        firstWeaponAmmoCounter.text = activeTerminal.weapon1.ammoDisplay;
        secondWeaponAmmoCounter.text = activeTerminal.weapon2.ammoDisplay;
        thirdWeaponAmmoCounter.text = activeTerminal.weapon3.ammoDisplay;

        if( activeTerminal.weapon1.overheat )
            firstWeaponHeatDisplay.color = overheated;
        else
            firstWeaponHeatDisplay.color = notOverheated;
        firstWeaponHeatDisplay.fillAmount = activeTerminal.weapon1.heatPercent;

        if( activeTerminal.weapon2.overheat )
            secondWeaponHeatDisplay.color = overheated;
        else
            secondWeaponHeatDisplay.color = notOverheated;
        secondWeaponHeatDisplay.fillAmount = activeTerminal.weapon2.heatPercent;

        if( activeTerminal.weapon3.overheat )
            thirdWeaponHeatDisplay.color = overheated;
        else
            thirdWeaponHeatDisplay.color = notOverheated;
        thirdWeaponHeatDisplay.fillAmount = activeTerminal.weapon3.heatPercent;


    }
    #endregion Terminal Weapons

}
