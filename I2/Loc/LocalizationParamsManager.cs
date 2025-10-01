using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020004A6 RID: 1190
	public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x0600353C RID: 13628 RVA: 0x00162CEC File Offset: 0x00160EEC
		public string GetParameterValue(string ParamName)
		{
			if (this._Params != null)
			{
				int i = 0;
				int count = this._Params.Count;
				while (i < count)
				{
					if (this._Params[i].Name == ParamName)
					{
						return this._Params[i].Value;
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x00162D48 File Offset: 0x00160F48
		public void SetParameterValue(string ParamName, string ParamValue, bool localize = true)
		{
			bool flag = false;
			int i = 0;
			int count = this._Params.Count;
			while (i < count)
			{
				if (this._Params[i].Name == ParamName)
				{
					LocalizationParamsManager.ParamValue value = this._Params[i];
					value.Value = ParamValue;
					this._Params[i] = value;
					flag = true;
					break;
				}
				i++;
			}
			if (!flag)
			{
				this._Params.Add(new LocalizationParamsManager.ParamValue
				{
					Name = ParamName,
					Value = ParamValue
				});
			}
			if (localize)
			{
				this.OnLocalize();
			}
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x00162DE0 File Offset: 0x00160FE0
		public void OnLocalize()
		{
			Localize component = base.GetComponent<Localize>();
			if (component != null)
			{
				component.OnLocalize(true);
			}
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x00162E04 File Offset: 0x00161004
		public virtual void OnEnable()
		{
			if (this._IsGlobalManager)
			{
				this.DoAutoRegister();
			}
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x00162E14 File Offset: 0x00161014
		public void DoAutoRegister()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x00162E34 File Offset: 0x00161034
		public void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x0400219C RID: 8604
		[SerializeField]
		public List<LocalizationParamsManager.ParamValue> _Params = new List<LocalizationParamsManager.ParamValue>();

		// Token: 0x0400219D RID: 8605
		public bool _IsGlobalManager;

		// Token: 0x0200082F RID: 2095
		[Serializable]
		public struct ParamValue
		{
			// Token: 0x04002E7E RID: 11902
			public string Name;

			// Token: 0x04002E7F RID: 11903
			public string Value;
		}
	}
}
