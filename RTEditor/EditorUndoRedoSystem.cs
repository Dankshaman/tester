using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F0 RID: 1008
	public class EditorUndoRedoSystem : MonoSingletonBase<EditorUndoRedoSystem>
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x0013E520 File Offset: 0x0013C720
		// (set) Token: 0x06002E57 RID: 11863 RVA: 0x0013E528 File Offset: 0x0013C728
		public int ActionLimit
		{
			get
			{
				return this._actionLimit;
			}
			set
			{
				this.ChangeActionLimit(value);
			}
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x0013E531 File Offset: 0x0013C731
		public void ClearActions()
		{
			this._actionStack.Clear();
			this._actionStackPointer = -1;
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x0013E548 File Offset: 0x0013C748
		public void RegisterAction(IUndoableAndRedoableAction action)
		{
			if (this._actionStackPointer < 0)
			{
				this._actionStack.Add(action);
				this._actionStackPointer = 0;
				return;
			}
			if (this._actionStackPointer < this._actionStack.Count - 1)
			{
				int num = this._actionStackPointer + 1;
				int count = this._actionStack.Count - num;
				this._actionStack.RemoveRange(num, count);
			}
			this._actionStack.Add(action);
			if (this._actionStack.Count > this._actionLimit)
			{
				this._actionStack.RemoveAt(0);
			}
			this._actionStackPointer = this._actionStack.Count - 1;
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x0013E5EC File Offset: 0x0013C7EC
		private void Update()
		{
			if (!Application.isEditor)
			{
				if (Input.GetKeyDown(KeyCode.Z) && InputHelper.IsAnyCtrlOrCommandKeyPressed())
				{
					this.Undo();
					return;
				}
				if (Input.GetKeyDown(KeyCode.Y) && InputHelper.IsAnyCtrlOrCommandKeyPressed())
				{
					this.Redo();
					return;
				}
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Z) && InputHelper.IsAnyCtrlOrCommandKeyPressed() && InputHelper.IsAnyShiftKeyPressed())
				{
					this.Undo();
					return;
				}
				if (Input.GetKeyDown(KeyCode.Y) && InputHelper.IsAnyCtrlOrCommandKeyPressed() && InputHelper.IsAnyShiftKeyPressed())
				{
					this.Redo();
				}
			}
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x0013E669 File Offset: 0x0013C869
		private void Undo()
		{
			if (this._actionStack.Count == 0 || this._actionStackPointer < 0)
			{
				return;
			}
			this._actionStack[this._actionStackPointer].Undo();
			this._actionStackPointer--;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x0013E6A8 File Offset: 0x0013C8A8
		private void Redo()
		{
			if (this._actionStack.Count == 0 || this._actionStackPointer == this._actionStack.Count - 1)
			{
				return;
			}
			this._actionStackPointer++;
			this._actionStack[this._actionStackPointer].Redo();
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x0013E6FC File Offset: 0x0013C8FC
		private void ChangeActionLimit(int newActionLimit)
		{
			int actionLimit = this._actionLimit;
			this._actionLimit = Mathf.Max(1, newActionLimit);
			if (Application.isPlaying && this._actionLimit < actionLimit && this._actionStackPointer >= this._actionLimit)
			{
				this._actionStack.RemoveRange(0, actionLimit - this._actionLimit);
				this._actionStackPointer = this._actionStack.Count - 1;
			}
		}

		// Token: 0x04001EE4 RID: 7908
		[SerializeField]
		private int _actionLimit = 50;

		// Token: 0x04001EE5 RID: 7909
		private List<IUndoableAndRedoableAction> _actionStack = new List<IUndoableAndRedoableAction>();

		// Token: 0x04001EE6 RID: 7910
		private int _actionStackPointer = -1;
	}
}
