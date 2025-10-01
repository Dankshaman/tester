using System;

namespace RTEditor
{
	// Token: 0x0200041F RID: 1055
	[Flags]
	public enum MouseCursorObjectPickFlags
	{
		// Token: 0x04001FBA RID: 8122
		None = 0,
		// Token: 0x04001FBB RID: 8123
		ObjectBox = 1,
		// Token: 0x04001FBC RID: 8124
		ObjectMesh = 2,
		// Token: 0x04001FBD RID: 8125
		ObjectSprite = 4,
		// Token: 0x04001FBE RID: 8126
		ObjectTerrain = 8
	}
}
