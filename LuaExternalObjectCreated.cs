using System;

// Token: 0x02000167 RID: 359
public class LuaExternalObjectCreated : LuaExternalMessage
{
	// Token: 0x0600116B RID: 4459 RVA: 0x00078BF1 File Offset: 0x00076DF1
	public LuaExternalObjectCreated(string guid)
	{
		this.messageID = LuaSendExternalMessageType.ObjectCreated;
		this.guid = guid;
	}

	// Token: 0x04000B44 RID: 2884
	public string guid;
}
