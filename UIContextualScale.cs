using System;

// Token: 0x020002AC RID: 684
public class UIContextualScale : UIPressedButtonCall
{
	// Token: 0x0600221B RID: 8731 RVA: 0x000F5874 File Offset: 0x000F3A74
	private void Start()
	{
		base.AddPressAction(this.ScaleUpButton.gameObject, new Action(this.ScaleUp), 0.2f, 0.025f);
		base.AddPressAction(this.ScaleDownButton.gameObject, new Action(this.ScaleDown), 0.2f, 0.025f);
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000025B8 File Offset: 0x000007B8
	private void OnDestroy()
	{
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000F58CF File Offset: 0x000F3ACF
	private void ScaleUp()
	{
		if (PlayerScript.Pointer)
		{
			PlayerScript.PointerScript.Scale(-1, true);
		}
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000F58E9 File Offset: 0x000F3AE9
	private void ScaleDown()
	{
		if (PlayerScript.Pointer)
		{
			PlayerScript.PointerScript.Scale(-1, false);
		}
	}

	// Token: 0x04001590 RID: 5520
	public UIButton ScaleUpButton;

	// Token: 0x04001591 RID: 5521
	public UIButton ScaleDownButton;
}
