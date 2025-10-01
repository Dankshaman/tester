using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200049F RID: 1183
	public class TermsPopup : PropertyAttribute
	{
		// Token: 0x0600351D RID: 13597 RVA: 0x001624FC File Offset: 0x001606FC
		public TermsPopup(string filter = "")
		{
			this.Filter = filter;
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x0600351E RID: 13598 RVA: 0x0016250B File Offset: 0x0016070B
		// (set) Token: 0x0600351F RID: 13599 RVA: 0x00162513 File Offset: 0x00160713
		public string Filter { get; private set; }
	}
}
