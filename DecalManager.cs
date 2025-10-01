using System;
using System.Collections.Generic;
using HighlightingSystem;
using NewNet;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class DecalManager : NetworkSingleton<DecalManager>
{
	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00051F94 File Offset: 0x00050194
	// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00051F9C File Offset: 0x0005019C
	public CustomDecalState SelectedDecal
	{
		get
		{
			return this._SelectedDecal;
		}
		set
		{
			if (this._SelectedDecal != null)
			{
				this.CheckURLCleanup(this._SelectedDecal.ImageURL);
				this.DummyDecal.GetComponent<Renderer>().material.mainTexture = null;
				this.DecalRotate = 0f;
			}
			this._SelectedDecal = value;
			if (this._SelectedDecal != null)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Load(this._SelectedDecal.ImageURL, new Action<CustomTextureContainer>(this.LoadingImageFinished), true, false, false, true, true, false, 8192, CustomLoadingManager.LoadType.Auto);
			}
		}
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00052030 File Offset: 0x00050230
	public void AddDecalPallet(CustomDecalState decalCustom)
	{
		if (this.GetMatchingDecal(decalCustom) != null)
		{
			return;
		}
		base.networkView.RPC<CustomDecalState>(RPCTarget.All, new Action<CustomDecalState>(this.RPCAddDecalPallet), decalCustom);
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x0005205B File Offset: 0x0005025B
	[Remote("Permissions/Decals")]
	public void RPCAddDecalPallet(CustomDecalState decalCustom)
	{
		this.DecalPallet.Add(decalCustom);
		if (NetworkSingleton<NetworkUI>.Instance.GUIDecals.activeSelf)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIDecals.GetComponent<UIGridMenuDecals>().Reload(true);
		}
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0005208F File Offset: 0x0005028F
	public void RemoveDecalPallet(CustomDecalState decalCustom)
	{
		base.networkView.RPC<CustomDecalState>(RPCTarget.All, new Action<CustomDecalState>(this.RPCRemoveDecalPallet), decalCustom);
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x000520AC File Offset: 0x000502AC
	[Remote("Permissions/Decals")]
	public void RPCRemoveDecalPallet(CustomDecalState decalCustom)
	{
		CustomDecalState matchingDecal = this.GetMatchingDecal(decalCustom);
		if (matchingDecal != null)
		{
			this.DecalPallet.Remove(matchingDecal);
			if (this.SelectedDecal == matchingDecal)
			{
				this.SelectedDecal = null;
				return;
			}
		}
		else
		{
			Debug.LogError("Couldn't find decal to remove.");
		}
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x000520F8 File Offset: 0x000502F8
	private CustomDecalState GetMatchingDecal(CustomDecalState decalCustom)
	{
		for (int i = 0; i < this.DecalPallet.Count; i++)
		{
			CustomDecalState customDecalState = this.DecalPallet[i];
			if (customDecalState.Name == decalCustom.Name && customDecalState.ImageURL == decalCustom.ImageURL && customDecalState.Size == decalCustom.Size)
			{
				return customDecalState;
			}
		}
		return null;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0005215F File Offset: 0x0005035F
	private uint GetGUID()
	{
		return Utilities.RandomUint();
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00052168 File Offset: 0x00050368
	public void AddDecal(DecalState decalState, NetworkView parent)
	{
		if (parent && parent.disableNetworking)
		{
			this.RPCAddDecal(decalState, parent, this.GetGUID());
			return;
		}
		base.networkView.RPC<DecalState, NetworkView, uint>(RPCTarget.All, new Action<DecalState, NetworkView, uint>(this.RPCAddDecal), decalState, parent, this.GetGUID());
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x000521B4 File Offset: 0x000503B4
	[Remote("Permissions/Decals")]
	private void RPCAddDecal(DecalState decalState, NetworkView parent, uint decalGUID)
	{
		this.CreateDecal(decalState, parent, decalGUID);
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x000521C0 File Offset: 0x000503C0
	[Remote("Permissions/Decals")]
	private void RPCSetDecals(List<DecalState> decalStates, List<uint> decalGUIDs, NetworkView parent)
	{
		for (int i = this.DecalObjects.Count - 1; i >= 0; i--)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.parent == parent)
			{
				this.RPCRemoveDecal(decalObject.decalGUID);
			}
		}
		for (int j = 0; j < decalStates.Count; j++)
		{
			DecalState decalState = decalStates[j];
			uint decalGUID = decalGUIDs[j];
			this.CreateDecal(decalState, parent, decalGUID);
		}
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00052238 File Offset: 0x00050438
	private void CreateDecal(DecalState decalState, NetworkView parent, uint decalGUID)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Decal"));
		if (parent)
		{
			gameObject.transform.parent = parent.transform;
		}
		gameObject.transform.localPosition = decalState.Transform.Position();
		gameObject.transform.localEulerAngles = decalState.Transform.Rotation();
		gameObject.transform.localScale = decalState.Transform.Scale();
		gameObject.GetComponent<Collider>().enabled = this.isDecalMode;
		DecalManager.DecalObject decalObject = new DecalManager.DecalObject(gameObject, decalState, parent, decalGUID);
		if (decalObject.parentNPO)
		{
			decalObject.parentNPO.AddRenderer(gameObject.GetComponent<Renderer>());
		}
		this.DecalObjects.Add(decalObject);
		Singleton<CustomLoadingManager>.Instance.Texture.Load(decalState.CustomDecal.ImageURL, new Action<CustomTextureContainer>(this.LoadingImageFinished), true, false, false, true, true, false, 8192, CustomLoadingManager.LoadType.Auto);
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0005232C File Offset: 0x0005052C
	private void LoadingImageFinished(CustomTextureContainer customTextureContainer)
	{
		if (customTextureContainer.texture == null)
		{
			return;
		}
		Material material = null;
		for (int i = 0; i < this.DecalObjects.Count; i++)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.decalState.CustomDecal.ImageURL == customTextureContainer.nonCodeStrippedURL)
			{
				Renderer component = decalObject.decal.GetComponent<Renderer>();
				if (material == null)
				{
					material = component.material;
					material.mainTexture = customTextureContainer.texture;
				}
				else
				{
					component.sharedMaterial = material;
				}
				decalObject.decal.GetComponent<Highlighter>().SetDirty();
			}
		}
		if (this.SelectedDecal != null && this.SelectedDecal.ImageURL == customTextureContainer.nonCodeStrippedURL && this.DummyDecal.GetComponent<Renderer>().material.mainTexture == null)
		{
			this.DummyDecal.GetComponent<Renderer>().material.mainTexture = customTextureContainer.texture;
			float size = this.SelectedDecal.Size;
			this.DummyDecal.transform.localScale = new Vector3(customTextureContainer.aspectRatio * size, size, size);
		}
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00052458 File Offset: 0x00050658
	public void RemoveDecal(GameObject decal)
	{
		for (int i = 0; i < this.DecalObjects.Count; i++)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.decal == decal)
			{
				base.networkView.RPC<uint>(RPCTarget.All, new Action<uint>(this.RPCRemoveDecal), decalObject.decalGUID);
				return;
			}
		}
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x000524B8 File Offset: 0x000506B8
	[Remote("Permissions/Decals")]
	private void RPCRemoveDecal(uint decalGUID)
	{
		for (int i = 0; i < this.DecalObjects.Count; i++)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.decalGUID == decalGUID)
			{
				if (decalObject.decal)
				{
					if (decalObject.parentNPO)
					{
						decalObject.parentNPO.TryRemoveRenderer(decalObject.decal.GetComponent<Renderer>());
					}
					UnityEngine.Object.Destroy(decalObject.decal);
				}
				this.DecalObjects.RemoveAt(i);
				this.CheckURLCleanup(decalObject.decalState.CustomDecal.ImageURL);
				return;
			}
		}
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00052554 File Offset: 0x00050754
	private void CheckURLCleanup(string url)
	{
		for (int i = 0; i < this.DecalObjects.Count; i++)
		{
			if (this.DecalObjects[i].decalState.CustomDecal.ImageURL == url)
			{
				return;
			}
		}
		Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(url, new Action<CustomTextureContainer>(this.LoadingImageFinished), true);
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x000525B8 File Offset: 0x000507B8
	public void ConfirmReset()
	{
		UIDialog.Show("Delete all decals?", "Yes", "No", delegate()
		{
			base.networkView.RPC<bool>(RPCTarget.All, new Action<bool>(this.RPCReset), true);
		}, null);
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x000525DB File Offset: 0x000507DB
	public void Reset()
	{
		base.networkView.RPC<bool>(RPCTarget.All, new Action<bool>(this.RPCReset), false);
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x000525F8 File Offset: 0x000507F8
	[Remote("Permissions/Decals")]
	private void RPCReset(bool notify)
	{
		if (notify)
		{
			Chat.NotifyFromNetworkSender("has deleted all decals");
		}
		for (int i = 0; i < this.DecalObjects.Count; i++)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.decal)
			{
				if (decalObject.parentNPO)
				{
					decalObject.parentNPO.TryRemoveRenderer(decalObject.decal.GetComponent<Renderer>());
				}
				UnityEngine.Object.Destroy(decalObject.decal);
			}
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(decalObject.decalState.CustomDecal.ImageURL, new Action<CustomTextureContainer>(this.LoadingImageFinished), true);
		}
		this.DecalObjects.Clear();
		this.DecalPallet.Clear();
		this.SelectedDecal = null;
		if (NetworkSingleton<NetworkUI>.Instance.GUIDecals.activeSelf)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIDecals.GetComponent<UIGridMenuDecals>().Reload(true);
		}
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x000526E4 File Offset: 0x000508E4
	public List<DecalState> GetDecalStates(NetworkView parent = null)
	{
		List<DecalState> list = null;
		for (int i = 0; i < this.DecalObjects.Count; i++)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.parent == parent)
			{
				if (list == null)
				{
					list = new List<DecalState>();
				}
				list.Add(decalObject.decalState);
			}
		}
		return list;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0005273C File Offset: 0x0005093C
	public void SetDecalStates(List<DecalState> decalStates, NetworkView parent = null)
	{
		List<uint> list = new List<uint>(decalStates.Count);
		for (int i = 0; i < decalStates.Count; i++)
		{
			list.Add(this.GetGUID());
		}
		base.networkView.RPC<List<DecalState>, List<uint>, NetworkView>(RPCTarget.All, new Action<List<DecalState>, List<uint>, NetworkView>(this.RPCSetDecals), decalStates, list, parent);
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x00052790 File Offset: 0x00050990
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnChangePointerMode += this.OnChangePointerMode;
		EventManager.OnNetworkObjectDestroy += this.OnNetworkObjectDestroy;
		EventManager.OnDummyObjectDestroy += this.OnDummyObjectDestroy;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		this.DummyDecal = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Decal"));
		this.DummyDecal.SetActive(false);
		this.DummyDecal.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0005283C File Offset: 0x00050A3C
	private void OnDestroy()
	{
		EventManager.OnChangePointerMode -= this.OnChangePointerMode;
		EventManager.OnNetworkObjectDestroy -= this.OnNetworkObjectDestroy;
		EventManager.OnDummyObjectDestroy -= this.OnDummyObjectDestroy;
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x00052890 File Offset: 0x00050A90
	private void LateUpdate()
	{
		bool flag = PlayerScript.Pointer && PlayerScript.PointerScript.CurrentPointerMode == PointerMode.Decal && this.SelectedDecal != null && !HoverScript.HoverToolObject && (!VRHMD.isVR || VRTrackedController.ToolAction || VRTrackedController.ToolActionUp);
		if (this.DummyDecal.activeSelf != flag)
		{
			this.DummyDecal.SetActive(flag);
		}
		if (PlayerScript.Pointer && PlayerScript.PointerScript.CurrentPointerMode == PointerMode.Decal && (!VRHMD.isVR || VRTrackedController.ToolAction || VRTrackedController.ToolActionUp))
		{
			Vector3 vector;
			Vector3 eulerAngles;
			if (VRHMD.isVR)
			{
				vector = VRTrackedController.LastToolController.InteractionPoint();
				eulerAngles = Quaternion.LookRotation(vector - Singleton<VRHMD>.Instance.trackedObject.transform.position).eulerAngles;
				eulerAngles.x = 90f;
			}
			else
			{
				vector = HoverScript.FirstHit.point;
				eulerAngles = new Vector3(0f, HoverScript.MainCamera.transform.eulerAngles.y, 0f);
				eulerAngles = Quaternion.LookRotation(HoverScript.FirstHit.normal * -1f).eulerAngles;
			}
			if (this.DummyDecal.activeSelf)
			{
				this.DummyDecal.transform.localPosition = vector;
				this.DummyDecal.transform.localEulerAngles = eulerAngles;
				if (!VRHMD.isVR)
				{
					this.DummyDecal.transform.Rotate(Vector3.forward * -1f, HoverScript.MainCamera.transform.eulerAngles.y - eulerAngles.y + this.DecalRotate, Space.Self);
				}
				this.DummyDecal.transform.Translate(0f, 0f, -0.01f, Space.Self);
			}
			if ((zInput.GetButtonDown("Grab", ControlType.All) && !UICamera.HoveredUIObject) || (VRHMD.isVR && VRTrackedController.ToolActionUp))
			{
				if (!HoverScript.HoverToolObject)
				{
					NetworkView parent = null;
					NetworkPhysicsObject networkPhysicsObject = HoverScript.HoverLockObject ? HoverScript.HoverLockObject.GetComponent<NetworkPhysicsObject>() : null;
					if (networkPhysicsObject)
					{
						this.DummyDecal.transform.parent = networkPhysicsObject.transform;
						parent = networkPhysicsObject.networkView;
					}
					this.CreateDecal(this.DummyDecal.transform.localPosition, this.DummyDecal.transform.localEulerAngles, this.DummyDecal.transform.localScale, parent);
					this.DummyDecal.transform.parent = null;
				}
				else
				{
					this.RemoveDecal(HoverScript.HoverToolObject);
				}
			}
			if (!UICamera.SelectIsInput())
			{
				if (Input.GetAxis("Mouse Wheel") == 0f)
				{
					if (zInput.GetRecursiveButton("Rotate Left", ref this.TimeHolder, 0.1f, ControlType.All))
					{
						this.DecalRotate = (this.DecalRotate - (float)(PlayerScript.PointerScript ? PlayerScript.PointerScript.RotationSnap : 15)) % 360f;
					}
					if (zInput.GetRecursiveButton("Rotate Right", ref this.TimeHolder, 0.1f, ControlType.All))
					{
						this.DecalRotate = (this.DecalRotate + (float)(PlayerScript.PointerScript ? PlayerScript.PointerScript.RotationSnap : 15)) % 360f;
					}
				}
				if (zInput.GetRecursiveButton("Scale Up", ref this.TimeHolder, 0.1f, ControlType.All))
				{
					this.DummyDecal.transform.localScale = this.DummyDecal.transform.localScale * 1.1f;
				}
				if (zInput.GetRecursiveButton("Scale Down", ref this.TimeHolder, 0.1f, ControlType.All))
				{
					this.DummyDecal.transform.localScale = this.DummyDecal.transform.localScale / 1.1f;
				}
			}
		}
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x00052C84 File Offset: 0x00050E84
	public void CreateDecal(Vector3 pos, Vector3 rot, Vector3 scale, NetworkView parent)
	{
		CustomDecalState selectedDecal = this.SelectedDecal;
		if (selectedDecal == null)
		{
			Chat.LogError("Need to create and select a decal so you can place it.", true);
			return;
		}
		this.AddDecal(new DecalState
		{
			CustomDecal = selectedDecal,
			Transform = new TransformState(pos, rot, scale)
		}, parent);
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x00052CD4 File Offset: 0x00050ED4
	private void OnNetworkObjectDestroy(NetworkPhysicsObject npo)
	{
		if (npo.GetComponent<DummyObject>())
		{
			return;
		}
		for (int i = this.DecalObjects.Count - 1; i >= 0; i--)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.parentNPO == npo)
			{
				this.RPCRemoveDecal(decalObject.decalGUID);
			}
		}
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x00052D30 File Offset: 0x00050F30
	private void OnDummyObjectDestroy(DummyObject dummy)
	{
		for (int i = this.DecalObjects.Count - 1; i >= 0; i--)
		{
			DecalManager.DecalObject decalObject = this.DecalObjects[i];
			if (decalObject.parent == dummy.GetComponent<NetworkView>())
			{
				this.RPCRemoveDecal(decalObject.decalGUID);
			}
		}
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00052D84 File Offset: 0x00050F84
	private void OnChangePointerMode(PointerMode pointerMode)
	{
		bool flag = pointerMode == PointerMode.Decal;
		NetworkSingleton<NetworkUI>.Instance.GUIDecals.SetActive(flag);
		if (this.isDecalMode != flag)
		{
			this.isDecalMode = flag;
			for (int i = 0; i < this.DecalObjects.Count; i++)
			{
				this.DecalObjects[i].decal.GetComponent<Collider>().enabled = this.isDecalMode;
			}
		}
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x00052DF0 File Offset: 0x00050FF0
	private void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			for (int i = 0; i < this.DecalObjects.Count; i++)
			{
				DecalManager.DecalObject decalObject = this.DecalObjects[i];
				base.networkView.RPC<DecalState, NetworkView, uint>(player, new Action<DecalState, NetworkView, uint>(this.RPCAddDecal), decalObject.decalState, decalObject.parent, decalObject.decalGUID);
			}
			for (int j = 0; j < this.DecalPallet.Count; j++)
			{
				CustomDecalState arg = this.DecalPallet[j];
				base.networkView.RPC<CustomDecalState>(player, new Action<CustomDecalState>(this.RPCAddDecalPallet), arg);
			}
		}
	}

	// Token: 0x04000844 RID: 2116
	public List<CustomDecalState> DecalPallet = new List<CustomDecalState>();

	// Token: 0x04000845 RID: 2117
	public List<DecalManager.DecalObject> DecalObjects = new List<DecalManager.DecalObject>();

	// Token: 0x04000846 RID: 2118
	private CustomDecalState _SelectedDecal;

	// Token: 0x04000847 RID: 2119
	private GameObject DummyDecal;

	// Token: 0x04000848 RID: 2120
	private bool isDecalMode;

	// Token: 0x04000849 RID: 2121
	private float TimeHolder;

	// Token: 0x0400084A RID: 2122
	private float DecalRotate;

	// Token: 0x020005D5 RID: 1493
	public class DecalObject
	{
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06003966 RID: 14694 RVA: 0x00175084 File Offset: 0x00173284
		// (set) Token: 0x06003967 RID: 14695 RVA: 0x0017508C File Offset: 0x0017328C
		public uint decalGUID { get; private set; }

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06003968 RID: 14696 RVA: 0x00175095 File Offset: 0x00173295
		// (set) Token: 0x06003969 RID: 14697 RVA: 0x0017509D File Offset: 0x0017329D
		public GameObject decal { get; private set; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x001750A6 File Offset: 0x001732A6
		// (set) Token: 0x0600396B RID: 14699 RVA: 0x001750AE File Offset: 0x001732AE
		public DecalState decalState { get; private set; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600396C RID: 14700 RVA: 0x001750B7 File Offset: 0x001732B7
		// (set) Token: 0x0600396D RID: 14701 RVA: 0x001750BF File Offset: 0x001732BF
		public NetworkView parent { get; private set; }

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600396E RID: 14702 RVA: 0x001750C8 File Offset: 0x001732C8
		// (set) Token: 0x0600396F RID: 14703 RVA: 0x001750D0 File Offset: 0x001732D0
		public NetworkPhysicsObject parentNPO { get; private set; }

		// Token: 0x06003970 RID: 14704 RVA: 0x001750D9 File Offset: 0x001732D9
		public DecalObject(GameObject decal, DecalState decalState, NetworkView parent, uint decalGUID)
		{
			this.decal = decal;
			this.decalState = decalState;
			this.parent = parent;
			this.decalGUID = decalGUID;
			if (parent)
			{
				this.parentNPO = parent.GetComponent<NetworkPhysicsObject>();
			}
		}
	}
}
