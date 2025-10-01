using System;

// Token: 0x0200014E RID: 334
public static class Libtypes
{
	// Token: 0x060010F4 RID: 4340 RVA: 0x0007550D File Offset: 0x0007370D
	public static trin trinFromBool(bool b)
	{
		if (b)
		{
			return trin.True;
		}
		return trin.False;
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x00075515 File Offset: 0x00073715
	public static trin trinFromBool(bool? b)
	{
		if (b == null)
		{
			return trin.Undecided;
		}
		if (b.Value)
		{
			return trin.True;
		}
		return trin.False;
	}
}
