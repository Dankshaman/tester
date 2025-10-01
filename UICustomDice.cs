using System;
using NewNet;

// Token: 0x020002B3 RID: 691
public class UICustomDice : UICustomObject<UICustomDice>
{
	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x0600224B RID: 8779 RVA: 0x000F6424 File Offset: 0x000F4624
	// (set) Token: 0x0600224C RID: 8780 RVA: 0x000F6431 File Offset: 0x000F4631
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

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x0600224D RID: 8781 RVA: 0x000F6440 File Offset: 0x000F4640
	// (set) Token: 0x0600224E RID: 8782 RVA: 0x000F6474 File Offset: 0x000F4674
	private int TypeInt
	{
		get
		{
			for (int i = 0; i < this.DiceTypeToggles.Length; i++)
			{
				if (this.DiceTypeToggles[i].value)
				{
					return i;
				}
			}
			return 0;
		}
		set
		{
			int group = this.DiceTypeToggles[0].group;
			for (int i = 0; i < this.DiceTypeToggles.Length; i++)
			{
				this.DiceTypeToggles[i].group = 0;
				this.DiceTypeToggles[i].value = (i == value);
				this.DiceTypeToggles[i].group = group;
			}
		}
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x000F64D0 File Offset: 0x000F46D0
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomDice = this.TargetCustomObject.GetComponent<CustomDice>();
		if (!this.TargetCustomDice)
		{
			return;
		}
		this.ImageInput.SelectAllTextOnClick = true;
		this.CustomImageURL = this.TargetCustomDice.CustomImageURL;
		this.TypeInt = (int)this.TargetCustomDice.CurrentDiceType;
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000F6530 File Offset: 0x000F4730
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
			this.TargetCustomDice.CustomImageURL = this.CustomImageURL;
			this.TargetCustomDice.CurrentDiceType = (DiceType)this.TypeInt;
			this.TargetCustomDice.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x040015AD RID: 5549
	private CustomDice TargetCustomDice;

	// Token: 0x040015AE RID: 5550
	public UIToggle[] DiceTypeToggles;

	// Token: 0x040015AF RID: 5551
	public UIInput ImageInput;
}
