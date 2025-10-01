using System;
using System.IO;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class ScreenshotScript : MonoBehaviour
{
	// Token: 0x06001B5C RID: 7004 RVA: 0x000BCB04 File Offset: 0x000BAD04
	public static Texture2D TakeScreenshot(float Supersampling = 1f)
	{
		return ScreenshotScript.TakeScreenshot((int)((float)Screen.width * Supersampling), (int)((float)Screen.height * Supersampling), null, false);
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x000BCB20 File Offset: 0x000BAD20
	public static Texture2D TakeScreenshot(int resWidth, int resHeight, Camera camera = null, bool transparentBackground = false)
	{
		if (camera == null)
		{
			camera = HoverScript.MainCamera;
		}
		RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 32, RenderTextureFormat.ARGB32);
		renderTexture.mipMapBias = MipmapBiasTextures.CurrentMipMapBias;
		renderTexture.antiAliasing = 8;
		renderTexture.filterMode = FilterMode.Trilinear;
		renderTexture.anisoLevel = 9;
		CameraClearFlags clearFlags = camera.clearFlags;
		if (transparentBackground)
		{
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = new Color(1f, 1f, 1f, 0f);
		}
		camera.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
		camera.Render();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)resWidth, (float)resHeight), 0, 0, false);
		texture2D.Apply();
		camera.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture);
		if (transparentBackground)
		{
			camera.clearFlags = clearFlags;
		}
		return texture2D;
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x000BCBF4 File Offset: 0x000BADF4
	public static void SaveScreenshot()
	{
		string str = "TTS_Screenshot_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
		string path = DirectoryScript.screenshotsFilePath + "//" + str;
		Chat.Log("Screenshot taken.", Colour.Blue, ChatMessageType.All, true);
		ScreenshotScript.SaveTexture(ScreenshotScript.TakeScreenshot(2f), path, true, true);
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x000BCC57 File Offset: 0x000BAE57
	public static void SaveThumbnail(string path)
	{
		ScreenshotScript.SaveTexture(ScreenshotScript.TakeScreenshot(256, 256, null, false), path, false, true);
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x000BCC72 File Offset: 0x000BAE72
	public static void SaveThumbnail(string path, GameObject target)
	{
		Singleton<AltZoomCamera>.Instance.SaveThumbnail(path, target);
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x000BCC80 File Offset: 0x000BAE80
	public static void SaveTexture(Texture2D texture, string path, bool log = false, bool destroyTexture = true)
	{
		try
		{
			byte[] bytes = texture.EncodeToPNG();
			File.WriteAllBytes(path, bytes);
			if (log)
			{
				Chat.Log("Image saved to " + SerializationScript.GetCleanPath(path), ChatMessageType.Game);
			}
		}
		catch (Exception e)
		{
			Chat.LogException("saving image", e, true, false);
		}
		if (destroyTexture)
		{
			UnityEngine.Object.Destroy(texture);
		}
	}
}
