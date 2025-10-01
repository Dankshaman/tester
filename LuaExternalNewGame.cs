using System;

// Token: 0x02000161 RID: 353
public class LuaExternalNewGame : LuaExternalMessage
{
	// Token: 0x06001165 RID: 4453 RVA: 0x00078B51 File Offset: 0x00076D51
	public LuaExternalNewGame(LuaScriptState[] scriptStates, string savePath)
	{
		this.messageID = LuaSendExternalMessageType.NewGame;
		this.scriptStates = scriptStates;
		this.savePath = savePath;
	}

	// Token: 0x04000B3A RID: 2874
	public LuaScriptState[] scriptStates;

	// Token: 0x04000B3B RID: 2875
	public string savePath;
}
