using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour {

	//Variables 
	public float pos;
	
	void Start () 
	{

	}
	

	void Update () 
	{
		transform.position = Vector2.zero;
		transform.localScale = Vector2.zero;
		Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		GetComponent<GUITexture>().pixelInset = new Rect(pos.x,pos.y,32,32);
		//guiTexture.pixelInset.x = pos.x;
		//guiTexture.pixelInset.y = pos.y - 32;
	
	}
}
