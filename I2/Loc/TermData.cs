using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200049E RID: 1182
	[Serializable]
	public class TermData
	{
		// Token: 0x06003513 RID: 13587 RVA: 0x0016228C File Offset: 0x0016048C
		public string GetTranslation(int idx, string specialization = null, bool editMode = false)
		{
			string text = this.Languages[idx];
			if (text != null)
			{
				text = SpecializationManager.GetSpecializedText(text, specialization);
				if (!editMode)
				{
					text = text.Replace("[i2nt]", "").Replace("[/i2nt]", "");
				}
			}
			return text;
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x001622D1 File Offset: 0x001604D1
		public void SetTranslation(int idx, string translation, string specialization = null)
		{
			this.Languages[idx] = SpecializationManager.SetSpecializedText(this.Languages[idx], translation, specialization);
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x001622EC File Offset: 0x001604EC
		public void RemoveSpecialization(string specialization)
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				this.RemoveSpecialization(i, specialization);
			}
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x00162314 File Offset: 0x00160514
		public void RemoveSpecialization(int idx, string specialization)
		{
			string text = this.Languages[idx];
			if (specialization == "Any" || !text.Contains("[i2s_" + specialization + "]"))
			{
				return;
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			specializations.Remove(specialization);
			this.Languages[idx] = SpecializationManager.SetSpecializedText(specializations);
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x0016236E File Offset: 0x0016056E
		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			return (this.Flags[idx] & 2) > 0;
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x00162380 File Offset: 0x00160580
		public void Validate()
		{
			int num = Mathf.Max(this.Languages.Length, this.Flags.Length);
			if (this.Languages.Length != num)
			{
				Array.Resize<string>(ref this.Languages, num);
			}
			if (this.Flags.Length != num)
			{
				Array.Resize<byte>(ref this.Flags, num);
			}
			if (this.Languages_Touch != null)
			{
				for (int i = 0; i < Mathf.Min(this.Languages_Touch.Length, num); i++)
				{
					if (string.IsNullOrEmpty(this.Languages[i]) && !string.IsNullOrEmpty(this.Languages_Touch[i]))
					{
						this.Languages[i] = this.Languages_Touch[i];
						this.Languages_Touch[i] = null;
					}
				}
				this.Languages_Touch = null;
			}
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x00162430 File Offset: 0x00160630
		public bool IsTerm(string name, bool allowCategoryMistmatch)
		{
			if (!allowCategoryMistmatch)
			{
				return name == this.Term;
			}
			return name == LanguageSourceData.GetKeyFromFullTerm(this.Term, false);
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x00162454 File Offset: 0x00160654
		public bool HasSpecializations()
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.Languages[i]) && this.Languages[i].Contains("[i2s_"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x0016249C File Offset: 0x0016069C
		public List<string> GetAllSpecializations()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.Languages.Length; i++)
			{
				SpecializationManager.AppendSpecializations(this.Languages[i], list);
			}
			return list;
		}

		// Token: 0x04002190 RID: 8592
		public string Term = string.Empty;

		// Token: 0x04002191 RID: 8593
		public eTermType TermType;

		// Token: 0x04002192 RID: 8594
		[NonSerialized]
		public string Description;

		// Token: 0x04002193 RID: 8595
		public string[] Languages = new string[0];

		// Token: 0x04002194 RID: 8596
		public byte[] Flags = new byte[0];

		// Token: 0x04002195 RID: 8597
		[SerializeField]
		private string[] Languages_Touch;
	}
}
