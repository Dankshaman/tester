using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour
{
	// Token: 0x06000505 RID: 1285 RVA: 0x00024DC7 File Offset: 0x00022FC7
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.mPanel = base.GetComponent<UIPanel>();
		this.LateUpdate();
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00024DE7 File Offset: 0x00022FE7
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.alpha = this.alpha;
		}
		if (this.mPanel != null)
		{
			this.mPanel.alpha = this.alpha;
		}
	}

	// Token: 0x0400037D RID: 893
	[Range(0f, 1f)]
	public float alpha = 1f;

	// Token: 0x0400037E RID: 894
	private UIWidget mWidget;

	// Token: 0x0400037F RID: 895
	private UIPanel mPanel;
}
