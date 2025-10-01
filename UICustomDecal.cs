using System;

// Token: 0x020002B1 RID: 689
public class UICustomDecal : Singleton<UICustomDecal>
{
	// Token: 0x06002238 RID: 8760 RVA: 0x000F5ED7 File Offset: 0x000F40D7
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.ImportButton.onClick, new EventDelegate.Callback(this.Import));
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000F5EFC File Offset: 0x000F40FC
	private void OnDestroy()
	{
		EventDelegate.Remove(this.ImportButton.onClick, new EventDelegate.Callback(this.Import));
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000F5F1B File Offset: 0x000F411B
	private void OnDisable()
	{
		this.OverrideCustomDecalState = null;
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000F5F24 File Offset: 0x000F4124
	public void Override(CustomDecalState customDecalState)
	{
		this.OverrideCustomDecalState = customDecalState;
		this.NameInput.value = customDecalState.Name;
		this.URLInput.value = customDecalState.ImageURL;
		this.SizeInput.value = customDecalState.Size.ToString();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000F5F7C File Offset: 0x000F417C
	private void Import()
	{
		if (string.IsNullOrEmpty(this.NameInput.value))
		{
			Chat.LogError("No name provided.", true);
			return;
		}
		if (string.IsNullOrEmpty(this.URLInput.value))
		{
			Chat.LogError("No url provided.", true);
			return;
		}
		if (string.IsNullOrEmpty(this.SizeInput.value))
		{
			Chat.LogError("No size provided.", true);
			return;
		}
		CustomDecalState customDecalState = new CustomDecalState();
		customDecalState.Name = this.NameInput.value;
		customDecalState.ImageURL = this.URLInput.value;
		float size = float.Parse(this.SizeInput.value);
		customDecalState.Size = size;
		if (this.OverrideCustomDecalState != null)
		{
			NetworkSingleton<DecalManager>.Instance.RemoveDecalPallet(this.OverrideCustomDecalState);
		}
		NetworkSingleton<DecalManager>.Instance.AddDecalPallet(customDecalState);
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400159E RID: 5534
	public UIInput NameInput;

	// Token: 0x0400159F RID: 5535
	public UIInput URLInput;

	// Token: 0x040015A0 RID: 5536
	public UIInput SizeInput;

	// Token: 0x040015A1 RID: 5537
	public UIButton ImportButton;

	// Token: 0x040015A2 RID: 5538
	private CustomDecalState OverrideCustomDecalState;
}
