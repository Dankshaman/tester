using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x02000485 RID: 1157
	[AddComponentMenu("I2/Localization/Localize Dropdown")]
	public class LocalizeDropdown : MonoBehaviour
	{
		// Token: 0x06003419 RID: 13337 RVA: 0x0015F079 File Offset: 0x0015D279
		public void Start()
		{
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
			this.OnLocalize();
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x0015F092 File Offset: 0x0015D292
		public void OnDestroy()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x0015F0A5 File Offset: 0x0015D2A5
		private void OnEnable()
		{
			if (this._Terms.Count == 0)
			{
				this.FillValues();
			}
			this.OnLocalize();
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x0015F0C0 File Offset: 0x0015D2C0
		public void OnLocalize()
		{
			if (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			this.UpdateLocalization();
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x0015F0FC File Offset: 0x0015D2FC
		private void FillValues()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null && I2Utils.IsPlaying())
			{
				this.FillValuesTMPro();
				return;
			}
			foreach (Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x0015F178 File Offset: 0x0015D378
		public void UpdateLocalization()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				this.UpdateLocalizationTMPro();
				return;
			}
			component.options.Clear();
			foreach (string term in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(term, true, 0, true, false, null, null);
				component.options.Add(new Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x0015F208 File Offset: 0x0015D408
		public void UpdateLocalizationTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			component.options.Clear();
			foreach (string term in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(term, true, 0, true, false, null, null);
				component.options.Add(new TMP_Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x0015F294 File Offset: 0x0015D494
		private void FillValuesTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			foreach (TMP_Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x04002157 RID: 8535
		public List<string> _Terms = new List<string>();
	}
}
