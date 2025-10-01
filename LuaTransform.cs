using System;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class LuaTransform
{
	// Token: 0x060013DA RID: 5082 RVA: 0x00002594 File Offset: 0x00000794
	public LuaTransform()
	{
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x000840A0 File Offset: 0x000822A0
	[MoonSharpHidden]
	public LuaTransform(Transform transform)
	{
		this.position = transform.position;
		this.rotation = transform.eulerAngles;
		this.scale = transform.localScale;
		this.forward = transform.forward;
		this.right = transform.right;
		this.up = transform.up;
	}

	// Token: 0x04000BB7 RID: 2999
	public Vector3 position;

	// Token: 0x04000BB8 RID: 3000
	public Vector3 rotation;

	// Token: 0x04000BB9 RID: 3001
	public Vector3 scale;

	// Token: 0x04000BBA RID: 3002
	public Vector3 forward;

	// Token: 0x04000BBB RID: 3003
	public Vector3 right;

	// Token: 0x04000BBC RID: 3004
	public Vector3 up;
}
