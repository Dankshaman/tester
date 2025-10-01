using System;
using System.Collections.Generic;

// Token: 0x020000A5 RID: 165
public class BlockList : Singleton<BlockList>
{
	// Token: 0x06000841 RID: 2113 RVA: 0x0003A510 File Offset: 0x00038710
	private void Start()
	{
		this.BlockedPlayers = SerializationScript.LoadBlock();
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0003A520 File Offset: 0x00038720
	public void AddBlock(string name, string steamid)
	{
		Chat.Log(name + " has been blocked. This can be changed in the configuration menu.", ChatMessageType.Game);
		BlockedPlayer blockedPlayer = new BlockedPlayer();
		blockedPlayer.Name = name;
		blockedPlayer.SteamID = steamid;
		this.BlockedPlayers.Add(blockedPlayer);
		SerializationScript.Save(this.BlockedPlayers);
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0003A56C File Offset: 0x0003876C
	public void RemoveBlock(string name, string steamid)
	{
		foreach (BlockedPlayer blockedPlayer in this.BlockedPlayers)
		{
			if (blockedPlayer.SteamID == steamid)
			{
				this.BlockedPlayers.Remove(blockedPlayer);
				break;
			}
		}
		Chat.Log(name + " has been removed from your block list.", ChatMessageType.Game);
		SerializationScript.Save(this.BlockedPlayers);
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0003A5F4 File Offset: 0x000387F4
	public bool Contains(string steamid)
	{
		using (List<BlockedPlayer>.Enumerator enumerator = this.BlockedPlayers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.SteamID == steamid)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040005C1 RID: 1473
	public List<BlockedPlayer> BlockedPlayers = new List<BlockedPlayer>();
}
