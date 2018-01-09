using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextureTools;
using System.IO;
using System.Text;

//  A tool that applies the Flood Fill algorithm to a texture
//  based on the point on the texture that was clicked.
//  Will optionally save the UV coordinates that were clicked
//  to a file in csv format.
public class FillOnClick : MonoBehaviour {

	public Camera cam;							// camera to use
	public Color fillColor;						// color to flood fill with
	public bool saveUVsToFile;					// option to save UVs clicked on to a file
	public string filename;						// name of file to save to
	public bool saveInStreamingAssetsFolder;	// save to StreamingAssets folder, or Resources if false

	// Update is called once per frame
	void Update () 
	{
		fillAtClick ();	
	}

	void fillAtClick () 
	{
		// check mouse
		if (!(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
			return;

		// check hit
		RaycastHit hit;
		if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
			return;

		// check texture interactive item
		TextureInteractiveItem item = hit.transform.gameObject.GetComponent<TextureInteractiveItem> ();
		if (item == null)
			return;

		// restore original texture on right click
		if (Input.GetMouseButtonDown(1)) {
			item.restore ();
			return;
		}

		// perform fill
		bool wasHit = fillFrom (hit);
		if (wasHit && saveUVsToFile) {
			saveCoords (hit.textureCoord);
		}
	}

	// perform flood fill at UV coords from hit
	bool fillFrom (RaycastHit hit) 
	{
		Renderer rend = hit.transform.GetComponent<Renderer>();
		MeshCollider meshCollider = hit.collider as MeshCollider;

		if (rend == null || rend.material == null 
			|| rend.material.mainTexture == null || meshCollider == null)
			return false;

		Texture2D tex = rend.material.mainTexture as Texture2D;
		Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		tex.FloodFillArea ((int)pixelUV.x, (int)pixelUV.y, fillColor);

		tex.Apply();
		return true;
	}

	// save coords to file
	// creates a new file if it does not exist, otherwise appends
	void saveCoords(Vector2 coords) 
	{

		string filePath;
		StreamWriter outstream;

		if (saveInStreamingAssetsFolder) 
		{
			filePath = System.IO.Path.Combine (Application.streamingAssetsPath, filename);
		} 
		else 
		{
			filePath = Application.dataPath + "/Resources/" + filename;
		}

		if (!File.Exists (filePath)) 
		{
			outstream = System.IO.File.CreateText (filePath);
		} 
		else 
		{
			outstream = System.IO.File.AppendText (filePath);
		}

		string output = string.Format ("{0:0.0000},{1:0.0000}", coords.x, coords.y);
		outstream.WriteLine(output);
		outstream.Close();
	}
}
