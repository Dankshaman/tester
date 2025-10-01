using System;
using System.Collections.Generic;

// Token: 0x02000164 RID: 356
public class LuaExternalCustomMessage : LuaExternalMessage
{
	// Token: 0x06001168 RID: 4456 RVA: 0x00078BA8 File Offset: 0x00076DA8
	public LuaExternalCustomMessage(Dictionary<object, object> customMessage)
	{
		this.messageID = LuaSendExternalMessageType.CustomMessage;
		this.customMessage = customMessage;
	}

	// Token: 0x04000B40 RID: 2880
	public Dictionary<object, object> customMessage;
}
