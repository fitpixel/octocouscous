using UnityEngine;
using Assets.Code.Interfaces;

namespace Assets.Code.States
{
	public class PS_Aphaia : IStateBase
	{
		private StateManager manager;

		public PS_Aphaia (StateManager managerRef)
		{
			manager = managerRef;
			if(Application.loadedLevelName != "03_Aphaia")
			{
				Application.LoadLevel("03_Aphaia");
			}
		}

		public void StateUpdate()
		{
			if(Input.GetKeyUp(KeyCode.Keypad2))
			{
				manager.SwitchState(new PS_Acropolis(manager));
			}

			if(Input.GetKeyUp(KeyCode.Keypad4))
			{
				manager.SwitchState(new PS_Apollo(manager));
			}
		}

		public void ShowIt()
		{
			//Debug.Log("In the Temple of Aphaia");
		}
	}
}
