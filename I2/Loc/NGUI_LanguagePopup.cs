using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000468 RID: 1128
	public class NGUI_LanguagePopup : MonoBehaviour
	{
		// Token: 0x06003317 RID: 13079 RVA: 0x00155378 File Offset: 0x00153578
		private void Start()
		{
			UIPopupList component = base.GetComponent<UIPopupList>();
			component.items = this.Source.mSource.GetLanguages(true);
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnValueChange));
			int num = component.items.IndexOf(LocalizationManager.CurrentLanguage);
			component.value = component.items[(num >= 0) ? num : 0];
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x001553E3 File Offset: 0x001535E3
		public void OnValueChange()
		{
			LocalizationManager.CurrentLanguage = UIPopupList.current.value;
		}

		// Token: 0x040020CB RID: 8395
		public LanguageSource Source;
	}
}
