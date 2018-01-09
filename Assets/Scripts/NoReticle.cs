using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

// this class hides the reticle when the given 
// VRInteractiveItem is gazed upon
public class NoReticle : MonoBehaviour {
       
	public VRInteractiveItem m_InteractiveItem;		// item being looked at 
	public Reticle m_Reticle;						// reticle to hide/show

	private void OnEnable()
	{
		m_InteractiveItem.OnOver += HandleOver;
		m_InteractiveItem.OnOut += HandleOut;
	}


	private void OnDisable()
	{
		m_InteractiveItem.OnOver -= HandleOver;
		m_InteractiveItem.OnOut -= HandleOut;
	}


	//Handle the Over event
	private void HandleOver()
	{
		m_Reticle.Hide ();
	}


	//Handle the Out event
	private void HandleOut()
	{
		m_Reticle.Show ();
	}
}
