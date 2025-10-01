using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003D7 RID: 983
	public class SelectionBoxObjectSelectionRenderer : ObjectSelectionRenderer
	{
		// Token: 0x06002DEF RID: 11759 RVA: 0x0013DA32 File Offset: 0x0013BC32
		public override void RenderObjectSelection(HashSet<GameObject> selectedObjects, ObjectSelectionSettings objectSelectionSettings)
		{
			ObjectSelectionBoxRendererFactory.CreateObjectSelectionBoxDrawer(objectSelectionSettings.ObjectSelectionBoxRenderSettings.SelectionBoxStyle).RenderObjectSelectionBoxes(selectedObjects);
		}
	}
}
