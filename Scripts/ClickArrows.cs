using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickArrows : MonoBehaviour {

	internal GameObject controlCenter; 
	internal int gridOffset;	
	internal float amount; //make global variable because of the etra function
	internal GameObject inventoryItems;
	//visibility for the grid
	internal int gridPosition;
	internal bool isActive = false;
	
	void Start () 
	{

		inventoryItems = GameObject.Find("InventoryItems");
		controlCenter = GameObject.Find("GameManager");
		gridOffset = 90;
		ArrowState(isActive); //update the arrow opacity
	}
	
	public void OnMouseEnter() 
	{	
		if(!isActive)
			return; //skip the mouse-over functionality if the arrow is not active

		GetComponent<GUITexture>().color = new Color(0.55f,0.55f,0.55f,1f); //brighten the texture on mouseover
	}

	public void OnMouseExit()
	{
		if(!isActive)
			return;

		GetComponent<GUITexture>().color = Color.gray; //return to normal
	}

	public void OnMouseDown()
	{
		if(!isActive)
			return; //skip this too if the arrow is now active

		if(this.name == "ArrowLeft")
		{
			string shiftDirection = "left";	
			ShiftGrid(shiftDirection);		
		}
		else 
		{
			string shiftDirection = "right";
			ShiftGrid(shiftDirection);
		}	
	}
	public void ShiftGrid( string shiftDirection)
	{
		gridPosition = controlCenter.GetComponent<GameData>().gridPosition; //gets the latest grid position
		amount = 1.0f * gridOffset/Screen.width;


		if(shiftDirection == "left")
		{
			//hide the column on the left, send its top element
			controlCenter.SendMessage("HideColumn", gridPosition * 3);
			gridPosition --; //increment the gridPosition counter by 1			
			controlCenter.SendMessage("ShowColumn", gridPosition * 3 + 14); //show the column on the right, send its top element
			//activeate the right arrow
			GameObject.Find("ArrowRight").SendMessage("ArrowState", true);

			//if there are no more columns to the right, disable the left arrow 
			int iLength = controlCenter.GetComponent<GameData>().currentInventoryObjects.Length;

			if(gridPosition == 0)
			{
				ArrowState(false); //deactivate the arrow
			}
		}
		else
		{
			amount = -amount;
			//hide the column on the right
			controlCenter.SendMessage("HideColumn", gridPosition * 3 * 14);
			gridPosition ++; //decrement the gridPosition counter by 1
			controlCenter.SendMessage("ShowColumn", gridPosition * 3); //show the column on the left
						
			//Activate the left arrow
			GameObject.Find("ArrowLeft").SendMessage("ArrowState", true);
			
			
		}

		inventoryItems.transform.position = new Vector3(inventoryItems.transform.position.x + amount, inventoryItems.transform.position.y);					

		controlCenter.GetComponent<GameData>().gridPosition = gridPosition; //the new grid position sent off to game manager
		//inventoryItems.SendMessage("MovingGrid", true);		
	}

	public void ArrowState (bool newState)
	{
		isActive = newState;
		if(isActive)
		{
			GetComponent<GUITexture>().color = new Color(0.5f,0.5f,0.5f,1.0f); //full opacity
		}
		else
		{
			GetComponent<GUITexture>().color = new Color(0.5f,0.5f,0.5f,.2f);//20% opacity
		}
	}
}
