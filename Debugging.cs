using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class Debugging : MonoBehaviour
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00051C7F File Offset: 0x0004FE7F
	// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00051C86 File Offset: 0x0004FE86
	public static bool bDebug
	{
		get
		{
			return Debugging._bDebug;
		}
		set
		{
			if (value != Debugging.bDebug)
			{
				Debugging._bDebug = value;
			}
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00051C96 File Offset: 0x0004FE96
	// (set) Token: 0x06000BFE RID: 3070 RVA: 0x00051CA2 File Offset: 0x0004FEA2
	public static bool bLog
	{
		get
		{
			return Debug.unityLogger.logEnabled;
		}
		set
		{
			if (value != Debug.unityLogger.logEnabled)
			{
				Debug.unityLogger.logEnabled = value;
			}
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00051CBC File Offset: 0x0004FEBC
	private void Awake()
	{
		Debugging.bLog = (Debug.isDebugBuild || Utilities.IsLaunchOption("-log"));
	}

	// Token: 0x0400083F RID: 2111
	private static bool _bDebug;
}
