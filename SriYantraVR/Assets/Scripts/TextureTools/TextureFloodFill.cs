using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Extends Texture2D to include Flood Fill algorithm implementations
public static class TextureFloodFill
{
	public struct Point
	{
		public short x;
		public short y;
		public Point(short aX, short aY) { x = aX; y = aY; }
		public Point(int aX, int aY) : this((short)aX, (short)aY) { }
	}

	// Fill this texture based on its pixels
	public static void FloodFillArea(this Texture2D aTex, int aX, int aY, Color aFillColor)
	{
		int w = aTex.width;
		int h = aTex.height;
		Color[] colors = aTex.GetPixels();
		Color refCol = colors[aX + aY * w];
		Queue<Point> nodes = new Queue<Point>();
		nodes.Enqueue(new Point(aX, aY));
		while (nodes.Count > 0)
		{
			Point current = nodes.Dequeue();
			for (int i = current.x; i < w; i++)
			{
				Color C = colors[i + current.y * w];
				if (C != refCol || C == aFillColor)
					break;
				colors[i + current.y * w] = aFillColor;
				if (current.y + 1 < h)
				{
					C = colors[i + current.y * w + w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y + 1));
				}
				if (current.y - 1 >= 0)
				{
					C = colors[i + current.y * w - w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y - 1));
				}
			}
			for (int i = current.x - 1; i >= 0; i--)
			{
				Color C = colors[i + current.y * w];
				if (C != refCol || C == aFillColor)
					break;
				colors[i + current.y * w] = aFillColor;
				if (current.y + 1 < h)
				{
					C = colors[i + current.y * w + w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y + 1));
				}
				if (current.y - 1 >= 0)
				{
					C = colors[i + current.y * w - w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y - 1));
				}
			}
		}
		aTex.SetPixels(colors);
	}

	// FloodFill another texture using this texture for the algorith
	public static void FloodFillBasedOnAnotherTex (
		this Texture2D aTex, Texture2D otherTex,
		int aX, int aY, Color aFillColor) 
	{
		List<int> fillIndices = otherTex.getFloodFillIndices (aX, aY, aFillColor);
		Color[] colors = aTex.GetPixels ();
		foreach (int fillIndex in fillIndices) {
			colors [fillIndex] = aFillColor;
		}
		aTex.SetPixels (colors);
	}

	public static void FloodFillBorder(this Texture2D aTex, int aX, int aY, Color aFillColor, Color aBorderColor)
	{
		int w = aTex.width;
		int h = aTex.height;
		Color[] colors = aTex.GetPixels();
		byte[] checkedPixels = new byte[colors.Length];
		Color refCol = aBorderColor;
		Queue<Point> nodes = new Queue<Point>();
		nodes.Enqueue(new Point(aX, aY));
		while (nodes.Count > 0)
		{
			Point current = nodes.Dequeue();

			for (int i = current.x; i < w; i++)
			{
				if (checkedPixels[i + current.y * w] > 0 || colors[i + current.y * w] == refCol)
					break;
				colors[i + current.y * w] = aFillColor;
				checkedPixels[i + current.y * w] = 1;
				if (current.y + 1 < h)
				{
					if (checkedPixels[i + current.y * w + w] == 0 && colors[i + current.y * w + w] != refCol)
						nodes.Enqueue(new Point(i, current.y + 1));
				}
				if (current.y - 1 >= 0)
				{
					if (checkedPixels[i + current.y * w - w] == 0 && colors[i + current.y * w - w] != refCol)
						nodes.Enqueue(new Point(i, current.y - 1));
				}
			}
			for (int i = current.x - 1; i >= 0; i--)
			{
				if (checkedPixels[i + current.y * w] > 0 || colors[i + current.y * w] == refCol)
					break;
				colors[i + current.y * w] = aFillColor;
				checkedPixels[i + current.y * w] = 1;
				if (current.y + 1 < h)
				{
					if (checkedPixels[i + current.y * w + w] == 0 && colors[i + current.y * w + w] != refCol)
						nodes.Enqueue(new Point(i, current.y + 1));
				}
				if (current.y - 1 >= 0)
				{
					if (checkedPixels[i + current.y * w - w] == 0 && colors[i + current.y * w - w] != refCol)
						nodes.Enqueue(new Point(i, current.y - 1));
				}
			}
		}
		aTex.SetPixels(colors);
	}

	// get indicies that would be flood filled
	public static List<int> getFloodFillIndices(this Texture2D aTex, int aX, int aY, Color aFillColor)
	{
		List<int> indices = new List<int> ();

		int w = aTex.width;
		int h = aTex.height;
		Color[] colors = aTex.GetPixels();
		Color refCol = colors[aX + aY * w];
		Queue<Point> nodes = new Queue<Point>();
		nodes.Enqueue(new Point(aX, aY));
		while (nodes.Count > 0)
		{
			Point current = nodes.Dequeue();
			for (int i = current.x; i < w; i++)
			{
				Color C = colors[i + current.y * w];
				if (C != refCol || C == aFillColor)
					break;
				colors[i + current.y * w] = aFillColor;
				indices.Add (i + current.y * w);
				if (current.y + 1 < h)
				{
					C = colors[i + current.y * w + w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y + 1));
				}
				if (current.y - 1 >= 0)
				{
					C = colors[i + current.y * w - w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y - 1));
				}
			}
			for (int i = current.x - 1; i >= 0; i--)
			{
				Color C = colors[i + current.y * w];
				if (C != refCol || C == aFillColor)
					break;
				colors[i + current.y * w] = aFillColor;
				indices.Add (i + current.y * w);
				if (current.y + 1 < h)
				{
					C = colors[i + current.y * w + w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y + 1));
				}
				if (current.y - 1 >= 0)
				{
					C = colors[i + current.y * w - w];
					if (C == refCol && C != aFillColor)
						nodes.Enqueue(new Point(i, current.y - 1));
				}
			}
		}
		return indices;
	}
		

}

