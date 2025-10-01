using System;
using NewNet;

// Token: 0x02000382 RID: 898
public class VoiceChat : Singleton<VoiceChat>
{
	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06002A10 RID: 10768 RVA: 0x0012C067 File Offset: 0x0012A267
	// (set) Token: 0x06002A11 RID: 10769 RVA: 0x0012C06F File Offset: 0x0012A26F
	public VoiceTalking Talking
	{
		get
		{
			return this._Talking;
		}
		set
		{
			if (value != this._Talking)
			{
				this._Talking = value;
				if (!Singleton<SteamP2PManager>.Instance.IsP2PConnectedToServer())
				{
					if (value != VoiceTalking.Off)
					{
						Chat.LogError("Not Steam P2P connected to Server for Voice Chat.", true);
					}
					return;
				}
				EventManager.TriggerVoiceTalk(value);
			}
		}
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x0012C0A4 File Offset: 0x0012A2A4
	private void Update()
	{
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			return;
		}
		switch (VoiceChat.Mode)
		{
		case VoiceChat.VoiceMode.Push:
			if (zInput.GetButton("Voice Chat", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.Talking = VoiceTalking.Game;
				return;
			}
			if (zInput.GetButton("Voice Team Chat", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.Talking = VoiceTalking.Team;
				return;
			}
			this.Talking = VoiceTalking.Off;
			return;
		case VoiceChat.VoiceMode.Toggle:
			if (zInput.GetButtonDown("Voice Chat", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.Talking = ((this.Talking == VoiceTalking.Game) ? VoiceTalking.Off : VoiceTalking.Game);
			}
			if (zInput.GetButtonDown("Voice Team Chat", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.Talking = ((this.Talking == VoiceTalking.Team) ? VoiceTalking.Off : VoiceTalking.Team);
				return;
			}
			break;
		case VoiceChat.VoiceMode.None:
			this.Talking = VoiceTalking.Off;
			break;
		default:
			return;
		}
	}

	// Token: 0x04001CB2 RID: 7346
	public static VoiceChat.VoiceMode Mode;

	// Token: 0x04001CB3 RID: 7347
	private VoiceTalking _Talking;

	// Token: 0x020007B7 RID: 1975
	public enum VoiceMode
	{
		// Token: 0x04002D31 RID: 11569
		Push,
		// Token: 0x04002D32 RID: 11570
		Toggle,
		// Token: 0x04002D33 RID: 11571
		None
	}
}
