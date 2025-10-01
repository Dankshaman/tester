using System;
using NewNet;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class CustomDice : CustomImage
{
	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000A59 RID: 2649 RVA: 0x00049224 File Offset: 0x00047424
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
					Singleton<UICustomDice>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0004924B File Offset: 0x0004744B
	protected override void OnCallCustomRPC()
	{
		base.OnCallCustomRPC();
		if (this.CurrentDiceType != DiceType.d6)
		{
			base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.RPCSetCurrentDiceType), (int)this.CurrentDiceType);
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0004927A File Offset: 0x0004747A
	protected override void OnCallCustomRPC(NetworkPlayer NP)
	{
		base.OnCallCustomRPC(NP);
		if (this.CurrentDiceType != DiceType.d6)
		{
			base.networkView.RPC<int>(NP, new Action<int>(this.RPCSetCurrentDiceType), (int)this.CurrentDiceType);
		}
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x000492AA File Offset: 0x000474AA
	[Remote(Permission.Admin)]
	private void RPCSetCurrentDiceType(int DiceInt)
	{
		this.CurrentDiceType = (DiceType)DiceInt;
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x000492B4 File Offset: 0x000474B4
	protected override void OnSetupImage(Texture T, float AspectRatio, Material mat)
	{
		base.OnSetupImage(T, AspectRatio, mat);
		if (this.CurrentDiceType != DiceType.d6)
		{
			if (mat == null)
			{
				Color color = base.GetComponent<Renderer>().sharedMaterial.color;
				if (!this.bFoundSharedMat)
				{
					base.GetComponent<Renderer>().sharedMaterial = UnityEngine.Object.Instantiate<Material>(this.DiceMaterials[(int)this.CurrentDiceType]);
					base.GetComponent<Renderer>().sharedMaterial.color = color;
				}
			}
			base.GetComponent<MeshFilter>().mesh = this.DiceMeshes[(int)this.CurrentDiceType];
			if (!this.DummyObject)
			{
				UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
				MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
				meshCollider.sharedMesh = this.DiceColliders[(int)this.CurrentDiceType];
			}
		}
		if (!this.DummyObject)
		{
			base.NPO.SetupDefaultRotationValues();
		}
		base.GetComponent<Renderer>().sharedMaterial.mainTexture = T;
		base.ResetObject();
	}

	// Token: 0x04000755 RID: 1877
	public Material[] DiceMaterials;

	// Token: 0x04000756 RID: 1878
	public Mesh[] DiceMeshes;

	// Token: 0x04000757 RID: 1879
	public Mesh[] DiceColliders;

	// Token: 0x04000758 RID: 1880
	public DiceType CurrentDiceType = DiceType.d6;
}
