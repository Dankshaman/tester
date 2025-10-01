using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class LuaInput : MonoBehaviour
{
	// Token: 0x060014C0 RID: 5312 RVA: 0x000887AB File Offset: 0x000869AB
	private void Start()
	{
		this.input = base.GetComponent<UIInput>();
		EventDelegate.Add(this.input.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x000887D6 File Offset: 0x000869D6
	private void OnDestroy()
	{
		EventDelegate.Remove(this.input.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x000887F5 File Offset: 0x000869F5
	private void OnChange()
	{
		if (this.input.isSelected)
		{
			this.luaObject.SubmitUIInput(this.inputState, true);
		}
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x00088816 File Offset: 0x00086A16
	private void OnSelect(bool Selected)
	{
		if (!PlayerScript.Pointer)
		{
			if (Selected)
			{
				base.SendMessage("OnSelect", false);
			}
			return;
		}
		if (!Selected)
		{
			this.luaObject.SubmitUIInput(this.inputState, false);
		}
	}

	// Token: 0x04000BE7 RID: 3047
	public LuaGameObjectScript luaObject;

	// Token: 0x04000BE8 RID: 3048
	public LuaUIInputState inputState;

	// Token: 0x04000BE9 RID: 3049
	private UIInput input;
}
