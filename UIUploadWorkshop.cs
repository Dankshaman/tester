using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts;
using I2.Loc;
using NewNet;
using Steamworks;
using UnityEngine;

// Token: 0x02000357 RID: 855
public class UIUploadWorkshop : MonoBehaviour
{
	// Token: 0x0600288D RID: 10381 RVA: 0x0011E6E8 File Offset: 0x0011C8E8
	private void Start()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.uploadButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnUploadClicked));
		UIEventListener uieventListener2 = UIEventListener.Get(this.updateButton);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnUpdateClicked));
		EventDelegate.Add(this.InfoOptionsButtonUpload.onClick, new EventDelegate.Callback(this.OnInfoOptionsClicked));
		EventDelegate.Add(this.InfoOptionsButtonUpdate.onClick, new EventDelegate.Callback(this.OnInfoOptionsClicked));
		EventDelegate.Add(this.updateWorkshopIdPopup.GetComponent<UIButton>().onClick, new EventDelegate.Callback(this.ClickWorkshopId));
		EventDelegate.Add(this.updateWorkshopIdPopup.onChange, new EventDelegate.Callback(this.UpdateWorkshopIdChange));
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x0011E7C8 File Offset: 0x0011C9C8
	private void OnDestroy()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.uploadButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnUploadClicked));
		UIEventListener uieventListener2 = UIEventListener.Get(this.updateButton);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnUpdateClicked));
		EventDelegate.Remove(this.InfoOptionsButtonUpload.onClick, new EventDelegate.Callback(this.OnInfoOptionsClicked));
		EventDelegate.Remove(this.InfoOptionsButtonUpdate.onClick, new EventDelegate.Callback(this.OnInfoOptionsClicked));
		EventDelegate.Remove(this.updateWorkshopIdPopup.GetComponent<UIButton>().onClick, new EventDelegate.Callback(this.ClickWorkshopId));
		EventDelegate.Remove(this.updateWorkshopIdPopup.onChange, new EventDelegate.Callback(this.UpdateWorkshopIdChange));
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x0011E8A8 File Offset: 0x0011CAA8
	private void OnEnable()
	{
		if (Network.peerType != NetworkPeerMode.Disconnected && Network.isClient)
		{
			Chat.LogError("Only the host can upload to the Workshop.", true);
			base.gameObject.SetActive(false);
			return;
		}
		if (this.ModType == SteamManager.ModInfo.ModType.Save)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.CheckLocalFiles();
			this.title.value = NetworkSingleton<GameOptions>.Instance.GameName;
		}
		else
		{
			if (string.IsNullOrEmpty(Language.CSVFilenameFromCode[Language.CurrentLanguageCode]))
			{
				Chat.LogError("You cannot upload a default translation to the Workshop. Export and create your own.", true);
				base.gameObject.SetActive(false);
				return;
			}
			CSV currentCustomCSVFromDisk = Language.GetCurrentCustomCSVFromDisk();
			if (currentCustomCSVFromDisk == null)
			{
				Chat.LogError("Error loading translation csv from disk.", true);
				base.gameObject.SetActive(false);
				return;
			}
			int num = currentCustomCSVFromDisk.RowFromKey("_Title");
			if (num != -1)
			{
				this.title.value = currentCustomCSVFromDisk[1][num];
			}
		}
		this.InfoOptionsButtonUpload.transform.parent.gameObject.SetActive(this.ModType == SteamManager.ModInfo.ModType.Save);
		this.InfoOptionsButtonUpdate.transform.parent.gameObject.SetActive(this.ModType == SteamManager.ModInfo.ModType.Save);
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x0011E9C4 File Offset: 0x0011CBC4
	private void ClickWorkshopId()
	{
		if (Singleton<SteamManager>.Instance.bCheckingSubscribe)
		{
			Chat.LogError("Still querying Workshop some of your mods might not be listed.", true);
		}
		this.updateWorkshopIdPopup.items.Clear();
		this.updateWorkshopIdPopup.items.Add("<None>");
		List<SteamManager.ModInfo> yourMods = Singleton<SteamManager>.Instance.YourMods;
		for (int i = 0; i < yourMods.Count; i++)
		{
			SteamManager.ModInfo modInfo = yourMods[i];
			if (modInfo.Type == this.ModType)
			{
				this.updateWorkshopIdPopup.items.Add(modInfo.Title);
			}
		}
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x0011EA58 File Offset: 0x0011CC58
	private void UpdateWorkshopIdChange()
	{
		List<SteamManager.ModInfo> yourMods = Singleton<SteamManager>.Instance.YourMods;
		for (int i = 0; i < yourMods.Count; i++)
		{
			SteamManager.ModInfo modInfo = yourMods[i];
			if (modInfo.Type == this.ModType && modInfo.Title == this.updateWorkshopIdPopup.value)
			{
				this.workshopId.value = modInfo.Id.ToString();
				return;
			}
		}
		this.workshopId.value = "";
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x0011EAE2 File Offset: 0x0011CCE2
	public void OnTabClick()
	{
		this.upload.SetActive(this.uploadTab.value);
		this.update.SetActive(this.updateTab.value);
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x0011EB10 File Offset: 0x0011CD10
	private byte[] GetModData()
	{
		SteamManager.ModInfo.ModType modType = this.ModType;
		if (modType == SteamManager.ModInfo.ModType.Save)
		{
			return Json.GetBson(NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentState());
		}
		if (modType != SteamManager.ModInfo.ModType.Translation)
		{
			return null;
		}
		CSV currentCustomCSVFromDisk = Language.GetCurrentCustomCSVFromDisk();
		if (currentCustomCSVFromDisk != null)
		{
			return Encoding.UTF8.GetBytes(currentCustomCSVFromDisk.ToString());
		}
		Chat.LogError("Error loading translation csv from disk.", true);
		return null;
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x0011EB68 File Offset: 0x0011CD68
	public void OnUploadClicked(GameObject go)
	{
		if (string.IsNullOrEmpty(this.title.value))
		{
			Chat.LogError("Please give a name to your mod.", true);
			return;
		}
		if (string.IsNullOrEmpty(this.urlUpload.value))
		{
			Chat.LogError("Please supply an image URL for your mod.", true);
			return;
		}
		if (SteamCloudURL.IsCloudURL(this.urlUpload.value))
		{
			Chat.LogError("Cannot use a Cloud thumbnail image. Please use a local or other online image.", true);
			return;
		}
		SteamManager.ModUploadInfo modUploadInfo = new SteamManager.ModUploadInfo
		{
			Title = this.title.value,
			Desc = this.desc.value,
			ThumbnailURL = this.urlUpload.value,
			Tags = this.GetTags(),
			Data = this.GetModData()
		};
		if (modUploadInfo.Data == null)
		{
			return;
		}
		Singleton<SteamManager>.Instance.UploadToWorkshop(modUploadInfo);
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x0011EC34 File Offset: 0x0011CE34
	public void OnUpdateClicked(GameObject go)
	{
		if (!string.IsNullOrEmpty(this.urlUpload.value) && SteamCloudURL.IsCloudURL(this.urlUpload.value))
		{
			Chat.LogError("Cannot use a Cloud thumbnail image. Please use a local or other online image.", true);
			return;
		}
		PublishedFileId_t publisherId;
		try
		{
			publisherId = new PublishedFileId_t(Convert.ToUInt64(this.workshopId.value));
		}
		catch (Exception)
		{
			Chat.LogError("Improper Workshop ID.", true);
			return;
		}
		SteamManager.ModUploadInfo modUploadInfo = new SteamManager.ModUploadInfo
		{
			PublisherId = publisherId,
			ThumbnailURL = this.urlUpdate.value,
			Tags = this.GetTags(),
			Data = this.GetModData()
		};
		if (modUploadInfo.Data == null)
		{
			return;
		}
		Singleton<SteamManager>.Instance.UpdateToWorkshop(modUploadInfo);
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x0011ECF4 File Offset: 0x0011CEF4
	private void OnInfoOptionsClicked()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIInfoOptions.SetActive(!NetworkSingleton<NetworkUI>.Instance.GUIInfoOptions.activeSelf);
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x0011ED18 File Offset: 0x0011CF18
	private static IEnumerable<string> GetPlayingTimeTags()
	{
		List<string> list = new List<string>();
		int[] playingTime = NetworkSingleton<GameOptions>.Instance.PlayingTime;
		if (playingTime == null)
		{
			return list;
		}
		if (playingTime[0] == 0 && playingTime[1] == 0)
		{
			return list;
		}
		if (playingTime[0] == playingTime[1])
		{
			list.Add(UIUploadWorkshop.GetPlayTimeTag(playingTime[0]));
			return list;
		}
		if (playingTime[0] == 0)
		{
			list.Add(UIUploadWorkshop.GetPlayTimeTag(playingTime[1]));
			return list;
		}
		int num = Array.IndexOf<int>(Tags.PlayTime, playingTime[0]);
		int num2 = Array.IndexOf<int>(Tags.PlayTime, playingTime[1]);
		for (int i = num; i <= num2; i++)
		{
			if (i != 0)
			{
				list.Add(UIUploadWorkshop.GetPlayTimeTag(Tags.PlayTime[i]));
			}
		}
		return list;
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x0011EDB0 File Offset: 0x0011CFB0
	private static string GetPlayTimeTag(int value)
	{
		if (value == 180)
		{
			return "180+ minutes";
		}
		return string.Format("{0} minutes", value);
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x0011EDD0 File Offset: 0x0011CFD0
	private static IEnumerable<string> GetPlayerCountTags()
	{
		List<string> list = new List<string>();
		int[] playerCounts = NetworkSingleton<GameOptions>.Instance.PlayerCounts;
		if (playerCounts == null)
		{
			return list;
		}
		if (playerCounts[0] == 0 && playerCounts[1] == 0)
		{
			return list;
		}
		if (playerCounts[0] == playerCounts[1])
		{
			list.Add(UIUploadWorkshop.GetPlayerCountTag(playerCounts[0]));
			return list;
		}
		if (playerCounts[0] == 0)
		{
			list.Add(UIUploadWorkshop.GetPlayerCountTag(playerCounts[1]));
			return list;
		}
		for (int i = playerCounts[0]; i <= playerCounts[1]; i++)
		{
			if (i != 0)
			{
				list.Add(UIUploadWorkshop.GetPlayerCountTag(i));
			}
		}
		return list;
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x0011EE4C File Offset: 0x0011D04C
	private static string GetPlayerCountTag(int value)
	{
		if (value == 10)
		{
			return "10+";
		}
		return value.ToString();
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x0011EE60 File Offset: 0x0011D060
	public List<string> GetTags()
	{
		SteamManager.ModInfo.ModType modType = this.ModType;
		if (modType == SteamManager.ModInfo.ModType.Save)
		{
			List<string> list = new List<string>();
			list.AddRange(UIUploadWorkshop.GetPlayingTimeTags());
			list.AddRange(UIUploadWorkshop.GetPlayerCountTags());
			list.AddRange(NetworkSingleton<GameOptions>.Instance.Tags);
			if (!string.IsNullOrEmpty(NetworkSingleton<GameOptions>.Instance.GameType))
			{
				list.Add(NetworkSingleton<GameOptions>.Instance.GameType);
			}
			if (!string.IsNullOrEmpty(NetworkSingleton<GameOptions>.Instance.GameComplexity))
			{
				list.Add(NetworkSingleton<GameOptions>.Instance.GameComplexity);
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (string.Equals(base.tag, "Translation", StringComparison.OrdinalIgnoreCase))
				{
					list.RemoveAt(i);
				}
			}
			return list;
		}
		if (modType != SteamManager.ModInfo.ModType.Translation)
		{
			return null;
		}
		return new List<string>
		{
			"Translation",
			LocalizationManager.CurrentLanguage
		};
	}

	// Token: 0x04001AB2 RID: 6834
	public GameObject upload;

	// Token: 0x04001AB3 RID: 6835
	public GameObject update;

	// Token: 0x04001AB4 RID: 6836
	public GameObject uploadButton;

	// Token: 0x04001AB5 RID: 6837
	public GameObject updateButton;

	// Token: 0x04001AB6 RID: 6838
	public UIToggle uploadTab;

	// Token: 0x04001AB7 RID: 6839
	public UIToggle updateTab;

	// Token: 0x04001AB8 RID: 6840
	public UIInput workshopId;

	// Token: 0x04001AB9 RID: 6841
	public UIInput title;

	// Token: 0x04001ABA RID: 6842
	public UIInput desc;

	// Token: 0x04001ABB RID: 6843
	public UIInput urlUpdate;

	// Token: 0x04001ABC RID: 6844
	public UIInput urlUpload;

	// Token: 0x04001ABD RID: 6845
	public UIPopupList updateWorkshopIdPopup;

	// Token: 0x04001ABE RID: 6846
	public UIButton InfoOptionsButtonUpload;

	// Token: 0x04001ABF RID: 6847
	public UIButton InfoOptionsButtonUpdate;

	// Token: 0x04001AC0 RID: 6848
	public SteamManager.ModInfo.ModType ModType;
}
