using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class CustomImage : CustomObject
{
	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000A62 RID: 2658 RVA: 0x00049464 File Offset: 0x00047664
	public override bool bCustomUI
	{
		get
		{
			return this.bcustomUI;
		}
		set
		{
			if (this.DummyObject && value)
			{
				return;
			}
			if (value != this.bcustomUI)
			{
				this.bcustomUI = value;
				if (value && Network.isServer)
				{
					if (this.ObjectName == "Figurine")
					{
						Singleton<UICustomImageDouble>.Instance.Queue(this);
						return;
					}
					Singleton<UICustomImage>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x000494C4 File Offset: 0x000476C4
	public override void Cancel()
	{
		if (Network.isServer && this.bDestroyOnCancel && !this.bSetupOnce)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x000494F0 File Offset: 0x000476F0
	protected override void Start()
	{
		base.Start();
		if (this.CustomRenderer == null)
		{
			Debug.LogError("No custom renderer set!");
		}
		if (this.DummyObject)
		{
			this.StartWWWs();
			return;
		}
		if (Network.isServer)
		{
			if (this.CustomImageURL != "" || this.CustomImageSecondaryURL != "")
			{
				this.CallCustomRPC();
				return;
			}
			this.bCustomUI = true;
		}
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00049568 File Offset: 0x00047768
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Singleton<CustomLoadingManager>.Instance)
		{
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this.CustomImageURL, new Action<CustomTextureContainer>(this.OnTextureFinish), true);
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this.CustomImageSecondaryURL, new Action<CustomTextureContainer>(this.OnTextureSecondaryFinish), true);
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x000495CC File Offset: 0x000477CC
	public override void CallCustomRPC()
	{
		base.CallCustomRPC();
		if (this.bSetupOnce && Network.isServer)
		{
			if (!(this.ObjectName == "Table"))
			{
				ObjectState os = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, false, false);
				return;
			}
			Debug.LogError("Table is trying to reload incorrectly!");
		}
		if (this.CustomImageURL != "" || this.CustomImageSecondaryURL != "")
		{
			this.OnCallCustomRPC();
			base.networkView.RPC<string, string, float>(RPCTarget.All, new Action<string, string, float>(this.SetCustomURL), this.CustomImageURL, this.CustomImageSecondaryURL, this.CardScalar);
		}
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00049690 File Offset: 0x00047890
	public override void CallCustomRPC(NetworkPlayer NP)
	{
		if (this.CustomImageURL != "" || this.CustomImageSecondaryURL != "")
		{
			this.OnCallCustomRPC(NP);
			base.networkView.RPC<string, string, float>(NP, new Action<string, string, float>(this.SetCustomURL), this.CustomImageURL, this.CustomImageSecondaryURL, this.CardScalar);
		}
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnCallCustomRPC()
	{
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnCallCustomRPC(NetworkPlayer NP)
	{
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x000496F2 File Offset: 0x000478F2
	[Remote(Permission.Admin)]
	private void SetCustomURL(string URL, string SecondaryURL, float Scalar)
	{
		this.CustomImageURL = URL;
		this.CustomImageSecondaryURL = SecondaryURL;
		this.CardScalar = Scalar;
		this.StartWWWs();
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x00049710 File Offset: 0x00047910
	private void StartWWWs()
	{
		if (this.CustomImageURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.Texture.Load(this.CustomImageURL, new Action<CustomTextureContainer>(this.OnTextureFinish), true, false, false, true, true, this.readable, 8192, CustomLoadingManager.LoadType.Auto);
		}
		if (this.CustomImageSecondaryURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.Texture.Load(this.CustomImageSecondaryURL, new Action<CustomTextureContainer>(this.OnTextureSecondaryFinish), true, false, false, true, true, this.readable, 8192, CustomLoadingManager.LoadType.Auto);
		}
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x000497B1 File Offset: 0x000479B1
	public void OnTextureFinish(CustomTextureContainer customTextureContainer)
	{
		if (customTextureContainer.texture == null)
		{
			base.RemoveLoading();
			return;
		}
		this.SetupImage(customTextureContainer.texture, customTextureContainer.aspectRatio, customTextureContainer.material);
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x000497E0 File Offset: 0x000479E0
	public void OnTextureSecondaryFinish(CustomTextureContainer customTextureContainer)
	{
		if (customTextureContainer.texture == null)
		{
			base.RemoveLoading();
			return;
		}
		this.SetupSecondaryImage(customTextureContainer.texture, customTextureContainer.aspectRatio, customTextureContainer.material);
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00049810 File Offset: 0x00047A10
	private void SetupImage(Texture customTex, float AspectRatio, Material mat)
	{
		if (base.GetComponent<CustomBoardScript>())
		{
			base.GetComponent<CustomBoardScript>().xMulti = AspectRatio;
		}
		if (!this.DummyObject)
		{
			List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
			int i = 0;
			while (i < grabbableNPOs.Count)
			{
				NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
				if (networkPhysicsObject.ID != -1 && networkPhysicsObject != base.NPO && networkPhysicsObject.customImage && networkPhysicsObject.customImage.bSetupImage && base.NPO.DiffuseColor == networkPhysicsObject.DiffuseColor && base.GetType() == networkPhysicsObject.customImage.GetType() && this.CustomImageURL == networkPhysicsObject.customImage.CustomImageURL && this.CustomImageSecondaryURL == networkPhysicsObject.customImage.CustomImageSecondaryURL)
				{
					this.bFoundSharedMat = true;
					this.CustomRenderer.materials = networkPhysicsObject.customImage.CustomRenderer.sharedMaterials;
					if (this.CustomRendererSecondary != null && networkPhysicsObject.customImage.CustomRendererSecondary != null)
					{
						this.CustomRendererSecondary.materials = networkPhysicsObject.customImage.CustomRendererSecondary.sharedMaterials;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		if (!this.bFoundSharedMat)
		{
			if (mat == null || base.GetComponent<CustomJigsawPuzzle>())
			{
				this.CustomRenderer.materials = Utilities.CloneMaterials(this.CustomRenderer.sharedMaterials);
			}
			else
			{
				Color color = this.CustomRenderer.sharedMaterial.color;
				if (this.ObjectName != "Tile")
				{
					this.CustomRenderer.material = UnityEngine.Object.Instantiate<Material>(mat);
				}
				else
				{
					this.CustomRenderer.materials = Utilities.CloneMaterials(new Material[]
					{
						this.CustomRenderer.sharedMaterials[0],
						mat,
						mat
					});
				}
				if (color != Color.white && this.CustomRenderer.material.HasProperty("_Color"))
				{
					this.CustomRenderer.material.color = color;
				}
			}
			if (this.CustomRendererSecondary != null && this.CustomImageSecondaryURL != "" && mat == null)
			{
				this.CustomRendererSecondary.materials = Utilities.CloneMaterials(this.CustomRendererSecondary.sharedMaterials);
			}
		}
		this.OnSetupImage(customTex, AspectRatio, mat);
		if (this.ObjectName == "Board" || this.ObjectName == "Table" || this.ObjectName == "Table Square" || this.ObjectName == "Background")
		{
			this.CustomRenderer.sharedMaterial.mainTexture = customTex;
		}
		else if (this.ObjectName == "Figurine")
		{
			GameObject gameObject = this.CustomRenderer.gameObject;
			this.CustomRenderer.sharedMaterial.mainTexture = customTex;
			TextureScript.UpdateMaterialTransparency(this.CustomRenderer.sharedMaterial);
			gameObject.transform.localScale = new Vector3(AspectRatio * CustomImage.originalLocalScale.y, CustomImage.originalLocalScale.y, CustomImage.originalLocalScale.z) * this.CardScalar;
			gameObject.transform.localPosition = CustomImage.originalPosition * this.CardScalar;
			this.CardCollider.transform.localScale = new Vector3(CustomImage.originalColliderLocalScale.x, CustomImage.originalColliderLocalScale.y, AspectRatio * CustomImage.originalColliderLocalScale.y) * this.CardScalar;
			this.CardCollider.transform.localPosition = CustomImage.originalColliderPosition * this.CardScalar;
			if (this.CustomImageSecondaryURL == "")
			{
				this.CustomRendererSecondary.sharedMaterial = this.CustomRenderer.sharedMaterial;
				this.CustomRendererSecondary.transform.localScale = gameObject.transform.localScale;
				this.CustomRendererSecondary.transform.localPosition = gameObject.transform.localPosition;
			}
		}
		this.bSetupImage = true;
		base.RemoveLoading();
		this.bSetupOnce = true;
		base.ResetObject();
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnSetupImage(Texture T, float AspectRatio, Material mat)
	{
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00049C6D File Offset: 0x00047E6D
	private void SetupSecondaryImage(Texture customTex, float AspectRatio, Material mat)
	{
		base.StartCoroutine(this.DelaySetupSecondaryImage(customTex, AspectRatio, mat));
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00049C7F File Offset: 0x00047E7F
	private IEnumerator DelaySetupSecondaryImage(Texture customTex, float AspectRatio, Material mat)
	{
		while (!this.bSetupImage)
		{
			yield return null;
		}
		if (!this.bFoundSharedMat && mat != null && this.CustomRendererSecondary != null)
		{
			this.CustomRendererSecondary.material = UnityEngine.Object.Instantiate<Material>(mat);
		}
		this.OnSetupSecondaryImage(customTex, AspectRatio, mat);
		if (this.ObjectName == "Figurine")
		{
			GameObject gameObject = this.CustomRendererSecondary.gameObject;
			this.CustomRendererSecondary.sharedMaterial.mainTexture = customTex;
			TextureScript.UpdateMaterialTransparency(this.CustomRendererSecondary.sharedMaterial);
			gameObject.transform.localScale = new Vector3(AspectRatio * CustomImage.originalLocalScale.y, CustomImage.originalLocalScale.y, CustomImage.originalLocalScale.z) * this.CardScalar;
			gameObject.transform.localPosition = CustomImage.originalPosition * this.CardScalar;
		}
		base.DirtyHighlighter();
		base.RemoveLoading();
		this.bSetupOnce = true;
		yield break;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnSetupSecondaryImage(Texture T, float AspectRatio, Material mat)
	{
	}

	// Token: 0x0400075A RID: 1882
	private bool bSetupOnce;

	// Token: 0x0400075B RID: 1883
	public string CustomImageURL = "";

	// Token: 0x0400075C RID: 1884
	public string CustomImageSecondaryURL = "";

	// Token: 0x0400075D RID: 1885
	public float CardScalar = 1f;

	// Token: 0x0400075E RID: 1886
	public string ObjectName = "Object";

	// Token: 0x0400075F RID: 1887
	public Renderer CustomRenderer;

	// Token: 0x04000760 RID: 1888
	public Renderer CustomRendererSecondary;

	// Token: 0x04000761 RID: 1889
	public Transform CardCollider;

	// Token: 0x04000762 RID: 1890
	public bool bDestroyOnCancel;

	// Token: 0x04000763 RID: 1891
	public bool readable;

	// Token: 0x04000764 RID: 1892
	public bool bSetupImage;

	// Token: 0x04000765 RID: 1893
	public bool bFoundSharedMat;

	// Token: 0x04000766 RID: 1894
	private static Vector3 originalLocalScale = new Vector3(1.44f, 1.96f, 1.093366f);

	// Token: 0x04000767 RID: 1895
	private static Vector3 originalPosition = new Vector3(0f, 1.08f, 0f);

	// Token: 0x04000768 RID: 1896
	private static Vector3 originalColliderLocalScale = new Vector3(0.001f, 1.279238f, 1.279238f);

	// Token: 0x04000769 RID: 1897
	private static Vector3 originalColliderPosition = new Vector3(0f, 1.077916f, 0f);
}
