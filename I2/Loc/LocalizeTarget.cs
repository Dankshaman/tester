using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000488 RID: 1160
	public abstract class LocalizeTarget<T> : ILocalizeTarget where T : UnityEngine.Object
	{
		// Token: 0x0600346F RID: 13423 RVA: 0x00160B2C File Offset: 0x0015ED2C
		public override bool IsValid(Localize cmp)
		{
			if (this.mTarget != null)
			{
				Component component = this.mTarget as Component;
				if (component != null && component.gameObject != cmp.gameObject)
				{
					this.mTarget = default(!0);
				}
			}
			if (this.mTarget == null)
			{
				this.mTarget = cmp.GetComponent<T>();
			}
			return this.mTarget != null;
		}

		// Token: 0x04002168 RID: 8552
		public T mTarget;
	}
}
