using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AniTwoState : MonoBehaviour {

	//variables to hold the clip and the parent
	public GameObject aniObject;
	public AnimationClip aniClipA;
	public AnimationClip aniClipB;
	public AudioClip audoClipA;
	public AudioClip audioClipB;
	public float audioDelayA = 0.0f;
	public float audioDelayB = 0.0f;
	public AnimationClip aniLoopClip;

	internal AnimationClip aniClip;
	internal AudioClip fxClip;
	internal float audioDelay;
	internal bool objState = true;


	//variable to set the object to two state or one state
	public bool twoStates = false;


	void Start () 
	{
		//if no parent was assigned, assume it is the object this scrip is on
		if(aniObject == null)
		{
			aniObject = this.gameObject;
		}
	}
	
	IEnumerator OnMouseDown()
	{
		if(twoStates == false)
		{
			objState = true;
		}

		if(objState) //if objState is true, use A
		{
			aniClip = aniClipA; //set the new animation clip
			fxClip = audoClipA; //set the new audio clip
			audioDelay = audioDelayA; //set the new delay
			objState = false; //change its state to false
		}
		else
		{
			aniClip = aniClipB;
			fxClip = audioClipB;
			audioDelay = audioDelayB;
			objState = true;
		}

		//Debug.Log(this.name + " picked using " + aniClip.name ".");
		aniObject.GetComponent<Animation>().Play(aniClip.name);

		if(GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().clip = fxClip; //change the audio components assigned sound file
			GetComponent<AudioSource>().PlayDelayed(audioDelay); //delay before playing
		}

		if(aniLoopClip)
		{
			//wait for the length of the first animation before you play the second
			yield return new WaitForSeconds(aniClipA.length);
			aniObject.GetComponent<Animation>().Play(aniLoopClip.name);//this one needs to be set to loop
		}
	}


}

/* *******************************************************************
 * This script is just to hold and trigger animations when ready     *
 * to implement them. Look at AniTwoState for more robust animations.*
 *********************************************************************/
