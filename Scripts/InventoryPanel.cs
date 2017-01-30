using UnityEngine;
using System.Collections;

public class InventoryPanel : MonoBehaviour {

	internal GameObject controlCenter;
	internal Texture defaultCursor;
	internal Texture currentCursor;

	void Start () 
	{
		controlCenter = GameObject.Find("GameManager");
		defaultCursor = controlCenter.GetComponent<GameData>().defaultCursor;
	}

	void OnMouseDown() 
	{
		currentCursor = controlCenter.GetComponent<GameData>().currentCursor;

		if(currentCursor == defaultCursor)
		{
			return;
		}
		else
		{
			//there is an action icon as cursor, so process it
			GameObject addObject = GameObject.Find(currentCursor.name);
			Debug.Log(addObject);
			Debug.Log(currentCursor.name);

			//update the icons current state to in inventory, 1, in the Interactor script
			addObject.GetComponent<Interactor>().currentState = 1;
			//after storing the cursors texture, reset the cursor
			controlCenter.SendMessage("ResetCursor");
			Debug.Log("Did you get past Rest Cursor?");
			//add object to inventory
			controlCenter.SendMessage("AddToInventory", addObject);
		}
	}
}
