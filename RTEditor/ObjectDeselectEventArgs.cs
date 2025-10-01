using System;

namespace RTEditor
{
	// Token: 0x020003DE RID: 990
	public class ObjectDeselectEventArgs
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002E02 RID: 11778 RVA: 0x0013DC67 File Offset: 0x0013BE67
		public ObjectDeselectActionType DeselectActionType
		{
			get
			{
				return this._deselectActionType;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002E03 RID: 11779 RVA: 0x0013DC6F File Offset: 0x0013BE6F
		public GizmoType GizmoType
		{
			get
			{
				return this._gizmoType;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002E04 RID: 11780 RVA: 0x0013DC77 File Offset: 0x0013BE77
		public bool IsGizmoActive
		{
			get
			{
				return this._isGizmoActive;
			}
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x0013DC7F File Offset: 0x0013BE7F
		public ObjectDeselectEventArgs(ObjectDeselectActionType deselectActionType)
		{
			this._deselectActionType = deselectActionType;
			this._gizmoType = MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType;
			this._isGizmoActive = !MonoSingletonBase<EditorGizmoSystem>.Instance.AreGizmosTurnedOff;
		}

		// Token: 0x04001EB0 RID: 7856
		private ObjectDeselectActionType _deselectActionType;

		// Token: 0x04001EB1 RID: 7857
		private GizmoType _gizmoType;

		// Token: 0x04001EB2 RID: 7858
		private bool _isGizmoActive;
	}
}
