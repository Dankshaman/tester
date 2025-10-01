using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003BD RID: 957
	[Serializable]
	public class EditorCameraMoveSettings
	{
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static float MinMoveSpeed
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x0013B34E File Offset: 0x0013954E
		// (set) Token: 0x06002D3A RID: 11578 RVA: 0x0013B356 File Offset: 0x00139556
		public float MoveSpeed
		{
			get
			{
				return this._moveSpeed;
			}
			set
			{
				this._moveSpeed = Mathf.Max(value, EditorCameraMoveSettings.MinMoveSpeed);
			}
		}

		// Token: 0x04001E4A RID: 7754
		[SerializeField]
		private float _moveSpeed = 10f;
	}
}
