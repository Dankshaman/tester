using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class DummyObject : MonoBehaviour
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x00056D79 File Offset: 0x00054F79
	private void OnDestroy()
	{
		EventManager.TriggerDummyObjectDestroy(this);
	}
}
