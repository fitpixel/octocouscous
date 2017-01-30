using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectLookup : MonoBehaviour 
{

	//Variables for the lookup states
	public int state = 1;

	public string[] lookupState1 = new string[3]; 
	public string[] lookupState2 = new string[3]; 
	public string[] lookupState3 = new string[3]; 	
	
	internal string[] currentStateArray = new string[3]; 

	//Variables for the replies for each state
	public string[] repliesState1 = new string[3];
	public string[] repliesState2 = new string[3];
	public string[] repliesState3 = new string[3];

	public string[] genericReplies = new string[3]; //Generic reply for each state in case something werid happens
												   //Need one for each state
	internal string[] currentReplyArray = new string[3];//Holds the current reply to process
	//call the control center
	public GameObject controlCenter;
	public GameObject auxObject;

		
	// Update is called once per frame

	void Update () 
	{
	
	}

	void Start()
	{
		controlCenter = GameObject.Find("GameManager");
	}

	public void LookUpState(GameObject whichObject, int currentState, string picker)
	{	
		state = currentState;//assign current state to the state variable
		string matchCursor = picker; //temporary to hold the temp cursor texture name
		Debug.Log("Are you getting to the SwitchCase?");
		switch(state)
		{
			case 1:
			currentStateArray = lookupState1;
			currentReplyArray = repliesState1;
			break;

			case 2:
			currentStateArray = lookupState2;
			currentReplyArray = repliesState2;
			break;

			case 3:
			currentStateArray = lookupState3;
			currentReplyArray = repliesState3;
			break;
		}

		int element = 0;//variable to track the element number for a match
		bool match = false;
		Debug.Log("RESULTS FOR STATE " + state );
		
		foreach(string contents in currentStateArray)
		{
			Debug.Log("Contents of state:" + contents);
			
			string[] readString = contents.Split(new char[] {','});
			
			Debug.Log("Elements in Array for State " + state + " = " + readString.Length);
			Debug.Log("Cursor = " + readString[0]);
			Debug.Log("New State = " + readString[1]);

			//Check for a cursor match with element 0 of the split
			if(readString[0] == matchCursor)
			{
				Debug.Log("In readString.");

				match = true;
				//print(currentReplyArray[element]); //okay so it is showing up in the console
				controlCenter.GetComponent<GameData>().actionMsg = currentReplyArray[element];
				//get the new state, element 1 of the split, then convert the string to an int
				int nextState = int.Parse(readString[1]);
				//transition the object into the new state over in the Interactor script
				SendMessage("ProcessObject", nextState);
				//now read through the remaninder in pairs
				//iterate through the array starting at element 2 and incrementing by 2
				//as long as the counting variable i is less than the length of the array
				for (int i = 2; i < readString.Length; i = i+2) 
				{
					Debug.Log("Auxiliary object = " + readString[i]);
					Debug.Log(readString[i] + "'s new state = " + readString[i+1]);
					//assign the first peice of data in the pair to a temp variable for processing
					string tempS = readString[i];
					Debug.Log(tempS);
					//check for special cases here
					
					//find an activate the object using its name
					GameObject auxObject = CheckForActive(tempS);
					Debug.Log(tempS);
					//conver the new state from a string value to an integer for use
					int newState = int.Parse(readString[i+1]);
					//process the auxiliary objext into the new state
					auxObject.SendMessage("ProcessObject", newState, SendMessageOptions.DontRequireReceiver); // <----ERROR, says this line			
					
				}
			}

			element ++; //increment the counter by 1 for each
		}
		if(!match)		
		{	
			SendMessage("HandleNoMatchReplies", picker);
		}
	}

	public GameObject CheckForActive (string name)
	{		
		Debug.Log("Did you check for active objects?");
		Debug.Log(name);
		//check to see if the object is active before assigning it to the auxObject
		if(GameObject.Find(name))
		{

			auxObject = GameObject.Find(name);
			Debug.Log(auxObject);
			return auxObject;//return the gameObject to where the function was called
		}

		else 
		{
			//for the action objects
			Debug.Log("Into the else statement?");
			GameObject[] actionObjects = new GameObject[0];			
	
			actionObjects = controlCenter.GetComponent<GameData>().actionObjects;
			for (int x = 0; x < actionObjects.Length; x++)
			{
				Debug.Log(actionObjects + "in the for loop");
				if(actionObjects[x].gameObject.name == name)//if there is a match for the name
				{
					Debug.Log("Into the if for the for loop? NAME SHOULD MATCH");
					actionObjects[x].gameObject.SetActive(true);
					auxObject = GameObject.Find(name); //assign the newly activated object
				}
				Debug.Log(auxObject);
			}			
			
			//for the inventory objects
			Debug.Log("Into the else statement?");
			GameObject[] inventoryObjects = new GameObject[0];

			Debug.Log(inventoryObjects);
			inventoryObjects = controlCenter.GetComponent<GameData>().inventoryObjects;
			for (int x = 0; x < inventoryObjects.Length; x++)
			{
				Debug.Log(inventoryObjects + "in the for loop");
				if(inventoryObjects[x].gameObject.name == name)//if there is a match for the name
				{
					Debug.Log("Into the if for the for loop? NAME SHOULD MATCH");
					inventoryObjects[x].gameObject.SetActive(true);
					auxObject = GameObject.Find(name); //assign the newly activated object
				}
				Debug.Log(auxObject);
			}
		}
			
		return auxObject;				
				
	}

	public IEnumerator HandleNoMatchReplies(string picker)
	{
		
		//picker = picker.ToLower();
		picker = picker.Substring(0,picker.Length - 4); //this line is to knock off the "icon" part of the name - comment out if you don't need it
		
		string tempObjectName = this.GetComponent<Interactor>().currentObjectName;

		string tempMsg = "The " + picker + " does not seem to effect the " + tempObjectName;
		
		controlCenter.GetComponent<GameData>().actionMsg = tempMsg;
		controlCenter.GetComponent<GameData>().showActionMsg = true;
		controlCenter.GetComponent<GameData>().NoMatchReturn();
		yield return new WaitForSeconds(2.0f);
		controlCenter.GetComponent<GameData>().showActionMsg = false;
			
	}
}	

