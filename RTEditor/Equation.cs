using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200042F RID: 1071
	public static class Equation
	{
		// Token: 0x06003176 RID: 12662 RVA: 0x00150500 File Offset: 0x0014E700
		public static bool SolveQuadratic(float a, float b, float c, out float t1, out float t2)
		{
			float num = b * b - 4f * a * c;
			if (num < 0f)
			{
				t1 = (t2 = float.MaxValue);
				return false;
			}
			if (num == 0f)
			{
				t1 = (t2 = -b / (2f * a));
				return true;
			}
			float num2 = 2f * a;
			float num3 = Mathf.Sqrt(num);
			t1 = (-b + num3) / num2;
			t2 = (-b - num3) / num2;
			if (t1 > t2)
			{
				float num4 = t1;
				t1 = t2;
				t2 = num4;
			}
			return true;
		}
	}
}
