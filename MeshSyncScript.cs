using System;
using NewNet;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class MeshSyncScript : NetworkBehavior
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060015F5 RID: 5621 RVA: 0x0009919B File Offset: 0x0009739B
	// (set) Token: 0x060015F6 RID: 5622 RVA: 0x000991A3 File Offset: 0x000973A3
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int MeshInt
	{
		get
		{
			return this._MeshInt;
		}
		set
		{
			if (value != this._MeshInt)
			{
				this._MeshInt = value;
				base.DirtySync("MeshInt");
			}
		}
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x000991C0 File Offset: 0x000973C0
	public override void OnSync()
	{
		this.SetMesh(this.MeshInt);
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x000991D0 File Offset: 0x000973D0
	public void SetMesh(int subscript)
	{
		if (subscript < this.Meshes.Length && subscript >= 0)
		{
			this.MeshInt = subscript;
			base.GetComponent<MeshFilter>().mesh = this.Meshes[this.MeshInt];
		}
		if (base.GetComponent<StackObject>())
		{
			base.GetComponent<StackObject>().RefreshChildren();
		}
		if (base.gameObject.CompareTag("Piecepack") && !base.gameObject.GetComponent<BoxCollider>())
		{
			base.gameObject.AddComponent<BoxCollider>();
			NetworkPhysicsObject component = base.gameObject.GetComponent<NetworkPhysicsObject>();
			if (component)
			{
				component.ResetBounds();
				component.ResetPhysicsMaterial();
			}
		}
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x00099273 File Offset: 0x00097473
	public int GetMesh()
	{
		return this.MeshInt;
	}

	// Token: 0x04000C64 RID: 3172
	public Mesh[] Meshes;

	// Token: 0x04000C65 RID: 3173
	private int _MeshInt;
}
