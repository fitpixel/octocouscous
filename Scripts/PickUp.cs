using UnityEngine;
using System.Collections;

namespace Assets.Code.States
{
	public class PickUp : MonoBehaviour 
	{
		
		public enum PickUpCategory
		{
			VORTEX, MAP, ARCH, NPC
		}

		public Texture icon;
		public int points;
		public string fitsTag;
		public PickUpCategory category;

		public void Awake()
		{
			DontDestroyOnLoad(transform.gameObject);
		}
	}
}