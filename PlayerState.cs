using System;
using NewNet;
using Steamworks;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class PlayerState
{
	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06001877 RID: 6263 RVA: 0x000A6405 File Offset: 0x000A4605
	// (set) Token: 0x06001878 RID: 6264 RVA: 0x000A640D File Offset: 0x000A460D
	public string steamId { get; private set; }

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06001879 RID: 6265 RVA: 0x000A6416 File Offset: 0x000A4616
	// (set) Token: 0x0600187A RID: 6266 RVA: 0x000A641E File Offset: 0x000A461E
	public CSteamID cSteamId { get; private set; }

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x0600187B RID: 6267 RVA: 0x000A6427 File Offset: 0x000A4627
	// (set) Token: 0x0600187C RID: 6268 RVA: 0x000A642F File Offset: 0x000A462F
	public NetworkPlayer networkPlayer { get; private set; }

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x0600187D RID: 6269 RVA: 0x000A6438 File Offset: 0x000A4638
	// (set) Token: 0x0600187E RID: 6270 RVA: 0x000A6440 File Offset: 0x000A4640
	public int id { get; private set; }

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x0600187F RID: 6271 RVA: 0x000A6449 File Offset: 0x000A4649
	// (set) Token: 0x06001880 RID: 6272 RVA: 0x000A6451 File Offset: 0x000A4651
	public bool isVR { get; set; }

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06001881 RID: 6273 RVA: 0x000A645A File Offset: 0x000A465A
	// (set) Token: 0x06001882 RID: 6274 RVA: 0x000A6464 File Offset: 0x000A4664
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			string text = value.Trim();
			if (text.Length >= 33)
			{
				text = text.Substring(0, 32);
			}
			if (this._name == text)
			{
				return;
			}
			this._name = text;
			if (this.ui != null)
			{
				this.ui.UpdateDropDown();
			}
			if (NetworkSingleton<NetworkUI>.Instance.bHotseat && this.id == NetworkSingleton<NetworkUI>.Instance.CurrentHotseat)
			{
				NetworkSingleton<Turns>.Instance.SetTurnButtonName(this._name);
			}
			EventManager.TriggerPlayersUpdate();
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06001883 RID: 6275 RVA: 0x000A64EE File Offset: 0x000A46EE
	// (set) Token: 0x06001884 RID: 6276 RVA: 0x000A64F6 File Offset: 0x000A46F6
	public string stringColor
	{
		get
		{
			return this._stringColor;
		}
		set
		{
			if (value == this._stringColor)
			{
				return;
			}
			this._stringColor = value;
			this.color = Colour.ColourFromLabel(this._stringColor);
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06001885 RID: 6277 RVA: 0x000A6524 File Offset: 0x000A4724
	// (set) Token: 0x06001886 RID: 6278 RVA: 0x000A652C File Offset: 0x000A472C
	public Color color
	{
		get
		{
			return this._color;
		}
		private set
		{
			if (value == this._color)
			{
				return;
			}
			this._color = value;
			this.UpdateUIColour();
		}
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x000A654C File Offset: 0x000A474C
	public void UpdateUIColour()
	{
		Colour colour = Colour.UIColourFromColour(this._color);
		this.ui.GetComponent<UIButton>().defaultColor = colour;
		this.ui.GetComponent<UIButton>().hover = colour;
		this.ui.GetComponent<UIButton>().pressed = colour;
		this.ui.GetComponent<UIButton>().disabledColor = colour;
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06001888 RID: 6280 RVA: 0x000A65C2 File Offset: 0x000A47C2
	// (set) Token: 0x06001889 RID: 6281 RVA: 0x000A65CA File Offset: 0x000A47CA
	public bool promoted
	{
		get
		{
			return this._promoted;
		}
		set
		{
			if (value == this._promoted)
			{
				return;
			}
			this._promoted = value;
			this.ui.bPromote = value;
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x0600188A RID: 6282 RVA: 0x000A65E9 File Offset: 0x000A47E9
	// (set) Token: 0x0600188B RID: 6283 RVA: 0x000A65F1 File Offset: 0x000A47F1
	public Team team
	{
		get
		{
			return this._team;
		}
		set
		{
			if (value == this._team)
			{
				return;
			}
			this._team = value;
			this.ui.team = value;
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x0600188C RID: 6284 RVA: 0x000A6610 File Offset: 0x000A4810
	// (set) Token: 0x0600188D RID: 6285 RVA: 0x000A6618 File Offset: 0x000A4818
	public bool blind
	{
		get
		{
			return this._blind;
		}
		set
		{
			if (value == this._blind)
			{
				return;
			}
			this._blind = value;
			this.ui.bBlind = value;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x0600188E RID: 6286 RVA: 0x000A6637 File Offset: 0x000A4837
	// (set) Token: 0x0600188F RID: 6287 RVA: 0x000A663F File Offset: 0x000A483F
	public bool muted
	{
		get
		{
			return this._muted;
		}
		set
		{
			if (value == this._muted)
			{
				return;
			}
			this._muted = value;
			this.ui.bMute = value;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06001890 RID: 6288 RVA: 0x000A665E File Offset: 0x000A485E
	// (set) Token: 0x06001891 RID: 6289 RVA: 0x000A6666 File Offset: 0x000A4866
	public int loadingPercent
	{
		get
		{
			return this._loadingPercent;
		}
		set
		{
			if (value == this._loadingPercent)
			{
				return;
			}
			this._loadingPercent = value;
			this.ui.loadingPercent = value;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06001892 RID: 6290 RVA: 0x000A6685 File Offset: 0x000A4885
	// (set) Token: 0x06001893 RID: 6291 RVA: 0x000A668D File Offset: 0x000A488D
	public bool typing
	{
		get
		{
			return this._typing;
		}
		set
		{
			if (value == this._typing)
			{
				return;
			}
			this._typing = value;
			if (!this.IsMe())
			{
				this.ui.bType = value;
			}
			EventManager.TriggerPlayerChatTyping(this, value);
		}
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x000A66BB File Offset: 0x000A48BB
	public bool CanHaveHand()
	{
		return this._stringColor != "Black" && this._stringColor != "Grey";
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x000A66E1 File Offset: 0x000A48E1
	public bool IsMe()
	{
		return this.id == NetworkID.ID;
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x000A66F0 File Offset: 0x000A48F0
	public bool IsValid()
	{
		return NetworkSingleton<PlayerManager>.Instance.ContainsID(this.id);
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06001897 RID: 6295 RVA: 0x000A6702 File Offset: 0x000A4902
	// (set) Token: 0x06001898 RID: 6296 RVA: 0x000A670A File Offset: 0x000A490A
	public Texture portrait
	{
		get
		{
			return this._portrait;
		}
		set
		{
			if (value == this.portrait)
			{
				return;
			}
			this._portrait = value;
			this.ui.portrait = value;
		}
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x000A6730 File Offset: 0x000A4930
	public PlayerState(string name, NetworkPlayer networkPlayer, string steamId, bool isVR)
	{
		this.name = name;
		this.networkPlayer = networkPlayer;
		this.id = NetworkID.IDFromNetworkPlayer(networkPlayer);
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			this.id = NetworkSingleton<PlayerManager>.Instance.PlayersList.Count + 1;
		}
		this.steamId = steamId;
		this.cSteamId = SteamManager.StringToSteamID(steamId);
		this.isVR = isVR;
	}

	// Token: 0x04000EAA RID: 3754
	public GameObject head;

	// Token: 0x04000EAB RID: 3755
	private string _name;

	// Token: 0x04000EAC RID: 3756
	private string _stringColor;

	// Token: 0x04000EAD RID: 3757
	private Color _color;

	// Token: 0x04000EAE RID: 3758
	private bool _promoted;

	// Token: 0x04000EAF RID: 3759
	private Team _team = Team.None;

	// Token: 0x04000EB0 RID: 3760
	private bool _blind;

	// Token: 0x04000EB1 RID: 3761
	private bool _muted;

	// Token: 0x04000EB2 RID: 3762
	private int _loadingPercent = 100;

	// Token: 0x04000EB3 RID: 3763
	private bool _typing;

	// Token: 0x04000EB4 RID: 3764
	public string ping;

	// Token: 0x04000EB5 RID: 3765
	public Pointer pointer;

	// Token: 0x04000EB6 RID: 3766
	public UINameButton ui;

	// Token: 0x04000EB7 RID: 3767
	private Texture _portrait;
}
