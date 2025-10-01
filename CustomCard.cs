using System;
using NewNet;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class CustomCard : CustomObject
{
	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000A4F RID: 2639 RVA: 0x0004909C File Offset: 0x0004729C
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
					Singleton<UICustomCard>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x000490C4 File Offset: 0x000472C4
	public override void Cancel()
	{
		if (Network.isServer && !base.GetComponent<Renderer>().materials[1].mainTexture && !base.GetComponent<Renderer>().materials[2].mainTexture)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		}
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0004911A File Offset: 0x0004731A
	protected override void Start()
	{
		base.Start();
		if (Network.isServer && base.gameObject.GetComponent<CardScript>().card_id_ == -1)
		{
			this.bCustomUI = true;
		}
	}

	// Token: 0x04000743 RID: 1859
	public CardType Type;

	// Token: 0x04000744 RID: 1860
	public string URLFace = "";

	// Token: 0x04000745 RID: 1861
	public string URLBack = "";

	// Token: 0x04000746 RID: 1862
	public bool bSideways;
}
