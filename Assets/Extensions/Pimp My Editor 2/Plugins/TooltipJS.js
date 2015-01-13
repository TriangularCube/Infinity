/*
	Because of compile order among scripts of different kinds
	(javascript, c# and boo)
	they each need an individual attribute. But it cannot
	be identical, since that causes multiple definitions
	of the same class in the completed dll.
*/

import System;
#pragma strict

@script AttributeUsageAttribute(AttributeTargets.Field | AttributeTargets.Class)
class Tooltip extends System.Attribute {
	public var tooltip:String;
	public function Tooltip(tooltip:String) {
		this.tooltip = tooltip;
	}	
}