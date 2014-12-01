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
class Group extends System.Attribute {
	public var name:String;
    public var tooltip:String;

	public function Group(groupName:String) {
		this.name = groupName;
	}	

    public function Group(groupName:String, helpText:String) {
        this.name = groupName;
        this.tooltip = helpText;
    }
}