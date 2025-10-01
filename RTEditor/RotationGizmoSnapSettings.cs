using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000405 RID: 1029
	[Serializable]
	public class RotationGizmoSnapSettings
	{
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002F6B RID: 12139 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinStepValue
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002F6C RID: 12140 RVA: 0x00143F44 File Offset: 0x00142144
		// (set) Token: 0x06002F6D RID: 12141 RVA: 0x00143F4C File Offset: 0x0014214C
		public float StepValueInDegrees
		{
			get
			{
				return this._stepValueInDegrees;
			}
			set
			{
				this._stepValueInDegrees = Mathf.Max(RotationGizmoSnapSettings.MinStepValue, value);
			}
		}

		// Token: 0x04001F32 RID: 7986
		[SerializeField]
		private float _stepValueInDegrees = 15f;
	}
}
