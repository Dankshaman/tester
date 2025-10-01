using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D7 RID: 727
public class UIGridMenuComponents : UIGridMenu
{
	// Token: 0x06002393 RID: 9107 RVA: 0x000FC0F0 File Offset: 0x000FA2F0
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
		base.ClearBackStates();
		base.Load<UIGridMenu.GridButtonFolder>(this.FolderComponents, 1, "COMPONENTS", false, true);
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000FC118 File Offset: 0x000FA318
	protected override void OnDisable()
	{
		base.OnDisable();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000FC126 File Offset: 0x000FA326
	protected override void OnLoad()
	{
		base.OnLoad();
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000FC130 File Offset: 0x000FA330
	public List<UIGridMenu.GridButtonComponent> GetAllComponents()
	{
		this.Init();
		List<UIGridMenu.GridButtonComponent> list = new List<UIGridMenu.GridButtonComponent>();
		foreach (UIGridMenu.GridButtonFolder folderButton in this.FolderComponents)
		{
			this.AddChildren(list, folderButton);
		}
		return list;
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000FC194 File Offset: 0x000FA394
	private void AddChildren(List<UIGridMenu.GridButtonComponent> buttons, UIGridMenu.GridButtonFolder folderButton)
	{
		foreach (UIGridMenu.GridButtonComponent gridButtonComponent in folderButton.ComponentButtons)
		{
			bool flag = true;
			using (List<UIGridMenu.GridButtonComponent>.Enumerator enumerator2 = buttons.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.SpawnName == gridButtonComponent.SpawnName)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				buttons.Add(gridButtonComponent);
			}
		}
		foreach (UIGridMenu.GridButtonFolder folderButton2 in folderButton.FolderButtons)
		{
			this.AddChildren(buttons, folderButton2);
		}
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x000FC280 File Offset: 0x000FA480
	private void Init()
	{
		if (this.inited)
		{
			return;
		}
		this.inited = true;
		foreach (UIGridMenu.GridButtonFolder folderButton in this.FolderComponents)
		{
			this.InitButton(new List<string>(), folderButton);
		}
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000FC2E8 File Offset: 0x000FA4E8
	private void InitButton(List<string> tags, UIGridMenu.GridButtonFolder folderButton)
	{
		tags.Add(folderButton.Name.ToLower());
		foreach (UIGridMenu.GridButtonComponent gridButtonComponent in folderButton.ComponentButtons)
		{
			if (string.IsNullOrEmpty(gridButtonComponent.SpawnName))
			{
				gridButtonComponent.SpawnName = gridButtonComponent.Name;
			}
			gridButtonComponent.Tags.AddRange(tags);
		}
		foreach (UIGridMenu.GridButtonFolder folderButton2 in folderButton.FolderButtons)
		{
			this.InitButton(new List<string>(tags), folderButton2);
		}
	}

	// Token: 0x040016A7 RID: 5799
	public List<UIGridMenu.GridButtonFolder> FolderComponents = new List<UIGridMenu.GridButtonFolder>();

	// Token: 0x040016A8 RID: 5800
	private bool inited;
}
