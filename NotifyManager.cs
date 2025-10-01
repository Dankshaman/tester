using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class NotifyManager : MonoBehaviour
{
	// Token: 0x06001848 RID: 6216 RVA: 0x000A53B8 File Offset: 0x000A35B8
	private void Awake()
	{
		List<GameObject> sceneRootGameObjects = Utilities.GetSceneRootGameObjects();
		for (int i = 0; i < sceneRootGameObjects.Count; i++)
		{
			sceneRootGameObjects[i].GetComponentsInChildren<INotifySceneAwake>(true, NotifyManager.notifyAwake);
			for (int j = 0; j < NotifyManager.notifyAwake.Count; j++)
			{
				NotifyManager.notifyAwake[j].SceneAwake();
			}
		}
		NotifyManager.notifyAwake.Clear();
		sceneRootGameObjects.Clear();
	}

	// Token: 0x04000E83 RID: 3715
	private static List<INotifySceneAwake> notifyAwake = new List<INotifySceneAwake>();
}
