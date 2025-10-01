using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class ListBuffer<T> : IPoolBufferObject, ICollection<!0>, IEnumerable<!0>, IEnumerable where T : IEquatable<!0>
{
	// Token: 0x06000CCA RID: 3274 RVA: 0x00056B08 File Offset: 0x00054D08
	public int IndexOf(T item)
	{
		for (int i = 0; i < this.Count; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x00056B48 File Offset: 0x00054D48
	public bool Remove(T item)
	{
		int num = this.IndexOf(item);
		if (num == -1)
		{
			return false;
		}
		this.RemoveAt(num);
		return true;
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x00056B6C File Offset: 0x00054D6C
	public void RemoveAt(int index)
	{
		for (int i = index + 1; i < this.Count; i++)
		{
			this.buffer[i - 1] = this.buffer[i];
		}
		int count = this.Count;
		this.Count = count - 1;
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00056BB6 File Offset: 0x00054DB6
	// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00056BBE File Offset: 0x00054DBE
	public int Count { get; private set; }

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00056BC7 File Offset: 0x00054DC7
	public bool IsReadOnly { get; }

	// Token: 0x06000CD0 RID: 3280 RVA: 0x00056BCF File Offset: 0x00054DCF
	public void Init(int bufferSize)
	{
		this.buffer = new !0[bufferSize];
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x00056BDD File Offset: 0x00054DDD
	public int GetBufferSize()
	{
		return this.buffer.Length;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x00056BE7 File Offset: 0x00054DE7
	public void Clear()
	{
		if (this.Count > 0)
		{
			Array.Clear(this.buffer, 0, this.Count);
			this.Count = 0;
		}
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00056C0B File Offset: 0x00054E0B
	public bool Contains(T item)
	{
		return this.IndexOf(item) != -1;
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0005473A File Offset: 0x0005293A
	public void CopyTo(T[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00056C1A File Offset: 0x00054E1A
	// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x00056C22 File Offset: 0x00054E22
	public bool inUse { get; set; }

	// Token: 0x06000CD7 RID: 3287 RVA: 0x00056C2B File Offset: 0x00054E2B
	public T[] GetBuffer()
	{
		return this.buffer;
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00056C34 File Offset: 0x00054E34
	public void Add(T data)
	{
		if (this.buffer.Length <= this.Count)
		{
			this.Resize(this.buffer.Length * 2);
		}
		this.buffer[this.Count] = data;
		int count = this.Count;
		this.Count = count + 1;
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00056C84 File Offset: 0x00054E84
	public void Add(T[] data, int dataLength)
	{
		if (this.buffer.Length < this.Count + dataLength)
		{
			this.Resize(Mathf.Max(this.buffer.Length * 2, this.Count + dataLength + 1));
		}
		Array.Copy(data, 0, this.buffer, this.Count, dataLength);
		this.Count += dataLength;
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00056CE4 File Offset: 0x00054EE4
	public void Add(T[] data)
	{
		this.Add(data, data.Length);
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00056CF0 File Offset: 0x00054EF0
	public void Resize(int newSize)
	{
		T[] destinationArray = new !0[newSize];
		Array.Copy(this.buffer, 0, destinationArray, 0, Mathf.Min(this.Count, newSize));
		this.buffer = destinationArray;
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x00056D28 File Offset: 0x00054F28
	public T[] GetData()
	{
		T[] array = new !0[this.Count];
		Array.Copy(this.buffer, 0, array, 0, this.Count);
		return array;
	}

	// Token: 0x17000217 RID: 535
	public T this[int index]
	{
		get
		{
			return this.buffer[index];
		}
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00056D64 File Offset: 0x00054F64
	public IEnumerator<T> GetEnumerator()
	{
		return new ListBuffer<!0>.Enumerator(this);
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x00056D71 File Offset: 0x00054F71
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x040008A3 RID: 2211
	private T[] buffer;

	// Token: 0x020005D7 RID: 1495
	public struct Enumerator : IEnumerator<!0>, IEnumerator, IDisposable
	{
		// Token: 0x06003977 RID: 14711 RVA: 0x0017515C File Offset: 0x0017335C
		internal Enumerator(ListBuffer<T> list)
		{
			this.list = list;
			this.index = 0;
			this.current = default(!0);
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x000025B8 File Offset: 0x000007B8
		public void Dispose()
		{
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x00175178 File Offset: 0x00173378
		public bool MoveNext()
		{
			if (this.index >= this.list.Count)
			{
				return this.MoveNextRare();
			}
			this.current = this.list.buffer[this.index];
			this.index++;
			return true;
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x001751CA File Offset: 0x001733CA
		private bool MoveNextRare()
		{
			this.index = this.list.Count + 1;
			this.current = default(!0);
			return false;
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x001751EC File Offset: 0x001733EC
		public T Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600397C RID: 14716 RVA: 0x001751F4 File Offset: 0x001733F4
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x00175201 File Offset: 0x00173401
		void IEnumerator.Reset()
		{
			this.index = 0;
			this.current = default(!0);
		}

		// Token: 0x04002710 RID: 10000
		private ListBuffer<T> list;

		// Token: 0x04002711 RID: 10001
		private int index;

		// Token: 0x04002712 RID: 10002
		private T current;
	}
}
