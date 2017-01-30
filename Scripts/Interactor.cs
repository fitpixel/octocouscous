using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ObjectLookup))]

public class Interactor : MonoBehaviour {
	//Calling on StateManager
	//private StateManager manager;
	//Variables for Picks and Mouse Over
	internal GameObject go;//transferring an item as a game object...
	internal Transform pickable; //into a transform value to find the distance from player
	internal GameObject controlCenter;
	internal GameObject cam; //main camera as game object
	public float triggerMoDis; //the distance the object needs to be for the playe to be able to pick it up
	public float moOffset; //this is the distance to offset the mouse over - player will see that it is clickable, but will have to come closer to read the message (5,8)
	internal bool picked = false; //temporarily prevent mouseover action
	internal bool mousedown = false; //so I know when mousedown is true
	internal bool processing = false; //so I can suspend mouseover actions or whatever actions when I need to
	
	//Variables for the State of each object
	public int initialState = 1;
	public bool objectIs3D = true;
	public int[] location = new int[3];
	public int[] visibility = new int[3];
	public string[] objectName = new string[3];
	public string[] description = new string[3];
	public AnimationClip[] animationClip = new AnimationClip[3];
	public float[] animationDelay = new float[3];
	public AudioClip[] soundClip = new AudioClip[3];
	public float[] audioDelay = new float[3];
	public AnimationClip[] loopAnimation = new AnimationClip[3];
	public AudioClip[] loopSoundFX = new AudioClip[3];
	public bool postLoop = false;
	public bool animates = false;
	public GameObject aniObject;

	//State Dependant Variables
	public int currentState = 1;
	public int iElement = 100;
	internal int currentLocation;
	internal int currentVisibility;
	internal string currentObjectName;
	internal string currentObjectDescription;
	internal AudioClip currentSound;
	internal float currentAudioDelay = 0.0f;
	internal AnimationClip currentAnimationClip;
	internal float currentAnimationDelay = 0.0f;
	//State dependant for 2D objects
	internal int previousState;


	//Variable for a temp cursor
	public string cursor;
	//Variables for the timer
	internal bool pickTimer; //make this variable public if you want to be able to change the time on different objects
	internal float pickTimerTime;
	//Variables for the inventory
	internal bool iMode;
	internal bool oOR;
	//variable for the transparency
	public bool useAlpha = true; //remember each object will need to have their own material?
	public Material originalMaterial;
	public Color originalColor;
	public Color alphaColor; //color types are vector4(RGBA)
	//temp variable for shader
	internal string tempShaderName;



	

	void Awake()
	{
		pickable = transform;
		
	}

	void Start () 
	{	
		cam = GameObject.FindWithTag("MainCamera");
		
		controlCenter = GameObject.Find("GameManager");
		if(GetComponent<MeshRenderer>()) //if the object has a Meshrenderer, get its material
		{
			originalMaterial = GetComponent<MeshRenderer>().material;
			//GO BACK TO pg 358 - I need to look into extension methods and how they work to relace substrings wit them - and the rest of the chapter just deals with visibility
			//print(this + " " + "Shader: " + renderer.material.shader.name + " " + renderer.material.color.a);
			//prep for auto fade by checking shader, unless specified as false
			if(useAlpha) //if it isn't set to false by me
			{
				useAlpha = false; // set it to false, there will be only one condition to make it true
				tempShaderName = GetComponent<Renderer>().material.shader.name; //get the shader teame, a string
				if(tempShaderName.Length > 11) //check for short names - they aren't transparent shaders
				{
					//check to see if the materials shader is a transparency shader
					if(tempShaderName.Substring(0,11) == "Transparent")
					{
						useAlpha = true;
						originalColor = GetComponent<Renderer>().material.color;
						//not sure what is going on with this line, i can't change the visibility of the renderer with this line
						//alphaColor = Color(originalColor.r, originalColor.b,originalColor.g,0);
					}
				}
			}
		}



		//load the initial values for the object
		currentState = initialState;
		
		currentObjectName = objectName[currentState];
		currentObjectDescription = description[currentState];
		currentLocation = location[currentState];
		currentVisibility = visibility[currentState];

		if(currentState == 1)
		{	
		 	controlCenter.GetComponent<GameData>().useText = true;
		}
	
		
	}

	void Update()
	{		
		if(currentState == 0)
		{
			if(objectIs3D && GetComponent<MeshRenderer>())
			{
				transform.GetComponent<Renderer>().enabled = false;
				
			}
		}
		if(pickTimer && Time.time > pickTimerTime -1.0)//if the time is up and flag is on
			{
				controlCenter.GetComponent<GameData>().suppressPointer = false;

			}
		if(pickTimer && Time.time > pickTimerTime)
		{
			pickTimer = false;
			
			if(controlCenter.GetComponent<GameData>().actionObject == this.name)
			{
				controlCenter.GetComponent<GameData>().showActionMsg = false;
				gameObject.SetActive(false);
			}	
		}	
	}
	
