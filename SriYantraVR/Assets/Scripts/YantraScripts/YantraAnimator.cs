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

	private int[,] frameThresholdArray = new int[10, 2] {{0,29},{30,46},{47,55},{56,70},{71,81},{82,92},{93,101},{102,109},{110,110},{111,111}};

	private float[] timeThresholdArray = new float [10] {0f,140f,173f,193f,224.4f,247.7f,270.2f,0,0,0};

	//private int[] RRThresholdArray = new int [9];

	private int level;

	private bool animateEnabled;	// animation status (enabled/disabled)

	void Start () 
	{
		level = 0;
		loadFrames ();
		Debug.Log ("Number of frames: " + animationFrames);
		reset ();
	}

	void Update () 
	{
		//Application.targetFrameRate = 2;
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
		level = 1;

		m_AudioSource.Pause ();
		m_AudioSource.time = audioStartTime;
		catchUpToAudio ();
	}

	public bool isAnimating() 
	{
		return animateEnabled;
	}

	public void toNextLevel(){
		Debug.Log ("toNextLevel");
		Debug.Log ("prevLevel" + level);
		if (level<9){
			level++;
		}
		Debug.Log ("nextLevel" + level);
	}

	public void toPreviousLevel(){
		Debug.Log ("toPrevLevel");
		Debug.Log ("prevLevel" + level);
		if (level > 0) {
			level--;
		}
		Debug.Log ("nextLevel" + level);
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
		/* placeholder code for level progression based on heartrate */
		//if (nextLevelMet (heartbeat) || frameReady (nextLevelFirstFrame)) {
			
		//}

		if (!frameExists (nextFrame) || !frameReady (nextFrame)) {
			return;
		}

		Debug.Log ("nextFrame:" + nextFrame);

		if (nextFrame < frameThresholdArray [level, 0] || nextFrame > frameThresholdArray [level, 1]) {
			if (nextFrame > 0)
				hideFrame (nextFrame - 1);
			nextFrame = frameThresholdArray [level, 0];
			m_AudioSource.time = timeThresholdArray [level];
		}

		Debug.Log ("nextFrame after:" + nextFrame);

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
		//Debug.Log (m_AudioSource.time);
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
