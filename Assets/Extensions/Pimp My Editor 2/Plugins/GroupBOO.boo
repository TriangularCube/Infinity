/*
	Because of compile order among scripts of different kinds
	(javascript, c# and boo)
	they each need an individual attribute. But it cannot
	be identical, since that causes multiple definitions
	of the same class in the completed dll. 
*/
import System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
class GroupedAttribute (System.Attribute): 
	
	public name as string;
	public tooltip as string
	
	def constructor(groupName as string):
		super()
		name = groupName

	def constructor(groupName as string, helpText as string):
		super()
		name = groupName
		tooltip = helpText