	void OnMouseOver()	
	{
		//if any of these are true, GTFO the function
		if(location[currentState] == 2)
			return;	
		if(objectIs3D && Vector3.Distance(cam.transform.position, transform.position) > triggerMoDis + moOffset)
			return;	
		if(processing) 
			return; //if processing something, leave the function		
		iMode = controlCenter.GetComponent<GameData>().iMode;
		if(iMode && gameObject.layer != 9)
			return;				
		
		if(!objectIs3D) GetComponent<GUITexture>().color = new Color(0.55f,0.55f,0.55f,1f);

		//activate the text vissdibility on mouseover
		controlCenter.GetComponent<GameData>().showText = true;
		controlCenter.GetComponent<GameData>().shortDesc = objectName[currentState];
		controlCenter.GetComponent<GameData>().longDesc = description[currentState];
		
		if(!objectIs3D || Vector3.Distance(cam.transform.position, transform.position) <= triggerMoDis)
		{
			controlCenter.GetComponent<GameData>().inRange = true;
		}
		else
		{
			controlCenter.GetComponent<GameData>().inRange = false;
		}
		
		//calculates the distance from camera and tells the cursor to  change or not	
		if(objectIs3D)
		{
			if(Vector3.Distance(cam.transform.position, transform.position) > triggerMoDis + moOffset) 
			{
				controlCenter.SendMessage("CursorColorChange", false);
				
			}
			else
			{
				controlCenter.SendMessage("CursorColorChange",true);
			}
		}
	}

	void OnMouseExit()
	{
		if(processing) return; //if processing something, leave the function

		if(!objectIs3D)
			{
				GetComponent<GUITexture>().color = Color.grey;
			}

		//deactivate the text visibility on mouseexit
		controlCenter.GetComponent<GameData>().showText = false;	
		controlCenter.SendMessage("CursorColorChange", false);
	}

	void OnMouseDown()
	{
		Debug.Log("Are you getting to Mouse Down?");
		print(controlCenter.GetComponent<GameData>().currentCursor);
		
		if(location[currentState] == 2) //if object not pickable, get out of the function
			return;

		if(objectIs3D)
		{
			if(Vector3.Distance(cam.transform.position, transform.position) > triggerMoDis) 
			{
				Debug.Log("Out of range from " + this.name);
				controlCenter.GetComponent<GameData>().showText = false;
				picked = false;
			}
			else
			{
				Debug.Log("We have mousedown on " + this.name);
			}
		}
		
		if(processing) return; //if processing something, leave the function

		if(iMode && gameObject.layer != 9)
			return;	
		
		controlCenter.SendMessage("CursorColorChange", false);

		//get and assign the current cursor value to current cursor
		cursor = controlCenter.GetComponent<GameData>().currentCursor.name;
		
		if(cursor == controlCenter.GetComponent<GameData>().defaultCursor.name)
		{
			cursor = "default";
		}
		
		GetComponent<ObjectLookup>().LookUpState(this.gameObject, currentState, cursor);
		 //check off animates in the inspector if it doesn't work
		
	}

	public IEnumerator ProcessObject (int newState)
	{
		Debug.Log("In ProcessObject");
		processing = true; //if you are in this function, you are processing an object
		//controlCenter.GetComponent<GameData>().suppressPointer = true;
		//tell game data to show the action text (replies)
	
		controlCenter.GetComponent<GameData>().suppressPointer = true;
						
		//deactivate the text messages
		controlCenter.GetComponent<GameData>().showText = false;
		controlCenter.GetComponent<GameData>().showActionMsg = true;
		//Code for the timer
		pickTimerTime = Time.time + 1.5f; //set the timer to go for .05 seconds
		pickTimer = true;
		controlCenter.GetComponent<GameData>().actionObject = this.name; //this line finds which action object started the timer first

		//HA. HAHAHAHAHHAHAHHAHAHHA
		previousState = currentState;
		currentState = newState; //updates the object's current state (from ObjectLookup)

		

		//update more of the data with the new state
		currentObjectName = objectName[currentState];
		currentObjectDescription = description[currentState];
		currentLocation = location[currentState];
		currentVisibility = visibility[currentState];
		Debug.Log("Finished ProcessObject");

		//assign the current clip and delay and audio for the new state
		if(animates)
		{
			currentAnimationClip = animationClip[currentState];
			currentAnimationDelay = animationDelay[currentState];
		}

		if(animates)
		{
			currentSound = soundClip[currentState];
			currentAudioDelay = audioDelay[currentState];
			
			if(objectIs3D)
			{
				HandleVisibility();
			}			
		}
		

		if(animates && animationClip[currentState] != null)
		{
			if(aniObject == null) 
			{
				aniObject = gameObject;
			}

			//pause before playing the animation
			yield return new WaitForSeconds(currentAnimationDelay);
			//play the animation
			aniObject.GetComponent<Animation>().Play(currentAnimationClip.name);
			//process audio
			ProcessAudio(currentSound);
			//wait some more
			yield return new WaitForSeconds(currentAnimationClip.length);
			//check for a looping animation to follow the primary animation
			if(postLoop) //if postLoop is checked/true, there is a looping animation to trigger
			{
				aniObject.GetComponent<Animation>().Play(loopAnimation[currentState].name);
				ProcessAudio(loopSoundFX[currentState]);
			}

			processing = false;
		}
		else
		{
			if(!objectIs3D) 
			{
				Debug.Log("Print if you send to Handle2D.");
				StartCoroutine(Handle2D());
				
				yield return new WaitForSeconds(.1f);
			}

			ProcessAudio(currentSound);
			yield return new WaitForSeconds(1.0f);
			processing = false;
		}
	}

