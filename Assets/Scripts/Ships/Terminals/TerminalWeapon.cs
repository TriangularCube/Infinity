using UnityEngine;
using System.Collections;

public abstract class TerminalWeapon : TNBehaviour {

#pragma warning disable 0649
    //Weapon Name
    [SerializeField]
    private string _weaponName = "Generic Weapon";
    public string weaponName { get { return _weaponName; } }

    [SerializeField]
    protected Terminal terminal;
#pragma warning restore 0649

    //Ammo Reserves
    public abstract int reserve { get; }

    #region Heat Handling
    protected float _currentHeat = 0f;
    /* <summery>
     * The current heat value as a percentage of max Heat Capacity
     */ 
    public float currentHeat { get { return _currentHeat / heatCapacity; } } //This returns a percentage

    protected bool _overHeated = false;
    public bool isOverHeated { get { return _overHeated; } }

    [SerializeField]
    protected float heatCapacity = 100f;
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

    #region Sync
    [SerializeField]
    private bool shouldSync = false;

    void OnEnable() {

        if( !TNManager.isHosting && shouldSync ) StartCoroutine( Sync() );

    }

    IEnumerator Sync() {
        while( true ) {
            SendData();
            yield return new WaitForSeconds( 1f / SessionManager.instance.maxNetworkUpdatesPerSecond );
        }
    }

    protected abstract void SendData();
    #endregion Sync
}
