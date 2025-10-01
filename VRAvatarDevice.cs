using System;
using UnityEngine;

// Token: 0x02000365 RID: 869
public class VRAvatarDevice : MonoBehaviour
{
	// Token: 0x06002911 RID: 10513 RVA: 0x00120EF4 File Offset: 0x0011F0F4
	public void SetAlpha(float a)
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material material = componentsInChildren[i].material;
			Colour colour = material.color;
			colour.a = a;
			material.color = colour;
		}
	}

	// Token: 0x04001AF1 RID: 6897
	public const float VISIBLE_ALPHA = 0.33f;

	// Token: 0x04001AF2 RID: 6898
	public bool IsController;
}
