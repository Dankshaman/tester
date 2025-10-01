using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E9 RID: 1001
	public class ObjectDuplicationAction : IUndoableAndRedoableAction, IUndoableAction, IRedoableAction, IAction
	{
		// Token: 0x06002E47 RID: 11847 RVA: 0x0013E1F7 File Offset: 0x0013C3F7
		public ObjectDuplicationAction(List<GameObject> sourceObjects)
		{
			if (sourceObjects != null && sourceObjects.Count != 0)
			{
				this._sourceParents = GameObjectExtensions.GetParentsFromObjectCollection(sourceObjects);
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x0013E22C File Offset: 0x0013C42C
		public void Execute()
		{
			if (this._sourceParents.Count != 0)
			{
				foreach (GameObject gameObject in this._sourceParents)
				{
					Transform transform = gameObject.transform;
					GameObject item = gameObject.CloneAbsTRS(transform.position, transform.rotation, transform.lossyScale, transform.parent);
					this._duplicateObjectParents.Add(item);
				}
				if (MonoSingletonBase<RuntimeEditorApplication>.Instance.EnableUndoRedo)
				{
					MonoSingletonBase<EditorUndoRedoSystem>.Instance.RegisterAction(this);
				}
			}
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x0013E2CC File Offset: 0x0013C4CC
		public void Undo()
		{
			if (this._duplicateObjectParents.Count != 0)
			{
				foreach (GameObject obj in this._duplicateObjectParents)
				{
					UnityEngine.Object.Destroy(obj);
				}
				this._duplicateObjectParents.Clear();
			}
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x0013E334 File Offset: 0x0013C534
		public void Redo()
		{
			if (this._sourceParents.Count != 0)
			{
				this._duplicateObjectParents.Clear();
				foreach (GameObject gameObject in this._sourceParents)
				{
					Transform transform = gameObject.transform;
					GameObject item = gameObject.CloneAbsTRS(transform.position, transform.rotation, transform.lossyScale, transform.parent);
					this._duplicateObjectParents.Add(item);
				}
			}
		}

		// Token: 0x04001EDC RID: 7900
		private List<GameObject> _sourceParents = new List<GameObject>();

		// Token: 0x04001EDD RID: 7901
		private List<GameObject> _duplicateObjectParents = new List<GameObject>();

		// Token: 0x04001EDE RID: 7902
		private ObjectSelectionSnapshot _preDuplicateSelectionSnapshot;
	}
}
