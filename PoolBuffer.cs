using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000105 RID: 261
public static class PoolBuffer<T> where T : class, IPoolBufferObject, new()
{
	// Token: 0x06000CC3 RID: 3267 RVA: 0x000569BC File Offset: 0x00054BBC
	public static T Get(int minSize = 0)
	{
		T t = default(!0);
		for (int i = 0; i < PoolBuffer<!0>.pool.Count; i++)
		{
			T t2 = PoolBuffer<!0>.pool[i];
			if (t2.GetBufferSize() >= minSize)
			{
				PoolBuffer<!0>.pool.RemoveAt(i);
				PoolBuffer<!0>.poolInUse.Add(t2);
				return t2;
			}
			if (t == null || t.GetBufferSize() < t2.GetBufferSize())
			{
				t = t2;
			}
		}
		if (t != null)
		{
			t.Resize(minSize);
			PoolBuffer<!0>.pool.Remove(t);
			PoolBuffer<!0>.poolInUse.Add(t);
			return t;
		}
		T t3 = Activator.CreateInstance<T>();
		t3.Init(minSize);
		PoolBuffer<!0>.poolInUse.Add(t3);
		long num = 0L;
		for (int j = 0; j < PoolBuffer<!0>.pool.Count; j++)
		{
			num += (long)PoolBuffer<!0>.pool[j].GetBufferSize();
		}
		return t3;
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x00056AC1 File Offset: 0x00054CC1
	public static void Return(T obj)
	{
		if (PoolBuffer<!0>.poolInUse.Remove(obj))
		{
			obj.Clear();
			PoolBuffer<!0>.pool.Add(obj);
			return;
		}
		Debug.LogError("Object was not in pool.");
	}

	// Token: 0x040008A1 RID: 2209
	private static readonly List<T> pool = new List<!0>();

	// Token: 0x040008A2 RID: 2210
	private static readonly List<T> poolInUse = new List<!0>();
}
