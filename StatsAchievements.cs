using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x0200023F RID: 575
public class StatsAchievements : Singleton<StatsAchievements>
{
	// Token: 0x06001C6A RID: 7274 RVA: 0x000C3D9F File Offset: 0x000C1F9F
	private void Start()
	{
		base.InvokeRepeating("SlowUpdate", 5f, 5f);
		base.InvokeRepeating("TimeIncrement", 60f, 60f);
		NetworkEvents.OnServerInitialized += delegate()
		{
			this.RestraintTimeHolder = Time.time;
		};
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x000C3DDC File Offset: 0x000C1FDC
	private void SlowUpdate()
	{
		if (NetworkSingleton<GameOptions>.Instance.GameName == "Chess")
		{
			if (this.ChessTimeHolder == 0f)
			{
				this.ChessTimeHolder = Time.time;
			}
			if (Time.time - this.ChessTimeHolder > 3600f)
			{
				Achievements.Set("ACH_PLAY_CHESS_1_HOUR");
				this.ChessTimeHolder = 0f;
			}
		}
		else
		{
			this.ChessTimeHolder = 0f;
		}
		if (Network.isServer && Time.time - this.RestraintTimeHolder > 3600f)
		{
			this.RestraintTimeHolder = Time.time;
			NetworkSingleton<AchievementManager>.Instance.SetAchievement(RPCTarget.All, "ACH_PLAY_GAME_1_HOUR_NO_FLIP");
		}
		List<Pointer> pointers = NetworkSingleton<ManagerPhysicsObject>.Instance.Pointers;
		if (pointers.Count >= 8)
		{
			int num = 0;
			for (int i = 0; i < pointers.Count; i++)
			{
				if (pointers[i].StartLineVector != Vector3.zero)
				{
					num++;
				}
			}
			if (num >= 8)
			{
				Achievements.Set("ACH_LASER_LIGHT_SHOW_8_PLAYER");
			}
		}
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x000C3ED2 File Offset: 0x000C20D2
	private void TimeIncrement()
	{
		Stats.INT_HOURS_PLAYED++;
	}

	// Token: 0x04001224 RID: 4644
	private const float HOUR = 3600f;

	// Token: 0x04001225 RID: 4645
	public float ChessTimeHolder;

	// Token: 0x04001226 RID: 4646
	public float RestraintTimeHolder;
}
