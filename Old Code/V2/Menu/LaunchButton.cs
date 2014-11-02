using UnityEngine;
using System.Collections;

public class LaunchButton : MonoBehaviour {

	public LaunchMenu menu;

	public void OnClick(){
		menu.LaunchTerminal ();
	}
}
