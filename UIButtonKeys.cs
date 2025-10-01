using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)")]
public class UIButtonKeys : UIKeyNavigation
{
	// Token: 0x060000EC RID: 236 RVA: 0x000065DF File Offset: 0x000047DF
	protected override void OnEnable()
	{
		this.Upgrade();
		base.OnEnable();
	}

	// Token: 0x060000ED RID: 237 RVA: 0x000065F0 File Offset: 0x000047F0
	public void Upgrade()
	{
		if (this.onClick == null && this.selectOnClick != null)
		{
			this.onClick = this.selectOnClick.gameObject;
			this.selectOnClick = null;
			NGUITools.SetDirty(this);
		}
		if (this.onLeft == null && this.selectOnLeft != null)
		{
			this.onLeft = this.selectOnLeft.gameObject;
			this.selectOnLeft = null;
			NGUITools.SetDirty(this);
		}
		if (this.onRight == null && this.selectOnRight != null)
		{
			this.onRight = this.selectOnRight.gameObject;
			this.selectOnRight = null;
			NGUITools.SetDirty(this);
		}
		if (this.onUp == null && this.selectOnUp != null)
		{
			this.onUp = this.selectOnUp.gameObject;
			this.selectOnUp = null;
			NGUITools.SetDirty(this);
		}
		if (this.onDown == null && this.selectOnDown != null)
		{
			this.onDown = this.selectOnDown.gameObject;
			this.selectOnDown = null;
			NGUITools.SetDirty(this);
		}
	}

	// Token: 0x040000C1 RID: 193
	public UIButtonKeys selectOnClick;

	// Token: 0x040000C2 RID: 194
	public UIButtonKeys selectOnUp;

	// Token: 0x040000C3 RID: 195
	public UIButtonKeys selectOnDown;

	// Token: 0x040000C4 RID: 196
	public UIButtonKeys selectOnLeft;

	// Token: 0x040000C5 RID: 197
	public UIButtonKeys selectOnRight;
}
