using System;

// Token: 0x02000162 RID: 354
public class LuaExternalPrintMessage : LuaExternalMessage
{
	// Token: 0x06001166 RID: 4454 RVA: 0x00078B6E File Offset: 0x00076D6E
	public LuaExternalPrintMessage(string message)
	{
		this.messageID = LuaSendExternalMessageType.PrintMessage;
		this.message = message;
	}

	// Token: 0x04000B3C RID: 2876
	public string message;
}
