# octocouscous
Early experimentations with coding inventory, GUI and fooling with Raycasting. State machine based off of Sue Blackman, pre Unity 4.5.

# state machine notes

GAME NOTES

ARRAY INFORMATION FOR OBJECT METADATA

Location:

3D Objects:

	0: Object is not in scene
  
	1: Object is in scene
  
	2: Object is not pickable (but in scene)
  
2D Objects:

	0: Object is not in scene
  
	1: Object is in inventory
  
	2: Object is cursor (for placing)
  

Visibility:

	0: N/A (no transparency shader)
  
	1: Show at Start (fade in)
  
	2: Show at start, hide at end (fade in, fade out)
  
	3: Hide at end (fade out)

STATE INFORMATION FOR WORLD OBJECTS (acted on)

	0: Not in scene
  
	1: In scene, usable
  
	2: In scene, used
  
	3: Action completed or destroyed (out of scene)
  
	
STATE INFORMATION FOR INVENTORY OBJECTS (picked up, initiating actions)

	0: Not in scene
  
	1: In scene, pickable
  
	2: In scene, in inventory
  
	3: Consumed or destroyed (out of scene)

CURSOR STATES:

	0: Not in scene
  
	1: Is in inventory
  
	2: Is cursor
  
Once an item is in the cursor and combines with another object, it is no longer the same object, and therefore has no other states.

# reminders
Syntax: Something I found out - code outside of functions should stick to declaring types or specific values only, not abstract values that have to be pulled from another variable.

Custom Cursor: REMEBER TO CHANGE THE SIZE OF THE RECTANGLE INSIDE OF YOUR GAME DATA WHEN CHANGING
TEXTURES!

Any object that has to disappear when you click on it (basically any inventory object), will have to use case 0 of the handleVisibility function - meaning it disappears as soon as it is clicked on.
