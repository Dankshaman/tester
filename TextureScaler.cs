using System;
using UnityEngine;

// Token: 0x02000258 RID: 600
public class TextureScaler
{
	// Token: 0x06001F9B RID: 8091 RVA: 0x000E1CB4 File Offset: 0x000DFEB4
	public static Texture2D scaled(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
	{
		Rect source = new Rect(0f, 0f, (float)width, (float)height);
		TextureScaler._gpu_scale(src, width, height, mode);
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, true);
		texture2D.Resize(width, height);
		texture2D.ReadPixels(source, 0, 0, true);
		return texture2D;
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x000E1CFC File Offset: 0x000DFEFC
	public static void scale(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear)
	{
		Rect source = new Rect(0f, 0f, (float)width, (float)height);
		TextureScaler._gpu_scale(tex, width, height, mode);
		tex.Resize(width, height);
		tex.ReadPixels(source, 0, 0, true);
		tex.Apply(true);
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x000E1D44 File Offset: 0x000DFF44
	private static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
	{
		src.filterMode = fmode;
		src.Apply(true);
		Graphics.SetRenderTarget(new RenderTexture(width, height, 32));
		GL.LoadPixelMatrix(0f, 1f, 1f, 0f);
		GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
		Graphics.DrawTexture(new Rect(0f, 0f, 1f, 1f), src);
	}
}
