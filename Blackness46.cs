using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class Blackness46 : MonoBehaviour
{
	// Token: 0x0600083E RID: 2110 RVA: 0x0003A4C0 File Offset: 0x000386C0
	private void Start()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
			{
				componentsInChildren[i].materials[j].SetVector("_UniformOcclusion", Vector4.zero);
			}
		}
	}
}
