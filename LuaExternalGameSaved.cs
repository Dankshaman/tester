using System;

// Token: 0x02000166 RID: 358
public class LuaExternalGameSaved : LuaExternalMessage
{
	// Token: 0x0600116A RID: 4458 RVA: 0x00078BDB File Offset: 0x00076DDB
	public LuaExternalGameSaved(string savePath)
	{
		this.messageID = LuaSendExternalMessageType.GameSaved;
		this.savePath = savePath;
	}

	// Token: 0x04000B43 RID: 2883
	public string savePath;
}
