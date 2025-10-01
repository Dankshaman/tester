using System;

// Token: 0x0200015E RID: 350
public enum LuaSendExternalMessageType
{
	// Token: 0x04000B2F RID: 2863
	None = -1,
	// Token: 0x04000B30 RID: 2864
	NewObject,
	// Token: 0x04000B31 RID: 2865
	NewGame,
	// Token: 0x04000B32 RID: 2866
	PrintMessage,
	// Token: 0x04000B33 RID: 2867
	ErrorMessage,
	// Token: 0x04000B34 RID: 2868
	CustomMessage,
	// Token: 0x04000B35 RID: 2869
	ReturnExecuteScript,
	// Token: 0x04000B36 RID: 2870
	GameSaved,
	// Token: 0x04000B37 RID: 2871
	ObjectCreated
}
