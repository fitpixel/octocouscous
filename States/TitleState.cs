using UnityEngine;
using Assets.Code.Interfaces;

namespace Assets.Code.States
{
	public class TitleState : IStateBase
	{
		//Variable to call on another class
		private StateManager manager;
		//Variables for Player
		private GameObject player;
		//private PlayerControl controller;

		public TitleState(StateManager managerRef)
		{
			manager = managerRef;
			if(Application.loadedLevelName != "00_Title")
			{
				Application.LoadLevel("00_Title");
			}

			//player = GameObject.Find("Player");
			//controller = player.GetComponent<PlayerControl>();
		}

		public void StateUpdate()
		{
			//Title State needs to switch to the Museum State, and only the Museum State (right now)
			if(Input.GetKeyUp(KeyCode.Space))
			{
				manager.SwitchState(new PS_Museum(manager));
			}
		}
		
		public void ShowIt()
		{
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), manager.gameDataRef.titleScreen, ScaleMode.StretchToFill);

			if(Input.anyKeyDown)
			{
				manager.SwitchState(new PS_Museum(manager));
			}
			//Debug.Log("In Title State");
		}
	}
}