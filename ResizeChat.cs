using System;
using UnityEngine;

// Token: 0x02000275 RID: 629
public class ResizeChat : MonoBehaviour
{
	// Token: 0x06002114 RID: 8468 RVA: 0x000402F2 File Offset: 0x0003E4F2
	private void OnHover(bool isOver)
	{
		NetworkSingleton<Chat>.Instance.RefreshChatLog();
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x000402F2 File Offset: 0x0003E4F2
	private void OnPress(bool pressed)
	{
		NetworkSingleton<Chat>.Instance.RefreshChatLog();
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x000402F2 File Offset: 0x0003E4F2
	public void OnDrag(Vector2 delta)
	{
		NetworkSingleton<Chat>.Instance.RefreshChatLog();
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000EF7D1 File Offset: 0x000ED9D1
	private void OnDragEnd()
	{
		this.chatSettings.OnChatResized();
	}

	// Token: 0x0400146E RID: 5230
	public ChatSettings chatSettings;
}
