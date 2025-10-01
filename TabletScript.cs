using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000250 RID: 592
public class TabletScript : NetworkBehavior
{
	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06001F59 RID: 8025 RVA: 0x000DFFCC File Offset: 0x000DE1CC
	// (set) Token: 0x06001F5A RID: 8026 RVA: 0x000DFFD4 File Offset: 0x000DE1D4
	public bool HasFocus
	{
		get
		{
			return this.hasFocus;
		}
		set
		{
			this.hasFocus = value;
			if (this.browser)
			{
				this.browser.EnableInput = this.hasFocus;
			}
		}
	}

	// Token: 0x06001F5B RID: 8027 RVA: 0x000DFFFC File Offset: 0x000DE1FC
	private void Awake()
	{
		this.browser = base.GetComponent<Browser>();
		this.GUIConfirmTablet = NetworkSingleton<NetworkUI>.Instance.GUIConnected.transform.root.gameObject.AddChild(this.GUIConfirmTablet);
		float num = 0f;
		if (TabletScript.GUILayer - 50 != 0)
		{
			num = ((float)TabletScript.GUILayer - 50f) / 100f;
		}
		this.GUIConfirmTablet.GetComponent<UIPanel>().depth = ++TabletScript.GUILayer;
		this.GUIConfirmTablet.transform.position = new Vector3(this.GUIConfirmTablet.transform.position.x + num, this.GUIConfirmTablet.transform.position.y + num, this.GUIConfirmTablet.transform.position.z);
		this.GUIConfirmTablet.transform.RoundLocalPosition();
		this.GUIConfirmTablet.gameObject.SetActive(false);
		this.ThisUIInput.SelectAllTextOnClick = true;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x000E0118 File Offset: 0x000DE318
	private void Start()
	{
		if (this.browser)
		{
			if (Network.isServer && this.OverrideURL != "")
			{
				if (TabletScript.ApprovedURLS.Contains(this.OverrideURL) || TabletScript.AutoconfirmURLChange)
				{
					this.LoadURL(this.OverrideURL);
				}
				else if (!TabletScript.WhiteListId.Contains(-1) && !TabletScript.BlackListId.Contains(-1))
				{
					this.SetConfirmGUI(-1, this.OverrideURL);
				}
				else if (TabletScript.WhiteListId.Contains(-1))
				{
					this.LoadURL(this.OverrideURL);
				}
			}
			this.browser.UIHandler = TTSClickMeshBrowserUI.Create(base.gameObject);
		}
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x000E01D0 File Offset: 0x000DE3D0
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		UnityEngine.Object.Destroy(this.GUIConfirmTablet);
		if (NetworkSingleton<NetworkUI>.Instance && NetworkSingleton<NetworkUI>.Instance.GUITabletWindow && NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.activeSelf && NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().CurrentTablet == this)
		{
			NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.SetActive(false);
		}
		if (NetworkSingleton<ManagerPhysicsObject>.Instance)
		{
			bool flag = false;
			using (List<NetworkPhysicsObject>.Enumerator enumerator = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.tabletScript)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				TabletScript.GUILayer = 50;
			}
		}
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x000E02C0 File Offset: 0x000DE4C0
	private void Update()
	{
		if (this.browser && this.browser.Url != this.prevURL)
		{
			this.prevURL = this.browser.Url;
			this.OnURLChange(this.prevURL);
		}
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x000E030F File Offset: 0x000DE50F
	public bool CheckHit()
	{
		return this.browser && ((TTSClickMeshBrowserUI)this.browser.UIHandler).CheckHit();
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x000E0338 File Offset: 0x000DE538
	private void OnURLChange(string URL)
	{
		if (string.IsNullOrEmpty(URL))
		{
			return;
		}
		if (URL != this.SetURL && (Network.isServer || !this.bFirstURLChange))
		{
			base.networkView.RPC<string>(RPCTarget.Others, new Action<string>(this.SetURLrpc), URL);
		}
		this.bFirstURLChange = false;
		this.SetURL = "";
		this.CurrentURL = URL;
		this.ThisUIInput.value = this.CurrentURL;
		if (NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().CurrentTablet == this)
		{
			NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().OnURLChange(URL);
		}
	}

	// Token: 0x06001F61 RID: 8033 RVA: 0x000E03E0 File Offset: 0x000DE5E0
	[Remote("Permissions/Tablets")]
	public void SetURLrpc(string URL)
	{
		if (string.IsNullOrEmpty(URL))
		{
			return;
		}
		if (Network.sender == Network.player)
		{
			return;
		}
		if (this.CurrentURL != URL && this.browser)
		{
			int num = NetworkID.IDFromNetworkPlayer(Network.sender);
			if (!TabletScript.ApprovedURLS.Contains(URL))
			{
				if (!TabletScript.WhiteListId.Contains(num) && !TabletScript.BlackListId.Contains(num))
				{
					this.SetConfirmGUI(num, URL);
					return;
				}
				if (TabletScript.BlackListId.Contains(num))
				{
					return;
				}
			}
			this.SetURL = URL;
			this.LoadURL(URL);
		}
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x000E047C File Offset: 0x000DE67C
	private void SetConfirmGUI(int id, string URL)
	{
		if (!this.GUIConfirmTablet.activeSelf)
		{
			this.GUIConfirmTablet.GetComponent<UIConfirmTablet>().CurrentTablet = this;
			this.GUIConfirmTablet.GetComponent<UIConfirmTablet>().PlayerID = id;
			this.GUIConfirmTablet.GetComponent<UIConfirmTablet>().URL = URL;
			this.GUIConfirmTablet.SetActive(true);
		}
	}

	// Token: 0x06001F63 RID: 8035 RVA: 0x000E04D5 File Offset: 0x000DE6D5
	private void OnPlayerConnect(NetworkPlayer NP)
	{
		if (Network.isServer && this.CurrentURL != "")
		{
			base.networkView.RPC<string>(NP, new Action<string>(this.SetURLrpc), this.CurrentURL);
		}
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x000E0510 File Offset: 0x000DE710
	public void GUIBack()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Tablets, -1))
		{
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (this.browser && this.browser.CanGoBack)
		{
			this.browser.GoBack();
		}
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x000E0564 File Offset: 0x000DE764
	public void GUIForward()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Tablets, -1))
		{
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		if (this.browser && this.browser.CanGoForward)
		{
			this.browser.GoForward();
		}
	}

	// Token: 0x06001F66 RID: 8038 RVA: 0x000E05B6 File Offset: 0x000DE7B6
	public void GUIHome()
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Tablets, -1))
		{
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		this.LoadURL("https://www.google.com/");
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x000E05E4 File Offset: 0x000DE7E4
	public void LoadSearchURL(string url)
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Tablets, -1))
		{
			return;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		string text = url;
		string text2 = text.ToLower();
		if (text2.EndsWith(".pdf") && !text2.Contains("docs.google.com/viewer?url=") && !text2.Contains("drive.google.com/file"))
		{
			text = "https://docs.google.com/viewer?url=" + text;
		}
		if (!text.Contains("."))
		{
			text = "http://www.google.com/search?q=" + text;
		}
		if (!text.Contains("://"))
		{
			text = "http://" + text;
		}
		this.LoadURL(text);
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x000E0688 File Offset: 0x000DE888
	public void LoadURL(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return;
		}
		string text = url.Trim();
		if (DataRequest.IsLocalFile(text) || text.StartsWith("localGame://", StringComparison.OrdinalIgnoreCase))
		{
			Chat.LogError("Tablet cannot load local urls.", true);
			return;
		}
		if (this.browser)
		{
			this.browser.LoadURL(url, true);
		}
	}

	// Token: 0x06001F69 RID: 8041 RVA: 0x000E06E4 File Offset: 0x000DE8E4
	public void OpenOnUI()
	{
		if (this.browser)
		{
			NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.SetActive(false);
			NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().CurrentTablet = this;
			NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.SetActive(true);
		}
	}

	// Token: 0x04001334 RID: 4916
	public static bool TabletHasFocus = false;

	// Token: 0x04001335 RID: 4917
	public static bool AutoconfirmURLChange = false;

	// Token: 0x04001336 RID: 4918
	private bool hasFocus;

	// Token: 0x04001337 RID: 4919
	public Browser browser;

	// Token: 0x04001338 RID: 4920
	public const string Home_URL = "https://www.google.com/";

	// Token: 0x04001339 RID: 4921
	public UIInput ThisUIInput;

	// Token: 0x0400133A RID: 4922
	public string CurrentURL = "";

	// Token: 0x0400133B RID: 4923
	public string SetURL = "";

	// Token: 0x0400133C RID: 4924
	public string OverrideURL = "";

	// Token: 0x0400133D RID: 4925
	public static List<int> WhiteListId = new List<int>();

	// Token: 0x0400133E RID: 4926
	public static List<int> BlackListId = new List<int>();

	// Token: 0x0400133F RID: 4927
	public static readonly List<string> ApprovedURLS = new List<string>
	{
		"https://docs.google.com/viewer?url=https://images-cdn.fantasyflightgames.com/filer_public/11/c6/11c61988-bb60-428f-b614-9c3a952f070b/cosmic-encounter-rulebook.pdf",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8OEZ5X1JkdlM4MG8/view?pli=1",
		"https://docs.google.com/viewer?url=https://images-cdn.fantasyflightgames.com/filer_public/ac/fc/acfc2145-19c0-4ca8-a0cf-e9f4bee0f987/cosmic-incursion-rules.pdf",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8TEt3WkdsWVBFZlE/view?pli=1",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8X05QYXVMRE8yb3c/view?pli=1",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8a1A1Z2lIV3hra00/view?pref=2&pli=1",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8ZDZRT3pBaDlXcm8/view?pref=2&pli=1",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8eF83bDUydlJOM0U/view?pref=2&pli=1",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8RE4xUjR6M3N0Nm8/view?pref=2&pli=1",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8UUd4LUE1azc4dEU/view?pref=2&pli=1",
		"https://docs.google.com/viewer?url=https://images-cdn.fantasyflightgames.com/filer_public/5f/20/5f2076a9-989a-4316-bade-c74ddd9f0e0f/cosmic-conflict-rules1-updated.pdf",
		"https://docs.google.com/viewer?url=https://images-cdn.fantasyflightgames.com/filer_public/ba/7b/ba7bffc2-88ff-4fd3-a425-f5ce055f557c/ca_rulessheet_eng.pdf",
		"https://docs.google.com/viewer?url=https://images-cdn.fantasyflightgames.com/filer_public/18/e5/18e59188-640a-4a93-b81a-2ac0f4198c17/ce05_rulesheet_eng.pdf",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8RE4xUjR6M3N0Nm8/view",
		"https://drive.google.com/file/d/0B9XeBXHwfx_8eVN5eWxISWktMzg/view",
		"https://drive.google.com/file/d/0B6lv1zLpNOaQY2RwUkc5TnpELU0/view",
		"https://drive.google.com/file/d/0B6lv1zLpNOaQUkU3VExYV0xjTkU/view",
		"https://docs.google.com/viewer?url=https://images-cdn.fantasyflightgames.com/filer_public/d0/fc/d0fc81f0-6489-4c50-a69f-d77bedabaabe/ce06-rulesheet-eng.pdf"
	};

	// Token: 0x04001340 RID: 4928
	public GameObject GUIConfirmTablet;

	// Token: 0x04001341 RID: 4929
	private static int GUILayer = 50;

	// Token: 0x04001342 RID: 4930
	private string prevURL;

	// Token: 0x04001343 RID: 4931
	private bool bFirstURLChange = true;
}
