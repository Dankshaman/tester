using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x0200046F RID: 1135
	public class BaseSpecializationManager
	{
		// Token: 0x06003342 RID: 13122 RVA: 0x00155D64 File Offset: 0x00153F64
		public virtual void InitializeSpecializations()
		{
			this.mSpecializations = new string[]
			{
				"Any",
				"PC",
				"Touch",
				"Controller",
				"VR",
				"XBox",
				"PS4",
				"OculusVR",
				"ViveVR",
				"GearVR",
				"Android",
				"IOS"
			};
			this.mSpecializationsFallbacks = new Dictionary<string, string>
			{
				{
					"XBox",
					"Controller"
				},
				{
					"PS4",
					"Controller"
				},
				{
					"OculusVR",
					"VR"
				},
				{
					"ViveVR",
					"VR"
				},
				{
					"GearVR",
					"VR"
				},
				{
					"Android",
					"Touch"
				},
				{
					"IOS",
					"Touch"
				}
			};
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x00155E5C File Offset: 0x0015405C
		public virtual string GetCurrentSpecialization()
		{
			if (this.mSpecializations == null)
			{
				this.InitializeSpecializations();
			}
			return "PC";
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x00155E74 File Offset: 0x00154074
		public virtual string GetFallbackSpecialization(string specialization)
		{
			if (this.mSpecializationsFallbacks == null)
			{
				this.InitializeSpecializations();
			}
			string result;
			if (this.mSpecializationsFallbacks.TryGetValue(specialization, out result))
			{
				return result;
			}
			return "Any";
		}

		// Token: 0x040020D0 RID: 8400
		public string[] mSpecializations;

		// Token: 0x040020D1 RID: 8401
		public Dictionary<string, string> mSpecializationsFallbacks;
	}
}
