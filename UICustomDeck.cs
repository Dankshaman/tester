using System;
using System.Collections.Generic;
using NewNet;

// Token: 0x020002B2 RID: 690
public class UICustomDeck : UICustomObject<UICustomDeck>
{
	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x0600223E RID: 8766 RVA: 0x000F6060 File Offset: 0x000F4260
	// (set) Token: 0x0600223F RID: 8767 RVA: 0x000F606D File Offset: 0x000F426D
	private string URLFace
	{
		get
		{
			return this.FaceInput.value;
		}
		set
		{
			this.FaceInput.value = value;
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06002240 RID: 8768 RVA: 0x000F607B File Offset: 0x000F427B
	// (set) Token: 0x06002241 RID: 8769 RVA: 0x000F6088 File Offset: 0x000F4288
	private bool bUniqueBacks
	{
		get
		{
			return this.UniqueBackToggle.value;
		}
		set
		{
			this.UniqueBackToggle.value = value;
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06002242 RID: 8770 RVA: 0x000F6096 File Offset: 0x000F4296
	// (set) Token: 0x06002243 RID: 8771 RVA: 0x000F60A3 File Offset: 0x000F42A3
	private string URLBack
	{
		get
		{
			return this.BackInput.value;
		}
		set
		{
			this.BackInput.value = value;
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06002244 RID: 8772 RVA: 0x000F60B1 File Offset: 0x000F42B1
	// (set) Token: 0x06002245 RID: 8773 RVA: 0x000F60BE File Offset: 0x000F42BE
	private bool bSideways
	{
		get
		{
			return this.SidewaysToggle.value;
		}
		set
		{
			this.SidewaysToggle.value = value;
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06002246 RID: 8774 RVA: 0x000F60CC File Offset: 0x000F42CC
	// (set) Token: 0x06002247 RID: 8775 RVA: 0x000F60D9 File Offset: 0x000F42D9
	private bool bBackIsHidden
	{
		get
		{
			return this.BackIsHiddenToggle.value;
		}
		set
		{
			this.BackIsHiddenToggle.value = value;
		}
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000F60E8 File Offset: 0x000F42E8
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomDeck = this.TargetCustomObject.GetComponent<CustomDeck>();
		if (!this.TargetCustomDeck)
		{
			return;
		}
		DeckScript component = this.TargetCustomDeck.GetComponent<DeckScript>();
		List<CustomDeckData> customDeckDatas = component.GetCustomDeckDatas();
		if (customDeckDatas.Count > 0)
		{
			CustomDeckData customDeckData = customDeckDatas[0];
			this.TargetCustomDeck.Type = customDeckData.Type;
			this.TargetCustomDeck.URLFace = customDeckData.FaceURL;
			this.TargetCustomDeck.bUniqueBacks = customDeckData.UniqueBack;
			this.TargetCustomDeck.URLBack = customDeckData.BackURL;
			this.TargetCustomDeck.bSideways = component.bSideways;
			this.TargetCustomDeck.bBackIsHidden = customDeckData.BackIsHidden;
			this.TargetCustomDeck.NumberCards = component.num_cards_;
			this.NumberRange.floatValue = (float)this.TargetCustomDeck.NumberCards;
			this.WidthRange.floatValue = (float)customDeckData.NumWidth;
			this.HeightRange.floatValue = (float)customDeckData.NumHeight;
		}
		this.TypePopupList.items = CardManagerScript.CardTypeNames;
		this.TypePopupList.value = this.TypePopupList.items[(int)this.TargetCustomDeck.Type];
		this.URLFace = this.TargetCustomDeck.URLFace;
		this.bUniqueBacks = this.TargetCustomDeck.bUniqueBacks;
		this.URLBack = this.TargetCustomDeck.URLBack;
		this.bSideways = this.TargetCustomDeck.bSideways;
		this.bBackIsHidden = this.TargetCustomDeck.bBackIsHidden;
		this.FaceInput.SelectAllTextOnClick = true;
		this.BackInput.SelectAllTextOnClick = true;
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000F6298 File Offset: 0x000F4498
	public override void Import()
	{
		this.URLFace = this.URLFace.Trim();
		this.URLBack = this.URLBack.Trim();
		if (string.IsNullOrEmpty(this.URLFace))
		{
			Chat.LogError("You must supply a face image URL.", true);
			return;
		}
		if (string.IsNullOrEmpty(this.URLBack))
		{
			Chat.LogError("You must supply a back image URL.", true);
			return;
		}
		if (Network.isServer)
		{
			this.TargetCustomDeck.Type = (CardType)this.TypePopupList.items.IndexOf(this.TypePopupList.value);
			this.TargetCustomDeck.URLFace = this.URLFace;
			this.TargetCustomDeck.bUniqueBacks = this.bUniqueBacks;
			this.TargetCustomDeck.URLBack = this.URLBack;
			this.TargetCustomDeck.bSideways = this.bSideways;
			this.TargetCustomDeck.bBackIsHidden = this.bBackIsHidden;
			this.TargetCustomDeck.NumberCards = this.NumberRange.intValue;
			this.TargetCustomObject.GetComponent<NetworkPhysicsObject>().IsHiddenWhenFaceDown = !this.bUniqueBacks;
			NetworkSingleton<CardManagerScript>.Instance.SetupCustomDeck(this.TargetCustomDeck.gameObject.GetComponent<DeckScript>(), new CustomDeckData(this.URLFace, this.URLBack, this.WidthRange.intValue, this.HeightRange.intValue, this.bBackIsHidden, this.bUniqueBacks, this.TargetCustomDeck.Type), this.NumberRange.intValue, this.bSideways);
		}
		base.Close();
	}

	// Token: 0x040015A3 RID: 5539
	private CustomDeck TargetCustomDeck;

	// Token: 0x040015A4 RID: 5540
	public UIPopupList TypePopupList;

	// Token: 0x040015A5 RID: 5541
	public UIInput FaceInput;

	// Token: 0x040015A6 RID: 5542
	public UIToggle UniqueBackToggle;

	// Token: 0x040015A7 RID: 5543
	public UIInput BackInput;

	// Token: 0x040015A8 RID: 5544
	public UISliderRange NumberRange;

	// Token: 0x040015A9 RID: 5545
	public UIToggle SidewaysToggle;

	// Token: 0x040015AA RID: 5546
	public UIToggle BackIsHiddenToggle;

	// Token: 0x040015AB RID: 5547
	public UISliderRange WidthRange;

	// Token: 0x040015AC RID: 5548
	public UISliderRange HeightRange;
}
