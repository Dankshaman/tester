using System;
using UnityEngine;

// Token: 0x020002D2 RID: 722
public class UIGameManagerItem : MonoBehaviour
{
	// Token: 0x0600233B RID: 9019 RVA: 0x000FA394 File Offset: 0x000F8594
	private void Start()
	{
		UIEventListener uieventListener = UIEventListener.Get(base.gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnClick));
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000FA3C2 File Offset: 0x000F85C2
	private void OnDestroy()
	{
		UIEventListener uieventListener = UIEventListener.Get(base.gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnClick));
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000FA3F0 File Offset: 0x000F85F0
	public void Init(Texture texture, string name)
	{
		this.uiTexture.mainTexture = texture;
		this.label.text = name;
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000FA40A File Offset: 0x000F860A
	public void OnClick(GameObject go)
	{
		if (this.onClick != null)
		{
			this.onClick();
		}
	}

	// Token: 0x04001658 RID: 5720
	public UITexture uiTexture;

	// Token: 0x04001659 RID: 5721
	public UILabel label;

	// Token: 0x0400165A RID: 5722
	public Action onClick;
}
