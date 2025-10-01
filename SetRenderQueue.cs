using System;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class SetRenderQueue : MonoBehaviour
{
	// Token: 0x06002129 RID: 8489 RVA: 0x000EFB24 File Offset: 0x000EDD24
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component == null)
		{
			ParticleSystem component2 = base.GetComponent<ParticleSystem>();
			if (component2 != null)
			{
				component = component2.GetComponent<Renderer>();
			}
		}
		if (component != null)
		{
			this.mMat = new Material(component.sharedMaterial);
			for (int i = 0; i < component.materials.Length; i++)
			{
				component.materials[i].renderQueue = this.renderQueue;
			}
			this.mMat.renderQueue = this.renderQueue;
			component.material = this.mMat;
		}
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x000EFBB5 File Offset: 0x000EDDB5
	private void OnDestroy()
	{
		if (this.mMat != null)
		{
			UnityEngine.Object.Destroy(this.mMat);
		}
	}

	// Token: 0x04001477 RID: 5239
	public int renderQueue = 4000;

	// Token: 0x04001478 RID: 5240
	private Material mMat;
}
