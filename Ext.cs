using System;

// Token: 0x02000130 RID: 304
public static class Ext
{
	// Token: 0x0600101C RID: 4124 RVA: 0x0006D850 File Offset: 0x0006BA50
	public static T[] ReverseA<T>(this T[] a)
	{
		return a.ReverseA(a.Length);
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0006D85C File Offset: 0x0006BA5C
	public static T[] ReverseA<T>(this T[] a, int len)
	{
		T[] array = new !!0[len];
		for (int i = 0; i < len; i++)
		{
			array[a.Length - i - 1] = a[i];
		}
		return array;
	}
}
