using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020004AF RID: 1199
	public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06003559 RID: 13657 RVA: 0x00162E14 File Offset: 0x00161014
		public virtual void OnEnable()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x00162E34 File Offset: 0x00161034
		public virtual void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x00079594 File Offset: 0x00077794
		public virtual string GetParameterValue(string ParamName)
		{
			return null;
		}
	}
}
