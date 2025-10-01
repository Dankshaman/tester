using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000407 RID: 1031
	[Serializable]
	public class ScaleGizmoSnapSettings
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002FC9 RID: 12233 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinStepValue
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002FCA RID: 12234 RVA: 0x00146445 File Offset: 0x00144645
		// (set) Token: 0x06002FCB RID: 12235 RVA: 0x0014644D File Offset: 0x0014464D
		public float StepValueInWorldUnits
		{
			get
			{
				return this._stepValueInWorldUnits;
			}
			set
			{
				this._stepValueInWorldUnits = Mathf.Max(ScaleGizmoSnapSettings.MinStepValue, value);
			}
		}

		// Token: 0x04001F50 RID: 8016
		[SerializeField]
		private float _stepValueInWorldUnits = 1f;
	}
}
