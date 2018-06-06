using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=============== FRAME CLASSES ==============/
//  frame interface intended for use with the YantraAnimator class
public interface AnimationFrame {
	float timestamp { get; set; }			// timestamp at which to play the frame

	void show ();							// show the frame
	void hide ();							// hide the frame
}

//  empty frame, used when you want nothing to be shown
public class EmptyFrame : AnimationFrame {
	public float timestamp { get; set; }

	public EmptyFrame(float timestamp) 
	{
		this.timestamp = timestamp;
	}

	public void show() { }

	public void hide() { }
}

//  shows/hides all frames within at the same time
public class LayeredFrame : AnimationFrame {
	public float timestamp { get; set; }
	public List<AnimationFrame> frames;

	public LayeredFrame (float timestamp) 
	{
		this.timestamp = timestamp;
		this.frames = new List<AnimationFrame>();
	}

	public LayeredFrame (float timestamp, params AnimationFrame[] frames) 
	{
		this.timestamp = timestamp;
		this.frames = new List<AnimationFrame>(frames);
	}

	public void addFrame(AnimationFrame frame) 
	{
		frames.Add (frame);
	}

	public void show() 
	{
		foreach (AnimationFrame f in frames)
			f.show ();
	}

	public void hide() 
	{
		foreach (AnimationFrame f in frames)
			f.hide ();
	}
}

//  place a marker at specified coordinates (in local space)
public class MarkerFrame : AnimationFrame {
	public float timestamp { get; set; }
	public GameObject marker { get; }			// marker to place
	public Vector2 coords { get; }				// local coordinates to place it at

	public MarkerFrame(float timestamp, GameObject marker, Vector2 coords) 
	{
		this.timestamp = timestamp;
		this.marker = marker;
		this.coords = coords;
	}

	public void show() 
	{
		marker.SetActive (true);
		Transform markerTransform = marker.transform;
		Vector3 newPos = new Vector3 (
			coords.x,
			coords.y,
			markerTransform.localPosition.z
		);
		markerTransform.localPosition = newPos;
	}

	public void hide() 
	{
		marker.SetActive (false);
	}
}

//  Flood fill based on UV coordinates.
//  
//  The pixels to fill are gotten using the flood fill
//  algorithm on one texture and then, rather than filling
//  that texture's pixels, instead those pixels are applied
//  on another texture.
//  Both textures must be the same dimensions.
//  
// Performance tip:
//	The flood fill algorithm is expensive on larger textures,
//  consider using a lower-res version of the texture as
//  the baseTex, and applying the pixels to an empty texture.
//  Then apply/place the modified empty texture on top of your
//  high-res texture. The result will look like the high-res 
//  texture was flood filled, while the algorithm was performed
//  on a low-res texture.
public class FillFrame : AnimationFrame {
	public float timestamp { get; set; }

	// texture to get the flood fill pixel indices from
	public Texture2D baseTex;

	// The main texture from this renderer is filled based on the flood
	// fill pixel indices from baseTex.
	// This is typically a blank texture, used as a canvas
	public Renderer r;				// renderer with the texture to apply fill indices too

	public Vector2 uvCoords;		// uv coordinates to apply flood fill at
	public Color fillColor;			// color to fill with

	private Color restoreColor;		// color to restore to
	private List<int> fillIndices;	// pixel indices to fill with fillColor

	public FillFrame(float timestamp, Renderer r, Texture2D baseTexture, Vector2 uvCoords, Color fillColor) 
	{
		this.timestamp = timestamp;
		this.r = r;
		this.baseTex = baseTexture;
		this.uvCoords = uvCoords;
		this.fillColor = fillColor;

		setRestoreColor();
		setFillIndices ();
	}

	public void show() 
	{
		fill (fillColor);
	}

	public void hide() 
	{
		fill (restoreColor);
	}
		
	private void fill(Color clr) 
	{
		Texture2D tex = r.material.mainTexture as Texture2D;
		Color[] colors = tex.GetPixels();
		foreach (int fillIndex in fillIndices) {
			colors [fillIndex] = clr;
		}
		tex.SetPixels (colors);
		tex.Apply ();
	}

	// keep record of the original color
	private void setRestoreColor() 
	{
		Texture2D tex = r.material.mainTexture as Texture2D;
		int pixelX = (int)(uvCoords.x * tex.width);
		int pixelY = (int)(uvCoords.y * tex.height);
		this.restoreColor = tex.GetPixel (pixelX, pixelY);
	}

	// set the indices to fill
	private void setFillIndices() 
	{
		int pixelX = (int)(uvCoords.x * baseTex.width);
		int pixelY = (int)(uvCoords.y * baseTex.height);
		this.fillIndices = baseTex.getFloodFillIndices (pixelX, pixelY, fillColor);
	}
}

// replace a renderer's texture, optionally have it fade in
public class TextureFrame : AnimationFrame {
	public float timestamp { get; set; }
	public Renderer r { get; set; }			// renderer to change texture
	public Texture tex;						// new texture to show

	private Texture origTex;				// original texture to replace upon hide
	private FadeTool fadeTool;				// fade tool attached to texture
	private float fadeInTime;				// time in seconds it takes to fade in

	public TextureFrame (float timestamp, Renderer r, Texture2D tex) 
	{
		this.timestamp = timestamp;
		this.r = r;
		this.tex = tex;
		this.origTex = r.material.mainTexture;
		this.fadeTool = null;
	}

	public TextureFrame (float timestamp, Renderer r, Texture2D tex, FadeTool fadeTool, float fadeInTime) 
	{
		this.timestamp = timestamp;
		this.r = r;
		this.tex = tex;
		this.origTex = r.material.mainTexture;
		this.fadeTool = fadeTool;
		this.fadeInTime = fadeInTime;
	}

	public void show() 
	{
		r.material.mainTexture = tex;
		r.enabled = true;
		if (fadeTool) {
			fadeTool.startFadeIn (fadeInTime);
		}
	}

	public void hide ()
	{
		if (fadeTool) {
			fadeTool.stopFadeIn ();
		}

		r.material.mainTexture = origTex;
		r.enabled = false;
	}
}