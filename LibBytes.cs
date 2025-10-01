using System;

// Token: 0x02000145 RID: 325
public static class LibBytes
{
	// Token: 0x060010BB RID: 4283 RVA: 0x00074024 File Offset: 0x00072224
	public static short ReadLittleEndianInt16(byte[] data, int head)
	{
		byte[] array = new byte[2];
		for (int i = 0; i < 2; i++)
		{
			array[1 - i] = data[head++];
		}
		return BitConverter.ToInt16(array, 0);
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x00074058 File Offset: 0x00072258
	public static short ReadLittleEndianInt16(byte[] data, ref int head)
	{
		byte[] array = new byte[2];
		for (int i = 0; i < 2; i++)
		{
			byte[] array2 = array;
			int num = 1 - i;
			int num2 = head;
			head = num2 + 1;
			array2[num] = data[num2];
		}
		return BitConverter.ToInt16(array, 0);
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x00074090 File Offset: 0x00072290
	public static ushort ReadLittleEndianUInt16(byte[] data, int head)
	{
		byte[] array = new byte[2];
		for (int i = 0; i < 2; i++)
		{
			array[1 - i] = data[head++];
		}
		return BitConverter.ToUInt16(array, 0);
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x000740C4 File Offset: 0x000722C4
	public static ushort ReadLittleEndianUInt16(byte[] data, ref int head)
	{
		byte[] array = new byte[2];
		for (int i = 0; i < 2; i++)
		{
			byte[] array2 = array;
			int num = 1 - i;
			int num2 = head;
			head = num2 + 1;
			array2[num] = data[num2];
		}
		return BitConverter.ToUInt16(array, 0);
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x000740FC File Offset: 0x000722FC
	public static int ReadLittleEndianInt32(byte[] data, int head)
	{
		byte[] array = new byte[4];
		for (int i = 0; i < 4; i++)
		{
			array[3 - i] = data[head++];
		}
		return BitConverter.ToInt32(array, 0);
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x00074130 File Offset: 0x00072330
	public static int ReadLittleEndianInt32(byte[] data, ref int head)
	{
		byte[] array = new byte[4];
		for (int i = 0; i < 4; i++)
		{
			byte[] array2 = array;
			int num = 3 - i;
			int num2 = head;
			head = num2 + 1;
			array2[num] = data[num2];
		}
		return BitConverter.ToInt32(array, 0);
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x00074168 File Offset: 0x00072368
	public static int NumberOfSetBits(ulong i)
	{
		i -= (i >> 1 & 6148914691236517205UL);
		i = (i & 3689348814741910323UL) + (i >> 2 & 3689348814741910323UL);
		return (int)((i + (i >> 4) & 1085102592571150095UL) * 72340172838076673UL >> 56);
	}
}
