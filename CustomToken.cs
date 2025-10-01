using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class CustomToken : CustomImage
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0005079E File Offset: 0x0004E99E
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
					Singleton<UICustomToken>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x000507C5 File Offset: 0x0004E9C5
	protected override void Awake()
	{
		base.Awake();
		this.readable = true;
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x000507D4 File Offset: 0x0004E9D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Singleton<CustomLoadingManager>.Instance)
		{
			Singleton<CustomLoadingManager>.Instance.Token.Cleanup(this.settings, new Action<CustomTokenContainer>(this.TokenFinished));
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00050809 File Offset: 0x0004EA09
	protected override void OnCallCustomRPC()
	{
		base.OnCallCustomRPC();
		base.networkView.RPC<float, float, bool, bool>(RPCTarget.All, new Action<float, float, bool, bool>(this.TokenRPC), this.Thickness, this.MergeDistancePixels, this.bStandUp, this.bStackable);
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00050841 File Offset: 0x0004EA41
	protected override void OnCallCustomRPC(NetworkPlayer NP)
	{
		base.OnCallCustomRPC(NP);
		base.networkView.RPC<float, float, bool, bool>(NP, new Action<float, float, bool, bool>(this.TokenRPC), this.Thickness, this.MergeDistancePixels, this.bStandUp, this.bStackable);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0005087C File Offset: 0x0004EA7C
	[Remote(Permission.Admin)]
	private void TokenRPC(float thickness, float mergeDistancePixels, bool bstandup, bool bstackable)
	{
		this.Thickness = thickness;
		this.MergeDistancePixels = mergeDistancePixels;
		this.bStandUp = bstandup;
		this.bStackable = bstackable;
		base.NPO.gameObject.tag = (this.bStandUp ? "Figurine" : "Tile");
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x000508CC File Offset: 0x0004EACC
	protected override void OnSetupImage(Texture T, float AspectRatio, Material mat)
	{
		base.OnSetupImage(T, AspectRatio, mat);
		Texture2D texture2D = T as Texture2D;
		if (!texture2D)
		{
			Chat.LogError("Custom Tokens only work with Texture2D.", true);
			return;
		}
		base.GetComponent<Renderer>().sharedMaterial.mainTexture = T;
		this.settings = new TokenSettings(texture2D, AspectRatio, this.Thickness, this.MergeDistancePixels);
		base.AddLoading();
		Singleton<CustomLoadingManager>.Instance.Token.Load(this.settings, new Action<CustomTokenContainer>(this.TokenFinished), CustomLoadingManager.LoadType.Auto, true);
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00050950 File Offset: 0x0004EB50
	protected override void OnSetupSecondaryImage(Texture T, float AspectRatio, Material mat)
	{
		base.OnSetupSecondaryImage(T, AspectRatio, mat);
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x0005095C File Offset: 0x0004EB5C
	public void TokenFinished(CustomTokenContainer customTokenContainer)
	{
		base.RemoveLoading();
		if (customTokenContainer == null || customTokenContainer.mesh == null)
		{
			return;
		}
		MeshFilter meshFilter = base.GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = base.GetComponent<MeshRenderer>();
		if (this.bStandUp)
		{
			Material sharedMaterial = meshRenderer.sharedMaterial;
			UnityEngine.Object.Destroy(meshFilter);
			UnityEngine.Object.Destroy(meshRenderer);
			GameObject gameObject = new GameObject("Anchor");
			gameObject.transform.parent = base.transform;
			gameObject.transform.Reset();
			gameObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
			meshFilter = gameObject.AddComponent<MeshFilter>();
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			this.CustomRenderer = meshRenderer;
			if (base.NPO)
			{
				base.NPO.Renderers = new List<Renderer>
				{
					meshRenderer
				};
			}
			meshRenderer.sharedMaterial = sharedMaterial;
		}
		meshFilter.mesh = customTokenContainer.mesh;
		if (!this.DummyObject)
		{
			BoxColliderState.CreateBoxColliders(meshFilter.gameObject, customTokenContainer.boxColliders);
			UnityEngine.Object.Destroy(base.GetComponent<Collider>());
			base.ResetObject();
			if (this.bStackable)
			{
				meshFilter.sharedMesh.name = string.Concat(new string[]
				{
					this.CustomImageURL,
					"*",
					this.Thickness.ToString(),
					"*",
					this.MergeDistancePixels.ToString()
				});
				if (base.GetComponent<StackObject>())
				{
					base.GetComponent<StackObject>().Spacing = customTokenContainer.mesh.bounds.size.y;
					base.GetComponent<StackObject>().RefreshChildren();
					base.GetComponent<StackObject>().ResetColliderOffset();
					return;
				}
				base.gameObject.AddComponent<CheckStackObject>();
			}
		}
	}

	// Token: 0x04000819 RID: 2073
	public float Thickness = 0.2f;

	// Token: 0x0400081A RID: 2074
	public float MergeDistancePixels = 15f;

	// Token: 0x0400081B RID: 2075
	public bool bStandUp;

	// Token: 0x0400081C RID: 2076
	public bool bStackable;

	// Token: 0x0400081D RID: 2077
	private TokenSettings settings;
}
