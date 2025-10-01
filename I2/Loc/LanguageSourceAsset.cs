using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200047F RID: 1151
	[CreateAssetMenu(fileName = "I2Languages", menuName = "I2 Localization/LanguageSource", order = 1)]
	public class LanguageSourceAsset : ScriptableObject, ILanguageSource
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x0015B9D7 File Offset: 0x00159BD7
		// (set) Token: 0x060033A3 RID: 13219 RVA: 0x0015B9DF File Offset: 0x00159BDF
		public LanguageSourceData SourceData
		{
			get
			{
				return this.mSource;
			}
			set
			{
				this.mSource = value;
			}
		}

		// Token: 0x04002117 RID: 8471
		public LanguageSourceData mSource = new LanguageSourceData();
	}
}
