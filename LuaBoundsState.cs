using System;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class LuaBoundsState
{
	// Token: 0x060011A6 RID: 4518 RVA: 0x00079363 File Offset: 0x00077563
	[MoonSharpHidden]
	public LuaBoundsState(Bounds bounds, Vector3 offset)
	{
		this.center = bounds.center;
		this.size = bounds.size;
		this.offset = offset;
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x0007938C File Offset: 0x0007758C
	[MoonSharpHidden]
	public LuaBoundsState(Vector3 center, Vector3 size, Vector3 offset)
	{
		this.center = center;
		this.size = size;
		this.offset = offset;
	}

	// Token: 0x04000B62 RID: 2914
	public Vector3 center;

	// Token: 0x04000B63 RID: 2915
	public Vector3 size;

	// Token: 0x04000B64 RID: 2916
	public Vector3 offset;
}
