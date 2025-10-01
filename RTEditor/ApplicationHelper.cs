using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000414 RID: 1044
	public static class ApplicationHelper
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x0014B4F3 File Offset: 0x001496F3
		public static void Quit()
		{
			if (Application.isEditor)
			{
				Debug.Break();
				return;
			}
			Application.Quit();
		}
	}
}
