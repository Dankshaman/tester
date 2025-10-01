using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using MoonSharp.Interpreter;
using NewNet;
using UI.Xml;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000383 RID: 899
public class XmlUIScript : NetworkBehavior
{
	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06002A15 RID: 10773 RVA: 0x0012C1A4 File Offset: 0x0012A3A4
	// (set) Token: 0x06002A16 RID: 10774 RVA: 0x0012C1AC File Offset: 0x0012A3AC
	public GraphicRaycaster raycaster { get; private set; }

	// Token: 0x06002A17 RID: 10775 RVA: 0x0012C1B8 File Offset: 0x0012A3B8
	private void Start()
	{
		this.startHappened = true;
		if (!this.xmlLayout)
		{
			this.isObject = true;
			GameObject gameObject = Resources.Load<GameObject>("XmlUIWorldCanvas");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity, base.gameObject.transform);
			gameObject2.transform.localPosition = gameObject.transform.localPosition;
			gameObject2.transform.localRotation = gameObject.transform.localRotation;
			gameObject2.transform.localScale = gameObject.transform.localScale;
			this.xmlLayout = gameObject2.GetComponentInChildren<XmlLayout>();
		}
		else
		{
			this.xmlui_code = "<!-- Xml UI. See documentation: https://api.tabletopsimulator.com/ui/introUI/ -->";
			EventManager.OnResetTable += this.OnResetTable;
			EventManager.OnHideCustomUI += this.OnHideCustomUI;
			EventManager.OnConfigSoundChange += this.OnConfigSoundChange;
			this.OnConfigSoundChange(ConfigSound.Settings);
			XmlLayoutResourceDatabase.instance.AddResource("Defaults/Sounds/ClickUI", NetworkSingleton<NetworkUI>.Instance.ButtonSound);
		}
		this.xmlLayout.GetComponent<XmlUILayoutController>().xmlUI = this;
		this.raycaster = this.xmlLayout.GetComponentInParent<GraphicRaycaster>();
		this.luaScript = base.GetComponent<LuaScript>();
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		EventManager.OnPlayerChangeColor += this.OnPlayerChangeColor;
		EventManager.OnChangePlayerTeam += this.OnChangePlayerTeam;
		EventManager.OnPlayerPromoted += this.OnPlayerPromoted;
		Application.logMessageReceived += this.Application_logMessageReceived;
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x0012C33C File Offset: 0x0012A53C
	private void OnDestroy()
	{
		if (!this.startHappened)
		{
			return;
		}
		EventManager.OnResetTable -= this.OnResetTable;
		EventManager.OnHideCustomUI -= this.OnHideCustomUI;
		EventManager.OnConfigSoundChange -= this.OnConfigSoundChange;
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		EventManager.OnPlayerChangeColor -= this.OnPlayerChangeColor;
		EventManager.OnChangePlayerTeam -= this.OnChangePlayerTeam;
		EventManager.OnPlayerPromoted -= this.OnPlayerPromoted;
		Application.logMessageReceived -= this.Application_logMessageReceived;
		this.CleanupCustomAssets();
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x0012C3E0 File Offset: 0x0012A5E0
	private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
	{
		if (type != LogType.Log && condition.StartsWith("[XmlLayout]"))
		{
			switch (type)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				Chat.LogError(condition, true);
				break;
			case LogType.Warning:
				Chat.LogWarning(condition, true);
				return;
			case LogType.Log:
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x0012C41D File Offset: 0x0012A61D
	public void Init(string xml, bool force = false)
	{
		if (!force && this.IsXmlNothing(xml))
		{
			this.FinishedLoading = true;
			return;
		}
		base.networkView.RPC<string, List<CustomAssetState>>(RPCTarget.All, new Action<string, List<CustomAssetState>>(this.RPCXml), xml, this.CustomAssets);
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x0012C454 File Offset: 0x0012A654
	private void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isClient)
		{
			return;
		}
		string xml = this.GetXml();
		if (this.IsXmlNothing(xml))
		{
			return;
		}
		base.networkView.RPC<string, List<CustomAssetState>>(player, new Action<string, List<CustomAssetState>>(this.RPCXml), xml, this.CustomAssets);
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x0012C49C File Offset: 0x0012A69C
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCXml(string xml, List<CustomAssetState> customAssets)
	{
		base.enabled = true;
		this.FinishedLoading = false;
		this.InitXmlCode = xml;
		this.CleanupCustomAssets();
		this.CustomAssets = customAssets;
		NetworkSingleton<NetworkUI>.Instance.GUIUIAssets.Reload(true);
		foreach (CustomAssetState customAssetState in this.CustomAssets)
		{
			if (customAssetState.Type == CustomAssetType.AssetBundle)
			{
				Singleton<CustomLoadingManager>.Instance.UIAssetbundle.Load(customAssetState.URL, new Action<CustomUIAssetbundleContainer>(this.OnAssetBundleFinish), null);
			}
			else if (customAssetState.Type == CustomAssetType.Image)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Load(customAssetState.URL, new Action<CustomTextureContainer>(this.OnTextureFinish), false, false, false, false, true, false, 8192, CustomLoadingManager.LoadType.Auto);
			}
		}
		base.StartCoroutine(this.WaitToLoad());
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x0012C58C File Offset: 0x0012A78C
	private IEnumerator WaitToLoad()
	{
		while (NetworkSingleton<PlayerManager>.Instance.MyPlayerState() == null)
		{
			yield return null;
		}
		yield return null;
		while (this.downloadsComplete < this.CustomAssets.Count)
		{
			yield return null;
		}
		this.OnLoadingComplete();
		yield break;
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x0012C59C File Offset: 0x0012A79C
	private void OnAssetBundleFinish(CustomUIAssetbundleContainer customAssetBundleContainer)
	{
		this.downloadsComplete++;
		string text = null;
		foreach (CustomAssetState customAssetState in this.CustomAssets)
		{
			if (customAssetState.URL == customAssetBundleContainer.nonCodeStrippedURL)
			{
				text = customAssetState.Name;
				break;
			}
		}
		if (customAssetBundleContainer.resources.Length != 0)
		{
			XmlLayoutResourceDatabase.instance.AddResource(text, customAssetBundleContainer.resources[0]);
		}
		foreach (UnityEngine.Object @object in customAssetBundleContainer.resources)
		{
			string path = text + "/" + @object.name;
			XmlLayoutResourceDatabase.instance.AddResource(path, @object);
		}
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x0012C674 File Offset: 0x0012A874
	private void OnTextureFinish(CustomTextureContainer customTextureContainer)
	{
		this.downloadsComplete++;
		if (!customTextureContainer.texture)
		{
			return;
		}
		string text = null;
		for (int i = 0; i < this.CustomAssets.Count; i++)
		{
			if (this.CustomAssets[i].URL == customTextureContainer.nonCodeStrippedURL)
			{
				text = this.CustomAssets[i].Name;
				break;
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Texture2D texture2D = customTextureContainer.texture as Texture2D;
		if (texture2D)
		{
			Sprite resource = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
			XmlLayoutResourceDatabase.instance.AddResource(text, resource);
			return;
		}
		Chat.LogError("Xml UI only supports Texture2D.", true);
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x0012C754 File Offset: 0x0012A954
	public bool IsXmlNothing(string xml)
	{
		return string.IsNullOrEmpty(xml) || xml == "<XmlLayout></XmlLayout>" || xml == "<!-- Xml UI. See documentation: https://api.tabletopsimulator.com/ui/introUI/ -->";
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x0012C778 File Offset: 0x0012A978
	private void RebuildLayout(bool force = false, bool exception = false)
	{
		bool activeSelf = this.xmlLayout.gameObject.activeSelf;
		this.xmlLayout.gameObject.SetActive(true);
		this.xmlLayout.RebuildLayout(force, exception);
		this.xmlLayout.gameObject.SetActive(activeSelf);
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x0012C7C8 File Offset: 0x0012A9C8
	private void OnLoadingComplete()
	{
		this.xmlLayout.Xml = "<XmlLayout>" + XmlUIScript.oneline_inserted_xmlui_code + this.InitXmlCode + "</XmlLayout>";
		try
		{
			this.RebuildLayout(true, true);
			this.FinishedLoading = true;
			this.CurrentXmlData = this.GetXmlData(this.InitXmlCode);
			if (this.CurrentXmlData == null)
			{
				this.CurrentXmlData = new XmlUIScript.XmlData();
			}
		}
		catch (Exception ex)
		{
			string text = string.Format("Error Building Xml UI on {0}: {1}", this.luaScript.GetScriptName(), ex.Message);
			Chat.LogError(text, true);
			Debug.Log(this.InitXmlCode);
			Debug.LogException(ex);
			LuaGlobalScriptManager.Instance.PushLuaErrorMessage(ex.ToString(), this.luaScript.guid, text);
		}
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x0012C894 File Offset: 0x0012AA94
	private void OnResetTable()
	{
		base.networkView.RPC(RPCTarget.All, new Action(this.RPCReset));
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x0012C8B0 File Offset: 0x0012AAB0
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void RPCReset()
	{
		this.FinishedLoading = false;
		this.xmlLayout.Xml = "<XmlLayout></XmlLayout>";
		this.RebuildLayout(false, false);
		this.InitXmlCode = "<!-- Xml UI. See documentation: https://api.tabletopsimulator.com/ui/introUI/ -->";
		this.xmlui_code = "<!-- Xml UI. See documentation: https://api.tabletopsimulator.com/ui/introUI/ -->";
		this.CurrentXmlData = new XmlUIScript.XmlData();
		this.CleanupCustomAssets();
		this.CustomAssets = new List<CustomAssetState>();
		this.downloadsComplete = 0;
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x0012C918 File Offset: 0x0012AB18
	public void CleanupCustomAssets()
	{
		for (int i = 0; i < this.CustomAssets.Count; i++)
		{
			if (Singleton<CustomLoadingManager>.Instance)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this.CustomAssets[i].URL, new Action<CustomTextureContainer>(this.OnTextureFinish), true);
			}
		}
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x0012C974 File Offset: 0x0012AB74
	private void OnHideCustomUI(bool enable)
	{
		this.xmlLayout.gameObject.SetActive(!enable);
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x0012C98A File Offset: 0x0012AB8A
	private void OnConfigSoundChange(ConfigSound.ConfigSoundState configSoundState)
	{
		UI.Xml.XmlElement.GlobalAudioVolume = SoundScript.GLOBAL_SOUND_MULTI;
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x0012C998 File Offset: 0x0012AB98
	public void ReceiveMessage(string methodName, string value, RectTransform source = null)
	{
		if (this.preventRecursiveEvents)
		{
			return;
		}
		string arg = null;
		if (source)
		{
			UI.Xml.XmlElement component = source.GetComponent<UI.Xml.XmlElement>();
			if (component)
			{
				arg = component.GetAttribute("id", null);
			}
		}
		base.networkView.RPC<string, string, string>(RPCTarget.Server, new Action<string, string, string>(this.RPCReceiveMessage), methodName, value, arg);
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x0012C9F0 File Offset: 0x0012ABF0
	[Remote(SendType.ReliableNoDelay)]
	private void RPCReceiveMessage(string methodName, string value, string id)
	{
		LuaScript luaScript = null;
		string functionName = methodName;
		if (methodName.Contains("/"))
		{
			string[] array = methodName.Split(new char[]
			{
				'/'
			});
			if (array.Length == 2)
			{
				if (string.Equals(array[0], "global", StringComparison.OrdinalIgnoreCase))
				{
					functionName = array[1];
					luaScript = LuaGlobalScriptManager.Instance;
				}
				else
				{
					NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(array[0]);
					if (!networkPhysicsObject)
					{
						Chat.LogError("Xml UI couldn't find Object with guid: " + array[0], true);
						return;
					}
					functionName = array[1];
					luaScript = networkPhysicsObject.luaGameObjectScript;
				}
			}
		}
		else if (this.isObject)
		{
			luaScript = base.GetComponent<LuaGameObjectScript>();
		}
		else
		{
			luaScript = LuaGlobalScriptManager.Instance;
		}
		int id2 = NetworkSingleton<NetworkUI>.Instance.bHotseat ? NetworkSingleton<NetworkUI>.Instance.CurrentHotseat : ((int)Network.sender.id);
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id2);
		if (luaScript)
		{
			luaScript.TryCall(functionName, new object[]
			{
				new LuaPlayer(playerState.stringColor, playerState.id),
				value,
				id
			});
		}
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x0012CAFF File Offset: 0x0012ACFF
	private void WaitToLoad(Action action)
	{
		Wait.Condition(action, () => this.FinishedLoading, float.PositiveInfinity, null);
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06002A2B RID: 10795 RVA: 0x0012CB1A File Offset: 0x0012AD1A
	public bool Loading
	{
		get
		{
			return !this.FinishedLoading;
		}
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x0012CB25 File Offset: 0x0012AD25
	public bool SetAttribute(string id, string name, string value)
	{
		if (!this.xmlLayout.GetElementById(id))
		{
			return false;
		}
		if (name == null || value == null)
		{
			return false;
		}
		base.networkView.RPC<string, string, string>(RPCTarget.All, new Action<string, string, string>(this.RPCSetAttribute), id, name, value);
		return true;
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x0012CB60 File Offset: 0x0012AD60
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetAttribute(string id, string name, string value)
	{
		this.WaitToLoad(delegate()
		{
			this.SetRuntimeAttribute(id, name, value);
			UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
			if (elementById)
			{
				this.preventRecursiveEvents = true;
				elementById.SetAndApplyAttribute(name, value);
				this.preventRecursiveEvents = false;
			}
		});
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x0012CBA4 File Offset: 0x0012ADA4
	private void SetRuntimeAttribute(string id, string name, string value)
	{
		XmlUIScript.XmlData xmlDataFromId = this.GetXmlDataFromId(this.CurrentXmlData, id);
		if (xmlDataFromId != null)
		{
			if (xmlDataFromId.attributes == null)
			{
				xmlDataFromId.attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			}
			xmlDataFromId.attributes.SetValue(name, value);
		}
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x0012CBE8 File Offset: 0x0012ADE8
	public bool SetAttributes(string id, Dictionary<string, string> attributes)
	{
		if (!this.xmlLayout.GetElementById(id))
		{
			return false;
		}
		foreach (KeyValuePair<string, string> keyValuePair in attributes)
		{
			if (keyValuePair.Key == null || keyValuePair.Value == null)
			{
				return false;
			}
		}
		base.networkView.RPC<string, Dictionary<string, string>>(RPCTarget.All, new Action<string, Dictionary<string, string>>(this.RPCSetAttributes), id, attributes);
		return true;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x0012CC78 File Offset: 0x0012AE78
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetAttributes(string id, Dictionary<string, string> attributes)
	{
		this.WaitToLoad(delegate()
		{
			XmlUIScript.XmlData xmlDataFromId = this.GetXmlDataFromId(this.CurrentXmlData, id);
			if (xmlDataFromId != null)
			{
				if (xmlDataFromId.attributes == null)
				{
					xmlDataFromId.attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}
				foreach (KeyValuePair<string, string> keyValuePair in attributes)
				{
					xmlDataFromId.attributes.SetValue(keyValuePair.Key, keyValuePair.Value);
				}
			}
			UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
			if (elementById)
			{
				this.preventRecursiveEvents = true;
				elementById.ApplyAttributes(attributes);
				this.preventRecursiveEvents = false;
			}
		});
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x0012CCB4 File Offset: 0x0012AEB4
	public string GetAttribute(string id, string name)
	{
		UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
		if (elementById)
		{
			return elementById.GetAttribute(name, null);
		}
		return null;
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x0012CCE0 File Offset: 0x0012AEE0
	public Dictionary<string, string> GetAttributes(string id)
	{
		UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
		if (elementById)
		{
			return elementById.attributes.AsDictionary;
		}
		return null;
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x0012CD0F File Offset: 0x0012AF0F
	public bool Show(string id)
	{
		if (!this.xmlLayout.GetElementById(id))
		{
			return false;
		}
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.RPCShow), id);
		return true;
	}

	// Token: 0x06002A34 RID: 10804 RVA: 0x0012CD40 File Offset: 0x0012AF40
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCShow(string id)
	{
		this.WaitToLoad(delegate()
		{
			this.SetRuntimeAttribute(id, "active", "true");
			UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
			if (elementById)
			{
				elementById.Show(false, null, false);
			}
		});
	}

	// Token: 0x06002A35 RID: 10805 RVA: 0x0012CD73 File Offset: 0x0012AF73
	public bool Hide(string id)
	{
		if (!this.xmlLayout.GetElementById(id))
		{
			return false;
		}
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.RPCHide), id);
		return true;
	}

	// Token: 0x06002A36 RID: 10806 RVA: 0x0012CDA4 File Offset: 0x0012AFA4
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCHide(string id)
	{
		this.WaitToLoad(delegate()
		{
			this.SetRuntimeAttribute(id, "active", "false");
			UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
			if (elementById)
			{
				elementById.Hide(false, null, false);
			}
		});
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x0012CDD7 File Offset: 0x0012AFD7
	public bool SetClass(string id, string name)
	{
		if (!this.xmlLayout.GetElementById(id))
		{
			return false;
		}
		base.networkView.RPC<string, string>(RPCTarget.All, new Action<string, string>(this.RPCSetClass), id, name);
		return true;
	}

	// Token: 0x06002A38 RID: 10808 RVA: 0x0012CE0C File Offset: 0x0012B00C
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetClass(string id, string name)
	{
		this.WaitToLoad(delegate()
		{
			this.SetRuntimeAttribute(id, "class", name);
			UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
			if (elementById)
			{
				string[] @class;
				if (name.Contains(" "))
				{
					@class = name.Split(new char[]
					{
						' '
					});
				}
				else
				{
					@class = new string[]
					{
						name
					};
				}
				elementById.SetClass(@class);
			}
		});
	}

	// Token: 0x06002A39 RID: 10809 RVA: 0x0012CE48 File Offset: 0x0012B048
	public string GetValue(string id)
	{
		XmlUIScript.XmlData xmlDataFromId = this.GetXmlDataFromId(this.CurrentXmlData, id);
		if (xmlDataFromId == null)
		{
			return null;
		}
		return xmlDataFromId.value;
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x0012CE6E File Offset: 0x0012B06E
	public bool SetValue(string id, string value)
	{
		if (!this.xmlLayout.GetElementById(id))
		{
			return false;
		}
		if (value == null)
		{
			return false;
		}
		base.networkView.RPC<string, string>(RPCTarget.All, new Action<string, string>(this.RPCSetValue), id, value);
		return true;
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x0012CEA8 File Offset: 0x0012B0A8
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCSetValue(string id, string value)
	{
		this.WaitToLoad(delegate()
		{
			XmlUIScript.XmlData xmlDataFromId = this.GetXmlDataFromId(this.CurrentXmlData, id);
			if (xmlDataFromId != null)
			{
				xmlDataFromId.value = value;
			}
			UI.Xml.XmlElement elementById = this.xmlLayout.GetElementById(id);
			if (elementById)
			{
				Text component = elementById.GetComponent<Text>();
				if (component)
				{
					component.text = XmlUIScript.ConvertRichText(value);
				}
			}
		});
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x0012CEE4 File Offset: 0x0012B0E4
	public static string ConvertRichText(string innerXml)
	{
		TextCode.LocalizeUIText(ref innerXml);
		innerXml = innerXml.Replace(" xmlns=\"XmlLayout\"", string.Empty).Replace(" xmlns=\"http://www.w3schools.com\"", string.Empty).Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty);
		innerXml = XmlUIScript.ReplaceIgnoreCase(innerXml, "<textcolor color=", "<color=");
		innerXml = XmlUIScript.ReplaceIgnoreCase(innerXml, "</textcolor", "</color");
		innerXml = XmlUIScript.ReplaceIgnoreCase(innerXml, "<textsize size=", "<size=");
		innerXml = XmlUIScript.ReplaceIgnoreCase(innerXml, "</textsize", "</size");
		innerXml = innerXml.Trim();
		innerXml = XmlUIScript.ReplaceIgnoreCase(innerXml, " {2,}", " ");
		for (int i = 0; i < XmlUIScript.XmlReplacements.Length; i++)
		{
			Replacement replacement = XmlUIScript.XmlReplacements[i];
			innerXml = XmlUIScript.ReplaceIgnoreCase(innerXml, replacement.from, replacement.to);
		}
		if (innerXml.Contains("\n"))
		{
			innerXml = innerXml.Replace("\\n", "\n");
			innerXml = string.Join("\n", (from s in innerXml.Split(new char[]
			{
				'\n'
			})
			select s.Trim()).ToArray<string>());
		}
		innerXml = StringExtensions.DecodeEncodedNonAsciiCharacters(innerXml);
		return innerXml;
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x0012D03A File Offset: 0x0012B23A
	private static string ReplaceIgnoreCase(string source, string match, string replace)
	{
		return Regex.Replace(source, match, replace, RegexOptions.IgnoreCase);
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x0012D045 File Offset: 0x0012B245
	public string GetXml()
	{
		return this.GetStringFromXmlData(this.CurrentXmlData);
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x0012D053 File Offset: 0x0012B253
	public bool SetXml(string xml)
	{
		if (xml == null)
		{
			return false;
		}
		this.Init(xml, true);
		return true;
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x0012D064 File Offset: 0x0012B264
	private XmlUIScript.XmlData GetXmlData(string xml)
	{
		string text = "<XML>" + xml + "</XML>";
		XElement xElement;
		try
		{
			xElement = XElement.Parse(text);
		}
		catch (XmlException exception)
		{
			Debug.LogException(exception);
			return null;
		}
		return this.GetXmlData(xElement);
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x0012D0B0 File Offset: 0x0012B2B0
	public XmlUIScript.XmlData GetXmlDataFromId(XmlUIScript.XmlData xmlData, string id)
	{
		string a;
		if (xmlData.attributes != null && xmlData.attributes.TryGetValue("id", out a) && a == id)
		{
			return xmlData;
		}
		for (int i = 0; i < xmlData.children.Count; i++)
		{
			XmlUIScript.XmlData xmlDataFromId = this.GetXmlDataFromId(xmlData.children[i], id);
			if (xmlDataFromId != null)
			{
				return xmlDataFromId;
			}
		}
		return null;
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x0012D114 File Offset: 0x0012B314
	public Table GetXmlTable(Script script)
	{
		return (Table)this.ConvertXmlDataToTable(script, this.CurrentXmlData)["children"];
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x0012D134 File Offset: 0x0012B334
	private XmlUIScript.XmlData GetXmlData(XElement xElement)
	{
		XmlUIScript.XmlData xmlData = new XmlUIScript.XmlData();
		xmlData.tag = xElement.Name.ToString();
		xmlData.attributes = xElement.Attributes().ToDictionary((XAttribute attribute) => attribute.Name.ToString(), (XAttribute attribute) => attribute.Value.ToString(), StringComparer.OrdinalIgnoreCase);
		if (this.HasRichText(xElement))
		{
			XmlReader xmlReader = xElement.CreateReader();
			xmlReader.MoveToContent();
			xmlData.value = xmlReader.ReadInnerXml();
		}
		else
		{
			XText xtext;
			if ((xtext = (xElement.FirstNode as XText)) != null)
			{
				xmlData.value = xtext.Value;
			}
			if (xElement.HasElements)
			{
				foreach (XElement xElement2 in xElement.Elements())
				{
					xmlData.children.Add(this.GetXmlData(xElement2));
				}
			}
		}
		return xmlData;
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x0012D244 File Offset: 0x0012B444
	private bool HasRichText(XElement xElement)
	{
		List<string> list = new List<string>
		{
			"b",
			"i",
			"textcolor",
			"textsize"
		};
		if (xElement.HasElements)
		{
			foreach (XElement xelement in xElement.Elements())
			{
				if (list.Contains(xelement.Name.ToString()))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x0012D2E0 File Offset: 0x0012B4E0
	public bool SetXmlTable(Script script, Table xmlTable)
	{
		if (xmlTable == null || xmlTable[1] == null)
		{
			this.luaScript.LogError("UI.setXmlTable", "table was null", null);
			return false;
		}
		if (xmlTable.Length != 0)
		{
			Table table = new Table(script);
			table["tag"] = "XML";
			table["value"] = string.Empty;
			table["children"] = xmlTable;
			xmlTable = table;
		}
		XmlUIScript.XmlData xmlData = this.ConvertTableToXmlData(xmlTable);
		string stringFromXmlData = this.GetStringFromXmlData(xmlData);
		return this.SetXml(stringFromXmlData);
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x0012D36C File Offset: 0x0012B56C
	public string GetStringFromXmlData(XmlUIScript.XmlData xmlData)
	{
		if (xmlData.tag == null)
		{
			return "";
		}
		XElement xelement;
		try
		{
			xelement = this.SetXmlData(xmlData);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return "";
		}
		if (xelement == null)
		{
			return "";
		}
		string text = string.Empty;
		if (xelement.Name == "XML")
		{
			using (IEnumerator<XElement> enumerator = xelement.Elements().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement arg = enumerator.Current;
					text = text + arg + "\n";
				}
				goto IL_A4;
			}
		}
		try
		{
			text = xelement.ToString();
		}
		catch (Exception)
		{
			return "";
		}
		IL_A4:
		StringBuilder stringBuilder = new StringBuilder(text.Length * 2);
		bool flag = true;
		foreach (char c in text)
		{
			if (flag && char.IsWhiteSpace(c))
			{
				stringBuilder.Append(' ');
				stringBuilder.Append(' ');
			}
			else
			{
				flag = (c == '\n');
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString().Replace("ttamp;t", "<").Replace("ttgt;t", ">");
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x0012D4CC File Offset: 0x0012B6CC
	private XElement SetXmlData(XmlUIScript.XmlData xmlData)
	{
		XElement xelement = new XElement(xmlData.tag);
		if (xmlData.value != null)
		{
			xelement.SetValue(xmlData.value.Replace("<", "ttamp;t").Replace(">", "ttgt;t"));
		}
		if (xmlData.attributes != null)
		{
			foreach (KeyValuePair<string, string> keyValuePair in xmlData.attributes)
			{
				XAttribute content = new XAttribute(keyValuePair.Key.Replace("\"", ""), keyValuePair.Value.Replace("\"", ""));
				xelement.Add(content);
			}
		}
		if (xmlData.children != null)
		{
			foreach (XmlUIScript.XmlData xmlData2 in xmlData.children)
			{
				xelement.Add(this.SetXmlData(xmlData2));
			}
		}
		return xelement;
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x0012D5F8 File Offset: 0x0012B7F8
	private XmlUIScript.XmlData ConvertTableToXmlData(Table table)
	{
		XmlUIScript.XmlData xmlData = new XmlUIScript.XmlData();
		xmlData.tag = table["tag"].ToString();
		if (table["value"] != null)
		{
			xmlData.value = table["value"].ToString();
		}
		if (table["attributes"] != null)
		{
			Table table2 = (Table)table["attributes"];
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (TablePair tablePair in table2.Pairs)
			{
				dictionary.Add(tablePair.Key.ToString(), tablePair.Value.ToString());
			}
			xmlData.attributes = dictionary;
		}
		else
		{
			xmlData.attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}
		if (table["children"] != null)
		{
			Table table3 = (Table)table["children"];
			if (table3.Length == 0 && table3["tag"] != null)
			{
				xmlData.children.Add(this.ConvertTableToXmlData(table3));
			}
			else
			{
				for (int i = 1; i <= table3.Length; i++)
				{
					if (((Table)table3[i])["tag"] != null)
					{
						xmlData.children.Add(this.ConvertTableToXmlData((Table)table3[i]));
					}
				}
			}
		}
		return xmlData;
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x0012D784 File Offset: 0x0012B984
	private Table ConvertXmlDataToTable(Script script, XmlUIScript.XmlData data)
	{
		Table table = new Table(script);
		table["tag"] = data.tag;
		if (data.value != null)
		{
			table["value"] = data.value;
		}
		else
		{
			table["value"] = null;
		}
		table["attributes"] = data.attributes;
		Table table2 = new Table(script);
		int num = 1;
		foreach (XmlUIScript.XmlData data2 in data.children)
		{
			table2[num] = this.ConvertXmlDataToTable(script, data2);
			num++;
		}
		table["children"] = table2;
		return table;
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x0012D850 File Offset: 0x0012BA50
	private void OnPlayerPromoted(bool isPromoted, int id)
	{
		if (id == NetworkID.ID)
		{
			this.UpdateVisibilities();
		}
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x0012D850 File Offset: 0x0012BA50
	private void OnChangePlayerTeam(bool join, int id)
	{
		if (id == NetworkID.ID)
		{
			this.UpdateVisibilities();
		}
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x0012D860 File Offset: 0x0012BA60
	private void OnPlayerChangeColor(PlayerState player)
	{
		if (player.id == NetworkID.ID)
		{
			this.UpdateVisibilities();
		}
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x0012D878 File Offset: 0x0012BA78
	private void UpdateVisibilities()
	{
		foreach (UI.Xml.XmlElement xmlElement in this.xmlLayout.GetComponentsInChildren<UI.Xml.XmlElement>(true))
		{
			if (xmlElement.HasAttribute("visibility"))
			{
				VisibilityAttribute.UpdateVisibility(xmlElement);
			}
		}
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x0012D8B7 File Offset: 0x0012BAB7
	public void AddCustomAsset(CustomAssetState customAssetState)
	{
		this.CustomAssets.Add(customAssetState);
		NetworkSingleton<NetworkUI>.Instance.GUIUIAssets.Reload(true);
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x0012D8D8 File Offset: 0x0012BAD8
	public void RemoveCustomAsset(CustomAssetState customAssetState)
	{
		Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(customAssetState.URL, new Action<CustomTextureContainer>(this.OnTextureFinish), true);
		this.CustomAssets.Remove(customAssetState);
		NetworkSingleton<NetworkUI>.Instance.GUIUIAssets.Reload(true);
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x0012D924 File Offset: 0x0012BB24
	public void UpdateCustomAsset(CustomAssetState original, CustomAssetState newAsset)
	{
		Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(original.URL, new Action<CustomTextureContainer>(this.OnTextureFinish), true);
		this.CustomAssets[this.CustomAssets.IndexOf(original)] = newAsset;
		NetworkSingleton<NetworkUI>.Instance.GUIUIAssets.Reload(true);
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x0012D97C File Offset: 0x0012BB7C
	public void PostLayoutRebuilt()
	{
		if (this.isObject)
		{
			Transform[] componentsInChildren = this.xmlLayout.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 17;
			}
		}
	}

	// Token: 0x04001CB4 RID: 7348
	public XmlLayout xmlLayout;

	// Token: 0x04001CB5 RID: 7349
	[NonSerialized]
	public string xmlui_code = "";

	// Token: 0x04001CB6 RID: 7350
	public List<CustomAssetState> CustomAssets = new List<CustomAssetState>();

	// Token: 0x04001CB7 RID: 7351
	private const string inserted_xmlui_code = "\r\n    <Defaults>\r\n\r\n    <Color name=\"white\" color=\"#FFFFFF\"/>\r\n    <Color name=\"brown\" color=\"#713B17\\\"/>\r\n    <Color name=\"red\" color=\"#DA1918\"/>\r\n    <Color name=\"orange\" color=\"#F4641D\"/>\r\n    <Color name=\"yellow\" color=\"#E7E52C\"/>\r\n    <Color name=\"green\" color=\"#31B32B\"/>\r\n    <Color name=\"teal\" color=\"#21B19B\"/>\r\n    <Color name=\"blue\" color=\"#1F87FF\"/>\r\n    <Color name=\"purple\" color=\"#A020F0\"/>\r\n    <Color name=\"pink\" color=\"#F570CE\"/>\r\n    <Color name=\"grey\" color=\"#AAAAAA\"/>\r\n    <Color name=\"black\" color=\"#191919\"/>\t\r\n\r\n    <Button onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <InputField onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <Toggle onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <ToggleButton onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <Slider onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <Dropdown onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n\r\n    </Defaults>\r\n    ";

	// Token: 0x04001CB8 RID: 7352
	private static readonly string oneline_inserted_xmlui_code = Regex.Replace("\r\n    <Defaults>\r\n\r\n    <Color name=\"white\" color=\"#FFFFFF\"/>\r\n    <Color name=\"brown\" color=\"#713B17\\\"/>\r\n    <Color name=\"red\" color=\"#DA1918\"/>\r\n    <Color name=\"orange\" color=\"#F4641D\"/>\r\n    <Color name=\"yellow\" color=\"#E7E52C\"/>\r\n    <Color name=\"green\" color=\"#31B32B\"/>\r\n    <Color name=\"teal\" color=\"#21B19B\"/>\r\n    <Color name=\"blue\" color=\"#1F87FF\"/>\r\n    <Color name=\"purple\" color=\"#A020F0\"/>\r\n    <Color name=\"pink\" color=\"#F570CE\"/>\r\n    <Color name=\"grey\" color=\"#AAAAAA\"/>\r\n    <Color name=\"black\" color=\"#191919\"/>\t\r\n\r\n    <Button onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <InputField onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <Toggle onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <ToggleButton onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <Slider onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n    <Dropdown onClickSound=\"Defaults/Sounds/ClickUI\" audioVolume=\"0.3\"/>\r\n\r\n    </Defaults>\r\n    ", "\\r\\n?|\\n", "");

	// Token: 0x04001CB9 RID: 7353
	private const string default_xmlui_code = "<!-- Xml UI. See documentation: https://api.tabletopsimulator.com/ui/introUI/ -->";

	// Token: 0x04001CBA RID: 7354
	private const string empty_xmlui_code = "<XmlLayout></XmlLayout>";

	// Token: 0x04001CBB RID: 7355
	private const string BeginXml = "<XmlLayout>";

	// Token: 0x04001CBC RID: 7356
	private const string EndXml = "</XmlLayout>";

	// Token: 0x04001CBD RID: 7357
	private bool FinishedLoading;

	// Token: 0x04001CBE RID: 7358
	private int downloadsComplete;

	// Token: 0x04001CBF RID: 7359
	private bool isObject;

	// Token: 0x04001CC0 RID: 7360
	private LuaScript luaScript;

	// Token: 0x04001CC1 RID: 7361
	private string InitXmlCode = "";

	// Token: 0x04001CC2 RID: 7362
	private XmlUIScript.XmlData CurrentXmlData = new XmlUIScript.XmlData();

	// Token: 0x04001CC4 RID: 7364
	private bool startHappened;

	// Token: 0x04001CC5 RID: 7365
	private bool preventRecursiveEvents;

	// Token: 0x04001CC6 RID: 7366
	private static readonly Replacement[] XmlReplacements = new Replacement[]
	{
		new Replacement("&lt;", "<"),
		new Replacement("&gt;", ">"),
		new Replacement("&amp;", "&"),
		new Replacement("&nbsp;", "\u00a0"),
		new Replacement("<br/>", "\n"),
		new Replacement("<br />", "\n")
	};

	// Token: 0x020007B8 RID: 1976
	public class XmlData
	{
		// Token: 0x04002D34 RID: 11572
		public string tag;

		// Token: 0x04002D35 RID: 11573
		public string value;

		// Token: 0x04002D36 RID: 11574
		public Dictionary<string, string> attributes;

		// Token: 0x04002D37 RID: 11575
		public List<XmlUIScript.XmlData> children = new List<XmlUIScript.XmlData>();
	}
}
