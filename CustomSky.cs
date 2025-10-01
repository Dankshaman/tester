using System;
using NewNet;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class CustomSky : CustomObject
{
	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000B9B RID: 2971 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000B9C RID: 2972 RVA: 0x00050284 File Offset: 0x0004E484
	public override bool bCustomUI
	{
		get
		{
			return this.bcustomUI;
		}
		set
		{
			if (value != this.bcustomUI)
			{
				this.bcustomUI = value;
				if (value && Network.isServer)
				{
					Singleton<UICustomSky>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x000502AB File Offset: 0x0004E4AB
	public override void Cancel()
	{
		if (Network.isServer)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		}
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x000502C4 File Offset: 0x0004E4C4
	protected override void Awake()
	{
		base.Awake();
		CustomSky.ActiveCustomSky = this;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x000502D2 File Offset: 0x0004E4D2
	protected override void Start()
	{
		base.Start();
		if (Network.isServer)
		{
			if (this.CustomSkyURL != "")
			{
				this.CallCustomRPC();
				return;
			}
			this.bCustomUI = true;
		}
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00050301 File Offset: 0x0004E501
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Singleton<CustomLoadingManager>.Instance)
		{
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this.CustomSkyURL, new Action<CustomTextureContainer>(this.OnTextureFinish), true);
		}
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00050337 File Offset: 0x0004E537
	public override void CallCustomRPC()
	{
		base.CallCustomRPC();
		if (this.CustomSkyURL != "")
		{
			base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.SetCustomURL), this.CustomSkyURL);
		}
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x0005036F File Offset: 0x0004E56F
	public override void CallCustomRPC(NetworkPlayer NP)
	{
		if (this.CustomSkyURL != "")
		{
			base.networkView.RPC<string>(NP, new Action<string>(this.SetCustomURL), this.CustomSkyURL);
		}
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x000503A4 File Offset: 0x0004E5A4
	[Remote(Permission.Admin)]
	private void SetCustomURL(string URL)
	{
		this.CustomSkyURL = URL;
		base.AddLoading();
		Singleton<CustomLoadingManager>.Instance.Texture.Load(this.CustomSkyURL, new Action<CustomTextureContainer>(this.OnTextureFinish), true, false, false, false, true, false, 8192, CustomLoadingManager.LoadType.Auto);
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x000503EB File Offset: 0x0004E5EB
	public void OnTextureFinish(CustomTextureContainer customTextureContainer)
	{
		base.RemoveLoading();
		if (customTextureContainer.texture == null)
		{
			return;
		}
		base.GetComponent<Renderer>().material.mainTexture = customTextureContainer.texture;
	}

	// Token: 0x0400080A RID: 2058
	public string CustomSkyURL = "";

	// Token: 0x0400080B RID: 2059
	public static CustomSky ActiveCustomSky;
}
