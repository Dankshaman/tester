using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
public class AnimatedColor : MonoBehaviour
{
	// Token: 0x06000508 RID: 1288 RVA: 0x00024E3A File Offset: 0x0002303A
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00024E4E File Offset: 0x0002304E
	private void LateUpdate()
	{
		this.mWidget.color = this.color;
	}

	// Token: 0x04000380 RID: 896
	public Color color = Color.white;

	// Token: 0x04000381 RID: 897
	private UIWidget mWidget;
}
