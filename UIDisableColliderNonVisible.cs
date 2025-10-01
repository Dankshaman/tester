using System;
using UnityEngine;

// Token: 0x020002C2 RID: 706
public class UIDisableColliderNonVisible : MonoBehaviour
{
	// Token: 0x060022EC RID: 8940 RVA: 0x000F94AB File Offset: 0x000F76AB
	private void Start()
	{
		this.ThisBoxCollider = base.GetComponent<BoxCollider2D>();
		this.ThisUIWidget = base.GetComponent<UIWidget>();
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000F94C5 File Offset: 0x000F76C5
	private void Update()
	{
		this.ThisBoxCollider.enabled = this.ThisUIWidget.isVisible;
	}

	// Token: 0x04001627 RID: 5671
	private BoxCollider2D ThisBoxCollider;

	// Token: 0x04001628 RID: 5672
	private UIWidget ThisUIWidget;
}
