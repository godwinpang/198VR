using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

// connects the view to the animation system
public class YantraController : MonoBehaviour {

	[SerializeField] private SelectionSlider toggleSlider;				// slider to play/pause animation
	[SerializeField] private UnityEngine.UI.Text toggleSliderText;		// text of play/pause slider
	[SerializeField] private SelectionSlider resetSlider;				// slider to reset animation
	[SerializeField] private YantraAnimator m_YantraAnimator;			// yantra animator class

	private void OnEnable()
	{
		toggleSlider.OnBarFilled += toggleAnimation;
		resetSlider.OnBarFilled += resetAnimation;
	}

	private void OnDisable()
	{
		toggleSlider.OnBarFilled -= toggleAnimation;
		resetSlider.OnBarFilled -= resetAnimation;
	}

	void toggleAnimation()
	{
		if (m_YantraAnimator.isAnimating ()) {
			m_YantraAnimator.pause ();
			toggleSliderText.text = "Play";
		}
		else {
			m_YantraAnimator.play();
			toggleSliderText.text = "Pause";
		}
	}
		
	void resetAnimation()
	{
		m_YantraAnimator.reset ();
		toggleSliderText.text = "Play";
	}
}
