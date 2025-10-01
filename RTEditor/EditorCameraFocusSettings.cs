using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003BC RID: 956
	[Serializable]
	public class EditorCameraFocusSettings
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06002D2C RID: 11564 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinFocusSpeed
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinSmoothFocusTime
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static float MinFocusScale
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x0013B2BC File Offset: 0x001394BC
		// (set) Token: 0x06002D30 RID: 11568 RVA: 0x0013B2C4 File Offset: 0x001394C4
		public EditorCameraFocusMode FocusMode
		{
			get
			{
				return this._focusMode;
			}
			set
			{
				this._focusMode = value;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06002D31 RID: 11569 RVA: 0x0013B2CD File Offset: 0x001394CD
		// (set) Token: 0x06002D32 RID: 11570 RVA: 0x0013B2D5 File Offset: 0x001394D5
		public float ConstantFocusSpeed
		{
			get
			{
				return this._constantFocusSpeed;
			}
			set
			{
				this._constantFocusSpeed = Mathf.Max(EditorCameraFocusSettings.MinFocusSpeed, value);
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x0013B2E8 File Offset: 0x001394E8
		// (set) Token: 0x06002D34 RID: 11572 RVA: 0x0013B2F0 File Offset: 0x001394F0
		public float SmoothFocusTime
		{
			get
			{
				return this._smoothFocusTime;
			}
			set
			{
				this._smoothFocusTime = Mathf.Max(EditorCameraFocusSettings.MinSmoothFocusTime, value);
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06002D35 RID: 11573 RVA: 0x0013B303 File Offset: 0x00139503
		// (set) Token: 0x06002D36 RID: 11574 RVA: 0x0013B30B File Offset: 0x0013950B
		public float FocusDistanceScale
		{
			get
			{
				return this._focusDistanceScale;
			}
			set
			{
				this._focusDistanceScale = Mathf.Max(EditorCameraFocusSettings.MinFocusScale, value);
			}
		}

		// Token: 0x04001E46 RID: 7750
		[SerializeField]
		private EditorCameraFocusMode _focusMode = EditorCameraFocusMode.Smooth;

		// Token: 0x04001E47 RID: 7751
		[SerializeField]
		private float _constantFocusSpeed = 10f;

		// Token: 0x04001E48 RID: 7752
		[SerializeField]
		private float _smoothFocusTime = 0.3f;

		// Token: 0x04001E49 RID: 7753
		[SerializeField]
		private float _focusDistanceScale = 1.5f;
	}
}
