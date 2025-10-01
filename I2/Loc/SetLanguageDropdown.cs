using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020004B3 RID: 1203
	[AddComponentMenu("I2/Localization/SetLanguage Dropdown")]
	public class SetLanguageDropdown : MonoBehaviour
	{
		// Token: 0x0600356A RID: 13674 RVA: 0x001647D0 File Offset: 0x001629D0
		private void OnEnable()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				return;
			}
			string currentLanguage = LocalizationManager.CurrentLanguage;
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			component.ClearOptions();
			component.AddOptions(allLanguages);
			component.value = allLanguages.IndexOf(currentLanguage);
			component.onValueChanged.RemoveListener(new UnityAction<int>(this.OnValueChanged));
			component.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x00164858 File Offset: 0x00162A58
		private void OnValueChanged(int index)
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (index < 0)
			{
				index = 0;
				component.value = index;
			}
			LocalizationManager.CurrentLanguage = component.options[index].text;
		}
	}
}
