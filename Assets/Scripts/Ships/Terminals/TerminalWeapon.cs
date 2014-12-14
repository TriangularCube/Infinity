using UnityEngine;
using System.Collections;

public abstract class TerminalWeapon : PimpedMonoBehaviour {

#pragma warning disable 0649
    //Weapon Name
    [SerializeField]
    private string _weaponName = "Generic Weapon";
    public string weaponName { get { return _weaponName; } }
#pragma warning restore 0649

    //Ammo Reserves
    public abstract int reserve { get; }

    #region Heat Handling
    //Overheating
    protected float _currentHeat = 0f;
    public float currentHeat { get { return _currentHeat / maxHeatBeforeOverheat; } } //This returns a percentage

    protected bool _overHeated = false;
    public bool isOverHeated { get { return _overHeated; } }

    [SerializeField]
    protected float maxHeatBeforeOverheat = 100f;
    [SerializeField]
    protected float heatGeneratedPerTick = 1f;
    [SerializeField]
    protected float heatSinkPerTick = 2f;

    protected static void HeatSink( ref float currentHeat, float heatSink, ref bool overHeat ) {

        currentHeat -= heatSink;

        if( currentHeat <= 0f ){

            currentHeat = 0f;

            if( overHeat ) {

                overHeat = false;

            }

        }

    }

    protected static void HeatHandling( ref float currentHeat, float heatToAdd, float maxHeat, ref bool overHeat ) {

        currentHeat = Mathf.Clamp( currentHeat + heatToAdd, 0f, maxHeat );
        if( currentHeat == maxHeat ) {
            overHeat = true;
        }

    }
    #endregion

    //The specific implementation of firing the weapon is handled in their own scripts
	public abstract void Fire();
	
}
