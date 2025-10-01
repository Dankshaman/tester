using System;
using NewNet;
using UnityEngine;

// Token: 0x020002F6 RID: 758
public class UIInviteFriends : MonoBehaviour
{
	// Token: 0x060024B4 RID: 9396 RVA: 0x00103224 File Offset: 0x00101424
	private void Start()
	{
		this.grid = base.GetComponentInParent<UIGrid>();
		NetworkEvents.OnPlayerConnected += this.NetworkEvents_OnPlayerConnected;
		NetworkEvents.OnPlayerDisconnected += this.NetworkEvents_OnPlayerDisconnected;
		NetworkEvents.OnSettingsChange += this.NetworkEvents_OnSettingsChange;
		this.Check();
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x00103276 File Offset: 0x00101476
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.NetworkEvents_OnPlayerConnected;
		NetworkEvents.OnPlayerDisconnected -= this.NetworkEvents_OnPlayerDisconnected;
		NetworkEvents.OnSettingsChange -= this.NetworkEvents_OnSettingsChange;
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x001032AB File Offset: 0x001014AB
	private void NetworkEvents_OnPlayerConnected(NetworkPlayer player)
	{
		this.Check();
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x001032AB File Offset: 0x001014AB
	private void NetworkEvents_OnPlayerDisconnected(NetworkPlayer player, DisconnectInfo info)
	{
		this.Check();
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x001032AB File Offset: 0x001014AB
	private void NetworkEvents_OnSettingsChange()
	{
		this.Check();
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x001032B4 File Offset: 0x001014B4
	private void Check()
	{
		bool flag = Singleton<SteamLobbyManager>.Instance.isInLobby && Network.connections.Count < Network.maxConnections;
		if (flag != base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(flag);
			this.grid.repositionNow = true;
		}
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x00103308 File Offset: 0x00101508
	private void OnClick()
	{
		Singleton<SteamLobbyManager>.Instance.OpenInviteOverlay();
	}

	// Token: 0x040017C1 RID: 6081
	private UIGrid grid;
}
