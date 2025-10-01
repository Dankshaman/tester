using System;
using UnityEngine;

// Token: 0x02000301 RID: 769
public class UILongLabelFix : MonoBehaviour
{
	// Token: 0x0600251D RID: 9501 RVA: 0x00105C10 File Offset: 0x00103E10
	private void Awake()
	{
		this.label = base.GetComponent<UILabel>();
		if (this.label == null)
		{
			return;
		}
		Transform transform = base.transform;
		UIScrollView component = transform.GetComponent<UIScrollView>();
		while (transform != null && component == null)
		{
			transform = transform.parent;
			component = transform.GetComponent<UIScrollView>();
		}
		if (component)
		{
			if (component.verticalScrollBar)
			{
				this.bar = component.verticalScrollBar;
			}
			else
			{
				if (!component.horizontalScrollBar)
				{
					return;
				}
				this.bar = component.horizontalScrollBar;
			}
			EventDelegate.Add(this.bar.onChange, new EventDelegate.Callback(this.OnScrollBarChanged));
		}
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x00105CC4 File Offset: 0x00103EC4
	private void OnDestroy()
	{
		if (this.bar)
		{
			EventDelegate.Remove(this.bar.onChange, new EventDelegate.Callback(this.OnScrollBarChanged));
		}
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x00105CF0 File Offset: 0x00103EF0
	private void OnScrollBarChanged()
	{
		if (this.label.text.Length > 10000)
		{
			this.label.ProcessText(false, true);
		}
	}

	// Token: 0x0400181E RID: 6174
	private UIProgressBar bar;

	// Token: 0x0400181F RID: 6175
	private UILabel label;
}
