using UnityEngine;
using Assets.Code.Interfaces;

namespace Assets.Code.States
{
	public class PS_Acropolis : IStateBase
	{
		private StateManager manager;
		
		public PS_Acropolis (StateManager managerRef)
		{
			manager = managerRef;
			if(Application.loadedLevelName != "02_Acropolis")
			{
				Application.LoadLevel("02_Acropolis");
			}

		}

		//Test for inventory

		public void StateUpdate()
		{
			if(Input.GetKeyUp(KeyCode.UpArrow))
			{
				manager.SwitchState(new PS_Museum(manager));
			}

			if(Input.GetKeyUp(KeyCode.Keypad4))
			{
				manager.SwitchState(new PS_Apollo(manager));
			}

		}

		public void ShowIt()
		{
			//Debug.Log("In Acropolis State");
		}


	}
}

