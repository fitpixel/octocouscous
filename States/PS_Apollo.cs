using UnityEngine;
using Assets.Code.Interfaces;

namespace Assets.Code.States
{
	public class PS_Apollo : IStateBase
	{
		private StateManager manager;

		public PS_Apollo(StateManager managerRef)
		{
			manager = managerRef;
			if(Application.loadedLevelName != "04_Apollo")
			{
				Application.LoadLevel("04_Apollo");
			}
		}

		public void StateUpdate()
		{
			if(Input.GetKeyUp(KeyCode.Keypad2))
			{
				manager.SwitchState(new PS_Acropolis(manager));
			}

			if(Input.GetKeyUp(KeyCode.Keypad3))
			{
				manager.SwitchState(new PS_Aphaia(manager));
			}
		}

		public void ShowIt()
		{
			//Debug.Log("In the Sanctuary of Apollo");
		}
	}
}
