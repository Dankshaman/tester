using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003DC RID: 988
	public abstract class ObjectSelectionShape
	{
		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002DFA RID: 11770 RVA: 0x0013DBDC File Offset: 0x0013BDDC
		// (set) Token: 0x06002DFB RID: 11771 RVA: 0x0013DBE4 File Offset: 0x0013BDE4
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				this._isVisible = value;
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x0013DBED File Offset: 0x0013BDED
		public void SetEnclosingRectTopLeftPoint(Vector2 topLeftPoint)
		{
			this._enclosingRectangle.xMin = topLeftPoint.x;
			this._enclosingRectangle.yMax = topLeftPoint.y;
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x0013DC11 File Offset: 0x0013BE11
		public void SetEnclosingRectBottomRightPoint(Vector2 bottomRightPoint)
		{
			this._enclosingRectangle.xMax = bottomRightPoint.x;
			this._enclosingRectangle.yMin = bottomRightPoint.y;
		}

		// Token: 0x06002DFE RID: 11774
		public abstract void Render();

		// Token: 0x06002DFF RID: 11775
		public abstract List<GameObject> GetIntersectingGameObjects(List<GameObject> gameObjects, Camera camera);

		// Token: 0x06002E00 RID: 11776 RVA: 0x0013DC35 File Offset: 0x0013BE35
		protected bool IsEnclosingRectangleBigEnoughForSelection()
		{
			return Mathf.Abs(this._enclosingRectangle.width) > 2f && Mathf.Abs(this._enclosingRectangle.height) > 2f;
		}

		// Token: 0x04001EA3 RID: 7843
		protected bool _isVisible;

		// Token: 0x04001EA4 RID: 7844
		protected Rect _enclosingRectangle;
	}
}
