using System;
using HighlightingSystem;
using NewNet;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class MaterialSyncScript : NetworkBehavior
{
	// Token: 0x1700038F RID: 911
	// (get) Token: 0x060015DF RID: 5599 RVA: 0x00098B61 File Offset: 0x00096D61
	// (set) Token: 0x060015E0 RID: 5600 RVA: 0x00098B69 File Offset: 0x00096D69
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int MaterialInt
	{
		get
		{
			return this._MaterialInt;
		}
		set
		{
			if (value != this._MaterialInt)
			{
				this._MaterialInt = value;
				base.DirtySync("MaterialInt");
			}
		}
	}

	// Token: 0x060015E1 RID: 5601 RVA: 0x00098B86 File Offset: 0x00096D86
	public override void OnSync()
	{
		this.SetMaterial(this.MaterialInt);
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x00098B94 File Offset: 0x00096D94
	public void SetMaterial(int subscript)
	{
		if (Network.isServer && (base.gameObject.CompareTag("Dice") || base.gameObject.CompareTag("Domino") || base.gameObject.CompareTag("Chess")) && base.gameObject.name != "Die_6_Rounded(Clone)" && base.gameObject.name != "Die_Piecepack(Clone)")
		{
			if (!base.gameObject.CompareTag("Chess"))
			{
				if (subscript == 2 && !SteamManager.bKickstarterGold)
				{
					Chat.Log("Could not load golden version of piece because you do not have that reward.", Colour.Yellow, ChatMessageType.Game, false);
					return;
				}
			}
			else if (subscript == 4 && !SteamManager.bKickstarterGold)
			{
				Chat.Log("Could not load golden version of piece because you do not have that reward.", Colour.Yellow, ChatMessageType.Game, false);
				return;
			}
		}
		if (subscript < this.Materials.Length && subscript >= 0)
		{
			this.MaterialInt = subscript;
			base.GetComponent<Renderer>().material = this.Materials[subscript];
			if (base.GetComponent<Highlighter>())
			{
				base.gameObject.GetComponent<Highlighter>().SetDirty();
			}
		}
		if (base.CompareTag("Dice") && base.GetComponent<Renderer>().materials.Length > 1)
		{
			if (base.gameObject.GetComponent<SoundMaterial>().soundMaterialType == SoundMaterialType.Metal)
			{
				base.GetComponent<Renderer>().materials[1].color = new Color(0.05f, 0.05f, 0.05f);
			}
			else
			{
				base.GetComponent<Renderer>().materials[1].color = new Color(0.9f, 0.9f, 0.9f);
			}
		}
		if (base.GetComponent<StackObject>())
		{
			base.GetComponent<StackObject>().RefreshChildren();
		}
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x00098D37 File Offset: 0x00096F37
	public int GetMaterial()
	{
		return this.MaterialInt;
	}

	// Token: 0x04000C47 RID: 3143
	public Material[] Materials;

	// Token: 0x04000C48 RID: 3144
	private int _MaterialInt;
}
