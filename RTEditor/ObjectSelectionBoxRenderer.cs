using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003D4 RID: 980
	public abstract class ObjectSelectionBoxRenderer
	{
		// Token: 0x06002DEC RID: 11756
		public abstract void RenderObjectSelectionBoxes(HashSet<GameObject> selectedObjects);
	}
}
