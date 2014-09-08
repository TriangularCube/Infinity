using UnityEngine;
using System.Collections.Generic;

public class PlayerProfile
{
	public string playerName { get; set; }
	public Dictionary<string, KeyCode> keycodes { get; set; }
	public float mouseSensitivityX, mouseSensitivityY;
}

