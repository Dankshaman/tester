using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Interaction/Envelop Content")]
public class EnvelopContent : MonoBehaviour
{
	// Token: 0x060000B8 RID: 184 RVA: 0x000053AF File Offset: 0x000035AF
	private void Start()
	{
		this.mStarted = true;
		this.Execute();
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000053BE File Offset: 0x000035BE
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.Execute();
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000053D0 File Offset: 0x000035D0
	[ContextMenu("Execute")]
	public void Execute()
	{
		if (this.targetRoot == base.transform)
		{
			Debug.LogError("Target Root object cannot be the same object that has Envelop Content. Make it a sibling instead.", this);
			return;
		}
		if (NGUITools.IsChild(this.targetRoot, base.transform))
		{
			Debug.LogError("Target Root object should not be a parent of Envelop Content. Make it a sibling instead.", this);
			return;
		}
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.transform.parent, this.targetRoot, !this.ignoreDisabled, true);
		float num = bounds.min.x + (float)this.padLeft;
		float num2 = bounds.min.y + (float)this.padBottom;
		float num3 = bounds.max.x + (float)this.padRight;
		float num4 = bounds.max.y + (float)this.padTop;
		base.GetComponent<UIWidget>().SetRect(num, num2, num3 - num, num4 - num2);
		base.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
		NGUITools.UpdateWidgetCollider(base.gameObject);
	}

	// Token: 0x04000086 RID: 134
	public Transform targetRoot;

	// Token: 0x04000087 RID: 135
	public int padLeft;

	// Token: 0x04000088 RID: 136
	public int padRight;

	// Token: 0x04000089 RID: 137
	public int padBottom;

	// Token: 0x0400008A RID: 138
	public int padTop;

	// Token: 0x0400008B RID: 139
	public bool ignoreDisabled = true;

	// Token: 0x0400008C RID: 140
	private bool mStarted;
}
