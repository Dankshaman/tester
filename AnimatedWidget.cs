using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
[ExecuteInEditMode]
public class AnimatedWidget : MonoBehaviour
{
	// Token: 0x0600050B RID: 1291 RVA: 0x00024E74 File Offset: 0x00023074
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00024E88 File Offset: 0x00023088
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.width = Mathf.RoundToInt(this.width);
			this.mWidget.height = Mathf.RoundToInt(this.height);
		}
	}

	// Token: 0x04000382 RID: 898
	public float width = 1f;

	// Token: 0x04000383 RID: 899
	public float height = 1f;

	// Token: 0x04000384 RID: 900
	private UIWidget mWidget;
}
