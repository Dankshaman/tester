using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class BetterList<T>
{
	// Token: 0x060002E5 RID: 741 RVA: 0x000130BC File Offset: 0x000112BC
	public IEnumerator<T> GetEnumerator()
	{
		if (this.buffer != null)
		{
			int num;
			for (int i = 0; i < this.size; i = num)
			{
				yield return this.buffer[i];
				num = i + 1;
			}
		}
		yield break;
	}

	// Token: 0x1700005C RID: 92
	[DebuggerHidden]
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x000130E8 File Offset: 0x000112E8
	private void AllocateMore()
	{
		T[] array = (this.buffer != null) ? new !0[Mathf.Max(this.buffer.Length << 1, 32)] : new !0[32];
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00013144 File Offset: 0x00011344
	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new !0[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
				return;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000131A9 File Offset: 0x000113A9
	public void Clear()
	{
		this.size = 0;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x000131B2 File Offset: 0x000113B2
	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000131C4 File Offset: 0x000113C4
	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		!0[] array = this.buffer;
		int num = this.size;
		this.size = num + 1;
		array[num] = item;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0001320C File Offset: 0x0001140C
	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index > -1 && index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
			return;
		}
		this.Add(item);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00013294 File Offset: 0x00011494
	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000132E0 File Offset: 0x000114E0
	public int IndexOf(T item)
	{
		if (this.buffer == null)
		{
			return -1;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0001332C File Offset: 0x0001152C
	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<!0>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(!0);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					this.buffer[this.size] = default(!0);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000133E4 File Offset: 0x000115E4
	public void RemoveAt(int index)
	{
		if (this.buffer != null && index > -1 && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(!0);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.size] = default(!0);
		}
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00013470 File Offset: 0x00011670
	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			!0[] array = this.buffer;
			int num = this.size - 1;
			this.size = num;
			T result = array[num];
			this.buffer[this.size] = default(!0);
			return result;
		}
		return default(!0);
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000134CD File Offset: 0x000116CD
	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000134DC File Offset: 0x000116DC
	[DebuggerHidden]
	[DebuggerStepThrough]
	public void Sort(BetterList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num2 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num2; i++)
			{
				if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i + 1];
					this.buffer[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = ((i == 0) ? 0 : (i - 1));
				}
			}
		}
	}

	// Token: 0x0400028C RID: 652
	public T[] buffer;

	// Token: 0x0400028D RID: 653
	public int size;

	// Token: 0x02000523 RID: 1315
	// (Invoke) Token: 0x06003777 RID: 14199
	public delegate int CompareFunc(T left, T right);
}
