using System;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x02000477 RID: 1143
	public class TranslationJob_WWW : TranslationJob
	{
		// Token: 0x0600337F RID: 13183 RVA: 0x0015AD3A File Offset: 0x00158F3A
		public override void Dispose()
		{
			if (this.www != null)
			{
				this.www.Dispose();
			}
			this.www = null;
		}

		// Token: 0x040020E6 RID: 8422
		public UnityWebRequest www;
	}
}
