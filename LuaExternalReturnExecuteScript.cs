using System;

// Token: 0x02000165 RID: 357
public class LuaExternalReturnExecuteScript : LuaExternalMessage
{
	// Token: 0x06001169 RID: 4457 RVA: 0x00078BBE File Offset: 0x00076DBE
	public LuaExternalReturnExecuteScript(object returnValue, int returnID)
	{
		this.messageID = LuaSendExternalMessageType.ReturnExecuteScript;
		this.returnValue = returnValue;
		this.returnID = returnID;
	}

	// Token: 0x04000B41 RID: 2881
	public object returnValue;

	// Token: 0x04000B42 RID: 2882
	public int returnID;
}
