using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;



//  Used as a place to create and/or load AnimationFrames.
//  Contains a get method to get all of the frames
//  sorted by increasing timestamp.
//  Should be set to load before other scripts that rely on it
public class YantraFrameLoader : MonoBehaviour {

	// ====== used as utilities for frame creation =======
	[SerializeField] private bool saveMarkerValues;		 		// whether or not to save all marker values 
	[SerializeField] private string filename_to_save_to;		// file to save markers to (in Resources folder)
	// ========= end of frame creation utilities =========

	[SerializeField] private string markerFilename;				// file to load MarkerFrame info from
	[SerializeField] private string fillFilename;				// file to load FillFrame info from

	// markers (for use with MarkerFrame)
	[SerializeField] private GameObject m_CircleMarker;			// normal sized
	[SerializeField] private GameObject m_SmallCircleMarker;	// smaller version

	// renderers (for use with TextureFrame)
	[SerializeField] private Renderer m_BackgroundRenderer;		// background of SriYantra
	[SerializeField] private Renderer m_ForegroundRenderer1;	// first foreground
	[SerializeField] private Renderer m_ForegroundRenderer2;	// intended to be used when multiple
																//   TextureFrames are being shown at
																//   the same time

	// fade tools (used by TextureFrames to fade a texture in)
	[SerializeField] private FadeTool foregroundFadeTool1;
	[SerializeField] private FadeTool foregroundFadeTool2;		

	// background textures (displayed by TextureFrames)
	// named after the filename of the texture that should be used
	[SerializeField] private Texture2D fill1;
	[SerializeField] private Texture2D fill2;
	[SerializeField] private Texture2D fill3; 
	[SerializeField] private Texture2D fill4; 
	[SerializeField] private Texture2D fill5; 
	[SerializeField] private Texture2D fill6;
	[SerializeField] private Texture2D fill7; 
	[SerializeField] private Texture2D fill8;
	[SerializeField] private Texture2D fill9; 
	[SerializeField] private Texture2D circlesVertical;
	[SerializeField] private Texture2D circlesEnlightened;

	// for use with FillFrame
	[SerializeField] private Color fillColor;					// color to perform fill with
	[SerializeField] private Renderer m_FillLayerRenderer;		// renderer fill will be performed with
	[SerializeField] private Texture2D lowresYantraTexture;		// low res for performance boost, 
																// increase resolution for more detailed
																//   fill. See FillFrame for details

	private List<AnimationFrame> animationFrames = new List<AnimationFrame>();	// list of AnimationFrames

	public List<AnimationFrame> getSortedFrames()
	{
		// sort frames
		if (animationFrames.Count > 1) {
			animationFrames.Sort (CompareFramesByTimestamp);
		}

		return animationFrames;
	}

	// Use this for initialization
	void Start () 
	{
		addHandmadeFrames ();

		try { loadMarkerFrames (markerFilename, m_CircleMarker); }
		catch (System.Exception e) { Debug.Log ("Could not open marker file: " + e.Message); }

		try { loadFillFrames (fillFilename, m_FillLayerRenderer, lowresYantraTexture, fillColor); }
		catch (System.Exception e) { Debug.Log ("Could not open fill file: " + e.Message); }

		// optionally save MarkerFrame info to file
		if(saveMarkerValues) {
			saveMarkerFrames(filename_to_save_to);
		}
	}

	// ideally these would be loaded from a file or made via
	// an editor script, but instead these frames are handmade
	// since there aren't enough of them to warrant the time
	// that would have taken
	private void addHandmadeFrames()
	{

		// first frame with a fill and a circle
		LayeredFrame lFrame;
		lFrame = new LayeredFrame (0f);
		lFrame.addFrame (new TextureFrame (0f, m_BackgroundRenderer, fill1));
		lFrame.addFrame (new MarkerFrame (0f, m_SmallCircleMarker, new Vector2 (0.02f, -0.02f)));
		animationFrames.Add (lFrame);

		// background textures (multiple fills, would be too expensive to use FillFrame
		animationFrames.Add (new TextureFrame (135f, m_BackgroundRenderer, fill2));
		animationFrames.Add (new TextureFrame (168f, m_BackgroundRenderer, fill3));
		animationFrames.Add (new TextureFrame (185f, m_BackgroundRenderer, fill4));
		animationFrames.Add (new TextureFrame (214f, m_BackgroundRenderer, fill5));
		animationFrames.Add (new TextureFrame (237f, m_BackgroundRenderer, fill6));
		animationFrames.Add (new TextureFrame (261f, m_BackgroundRenderer, fill7));
		animationFrames.Add (new TextureFrame (275f, m_BackgroundRenderer, fill8));

		// four small circles
		animationFrames.Add (new MarkerFrame (280.00f, m_SmallCircleMarker, new Vector2(0.35f, 0.28f)));
		animationFrames.Add (new MarkerFrame (281.33f, m_SmallCircleMarker, new Vector2(-0.31f, 0.28f)));
		animationFrames.Add (new MarkerFrame (282.66f, m_SmallCircleMarker, new Vector2(0.46f, 0.06f)));
		animationFrames.Add (new MarkerFrame (284.00f, m_SmallCircleMarker, new Vector2(-0.43f, 0.06f)));

		// one of the last frames w/ a fill and a circle
		lFrame = new LayeredFrame (291f);
		lFrame.addFrame (new TextureFrame (0f, m_BackgroundRenderer, fill9));
		lFrame.addFrame (new MarkerFrame (0f, m_SmallCircleMarker, new Vector2 (0.02f, -0.02f)));
		animationFrames.Add (lFrame);

		// second to last frame with lots of circles that fade in
		animationFrames.Add (new TextureFrame (305f, m_ForegroundRenderer1, circlesVertical, foregroundFadeTool1, 1.0f));

		// last frame, layered so the vertical circles stay while the enlightened ones fade in
		lFrame = new LayeredFrame (320f);
		lFrame.addFrame (new TextureFrame (0f, m_ForegroundRenderer1, circlesVertical));
		lFrame.addFrame (new TextureFrame (0f, m_ForegroundRenderer2, circlesEnlightened, foregroundFadeTool2, 4.0f));
		animationFrames.Add (lFrame);

		// hide everything at the end of the meditation
		animationFrames.Add (new EmptyFrame (344f));

	}

