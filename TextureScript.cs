using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000259 RID: 601
public static class TextureScript
{
	// Token: 0x06001F9F RID: 8095 RVA: 0x000E1DC8 File Offset: 0x000DFFC8
	public static void ApplyTextSettings(Texture Text)
	{
		Text.filterMode = FilterMode.Trilinear;
		Text.anisoLevel = 9;
		Texture2D texture2D;
		if ((texture2D = (Text as Texture2D)) != null && CustomLoadingManager.CompressedFormats.Contains(texture2D.format))
		{
			Text.mipMapBias = MipmapBiasTextures.CurrentMipMapBias;
		}
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x000E1E0C File Offset: 0x000E000C
	public static Texture2D ResizeTexture(Texture2D pSource, TextureScript.ImageFilterMode pFilterMode, float pScale)
	{
		float num = (float)Mathf.RoundToInt((float)pSource.width * pScale);
		float num2 = (float)Mathf.RoundToInt((float)pSource.height * pScale);
		return TextureScript.ResizeTexture(pSource, pFilterMode, (int)num, (int)num2);
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x000E1E44 File Offset: 0x000E0044
	public static Texture2D ResizeTexture(Texture2D pSource, TextureScript.ImageFilterMode pFilterMode, int xWidth, int xHeight)
	{
		Color[] pixels = pSource.GetPixels(0);
		Vector2 vector = new Vector2((float)pSource.width, (float)pSource.height);
		Texture2D texture2D = new Texture2D(xWidth, xHeight, TextureFormat.RGBA32, false);
		int num = xWidth * xHeight;
		Color[] array = new Color[num];
		Vector2 vector2 = new Vector2(vector.x / (float)xWidth, vector.y / (float)xHeight);
		Vector2 vector3 = default(Vector2);
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)i % (float)xWidth;
			float num3 = Mathf.Floor((float)i / (float)xWidth);
			vector3.x = num2 / (float)xWidth * vector.x;
			vector3.y = num3 / (float)xHeight * vector.y;
			if (pFilterMode == TextureScript.ImageFilterMode.Nearest)
			{
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				int num4 = (int)(vector3.y * vector.x + vector3.x);
				array[i] = pixels[num4];
			}
			else if (pFilterMode == TextureScript.ImageFilterMode.Biliner)
			{
				float t = vector3.x - Mathf.Floor(vector3.x);
				float t2 = vector3.y - Mathf.Floor(vector3.y);
				int num5 = (int)(Mathf.Floor(vector3.y) * vector.x + Mathf.Floor(vector3.x));
				int num6 = (int)(Mathf.Floor(vector3.y) * vector.x + Mathf.Ceil(vector3.x));
				int num7 = (int)(Mathf.Ceil(vector3.y) * vector.x + Mathf.Floor(vector3.x));
				int num8 = (int)(Mathf.Ceil(vector3.y) * vector.x + Mathf.Ceil(vector3.x));
				array[i] = Color.Lerp(Color.Lerp(pixels[num5], pixels[num6], t), Color.Lerp(pixels[num7], pixels[num8], t), t2);
			}
			else if (pFilterMode == TextureScript.ImageFilterMode.Average)
			{
				int num9 = (int)Mathf.Max(Mathf.Floor(vector3.x - vector2.x * 0.5f), 0f);
				int num10 = (int)Mathf.Min(Mathf.Ceil(vector3.x + vector2.x * 0.5f), vector.x);
				int num11 = (int)Mathf.Max(Mathf.Floor(vector3.y - vector2.y * 0.5f), 0f);
				int num12 = (int)Mathf.Min(Mathf.Ceil(vector3.y + vector2.y * 0.5f), vector.y);
				Color a = default(Color);
				float num13 = 0f;
				for (int j = num11; j < num12; j++)
				{
					for (int k = num9; k < num10; k++)
					{
						a += pixels[(int)((float)j * vector.x + (float)k)];
						num13 += 1f;
					}
				}
				array[i] = a / num13;
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06001FA2 RID: 8098 RVA: 0x000E2168 File Offset: 0x000E0368
	public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, true);
		Color[] pixels = texture2D.GetPixels(0);
		float num = 1f / (float)targetWidth;
		float num2 = 1f / (float)targetHeight;
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = source.GetPixelBilinear(num * ((float)i % (float)targetWidth), num2 * Mathf.Floor((float)(i / targetWidth)));
		}
		texture2D.SetPixels(pixels, 0);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x000E21E2 File Offset: 0x000E03E2
	public static bool HasAlpha(Texture texture)
	{
		return texture is Texture2D && TextureScript.TextureFormatWithAlpha.Contains(((Texture2D)texture).format);
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x000E2204 File Offset: 0x000E0404
	public static void UpdateMaterialTransparency(Material mat)
	{
		if (!mat.HasProperty("_Color"))
		{
			return;
		}
		string text = mat.shader.name;
		if (!text.StartsWith("Marmoset"))
		{
			return;
		}
		bool flag = mat.color.a < 1f;
		bool flag2 = mat.mainTexture != null && TextureScript.HasAlpha(mat.mainTexture);
		if (flag || flag2)
		{
			string text2 = "Marmoset/Transparent";
			if (!flag)
			{
				text2 = "Marmoset/Transparent/Cutout";
			}
			if (!text.StartsWith(text2))
			{
				text = TextureScript.ResetShaderName(text);
				Shader shader = Shader.Find(text.Replace("Marmoset", text2));
				if (shader != null)
				{
					mat.shader = shader;
					return;
				}
			}
		}
		else if (text.StartsWith("Marmoset/Transparent"))
		{
			Shader shader2 = Shader.Find(TextureScript.ResetShaderName(text));
			if (shader2 != null)
			{
				mat.shader = shader2;
			}
		}
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x000E22DF File Offset: 0x000E04DF
	private static string ResetShaderName(string shaderName)
	{
		return shaderName.Replace("Marmoset/Transparent/Cutout", "Marmoset").Replace("Marmoset/Transparent", "Marmoset");
	}

	// Token: 0x0400136A RID: 4970
	public static readonly List<TextureFormat> TextureFormatWithAlpha = new List<TextureFormat>
	{
		TextureFormat.DXT5,
		TextureFormat.ARGB32,
		TextureFormat.RGBA32
	};

	// Token: 0x020006FB RID: 1787
	public enum ImageFilterMode
	{
		// Token: 0x04002A3F RID: 10815
		Nearest,
		// Token: 0x04002A40 RID: 10816
		Biliner,
		// Token: 0x04002A41 RID: 10817
		Average
	}
}
