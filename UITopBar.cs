using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000353 RID: 851
public class UITopBar : MonoBehaviour
{
	// Token: 0x06002847 RID: 10311 RVA: 0x0011C618 File Offset: 0x0011A818
	private void Awake()
	{
		EventManager.OnPlayerPromoted += this.PlayerPromoted;
		EventManager.OnCanFlipTable += this.CanFlipTable;
		EventManager.OnChangePlayerColor += this.ChangePlayerColor;
		EventDelegate.Add(this.GamesButton.onClick, new EventDelegate.Callback(this.OnClickGames));
		EventDelegate.Add(this.ObjectsButton.onClick, new EventDelegate.Callback(this.OnClickObjects));
		EventDelegate.Add(this.ScriptingButton.onClick, new EventDelegate.Callback(this.OnClickScripting));
		EventDelegate.Add(this.NotebookButton.onClick, new EventDelegate.Callback(this.OnClickNotebook));
		EventDelegate.Add(this.TableFlipButton.onClick, new EventDelegate.Callback(this.OnClickTableFlip));
		EventDelegate.Add(this.MenuButton.onClick, new EventDelegate.Callback(this.OnClickMenu));
		EventDelegate.Add(this.MusicButton.onClick, new EventDelegate.Callback(this.OnClickMusic));
		EventDelegate.Add(this.OptionsButton.GetComponent<UIPopupList>().onChange, new EventDelegate.Callback(this.OnChangeOptions));
		EventDelegate.Add(this.UploadButton.GetComponent<UIPopupList>().onChange, new EventDelegate.Callback(this.OnChangeUpload));
		EventDelegate.Add(this.ModdingButton.GetComponent<UIPopupList>().onChange, new EventDelegate.Callback(this.OnChangeModding));
	}