	// load marker frames from a file
	// file format: csv
	//   exactly 3 entries per row:
	//   	timestamp (float), xCoord (float), yCoord (float)
	private void loadMarkerFrames(string filename, GameObject marker)
	{

		foreach (string[] data in getCSVData(filename)) {
			if (data.Length != 3)
				continue;

			float timestamp = float.Parse (data [0]);
			float xCoord = float.Parse (data [1]);
			float yCoord = float.Parse (data [2]);
			animationFrames.Add(
				new MarkerFrame(timestamp, marker, new Vector2(xCoord, yCoord))
			);
		}
	}

	// load fill frames from a file
	// file format: csv
	//   exactly 3 entries per row:
	//   	timestamp (float), uCoord (float), vCoord (float)
	private void loadFillFrames(string filename, 
								Renderer rendererToBeFilled, 
								Texture2D baseTex,
								Color fillColor)
	{
		foreach(string[] data in getCSVData(filename)) {
			if (data.Length != 3)
				continue;

			float timestamp = float.Parse (data [0]);
			float uCoord = float.Parse (data [1]);
			float vCoord = float.Parse (data [2]);

			animationFrames.Add(
				new FillFrame(
					timestamp, 
					rendererToBeFilled,
					baseTex, 
					new Vector2(uCoord, vCoord),
					fillColor
				)
			);
		}
	}

	// read a csv file from StreamingAssets
	// return format:
	//		List<string[]> where each string[] is a row of csv data
	private List<string[]> getCSVData(string filename)
	{
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, filename);

		string[] lines;
		if(Application.platform == RuntimePlatform.Android) //Need to extract file from apk first
		{
			WWW reader = new WWW(filePath);
			while (!reader.isDone) { }

			string[] newLineSeparator;
			if (reader.text.Contains ("\r\n"))
				newLineSeparator = new string[] { "\r\n" };
			else
				newLineSeparator = new string[] { "\n" };
			
			lines = reader.text.Split(newLineSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		}
		else {
			lines = System.IO.File.ReadAllLines (filePath);
		}

		char[] csvSeparator = { ',' }; 

		List<string[]> csvData = new List<string[]> ();
		foreach (string line in lines) {
			csvData.Add(line.Split(csvSeparator, System.StringSplitOptions.RemoveEmptyEntries));
		}

		return csvData;
	}

	// save all marker frames into a file
	// useful for saving marker frames created in the editor
	private void saveMarkerFrames(string filename)
	{
		StringBuilder sb = new StringBuilder();

		string delimiter = ",";
		string[] output = new string[3];
		int length = animationFrames.Count;

		for (int i = 0; i < length; i++) {
			if (animationFrames [i].GetType () != typeof(MarkerFrame))
				continue;

			MarkerFrame mFrame = (MarkerFrame)animationFrames [i];
			output [0] = string.Format ("{0}", mFrame.timestamp);
			output [1] = string.Format ("{0:0.00}", mFrame.coords.x - .15f);
			output [2] = string.Format ("{0:0.00}", mFrame.coords.y + .15f);
			sb.AppendLine (string.Join (delimiter, output));
		}

		string filePath = Application.dataPath + "/Resources/" + filename;

		StreamWriter outStream = System.IO.File.CreateText(filePath);
		outStream.WriteLine(sb.ToString());
		outStream.Close();
	}	

	// used to sort frames
	private int CompareFramesByTimestamp (AnimationFrame f1, AnimationFrame f2)
	{
		if (f1.timestamp == f2.timestamp) 
		{
			return 0;
		}
		else if (f1.timestamp > f2.timestamp)
		{
			return 1;
		}
		else
		{
			return -1;	
		}
	}
}