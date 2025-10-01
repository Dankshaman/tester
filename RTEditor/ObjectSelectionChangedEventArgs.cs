using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E2 RID: 994
	public class ObjectSelectionChangedEventArgs
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002E0C RID: 11788 RVA: 0x0013DDEC File Offset: 0x0013BFEC
		public ObjectSelectActionType SelectActionType
		{
			get
			{
				return this._selectActionType;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06002E0D RID: 11789 RVA: 0x0013DDF4 File Offset: 0x0013BFF4
		public List<GameObject> SelectedObjects
		{
			get
			{
				return new List<GameObject>(this._selectedObjects);
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06002E0E RID: 11790 RVA: 0x0013DE01 File Offset: 0x0013C001
		public ObjectDeselectActionType DeselectActionType
		{
			get
			{
				return this._deselectActionType;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06002E0F RID: 11791 RVA: 0x0013DE09 File Offset: 0x0013C009
		public List<GameObject> DeselectedObjects
		{
			get
			{
				return new List<GameObject>(this._deselectedObjects);
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002E10 RID: 11792 RVA: 0x0013DE16 File Offset: 0x0013C016
		public GizmoType GizmoType
		{
			get
			{
				return this._gizmoType;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x0013DE1E File Offset: 0x0013C01E
		public bool IsGizmoActive
		{
			get
			{
				return this._isGizmoActive;
			}
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0013DE28 File Offset: 0x0013C028
		public ObjectSelectionChangedEventArgs(ObjectSelectActionType selectActionType, List<GameObject> selectedObjects, ObjectDeselectActionType deselectActionType, List<GameObject> deselectedObjects)
		{
			this._selectActionType = selectActionType;
			this._selectedObjects = new List<GameObject>();
			if (selectedObjects != null)
			{
				this._selectedObjects = new List<GameObject>(selectedObjects);
			}
			this._deselectActionType = deselectActionType;
			this._deselectedObjects = new List<GameObject>();
			if (this._deselectedObjects != null)
			{
				this._deselectedObjects = new List<GameObject>(deselectedObjects);
			}
			this._gizmoType = MonoSingletonBase<EditorGizmoSystem>.Instance.ActiveGizmoType;
			this._isGizmoActive = !MonoSingletonBase<EditorGizmoSystem>.Instance.AreGizmosTurnedOff;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0013DEA8 File Offset: 0x0013C0A8
		public static ObjectSelectionChangedEventArgs FromSnapshots(ObjectSelectActionType selectActionType, ObjectDeselectActionType deselectActionType, ObjectSelectionSnapshot preChangeSnapshot, ObjectSelectionSnapshot postChangeSnapshot)
		{
			List<GameObject> diff = preChangeSnapshot.GetDiff(postChangeSnapshot);
			List<GameObject> diff2 = postChangeSnapshot.GetDiff(preChangeSnapshot);
			return new ObjectSelectionChangedEventArgs(selectActionType, diff2, deselectActionType, diff);
		}

		// Token: 0x04001EBF RID: 7871
		private ObjectSelectActionType _selectActionType;

		// Token: 0x04001EC0 RID: 7872
		private List<GameObject> _selectedObjects;

		// Token: 0x04001EC1 RID: 7873
		private ObjectDeselectActionType _deselectActionType;

		// Token: 0x04001EC2 RID: 7874
		private List<GameObject> _deselectedObjects;

		// Token: 0x04001EC3 RID: 7875
		private GizmoType _gizmoType;

		// Token: 0x04001EC4 RID: 7876
		private bool _isGizmoActive;
	}
}
