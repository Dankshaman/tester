using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020004A0 RID: 1184
	public class AutoChangeCultureInfo : MonoBehaviour
	{
		// Token: 0x06003520 RID: 13600 RVA: 0x0016251C File Offset: 0x0016071C
		public void Start()
		{
			LocalizationManager.EnableChangingCultureInfo(true);
		}
	}
}
