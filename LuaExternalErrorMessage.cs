using System;

// Token: 0x02000163 RID: 355
public class LuaExternalErrorMessage : LuaExternalMessage
{
	// Token: 0x06001167 RID: 4455 RVA: 0x00078B84 File Offset: 0x00076D84
	public LuaExternalErrorMessage(string error, string guid, string errorMessagePrefix)
	{
		this.messageID = LuaSendExternalMessageType.ErrorMessage;
		this.error = error;
		this.guid = guid;
		this.errorMessagePrefix = errorMessagePrefix;
	}

	// Token: 0x04000B3D RID: 2877
	public string error;

	// Token: 0x04000B3E RID: 2878
	public string guid;

	// Token: 0x04000B3F RID: 2879
	public string errorMessagePrefix;
}
