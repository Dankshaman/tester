using System;
using System.Collections.Generic;
using NewNet;

// Token: 0x020002B0 RID: 688
public class UICustomCard : UICustomObject<UICustomCard>
{
	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x0600222F RID: 8751 RVA: 0x000F5C46 File Offset: 0x000F3E46
	// (set) Token: 0x06002230 RID: 8752 RVA: 0x000F5C53 File Offset: 0x000F3E53
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

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06002231 RID: 8753 RVA: 0x000F5C61 File Offset: 0x000F3E61
	// (set) Token: 0x06002232 RID: 8754 RVA: 0x000F5C6E File Offset: 0x000F3E6E
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

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06002233 RID: 8755 RVA: 0x000F5C7C File Offset: 0x000F3E7C
	// (set) Token: 0x06002234 RID: 8756 RVA: 0x000F5C89 File Offset: 0x000F3E89
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

	// Token: 0x06002235 RID: 8757 RVA: 0x000F5C98 File Offset: 0x000F3E98
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomCard = this.TargetCustomObject.GetComponent<CustomCard>();
		if (!this.TargetCustomCard)
		{
			return;
		}
		CardScript component = this.TargetCustomCard.GetComponent<CardScript>();
		List<CustomDeckData> customDeckDatas = component.GetCustomDeckDatas();
		if (customDeckDatas.Count > 0)
		{
			CustomDeckData customDeckData = customDeckDatas[0];
			this.TargetCustomCard.Type = customDeckData.Type;
			this.TargetCustomCard.URLFace = customDeckData.FaceURL;
			this.TargetCustomCard.URLBack = customDeckData.BackURL;
			this.TargetCustomCard.bSideways = component.bSideways;
		}
		this.TypePopupList.items = CardManagerScript.CardTypeNames;
		this.TypePopupList.value = this.TypePopupList.items[(int)this.TargetCustomCard.Type];
		this.URLFace = this.TargetCustomCard.URLFace;
		this.URLBack = this.TargetCustomCard.URLBack;
		this.bSideways = this.TargetCustomCard.bSideways;
		this.FaceInput.SelectAllTextOnClick = true;
		this.BackInput.SelectAllTextOnClick = true;
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000F5DB4 File Offset: 0x000F3FB4
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
			this.TargetCustomCard.Type = (CardType)this.TypePopupList.items.IndexOf(this.TypePopupList.value);
			this.TargetCustomCard.URLFace = this.URLFace;
			this.TargetCustomCard.URLBack = this.URLBack;
			this.TargetCustomCard.bSideways = this.bSideways;
			this.TargetCustomObject.GetComponent<NetworkPhysicsObject>().IsHiddenWhenFaceDown = true;
			NetworkSingleton<CardManagerScript>.Instance.SetupCustomCard(this.TargetCustomCard.gameObject.GetComponent<CardScript>(), new CustomDeckData(this.URLFace, this.URLBack, 1, 1, true, false, this.TargetCustomCard.Type), this.bSideways);
		}
		base.Close();
	}

	// Token: 0x04001599 RID: 5529
	private CustomCard TargetCustomCard;

	// Token: 0x0400159A RID: 5530
	public UIPopupList TypePopupList;

	// Token: 0x0400159B RID: 5531
	public UIInput FaceInput;

	// Token: 0x0400159C RID: 5532
	public UIInput BackInput;

	// Token: 0x0400159D RID: 5533
	public UIToggle SidewaysToggle;
}
