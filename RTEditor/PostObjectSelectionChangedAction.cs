using System;

namespace RTEditor
{
	// Token: 0x020003EA RID: 1002
	public class PostObjectSelectionChangedAction : IUndoableAndRedoableAction, IUndoableAction, IRedoableAction, IAction
	{
		// Token: 0x06002E4B RID: 11851 RVA: 0x0013E3C8 File Offset: 0x0013C5C8
		public PostObjectSelectionChangedAction(ObjectSelectionSnapshot preChangeSelectionSnapshot, ObjectSelectionSnapshot postChangeSelectionSnapshot)
		{
			this._preChangeSelectionSnapshot = preChangeSelectionSnapshot;
			this._postChangeSelectionSnapshot = postChangeSelectionSnapshot;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x0013E3DE File Offset: 0x0013C5DE
		public void Execute()
		{
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.EnableUndoRedo)
			{
				MonoSingletonBase<EditorUndoRedoSystem>.Instance.RegisterAction(this);
			}
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x0013E3F7 File Offset: 0x0013C5F7
		public void Undo()
		{
			MonoSingletonBase<EditorObjectSelection>.Instance.UndoRedoSelection(this._preChangeSelectionSnapshot, UndoRedoActionType.Undo);
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x0013E40A File Offset: 0x0013C60A
		public void Redo()
		{
			MonoSingletonBase<EditorObjectSelection>.Instance.UndoRedoSelection(this._postChangeSelectionSnapshot, UndoRedoActionType.Redo);
		}

		// Token: 0x04001EDF RID: 7903
		private ObjectSelectionSnapshot _preChangeSelectionSnapshot;

		// Token: 0x04001EE0 RID: 7904
		private ObjectSelectionSnapshot _postChangeSelectionSnapshot;
	}
}
