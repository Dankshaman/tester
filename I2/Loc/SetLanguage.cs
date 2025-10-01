using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020004B2 RID: 1202
	[AddComponentMenu("I2/Localization/SetLanguage Button")]
	public class SetLanguage : MonoBehaviour
	{
		// Token: 0x06003567 RID: 13671 RVA: 0x001647AB File Offset: 0x001629AB
		private void OnClick()
		{
			this.ApplyLanguage();
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x001647B3 File Offset: 0x001629B3
		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true, true, true))
			{
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}

		// Token: 0x04002203 RID: 8707
		public string _Language;
	}
}
