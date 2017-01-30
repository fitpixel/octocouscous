using UnityEngine;
using System.Collections;

namespace Assets.Code.States
{
	public class Map : MonoBehaviour 
	{
		//access the State Manager
		private StateManager manager;
		//May need to access the player...? We'll see
		//Variable for the window
		public Rect mapWindow = new Rect(50, 50, 300, 600);
		
		public Map(StateManager managerRef)
		{
			manager = managerRef;		
		}
		
		void Awake()
		{
			useGUILayout = true;
		}
		void Update () 
		{
		
		}

		void OnGUI()
		{
			mapWindow = GUILayout.Window(0, mapWindow, RenderWindow, "Click A Button to Switch", GUILayout.Width(150));
		}

		void RenderWindow(int windowID)
		{
				if(GUILayout.Button("The Museum", GUILayout.Width(150)))
				{
					manager.SwitchState(new PS_Museum(manager));
					Debug.Log("You are in the Museum");
				}

				if(GUILayout.Button("The Acropolis", GUILayout.Width(150)))
				{
					manager.SwitchState(new PS_Acropolis(manager));
					Debug.Log("You are in the Acropolis");
				}

				if(GUILayout.Button("The Temple of Apollo", GUILayout.Width(150)))
				{
					manager.SwitchState(new PS_Apollo(manager));
					Debug.Log("You are in the Acropolis");
				}

				if(GUILayout.Button("The Temple of Aphaia", GUILayout.Width(150)))
				{
					manager.SwitchState(new PS_Aphaia(manager));
					Debug.Log("You are in the Acropolis");
				}

				GUI.DragWindow(new Rect(0,0,10000,10000));
			
		}
	}
}