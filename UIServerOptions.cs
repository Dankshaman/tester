using System;
using NewNet;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class UIServerOptions : UIReactiveMenu
{
	// Token: 0x06002717 RID: 10007 RVA: 0x0011629C File Offset: 0x0011449C
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveInputs.Add(this.ServerNameInput.gameObject);
		this.ReactiveElements.Add(this.PublicToggle.onChange);
		this.ReactiveElements.Add(this.FriendsToggle.onChange);
		this.ReactiveElements.Add(this.InviteToggle.onChange);
		this.ReactiveInputs.Add(this.ServerPasswordInput.gameObject);
		this.ReactiveElements.Add(this.ServerMaxPlayersSlider.onChange);
		if (this.ServerLookingForPlayers)
		{
			this.ReactiveElements.Add(this.ServerLookingForPlayers.onChange);
		}
		if (this.ServerLookingForPlayers && Network.maxConnections == 0)
		{
			this.ServerNameInput.GetComponent<Collider2D>().enabled = false;
			this.PublicToggle.GetComponent<Collider2D>().enabled = false;
			this.FriendsToggle.GetComponent<Collider2D>().enabled = false;
			this.InviteToggle.GetComponent<Collider2D>().enabled = false;
			this.ServerPasswordInput.GetComponent<Collider2D>().enabled = false;
			this.ServerLookingForPlayers.GetComponent<Collider2D>().enabled = false;
			Collider2D[] componentsInChildren = this.ServerMaxPlayersSlider.GetComponentsInChildren<Collider2D>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
		if (this.CreateServerButton)
		{
			EventDelegate.Add(this.CreateServerButton.onClick, new EventDelegate.Callback(this.ClickCreateServer));
		}
		Wait.Frames(new Action(this.UpdatePasswordInput), 2);
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x00116436 File Offset: 0x00114636
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.CreateServerButton)
		{
			EventDelegate.Remove(this.CreateServerButton.onClick, new EventDelegate.Callback(this.ClickCreateServer));
		}
	}

	// Token: 0x06002719 RID: 10009 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x0600271A RID: 10010 RVA: 0x00116468 File Offset: 0x00114668
	protected override void ReloadUI()
	{
		if (this.BlockSync)
		{
			return;
		}
		this.ServerNameInput.value = NetworkSingleton<ServerOptions>.Instance.ServerName;
		int group = this.PublicToggle.group;
		this.PublicToggle.group = 0;
		this.FriendsToggle.group = 0;
		this.InviteToggle.group = 0;
		this.PublicToggle.value = (NetworkSingleton<ServerOptions>.Instance.ServerType == ServerType.Public);
		this.FriendsToggle.value = (NetworkSingleton<ServerOptions>.Instance.ServerType == ServerType.Friends);
		this.InviteToggle.value = (NetworkSingleton<ServerOptions>.Instance.ServerType == ServerType.Invite);
		this.PublicToggle.group = group;
		this.FriendsToggle.group = group;
		this.InviteToggle.group = group;
		this.ServerPasswordInput.value = NetworkSingleton<ServerOptions>.Instance.Password;
		this.ServerMaxPlayersSlider.GetComponentInChildren<UISliderRange>().intValue = NetworkSingleton<ServerOptions>.Instance.MaxConnections;
		if (this.ServerLookingForPlayers)
		{
			this.ServerLookingForPlayers.value = NetworkSingleton<ServerOptions>.Instance.LookingForPlayers;
		}
	}

	// Token: 0x0600271B RID: 10011 RVA: 0x00116580 File Offset: 0x00114780
	private void UpdatePasswordInput()
	{
		if ((this.PublicToggle.value && !this.ServerLookingForPlayers) || (this.PublicToggle.value && this.ServerLookingForPlayers && Network.maxConnections > 0))
		{
			this.ServerPasswordInput.readOnly = false;
			this.ServerPasswordInput.gameObject.SetActive(true);
			return;
		}
		this.ServerPasswordInput.readOnly = true;
		this.ServerPasswordInput.SetReadOnlyValue("");
		this.ServerPasswordInput.gameObject.SetActive(false);
	}

	// Token: 0x0600271C RID: 10012 RVA: 0x0011661C File Offset: 0x0011481C
	protected override void UpdateSource()
	{
		this.UpdatePasswordInput();
		if (this.BlockSync)
		{
			return;
		}
		NetworkSingleton<ServerOptions>.Instance.ServerName = this.ServerNameInput.value;
		if (this.InviteToggle.value)
		{
			NetworkSingleton<ServerOptions>.Instance.ServerType = ServerType.Invite;
		}
		else if (this.FriendsToggle.value)
		{
			NetworkSingleton<ServerOptions>.Instance.ServerType = ServerType.Friends;
		}
		else
		{
			NetworkSingleton<ServerOptions>.Instance.ServerType = ServerType.Public;
		}
		NetworkSingleton<ServerOptions>.Instance.Password = this.ServerPasswordInput.value;
		NetworkSingleton<ServerOptions>.Instance.MaxConnections = this.ServerMaxPlayersSlider.GetComponentInChildren<UISliderRange>().intValue;
		if (this.ServerLookingForPlayers)
		{
			NetworkSingleton<ServerOptions>.Instance.LookingForPlayers = this.ServerLookingForPlayers.value;
		}
	}

	// Token: 0x0600271D RID: 10013 RVA: 0x001166DD File Offset: 0x001148DD
	private void ClickCreateServer()
	{
		this.BlockSync = false;
		this.UpdateSource();
		Network.InitializeServer();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400199A RID: 6554
	public UIInput ServerNameInput;

	// Token: 0x0400199B RID: 6555
	public UIToggle PublicToggle;

	// Token: 0x0400199C RID: 6556
	public UIToggle FriendsToggle;

	// Token: 0x0400199D RID: 6557
	public UIToggle InviteToggle;

	// Token: 0x0400199E RID: 6558
	public UIInput ServerPasswordInput;

	// Token: 0x0400199F RID: 6559
	public UISlider ServerMaxPlayersSlider;

	// Token: 0x040019A0 RID: 6560
	public UIToggle ServerLookingForPlayers;

	// Token: 0x040019A1 RID: 6561
	public UIButton CreateServerButton;

	// Token: 0x040019A2 RID: 6562
	public bool BlockSync;
}
