using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class Turns : NetworkSingleton<Turns>
{
	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06002024 RID: 8228 RVA: 0x000E6277 File Offset: 0x000E4477
	// (set) Token: 0x06002025 RID: 8229 RVA: 0x000E6280 File Offset: 0x000E4480
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public TurnsState turnsState
	{
		get
		{
			return this._turnsState;
		}
		set
		{
			TurnsState turnsState = this._turnsState;
			string a = (turnsState != null) ? turnsState.TurnColor : null;
			this._turnsState = value;
			string text = null;
			if (this._turnsState.Enable)
			{
				if (string.IsNullOrEmpty(this._turnsState.TurnColor))
				{
					List<string> turnOrder = this.GetTurnOrder();
					if (turnOrder.Count > 0)
					{
						text = turnOrder[0];
					}
				}
				else
				{
					text = this._turnsState.TurnColor;
				}
			}
			else
			{
				text = "";
			}
			if (text != null && a != text)
			{
				this._turnsState.TurnColor = text;
				this.SetPlayerTurn(this._turnsState.TurnColor);
			}
			NetworkSingleton<PlayerManager>.Instance.UpdateOrder();
			base.DirtySync("turnsState");
		}
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x000E6334 File Offset: 0x000E4534
	private void Update()
	{
		if (Network.isServer && this.turnsState.Enable && !string.IsNullOrEmpty(this.turnsState.TurnColor))
		{
			Color color = Colour.ColourFromLabel(this.turnsState.TurnColor);
			if (!NetworkSingleton<PlayerManager>.Instance.ColourInUse(color))
			{
				string nextTurnColor = this.GetNextTurnColor(this.turnsState.TurnColor, false);
				Color color2 = Colour.ColourFromLabel(nextTurnColor);
				string text = NetworkSingleton<PlayerManager>.Instance.NameFromColour(color2);
				if (string.IsNullOrEmpty(nextTurnColor) || string.IsNullOrEmpty(text))
				{
					this.turnsState.Enable = false;
					this.RPCSetPlayerTurn("", RPCTarget.All);
					return;
				}
				this.RPCSetPlayerTurn(text, RPCTarget.All);
			}
		}
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x000E63F8 File Offset: 0x000E45F8
	public void RPCSetPlayerTurn(string name, RPCTarget RPCmode = RPCTarget.All)
	{
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			string hotSeatTurn = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromName(name));
			this.SetHotSeatTurn(hotSeatTurn);
			return;
		}
		base.networkView.RPC<string>(RPCmode, new Action<string>(this.SetPlayerTurn), string.IsNullOrEmpty(name) ? name : Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromName(name)));
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x000E6460 File Offset: 0x000E4660
	public bool SetPlayerTurnByColourLabel(string colourLabel, RPCTarget RPCmode = RPCTarget.All)
	{
		if (!Colour.IsColourLabel(colourLabel) || !this.GetTurnOrder().Contains(colourLabel))
		{
			return false;
		}
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			this.SetHotSeatTurn(colourLabel);
			return true;
		}
		base.networkView.RPC<string>(RPCmode, new Action<string>(this.SetPlayerTurn), colourLabel);
		return true;
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x000E64B4 File Offset: 0x000E46B4
	[Remote(Permission.Admin)]
	public void SetPlayerTurn(string label)
	{
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat && string.IsNullOrEmpty(label))
		{
			return;
		}
		this.turnsState.TurnColor = label;
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
		if (playerState != null)
		{
			bool flag = playerState.stringColor == label;
			if (flag)
			{
				NetworkSingleton<NetworkUI>.Instance.GetComponent<SoundScript>().PlayGUISound(NetworkSingleton<NetworkUI>.Instance.YourTurnSound, 0.8f, 1f);
				NetworkSingleton<NetworkUI>.Instance.GUIEndTurn.transform.GetChild(0).GetComponent<UISprite>().color = playerState.color;
				NetworkSingleton<NetworkUI>.Instance.GUIEndTurn.GetComponent<UIWidget>().SetDimensions(470, 56);
				this.SetTurnButtonName(playerState.name);
			}
			NetworkSingleton<NetworkUI>.Instance.GUIEndTurn.SetActive(flag && (!NetworkSingleton<NetworkUI>.Instance.bHotseat || Turns.ShowHotseatTurnButton));
		}
		Colour colour = (!string.IsNullOrEmpty(label)) ? Colour.ColourFromLabel(label) : Colour.White;
		if (!string.IsNullOrEmpty(label) && NetworkSingleton<PlayerManager>.Instance.ColourInUse(colour))
		{
			Chat.Log(NetworkSingleton<PlayerManager>.Instance.NameFromColour(colour) + "'s turn.", colour, ChatMessageType.Game, true);
		}
		if (label != this.lastTurnColor && !NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			EventManager.TriggerPlayerTurnEnd(this.lastTurnColor, label);
			EventManager.TriggerPlayerTurnStart(this.lastTurnColor, label);
			this.lastTurnColor = label;
		}
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x000E661C File Offset: 0x000E481C
	public void SetTurnButtonName(string name)
	{
		NetworkSingleton<NetworkUI>.Instance.GUIEndTurn.transform.GetChild(2).GetComponent<UILabel>().text = name;
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x000E6640 File Offset: 0x000E4840
	public string GetPreviousTurnColor()
	{
		this.turnsState.Reverse = !this.turnsState.Reverse;
		string nextTurnColor = this.GetNextTurnColor(this.turnsState.TurnColor, false);
		this.turnsState.Reverse = !this.turnsState.Reverse;
		return nextTurnColor;
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x000E6691 File Offset: 0x000E4891
	public string GetNextTurnColor()
	{
		return this.GetNextTurnColor(this.turnsState.TurnColor, false);
	}

	// Token: 0x0600202D RID: 8237 RVA: 0x000E66A5 File Offset: 0x000E48A5
	public string GetNextTurnPlayer(Color color, bool IgnorePlayer = false)
	{
		return this.GetNextTurnPlayer(Colour.LabelFromColour(color), IgnorePlayer);
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x000E66BC File Offset: 0x000E48BC
	public string GetNextTurnPlayer(string ColorString, bool IgnorePlayer = false)
	{
		string nextTurnColor = this.GetNextTurnColor(ColorString, IgnorePlayer);
		if (string.IsNullOrEmpty(nextTurnColor))
		{
			return "";
		}
		return NetworkSingleton<PlayerManager>.Instance.NameFromColour(Colour.ColourFromLabel(nextTurnColor));
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x000E66F0 File Offset: 0x000E48F0
	public string GetNextTurnColor(Color color, bool IgnorePlayer = false)
	{
		return this.GetNextTurnColor(Colour.LabelFromColour(color), IgnorePlayer);
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000E6704 File Offset: 0x000E4904
	public string GetNextTurnColor(string ColorString, bool VisualTurn = false)
	{
		TurnType type = this.turnsState.Type;
		if (type != TurnType.Auto)
		{
			if (type == TurnType.Custom)
			{
				if (this.turnsState.TurnOrder.Count > 0)
				{
					int num = this.turnsState.TurnOrder.IndexOf(this.turnsState.TurnColor);
					if (num == -1)
					{
						num = 0;
					}
					int num2 = 0;
					string text;
					for (;;)
					{
						num2++;
						if (num2 > 10)
						{
							break;
						}
						if (!this.turnsState.Reverse)
						{
							num++;
						}
						else
						{
							num--;
						}
						if (num < 0)
						{
							num = this.turnsState.TurnOrder.Count - 1;
						}
						if (this.turnsState.TurnOrder.Count > num)
						{
							text = this.turnsState.TurnOrder[num];
						}
						else
						{
							num = 0;
							text = this.turnsState.TurnOrder[num];
						}
						Color color = Colour.ColourFromLabel(text);
						if (NetworkSingleton<PlayerManager>.Instance.ColourInUse(color) || VisualTurn)
						{
							return text;
						}
					}
					text = null;
					return text;
				}
			}
			return "";
		}
		GameObject gameObject = HandZone.GetHand(ColorString, 0);
		Color rhs = Colour.ColourFromLabel(ColorString);
		if (gameObject)
		{
			string[] handPlayerLabels = Colour.HandPlayerLabels;
			Color color2 = Color.white;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			for (int i = 0; i < handPlayerLabels.Length; i++)
			{
				GameObject hand = HandZone.GetHand(handPlayerLabels[i], 0);
				if (hand)
				{
					Color color3 = Colour.ColourFromLabel(handPlayerLabels[i]);
					if (!(color3 == rhs) && (NetworkSingleton<PlayerManager>.Instance.ColourInUse(color3) || VisualTurn))
					{
						if (this.turnsState.SkipEmpty && !VisualTurn)
						{
							HandZone handZone = HandZone.GetHandZone(handPlayerLabels[i], 0, false);
							if (handZone && !handZone.HasHandObjects)
							{
								goto IL_11F;
							}
						}
						float num5 = HandZone.HandAngle(gameObject.transform.position, hand.transform.position);
						if (!this.turnsState.Reverse)
						{
							if (num5 < num3)
							{
								num3 = num5;
								color2 = color3;
							}
						}
						else if (num5 > num4)
						{
							num4 = num5;
							color2 = color3;
						}
					}
				}
				IL_11F:;
			}
			if (num3 != 3.4028235E+38f || num4 != -3.4028235E+38f)
			{
				return Colour.LabelFromColour(color2);
			}
		}
		gameObject = HandZone.GetStartHand();
		if (gameObject)
		{
			return gameObject.GetComponent<HandZone>().TriggerLabel;
		}
		return NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(0).stringColor;
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000E6984 File Offset: 0x000E4B84
	public List<string> GetTurnOrder()
	{
		TurnType type = this.turnsState.Type;
		if (type == TurnType.Auto)
		{
			return this.GetAutoTurnOrder();
		}
		if (type != TurnType.Custom)
		{
			return null;
		}
		return this.GetCustomTurnOrder();
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000E69B8 File Offset: 0x000E4BB8
	public List<string> GetAutoTurnOrder()
	{
		List<string> list = new List<string>();
		HandZone startHandZone = HandZone.GetStartHandZone();
		if (startHandZone)
		{
			string text = startHandZone.TriggerLabel;
			list.Add(text);
			for (;;)
			{
				text = this.GetNextTurnColor(text, true);
				if (list.Contains(text))
				{
					break;
				}
				list.Add(text);
			}
		}
		return list;
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000E6A04 File Offset: 0x000E4C04
	public List<string> GetCustomTurnOrder()
	{
		List<string> turnOrder = this.turnsState.TurnOrder;
		if (turnOrder == null || turnOrder.Count == 0)
		{
			return this.GetAutoTurnOrder();
		}
		List<string> list = new List<string>();
		List<HandZone> handZones = HandZone.GetHandZones();
		for (int i = 0; i < turnOrder.Count; i++)
		{
			if (HandZone.GetHand(turnOrder[i], 0))
			{
				list.Add(turnOrder[i]);
			}
		}
		for (int j = 0; j < handZones.Count; j++)
		{
			HandZone handZone = handZones[j];
			if (!list.Contains(handZone.TriggerLabel))
			{
				list.Add(handZone.TriggerLabel);
			}
		}
		return list;
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000E6AAC File Offset: 0x000E4CAC
	public void GUIEndTurn()
	{
		string turnColor = NetworkSingleton<Turns>.Instance.turnsState.TurnColor;
		if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
		{
			this.SetHotSeatTurn(this.GetNextTurnColor());
			return;
		}
		if (Network.isServer)
		{
			string nextTurnPlayer = this.GetNextTurnPlayer(turnColor, false);
			this.RPCSetPlayerTurn(nextTurnPlayer, RPCTarget.All);
			return;
		}
		this.turnsState.TurnColor = "";
		base.networkView.RPC(RPCTarget.Server, new Action(this.RPCEndTurn));
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x000E6B24 File Offset: 0x000E4D24
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RPCEndTurn()
	{
		int id = NetworkID.IDFromNetworkPlayer(Network.sender);
		string text = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(id));
		if (this.turnsState.TurnColor == text)
		{
			string nextTurnPlayer = NetworkSingleton<Turns>.Instance.GetNextTurnPlayer(text, false);
			this.RPCSetPlayerTurn(nextTurnPlayer, RPCTarget.All);
		}
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000E6B7C File Offset: 0x000E4D7C
	public void Reset()
	{
		TurnsState turnsState = new TurnsState();
		if (turnsState != this.turnsState)
		{
			this.turnsState = turnsState;
		}
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000E6BA4 File Offset: 0x000E4DA4
	public bool CanInteract()
	{
		return PlayerScript.Pointer && this.CanInteract(PlayerScript.PointerScript.PointerColorLabel);
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000E6BC4 File Offset: 0x000E4DC4
	public bool CanInteract(string Color)
	{
		return !this.turnsState.Enable || !this.turnsState.DisableInteractions || this.turnsState.TurnColor == Color || Color == "Black";
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000E6C00 File Offset: 0x000E4E00
	public bool IsTurn()
	{
		return PlayerScript.Pointer && this.IsTurn(PlayerScript.PointerScript.PointerColorLabel);
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x000E6C20 File Offset: 0x000E4E20
	public bool IsTurn(string Color)
	{
		return this.turnsState.Enable && this.turnsState.TurnColor == Color;
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x000E6C44 File Offset: 0x000E4E44
	private void SetHotSeatTurn(string colourLabel)
	{
		Color color = Colour.ColourFromLabel(colourLabel);
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromColour(color);
		EventManager.TriggerPlayerTurnEnd(NetworkSingleton<NetworkUI>.Instance.HotseatPreviousColor, colourLabel);
		NetworkSingleton<NetworkUI>.Instance.GUIStartConfirmHotseat(playerState.id, false);
	}

	// Token: 0x040013A9 RID: 5033
	private TurnsState _turnsState = new TurnsState();

	// Token: 0x040013AA RID: 5034
	private string lastTurnColor = "";

	// Token: 0x040013AB RID: 5035
	public static bool ConfirmHotseatTurnStart = false;

	// Token: 0x040013AC RID: 5036
	public static float HotseatTurnInterval = 2f;

	// Token: 0x040013AD RID: 5037
	public static bool ShowHotseatTurnButton = true;

	// Token: 0x040013AE RID: 5038
	public static bool HotseatCameraReset = true;

	// Token: 0x040013AF RID: 5039
	public static bool HotseatAskForNames = true;
}
