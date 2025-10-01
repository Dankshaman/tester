using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003BB RID: 955
	public class EditorCameraFocusOperationInfo
	{
		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x0013B289 File Offset: 0x00139489
		// (set) Token: 0x06002D26 RID: 11558 RVA: 0x0013B291 File Offset: 0x00139491
		public Vector3 CameraDestinationPosition { get; set; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x0013B29A File Offset: 0x0013949A
		// (set) Token: 0x06002D28 RID: 11560 RVA: 0x0013B2A2 File Offset: 0x001394A2
		public float OrthoCameraHalfVerticalSize { get; set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x0013B2AB File Offset: 0x001394AB
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x0013B2B3 File Offset: 0x001394B3
		public Vector3 FocusPoint { get; set; }
	}
}
