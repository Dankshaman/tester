using System;

// Token: 0x0200012E RID: 302
public interface iIrc
{
	// Token: 0x0600100A RID: 4106
	void OnPm(string user, string message);

	// Token: 0x0600100B RID: 4107
	void OnMessage(string message);

	// Token: 0x0600100C RID: 4108
	void OnConnected();
}
