using UnityEngine;
using System.Collections;

namespace Assets.Code.States
{
	public class PlayerControl : MonoBehaviour 
	{
		//Calling on StateManager
		private StateManager manager;
		//Variables for Player
		private GameObject player;
		private PlayerControl controller;

		public PlayerControl(StateManager managerRef)
		{
			manager = managerRef;

			player = GameObject.Find("Player");
			controller = player.GetComponent<PlayerControl>();
		}
		void Start () 
		{

		}
		
		void PlayerUpdate () //This function can be called from any states' StateUpdate function
		{

		
		}

		void OnTriggerStay(Collider other)
		{
			if(other.gameObject.tag == "MuseumWalkSpeed")
			{
					GetComponent<CharacterMotor>().movement.maxForwardSpeed = 3;
					GetComponent<CharacterMotor>().movement.maxSidewaysSpeed = 3;	
					GetComponent<CharacterMotor>().movement.maxBackwardsSpeed = 3;						;
						
			}

			if(other.gameObject.tag == "DoorPar" && Input.GetKeyDown(KeyCode.E))
			{
					manager.SwitchState(new PS_Parthenon(manager));
					Debug.Log("Inside the Parthenon");				
			}

			if(other.gameObject.tag == "DoorGal" && Input.GetKeyDown(KeyCode.E))
			{
				manager.SwitchState(new PS_Gallery(manager));
				Debug.Log("Inside the Gallery");
			}	

			if(other.gameObject.tag == "DoorAcr" && Input.GetKeyDown(KeyCode.E))
			{
				manager.SwitchState(new PS_Acropolis(manager));
				Debug.Log("At the Acropolis");				
			}	
		}
	}
}