using System;

namespace I2.Loc
{
	// Token: 0x020004A7 RID: 1191
	[Serializable]
	public struct LocalizedString
	{
		// Token: 0x06003543 RID: 13635 RVA: 0x00162E55 File Offset: 0x00161055
		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x00162E64 File Offset: 0x00161064
		public static implicit operator LocalizedString(string term)
		{
			return new LocalizedString
			{
				mTerm = term
			};
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x00162E82 File Offset: 0x00161082
		public LocalizedString(LocalizedString str)
		{
			this.mTerm = str.mTerm;
			this.mRTL_IgnoreArabicFix = str.mRTL_IgnoreArabicFix;
			this.mRTL_MaxLineLength = str.mRTL_MaxLineLength;
			this.mRTL_ConvertNumbers = str.mRTL_ConvertNumbers;
			this.m_DontLocalizeParameters = str.m_DontLocalizeParameters;
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x00162EC0 File Offset: 0x001610C0
		public override string ToString()
		{
			string translation = LocalizationManager.GetTranslation(this.mTerm, !this.mRTL_IgnoreArabicFix, this.mRTL_MaxLineLength, !this.mRTL_ConvertNumbers, true, null, null);
			LocalizationManager.ApplyLocalizationParams(ref translation, !this.m_DontLocalizeParameters);
			return translation;
		}

		// Token: 0x0400219E RID: 8606
		public string mTerm;

		// Token: 0x0400219F RID: 8607
		public bool mRTL_IgnoreArabicFix;

		// Token: 0x040021A0 RID: 8608
		public int mRTL_MaxLineLength;

		// Token: 0x040021A1 RID: 8609
		public bool mRTL_ConvertNumbers;

		// Token: 0x040021A2 RID: 8610
		public bool m_DontLocalizeParameters;
	}
}
