using UnityEngine;
using Assets.Code.Interfaces;

namespace Assets.Code.States
{
	public class PS_Museum : IStateBase
	{
		//Variable to call on a different class; I need to be able to call on state mangager because I will be switiching states
		private StateManager manager;

		public PS_Museum (StateManager managerRef)
		{
			manager = managerRef;
			if(Application.loadedLevelName != "01_Museum")
			{
				Application.LoadLevel("01_Museum");
			}
		}

		void Start()
		{
			
		}

		public void StateUpdate()
		{
			if(Input.GetKeyUp (KeyCode.UpArrow))
			{
				manager.SwitchState(new PS_Acropolis(manager));
			}
		}

		public void ShowIt()
		{
			//Debug.Log("In Museum State");
		}
	}
}
