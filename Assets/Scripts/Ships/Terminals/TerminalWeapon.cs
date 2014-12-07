using UnityEngine;
using System.Collections;

public abstract class TerminalWeapon : PimpedMonoBehaviour {

    [SerializeField]
    private string _weaponName;
    public string weaponName { get { return _weaponName; } }

    public abstract int reserve { get; }

	public abstract void Fire();
	
}
