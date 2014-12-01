/*
	Because of compile order among scripts of different kinds
	(javascript, c# and boo)
	they each need an individual attribute. But it cannot
	be identical, since that causes multiple definitions
	of the same class in the completed dll. 
*/
using System;

[AttributeUsage(AttributeTargets.Field)] 
public class GroupAttribute : System.Attribute
{
    public readonly string name;
    public readonly string tooltip;

    public GroupAttribute(string groupName)
    {
        this.name = groupName;
        this.tooltip = null;
    }

    public GroupAttribute(string groupName, string helpText)
    {
        this.name = groupName;
        this.tooltip = helpText;
    }
}
