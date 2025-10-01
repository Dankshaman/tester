using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200048A RID: 1162
	public abstract class LocalizeTargetDesc<T> : ILocalizeTargetDescriptor where T : ILocalizeTarget
	{
		// Token: 0x06003475 RID: 13429 RVA: 0x00160BBD File Offset: 0x0015EDBD
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			return ScriptableObject.CreateInstance<T>();
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x00160BC9 File Offset: 0x0015EDC9
		public override Type GetTargetType()
		{
			return typeof(!0);
		}
	}
}
