using System;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class LuaHit
{
	// Token: 0x060013D9 RID: 5081 RVA: 0x00084069 File Offset: 0x00082269
	[MoonSharpHidden]
	public LuaHit(RaycastHit hit, LuaGameObjectScript luaObject)
	{
		this.point = hit.point;
		this.normal = hit.normal;
		this.distance = hit.distance;
		this.hit_object = luaObject;
	}

	// Token: 0x04000BB3 RID: 2995
	public Vector3 point;

	// Token: 0x04000BB4 RID: 2996
	public Vector3 normal;

	// Token: 0x04000BB5 RID: 2997
	public float distance;

	// Token: 0x04000BB6 RID: 2998
	public LuaGameObjectScript hit_object;
}
