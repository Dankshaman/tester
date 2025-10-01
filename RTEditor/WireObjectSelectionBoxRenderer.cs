using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003D8 RID: 984
	public class WireObjectSelectionBoxRenderer : ObjectSelectionBoxRenderer
	{
		// Token: 0x06002DF1 RID: 11761 RVA: 0x0013DA54 File Offset: 0x0013BC54
		public override void RenderObjectSelectionBoxes(HashSet<GameObject> selectedObjects)
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			Material glline = SingletonBase<MaterialPool>.Instance.GLLine;
			ObjectSelectionBoxRenderSettings objectSelectionBoxRenderSettings = instance.ObjectSelectionSettings.ObjectSelectionBoxRenderSettings;
			if (objectSelectionBoxRenderSettings.SelectionBoxRenderMode == ObjectSelectionBoxRenderMode.PerObject)
			{
				GLPrimitives.DrawWireSelectionBoxes(ObjectSelectionBoxCalculator.CalculatePerObject(selectedObjects), objectSelectionBoxRenderSettings.BoxSizeAdd, MonoSingletonBase<EditorCamera>.Instance.Camera, objectSelectionBoxRenderSettings.SelectionBoxLineColor, glline);
				return;
			}
			if (objectSelectionBoxRenderSettings.SelectionBoxRenderMode == ObjectSelectionBoxRenderMode.FromParentToBottom)
			{
				GLPrimitives.DrawWireSelectionBoxes(ObjectSelectionBoxCalculator.CalculateFromParentsToBottom(selectedObjects), objectSelectionBoxRenderSettings.BoxSizeAdd, MonoSingletonBase<EditorCamera>.Instance.Camera, objectSelectionBoxRenderSettings.SelectionBoxLineColor, glline);
			}
		}
	}
}
