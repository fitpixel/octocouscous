using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour
{
	public GameObject controlCenter;
	public Texture2D titleScreen; //These assets are added via the inspector, not code
	public Texture2D cutScene1;	
	public Texture2D cutScene2;
	//cursor variables
	public Texture defaultCursor;
	internal Texture currentCursor;
	internal float pos;
	internal bool navigating;
	//mouseover and interactivity variables
	public Color mouseOverColor = new Color(1,0,1,1);
	internal Color currentCursorColor;
	//creating a timer for the cursor
	internal bool suppressPointer = false;
	//variable for inventory
	internal bool iMode = false;
	//inventory icon
	public Texture2D iButton;
	//array to hold the action objects for proccessing
	internal GameObject[] actionObjects = new GameObject[0];
	internal GameObject[] aObjects = new GameObject[0];
	//array to hold the inventory objects
	internal GameObject[] inventoryObjects = new GameObject[0];

	//for custom GUI skin
	public GUISkin customSkin;
	//for overridng the skin
	public GUIStyle customGUIStyle;
	//the flag used to suppress text
	public bool useText = true;
	public bool showText = false; //only is true if mouse is over whatever object it is assigned to
	//flags for accessing the metadata of each object
	public bool useLongDesc = true;
	internal bool showShortDesc = true;
	internal bool showLongDesc = true;
	internal bool showActionMsg = false;
	//to clear the above values
	internal string shortDesc = " ";
	internal string longDesc = " ";
	internal string actionMsg = " ";
	//for the "in range" message 
	internal bool inRange;
	///name of the last action object to turn on the timer
	internal string actionObject;
	internal bool pickTimer;
	internal float pickTimerTime;
	internal string tempActionObject;
	

	//for da inventory...array of object currently in inventory
	internal GameObject[] currentInventoryObjects = new GameObject[0];
	internal GameObject[] iObjects = new GameObject[0];
	internal int iLength;
	//inventory layout 
	internal int iconSize = 60;
	public int gridPosition = 0; //default position for the inventory grid
	internal bool visibility;
	//test variable for cursor reset
	internal string cursor;

	void Awake()
	{
		Dialoguer.Initialize();
	}
	void Start()
	{
		controlCenter = GameObject.Find("GameManager");
		//Hide the hardware cursor and make the current cursor the default cursor that you set in the inspector
		Cursor.visible = false; //Allegedly at build time the cursor will hide itself
		//Screen.lockCursor = false;
		currentCursor = defaultCursor;
		currentCursorColor = Color.white;

		
	}

	void OnLevelWasLoaded(int level)
	{
		if (level == 1)
		{
			//check for action objects
			//get a list of the gameObjects with the ActionObject tag
			aObjects = GameObject.FindGameObjectsWithTag("ActionObject");
			//redefine the ActionObject array with the number of elements in the list
			actionObjects = new GameObject[aObjects.Length];
			//save the action objects into the array for easy access when deactivated
			for (int i = 0; i < aObjects.Length; i++)
			{
				actionObjects[i] = aObjects[i];
				Debug.Log("The action objects in the scene: " + actionObjects[i].name);
				
			}

			//check for inventory objects
			//get a list of the gameObjects with the InventoryObjects tag
			iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");
			//redefine the invenory objects array with the number of elements in the list
			inventoryObjects = new GameObject[iObjects.Length];
			//save the action objects into the array for easy access when deactivated
			for (int k = 0; k < iObjects.Length; k++)
			{
				inventoryObjects[k] = iObjects[k];
				Debug.Log("The inventory objects in the scene: " + inventoryObjects[k].name);
				
			}


		
			//Inventory Array
			iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");
			//this is the start of a counter for ordering active inventory objects
			int element = 0;
			//redefine the current invetory array
		
			//save the inventory objects into the array for processing in iMode
			for(int i = 0; i < iObjects.Length; i++) 
			{

				if(iObjects[i].GetComponent<Interactor>().initialState == 1)
				{
					//assign the element number to the current object
					iObjects[i].GetComponent<Interactor>().iElement = element;
					element++; //increment the element
				}
				else 
				{
					iObjects[i].GetComponent<Interactor>().iElement = 100;
				}
				
				print(iObjects[i].GetComponent<Interactor>().name + " " + iObjects[i].GetComponent<Interactor>().iElement);

			}

			iLength = element; //save the number of state 1 objects (elements)
			OrderInventoryArray();

			iLength = currentInventoryObjects.Length;
			InventoryGrid(); //arrange the inventory icons

			if(gridPosition == 0)
			{
				 GameObject.Find("ArrowLeft").SendMessage("ArrowState", false);//deactivate the right arrow
			}
		}
	}
	void Update()
	{

		//This conditional is to determin if the player is navigating and when to hide the custom cursor
		if(Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetButton("Turn") || Input.GetButton("ML Enable"))
		{
			navigating = true;
		}
		else
		{
			navigating = false;
		}

		if(pickTimer && Time.time > pickTimerTime)
		{
			pickTimer = false;		
		}

		//This is for handling dropped cursors
		if(Input.GetMouseButtonDown(0) && currentCursor != defaultCursor && ! iMode) 
		{
			print(currentCursor);
			Debug.Log("Did you get to NoMatchReturn?");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			//bool didHit = Physics.Raycast(ray,hit); //did it hit anything?		

			if(Physics.Raycast(ray, out hit))
			{
				if(hit.transform.tag == "ActionObject")
				{
					return;
				}
				else
				{
					GameObject cursor = GameObject.Find(currentCursor.name);
									
					AddToInventory(cursor);				
					
					cursor.GetComponent<GUITexture>().enabled = true;
					
					cursor.GetComponent<Interactor>().currentState = 1;
					
					ResetCursor();

					print("Nothing of note..." + hit.transform.name);
				}
			}
		}
			
		
	}

	public void CursorColorChange(bool colorize)
	{
		if(colorize)
		{
			currentCursorColor = mouseOverColor;
		}
		else
		{
			currentCursorColor = Color.white;
		}

	

	}

	void OnGUI() //for future reference, this is basically the "update" of the GUI world; called once every frame
	{
		//for the inventory button
		if(GUI.Button(new Rect(5,Screen.height - 35,32,32), "I"))//This is where you could iButton for the texture
		{
			GameObject.Find("Camera Inventory").SendMessage("ToggleMode");
		}

		//For the GUI label
		GUI.skin = customSkin;
		if(useText)
		{
			if(showActionMsg)
			{
				GUI.Label(new Rect(Screen.width/2 - 300, Screen.height - 95, 600,35), actionMsg);
			}
			if(showText && !showActionMsg)
			{				
				if(useLongDesc)
				{
					if(showLongDesc && inRange)
					{
						GUI.Label(new Rect(Screen.width/2 - 250, Screen.height - 95, 500,35), longDesc, customGUIStyle);
					}
					if(showShortDesc)
					{
						GUI.Label(new Rect(Screen.width/2 - 250, Screen.height - 125, 500,35), shortDesc, customGUIStyle);
					}
				}

				//GUI.Label(new Rect(0,0,300,100), "This is the text string for a Label control");
				//GUI.Box(new Rect(Screen.width/2 - 250, Screen.height -65, 500,50), "This is a string for the box!");
				
			}

		}
	//so the mesh is still staying active once clicked on...fgiure out what do to about that
		//for the cursor
		if(!navigating && !suppressPointer)
		{	
			
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			GUI.color = currentCursorColor;
			GUI.DrawTexture(new Rect(pos.x,(Screen.height - pos.y),32,32), currentCursor);
			GUI.color = Color.white;
			
		}
	}

	public void EmergencyTimer(string dyingObject)
	{
		tempActionObject = dyingObject; //assign the name of the dying object to the temp var
		//start pick timer
		pickTimerTime = Time.time + 2.0f;
		pickTimer = true;
	}

	public void ResetCursor()
	{
		if(currentCursor != defaultCursor)
		{
			print(currentCursor);
			currentCursor = defaultCursor; // reset the cursor to default
			print(currentCursor);
		}
		else
		{
			return;
		}		
	}

	public void OrderInventoryArray()
	{
		//re-initialize array for new length
		currentInventoryObjects = new GameObject[iLength];

		//get a list of the gameObjects with the InventoryObject tag
		iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");

		//load the new array with the state 1 objects, according to their iElement numbers/order
		for( int e = 0; e < iObjects.Length; e++)
		{
			for(int i = 0; i < iObjects.Length; i++)
			{
				//if there is a match
				if(iObjects[i].GetComponent<Interactor>().iElement == e)
				{
					//add that object to the new array
					currentInventoryObjects[e] = iObjects[i];

					//tell it you're finished looking for that element number
					i = iObjects.Length;
				}
			}
		
			//print(currentInventoryObjects[e] + " * " + e);
		}		
	}

	public void InventoryGrid()
	{
		int xPos = -155 - iconSize; //adjust column offset start positioning according to icon
		int spacer = iconSize/2; //calculate the spacer size
		iLength = currentInventoryObjects.Length; //length of the array
		Debug.Log("I know you're here.");
		for(int k = 0; k < iLength; k = k + 3)
		{  
			Debug.Log(visibility);
			//calculate the column visibility for the top element, k
			if(k < gridPosition * 3 || k > gridPosition * 3 + 14) 
			{
				visibility = false;
				Debug.Log(visibility);
			}
			else 
			{
				visibility = true;
			}
			if(!visibility)
				HideColumn(k); //send the top row element for processing

			//row 1
			int yPos = 180 - iconSize/2;
			currentInventoryObjects[k].GetComponent<GUITexture>().pixelInset = new Rect(xPos, yPos, iconSize,iconSize);
			//row 2
			yPos = yPos - iconSize - spacer + 3;
			if(k + 1 < iLength)
			{
				currentInventoryObjects[k+1].GetComponent<GUITexture>().pixelInset = new Rect(xPos, yPos, iconSize,iconSize);
			}
			//row 3
			yPos = yPos - iconSize - spacer + 3;
			if(k + 2 < iLength)
			{
				currentInventoryObjects[k+2].GetComponent<GUITexture>().pixelInset = new Rect(xPos, yPos, iconSize,iconSize);
			}
 			//if elements need to be shown, do so after positioning
 			if(visibility)
 				ShowColumn(k);
			xPos = xPos + iconSize + 30; //update the column position for the next group
		}

		//for the arrows
		//if there are icons to the left of the grid, activate the right arrow
		if(gridPosition > 0)
		{
			GameObject.Find("ArrowRight").SendMessage("ArrowState", true);
		}
		else
		{
			GameObject.Find("ArrowRight").SendMessage("ArrowState", true);
		}

		//check overflow
		if(iLength > gridPosition * 3 + 15)
		{
			GameObject.Find("ArrowLeft").SendMessage("ArrowState", true);
		}
		else
		{
			GameObject.Find("ArrowLeft").SendMessage("ArrowState", false);
		}
	}

	public void ShowColumn(int topElement)
	{
		Debug.Log("ShowColumn Function");
		topElement = 0;
		//show the elements in the 2 rows for the top elements column
		currentInventoryObjects[topElement].GetComponent<GUITexture>().enabled = true;//row 1
		if(topElement >= iLength) 
			return;
		if(topElement + 1 < iLength)//row 2
		{
			currentInventoryObjects[topElement + 1].GetComponent<GUITexture>().enabled = true;
		}

		if(topElement + 2 < iLength) //row 3
		{
			currentInventoryObjects[topElement + 2].GetComponent<GUITexture>().enabled = true;
		}
	}

	public void HideColumn(int topElement)
	{
		Debug.Log("HideColumn Function");
		topElement = 0;
		//hide elements in the 3 rows for the top elements column
		currentInventoryObjects[topElement].GetComponent<GUITexture>().enabled = false; //row 1
		if(topElement >= iLength) 
			return;

		if(topElement + 1 < iLength) //row 2
		{
			
			currentInventoryObjects[topElement + 1].GetComponent<GUITexture>().enabled = false;
			Debug.Log(topElement);
		}

		if(topElement + 2 < iLength)
		{
			Debug.Log(topElement);
			currentInventoryObjects[topElement + 2].GetComponent<GUITexture>().enabled = false; //row 3
		}
	}
	public void AddToInventory(GameObject whichObject)
	{
		//update the object's element to be the new last element
		controlCenter.GetComponent<Interactor>().iElement = iLength;
		iLength = iLength + 1;
		//update the array
		OrderInventoryArray();
		//update the grid
		InventoryGrid();

		if(iMode && iLength > 15)
		{
			//shift the gird to the right unitl you get to the end where the new object was added
			while((gridPosition * 3 + 15) < iLength)
			{
				GameObject.Find("ArrowLeft").SendMessage("ShiftGrid", "left");
			}
		}
	}

	public void RemoveFromInventory(GameObject whichObject)
	{
		
		print("Removing " + whichObject.name + " from inventory.");
		// retrive the picked icon/object's inventory element number
		int iRemove = whichObject.GetComponent<Interactor>().iElement;
		//get the list of the active gameObjects (this should activate proerly everytime the function is called)
		iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");
		//go through the list and decrement the iElement 
		for(int i = 0; i < iObjects.Length; i++)
		{
			//only if its current state is state 1 (in inventory)
			if(iObjects[i].GetComponent<Interactor>().iElement > iRemove)
			{
				iObjects[i].GetComponent<Interactor>().iElement --;
				//decrement all the iElements
			}
		}

		iLength = iLength - 1; //the length of the inventory objects is now lessened by 1
		whichObject.GetComponent<Interactor>().iElement = 100;
		//update the array
		OrderInventoryArray();
		//update grid
		//if the thrid column is empty, shift the grid to the right
		if((gridPosition * 3 + 12) >= iLength && iLength >= 12)
		{
			GameObject.Find("ArrowRight").SendMessage("ShiftGrid", "right");
		}

		InventoryGrid();		
	}

	public void NoMatchReturn()//run this method if the cursor has no effect on the item
	{
		print(currentCursor);
		Debug.Log("Did you get to NoMatchReturn?");
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		//bool didHit = Physics.Raycast(ray,hit); //did it hit anything?		

		if(Physics.Raycast(ray, out hit))
		{
			if(hit.transform.tag == "ActionObject")
			{
				return;
			}
			else
			{
				GameObject cursor = GameObject.Find(currentCursor.name);
								
				AddToInventory(cursor);				
				
				cursor.GetComponent<GUITexture>().enabled = true;
				
				cursor.GetComponent<Interactor>().currentState = 1;
				
				ResetCursor();

				print("Nothing of note..." + hit.transform.name);
			}
		}
	}
}	