using UnityEngine;
using Assets.Code.Interfaces;

namespace Assets.Code.States
{
	public class PS_Parthenon : IStateBase
	{
		private StateManager manager;

		public PS_Parthenon(StateManager managerRef)
		{
			manager = managerRef;
			if(Application.loadedLevelName != "05_Parthenon")
			{
				Application.LoadLevel("05_Parthenon");
			}
		}

		public void StateUpdate()
		{
			if(Input.GetKeyUp(KeyCode.F))
			{
				manager.SwitchState(new PS_Acropolis(manager));
			}
		}

		public void ShowIt()
		{
			//Debug.Log("Inside the Parthenon");
		}
	}
}