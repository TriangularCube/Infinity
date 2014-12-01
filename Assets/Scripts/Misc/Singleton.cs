using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : PimpedMonoBehaviour where T : PimpedMonoBehaviour{

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
