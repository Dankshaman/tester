using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class WebsiteWWW : MonoBehaviour
{
	// Token: 0x06002900 RID: 10496 RVA: 0x00120C5F File Offset: 0x0011EE5F
	private void Start()
	{
		SteamManager.OverrideIsSubscribedAppTrue = this.OverrideIsSubscribedAppTrue;
		base.StartCoroutine(this.WWWInfoWebsite());
		NetworkEvents.OnServerInitialized += this.ServerInitialized;
		NetworkEvents.OnConnectedToServer += this.ConnectedToServer;
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x00120C9B File Offset: 0x0011EE9B
	private void OnDestroy()
	{
		this.CleanupSpotlightThumbnails();
		NetworkEvents.OnServerInitialized -= this.ServerInitialized;
		NetworkEvents.OnConnectedToServer -= this.ConnectedToServer;
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x00120CC5 File Offset: 0x0011EEC5
	private void ServerInitialized()
	{
		this.CleanupSpotlightThumbnails();
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x00120CC5 File Offset: 0x0011EEC5
	private void ConnectedToServer()
	{
		this.CleanupSpotlightThumbnails();
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x00120CD0 File Offset: 0x0011EED0
	private void CleanupSpotlightThumbnails()
	{
		foreach (KeyValuePair<string, UIGridMenu.GridButtonSpotlight> keyValuePair in this.spotLightDictionary)
		{
			if (Singleton<CustomLoadingManager>.Instance)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(keyValuePair.Key, new Action<CustomTextureContainer>(this.SpotlightImage), true);
			}
		}
		this.spotLightDictionary.Clear();
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x00120D58 File Offset: 0x0011EF58
	private IEnumerator WWWInfoWebsite()
	{
		Singleton<DLCManager>.Instance.DLCWebsites = this.WebsiteJson.websiteInfo.DLCs;
		DataRequest dataRequest = DataRequest.Get(SteamManager.IsInPrivateBetaBranch() ? "http://berserk-games.com/websiteinfobeta/" : "http://berserk-games.com/websiteinfo/", null);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		WebsiteWWW.WebsiteInfo websiteInfo;
		if (dataRequest.isError)
		{
			Chat.LogError("Error getting website info. Please check your firewall/antivirus/internet.", true);
			if (PlayerPrefs.HasKey("WebsiteWWW.WebsiteInfo"))
			{
				websiteInfo = Json.Load<WebsiteWWW.WebsiteInfo>(PlayerPrefs.GetString("WebsiteWWW.WebsiteInfo"));
			}
			else
			{
				websiteInfo = this.WebsiteJson.websiteInfo;
			}
		}
		else
		{
			websiteInfo = Json.Load<WebsiteWWW.WebsiteInfo>(dataRequest.text);
			PlayerPrefs.SetString("WebsiteWWW.WebsiteInfo", dataRequest.text);
		}
		if (this.UseLocalWebsiteInfo)
		{
			websiteInfo = this.WebsiteJson.websiteInfo;
		}
		Singleton<DLCManager>.Instance.DLCWebsites = websiteInfo.DLCs;
		SteamLobbyManager.LOBBY_PAGES = websiteInfo.ServerBrowser / 50;
		if (Utilities.GetVersionFromString(websiteInfo.VersionNumber) > Utilities.GetVersionFromString(NetworkSingleton<NetworkUI>.Instance.VersionNumber))
		{
			Chat.LogError("Tabletop Simulator is out-of-date! Latest version: " + websiteInfo.VersionNumber, true);
		}
		for (int i = 0; i < websiteInfo.Bans.Count; i++)
		{
			string text = websiteInfo.Bans[i];
			if (!GlobalBanList.BannedSteamIds.Contains(text))
			{
				Debug.Log("Add ban: " + text);
				GlobalBanList.BannedSteamIds.Add(text);
			}
		}
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			List<WebsiteWWW.SpotlightInfo> spotlights = websiteInfo.Spotlights;
			if (spotlights.Count > 4)
			{
				spotlights.Randomize<WebsiteWWW.SpotlightInfo>();
				for (int j = spotlights.Count - 1; j > 3; j--)
				{
					spotlights.RemoveAt(j);
				}
			}
			foreach (WebsiteWWW.SpotlightInfo spotlightInfo in spotlights)
			{
				UIGridMenu.GridButtonSpotlight gridButtonSpotlight = new UIGridMenu.GridButtonSpotlight();
				gridButtonSpotlight.Name = spotlightInfo.Name;
				gridButtonSpotlight.ClickURL = spotlightInfo.ClickURL;
				gridButtonSpotlight.ButtonColor = Colour.ColourFromRGBHex("1C97FFFF");
				gridButtonSpotlight.ThemeNormalAs = UIPalette.UI.Motif;
				gridButtonSpotlight.ThemeHoverAs = UIPalette.UI.MotifHighlightA;
				gridButtonSpotlight.ThemeLabelAs = UIPalette.UI.Label;
				this.spotLightDictionary.Add(spotlightInfo.ImageURL, gridButtonSpotlight);
				Singleton<CustomLoadingManager>.Instance.Texture.Load(spotlightInfo.ImageURL, new Action<CustomTextureContainer>(this.SpotlightImage), false, false, false, true, false, false, 512, CustomLoadingManager.LoadType.Auto);
			}
		}
		dataRequest.Dispose();
		yield break;
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x00120D68 File Offset: 0x0011EF68
	public void SpotlightImage(CustomTextureContainer customTextureContainer)
	{
		this.spotLightDictionary[customTextureContainer.nonCodeStrippedURL].Thumbnail = customTextureContainer.texture;
		this.SplotlightsGridMenu.gameObject.SetActive(true);
		this.SplotlightsGridMenu.Load<UIGridMenu.GridButtonSpotlight>(new List<UIGridMenu.GridButtonSpotlight>(this.spotLightDictionary.Values), 1, "", true, true);
	}

	// Token: 0x04001AE7 RID: 6887
	public bool UseLocalWebsiteInfo;

	// Token: 0x04001AE8 RID: 6888
	public bool OverrideIsSubscribedAppTrue;

	// Token: 0x04001AE9 RID: 6889
	public WebsiteJsonGenerator WebsiteJson;

	// Token: 0x04001AEA RID: 6890
	public UIGridMenuSpotlights SplotlightsGridMenu;

	// Token: 0x04001AEB RID: 6891
	private const string WebsiteInfoURL = "http://berserk-games.com/websiteinfo/";

	// Token: 0x04001AEC RID: 6892
	private const string WebsiteInfoURLBeta = "http://berserk-games.com/websiteinfobeta/";

	// Token: 0x04001AED RID: 6893
	private const string WEBSITE_INFO_PREF = "WebsiteWWW.WebsiteInfo";

	// Token: 0x04001AEE RID: 6894
	private Dictionary<string, UIGridMenu.GridButtonSpotlight> spotLightDictionary = new Dictionary<string, UIGridMenu.GridButtonSpotlight>();

	// Token: 0x020007A1 RID: 1953
	[Serializable]
	public class SpotlightInfo
	{
		// Token: 0x04002CCF RID: 11471
		public string Name;

		// Token: 0x04002CD0 RID: 11472
		public string ClickURL;

		// Token: 0x04002CD1 RID: 11473
		public string ImageURL;
	}

	// Token: 0x020007A2 RID: 1954
	[Serializable]
	public class CreditsInfo
	{
		// Token: 0x04002CD2 RID: 11474
		public string Name;

		// Token: 0x04002CD3 RID: 11475
		public string Description;
	}

	// Token: 0x020007A3 RID: 1955
	[Serializable]
	public class WebsiteInfo
	{
		// Token: 0x04002CD4 RID: 11476
		public string VersionNumber;

		// Token: 0x04002CD5 RID: 11477
		public int ServerBrowser = 1500;

		// Token: 0x04002CD6 RID: 11478
		public List<string> Bans = new List<string>();

		// Token: 0x04002CD7 RID: 11479
		public List<WebsiteWWW.SpotlightInfo> Spotlights = new List<WebsiteWWW.SpotlightInfo>();

		// Token: 0x04002CD8 RID: 11480
		public List<DLCWebsiteInfo> DLCs = new List<DLCWebsiteInfo>();
	}
}
