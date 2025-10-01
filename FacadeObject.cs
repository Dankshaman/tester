using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class FacadeObject : MonoBehaviour
{
	// Token: 0x06000DDB RID: 3547 RVA: 0x000594C0 File Offset: 0x000576C0
	private void Start()
	{
		this.ThisRender = base.GetComponent<Renderer>();
		base.transform.localScale = Vector3.one;
		this.ParentStackObject = base.transform.parent.gameObject;
		this.ParentRender = this.ParentStackObject.GetComponent<Renderer>();
		base.GetComponent<Renderer>().materials = this.ParentStackObject.GetComponent<Renderer>().sharedMaterials;
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x0005952B File Offset: 0x0005772B
	private void Update()
	{
		if (!this.ParentStackObject)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.ThisRender.enabled = this.ParentRender.enabled;
	}

	// Token: 0x040008FB RID: 2299
	private GameObject ParentStackObject;

	// Token: 0x040008FC RID: 2300
	private Renderer ThisRender;

	// Token: 0x040008FD RID: 2301
	private Renderer ParentRender;
}
