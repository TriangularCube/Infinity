using UnityEngine;
using System.Collections;

public abstract class TerminalWeapon : PimpedMonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
    private string weaponName = "Generic Weapon";

    [SerializeField]
    protected int ammoCapacity = 100;
#pragma warning restore 0649

    protected TerminalWeaponStat stat;

    public void WeaponStatLink( TerminalWeaponStat slot ) {
        stat = slot;

        stat.name = weaponName;
        stat.ammo = ammoCapacity;
        stat.overheat = false;
    }



    #region Heat Handling
    protected float currentHeat = 0f;

    //Heat stats
    [SerializeField]
    protected float maxHeat = 100f;
    [SerializeField]
    protected float heatSinkPerTick = 2f;

    //Called to process heat sink
    protected static void HeatSink( ref float heat, float heatSink, ref bool overHeat ) {

        if( heat == 0f ) {
            return;
        }

        heat -= heatSink;

        if( heat <= 0f ){

            heat = 0f;
            overHeat = false;

        }

    }


    //Called to process heat generation
    protected static void HeatHandling( ref float heat, float heatToAdd, float maxHeat, ref bool overHeat ) {

        heat += heatToAdd;
        if( heat >= maxHeat ) {
            heat = maxHeat;
            overHeat = true;
        }

    }
    #endregion Heat Handling

    void OnDisable() {
        stat = null;
    }

}
