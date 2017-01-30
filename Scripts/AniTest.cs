using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AniTest : MonoBehaviour {

	//variables to hold the clip and the parent
	public GameObject aniParent;
	public AnimationClip aniClip;
	public float audioDelay = 0.0f;

	void Start () 
	{
	
	}
	
	IEnumerator OnMouseDown()
	{
		//Debug.Log(name + " picked using " + aniClip.name ".");
		aniParent.GetComponent<Animation>().Play(aniClip.name);

		if(GetComponent<AudioSource>())
		{
			yield return new WaitForSeconds(audioDelay);
			GetComponent<AudioSource>().Play();
		}
	}


}

/* *******************************************************************
 * This script is just to hold and trigger animations when ready     *
 * to implement them. Look at AniTwoState for more robust animations.*
 *********************************************************************/
