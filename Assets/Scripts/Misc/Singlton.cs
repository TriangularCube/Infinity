using UnityEngine;
using System.Collections;

public class Singlton<T> : MonoBehaviour where T : MonoBehaviour{

	protected static T _instance;

	public static T instance{
		get{
			return _instance;
		}
	}

	protected virtual void Awake(){
		_instance = this as T;
	}
}
