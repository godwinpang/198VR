using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// mimics UnityVRAssets' VRInput.cs (UnityVRAssets > VRStandardAssets > Scripts)
// so the mouse can be used in the editor in place of gaze controls
public class MouseInput : MonoBehaviour {

	public event Action OnDown;
	public event Action OnUp;
	public event Action OnClick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		checkInput ();	
	}

	void checkInput () 
	{
		if (Input.GetMouseButtonDown (0))
			OnDown ();

		if (Input.GetMouseButtonUp (0)) {
			OnUp ();
			OnClick ();
		}
	}
}
