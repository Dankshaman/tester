using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class FixRenderQueues : MonoBehaviour
{
	// Token: 0x06000DF0 RID: 3568 RVA: 0x00059AC4 File Offset: 0x00057CC4
	private void Awake()
	{
		Renderer component = base.GetComponent<Renderer>();
		for (int i = 0; i < component.materials.Length; i++)
		{
			component.materials[i].renderQueue = component.materials[i].shader.renderQueue + i;
		}
	}
}
