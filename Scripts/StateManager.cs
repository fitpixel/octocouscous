using UnityEngine;
using Assets.Code.States;
using Assets.Code.Interfaces;

public class StateManager : MonoBehaviour
{
	private IStateBase activeState; 

	[HideInInspector]
	public GameData gameDataRef;
	//[HideInInspector]
	//public PlayerInventory playerInv;
	
	private static StateManager instanceRef;

	void Awake()
	{
		Screen.SetResolution(1280,720,false); //false = use windowed mode

		if(instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

	void Start()
	{
		activeState = new TitleState(this); 
		gameDataRef = GetComponent<GameData>(); //Get the game data component which is stored in the GameManager
		//playerInv = GetComponent<PlayerInventory>(); //get the player inventory and don't destroy it or what it is attatched to
		//DontDestroyOnLoad(playerInv);
	}										
	
	void Update()
	{
		if(activeState != null) 				
		{					   
			activeState.StateUpdate();
		}
}
	
	void OnGUI()
	{
		if(activeState != null)
		{
			activeState.ShowIt();
		}
	}
	
	public void SwitchState(IStateBase newState)
	{
		activeState = newState;
	}

	public void Restart()
	{
		Destroy(gameObject);
		Application.LoadLevel("00_Title");
	}
}