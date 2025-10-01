using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000418 RID: 1048
	public static class MathHelper
	{
		// Token: 0x06003094 RID: 12436 RVA: 0x0014CCD7 File Offset: 0x0014AED7
		public static float SafeAcos(float cosine)
		{
			cosine = Mathf.Max(-1f, Mathf.Min(1f, cosine));
			return Mathf.Acos(cosine);
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x0014CCF6 File Offset: 0x0014AEF6
		public static int GetNumberOfDigits(int number)
		{
			if (number != 0)
			{
				return Mathf.FloorToInt(Mathf.Log10((float)Mathf.Abs(number)) + 1f);
			}
			return 1;
		}
	}
}
