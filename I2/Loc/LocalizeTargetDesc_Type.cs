using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200048B RID: 1163
	public class LocalizeTargetDesc_Type<T, G> : LocalizeTargetDesc<!1> where T : UnityEngine.Object where G : LocalizeTarget<!0>
	{
		// Token: 0x06003478 RID: 13432 RVA: 0x00160BDD File Offset: 0x0015EDDD
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.GetComponent<T>() != null;
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x00160BF0 File Offset: 0x0015EDF0
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			T component = cmp.GetComponent<T>();
			if (component == null)
			{
				return null;
			}
			G g = ScriptableObject.CreateInstance<G>();
			g.mTarget = component;
			return g;
		}
	}
}
