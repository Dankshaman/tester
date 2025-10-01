using System;
using NewNet;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class CustomTile : CustomImage
{
	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x0005042B File Offset: 0x0004E62B
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
					Singleton<UICustomTile>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00050452 File Offset: 0x0004E652
	protected override void OnDestroy()
	{
		base.OnDestroy();
		UnityEngine.Object.Destroy(this.TempMesh);
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00050465 File Offset: 0x0004E665
	protected override void OnCallCustomRPC()
	{
		base.OnCallCustomRPC();
		base.networkView.RPC<TileType, float, bool, bool>(RPCTarget.Others, new Action<TileType, float, bool, bool>(this.RPCExtraInfo), this.CurrentTileType, this.Thickness, this.bStackable, this.bStretch);
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x0005049D File Offset: 0x0004E69D
	protected override void OnCallCustomRPC(NetworkPlayer NP)
	{
		base.OnCallCustomRPC(NP);
		base.networkView.RPC<TileType, float, bool, bool>(NP, new Action<TileType, float, bool, bool>(this.RPCExtraInfo), this.CurrentTileType, this.Thickness, this.bStackable, this.bStretch);
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x000504D6 File Offset: 0x0004E6D6
	[Remote(Permission.Admin)]
	private void RPCExtraInfo(TileType currentTileType, float thickness, bool bstackable, bool bstretch)
	{
		this.CurrentTileType = currentTileType;
		this.Thickness = thickness;
		this.bStackable = bstackable;
		this.bStretch = bstretch;
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x000504F8 File Offset: 0x0004E6F8
	protected override void OnSetupImage(Texture T, float AspectRatio, Material mat)
	{
		base.OnSetupImage(T, AspectRatio, mat);
		this.SetupTile(AspectRatio);
		base.GetComponent<Renderer>().sharedMaterials[1].mainTexture = T;
		if (this.CustomImageSecondaryURL == "")
		{
			base.GetComponent<Renderer>().sharedMaterials[2].mainTexture = T;
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0005054D File Offset: 0x0004E74D
	protected override void OnSetupSecondaryImage(Texture T, float AspectRatio, Material mat)
	{
		base.OnSetupSecondaryImage(T, AspectRatio, mat);
		this.SetupTile(AspectRatio);
		base.GetComponent<Renderer>().sharedMaterials[2].mainTexture = T;
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00050574 File Offset: 0x0004E774
	private void SetupTile(float AspectRatio)
	{
		if (this.bSetupTile)
		{
			return;
		}
		if (!this.bStretch)
		{
			AspectRatio = 1f;
		}
		this.bSetupTile = true;
		bool flag = false;
		float num = this.Thickness * 5f;
		if (this.CurrentTileType != TileType.Box)
		{
			base.GetComponent<MeshFilter>().mesh = this.TileMeshes[(int)this.CurrentTileType];
			if (!this.DummyObject)
			{
				if (!base.GetComponent<StackObject>() && this.CurrentTileType != TileType.Rounded)
				{
					UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
					MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
					this.TempMesh = UnityEngine.Object.Instantiate<Mesh>(this.TileColliders[(int)this.CurrentTileType]);
					if (num != 1f || AspectRatio != 1f)
					{
						Utilities.ScaleMesh(this.TempMesh, new Vector3(1f * AspectRatio, num, 1f), false);
					}
					meshCollider.sharedMesh = this.TempMesh;
					meshCollider.convex = true;
				}
				else
				{
					flag = true;
				}
			}
		}
		else
		{
			flag = true;
		}
		if (num != 1f || AspectRatio != 1f)
		{
			Utilities.ScaleMesh(base.GetComponent<MeshFilter>().mesh, new Vector3(1f * AspectRatio, num, 1f), true);
		}
		if (!this.DummyObject)
		{
			if (flag)
			{
				UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
				base.gameObject.AddComponent<BoxCollider>();
			}
			base.GetComponent<NetworkPhysicsObject>().ResetBounds();
			if (this.bStackable)
			{
				base.GetComponent<MeshFilter>().sharedMesh.name = string.Concat(new object[]
				{
					this.CustomImageURL,
					"*",
					this.CustomImageSecondaryURL,
					"*",
					this.Thickness
				});
				if (base.GetComponent<StackObject>())
				{
					base.GetComponent<StackObject>().Spacing = base.GetComponent<MeshFilter>().sharedMesh.bounds.size.y;
					base.GetComponent<StackObject>().RefreshChildren();
					base.GetComponent<StackObject>().ResetColliderOffset();
					return;
				}
				base.gameObject.AddComponent<CheckStackObject>();
			}
		}
	}

	// Token: 0x04000811 RID: 2065
	public Mesh[] TileMeshes;

	// Token: 0x04000812 RID: 2066
	public Mesh[] TileColliders;

	// Token: 0x04000813 RID: 2067
	public float Thickness = 0.5f;

	// Token: 0x04000814 RID: 2068
	public bool bStackable;

	// Token: 0x04000815 RID: 2069
	public bool bStretch = true;

	// Token: 0x04000816 RID: 2070
	public TileType CurrentTileType;

	// Token: 0x04000817 RID: 2071
	private Mesh TempMesh;

	// Token: 0x04000818 RID: 2072
	private bool bSetupTile;
}
