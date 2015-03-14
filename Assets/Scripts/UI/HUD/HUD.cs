using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Netplayer = TNet.Player;

public class HUD : Singleton<HUD> {

#pragma warning disable 0649
    [SerializeField]
    private LaunchMenu launchMenu;

    [SerializeField]
    private TerminalHUD terminalHUD;

    [SerializeField]
    private TargetingHUD targetingHUD;

    [SerializeField]
    private FlagshipHUD flagshipHUD;
#pragma warning restore 0649

    void Update(){

		//SuppressHUD();

	}

    #region Supression
    private bool suppressHUD = false;

#pragma warning disable 0649
    [SerializeField, Group( "Fade Values" )]
    private UIPanel panel;
    [SerializeField, Group( "Fade Values" )]
    private float SuppressedAlpha = 0.2f;
    [SerializeField, Group( "Fade Values" )]
    private float UnsupressedAlpha = 1f;
#pragma warning restore 0649

    //This needs to be reimplemented
    private void SuppressHUD() {

        if( suppressHUD )
            panel.alpha = SuppressedAlpha;
        else
            panel.alpha = UnsupressedAlpha;

    }
    #endregion Supression

    #region HUD Hooks
    public void AllyShipLaunched( Terminal term ) {
        //Turn indicator on
        targetingHUD.TurnOnAllyIndicator( term );
    }

    
    public void AllyShipDocked( Terminal term ) {
        //Turn indicator off
        targetingHUD.TurnOffAllyIndicator( term );
    }

    public void PlayerShipLaunched( Terminal term ) {
        //Turn all Flagship HUD off
        launchMenu.TurnMenuOff();
        launchMenu.enabled = false;

        //Turn Terminal HUD on
        terminalHUD.EnableTerminalHUD( term );
    }

    public void PlayerShipDocked() {
        //Turn Terminal HUD off
        terminalHUD.DisableTerminalHUD();

        //Enable the Launch Menu
        launchMenu.enabled = true;
    }

    //TODO
    public void RoleAssigned( /*Some Role*/ ) {

        if( flagshipHUD ) return;//DEBUG
        //flagshipIndicator.gameObject.SetActive( false );

    }

    public ShipSelectButton RequestNewShipButton( Terminal term ) {
        return launchMenu.RequestNewShipButton( term );
    }

    public void DockingRangeChange( bool inRange ) {
        terminalHUD.DockingRangeChange( inRange );
    }
    #endregion HUD Hooks

    //For use of ShipSelectButton
    //OK for now. Might change implementation later
    public LaunchMenu getLaunchMenu() {
        return launchMenu;
    }
   

}
