using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D1 RID: 721
public class UIGameManager : MonoBehaviour
{
	// Token: 0x06002333 RID: 9011 RVA: 0x000FA27F File Offset: 0x000F847F
	private void Start()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.backButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnBackButtonClicked));
		this.currentTabState = UIGameManager.TabState.None;
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000FA2B4 File Offset: 0x000F84B4
	private void OnDestroy()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.backButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnBackButtonClicked));
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000FA2E2 File Offset: 0x000F84E2
	public void Init()
	{
		this.currentTabState = UIGameManager.TabState.None;
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000FA2EC File Offset: 0x000F84EC
	private void DisplayGameTab()
	{
		this.CleanupItems();
		for (int i = 0; i < this.defaultGames.Length; i++)
		{
			this.defaultGames[i].gameObject.SetActive(true);
		}
		this.currentTabState = UIGameManager.TabState.Game;
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000FA32C File Offset: 0x000F852C
	private void CleanupItems()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			UnityEngine.Object.Destroy(this.items[i].gameObject);
		}
		this.items.Clear();
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000025B8 File Offset: 0x000007B8
	public void OnBackButtonClicked(GameObject go)
	{
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000FA370 File Offset: 0x000F8570
	public void OnTabClick()
	{
		if (this.currentTabState != UIGameManager.TabState.Game)
		{
			this.DisplayGameTab();
		}
	}

	// Token: 0x0400164F RID: 5711
	public UIToggle gameTab;

	// Token: 0x04001650 RID: 5712
	public UIToggle chestTab;

	// Token: 0x04001651 RID: 5713
	public UIToggle saveTab;

	// Token: 0x04001652 RID: 5714
	public GameObject backButton;

	// Token: 0x04001653 RID: 5715
	public UIGameManagerItem[] defaultGames;

	// Token: 0x04001654 RID: 5716
	public GameObject gameManagerItemPrefab;

	// Token: 0x04001655 RID: 5717
	public UIGrid itemGrid;

	// Token: 0x04001656 RID: 5718
	private List<UIGameManagerItem> items = new List<UIGameManagerItem>();

	// Token: 0x04001657 RID: 5719
	private UIGameManager.TabState currentTabState;

	// Token: 0x02000724 RID: 1828
	private enum TabState
	{
		// Token: 0x04002AC8 RID: 10952
		None,
		// Token: 0x04002AC9 RID: 10953
		Game,
		// Token: 0x04002ACA RID: 10954
		Chest,
		// Token: 0x04002ACB RID: 10955
		Save
	}
}
