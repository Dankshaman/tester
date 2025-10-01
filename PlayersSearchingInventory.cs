using System;
using System.Collections.Generic;
using NewNet;

// Token: 0x02000274 RID: 628
public class PlayersSearchingInventory : NetworkBehavior
{
	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06002105 RID: 8453 RVA: 0x000EF448 File Offset: 0x000ED648
	// (remove) Token: 0x06002106 RID: 8454 RVA: 0x000EF480 File Offset: 0x000ED680
	public event Action OnSearchDelete;

	// Token: 0x06002107 RID: 8455 RVA: 0x000EF4B5 File Offset: 0x000ED6B5
	public void Init()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x000EF4C3 File Offset: 0x000ED6C3
	private void Awake()
	{
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected += this.OnPlayerDisconnect;
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000EF4E7 File Offset: 0x000ED6E7
	private void OnDestroy()
	{
		if (this.OnSearchDelete != null)
		{
			this.OnSearchDelete();
		}
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected -= this.OnPlayerDisconnect;
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000EF51E File Offset: 0x000ED71E
	public bool AnyoneSearching()
	{
		return this.playersSearching.Count > 0;
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x000EF530 File Offset: 0x000ED730
	private void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			for (int i = 0; i < this.playersSearching.Count; i++)
			{
				base.networkView.RPC<PlayerSearch>(player, new Action<PlayerSearch>(this.RPCAddSearchId), this.playersSearching[i]);
			}
		}
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000EF580 File Offset: 0x000ED780
	private int playerSearchIndexFromID(int id)
	{
		for (int i = 0; i < this.playersSearching.Count; i++)
		{
			if (this.playersSearching[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000EF5BC File Offset: 0x000ED7BC
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCAddSearchId(PlayerSearch search)
	{
		try
		{
			if (this.playerSearchIndexFromID(search.id) < 0)
			{
				this.playersSearching.Add(search);
				if (Singleton<UIObjectIndicatorManager>.Instance)
				{
					Singleton<UIObjectIndicatorManager>.Instance.AddIndicator(this.NPO, UIObjectIndicatorManager.IndicatorType.Search, Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(search.id)), (search.count >= 0) ? search.count.ToString() : "");
				}
				PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(search.id);
				EventManager.TriggerObjectSearchStart(this.NPO, playerState.stringColor);
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000EF674 File Offset: 0x000ED874
	private void OnPlayerDisconnect(NetworkPlayer player, DisconnectInfo info)
	{
		if (Network.isServer)
		{
			int id = NetworkID.IDFromNetworkPlayer(player);
			if (this.playerSearchIndexFromID(id) >= 0)
			{
				this.RemoveSearchId(id);
			}
		}
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000EF6A0 File Offset: 0x000ED8A0
	public void AddSearchId(int id, int count = -1)
	{
		base.networkView.RPC<PlayerSearch>(RPCTarget.All, new Action<PlayerSearch>(this.RPCAddSearchId), new PlayerSearch(id, count));
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000EF6C1 File Offset: 0x000ED8C1
	public void RemoveSearchId(int id)
	{
		base.networkView.RPC<int>(RPCTarget.All, new Action<int>(this.RPCRemoveSearchId), id);
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000EF6DC File Offset: 0x000ED8DC
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCRemoveSearchId(int id)
	{
		try
		{
			int num = this.playerSearchIndexFromID(id);
			if (num >= 0)
			{
				this.playersSearching.RemoveAt(num);
				if (Singleton<UIObjectIndicatorManager>.Instance)
				{
					Singleton<UIObjectIndicatorManager>.Instance.RemoveIndicator(this.NPO, UIObjectIndicatorManager.IndicatorType.Search, Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(id)));
				}
				PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
				EventManager.TriggerObjectSearchEnd(this.NPO, playerState.stringColor);
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000EF768 File Offset: 0x000ED968
	public void SetSearch(int PlayerId, int objectId, List<ObjectState> objectStates, int maxCards)
	{
		for (int i = 0; i < this.playersSearching.Count; i++)
		{
			if (this.playersSearching[i].id != PlayerId)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SendSearchInventoryBytesToPlayer(this.playersSearching[i].id, objectId, objectStates, maxCards);
			}
		}
	}

	// Token: 0x0400146B RID: 5227
	private NetworkPhysicsObject NPO;

	// Token: 0x0400146C RID: 5228
	public List<PlayerSearch> playersSearching = new List<PlayerSearch>();
}
