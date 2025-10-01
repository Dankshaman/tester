using System;
using UnityEngine;

// Token: 0x02000352 RID: 850
public class UITooltipScript : MonoBehaviour
{
	// Token: 0x0600283B RID: 10299 RVA: 0x0011C1E8 File Offset: 0x0011A3E8
	private void Start()
	{
		if (this.Tooltip == "" && this.DelayTooltip == "" && this.DeleteIfEmpty)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (this.Tooltip == null)
		{
			return;
		}
		if (this.QuestionMark && UITooltipScript.SHOW_QUESTION_MARK)
		{
			UILabel componentInChildren = base.GetComponentInChildren<UILabel>();
			if (componentInChildren)
			{
				UILabel uilabel = componentInChildren;
				uilabel.text += " [i][sup](?)[/sup][/i]";
			}
		}
		if (this.Tooltip.Contains("<Click to open page to knowledge base.>"))
		{
			if (this.Tooltip.EndsWith(" <Click to open page to knowledge base.>"))
			{
				this.Tooltip = this.Tooltip.Replace(" <Click to open page to knowledge base.>", "\n<Click to open page to knowledge base.>");
			}
			Colour colour = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.TooltipMotif];
			colour.a = 1f;
			this.currentMotifTag = colour.RGBHex;
			this.Tooltip = this.Tooltip.Replace("<Click to open page to knowledge base.>", this.currentMotifTag + "<Click to open page to knowledge base.>");
			EventManager.OnUIThemeChange += this.OnThemeChange;
		}
		switch (this.AddFileType)
		{
		case FileType.All:
			this.Tooltip = this.Tooltip + " (" + CustomLoadingManager.GetSupportedAllFormatExtensionsString() + ")";
			return;
		case FileType.Image:
			this.Tooltip = this.Tooltip + " (" + CustomLoadingManager.GetSupportedImageFormatExtensionsString() + ")";
			return;
		case FileType.Model:
			this.Tooltip = this.Tooltip + " (" + CustomLoadingManager.GetSupportedMeshFormatExtensionsString() + ")";
			return;
		case FileType.AssetBundle:
			this.Tooltip = this.Tooltip + " (" + CustomLoadingManager.GetSupportedAssetBundleFormatExtensionsString() + ")";
			return;
		case FileType.Audio:
			this.Tooltip = this.Tooltip + " (" + CustomLoadingManager.GetSupportedAudioFormatExtensionsString() + ")";
			return;
		case FileType.PDF:
			this.Tooltip = this.Tooltip + " (" + CustomLoadingManager.GetSupportedPDFFormatExtensionsString() + ")";
			return;
		default:
			return;
		}
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x0011C3F6 File Offset: 0x0011A5F6
	private void OnScroll(float delta)
	{
		UITooltipScript.ScrollHandlerFunction scrollHandler = this.ScrollHandler;
		if (scrollHandler == null)
		{
			return;
		}
		scrollHandler(delta);
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x0011C40C File Offset: 0x0011A60C
	private void OnHover(bool Enter)
	{
		if (!base.enabled)
		{
			return;
		}
		this.translatedTooltip = this.Tooltip;
		if (!string.IsNullOrEmpty(this.translatedTooltip))
		{
			TextCode.LocalizeUIText(ref this.translatedTooltip);
		}
		this.translatedDelayTooltip = this.DelayTooltip;
		if (!string.IsNullOrEmpty(this.translatedDelayTooltip))
		{
			TextCode.LocalizeUIText(ref this.translatedDelayTooltip);
		}
		if (Enter)
		{
			UIHoverText.text = this.translatedTooltip;
			if (!string.IsNullOrEmpty(this.translatedDelayTooltip))
			{
				base.Invoke("InvokeDelayTooltip", UIHoverText.UIDelayTooltipTime);
				return;
			}
		}
		else
		{
			this.CancelTooltip();
		}
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x0011C4A0 File Offset: 0x0011A6A0
	private void InvokeDelayTooltip()
	{
		if (UIHoverText.text == this.translatedTooltip)
		{
			if (!string.IsNullOrEmpty(UIHoverText.text))
			{
				UIHoverText.text += "\n----------\n";
			}
			UIHoverText.text += this.translatedDelayTooltip;
		}
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x0011C4F4 File Offset: 0x0011A6F4
	private void OnPress()
	{
		this.CancelTooltip();
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x0011C4F4 File Offset: 0x0011A6F4
	private void OnClick()
	{
		this.CancelTooltip();
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x0011C4FC File Offset: 0x0011A6FC
	private void OnDestroy()
	{
		this.CancelTooltip();
		if (this.currentMotifTag != null)
		{
			EventManager.OnUIThemeChange -= this.OnThemeChange;
		}
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x0011C4F4 File Offset: 0x0011A6F4
	private void OnDisable()
	{
		this.CancelTooltip();
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x0011C520 File Offset: 0x0011A720
	private void CancelTooltip()
	{
		base.CancelInvoke("InvokeDelayTooltip");
		if (UIHoverText.text == this.translatedTooltip || UIHoverText.text == this.translatedDelayTooltip || UIHoverText.text == this.translatedTooltip + "\n----------\n" + this.translatedDelayTooltip)
		{
			UIHoverText.text = "";
		}
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x0011C588 File Offset: 0x0011A788
	private void OnThemeChange()
	{
		if (this.currentMotifTag != null)
		{
			Colour colour = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.TooltipMotif];
			colour.a = 1f;
			string rgbhex = colour.RGBHex;
			this.Tooltip = this.Tooltip.Replace(this.currentMotifTag, rgbhex);
			this.currentMotifTag = rgbhex;
		}
	}

	// Token: 0x04001A86 RID: 6790
	public static bool SHOW_QUESTION_MARK = true;

	// Token: 0x04001A87 RID: 6791
	[TextArea]
	public string Tooltip = "";

	// Token: 0x04001A88 RID: 6792
	[TextArea]
	public string DelayTooltip = "";

	// Token: 0x04001A89 RID: 6793
	public bool QuestionMark;

	// Token: 0x04001A8A RID: 6794
	public FileType AddFileType = FileType.None;

	// Token: 0x04001A8B RID: 6795
	public bool DeleteIfEmpty = true;

	// Token: 0x04001A8C RID: 6796
	public UITooltipScript.ScrollHandlerFunction ScrollHandler;

	// Token: 0x04001A8D RID: 6797
	private string currentMotifTag;

	// Token: 0x04001A8E RID: 6798
	private const string clickKnowledgebase = "<Click to open page to knowledge base.>";

	// Token: 0x04001A8F RID: 6799
	private string translatedTooltip;

	// Token: 0x04001A90 RID: 6800
	private string translatedDelayTooltip;

	// Token: 0x02000799 RID: 1945
	// (Invoke) Token: 0x06003F56 RID: 16214
	public delegate void ScrollHandlerFunction(float delta);
}
