using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// mimics UnityVRAssets' VRMouseRaycaster.cs (UnityVRAssets > VRStandardAssets > Scripts)
// so the mouse can be used in the editor in place of gaze controls
public class MouseRaycaster : MonoBehaviour {


    [SerializeField] private Camera m_Camera;
	[SerializeField] private MouseInput m_MouseInput;

	private MouseInteractiveItem currentItem;
	private MouseInteractiveItem lastItem;

	private void OnEnable()
	{
		m_MouseInput.OnClick += HandleClick;
		m_MouseInput.OnUp += HandleUp;
		m_MouseInput.OnDown += HandleDown;
	}


	private void OnDisable ()
	{
		m_MouseInput.OnClick -= HandleClick;
		m_MouseInput.OnUp -= HandleUp;
		m_MouseInput.OnDown -= HandleDown;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MouseRaycast ();
	}

	void MouseRaycast () {
		// check hit
		RaycastHit hit;
		if (Physics.Raycast (m_Camera.ScreenPointToRay (Input.mousePosition), out hit)) {

			// check mouse interactive item
			MouseInteractiveItem item = hit.transform.gameObject.GetComponent<MouseInteractiveItem> ();
			currentItem = item;

			// if current item exists, call Over
			if (currentItem && currentItem != lastItem) {
				currentItem.Over ();
			}

			// if it's not the same as the last item, deactivate the last item
			if (currentItem != lastItem)
				DeactivateLastItem ();

			lastItem = item;
		} 
		else
		{
			// nothing was hit
			DeactivateLastItem ();
			currentItem = null;
		}
	}

	void DeactivateLastItem () {
		if (lastItem == null)
			return;

		lastItem.Out ();
		lastItem = null;
	}

	void HandleClick () {
		if (currentItem != null)
			currentItem.Click ();
	}

	void HandleUp () {
		if (currentItem != null)
			currentItem.Up ();
	}

	void HandleDown () {
		if (currentItem != null)
			currentItem.Down ();
	}
}
