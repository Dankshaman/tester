using System;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x020004A2 RID: 1186
	[AddComponentMenu("I2/Localization/I2 Localize Callback")]
	public class CustomLocalizeCallback : MonoBehaviour
	{
		// Token: 0x06003526 RID: 13606 RVA: 0x0016258F File Offset: 0x0016078F
		public void OnEnable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x001625B3 File Offset: 0x001607B3
		public void OnDisable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x001625C6 File Offset: 0x001607C6
		public void OnLocalize()
		{
			this._OnLocalize.Invoke();
		}

		// Token: 0x04002198 RID: 8600
		public UnityEvent _OnLocalize = new UnityEvent();
	}
}
