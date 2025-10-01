using System;
using System.Collections.Generic;

// Token: 0x0200015C RID: 348
public class LuaEditorResponse
{
	// Token: 0x04000B1F RID: 2847
	public LuaReceiveExternalMessageType messageID = LuaReceiveExternalMessageType.None;

	// Token: 0x04000B20 RID: 2848
	public LuaScriptState[] scriptStates;

	// Token: 0x04000B21 RID: 2849
	public string xmlUI;

	// Token: 0x04000B22 RID: 2850
	public Dictionary<object, object> customMessage;

	// Token: 0x04000B23 RID: 2851
	public string guid;

	// Token: 0x04000B24 RID: 2852
	public string script;

	// Token: 0x04000B25 RID: 2853
	public int returnID = -1;
}
