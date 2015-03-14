using UnityEngine;
using System.Collections;

public class TerminalHUD : PimpedMonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
    private GameObject terminalHUD;

    private Terminal activeTerminal = null;
#pragma warning restore 0649

    private void Update() {
        UpdateTerminalWeaponStatus();
    }

    private void Awake() {
        SetupWeapons();
    }

    #region Enable and Disable
    public void EnableTerminalHUD( Terminal term ) {

        activeTerminal = term;

        weapons = activeTerminal.weaponSelection;

        //Set the weapon name labels
        firstWeaponLabel.text = weapons[0].weaponName;
        secondWeaponLabel.text = weapons[1].weaponName;
        thirdWeaponLabel.text = weapons[2].weaponName;

        terminalHUD.SetActive( true );

        //Enable this script
        enabled = true;

    }
    public void DisableTerminalHUD() {

        terminalHUD.SetActive( false );

        activeTerminal = null;
        weapons = null;

        selectedWeapon = 1;

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
#pragma warning restore 0649


    private Vector3 firstWeaponPosition;
    private Vector3 secondWeaponPosition;
    private Vector3 thirdWeaponPosition;

    private void SetupWeapons() {
        //Setup Position Refrences for Weapon Labels
        firstWeaponPosition = firstWeaponLabel.transform.position;
        secondWeaponPosition = secondWeaponLabel.transform.position;
        thirdWeaponPosition = thirdWeaponLabel.transform.position;
    }


    private TerminalWeapon[] weapons;
    private int selectedWeapon = 1;

    private void UpdateTerminalWeaponStatus() {

        if( selectedWeapon != activeTerminal.selectedWeapon ) {

            selectedWeapon = activeTerminal.selectedWeapon;

            switch( selectedWeapon ) {
                case 1:
                    WeaponSelectionLabel.position = firstWeaponPosition;
                    break;
                case 2:
                    WeaponSelectionLabel.position = secondWeaponPosition;
                    break;
                case 3:
                    WeaponSelectionLabel.position = thirdWeaponPosition;
                    break;
                default:
                    throw new UnityException( "Weapon Selection is not the three weapons" );
            }

        }

        firstWeaponAmmoCounter.text = weapons[0].reserve.ToString();
        secondWeaponAmmoCounter.text = weapons[1].reserve.ToString();
        thirdWeaponAmmoCounter.text = weapons[2].reserve.ToString();

        if( weapons[0].isOverHeated )
            firstWeaponHeatDisplay.color = overheated;
        else
            firstWeaponHeatDisplay.color = notOverheated;
        firstWeaponHeatDisplay.fillAmount = weapons[0].currentHeat;

        if( weapons[1].isOverHeated )
            secondWeaponHeatDisplay.color = overheated;
        else
            secondWeaponHeatDisplay.color = notOverheated;
        secondWeaponHeatDisplay.fillAmount = weapons[1].currentHeat;

        if( weapons[2].isOverHeated )
            thirdWeaponHeatDisplay.color = overheated;
        else
            thirdWeaponHeatDisplay.color = notOverheated;
        thirdWeaponHeatDisplay.fillAmount = weapons[2].currentHeat;



    }
    #endregion Terminal Weapons

}
