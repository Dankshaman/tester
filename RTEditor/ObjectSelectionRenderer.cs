using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003D9 RID: 985
	public abstract class ObjectSelectionRenderer
	{
		// Token: 0x06002DF3 RID: 11763
		public abstract void RenderObjectSelection(HashSet<GameObject> selectedObjects, ObjectSelectionSettings objectSelectionSettings);
	}
}
