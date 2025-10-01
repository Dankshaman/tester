using System;
using TouchScript;
using UnityEngine;

// Token: 0x02000261 RID: 609
public class TouchScriptUpdater : MonoBehaviour
{
	// Token: 0x06002022 RID: 8226 RVA: 0x000E623C File Offset: 0x000E443C
	private void Update()
	{
		if (TouchManager.Instance.NumberOfTouches > 0)
		{
			this.FrameBuffer = 0;
			zInput.bTouching = true;
			return;
		}
		if (this.FrameBuffer < 5)
		{
			this.FrameBuffer++;
			return;
		}
		zInput.bTouching = false;
	}

	// Token: 0x040013A6 RID: 5030
	public static int BeginTouchId;

	// Token: 0x040013A7 RID: 5031
	public static int EndTouchId;

	// Token: 0x040013A8 RID: 5032
	private int FrameBuffer;
}
