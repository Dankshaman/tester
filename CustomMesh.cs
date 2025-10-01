using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000E4 RID: 228
public class CustomMesh : CustomObject
{
	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000B02 RID: 2818 RVA: 0x0004CE28 File Offset: 0x0004B028
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
					Singleton<UICustomMesh>.Instance.Queue(this);
				}
			}
		}
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0004CE5F File Offset: 0x0004B05F
	public override void Cancel()
	{
		if (Network.isServer && !this.bSetupOnce)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000B04 RID: 2820 RVA: 0x0004CE80 File Offset: 0x0004B080
	// (set) Token: 0x06000B05 RID: 2821 RVA: 0x0004CE88 File Offset: 0x0004B088
	public CustomMeshState customMeshState { get; set; } = new CustomMeshState();

	// Token: 0x06000B06 RID: 2822 RVA: 0x0004CE94 File Offset: 0x0004B094
	protected override void Start()
	{
		base.Start();
		if (this.DummyObject)
		{
			this.StartWWWs();
			return;
		}
		if (Network.isServer)
		{
			if (!string.IsNullOrEmpty(this.customMeshState.MeshURL))
			{
				this.CallCustomRPC();
				return;
			}
			this.bCustomUI = true;
		}
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0004CEE4 File Offset: 0x0004B0E4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Singleton<CustomLoadingManager>.Instance)
		{
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this.customMeshState.DiffuseURL, new Action<CustomTextureContainer>(this.OnLoadImageFinish), true);
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this.customMeshState.NormalURL, new Action<CustomTextureContainer>(this.OnLoadNormalImageFinish), true);
			Singleton<CustomLoadingManager>.Instance.Mesh.Cleanup(this.customMeshState.MeshURL, new Action<CustomMeshContainer>(this.OnLoadModelFinish), true);
			Singleton<CustomLoadingManager>.Instance.Mesh.Cleanup(this.customMeshState.ColliderURL, new Action<CustomMeshContainer>(this.OnLoadColliderFinish), true);
		}
		if (this.CustomObjects != null)
		{
			for (int i = 0; i < this.CustomObjects.Length; i++)
			{
				if (this.CustomObjects[i])
				{
					UnityEngine.Object.Destroy(this.CustomObjects[i]);
				}
			}
		}
		if (this.ColliderObjects != null)
		{
			for (int j = 0; j < this.ColliderObjects.Length; j++)
			{
				if (this.ColliderObjects[j])
				{
					UnityEngine.Object.Destroy(this.ColliderObjects[j]);
				}
			}
		}
		if (this.CompoundColliders != null)
		{
			for (int k = 0; k < this.CompoundColliders.Length; k++)
			{
				if (this.CompoundColliders[k])
				{
					UnityEngine.Object.Destroy(this.CompoundColliders[k]);
				}
			}
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0004D048 File Offset: 0x0004B248
	public override void CallCustomRPC()
	{
		base.CallCustomRPC();
		if (this.bMeshSet && Network.isServer)
		{
			ObjectState os = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
			NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, false, false);
			return;
		}
		if (Network.isServer)
		{
			if (this.customMeshState.TypeIndex == 6 && base.NPO.InternalName != NetworkSingleton<GameMode>.Instance.CustomModelBag.name)
			{
				ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState.Name = NetworkSingleton<GameMode>.Instance.CustomModelBag.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, false, false);
				return;
			}
			if (this.customMeshState.TypeIndex != 6 && base.NPO.InternalName == NetworkSingleton<GameMode>.Instance.CustomModelBag.name)
			{
				ObjectState objectState2 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState2.Name = NetworkSingleton<GameMode>.Instance.CustomModel.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState2, false, false);
				return;
			}
			if (this.customMeshState.TypeIndex == 7 && base.NPO.InternalName != NetworkSingleton<GameMode>.Instance.CustomModelInfiniteBag.name)
			{
				ObjectState objectState3 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState3.Name = NetworkSingleton<GameMode>.Instance.CustomModelInfiniteBag.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState3, false, false);
				return;
			}
			if (this.customMeshState.TypeIndex != 7 && base.NPO.InternalName == NetworkSingleton<GameMode>.Instance.CustomModelInfiniteBag.name)
			{
				ObjectState objectState4 = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(base.gameObject);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
				objectState4.Name = NetworkSingleton<GameMode>.Instance.CustomModel.name;
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState4, false, false);
				return;
			}
		}
		base.networkView.RPC<CustomMeshState>(RPCTarget.All, new Action<CustomMeshState>(this.SetCustom), this.customMeshState);
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0004D295 File Offset: 0x0004B495
	public override void CallCustomRPC(NetworkPlayer NP)
	{
		base.networkView.RPC<CustomMeshState>(NP, new Action<CustomMeshState>(this.SetCustom), this.customMeshState);
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0004D2B5 File Offset: 0x0004B4B5
	[Remote(Permission.Admin)]
	private void SetCustom(CustomMeshState customMeshState)
	{
		this.customMeshState = customMeshState;
		if (customMeshState.CastShadows)
		{
			base.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
		}
		else
		{
			base.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
		}
		this.StartWWWs();
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0004D2E8 File Offset: 0x0004B4E8
	private void StartWWWs()
	{
		base.AddLoading();
		Singleton<CustomLoadingManager>.Instance.Mesh.Load(this.customMeshState.MeshURL, new Action<CustomMeshContainer>(this.OnLoadModelFinish), null);
		if (this.customMeshState.DiffuseURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.Texture.Load(this.customMeshState.DiffuseURL, new Action<CustomTextureContainer>(this.OnLoadImageFinish), true, false, false, true, true, false, 8192, CustomLoadingManager.LoadType.Auto);
		}
		if (this.customMeshState.NormalURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.Texture.Load(this.customMeshState.NormalURL, new Action<CustomTextureContainer>(this.OnLoadNormalImageFinish), false, true, true, true, true, false, 8192, CustomLoadingManager.LoadType.Auto);
		}
		if (this.customMeshState.ColliderURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.Mesh.Load(this.customMeshState.ColliderURL, new Action<CustomMeshContainer>(this.OnLoadColliderFinish), null);
		}
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0004D404 File Offset: 0x0004B604
	public void OnLoadModelFinish(CustomMeshContainer container)
	{
		if (container.mesh == null)
		{
			this.bMeshSet = true;
			base.RemoveLoading();
			return;
		}
		this.SetupMesh(container.mesh);
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0004D42E File Offset: 0x0004B62E
	public void OnLoadColliderFinish(CustomMeshContainer container)
	{
		if (container.mesh == null)
		{
			base.RemoveLoading();
			return;
		}
		this.SetupCollider(container.mesh);
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0004D451 File Offset: 0x0004B651
	public void OnLoadImageFinish(CustomTextureContainer container)
	{
		if (container.texture == null)
		{
			base.RemoveLoading();
			return;
		}
		base.StartCoroutine(this.SetupImage(container.texture, container.aspectRatio));
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0004D481 File Offset: 0x0004B681
	public void OnLoadNormalImageFinish(CustomTextureContainer container)
	{
		if (container.texture == null)
		{
			base.RemoveLoading();
			return;
		}
		base.StartCoroutine(this.SetupNormalImage(container.texture));
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0004D4AC File Offset: 0x0004B6AC
	public static void SetTypeAndMaterial(NetworkPhysicsObject NPO, int TypeIndex, int MaterialInt, float stackSize = -1f)
	{
		NPO.gameObject.tag = CustomMesh.TypeList[TypeIndex];
		SoundScript soundScript = NPO.soundScript;
		if (MaterialInt == 0 && NPO.gameObject.CompareTag("Dice"))
		{
			soundScript.sounds = Resources.Load<ObjectSounds>("SoundObjects/Plastic Die");
		}
		else if (MaterialInt == 1)
		{
			soundScript.sounds = Resources.Load<ObjectSounds>("SoundObjects/Wood");
			NPO.GetComponent<SoundMaterial>().soundMaterialType = SoundMaterialType.Wood;
		}
		else if (MaterialInt == 2)
		{
			if (NPO.gameObject.CompareTag("Dice"))
			{
				soundScript.sounds = Resources.Load<ObjectSounds>("SoundObjects/Metal Die");
			}
			else if (NPO.gameObject.CompareTag("Coin"))
			{
				soundScript.sounds = Resources.Load<ObjectSounds>("SoundObjects/Metal Coin");
			}
			else
			{
				soundScript.sounds = Resources.Load<ObjectSounds>("SoundObjects/Metal");
			}
			NPO.GetComponent<SoundMaterial>().soundMaterialType = SoundMaterialType.Metal;
		}
		else if (MaterialInt == 3)
		{
			soundScript.sounds = Resources.Load<ObjectSounds>("SoundObjects/Cardboard");
			NPO.GetComponent<SoundMaterial>().soundMaterialType = SoundMaterialType.Cardboard;
		}
		else if (MaterialInt == 4)
		{
			soundScript.sounds = Resources.Load<ObjectSounds>("SoundObjects/Glass");
			NPO.GetComponent<SoundMaterial>().soundMaterialType = SoundMaterialType.Glass;
		}
		if (NPO.gameObject.CompareTag("Board"))
		{
			NPO.ResetHiders();
		}
		NPO.SetupGrabbableLayer(false);
		NPO.SetLayerToHeld(true);
		NPO.SetLayerToHeld(false);
		if (TypeIndex == 5)
		{
			if (NPO.stackObject)
			{
				if (stackSize != -1f)
				{
					NPO.stackObject.Spacing = stackSize;
					NPO.stackObject.ResetColliderOffset();
				}
			}
			else
			{
				NPO.gameObject.AddComponent<CheckStackObject>();
			}
		}
		if (Network.isServer && NPO.gameObject.CompareTag("Dice"))
		{
			NPO.IgnoresGrid = true;
			NPO.IgnoresSnap = true;
		}
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x0004D660 File Offset: 0x0004B860
	private void SetupMesh(Mesh M)
	{
		int num = M.subMeshCount;
		if (num == 0)
		{
			num = 1;
		}
		Material[] array = new Material[num];
		bool flag = false;
		if (!this.DummyObject)
		{
			List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
			for (int i = 0; i < grabbableNPOs.Count; i++)
			{
				NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
				if (networkPhysicsObject.ID != -1 && networkPhysicsObject != base.NPO && networkPhysicsObject.customMesh && networkPhysicsObject.customMesh.bMeshSet && base.NPO.DiffuseColor == networkPhysicsObject.DiffuseColor && this.customMeshState.DiffuseURL == networkPhysicsObject.customMesh.customMeshState.DiffuseURL && this.customMeshState.NormalURL == networkPhysicsObject.customMesh.customMeshState.NormalURL && this.customMeshState.MaterialIndex == networkPhysicsObject.customMesh.customMeshState.MaterialIndex && ((this.customMeshState.CustomShader == null && networkPhysicsObject.customMesh.customMeshState.CustomShader == null) || (this.customMeshState.CustomShader != null && networkPhysicsObject.customMesh.customMeshState.CustomShader != null && this.customMeshState.CustomShader.FresnelStrength == networkPhysicsObject.customMesh.customMeshState.CustomShader.FresnelStrength && this.customMeshState.CustomShader.SpecularColor.ToColour() == networkPhysicsObject.customMesh.customMeshState.CustomShader.SpecularColor.ToColour() && this.customMeshState.CustomShader.SpecularIntensity == networkPhysicsObject.customMesh.customMeshState.CustomShader.SpecularIntensity && this.customMeshState.CustomShader.SpecularSharpness == networkPhysicsObject.customMesh.customMeshState.CustomShader.SpecularSharpness)) && networkPhysicsObject.GetComponent<Renderer>().sharedMaterials.Length == array.Length)
				{
					array = networkPhysicsObject.GetComponent<Renderer>().sharedMaterials;
					flag = true;
					break;
				}
			}
		}
		Color color = base.GetComponent<Renderer>().sharedMaterial.color;
		if (!flag)
		{
			array[0] = UnityEngine.Object.Instantiate<Material>(this.Materials[this.customMeshState.MaterialIndex]);
			if (!string.IsNullOrEmpty(this.customMeshState.NormalURL))
			{
				array[0].shader = Shader.Find("Marmoset/Bumped Specular IBL");
			}
			if (this.customMeshState.CustomShader != null)
			{
				this.customMeshState.CustomShader.AssignToMarmosetMaterial(array[0]);
			}
			if (color == array[0].color)
			{
				for (int j = 1; j < num; j++)
				{
					array[j] = array[0];
				}
			}
			else if (array.Length > 1)
			{
				Material material = UnityEngine.Object.Instantiate<Material>(array[0]);
				for (int k = 1; k < num; k++)
				{
					array[k] = material;
				}
			}
			array[0].color = color;
			TextureScript.UpdateMaterialTransparency(array[0]);
		}
		base.GetComponent<Renderer>().materials = array;
		base.DirtyHighlighter();
		base.GetComponent<MeshFilter>().mesh = M;
		if (this.customMeshState.ColliderURL == "")
		{
			UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
			base.gameObject.AddComponent<BoxCollider>();
			base.ResetObject();
		}
		if (base.NPO)
		{
			CustomMesh.SetTypeAndMaterial(base.NPO, this.customMeshState.TypeIndex, this.customMeshState.MaterialIndex, (this.customMeshState.ColliderURL == "") ? M.bounds.size.y : -1f);
		}
		this.bMeshSet = true;
		this.bSetupOnce = true;
		base.RemoveLoading();
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0004DA67 File Offset: 0x0004BC67
	private IEnumerator SetupImage(Texture T, float AspectRatio)
	{
		while (!this.bMeshSet)
		{
			yield return null;
		}
		int num = base.GetComponent<Renderer>().sharedMaterials.Length;
		for (int i = 0; i < num; i++)
		{
			base.GetComponent<Renderer>().sharedMaterials[i].mainTexture = T;
			TextureScript.UpdateMaterialTransparency(base.GetComponent<Renderer>().sharedMaterials[i]);
		}
		base.DirtyHighlighter();
		base.RemoveLoading();
		yield break;
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x0004DA7D File Offset: 0x0004BC7D
	private IEnumerator SetupNormalImage(Texture T)
	{
		while (!this.bMeshSet)
		{
			yield return null;
		}
		int num = base.GetComponent<Renderer>().sharedMaterials.Length;
		for (int i = 0; i < num; i++)
		{
			base.GetComponent<Renderer>().sharedMaterials[i].SetTexture("_BumpMap", T);
		}
		base.DirtyHighlighter();
		base.RemoveLoading();
		yield break;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0004DA94 File Offset: 0x0004BC94
	private void SetupCollider(Mesh M)
	{
		if (this.customMeshState.TypeIndex == 5)
		{
			UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
			base.gameObject.AddComponent<BoxCollider>();
			if (base.GetComponent<StackObject>())
			{
				base.GetComponent<StackObject>().Spacing = M.bounds.size.y;
				base.GetComponent<StackObject>().ResetColliderOffset();
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
			if (M.subMeshCount > 1 && this.customMeshState.Convex)
			{
				this.CompoundColliders = new Mesh[M.subMeshCount];
				for (int i = 0; i < M.subMeshCount; i++)
				{
					Mesh mesh = new Mesh();
					Vector3[] vertices = M.vertices;
					int[] triangles = M.GetTriangles(i);
					List<Vector3> list = new List<Vector3>();
					List<int> list2 = new List<int>();
					for (int j = 0; j < vertices.Length; j++)
					{
						for (int k = 0; k < triangles.Length; k++)
						{
							if (triangles[k] == j)
							{
								list2.Add(list.Count);
								list.Add(vertices[j]);
							}
						}
					}
					mesh.SetVertices(list);
					mesh.SetTriangles(list2, 0);
					MeshCollider meshCollider;
					if (i == 0)
					{
						meshCollider = base.gameObject.AddComponent<MeshCollider>();
					}
					else
					{
						GameObject gameObject = new GameObject();
						gameObject.transform.parent = base.transform;
						gameObject.transform.Reset();
						gameObject.layer = base.gameObject.layer;
						meshCollider = gameObject.AddComponent<MeshCollider>();
					}
					meshCollider.sharedMesh = mesh;
					meshCollider.convex = this.customMeshState.Convex;
					this.CompoundColliders[i] = mesh;
				}
			}
			else
			{
				MeshCollider meshCollider2 = base.gameObject.AddComponent<MeshCollider>();
				meshCollider2.sharedMesh = M;
				meshCollider2.convex = this.customMeshState.Convex;
			}
		}
		base.ResetObject();
		base.RemoveLoading();
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0004DC7D File Offset: 0x0004BE7D
	public CustomShaderState GetCustomShaderState()
	{
		if (this.customMeshState.CustomShader != null)
		{
			return this.customMeshState.CustomShader;
		}
		return this.GetCustomShaderState(this.customMeshState.MaterialIndex);
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0004DCAF File Offset: 0x0004BEAF
	public CustomShaderState GetCustomShaderState(int matIndex)
	{
		return new CustomShaderState(this.Materials[matIndex]);
	}

	// Token: 0x040007C9 RID: 1993
	private GameObject[] CustomObjects;

	// Token: 0x040007CA RID: 1994
	private GameObject[] ColliderObjects;

	// Token: 0x040007CB RID: 1995
	private Mesh[] CompoundColliders;

	// Token: 0x040007CC RID: 1996
	private bool bSetupOnce;

	// Token: 0x040007CE RID: 1998
	public static string[] MaterialList = new string[]
	{
		"Plastic",
		"Wood",
		"Metal",
		"Cardboard",
		"Glass"
	};

	// Token: 0x040007CF RID: 1999
	public static string[] TypeList = new string[]
	{
		"Generic",
		"Figurine",
		"Dice",
		"Coin",
		"Board",
		"Chip",
		"Bag",
		"Infinite"
	};

	// Token: 0x040007D0 RID: 2000
	public Material[] Materials;

	// Token: 0x040007D1 RID: 2001
	public bool bMeshSet;
}
