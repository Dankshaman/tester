using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class UIShowControlScheme : MonoBehaviour
{
	// Token: 0x0600027A RID: 634 RVA: 0x0001073B File Offset: 0x0000E93B
	private void OnEnable()
	{
		UICamera.onSchemeChange = (UICamera.OnSchemeChange)Delegate.Combine(UICamera.onSchemeChange, new UICamera.OnSchemeChange(this.OnScheme));
		this.OnScheme();
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00010763 File Offset: 0x0000E963
	private void OnDisable()
	{
		UICamera.onSchemeChange = (UICamera.OnSchemeChange)Delegate.Remove(UICamera.onSchemeChange, new UICamera.OnSchemeChange(this.OnScheme));
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00010788 File Offset: 0x0000E988
	private void OnScheme()
	{
		if (this.target != null)
		{
			UICamera.ControlScheme currentScheme = UICamera.currentScheme;
			if (currentScheme == UICamera.ControlScheme.Mouse)
			{
				this.target.SetActive(this.mouse);
				return;
			}
			if (currentScheme == UICamera.ControlScheme.Touch)
			{
				this.target.SetActive(this.touch);
				return;
			}
			if (currentScheme == UICamera.ControlScheme.Controller)
			{
				this.target.SetActive(this.controller);
			}
		}
	}

	// Token: 0x04000225 RID: 549
	public GameObject target;

	// Token: 0x04000226 RID: 550
	public bool mouse;

	// Token: 0x04000227 RID: 551
	public bool touch;

	// Token: 0x04000228 RID: 552
	public bool controller = true;
}
