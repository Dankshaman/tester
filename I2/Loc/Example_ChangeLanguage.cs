using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000465 RID: 1125
	public class Example_ChangeLanguage : MonoBehaviour
	{
		// Token: 0x0600330E RID: 13070 RVA: 0x00155228 File Offset: 0x00153428
		public void SetLanguage_English()
		{
			this.SetLanguage("English");
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00155235 File Offset: 0x00153435
		public void SetLanguage_French()
		{
			this.SetLanguage("French");
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00155242 File Offset: 0x00153442
		public void SetLanguage_Spanish()
		{
			this.SetLanguage("Spanish");
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x0015524F File Offset: 0x0015344F
		public void SetLanguage(string LangName)
		{
			if (LocalizationManager.HasLanguage(LangName, true, true, true))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}
	}
}
