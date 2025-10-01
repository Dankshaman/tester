using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class UIFinder : Singleton<UIFinder>
{
	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06002308 RID: 8968 RVA: 0x000F9B64 File Offset: 0x000F7D64
	// (set) Token: 0x06002309 RID: 8969 RVA: 0x000F9B71 File Offset: 0x000F7D71
	private bool open
	{
		get
		{
			return this.finder.activeSelf;
		}
		set
		{
			this.finder.SetActive(value);
		}
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000F9B7F File Offset: 0x000F7D7F
	private void Start()
	{
		this.gridMenuGames = NetworkSingleton<NetworkUI>.Instance.GUIGames.GetComponent<UIGridMenuGames>();
		this.gridMenuObjects = NetworkSingleton<NetworkUI>.Instance.GUIObjects.GetComponent<UIGridMenuObjects>();
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000F9BAC File Offset: 0x000F7DAC
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftControl))
		{
			this.Toggle();
		}
		if (this.open)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.SelectUp();
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.SelectDown();
			}
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				this.Submit();
			}
		}
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000F9C1B File Offset: 0x000F7E1B
	private void Toggle()
	{
		if (this.open)
		{
			this.Close();
			return;
		}
		this.Open();
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000F9C32 File Offset: 0x000F7E32
	private void Open()
	{
		this.open = true;
		UICamera.selectedObject = null;
		this.finderInput.SelectAllTextOnClick = true;
		UICamera.selectedObject = this.finderInput.gameObject;
		this.LoadGames();
		this.LoadObjects();
		this.LoadOptions();
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000F9C70 File Offset: 0x000F7E70
	private async void LoadGames()
	{
		List<UIGridMenu.GridButton> buttons = await this.gridMenuGames.AsyncGetAllGridButtons(this.finder);
		this.GamesGridMenu.Load<UIGridMenu.GridButton>(buttons, 1, "Games", true, false);
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000F9CAC File Offset: 0x000F7EAC
	private async void LoadObjects()
	{
		List<UIGridMenu.GridButton> buttons = await this.gridMenuObjects.AsyncGetAllObjects();
		this.ObjectsGridMenu.Load<UIGridMenu.GridButton>(buttons, 1, "Objects", true, false);
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000F9CE8 File Offset: 0x000F7EE8
	private async void LoadOptions()
	{
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000F9D1C File Offset: 0x000F7F1C
	private async void LoadSettings()
	{
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000F9D50 File Offset: 0x000F7F50
	private async void UpdateSearch(string search)
	{
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000F9D81 File Offset: 0x000F7F81
	private void Close()
	{
		this.open = false;
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000025B8 File Offset: 0x000007B8
	private void SelectUp()
	{
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000025B8 File Offset: 0x000007B8
	private void SelectDown()
	{
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Submit()
	{
	}

	// Token: 0x0400163C RID: 5692
	public GameObject finder;

	// Token: 0x0400163D RID: 5693
	public UIInput finderInput;

	// Token: 0x0400163E RID: 5694
	public UIGridMenu GamesGridMenu;

	// Token: 0x0400163F RID: 5695
	public UIGridMenu ObjectsGridMenu;

	// Token: 0x04001640 RID: 5696
	private UIGridMenuGames gridMenuGames;

	// Token: 0x04001641 RID: 5697
	private UIGridMenuObjects gridMenuObjects;
}