	public void ProcessAudio(AudioClip theClip)
	{
		if(theClip) //if there is a sound clip
		{
			if(GetComponent<AudioSource>()) //check to make sure an audio source ocmponent exists before playing
			{
				GetComponent<AudioSource>().clip = theClip; //change the audio components assigned sound file
				GetComponent<AudioSource>().PlayDelayed(currentAudioDelay); //delay before playing it
			}
		}
	}

	public IEnumerator HandleVisibility()
	{
		switch(currentVisibility)
		{
			case 0: //deactive immediately, no fades as per GameNotes
			Debug.Log("Get past the case?");
			if(currentLocation == 0)
			{
				controlCenter.GetComponent<GameData>().EmergencyTimer(this.name);
				yield return new WaitForSeconds(2.0f);
				gameObject.SetActive(false);				
			}
			
			break;

			case 1: //active at start
			break;

			case 2:// deactivate at start
			break;

			case 3: //activate at start, deactivate at end
			break;

			case 4: //deactivate at end
			break;
		}
	}

	public IEnumerator Handle2D()
	{
		Debug.Log("Print if you get in this function.");
		print("Previous State: " + previousState + " Current State: " + currentState);

		//Not in Scene -> is cursor
		if(previousState == 0 && currentState == 2)
		{
			Debug.Log("Print this if you get here. 1.");
			controlCenter.GetComponent<GameData>().currentCursor = GetComponent<GUITexture>().texture;
			Debug.Log(GetComponent<GUITexture>().texture);
			gameObject.GetComponent<GUITexture>().enabled = false;
			cursor = controlCenter.GetComponent<GameData>().currentCursor.ToString() ;
		}
		else
		{

			ProcessObject(currentState);
		}

		//Not in scene -> In Inventory
		if(previousState == 0 && currentState == 1)
		{
			Debug.Log("Print this if you get here. 2.");
			controlCenter.SendMessage("AddToInventory", gameObject);
			gameObject.GetComponent<GUITexture>().enabled = true;
			cursor = controlCenter.GetComponent<GameData>().currentCursor.ToString() ;
		}
		else
		{
			ProcessObject(currentState);
		}

		//Is Cursor -> Not in scene
		if(previousState == 2 && currentState == 0)
		{
			Debug.Log("Print this if you get here. 3.");
			controlCenter.SendMessage("ResetCursor");
			gameObject.GetComponent<GUITexture>().enabled = true;
			yield return new WaitForSeconds(1.0f);
			gameObject.SetActive(false); //deactivate the object immediately ... there is also supposed to be a yield above this line, I just took it out
		}
		else
		{
			ProcessObject(currentState);
		}

		//Is Cursor -> In Inventory
		if(previousState == 2 && currentState == 1)
		{
			Debug.Log("Print this if you get here. 4.");
			controlCenter.SendMessage("AddToInventory", gameObject);
			gameObject.GetComponent<GUITexture>().enabled = true;
			cursor = controlCenter.GetComponent<GameData>().currentCursor.ToString() ;
			controlCenter.SendMessage("ResetCursor");
		}
		else
		{
			ProcessObject(currentState);
		}

		//In Inventory -> Not in scene
		if(previousState == 1 && currentState == 0)
		{
			Debug.Log("Print this if you get here. 5.");
					
			controlCenter.SendMessage("RemoveFromInventory", gameObject);
			yield return new WaitForSeconds(.1f);			
			gameObject.GetComponent<GUITexture>().enabled = false;						
			controlCenter.SendMessage("ResetCursor");
			cursor = controlCenter.GetComponent<GameData>().currentCursor.ToString();
			
		}
		else
		{
			ProcessObject(currentState);
		}

		//In inventory -> Is Cursor
		if(previousState == 1 && currentState == 2)
		{
			Debug.Log("Print this if you get here. 6.");
			gameObject.GetComponent<GUITexture>().enabled = false;
			controlCenter.SendMessage("RemoveFromInventory", gameObject);
			controlCenter.GetComponent<GameData>().currentCursor = GetComponent<GUITexture>().texture;
			cursor = controlCenter.GetComponent<GameData>().currentCursor.ToString() ;
		}
		else
		{
			ProcessObject(currentState);
		}
	}
}
