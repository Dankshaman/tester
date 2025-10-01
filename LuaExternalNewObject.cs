using System;

// Token: 0x02000160 RID: 352
public class LuaExternalNewObject : LuaExternalMessage
{
	// Token: 0x06001164 RID: 4452 RVA: 0x00078B3B File Offset: 0x00076D3B
	public LuaExternalNewObject(LuaScriptState[] scriptStates)
	{
		this.messageID = LuaSendExternalMessageType.NewObject;
		this.scriptStates = scriptStates;
	}

	// Token: 0x04000B39 RID: 2873
	public LuaScriptState[] scriptStates;
}
