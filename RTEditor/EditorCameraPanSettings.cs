using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003C1 RID: 961
	[Serializable]
	public class EditorCameraPanSettings
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002D3E RID: 11582 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinPanSpeed
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x0013B416 File Offset: 0x00139616
		public static float MinSmoothValue
		{
			get
			{
				return 1E-05f;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06002D40 RID: 11584 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static float MaxSmoothValue
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x0013B41D File Offset: 0x0013961D
		// (set) Token: 0x06002D42 RID: 11586 RVA: 0x0013B425 File Offset: 0x00139625
		public EditorCameraPanMode PanMode
		{
			get
			{
				return this._panMode;
			}
			set
			{
				this._panMode = value;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06002D43 RID: 11587 RVA: 0x0013B42E File Offset: 0x0013962E
		// (set) Token: 0x06002D44 RID: 11588 RVA: 0x0013B436 File Offset: 0x00139636
		public float SmoothValue
		{
			get
			{
				return this._smoothValue;
			}
			set
			{
				this._smoothValue = Mathf.Min(EditorCameraPanSettings.MaxSmoothValue, Mathf.Max(EditorCameraPanSettings.MinSmoothValue, value));
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06002D45 RID: 11589 RVA: 0x0013B453 File Offset: 0x00139653
		// (set) Token: 0x06002D46 RID: 11590 RVA: 0x0013B45B File Offset: 0x0013965B
		public float StandardPanSpeed
		{
			get
			{
				return this._standardPanSpeed;
			}
			set
			{
				this._standardPanSpeed = Mathf.Max(value, EditorCameraPanSettings.MinPanSpeed);
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06002D47 RID: 11591 RVA: 0x0013B46E File Offset: 0x0013966E
		// (set) Token: 0x06002D48 RID: 11592 RVA: 0x0013B476 File Offset: 0x00139676
		public float SmoothPanSpeed
		{
			get
			{
				return this._smoothPanSpeed;
			}
			set
			{
				this._smoothPanSpeed = Mathf.Max(value, EditorCameraPanSettings.MinPanSpeed);
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x0013B489 File Offset: 0x00139689
		// (set) Token: 0x06002D4A RID: 11594 RVA: 0x0013B491 File Offset: 0x00139691
		public bool InvertXAxis
		{
			get
			{
				return this._invertXAxis;
			}
			set
			{
				this._invertXAxis = value;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x0013B49A File Offset: 0x0013969A
		// (set) Token: 0x06002D4C RID: 11596 RVA: 0x0013B4A2 File Offset: 0x001396A2
		public bool InvertYAxis
		{
			get
			{
				return this._invertYAxis;
			}
			set
			{
				this._invertYAxis = value;
			}
		}

		// Token: 0x04001E4E RID: 7758
		private EditorCameraPanMode _panMode;

		// Token: 0x04001E4F RID: 7759
		private float _smoothValue = 0.15f;

		// Token: 0x04001E50 RID: 7760
		[SerializeField]
		private float _standardPanSpeed = 3f;

		// Token: 0x04001E51 RID: 7761
		[SerializeField]
		private float _smoothPanSpeed = 3f;

		// Token: 0x04001E52 RID: 7762
		[SerializeField]
		private bool _invertXAxis;

		// Token: 0x04001E53 RID: 7763
		[SerializeField]
		private bool _invertYAxis;
	}
}
