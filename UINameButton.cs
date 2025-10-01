using System;
using System.Collections.Generic;
using System.ComponentModel;
using NewNet;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class UINameButton : MonoBehaviour
{
	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06002542 RID: 9538 RVA: 0x0010705B File Offset: 0x0010525B
	// (set) Token: 0x06002543 RID: 9539 RVA: 0x00107068 File Offset: 0x00105268
	public bool bHost
	{
		get
		{
			return this.IconHost.activeSelf;
		}
		set
		{
			if (this.IconHost.activeSelf != value)
			{
				this.IconHost.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06002544 RID: 9540 RVA: 0x0010708A File Offset: 0x0010528A
	// (set) Token: 0x06002545 RID: 9541 RVA: 0x00107097 File Offset: 0x00105297
	public bool bLookingForPlayer
	{
		get
		{
			return this.IconLookingForPlayer.activeSelf;
		}
		set
		{
			if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				return;
			}
			if (this.IconLookingForPlayer.activeSelf == value)
			{
				return;
			}
			this.IconLookingForPlayer.SetActive(value);
			this.RepositionGrid();
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06002546 RID: 9542 RVA: 0x001070C8 File Offset: 0x001052C8
	// (set) Token: 0x06002547 RID: 9543 RVA: 0x001070D5 File Offset: 0x001052D5
	public bool bServerOptions
	{
		get
		{
			return this.IconServerOptions.activeSelf;
		}
		set
		{
			if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				return;
			}
			if (this.IconServerOptions.activeSelf == value)
			{
				return;
			}
			this.IconServerOptions.SetActive(value);
			this.RepositionGrid();
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06002548 RID: 9544 RVA: 0x00107106 File Offset: 0x00105306
	// (set) Token: 0x06002549 RID: 9545 RVA: 0x00107113 File Offset: 0x00105313
	public bool bTurn
	{
		get
		{
			return this.IconTurn.activeSelf;
		}
		set
		{
			if (this.IconTurn.activeSelf != value)
			{
				this.IconTurn.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x0600254A RID: 9546 RVA: 0x00107135 File Offset: 0x00105335
	// (set) Token: 0x0600254B RID: 9547 RVA: 0x00107142 File Offset: 0x00105342
	public bool bNotepad
	{
		get
		{
			return this.IconNotepad.activeSelf;
		}
		set
		{
			if (this.IconNotepad.activeSelf != value)
			{
				this.IconNotepad.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x0600254C RID: 9548 RVA: 0x00107164 File Offset: 0x00105364
	// (set) Token: 0x0600254D RID: 9549 RVA: 0x00107171 File Offset: 0x00105371
	public bool bType
	{
		get
		{
			return this.IconType.activeSelf;
		}
		set
		{
			if (this.IconType.activeSelf != value)
			{
				this.IconType.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x0600254E RID: 9550 RVA: 0x00107193 File Offset: 0x00105393
	// (set) Token: 0x0600254F RID: 9551 RVA: 0x001071A0 File Offset: 0x001053A0
	public bool bTalkTeam
	{
		get
		{
			return this.IconTalkTeam.activeSelf;
		}
		set
		{
			if (this.IconTalkTeam.activeSelf != value)
			{
				this.IconTalkTeam.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002550 RID: 9552 RVA: 0x001071C2 File Offset: 0x001053C2
	// (set) Token: 0x06002551 RID: 9553 RVA: 0x001071CF File Offset: 0x001053CF
	public bool bTalk
	{
		get
		{
			return this.IconTalk.activeSelf;
		}
		set
		{
			if (this.IconTalk.activeSelf != value)
			{
				this.IconTalk.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002552 RID: 9554 RVA: 0x001071F1 File Offset: 0x001053F1
	// (set) Token: 0x06002553 RID: 9555 RVA: 0x001071FE File Offset: 0x001053FE
	public bool bMute
	{
		get
		{
			return this.IconMute.activeSelf;
		}
		set
		{
			if (this.IconMute.activeSelf != value)
			{
				this.IconMute.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002554 RID: 9556 RVA: 0x00107220 File Offset: 0x00105420
	// (set) Token: 0x06002555 RID: 9557 RVA: 0x0010722D File Offset: 0x0010542D
	public bool bPromote
	{
		get
		{
			return this.IconPromote.activeSelf;
		}
		set
		{
			if (this.IconPromote.activeSelf != value)
			{
				this.IconPromote.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06002556 RID: 9558 RVA: 0x0010724F File Offset: 0x0010544F
	// (set) Token: 0x06002557 RID: 9559 RVA: 0x00107258 File Offset: 0x00105458
	public int Ping
	{
		get
		{
			return this._ping;
		}
		set
		{
			if (this._ping != value)
			{
				bool flag = value != -1;
				this.PingObject.gameObject.SetActive(flag);
				this.PingObject.text = value.ToString();
				this._ping = value;
				if (flag)
				{
					if (this._ping < 100)
					{
						this.PingObject.color = Colour.Green;
					}
					else if (this._ping < 200)
					{
						this.PingObject.color = Colour.Orange;
					}
					else
					{
						this.PingObject.color = Colour.Red;
					}
				}
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06002558 RID: 9560 RVA: 0x00107306 File Offset: 0x00105506
	// (set) Token: 0x06002559 RID: 9561 RVA: 0x00107310 File Offset: 0x00105510
	public Team team
	{
		get
		{
			return this._team;
		}
		set
		{
			if (value != this._team)
			{
				if (value == Team.None)
				{
					this.IconTeam.SetActive(false);
				}
				else
				{
					this.IconTeam.SetActive(true);
					this.IconTeam.GetComponent<UISprite>().spriteName = "Team-" + value.ToString();
				}
				this._team = value;
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x0600255A RID: 9562 RVA: 0x00107378 File Offset: 0x00105578
	// (set) Token: 0x0600255B RID: 9563 RVA: 0x00107385 File Offset: 0x00105585
	public bool bBlind
	{
		get
		{
			return this.IconBlind.activeSelf;
		}
		set
		{
			if (this.IconBlind.activeSelf != value)
			{
				this.IconBlind.SetActive(value);
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x0600255C RID: 9564 RVA: 0x001073A7 File Offset: 0x001055A7
	// (set) Token: 0x0600255D RID: 9565 RVA: 0x001073B4 File Offset: 0x001055B4
	public Texture portrait
	{
		get
		{
			return this.Portrait.mainTexture;
		}
		set
		{
			this.Portrait.mainTexture = value;
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x0600255E RID: 9566 RVA: 0x001073C2 File Offset: 0x001055C2
	// (set) Token: 0x0600255F RID: 9567 RVA: 0x001073CC File Offset: 0x001055CC
	public UINameButton.Connection connection
	{
		get
		{
			return this._connection;
		}
		set
		{
			if (value != this._connection)
			{
				this.IconConnecting.SetActive(false);
				this.IconDisconnected.SetActive(false);
				this.IconBadConnection.SetActive(false);
				if (value != UINameButton.Connection.None && value != UINameButton.Connection.Connected)
				{
					switch (value)
					{
					case UINameButton.Connection.Connecting:
						this.IconConnecting.SetActive(true);
						break;
					case UINameButton.Connection.Disconnected:
						this.IconDisconnected.SetActive(true);
						break;
					case UINameButton.Connection.BadConnection:
						this.IconBadConnection.SetActive(true);
						break;
					}
				}
				this._connection = value;
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06002560 RID: 9568 RVA: 0x00107458 File Offset: 0x00105658
	// (set) Token: 0x06002561 RID: 9569 RVA: 0x00107460 File Offset: 0x00105660
	public int loadingPercent
	{
		get
		{
			return this._loadingPercent;
		}
		set
		{
			if (value != this._loadingPercent)
			{
				this._loadingPercent = value;
				this.LoadingLabel.gameObject.SetActive(this._loadingPercent >= 0 && this._loadingPercent < 100);
				if (this.LoadingLabel.gameObject.activeSelf)
				{
					this.LoadingLabel.text = string.Format("{0}%", this._loadingPercent);
				}
				this.RepositionGrid();
			}
		}
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x001074DC File Offset: 0x001056DC
	private void Start()
	{
		this.UpdateDropDown();
		NetworkSingleton<ServerOptions>.Instance.PropertyChanged += this.OnPropertyChangedServerOptions;
		EventManager.OnPlayerTurnStart += this.EventManager_OnPlayerTurnStart;
		EventDelegate.Add(this.PopupList.onChange, new EventDelegate.Callback(this.PopupListOnChange));
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x00107534 File Offset: 0x00105734
	private void OnDestroy()
	{
		if (NetworkSingleton<ServerOptions>.Instance)
		{
			NetworkSingleton<ServerOptions>.Instance.PropertyChanged -= this.OnPropertyChangedServerOptions;
		}
		EventManager.OnPlayerTurnStart -= this.EventManager_OnPlayerTurnStart;
		EventDelegate.Remove(this.PopupList.onChange, new EventDelegate.Callback(this.PopupListOnChange));
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x00107591 File Offset: 0x00105791
	private void OnPropertyChangedServerOptions(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "LookingForPlayers")
		{
			this.bLookingForPlayer = (this.bHost && NetworkSingleton<ServerOptions>.Instance.LookingForPlayers && Network.isServer && Network.maxConnections > 0);
		}
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x001075D1 File Offset: 0x001057D1
	private void RepositionGrid()
	{
		this.GridObject.Reposition();
		this.GridObject.repositionNow = true;
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x001075EA File Offset: 0x001057EA
	private void OnHover()
	{
		this.UpdateDropDown();
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x001075EA File Offset: 0x001057EA
	private void EventManager_OnPlayerTurnStart(string EndColor, string StartColor)
	{
		this.UpdateDropDown();
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x001075F4 File Offset: 0x001057F4
	public void UpdateDropDown()
	{
		bool flag;
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			flag = (this.id == NetworkSingleton<NetworkUI>.Instance.CurrentHotseat);
		}
		else
		{
			flag = (this.id == NetworkID.ID);
		}
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(this.id);
		if (playerState != null)
		{
			this.NameLabel.text = playerState.name;
		}
		string color = Colour.LabelFromColour(Colour.ColourFromUIColour(base.GetComponent<UIButton>().defaultColor));
		List<string> list = new List<string>();
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			if (flag)
			{
				list.Add("Change Color");
				list.Add("Change Team");
				list.Add("Change Name");
			}
			else if (NetworkSingleton<Turns>.Instance.turnsState.PassTurns)
			{
				list.Add("Pass Turn");
			}
		}
		else
		{
			if (NetworkSingleton<Turns>.Instance.turnsState.Enable && !NetworkSingleton<Turns>.Instance.IsTurn(color))
			{
				if (NetworkSingleton<Turns>.Instance.turnsState.PassTurns && NetworkSingleton<Turns>.Instance.IsTurn())
				{
					list.Add("Pass Turn");
				}
				else if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
				{
					list.Add("Set Turn");
				}
			}
			if (Network.isServer || flag)
			{
				list.Add("Change Color");
				list.Add("Change Team");
				if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
				{
					list.Add("Change Name");
				}
			}
			if (!flag)
			{
				if (!NetworkSingleton<PlayerManager>.Instance.IsMuted(this.id))
				{
					list.Add("Mute");
				}
				else
				{
					list.Add("Unmute");
				}
			}
			if (Network.isServer && !flag)
			{
				if (!NetworkSingleton<PlayerManager>.Instance.IsPromoted(this.id))
				{
					list.Add("Promote");
				}
				else
				{
					list.Add("Demote");
				}
				list.Add("Kick");
				list.Add("Ban");
				list.Add("Give Host");
			}
		}
		base.GetComponent<BoxCollider2D>().enabled = (list.Count > 0);
		this.PopupList.items = list;
	}

	// Token: 0x06002569 RID: 9577 RVA: 0x00107807 File Offset: 0x00105A07
	public void TurnOffLookingForPlayers()
	{
		NetworkSingleton<ServerOptions>.Instance.LookingForPlayers = false;
	}

	// Token: 0x0600256A RID: 9578 RVA: 0x00107814 File Offset: 0x00105A14
	public void ToggleServerOptions()
	{
		this.ServerOptionsGameObject.SetActive(!this.ServerOptionsGameObject.activeSelf);
	}

	// Token: 0x0600256B RID: 9579 RVA: 0x0010782F File Offset: 0x00105A2F
	private void PopupListOnChange()
	{
		this.PopupConfirm(this.PopupList.value);
	}

	// Token: 0x0600256C RID: 9580 RVA: 0x00107844 File Offset: 0x00105A44
	private void PopupConfirm(string Value)
	{
		if (!this.DoNotConfirm.Contains(Value))
		{
			PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(this.id);
			UIDialog.Show(string.Format("{0} {1}?", Value, playerState.name), "Yes", "No", new Action(this.Confirm), null);
			return;
		}
		this.Confirm();
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x001078A4 File Offset: 0x00105AA4
	private void Confirm()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIPlayerSelection(this.PopupList.value, this.id);
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x001078C1 File Offset: 0x00105AC1
	public void SetPlayer(int playerID)
	{
		this.id = playerID;
	}

	// Token: 0x0400182A RID: 6186
	private int _ping = -1;

	// Token: 0x0400182B RID: 6187
	private Team _team = Team.None;

	// Token: 0x0400182C RID: 6188
	private UINameButton.Connection _connection = UINameButton.Connection.None;

	// Token: 0x0400182D RID: 6189
	private int _loadingPercent = 100;

	// Token: 0x0400182E RID: 6190
	public GameObject IconHost;

	// Token: 0x0400182F RID: 6191
	public GameObject IconLookingForPlayer;

	// Token: 0x04001830 RID: 6192
	public GameObject IconServerOptions;

	// Token: 0x04001831 RID: 6193
	public GameObject ServerOptionsGameObject;

	// Token: 0x04001832 RID: 6194
	public GameObject IconTurn;

	// Token: 0x04001833 RID: 6195
	public GameObject IconNotepad;

	// Token: 0x04001834 RID: 6196
	public GameObject IconTalk;

	// Token: 0x04001835 RID: 6197
	public GameObject IconTalkTeam;

	// Token: 0x04001836 RID: 6198
	public GameObject IconType;

	// Token: 0x04001837 RID: 6199
	public GameObject IconMute;

	// Token: 0x04001838 RID: 6200
	public GameObject IconPromote;

	// Token: 0x04001839 RID: 6201
	public UILabel PingObject;

	// Token: 0x0400183A RID: 6202
	public GameObject IconTeam;

	// Token: 0x0400183B RID: 6203
	public GameObject IconBlind;

	// Token: 0x0400183C RID: 6204
	public GameObject IconConnecting;

	// Token: 0x0400183D RID: 6205
	public GameObject IconDisconnected;

	// Token: 0x0400183E RID: 6206
	public GameObject IconBadConnection;

	// Token: 0x0400183F RID: 6207
	public UILabel LoadingLabel;

	// Token: 0x04001840 RID: 6208
	public UITexture Portrait;

	// Token: 0x04001841 RID: 6209
	public UIGrid GridObject;

	// Token: 0x04001842 RID: 6210
	public UIPopupList PopupList;

	// Token: 0x04001843 RID: 6211
	public UILabel NameLabel;

	// Token: 0x04001844 RID: 6212
	[NonSerialized]
	public int id;

	// Token: 0x04001845 RID: 6213
	private List<string> DoNotConfirm = new List<string>
	{
		"Set Turn",
		"Pass Turn",
		"Change Color",
		"Change Team",
		"Change Name"
	};

	// Token: 0x0200076E RID: 1902
	public enum Connection
	{
		// Token: 0x04002BE5 RID: 11237
		None = -1,
		// Token: 0x04002BE6 RID: 11238
		Connected,
		// Token: 0x04002BE7 RID: 11239
		Connecting,
		// Token: 0x04002BE8 RID: 11240
		Disconnected,
		// Token: 0x04002BE9 RID: 11241
		BadConnection
	}
}
