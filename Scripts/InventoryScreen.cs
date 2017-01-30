using UnityEngine;
using System.Collections;

public class InventoryScreen : MonoBehaviour {

	void Start () 
	{
		//set the GUI texture to be the same size as the screen on startup - though this prolly isn't needed
		//guiTexture.pixelInset = new Rect(0,0,Screen.width, Screen.height);
	}
	
	public IEnumerator OnMouseDown() 
	{
		GameObject.Find("Camera Inventory").SendMessage("ToggleMode");
		yield return new WaitForSeconds(.25f);
	}
}
