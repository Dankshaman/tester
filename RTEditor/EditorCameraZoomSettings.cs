using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003C6 RID: 966
	[Serializable]
	public class EditorCameraZoomSettings
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002D51 RID: 11601 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinZoomSpeed
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002D52 RID: 11602 RVA: 0x0013B416 File Offset: 0x00139616
		public static float MinSmoothValue
		{
			get
			{
				return 1E-05f;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002D53 RID: 11603 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static float MaxSmoothValue
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002D54 RID: 11604 RVA: 0x0013B5A0 File Offset: 0x001397A0
		// (set) Token: 0x06002D55 RID: 11605 RVA: 0x0013B5A8 File Offset: 0x001397A8
		public EditorCameraZoomMode ZoomMode
		{
			get
			{
				return this._zoomMode;
			}
			set
			{
				this._zoomMode = value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06002D56 RID: 11606 RVA: 0x0013B5B1 File Offset: 0x001397B1
		// (set) Token: 0x06002D57 RID: 11607 RVA: 0x0013B5B9 File Offset: 0x001397B9
		public bool IsZoomEnabled
		{
			get
			{
				return this._isZoomEnabled;
			}
			set
			{
				this._isZoomEnabled = value;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002D58 RID: 11608 RVA: 0x0013B5C2 File Offset: 0x001397C2
		// (set) Token: 0x06002D59 RID: 11609 RVA: 0x0013B5CA File Offset: 0x001397CA
		public float OrthographicSmoothValue
		{
			get
			{
				return this._orthographicSmoothValue;
			}
			set
			{
				this._orthographicSmoothValue = Mathf.Min(EditorCameraZoomSettings.MaxSmoothValue, Mathf.Max(EditorCameraZoomSettings.MinSmoothValue, value));
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06002D5A RID: 11610 RVA: 0x0013B5E7 File Offset: 0x001397E7
		// (set) Token: 0x06002D5B RID: 11611 RVA: 0x0013B5EF File Offset: 0x001397EF
		public float PerspectiveSmoothValue
		{
			get
			{
				return this._perspectiveSmoothValue;
			}
			set
			{
				this._perspectiveSmoothValue = Mathf.Min(EditorCameraZoomSettings.MaxSmoothValue, Mathf.Max(EditorCameraZoomSettings.MinSmoothValue, value));
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06002D5C RID: 11612 RVA: 0x0013B60C File Offset: 0x0013980C
		// (set) Token: 0x06002D5D RID: 11613 RVA: 0x0013B614 File Offset: 0x00139814
		public float OrthographicStandardZoomSpeed
		{
			get
			{
				return this._orthographicStandardZoomSpeed;
			}
			set
			{
				this._orthographicStandardZoomSpeed = Mathf.Max(value, EditorCameraZoomSettings.MinZoomSpeed);
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06002D5E RID: 11614 RVA: 0x0013B627 File Offset: 0x00139827
		// (set) Token: 0x06002D5F RID: 11615 RVA: 0x0013B62F File Offset: 0x0013982F
		public float PerspectiveStandardZoomSpeed
		{
			get
			{
				return this._perspectiveStandardZoomSpeed;
			}
			set
			{
				this._perspectiveStandardZoomSpeed = Mathf.Max(value, EditorCameraZoomSettings.MinZoomSpeed);
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002D60 RID: 11616 RVA: 0x0013B642 File Offset: 0x00139842
		// (set) Token: 0x06002D61 RID: 11617 RVA: 0x0013B64A File Offset: 0x0013984A
		public float OrthographicSmoothZoomSpeed
		{
			get
			{
				return this._orthographicSmoothZoomSpeed;
			}
			set
			{
				this._orthographicSmoothZoomSpeed = Mathf.Max(value, EditorCameraZoomSettings.MinZoomSpeed);
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x0013B65D File Offset: 0x0013985D
		// (set) Token: 0x06002D63 RID: 11619 RVA: 0x0013B665 File Offset: 0x00139865
		public float PerspectiveSmoothZoomSpeed
		{
			get
			{
				return this._perspectiveSmoothZoomSpeed;
			}
			set
			{
				this._perspectiveSmoothZoomSpeed = Mathf.Max(value, EditorCameraZoomSettings.MinZoomSpeed);
			}
		}

		// Token: 0x04001E5A RID: 7770
		[SerializeField]
		private EditorCameraZoomMode _zoomMode;

		// Token: 0x04001E5B RID: 7771
		[SerializeField]
		private bool _isZoomEnabled = true;

		// Token: 0x04001E5C RID: 7772
		[SerializeField]
		private float _orthographicSmoothValue = 0.1f;

		// Token: 0x04001E5D RID: 7773
		[SerializeField]
		private float _perspectiveSmoothValue = 0.2f;

		// Token: 0x04001E5E RID: 7774
		[SerializeField]
		private float _orthographicStandardZoomSpeed = 10f;

		// Token: 0x04001E5F RID: 7775
		[SerializeField]
		private float _perspectiveStandardZoomSpeed = 400f;

		// Token: 0x04001E60 RID: 7776
		[SerializeField]
		private float _orthographicSmoothZoomSpeed = 65f;

		// Token: 0x04001E61 RID: 7777
		[SerializeField]
		private float _perspectiveSmoothZoomSpeed = 400f;
	}
}
