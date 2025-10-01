using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200040F RID: 1039
	[Serializable]
	public class TranslationGizmoSnapSettings
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600304D RID: 12365 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinStepValue
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x0014A5F4 File Offset: 0x001487F4
		// (set) Token: 0x0600304F RID: 12367 RVA: 0x0014A5FC File Offset: 0x001487FC
		public float StepValueInWorldUnits
		{
			get
			{
				return this._stepValueInWorldUnits;
			}
			set
			{
				this._stepValueInWorldUnits = Mathf.Max(TranslationGizmoSnapSettings.MinStepValue, value);
			}
		}

		// Token: 0x04001F91 RID: 8081
		private float _stepValueInWorldUnits = 1f;
	}
}
