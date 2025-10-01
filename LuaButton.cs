using System;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class LuaButton : MonoBehaviour
{
	// Token: 0x0600112D RID: 4397 RVA: 0x000763B9 File Offset: 0x000745B9
	public void OnClick()
	{
		this.luaObject.ClickUIButton(this.buttonState, false);
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x000763CD File Offset: 0x000745CD
	public void OnAltClick()
	{
		this.luaObject.ClickUIButton(this.buttonState, true);
	}

	// Token: 0x04000B07 RID: 2823
	public LuaGameObjectScript luaObject;

	// Token: 0x04000B08 RID: 2824
	public LuaUIButtonState buttonState;
}
