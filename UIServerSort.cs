using System;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class UIServerSort : MonoBehaviour
{
	// Token: 0x0600271F RID: 10015 RVA: 0x00116700 File Offset: 0x00114900
	private void OnClick()
	{
		if (UIServerSort.LastClickedSort == this)
		{
			UIServerSort.ReverseSort = !UIServerSort.ReverseSort;
		}
		else
		{
			UIServerSort.ReverseSort = false;
		}
		UIServerSort.LastClickedSort = this;
		NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.GetComponent<UIServerBrowser>().SetSort(this.sortType, UIServerSort.ReverseSort);
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x00116754 File Offset: 0x00114954
	public static int CompareLocked(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		if (x.Passworded == y.Passworded)
		{
			return 0;
		}
		if (!y.Passworded)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x00116771 File Offset: 0x00114971
	public static int CompareServerName(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		return string.Compare(x.Server.ToLower(), y.Server.ToLower());
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x0011678E File Offset: 0x0011498E
	public static int CompareGameName(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		return string.Compare(x.Game.ToLower(), y.Game.ToLower());
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x001167AB File Offset: 0x001149AB
	public static int CompareHostName(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		return string.Compare(x.Host.ToLower(), y.Host.ToLower());
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x001167C8 File Offset: 0x001149C8
	public static int CompareRegion(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		return string.Compare(x.Country, y.Country);
	}

	// Token: 0x06002725 RID: 10021 RVA: 0x001167DB File Offset: 0x001149DB
	public static int CompareLookingForPlayers(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		if (x.LookingForPlayers == y.LookingForPlayers)
		{
			return 0;
		}
		if (!y.LookingForPlayers)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x06002726 RID: 10022 RVA: 0x001167F8 File Offset: 0x001149F8
	public static int ComparePlayerCountForwards(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		int num = x.CurrentPlayers;
		int num2 = y.CurrentPlayers;
		if (num == num2)
		{
			num = x.MaxPlayers;
			num2 = y.MaxPlayers;
			if (num == num2)
			{
				return 0;
			}
		}
		if (num >= num2)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x00116834 File Offset: 0x00114A34
	public static int ComparePlayerCountBackwards(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		int num = x.MaxPlayers;
		int num2 = y.MaxPlayers;
		if (num == num2)
		{
			num = x.CurrentPlayers;
			num2 = y.CurrentPlayers;
			if (num == num2)
			{
				return 0;
			}
		}
		if (num <= num2)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x0011686E File Offset: 0x00114A6E
	public static int CompareDefault(UIServerBrowser.TTSLobbyInfo x, UIServerBrowser.TTSLobbyInfo y)
	{
		if (x.Order == y.Order)
		{
			return 0;
		}
		if (x.Order >= y.Order)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x040019A3 RID: 6563
	public UIServerBrowser.ServerSortOptions sortType;

	// Token: 0x040019A4 RID: 6564
	private static UIServerSort LastClickedSort;

	// Token: 0x040019A5 RID: 6565
	public static bool ReverseSort;
}
