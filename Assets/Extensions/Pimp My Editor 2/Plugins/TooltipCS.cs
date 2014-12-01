/*
	Because of compile order among scripts of different kinds
	(javascript, c# and boo)
	they each need an individual attribute. But it cannot
	be identical, since that causes multiple definitions
	of the same class in the completed dll. 	
*/

using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
public class TooltipAttribute : System.Attribute
{
	public readonly string tooltip;
	
	public TooltipAttribute(string tooltip)
	{
		this.tooltip = tooltip;
	}
}