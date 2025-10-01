using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class UIPlayerChatTyping : MonoBehaviour
{
	// Token: 0x06002669 RID: 9833 RVA: 0x00112382 File Offset: 0x00110582
	private void Start()
	{
		this.label = base.GetComponent<UILabel>();
		EventManager.OnPlayerChatTyping += this.OnPlayerChatTyping;
		EventManager.OnPlayersRemove += this.OnPlayersRemove;
	}

	// Token: 0x0600266A RID: 9834 RVA: 0x001123B2 File Offset: 0x001105B2
	private void OnDestroy()
	{
		EventManager.OnPlayerChatTyping -= this.OnPlayerChatTyping;
		EventManager.OnPlayersRemove -= this.OnPlayersRemove;
	}

	// Token: 0x0600266B RID: 9835 RVA: 0x001123D6 File Offset: 0x001105D6
	private void OnPlayerChatTyping(PlayerState player, bool typing)
	{
		if (player.IsMe())
		{
			return;
		}
		if (typing)
		{
			this.typingPlayers.TryAddUnique(player);
		}
		else
		{
			this.typingPlayers.Remove(player);
		}
		this.UpdateText();
	}

	// Token: 0x0600266C RID: 9836 RVA: 0x00112406 File Offset: 0x00110606
	private void OnPlayersRemove(PlayerState player)
	{
		this.typingPlayers.Remove(player);
		this.UpdateText();
	}

	// Token: 0x0600266D RID: 9837 RVA: 0x0011241C File Offset: 0x0011061C
	private void UpdateText()
	{
		Wait.Stop(this.waitID);
		if (this.typingPlayers.Count == 0)
		{
			this.label.text = "";
			return;
		}
		for (int i = 0; i < this.typingPlayers.Count; i++)
		{
			PlayerState playerState = this.typingPlayers[i];
			if (i != 0)
			{
				this.stringBuilder.Append(',');
				this.stringBuilder.Append(' ');
			}
			this.stringBuilder.Append(Colour.HexFromColour(playerState.color));
			this.stringBuilder.Append(playerState.name);
		}
		this.stringBuilder.Append(" [-]");
		this.stringBuilder.Append(Language.Translate("typing"));
		for (int j = 0; j < this.ticks; j++)
		{
			this.stringBuilder.Append('.');
		}
		this.label.text = this.stringBuilder.ToString();
		this.stringBuilder.Clear();
		this.waitID = Wait.Time(new Action(this.UpdateText), 0.5f, 1);
		this.ticks = (this.ticks + 1) % 4;
	}

	// Token: 0x0400190F RID: 6415
	private List<PlayerState> typingPlayers = new List<PlayerState>();

	// Token: 0x04001910 RID: 6416
	private UILabel label;

	// Token: 0x04001911 RID: 6417
	private StringBuilder stringBuilder = new StringBuilder();

	// Token: 0x04001912 RID: 6418
	private int ticks;

	// Token: 0x04001913 RID: 6419
	private Wait.Identifier waitID;
}
