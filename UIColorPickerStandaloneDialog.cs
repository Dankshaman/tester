using System;
using NewNet;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class UIColorPickerStandaloneDialog : Singleton<UIColorPickerStandaloneDialog>
{
	// Token: 0x0600218B RID: 8587 RVA: 0x000F1854 File Offset: 0x000EFA54
	public void Show(Action<Color> callback, Color color)
	{
		this.DefaultColor = color;
		NetworkSingleton<NetworkUI>.Instance.GUIColorPickerScript.Show(color, delegate(Color pickedColor)
		{
			this.DefaultColor = pickedColor;
			callback(pickedColor);
		});
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000F1898 File Offset: 0x000EFA98
	public void Show(Action<Color> callback)
	{
		this.Show(callback, this.DefaultColor);
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x000F18A8 File Offset: 0x000EFAA8
	public void Show(NetworkPlayer targetPlayer, Action<Color> callback, Color color)
	{
		if (!Network.isServer)
		{
			return;
		}
		if (targetPlayer.isServer)
		{
			this.Show(callback, color);
			return;
		}
		LuaGlobalScriptManager.Instance.ActionColorCallbackFromPlayerID[(int)targetPlayer.id] = callback;
		LuaGlobalScriptManager.Instance.networkView.RPC<int, Color>(targetPlayer, new Action<int, Color>(LuaGlobalScriptManager.Instance.RPCShowColorPicker), callback.GetHashCode(), color);
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000F190D File Offset: 0x000EFB0D
	public void Show(NetworkPlayer targetPlayer, Action<Color> callback)
	{
		this.Show(targetPlayer, callback, this.DefaultColor);
	}

	// Token: 0x040014D3 RID: 5331
	[NonSerialized]
	public Color DefaultColor = Color.white;
}
