using UnityEngine;
using System.Collections;

namespace Assets.Code.States
{

	public class InventoryManager : MonoBehaviour {
		public bool iMode = false;
		//variable for the player
		private GameObject player;
		internal GameObject controlCenter;
		internal GameObject firstPerson;
		public GameObject FPC;
		public GameObject FPCCamera;

		
		void Start () 
		{
			GetComponent<Camera>().enabled = false;
			controlCenter = GameObject.Find("GameManager"); //access game manager
			iMode = controlCenter.GetComponent<GameData>().iMode;			
		}		
		
		void Update () 
		{		
			FPC = GameObject.FindWithTag("Player");
			FPCCamera = GameObject.FindWithTag("MainCamera");

			if(Input.GetKeyDown("i"))
			{
				ToggleMode();
			}
		}

		public void ToggleMode()
		{
			if(iMode)
			{
				GetComponent<Camera>().enabled = false;

				FPC.GetComponent<CharacterMotor>().enabled = true;
				FPC.GetComponent<FPSAdventurerInputController>().enabled = true;
				FPC.GetComponent<MouseLookRestricted>().enabled = true;
				FPCCamera.GetComponent<MouseLookRestricted>().enabled = true;
				iMode = false;
				controlCenter.GetComponent<GameData>().iMode = false; //this is here to inform the game manager, because code is stupid and needs to be walked through everything
				return; //after doing the shit above, get out of the function
			}
			else
			{
				GetComponent<Camera>().enabled = true;
				FPC.GetComponent<CharacterMotor>().enabled = false;
				FPC.GetComponent<FPSAdventurerInputController>().enabled = false;
				FPC.GetComponent<MouseLookRestricted>().enabled = false;
				FPCCamera.GetComponent<MouseLookRestricted>().enabled = false;
				iMode = true;
				controlCenter.GetComponent<GameData>().iMode = true;
				return;
			}
		}
	}
}
