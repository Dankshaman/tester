using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003D1 RID: 977
	public class CornerLinesObjectSelectionBoxRenderer : ObjectSelectionBoxRenderer
	{
		// Token: 0x06002DE3 RID: 11747 RVA: 0x0013D918 File Offset: 0x0013BB18
		public override void RenderObjectSelectionBoxes(HashSet<GameObject> selectedObjects)
		{
			EditorObjectSelection instance = MonoSingletonBase<EditorObjectSelection>.Instance;
			Material glline = SingletonBase<MaterialPool>.Instance.GLLine;
			ObjectSelectionBoxRenderSettings objectSelectionBoxRenderSettings = instance.ObjectSelectionSettings.ObjectSelectionBoxRenderSettings;
			if (objectSelectionBoxRenderSettings.SelectionBoxRenderMode == ObjectSelectionBoxRenderMode.PerObject)
			{
				GLPrimitives.DrawCornerLinesForSelectionBoxes(ObjectSelectionBoxCalculator.CalculatePerObject(selectedObjects), objectSelectionBoxRenderSettings.BoxSizeAdd, objectSelectionBoxRenderSettings.SelectionBoxCornerLinePercentage, MonoSingletonBase<EditorCamera>.Instance.Camera, objectSelectionBoxRenderSettings.SelectionBoxLineColor, glline);
				return;
			}
			if (objectSelectionBoxRenderSettings.SelectionBoxRenderMode == ObjectSelectionBoxRenderMode.FromParentToBottom)
			{
				GLPrimitives.DrawCornerLinesForSelectionBoxes(ObjectSelectionBoxCalculator.CalculateFromParentsToBottom(selectedObjects), objectSelectionBoxRenderSettings.BoxSizeAdd, objectSelectionBoxRenderSettings.SelectionBoxCornerLinePercentage, MonoSingletonBase<EditorCamera>.Instance.Camera, objectSelectionBoxRenderSettings.SelectionBoxLineColor, glline);
			}
		}
	}
}
