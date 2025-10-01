using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000417 RID: 1047
	public static class LayerHelper
	{
		// Token: 0x0600308D RID: 12429 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public static int GetMinLayerNumber()
		{
			return 0;
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x0014CC62 File Offset: 0x0014AE62
		public static int GetMaxLayerNumber()
		{
			return 31;
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x0014CC66 File Offset: 0x0014AE66
		public static bool IsLayerBitSet(int layerBits, int layerNumber)
		{
			return (layerBits & 1 << layerNumber) != 0;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x0014CC73 File Offset: 0x0014AE73
		public static int SetLayerBit(int layerBits, int layerNumber)
		{
			return layerBits | 1 << layerNumber;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x0014CC7D File Offset: 0x0014AE7D
		public static int ClearLayerBit(int layerBits, int layerNumber)
		{
			return layerBits & ~(1 << layerNumber);
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x0014CC88 File Offset: 0x0014AE88
		public static bool IsLayerNumberValid(int layerNumber)
		{
			return layerNumber >= LayerHelper.GetMinLayerNumber() && layerNumber <= LayerHelper.GetMaxLayerNumber();
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x0014CCA0 File Offset: 0x0014AEA0
		public static List<string> GetAllLayerNames()
		{
			List<string> list = new List<string>();
			for (int i = 0; i <= 31; i++)
			{
				string text = LayerMask.LayerToName(i);
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
			return list;
		}
	}
}
