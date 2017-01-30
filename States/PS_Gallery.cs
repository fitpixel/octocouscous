using UnityEngine;
using Assets.Code.Interfaces;

namespace Assets.Code.States
{
	public class PS_Gallery : IStateBase
	{
		private StateManager manager;
		
		public PS_Gallery (StateManager managerRef)
		{
			manager = managerRef;
			if(Application.loadedLevelName != "06_PictureGallery")
			{
				Application.LoadLevel("06_PictureGallery");
			}
		}

		public void StateUpdate()
		{
			if(Input.GetKeyUp(KeyCode.E))
			{
				manager.SwitchState(new PS_Acropolis(manager));
			}
		}

		public void ShowIt()
		{
			//Debug.Log("Inside the Gallery");
		}

	}
}