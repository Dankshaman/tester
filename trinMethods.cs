using System;

// Token: 0x0200014D RID: 333
public static class trinMethods
{
	// Token: 0x060010F1 RID: 4337 RVA: 0x000754FB File Offset: 0x000736FB
	public static bool isTrue(this trin t)
	{
		return t == trin.True;
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00075501 File Offset: 0x00073701
	public static bool isFalse(this trin t)
	{
		return t == trin.False;
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x00075507 File Offset: 0x00073707
	public static bool isUndecided(this trin t)
	{
		return t == trin.Undecided;
	}
}
