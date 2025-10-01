using System;
using Discord;
using Steamworks;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class SteamRichPresenceController : Singleton<SteamRichPresenceController>
{
	// Token: 0x06001D2D RID: 7469 RVA: 0x000C8A50 File Offset: 0x000C6C50
	private void Update()
	{
		if (Time.time > this.nextCheck && (Singleton<DiscordController>.Instance.DiscordActivityLastRefresh > this.discordActivityLastRefresh || this.status != Singleton<DiscordController>.Instance.DiscordActivity.Details))
		{
			this.nextCheck = Time.time + 10f;
			this.discordActivityLastRefresh = Singleton<DiscordController>.Instance.DiscordActivityLastRefresh;
			this.discordActivity = Singleton<DiscordController>.Instance.DiscordActivity;
			this.status = this.discordActivity.Details;
			string text = "";
			string details = this.discordActivity.Details;
			string pchValue;
			if (!(details == "Connecting..."))
			{
				if (!(details == "Grooving in the menus"))
				{
					if (!(details == "Chilling in the menus"))
					{
						if (!(details == "-"))
						{
							if (this.discordActivity.Details.StartsWith("No game loaded"))
							{
								if (this.discordActivity.State == "Multiplayer")
								{
									pchValue = "#Status_NoGameWithSeats";
								}
								else
								{
									pchValue = "#Status_NoGame";
								}
							}
							else
							{
								if (this.discordActivity.State == "Multiplayer")
								{
									pchValue = "#Status_GameWithSeats";
								}
								else
								{
									pchValue = "#Status_Game";
								}
								text = LibString.StripBBCode(Singleton<DiscordController>.Instance.GameName);
							}
						}
						else
						{
							pchValue = "#Status_Hacking";
						}
					}
					else
					{
						pchValue = "#Status_Chilling";
					}
				}
				else
				{
					pchValue = "#Status_Grooving";
				}
			}
			else
			{
				pchValue = "#Status_Connecting";
			}
			if (VRHMD.isVR && text != "")
			{
				text += " in VR";
			}
			SteamFriends.SetRichPresence("status", this.status);
			SteamFriends.SetRichPresence("steam_display", pchValue);
			SteamFriends.SetRichPresence("game", text);
			SteamFriends.SetRichPresence("seated", string.Format("{0}", this.discordActivity.Party.Size.CurrentSize));
			SteamFriends.SetRichPresence("max_seats", string.Format("{0}", this.discordActivity.Party.Size.MaxSize));
			SteamFriends.SetRichPresence("color", this.discordActivity.Assets.SmallImage);
		}
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x000C8C83 File Offset: 0x000C6E83
	public void Refresh()
	{
		this.discordActivityLastRefresh = 0f;
		this.nextCheck = 0f;
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x000C8C9B File Offset: 0x000C6E9B
	private void OnEnable()
	{
		SteamFriends.ClearRichPresence();
	}

	// Token: 0x040012A5 RID: 4773
	private const float UPDATE_THROTTLE = 10f;

	// Token: 0x040012A6 RID: 4774
	private float discordActivityLastRefresh;

	// Token: 0x040012A7 RID: 4775
	private float nextCheck;

	// Token: 0x040012A8 RID: 4776
	private Activity discordActivity;

	// Token: 0x040012A9 RID: 4777
	private string status;
}
