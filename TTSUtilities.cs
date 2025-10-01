using System;
using UnityEngine;

// Token: 0x0200024D RID: 589
public class TTSUtilities
{
	// Token: 0x06001F2D RID: 7981 RVA: 0x000DE5D1 File Offset: 0x000DC7D1
	public static void CopyToClipboard(string copyText)
	{
		GUIUtility.systemCopyBuffer = copyText;
		Chat.Log(Language.Translate("{0} copied to clipboard", copyText), Colour.Blue, ChatMessageType.All, true);
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x000DE5F0 File Offset: 0x000DC7F0
	public static string CleanName(NetworkPhysicsObject npo)
	{
		if (!string.IsNullOrEmpty(npo.Name))
		{
			return npo.Name;
		}
		return Utilities.CleanName(npo.gameObject.name);
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x000DE616 File Offset: 0x000DC816
	public static string CleanName(ObjectState OS)
	{
		if (!string.IsNullOrEmpty(OS.Nickname))
		{
			return OS.Nickname;
		}
		return Utilities.CleanName(OS.Name);
	}

	// Token: 0x06001F30 RID: 7984 RVA: 0x000DE638 File Offset: 0x000DC838
	public static void DestroyChildren(Transform T)
	{
		int childCount = T.childCount;
		for (int i = 0; i < childCount; i++)
		{
			UnityEngine.Object.Destroy(T.GetChild(i).gameObject);
		}
		if (T.gameObject.GetComponent<UIGrid>())
		{
			T.gameObject.GetComponent<UIGrid>().GetChildList().Clear();
		}
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x000DE690 File Offset: 0x000DC890
	public static void OpenURL(string URL)
	{
		if (SteamManager.bSteam && Singleton<SteamManager>.Instance.IsOverlayEnable() && !Application.isEditor && (URL.Contains("steamcommunity.com") || URL.Contains("steampowered.com")))
		{
			Singleton<SteamManager>.Instance.OpenURLOverlay(URL);
			return;
		}
		Application.OpenURL(URL);
	}
}