	// Token: 0x06002848 RID: 10312 RVA: 0x0011C78C File Offset: 0x0011A98C
	private void Start()
	{
		this.UpdateButtonDisables();
		NGUIHelper.ButtonDisable(this.ScriptingButton, Network.isClient, null, null);
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x0011C7C4 File Offset: 0x0011A9C4
	private void OnDestroy()
	{
		EventManager.OnPlayerPromoted -= this.PlayerPromoted;
		EventManager.OnCanFlipTable -= this.CanFlipTable;
		EventManager.OnChangePlayerColor -= this.ChangePlayerColor;
		EventDelegate.Remove(this.GamesButton.onClick, new EventDelegate.Callback(this.OnClickGames));
		EventDelegate.Remove(this.ObjectsButton.onClick, new EventDelegate.Callback(this.OnClickObjects));
		EventDelegate.Remove(this.ScriptingButton.onClick, new EventDelegate.Callback(this.OnClickScripting));
		EventDelegate.Remove(this.NotebookButton.onClick, new EventDelegate.Callback(this.OnClickNotebook));
		EventDelegate.Remove(this.TableFlipButton.onClick, new EventDelegate.Callback(this.OnClickTableFlip));
		EventDelegate.Remove(this.MenuButton.onClick, new EventDelegate.Callback(this.OnClickMenu));
		EventDelegate.Remove(this.MusicButton.onClick, new EventDelegate.Callback(this.OnClickMusic));
		EventDelegate.Remove(this.OptionsButton.GetComponent<UIPopupList>().onChange, new EventDelegate.Callback(this.OnChangeOptions));
		EventDelegate.Remove(this.UploadButton.GetComponent<UIPopupList>().onChange, new EventDelegate.Callback(this.OnChangeUpload));
		EventDelegate.Remove(this.ModdingButton.GetComponent<UIPopupList>().onChange, new EventDelegate.Callback(this.OnChangeModding));
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x0011C935 File Offset: 0x0011AB35
	private void OnClickGames()
	{
		NGUIHelper.Toggle(NetworkSingleton<NetworkUI>.Instance.GUIGames);
	}

	// Token: 0x0600284B RID: 10315 RVA: 0x0011C946 File Offset: 0x0011AB46
	private void OnClickObjects()
	{
		NGUIHelper.Toggle(NetworkSingleton<NetworkUI>.Instance.GUIObjects);
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x0011C958 File Offset: 0x0011AB58
	private void OnClickScripting()
	{
		if (Network.isClient)
		{
			Chat.LogError("Only the Host can use Scripting.", true);
			return;
		}
		if (!NetworkSingleton<NetworkUI>.Instance.GUILuaNotepad.gameObject.activeSelf)
		{
			NetworkSingleton<NetworkUI>.Instance.GUILuaNotepad.Init(null);
			return;
		}
		NetworkSingleton<NetworkUI>.Instance.GUILuaNotepad.gameObject.SetActive(false);
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x0011C9B4 File Offset: 0x0011ABB4
	private void OnClickTags()
	{
		if (Network.isClient)
		{
			Chat.LogError("Only the Host can edit tags.", true);
			return;
		}
		GameObject gameObject = NetworkSingleton<NetworkUI>.Instance.GUITagEditor.gameObject;
		gameObject.SetActive(!gameObject.activeSelf);
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x0011C9E6 File Offset: 0x0011ABE6
	private void OnClickCustomUI()
	{
		Debug.Log("Clicked On New Button");
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x0011C9F2 File Offset: 0x0011ABF2
	private void OnClickNotebook()
	{
		NGUIHelper.Toggle(NetworkSingleton<NetworkUI>.Instance.GUINotepad);
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x0011CA03 File Offset: 0x0011AC03
	private void OnClickTableFlip()
	{
		UIDialog.Show("Flip the table", "Yes", "No", new Action(NetworkSingleton<NetworkUI>.Instance.FlipTable), null);
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x0011CA2A File Offset: 0x0011AC2A
	private void OnClickMenu()
	{
		NGUIHelper.Toggle(NetworkSingleton<NetworkUI>.Instance.GUIMenu);
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x0011CA3B File Offset: 0x0011AC3B
	private void OnClickMusic()
	{
		NGUIHelper.Toggle(Singleton<UICustomMusicPlayer>.Instance.gameObject);
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x0011CA4C File Offset: 0x0011AC4C
	private void OnChangeComponents()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIHostSelection(this.ObjectsButton.GetComponent<UIPopupList>().value);
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x0011CA68 File Offset: 0x0011AC68
	private void OnChangeOptions()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIHostSelection(this.OptionsButton.GetComponent<UIPopupList>().value);
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x0011CA84 File Offset: 0x0011AC84
	private void OnChangeUpload()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIMenuSelection(this.UploadButton.GetComponent<UIPopupList>().value);
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x0011CAA0 File Offset: 0x0011ACA0
	private void OnChangeModding()
	{
		string value = this.ModdingButton.GetComponent<UIPopupList>().value;
		if (value == "Scripting")
		{
			this.OnClickScripting();
			return;
		}
		if (!(value == "Tags"))
		{
			NetworkSingleton<NetworkUI>.Instance.GUIMenuSelection(this.ModdingButton.GetComponent<UIPopupList>().value);
			return;
		}
		this.OnClickTags();
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x0011CB04 File Offset: 0x0011AD04
	private void UpdateButtonDisables()
	{
		bool flag = !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		NGUIHelper.ButtonDisable(this.GamesButton, flag, null, null);
		NGUIHelper.ButtonDisable(this.ObjectsButton, flag, null, null);
		NGUIHelper.ButtonDisable(this.UploadButton, flag, null, null);
		NGUIHelper.ButtonDisable(this.ModdingButton, flag, null, null);
		if (flag)
		{
			this.OptionsButton.GetComponent<UIPopupList>().items = new List<string>
			{
				"Game Keys"
			};
		}
		else
		{
			this.OptionsButton.GetComponent<UIPopupList>().items = new List<string>
			{
				"Info",
				"Server",
				"Permissions",
				"Grid",
				"Lighting",
				"Physics",
				"Hands",
				"Turns",
				"Game Keys"
			};
		}
		if (Network.isServer)
		{
			this.ModdingButton.GetComponent<UIPopupList>().items = new List<string>
			{
				"Workshop Upload",
				"Cloud Manager",
				"Scripting",
				"Tags"
			};
			return;
		}
		this.ModdingButton.GetComponent<UIPopupList>().items = new List<string>
		{
			"Workshop Upload",
			"Cloud Manager",
			"Scripting"
		};
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x0011CCB4 File Offset: 0x0011AEB4
	private void PlayerPromoted(bool isPromoted, int id)
	{
		if (id != NetworkID.ID)
		{
			return;
		}
		this.UpdateButtonDisables();
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x0011CCC8 File Offset: 0x0011AEC8
	private void CanFlipTable(bool canFlip)
	{
		NGUIHelper.ButtonDisable(this.TableFlipButton, !canFlip, null, null);
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x0011CCF6 File Offset: 0x0011AEF6
	private void ChangePlayerColor(Color newColor, int id)
	{
		if (id != NetworkID.ID)
		{
			return;
		}
		if (NetworkSingleton<NetworkUI>.Instance)
		{
			this.CanFlipTable(NetworkSingleton<NetworkUI>.Instance.bCanFlipTable);
		}
	}

	// Token: 0x04001A91 RID: 6801
	public UIGrid Grid;

	// Token: 0x04001A92 RID: 6802
	public UIButton GamesButton;

	// Token: 0x04001A93 RID: 6803
	public UIButton ObjectsButton;

	// Token: 0x04001A94 RID: 6804
	public UIButton OptionsButton;

	// Token: 0x04001A95 RID: 6805
	public UIButton UploadButton;

	// Token: 0x04001A96 RID: 6806
	public UIButton ScriptingButton;

	// Token: 0x04001A97 RID: 6807
	public UIButton NotebookButton;

	// Token: 0x04001A98 RID: 6808
	public UIButton TableFlipButton;

	// Token: 0x04001A99 RID: 6809
	public UIButton MenuButton;

	// Token: 0x04001A9A RID: 6810
	public UIButton MusicButton;

	// Token: 0x04001A9B RID: 6811
	public UIButton ToolsButton;

	// Token: 0x04001A9C RID: 6812
	public UIButton ModdingButton;
}
