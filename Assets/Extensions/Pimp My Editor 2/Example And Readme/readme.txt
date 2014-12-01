Pimp My Editor

A system of editor inspector improvements to improve workflow. Split into 4 components:

* Groups
* Getter/Setter
* Tooltips
* Sort


GROUPS
A set of attributes you can add to your fields, to make them appear in the editor as a logically connected set of properties.
c#:	[Group("Group Name")]
js:	@Group("Group Name")]
boo:	[Grouped("Group Name")]



GETTER/SETTER
Exposes properties to the inspector. Changing properties executes code, so be vary of this.
You have the option to force hide or force show properties. Untagged properties are shown/hidden through an inspector option at the bottom of each inspector.

c#:	[GetSetVisible]	or [GetSetHidden]
js:	@GetSetVisible	or @GetSetVisible
boo:	[GetterSetterVisible]	or [GetterSetterHidden]



TOOLTIPS
Show tooltips or hovertips on your inspector properties.

c#:	[Tooltip("Your tip here")]	or [Hovertip("Your tip here")]
js:	@Tooltip("Your tip here")]	or @Hovertip("Your tip here")]
boo:	[Tip("Your tip here")]		or [Hover("Your tip here")]



SORT
Sorts your inspector properties by name. Can be toggled on or off through an inspector option at the bottom of each inspector.

===============

HOW TO:

To get it to work, your class needs to inherit from PimpedMonoBehaviour.

c#: public class Yourclass : PimpedMonoBehaviour { /* c# stuff */ }
js: class YourClass extends PimpedMonoBehavour { /* js stuff */ }
boo: class YourClass(PimpedMonoBehaviour): /* boo stuff */

NB: To get it to work with javascript, the PimpedMonoBehaviour.cs script needs to be moved to a root Plugins folder (psudo: Project Folder/Assets/Plugins)
