using System;
using NewNet;

// Token: 0x020002B4 RID: 692
public class UICustomImage : UICustomObject<UICustomImage>
{
	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06002252 RID: 8786 RVA: 0x000F65AF File Offset: 0x000F47AF
	// (set) Token: 0x06002253 RID: 8787 RVA: 0x000F65BC File Offset: 0x000F47BC
	private string CustomImageURL
	{
		get
		{
			return this.ImageInput.value;
		}
		set
		{
			this.ImageInput.value = value;
		}
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000F65CC File Offset: 0x000F47CC
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomImage = this.TargetCustomObject.GetComponent<CustomImage>();
		if (!this.TargetCustomImage)
		{
			return;
		}
		Language.UpdateUILabel(this.HeaderLabel, string.Format("Custom {0}", this.TargetCustomImage.ObjectName).ToUpper());
		this.ImageInput.SelectAllTextOnClick = true;
		this.CustomImageURL = this.TargetCustomImage.CustomImageURL;
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000F6640 File Offset: 0x000F4840
	public override void Import()
	{
		this.CustomImageURL = this.CustomImageURL.Trim();
		if (string.IsNullOrEmpty(this.CustomImageURL))
		{
			Chat.LogError("You must supply a custom image URL.", true);
			return;
		}
		if (Network.isServer)
		{
			base.CheckUpdateMatchingCustomObjects();
			this.TargetCustomImage.CustomImageURL = this.CustomImageURL;
			this.TargetCustomImage.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x040015B0 RID: 5552
	private CustomImage TargetCustomImage;

	// Token: 0x040015B1 RID: 5553
	public UILabel HeaderLabel;

	// Token: 0x040015B2 RID: 5554
	public UIInput ImageInput;
}
