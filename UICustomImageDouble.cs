using System;
using NewNet;

// Token: 0x020002B5 RID: 693
public class UICustomImageDouble : UICustomObject<UICustomImageDouble>
{
	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06002257 RID: 8791 RVA: 0x000F66AE File Offset: 0x000F48AE
	// (set) Token: 0x06002258 RID: 8792 RVA: 0x000F66BB File Offset: 0x000F48BB
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

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x06002259 RID: 8793 RVA: 0x000F66C9 File Offset: 0x000F48C9
	// (set) Token: 0x0600225A RID: 8794 RVA: 0x000F66D6 File Offset: 0x000F48D6
	private string CustomImageSecondaryURL
	{
		get
		{
			return this.ImageInput2.value;
		}
		set
		{
			this.ImageInput2.value = value;
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x0600225B RID: 8795 RVA: 0x000F66E4 File Offset: 0x000F48E4
	// (set) Token: 0x0600225C RID: 8796 RVA: 0x000F670C File Offset: 0x000F490C
	private float CustomImageScalar
	{
		get
		{
			float result;
			if (float.TryParse(this.CustomImageScalarInput.value, out result))
			{
				return result;
			}
			return 1f;
		}
		set
		{
			this.CustomImageScalarInput.value = value.ToString();
		}
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x000F6720 File Offset: 0x000F4920
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomImage = this.TargetCustomObject.GetComponent<CustomImage>();
		if (!this.TargetCustomImage)
		{
			return;
		}
		this.HeaderLabel.text = string.Format("Custom {0}", this.TargetCustomImage.ObjectName).ToUpper();
		this.ImageInput.SelectAllTextOnClick = true;
		this.ImageInput2.SelectAllTextOnClick = true;
		this.CustomImageScalarInput.SelectAllTextOnClick = true;
		this.CustomImageURL = this.TargetCustomImage.CustomImageURL;
		this.CustomImageSecondaryURL = this.TargetCustomImage.CustomImageSecondaryURL;
		this.CustomImageScalar = this.TargetCustomImage.CardScalar;
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000F67D0 File Offset: 0x000F49D0
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
			this.TargetCustomImage.CustomImageSecondaryURL = this.CustomImageSecondaryURL;
			this.TargetCustomImage.CardScalar = this.CustomImageScalar;
			this.TargetCustomImage.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x040015B3 RID: 5555
	private CustomImage TargetCustomImage;

	// Token: 0x040015B4 RID: 5556
	public UILabel HeaderLabel;

	// Token: 0x040015B5 RID: 5557
	public UIInput ImageInput;

	// Token: 0x040015B6 RID: 5558
	public UIInput ImageInput2;

	// Token: 0x040015B7 RID: 5559
	public UIInput CustomImageScalarInput;
}
