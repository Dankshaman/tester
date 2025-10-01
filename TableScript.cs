using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200024F RID: 591
public class TableScript : NetworkBehavior, INetworkLifeCycle
{
	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06001F36 RID: 7990 RVA: 0x000DE6F3 File Offset: 0x000DC8F3
	// (set) Token: 0x06001F37 RID: 7991 RVA: 0x000DE6FB File Offset: 0x000DC8FB
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int TableTextureInt
	{
		get
		{
			return this._TableTextureInt;
		}
		set
		{
			if (value != this._TableTextureInt)
			{
				this._TableTextureInt = value;
				base.DirtySync("TableTextureInt");
			}
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06001F38 RID: 7992 RVA: 0x000DE718 File Offset: 0x000DC918
	// (set) Token: 0x06001F39 RID: 7993 RVA: 0x000DE720 File Offset: 0x000DC920
	public NetworkPhysicsObject NPO { get; private set; }

	// Token: 0x06001F3A RID: 7994 RVA: 0x000DE72C File Offset: 0x000DC92C
	private void Awake()
	{
		this.transform = base.GetComponent<Transform>();
		NetworkSingleton<ManagerPhysicsObject>.Instance.Table = base.gameObject;
		this.renderer = base.gameObject.GetComponent<Renderer>();
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
		this.NPO.ShowGridProjection = true;
		base.GetComponent<Rigidbody>().isKinematic = true;
		this.NPO.IsLocked = true;
		this.NPO.IsGrabbable = false;
		this.HandsParent = this.transform.Find("Trigger");
		this.TextParent = this.transform.Find("Text");
		if (this.StaticHandTransforms.Count > 0)
		{
			Transform transform = new GameObject("Static Hand Transform").transform;
			transform.parent = this.transform;
			transform.localPosition = this.TextParent.localPosition;
			transform.localRotation = this.TextParent.localRotation;
			transform.localScale = this.TextParent.localScale;
			foreach (Transform transform2 in this.StaticHandTransforms)
			{
				Transform transform3 = new GameObject("Static Hand Transform " + transform2.name).transform;
				transform3.parent = transform;
				transform3.localPosition = transform2.localPosition;
				transform3.localRotation = transform2.localRotation;
				transform3.localScale = transform2.localScale;
				this.createdStaticHandTransforms.Add(transform3);
			}
		}
		OutOfBounds.bStopTeleport = false;
		if (Network.isServer)
		{
			ValueTuple<RaycastHit[], int> valueTuple = PhysicsNonAlloc.RayCast(new Vector3(0f, 2f, 0f), Vector3.down, 5f, -5, true);
			RaycastHit[] item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			float num = -1f;
			for (int i = 0; i < item2; i++)
			{
				RaycastHit raycastHit = item[i];
				GameObject gameObject = raycastHit.collider.transform.root.gameObject;
				if (gameObject == base.gameObject && (gameObject.name != "Table_Circular(Clone)" || raycastHit.collider.gameObject.GetComponentInParent<SoundMaterial>().soundMaterialType != SoundMaterialType.GlassSurface) && raycastHit.point.y > num)
				{
					num = raycastHit.point.y;
				}
			}
			if (num != -1f)
			{
				float num2 = -(num - 0.96f);
				Debug.Log("Hit offset: " + num2);
				Vector3 position = this.transform.position;
				position = new Vector3(position.x, position.y + num2, position.z);
				this.transform.position = position;
			}
			this.SpawnPos = this.transform.position;
			this.SpawnRot = this.transform.rotation;
		}
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x000DEA24 File Offset: 0x000DCC24
	private void Start()
	{
		if (this.renderer && this.renderer.material.mainTexture)
		{
			this.renderer.material.mainTexture.mipMapBias = MipmapBiasTextures.CurrentMipMapBias;
		}
		Singleton<CameraController>.Instance.bOblong = this.bOblong;
		this.UpdateNames();
		if (Network.isServer)
		{
			if (this.CustomHands)
			{
				this.DestroyDefaultHands();
				base.networkView.RPC(RPCTarget.All, new Action(this.InitHands));
			}
			else
			{
				this.SpawnDefaultHands();
			}
		}
		else
		{
			this.DestroyDefaultHands();
		}
		EventManager.OnHandZoneChange += this.OnHandZoneChange;
		EventManager.OnPlayersUpdate += this.UpdateNames;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnected;
		EventManager.TriggerChangeTable(base.gameObject);
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x000DEB00 File Offset: 0x000DCD00
	private void OnDestroy()
	{
		EventManager.OnHandZoneChange -= this.OnHandZoneChange;
		EventManager.OnPlayersUpdate -= this.UpdateNames;
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnected;
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x000DEB38 File Offset: 0x000DCD38
	public static string GetTablePrefabName(string tableName)
	{
		if (tableName == "Square" || tableName == "Hexagon" || tableName == "Octagon" || tableName == "Poker" || tableName == "None")
		{
			tableName = "Table_" + tableName;
		}
		else if (tableName == "Round")
		{
			tableName = "Table_Circular";
		}
		else if (tableName == "Rectangle")
		{
			tableName = "Table_RPG";
		}
		else if (tableName == "Custom Rectangle")
		{
			tableName = "Table_Custom";
		}
		else if (tableName == "Round Glass")
		{
			tableName = "Table_Glass";
		}
		else if (tableName == "Custom Square")
		{
			tableName = "Table_Custom_Square";
		}
		else if (tableName == "Round Plastic")
		{
			tableName = "Table_Plastic";
		}
		return tableName;
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x000DEC1B File Offset: 0x000DCE1B
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			base.networkView.RPC(player, new Action(this.InitHands));
		}
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x000DEC3C File Offset: 0x000DCE3C
	private void DestroyHandZones()
	{
		List<HandZone> handZones = HandZone.GetHandZones();
		for (int i = handZones.Count - 1; i >= 0; i--)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(handZones[i]);
		}
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x000DEC73 File Offset: 0x000DCE73
	private void OnHandZoneChange(HandZone handZone)
	{
		this.PositionNames();
	}

	// Token: 0x06001F41 RID: 8001 RVA: 0x000DEC7C File Offset: 0x000DCE7C
	private void PositionNames()
	{
		ManagerPhysicsObject instance = NetworkSingleton<ManagerPhysicsObject>.Instance;
		if (!instance)
		{
			return;
		}
		for (int i = 0; i < this.TextParent.childCount; i++)
		{
			GameObject gameObject = this.TextParent.GetChild(i).gameObject;
			string b = gameObject.name.Replace("Text ", "");
			bool flag = false;
			foreach (HandZone handZone in HandZone.GetHandZones())
			{
				if (handZone && handZone.TriggerLabel == b)
				{
					Vector3 vector2;
					GameObject gameObject2;
					Vector3 vector = instance.StaticSurfacePointBelowWorldPos(handZone.transform.position, out vector2, out gameObject2);
					if (this.createdStaticHandTransforms.Count == 0)
					{
						vector.y += this.NameHeightOffset;
						vector.y += 0.05f;
						gameObject.transform.position = vector;
						gameObject.transform.eulerAngles = new Vector3(90f, handZone.transform.eulerAngles.y - 180f, 0f);
					}
					else
					{
						Transform transform = null;
						float num = float.MaxValue;
						foreach (Transform transform2 in this.createdStaticHandTransforms)
						{
							float num2 = Vector3.Distance(transform2.position, vector);
							if (num2 < num)
							{
								transform = transform2;
								num = num2;
							}
						}
						if (transform != null)
						{
							gameObject.transform.localPosition = transform.localPosition;
							gameObject.transform.localEulerAngles = transform.localEulerAngles;
						}
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				gameObject.transform.position = new Vector3(0f, 1000f, 0f);
			}
		}
	}

	// Token: 0x06001F42 RID: 8002 RVA: 0x000DEEA0 File Offset: 0x000DD0A0
	private void ResetCameraRotation()
	{
		if (this.bResetCameraRotation)
		{
			Singleton<CameraController>.Instance.ResetCameraRotation();
			this.bResetCameraRotation = false;
		}
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x000DEEBC File Offset: 0x000DD0BC
	private void SpawnDefaultHands()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < this.HandsParent.childCount; i++)
		{
			list.Add(this.HandsParent.GetChild(i));
		}
		for (int j = 0; j < list.Count; j++)
		{
			HandTransformState handTransform = this.GetHandTransform(list[j]);
			this.LoadHand(handTransform);
		}
		this.DestroyDefaultHands();
		base.networkView.RPC(RPCTarget.All, new Action(this.InitHands));
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x000DEF3C File Offset: 0x000DD13C
	private void DestroyDefaultHands()
	{
		int childCount = this.HandsParent.childCount;
		for (int i = 0; i < childCount; i++)
		{
			UnityEngine.Object.Destroy(this.HandsParent.GetChild(i).gameObject);
		}
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x000DEF78 File Offset: 0x000DD178
	public void LoadHands(List<HandTransformState> HandTransforms)
	{
		if (HandTransforms == null)
		{
			return;
		}
		this.DestroyDefaultHands();
		List<HandTransformState> list = new List<HandTransformState>();
		foreach (HandTransformState handTransformState in HandTransforms)
		{
			if (!list.Contains(handTransformState))
			{
				foreach (HandTransformState handTransformState2 in HandTransforms)
				{
					if (handTransformState != handTransformState2 && !list.Contains(handTransformState2) && handTransformState.Transform.Position().ToString("F") == handTransformState2.Transform.Position().ToString("F") && handTransformState.Transform.Rotation().ToString("F") == handTransformState2.Transform.Rotation().ToString("F") && handTransformState.Transform.Scale().ToString("F") == handTransformState2.Transform.Scale().ToString("F") && handTransformState.Color == handTransformState2.Color)
					{
						list.Add(handTransformState2);
					}
				}
			}
		}
		foreach (HandTransformState item in list)
		{
			if (HandTransforms.Contains(item))
			{
				HandTransforms.Remove(item);
			}
		}
		foreach (HandTransformState hts in HandTransforms)
		{
			this.LoadHand(hts);
		}
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x000DF1C0 File Offset: 0x000DD3C0
	[Remote(Permission.Server)]
	private void InitHands()
	{
		Wait.Frames(delegate
		{
			this.ResetCameraRotation();
			this.PositionNames();
			HandZone.ResetHandZones();
		}, 1);
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x000DF1D8 File Offset: 0x000DD3D8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void LoadHand(HandTransformState HTS)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<HandTransformState>(RPCTarget.Server, new Action<HandTransformState>(this.LoadHand), HTS);
			return;
		}
		HandZone component = Network.Instantiate(NetworkSingleton<GameMode>.Instance.HandTrigger, HTS.Transform.Position(), Quaternion.Euler(HTS.Transform.Rotation()), default(NetworkPlayer)).GetComponent<HandZone>();
		component.TriggerLabel = HTS.Color;
		component.NPO.SetScale(HTS.Transform.Scale(), false);
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x000DF260 File Offset: 0x000DD460
	public List<HandTransformState> GetHandTransforms()
	{
		List<HandTransformState> list = new List<HandTransformState>();
		foreach (HandZone handZone in HandZone.GetHandZones())
		{
			HandTransformState handTransform = this.GetHandTransform(handZone.transform);
			list.Add(handTransform);
		}
		return list;
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x000DF2C8 File Offset: 0x000DD4C8
	public HandTransformState GetHandTransform(Transform handt)
	{
		GameObject gameObject = handt.gameObject;
		HandTransformState handTransformState = new HandTransformState();
		Transform parent = handt.parent;
		if (parent)
		{
			handt.parent = null;
		}
		handTransformState.Transform = new TransformState(handt);
		handt.parent = parent;
		handTransformState.Color = gameObject.GetComponent<HandZone>().TriggerLabel;
		return handTransformState;
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x000DF31C File Offset: 0x000DD51C
	private void Update()
	{
		if (this.RenderersToHideForAbstract.Length != 0 && this.RenderersToHideForAbstract[0].enabled == TableScript.PURE_MODE)
		{
			this.UpdatePure();
		}
		if (!this.renderer)
		{
			return;
		}
		if (this.TableMat.Length == 0)
		{
			return;
		}
		if (Network.isServer)
		{
			string gameName = NetworkSingleton<GameOptions>.Instance.GameName;
			if (gameName == "Poker" || gameName == "Cards" || gameName == "Felt")
			{
				this.TableTextureInt = 1;
			}
			else
			{
				this.TableTextureInt = 0;
			}
		}
		if (this.TableTextureInt != this.PrevTextureInt)
		{
			this.PrevTextureInt = this.TableTextureInt;
			Material[] materials = base.gameObject.GetComponent<Renderer>().materials;
			materials[0] = this.TableMat[this.TableTextureInt];
			base.gameObject.GetComponent<Renderer>().materials = materials;
			if (this.TableTextureInt == 1)
			{
				Transform transform = base.gameObject.transform.Find("Collision");
				for (int i = 0; i < transform.childCount; i++)
				{
					transform.GetChild(i).gameObject.AddMissingComponent<SoundMaterial>().soundMaterialType = SoundMaterialType.FeltSurface;
					transform.GetChild(i).gameObject.GetComponent<BoxCollider>().material = this.FeltPhysicsMat;
				}
				return;
			}
			Transform transform2 = base.gameObject.transform.Find("Collision");
			for (int j = 0; j < transform2.childCount; j++)
			{
				transform2.GetChild(j).gameObject.AddMissingComponent<SoundMaterial>().soundMaterialType = SoundMaterialType.WoodSurface;
				transform2.GetChild(j).gameObject.GetComponent<BoxCollider>().material = this.WoodPhysicsMat;
			}
		}
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x000DF4C8 File Offset: 0x000DD6C8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default, sendType = SendType.ReliableNoDelay)]
	public void FlipTable()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.FlipTable));
			return;
		}
		Color color = NetworkSingleton<PlayerManager>.Instance.ColourFromID((int)Network.sender.id);
		string text = Colour.LabelFromColour(color);
		string text2 = NetworkSingleton<PlayerManager>.Instance.NameFromColour(color);
		if (!PermissionsOptions.CheckAllowSender(PermissionsOptions.options.TableFlip))
		{
			NetworkSingleton<AchievementManager>.Instance.SetAchievement(Network.sender, "ACH_FLIP_TABLE_FAILED");
			Chat.SendChat(string.Concat(new object[]
			{
				text2,
				" tries to flip the table but is too weak! (Strength check failed 1d20 = ",
				UnityEngine.Random.Range(1, 20),
				")"
			}), text);
			return;
		}
		Singleton<StatsAchievements>.Instance.RestraintTimeHolder = Time.time;
		Chat.SendChat(text2 + " has flipped the table in a rage! (Strength check passed 1d20 = 20)", text);
		this.TableFlip(text);
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06001F4C RID: 8012 RVA: 0x000DF5AC File Offset: 0x000DD7AC
	// (set) Token: 0x06001F4D RID: 8013 RVA: 0x000DF5B4 File Offset: 0x000DD7B4
	public bool IsTableFlipping { get; private set; }

	// Token: 0x06001F4E RID: 8014 RVA: 0x000DF5C0 File Offset: 0x000DD7C0
	public void TableFlip(string stringcolor)
	{
		if (!this.IsTableFlipping)
		{
			NetworkSingleton<SaveManager>.Instance.SaveRewindState();
		}
		this.IsTableFlipping = true;
		OutOfBounds.bStopTeleport = true;
		HandZone.ResetHandZones();
		foreach (HandZone handZone in HandZone.GetHandZones())
		{
			handZone.BoxCollider.enabled = false;
		}
		NetworkSingleton<NetworkUI>.Instance.GetComponent<SoundScript>().TableFlipSound();
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			if (!networkPhysicsObject.handZone)
			{
				networkPhysicsObject.IsLocked = false;
				networkPhysicsObject.ResetIdleFreeze();
				networkPhysicsObject.rigidbody.isKinematic = false;
				networkPhysicsObject.rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
				networkPhysicsObject.rigidbody.useGravity = true;
				networkPhysicsObject.SetCollision(true);
				networkPhysicsObject.FixConcave();
			}
		}
		GameObject gameObject = HandZone.GetHand(stringcolor, 0);
		if (!gameObject)
		{
			gameObject = HandZone.GetStartHand();
		}
		if (!gameObject)
		{
			gameObject = base.gameObject;
		}
		this.NPO.rigidbody.isKinematic = false;
		this.NPO.rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		this.NPO.IsLocked = false;
		this.NPO.FixConcave();
		Vector3 vector = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
		Vector3 position = base.gameObject.transform.position;
		Vector3 position2 = gameObject.transform.position;
		this.NPO.rigidbody.AddForce(new Vector3(position.x - position2.x, 10f, position.z - position2.z), ForceMode.VelocityChange);
		this.NPO.rigidbody.AddTorque(new Vector3(position.z - position2.z + vector.x, vector.y, position.x - position2.x + vector.z) / 10f, ForceMode.VelocityChange);
		base.InvokeRepeating("Freeze", 0f, 0.5f);
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x000DF828 File Offset: 0x000DDA28
	private void Freeze()
	{
		if (Vector3.Distance(this.NPO.rigidbody.position, this.SpawnPos) > 1750f)
		{
			this.NPO.IsLocked = true;
			this.NPO.rigidbody.isKinematic = true;
			base.CancelInvoke("Freeze");
		}
	}

	// Token: 0x06001F50 RID: 8016 RVA: 0x000DF880 File Offset: 0x000DDA80
	public void Reset()
	{
		this.IsTableFlipping = false;
		OutOfBounds.bStopTeleport = false;
		foreach (HandZone handZone in HandZone.GetHandZones())
		{
			handZone.BoxCollider.enabled = true;
		}
		this.transform.position = this.SpawnPos;
		this.transform.rotation = this.SpawnRot;
		this.NPO.rigidbody.position = this.SpawnPos;
		this.NPO.rigidbody.rotation = this.SpawnRot;
		this.NPO.rigidbody.isKinematic = true;
		this.NPO.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		this.NPO.IsLocked = true;
		this.NPO.FixConcave();
		base.CancelInvoke("Freeze");
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x000DF974 File Offset: 0x000DDB74
	private void UpdateNames()
	{
		for (int i = 0; i < Colour.HandPlayerLabels.Length; i++)
		{
			string text = Colour.HandPlayerLabels[i];
			Color color = Colour.ColourFromLabel(text);
			bool flag = NetworkSingleton<PlayerManager>.Instance.ColourInUse(color);
			Transform transform = this.TextParent.Find("Text " + text);
			if (transform)
			{
				TextMesh component = transform.GetComponent<TextMesh>();
				if (flag)
				{
					component.fontSize = 70;
					component.text = NetworkSingleton<PlayerManager>.Instance.NameFromColour(text);
					this.textRenderer = transform.GetComponent<Renderer>();
					this.textRenderer.material.color = color;
					while (this.textRenderer.bounds.size.x * Mathf.Abs(transform.transform.right.normalized.x) + this.textRenderer.bounds.size.z * Mathf.Abs(transform.transform.right.normalized.z) > 12f)
					{
						component.fontSize--;
					}
				}
				else
				{
					component.text = "";
				}
			}
		}
	}

	// Token: 0x06001F52 RID: 8018 RVA: 0x000DFAC8 File Offset: 0x000DDCC8
	public void UpdatePure()
	{
		for (int i = 0; i < this.RenderersToHideForAbstract.Length; i++)
		{
			this.RenderersToHideForAbstract[i].enabled = !TableScript.PURE_MODE;
		}
		for (int j = 0; j < this.CustomRenderersToHideForAbstract.Length; j++)
		{
			this.CustomRenderersToHideForAbstract[j].enabled = (!TableScript.PURE_MODE || !TableScript.PURE_HIDE_CUSTOM);
		}
		if (TableScript.PURE_MODE)
		{
			Singleton<SystemConsole>.Instance.PureModePrimaryMaterial.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureTableA];
			Singleton<SystemConsole>.Instance.PureModeSecondaryMaterial.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureTableB];
			Singleton<SystemConsole>.Instance.PureModeSplashMaterial.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSplash];
			Singleton<SystemConsole>.Instance.PureModePrimaryMaterial.SetColor("_SpecColor", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureTableASpecular]);
			Singleton<SystemConsole>.Instance.PureModeSecondaryMaterial.SetColor("_SpecColor", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureTableBSpecular]);
			Singleton<SystemConsole>.Instance.PureModeSplashMaterial.SetColor("_SpecColor", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSplashSpecular]);
			Singleton<CameraManager>.Instance.PureSkyboxMaterial.SetColor("_SkyColor1", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSkyAbove]);
			Singleton<CameraManager>.Instance.PureSkyboxMaterial.SetFloat("_SkyExponent1", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSkyAbove].a * 25.5f);
			Singleton<CameraManager>.Instance.PureSkyboxMaterial.SetColor("_SkyColor2", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSkyHorizon]);
			Singleton<CameraManager>.Instance.PureSkyboxMaterial.SetFloat("_SkyIntensity", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSkyHorizon].a * 25.5f);
			Singleton<CameraManager>.Instance.PureSkyboxMaterial.SetColor("_SkyColor3", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSkyBelow]);
			Singleton<CameraManager>.Instance.PureSkyboxMaterial.SetFloat("_SkyExponent2", Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.PureSkyBelow].a * 25.5f);
		}
		for (int k = 0; k < this.ObjectsToShowForAbstract.Length; k++)
		{
			this.ObjectsToShowForAbstract[k].SetActive(TableScript.PURE_MODE);
		}
		for (int l = 0; l < this.CustomObjectsToShowForAbstract.Length; l++)
		{
			this.CustomObjectsToShowForAbstract[l].SetActive(TableScript.PURE_MODE && TableScript.PURE_HIDE_CUSTOM);
		}
		if (TableScript.PURE_MODE)
		{
			RenderSettings.skybox = Singleton<CameraManager>.Instance.PureSkyboxMaterial;
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.ambientLight = new Color(0.6f, 0.6f, 0.6f);
			DynamicGI.UpdateEnvironment();
			for (int m = 0; m < Singleton<CameraManager>.Instance.AllCameras.Length; m++)
			{
				Camera camera = Singleton<CameraManager>.Instance.AllCameras[m];
				SSAOPro component = camera.GetComponent<SSAOPro>();
				if (component)
				{
					component.enabled = false;
				}
				PostProcessingLayering component2 = camera.GetComponent<PostProcessingLayering>();
				if (component2)
				{
					component2.enabled = false;
				}
				PostProcessLayer component3 = camera.GetComponent<PostProcessLayer>();
				if (component3)
				{
					component3.enabled = false;
				}
			}
			return;
		}
		RenderSettings.skybox = Singleton<CameraManager>.Instance.DefaultSkyboxMaterial;
		RenderSettings.ambientMode = AmbientMode.Skybox;
		DynamicGI.UpdateEnvironment();
		for (int n = 0; n < Singleton<CameraManager>.Instance.AllCameras.Length; n++)
		{
			Camera camera2 = Singleton<CameraManager>.Instance.AllCameras[n];
			bool enabled = true;
			if (camera2 == Singleton<CameraManager>.Instance.SpectatorCamera)
			{
				enabled = SystemConsole.SpectatorPostProcessing;
			}
			SSAOPro component4 = camera2.GetComponent<SSAOPro>();
			if (component4)
			{
				component4.enabled = enabled;
			}
			PostProcessingLayering component5 = camera2.GetComponent<PostProcessingLayering>();
			if (component5)
			{
				component5.enabled = enabled;
			}
			PostProcessLayer component6 = camera2.GetComponent<PostProcessLayer>();
			if (component6)
			{
				component6.enabled = enabled;
			}
		}
	}

	// Token: 0x06001F53 RID: 8019 RVA: 0x000DFEE6 File Offset: 0x000DE0E6
	public static void UpdatePureMode()
	{
		if (NetworkSingleton<ManagerPhysicsObject>.Instance.TableScript)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.TableScript.UpdatePure();
		}
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x000025B8 File Offset: 0x000007B8
	public void Creation()
	{
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x000DFF08 File Offset: 0x000DE108
	public void Destruction()
	{
		if (Network.isServer)
		{
			this.DestroyHandZones();
		}
	}

	// Token: 0x04001310 RID: 4880
	public static bool RESET_CAMERA_ON_LOAD = true;

	// Token: 0x04001311 RID: 4881
	public static bool PURE_MODE = false;

	// Token: 0x04001312 RID: 4882
	public static bool PURE_FOG = false;

	// Token: 0x04001313 RID: 4883
	public static bool PURE_HIDE_CUSTOM = false;

	// Token: 0x04001314 RID: 4884
	public static float PURE_PRIMARY_SPECULAR_INTENSITY = 0f;

	// Token: 0x04001315 RID: 4885
	public static float PURE_PRIMARY_SPECULAR_SHARPNESS = 2f;

	// Token: 0x04001316 RID: 4886
	public static float PURE_SECONDARY_SPECULAR_INTENSITY = 0f;

	// Token: 0x04001317 RID: 4887
	public static float PURE_SECONDARY_SPECULAR_SHARPNESS = 2f;

	// Token: 0x04001318 RID: 4888
	public static float PURE_SPLASH_SPECULAR_INTENSITY = 0f;

	// Token: 0x04001319 RID: 4889
	public static float PURE_SPLASH_SPECULAR_SHARPNESS = 2f;

	// Token: 0x0400131A RID: 4890
	public static Material SkyBoxMaterial;

	// Token: 0x0400131B RID: 4891
	public Material[] TableMat;

	// Token: 0x0400131C RID: 4892
	public PhysicMaterial WoodPhysicsMat;

	// Token: 0x0400131D RID: 4893
	public PhysicMaterial FeltPhysicsMat;

	// Token: 0x0400131E RID: 4894
	public Renderer[] RenderersToHideForAbstract;

	// Token: 0x0400131F RID: 4895
	public GameObject[] ObjectsToShowForAbstract;

	// Token: 0x04001320 RID: 4896
	public Renderer[] CustomRenderersToHideForAbstract;

	// Token: 0x04001321 RID: 4897
	public GameObject[] CustomObjectsToShowForAbstract;

	// Token: 0x04001322 RID: 4898
	public float NameHeightOffset;

	// Token: 0x04001323 RID: 4899
	private int _TableTextureInt;

	// Token: 0x04001324 RID: 4900
	private int PrevTextureInt;

	// Token: 0x04001325 RID: 4901
	private Vector3 SpawnPos = Vector3.zero;

	// Token: 0x04001326 RID: 4902
	private Quaternion SpawnRot = Quaternion.identity;

	// Token: 0x04001327 RID: 4903
	public bool bOblong;

	// Token: 0x04001328 RID: 4904
	private Renderer renderer;

	// Token: 0x04001329 RID: 4905
	private Transform HandsParent;

	// Token: 0x0400132A RID: 4906
	private Transform TextParent;

	// Token: 0x0400132B RID: 4907
	public List<Transform> StaticHandTransforms = new List<Transform>();

	// Token: 0x0400132C RID: 4908
	private List<Transform> createdStaticHandTransforms = new List<Transform>();

	// Token: 0x0400132D RID: 4909
	public const float FloorYPosition = 0.96f;

	// Token: 0x0400132F RID: 4911
	private new Transform transform;

	// Token: 0x04001330 RID: 4912
	private bool bResetCameraRotation = TableScript.RESET_CAMERA_ON_LOAD;

	// Token: 0x04001331 RID: 4913
	private Renderer textRenderer;

	// Token: 0x04001332 RID: 4914
	public bool CustomHands;
}
