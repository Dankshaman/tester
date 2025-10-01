using System;
using NewNet;

// Token: 0x0200009B RID: 155
public class AchievementManager : NetworkSingleton<AchievementManager>
{
	// Token: 0x0600081A RID: 2074 RVA: 0x0003890F File Offset: 0x00036B0F
	public void SetAchievement(string achievementName)
	{
		Achievements.Set(achievementName);
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00038917 File Offset: 0x00036B17
	public void SetAchievement(RPCTarget target, string achievementName)
	{
		base.networkView.RPC<string>(target, new Action<string>(this.RPCSetAchievement), achievementName);
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00038932 File Offset: 0x00036B32
	public void SetAchievement(NetworkPlayer player, string achievementName)
	{
		base.networkView.RPC<string>(player, new Action<string>(this.RPCSetAchievement), achievementName);
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x0003894D File Offset: 0x00036B4D
	[Remote(Permission.Server)]
	private void RPCSetAchievement(string achievementName)
	{
		this.SetAchievement(achievementName);
	}
}
