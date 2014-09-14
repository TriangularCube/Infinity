using UnityEngine;
using System.Collections;

public abstract class Terminal : Ship {

	public TNet.Player pilot{ get; set; }

	public override bool ContainsPlayer (TNet.Player check)
	{
		return pilot == check;
	}

}
