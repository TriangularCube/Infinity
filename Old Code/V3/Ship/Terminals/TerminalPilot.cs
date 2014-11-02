using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerminalPilot : ShipControl {

	protected virtual void Start(){
		SortAndTrimWeapons();
	}
	
	protected virtual void Update(){
		//Back out if our mouse isn't locked
		//HACK This might need to be changed later
		if ( !Screen.lockCursor ) return;

		//HACK Fix later
		if (Input.GetButtonDown ("Dock")) {
			GetComponent<Terminal>().InitiateDocking();
		}

		ResolveWeaponSwitch ();

		ResolveWeaponFire ();
	}

	#region Fire Control
	//Our list of active weapons
	List<TerminalWeapon> activeWeaponList = new List<TerminalWeapon>();
	//Our current weapon
	int currentWeapon = 0;
	
	//Scrolling mousewheel cooldown counter
	bool weaponSwitchCooldown = false;
	
	[SerializeField]
	//Weapon Switch cooldown time
	float weaponSwitchCooldownTime = 0.3f;
	
	
	void SortAndTrimWeapons(){
		if (activeWeaponList.Count < 1) {
			Debug.LogError( "What? This terminal has no usable weapons? Check if this is right." );
		}
		
		//TODO Sort the weapons list
		
		activeWeaponList.TrimExcess ();
	}
	
	//All active weapons will call this to register themselves
	public void RegisterWeapon( TerminalWeapon weapon ){
		activeWeaponList.Add (weapon);
	}

	void ResolveWeaponSwitch(){

		//To switch weapons, first check check if we activated the switch, then check if we're in cooldown 
		if ( Input.GetAxis ( "Mouse ScrollWheel" ) != 0f && !weaponSwitchCooldown ) {
			//Do switch
			if( Input.GetAxis( "Mouse ScrollWheel" ) > 0f ){
				currentWeapon++;
				
				//Overflow check
				if( currentWeapon > activeWeaponList.Count - 1){
					currentWeapon = 0;
				}
			} else if( Input.GetAxis( "Mouse ScrollWheel") < 0f ){
				currentWeapon--;
				
				//Overflow Check
				if( currentWeapon < 0 ){
					currentWeapon = activeWeaponList.Count - 1;
				}
			}
			
			//Activate cooldown
			StartCoroutine( Cooldown() );
			
		}

	}

	IEnumerator Cooldown(){
		weaponSwitchCooldown = true;
		yield return new WaitForSeconds( weaponSwitchCooldownTime );
		weaponSwitchCooldown = false;
	}

	void ResolveWeaponFire(){
		
		//Fire the current weapon
		if( Input.GetButton( "Fire Weapon" ) && !stats.hyperThurst && !weaponSwitchCooldown ){
			activeWeaponList[currentWeapon].Fire();
		}
	}
	#endregion

	public virtual void OnLaunch( Quaternion facing ){}
}
