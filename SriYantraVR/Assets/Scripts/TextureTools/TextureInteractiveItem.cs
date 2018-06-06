using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for designating GameObject's as texture interactible.
// 
// Replaces the GameObject's main texture with an instance of
// it. An new instance of the original texture can be restored
// at any time.
namespace TextureTools {
	public class TextureInteractiveItem : MonoBehaviour {

		private Renderer r;				// renderer texture is attached to
		private Texture t;				// stores original texture

		// Use this for initialization
		void Start () 
		{
			r = GetComponent<Renderer> ();
			t = r.material.mainTexture;
			r.material.mainTexture = Instantiate (t);
		}

		// restores to an instance of the original texture
		public void restore () 
		{
			r.material.mainTexture = Instantiate (t);
		}

		public Texture getActiveTexture() 
		{
			return r.material.mainTexture;
		}
	}
}