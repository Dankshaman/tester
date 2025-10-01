using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E1 RID: 993
	public static class ObjectSelectionBoxCalculator
	{
		// Token: 0x06002E0A RID: 11786 RVA: 0x0013DCFC File Offset: 0x0013BEFC
		public static List<ObjectSelectionBox> CalculatePerObject(IEnumerable<GameObject> selectedObjects)
		{
			List<ObjectSelectionBox> list = new List<ObjectSelectionBox>(20);
			foreach (GameObject gameObject in selectedObjects)
			{
				Box modelSpaceBox = gameObject.GetModelSpaceBox();
				if (modelSpaceBox.IsValid())
				{
					list.Add(new ObjectSelectionBox(modelSpaceBox, gameObject.transform.localToWorldMatrix));
				}
			}
			return list;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x0013DD70 File Offset: 0x0013BF70
		public static List<ObjectSelectionBox> CalculateFromParentsToBottom(IEnumerable<GameObject> selectedObjects)
		{
			List<GameObject> parentsFromObjectCollection = GameObjectExtensions.GetParentsFromObjectCollection(selectedObjects);
			List<ObjectSelectionBox> list = new List<ObjectSelectionBox>(20);
			foreach (GameObject gameObject in parentsFromObjectCollection)
			{
				Box hierarchyModelSpaceBox = gameObject.GetHierarchyModelSpaceBox();
				if (hierarchyModelSpaceBox.IsValid())
				{
					list.Add(new ObjectSelectionBox(hierarchyModelSpaceBox, gameObject.transform.localToWorldMatrix));
				}
			}
			return list;
		}
	}
}
