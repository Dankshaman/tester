using System;

namespace RTEditor
{
	// Token: 0x020003E0 RID: 992
	public class ObjectSelectEventArgs
	{
		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002E06 RID: 11782 RVA: 0x0013DCB1 File Offset: 0x0013BEB1
		public ObjectSelectActionType SelectActionType
		{
			get
			{
				return this._selectActionType;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002E07 RID: 11783 RVA: 0x0013DCB9 File Offset: 0x0013BEB9
		public GizmoType GizmoType
		{
			get
			{
				return this._gizmoType;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002E08 RID: 11784 RVA: 0x0013DCC1 File Offset: 0x0013BEC1
		public bool IsGizmoActive
		{
			get
			{
				return this._isGizmoActive;
			}
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x0013DCC9 File Offset: 0x0013BEC9
		public ObjectSelectEventArgs(ObjectSelectActionType selectActionType)
		{
			this._selectActionType = selectActionType;
			this._gizmoType = MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType;
			this._isGizmoActive = !MonoSingletonBase<EditorGizmoSystem>.Instance.AreGizmosTurnedOff;
		}

		// Token: 0x04001EBC RID: 7868
		private ObjectSelectActionType _selectActionType;

		// Token: 0x04001EBD RID: 7869
		private GizmoType _gizmoType;

		// Token: 0x04001EBE RID: 7870
		private bool _isGizmoActive;
	}
}
