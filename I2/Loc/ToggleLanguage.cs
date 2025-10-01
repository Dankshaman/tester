using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200046B RID: 1131
	public class ToggleLanguage : MonoBehaviour
	{
		// Token: 0x06003328 RID: 13096 RVA: 0x001557C5 File Offset: 0x001539C5
		private void Start()
		{
			base.Invoke("test", 3f);
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x001557D8 File Offset: 0x001539D8
		private void test()
		{
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			int num = allLanguages.IndexOf(LocalizationManager.CurrentLanguage);
			if (num >= 0)
			{
				num = (num + 1) % allLanguages.Count;
			}
			base.Invoke("test", 3f);
		}
	}
}
