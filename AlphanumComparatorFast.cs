using System;
using System.Collections;

// Token: 0x0200009D RID: 157
public class AlphanumComparatorFast : IComparer
{
	// Token: 0x06000821 RID: 2081 RVA: 0x0003896C File Offset: 0x00036B6C
	public int Compare(object x, object y)
	{
		string s;
		if ((s = (x as string)) == null)
		{
			return 0;
		}
		string s2;
		if ((s2 = (y as string)) == null)
		{
			return 0;
		}
		return AlphanumComparatorFast.Compare(s, s2);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00038998 File Offset: 0x00036B98
	public static int Compare(string s1, string s2)
	{
		int result;
		try
		{
			int length = s1.Length;
			int length2 = s2.Length;
			int num = 0;
			int num2 = 0;
			while (num < length && num2 < length2)
			{
				char c = s1[num];
				char c2 = s2[num2];
				char[] array = new char[length];
				int num3 = 0;
				char[] array2 = new char[length2];
				int num4 = 0;
				do
				{
					array[num3++] = c;
					num++;
					if (num >= length)
					{
						break;
					}
					c = s1[num];
				}
				while (char.IsDigit(c) == char.IsDigit(array[0]));
				do
				{
					array2[num4++] = c2;
					num2++;
					if (num2 >= length2)
					{
						break;
					}
					c2 = s2[num2];
				}
				while (char.IsDigit(c2) == char.IsDigit(array2[0]));
				string text = new string(array);
				string text2 = new string(array2);
				int num6;
				if (char.IsDigit(array[0]) && char.IsDigit(array2[0]))
				{
					int num5 = int.Parse(text);
					int value = int.Parse(text2);
					num6 = num5.CompareTo(value);
				}
				else
				{
					num6 = text.CompareTo(text2);
				}
				if (num6 != 0)
				{
					return num6;
				}
			}
			result = length - length2;
		}
		catch (Exception)
		{
			result = 0;
		}
		return result;
	}
}
