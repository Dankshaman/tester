using System;
using System.Collections.Generic;

namespace RTEditor
{
	// Token: 0x020003EB RID: 1003
	public class PostGizmoTransformedObjectsAction : IUndoableAndRedoableAction, IUndoableAction, IRedoableAction, IAction
	{
		// Token: 0x06002E4F RID: 11855 RVA: 0x0013E41D File Offset: 0x0013C61D
		public PostGizmoTransformedObjectsAction(List<ObjectTransformSnapshot> preTransformObjectSnapshots, List<ObjectTransformSnapshot> postTransformObjectSnapshot, Gizmo gizmoWhichTransformedObjects)
		{
			this._preTransformObjectSnapshots = new List<ObjectTransformSnapshot>(preTransformObjectSnapshots);
			this._postTransformObjectSnapshot = new List<ObjectTransformSnapshot>(postTransformObjectSnapshot);
			this._gizmoWhichTransformedObjects = gizmoWhichTransformedObjects;
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x0013E444 File Offset: 0x0013C644
		public void Execute()
		{
			GizmoTransformedObjectsMessage.SendToInterestedListeners(this._gizmoWhichTransformedObjects);
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.EnableUndoRedo)
			{
				MonoSingletonBase<EditorUndoRedoSystem>.Instance.RegisterAction(this);
			}
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x0013E468 File Offset: 0x0013C668
		public void Undo()
		{
			foreach (ObjectTransformSnapshot objectTransformSnapshot in this._preTransformObjectSnapshots)
			{
				objectTransformSnapshot.ApplySnapshot();
			}
			GizmoTransformOperationWasUndoneMessage.SendToInterestedListeners(this._gizmoWhichTransformedObjects);
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x0013E4C4 File Offset: 0x0013C6C4
		public void Redo()
		{
			foreach (ObjectTransformSnapshot objectTransformSnapshot in this._postTransformObjectSnapshot)
			{
				objectTransformSnapshot.ApplySnapshot();
			}
			GizmoTransformOperationWasRedoneMessage.SendToInterestedListeners(this._gizmoWhichTransformedObjects);
		}

		// Token: 0x04001EE1 RID: 7905
		private List<ObjectTransformSnapshot> _preTransformObjectSnapshots;

		// Token: 0x04001EE2 RID: 7906
		private List<ObjectTransformSnapshot> _postTransformObjectSnapshot;

		// Token: 0x04001EE3 RID: 7907
		private Gizmo _gizmoWhichTransformedObjects;
	}
}
