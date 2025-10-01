using System;

// Token: 0x020002AB RID: 683
public class UIContextualRotate : UIPressedButtonCall
{
	// Token: 0x06002219 RID: 8729 RVA: 0x000F5808 File Offset: 0x000F3A08
	private void Start()
	{
		base.AddPressAction(this.RightButton.gameObject, new Action(NetworkSingleton<NetworkUI>.Instance.GUIRotateRightContextual), 0.15f, 0f);
		base.AddPressAction(this.LeftButton.gameObject, new Action(NetworkSingleton<NetworkUI>.Instance.GUIRotateLeftContextual), 0.15f, 0f);
	}

	// Token: 0x0400158E RID: 5518
	public UIButton RightButton;

	// Token: 0x0400158F RID: 5519
	public UIButton LeftButton;
}
