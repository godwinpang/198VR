using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Attach to a game object to give its texture the ability to fade in.
//  Contains fade functions that must be called from another class.
public class FadeTool : MonoBehaviour {

	private Renderer r;

	void Start () 
	{
		r = GetComponent<Renderer> ();
	}

	public void startFadeIn(float fadeInTime) 
	{
		StartCoroutine ("fadeInCoroutine", fadeInTime);
	}

	// immediately complete fade in and end the coroutine
	public void stopFadeIn() 
	{
		Color c = r.material.color;
		c.a = 1.0f;
		r.material.color = c;
		StopCoroutine ("fadeInCoroutine");
	}

	private IEnumerator fadeInCoroutine(float timeToFadeIn) 
	{       
		if (timeToFadeIn == 0.0f) {
			yield break;
		}

		Color c = r.material.color;
		c.a = 0.0f;
		while (c.a < 1.0f) {
			c.a += (1.0f / timeToFadeIn) * Time.deltaTime;
			r.material.color = c;
			yield return null;
		}
		c.a = 1.0f;
		r.material.color = c;
	}
}
