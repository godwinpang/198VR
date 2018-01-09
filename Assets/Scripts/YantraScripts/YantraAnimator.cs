using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextureTools;

// Plays AnimationFrames based on a specified audio track.
// When animating, a frame is shown when the audio track's
// time reaches that frame's timestamp.
// 
// Frames are loaded from a FrameLoader instance.
//
// Initializes in the paused state.
public class YantraAnimator : MonoBehaviour {

	[SerializeField] private YantraFrameLoader m_FrameLoader;	// loads frames into the animator
	[SerializeField] private AudioSource m_AudioSource;			// audio track
	[SerializeField] private float audioStartTime = 0f;			// time in track audio starts at

	private List<AnimationFrame> animationFrames = new List<AnimationFrame>();	// list of AnimationFrames
	private int nextFrame = 0;													// index of the next frame

	private bool animateEnabled;	// animation status (enabled/disabled)

	void Start () 
	{
		loadFrames ();
		reset ();
	}

	void Update () 
	{
		if(animateEnabled)
			animate ();
	}

	public void play() 
	{
		catchUpToAudio ();
		m_AudioSource.Play();
		animateEnabled = true;
	}

	public void pause() 
	{
		m_AudioSource.Pause ();
		animateEnabled = false;
	}

	public void reset() 
	{
		if (frameExists (nextFrame - 1))
			hideFrame (nextFrame - 1);

		animateEnabled = false;
		nextFrame = 0;

		m_AudioSource.Pause ();
		m_AudioSource.time = audioStartTime;
		catchUpToAudio ();
	}

	public bool isAnimating() 
	{
		return animateEnabled;
	}

	private void loadFrames() 
	{
		foreach (AnimationFrame f in m_FrameLoader.getSortedFrames ()) {
			animationFrames.Add (f);
		}
	}

	// play the next frame, if it's ready
	private void animate() 
	{
		if (!frameExists (nextFrame) || !frameReady (nextFrame)) {
			return;
		}

		if (nextFrame > 0)
			hideFrame (nextFrame - 1);

		showFrame (nextFrame);
		nextFrame++;
	}

	private void showFrame(int frame) 
	{
		animationFrames [frame].show ();
	}

	private void hideFrame(int frame)
	{
		animationFrames [frame].hide ();
	}

	// true if frame exists and audio has progressed past timestamp
	private bool frameReady(int frame)
	{
		return m_AudioSource.time >= animationFrames [frame].timestamp;
	}
		
	private bool frameExists(int frame)
	{
		return frame >= 0 && frame < animationFrames.Count;
	}

	// skip past all the ready frames except the last one
	private void catchUpToAudio()
	{
		while (frameExists (nextFrame + 1) && frameReady (nextFrame + 1)) {
			nextFrame++;
		}
	}
}
