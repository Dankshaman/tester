using System;
using NewNet;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class CustomDeck : CustomObject
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000A54 RID: 2644 RVA: 0x00049161 File Offset: 0x00047361
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
					Singleton<UICustomDeck>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x00049188 File Offset: 0x00047388
	public override void Cancel()
	{
		if (Network.isServer && !base.GetComponent<Renderer>().materials[1].mainTexture && !base.GetComponent<Renderer>().materials[2].mainTexture)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x000491DE File Offset: 0x000473DE
	protected override void Start()
	{
		base.Start();
		if (Network.isServer && base.GetComponent<DeckScript>().GetDeck().Count == 0)
		{
			this.bCustomUI = true;
		}
	}

	// Token: 0x04000747 RID: 1863
	public CardType Type;

	// Token: 0x04000748 RID: 1864
	public string URLFace = "";

	// Token: 0x04000749 RID: 1865
	public string URLBack = "";

	// Token: 0x0400074A RID: 1866
	public int NumberCards;

	// Token: 0x0400074B RID: 1867
	public bool bSideways;

	// Token: 0x0400074C RID: 1868
	public bool bUniqueBacks;

	// Token: 0x0400074D RID: 1869
	public bool bBackIsHidden;
}
