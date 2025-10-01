using System;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class UITagRow : MonoBehaviour
{
	// Token: 0x060027F2 RID: 10226 RVA: 0x0011A8B5 File Offset: 0x00118AB5
	private void Awake()
	{
		this.button = base.transform.GetChild(0).GetComponent<UIButton>();
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x060027F3 RID: 10227 RVA: 0x0011A8CE File Offset: 0x00118ACE
	// (set) Token: 0x060027F4 RID: 10228 RVA: 0x0011A8DB File Offset: 0x00118ADB
	public bool isSelected
	{
		get
		{
			return base.GetComponent<UISprite>().enabled;
		}
		set
		{
			base.GetComponent<UISprite>().enabled = value;
		}
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x0011A8E9 File Offset: 0x00118AE9
	public void OnClick()
	{
		if (!this.isSelected)
		{
			this.tagEditor.Select(this.tagIndex);
			return;
		}
		this.tagEditor.Deselect();
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x0011A910 File Offset: 0x00118B10
	private void Update()
	{
		if (this.button.state == UIButtonColor.State.Hover)
		{
			if (!this.wasHovered)
			{
				this.tagEditor.HoverTag(this.tagIndex);
				this.wasHovered = true;
				return;
			}
		}
		else if (this.wasHovered)
		{
			this.tagEditor.UnHoverTag(this.tagIndex);
			this.wasHovered = false;
		}
	}

	// Token: 0x04001A38 RID: 6712
	public UITagEditor tagEditor;

	// Token: 0x04001A39 RID: 6713
	public TagLabel tagLabel;

	// Token: 0x04001A3A RID: 6714
	public int tagIndex;

	// Token: 0x04001A3B RID: 6715
	private UIButton button;

	// Token: 0x04001A3C RID: 6716
	private bool wasHovered;
}
