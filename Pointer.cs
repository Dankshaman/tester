using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HighlightingSystem;
using NewNet;
using RTEditor;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class Pointer : NetworkBehavior
{
	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x060018F9 RID: 6393 RVA: 0x000A7ECA File Offset: 0x000A60CA
	// (set) Token: 0x060018FA RID: 6394 RVA: 0x000A7EE9 File Offset: 0x000A60E9
	public int ID
	{
		get
		{
			if (!NetworkSingleton<NetworkUI>.Instance.bHotseat)
			{
				return this._ID;
			}
			return NetworkSingleton<NetworkUI>.Instance.CurrentHotseat;
		}
		private set
		{
			if (value != this._ID)
			{
				this._ID = value;
			}
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x060018FB RID: 6395 RVA: 0x000A7EFB File Offset: 0x000A60FB
	// (set) Token: 0x060018FC RID: 6396 RVA: 0x000A7F03 File Offset: 0x000A6103
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private bool bTap
	{
		get
		{
			return this._bTap;
		}
		set
		{
			if (value != this._bTap)
			{
				this._bTap = value;
				base.DirtySync("bTap");
			}
		}
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x060018FD RID: 6397 RVA: 0x000A7F20 File Offset: 0x000A6120
	// (set) Token: 0x060018FE RID: 6398 RVA: 0x000A7F28 File Offset: 0x000A6128
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private bool bRaise
	{
		get
		{
			return this._bRaise;
		}
		set
		{
			if (value != this._bRaise)
			{
				this._bRaise = value;
				base.DirtySync("bRaise");
			}
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x060018FF RID: 6399 RVA: 0x000A7F45 File Offset: 0x000A6145
	// (set) Token: 0x06001900 RID: 6400 RVA: 0x000A7F4D File Offset: 0x000A614D
	public Colour PointerColour { get; private set; }

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06001901 RID: 6401 RVA: 0x000A7F56 File Offset: 0x000A6156
	// (set) Token: 0x06001902 RID: 6402 RVA: 0x000A7F5E File Offset: 0x000A615E
	public Colour PointerDarkColour { get; private set; }

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06001903 RID: 6403 RVA: 0x000A7F67 File Offset: 0x000A6167
	// (set) Token: 0x06001904 RID: 6404 RVA: 0x000A7F6F File Offset: 0x000A616F
	public bool bSelecting { get; private set; }

	// Token: 0x06001905 RID: 6405 RVA: 0x000A7F78 File Offset: 0x000A6178
	public override void OnSync()
	{
		if (this.PointerMeshIndex != -1 && this.ReferenceFollower)
		{
			this.ReferenceFollower.GetComponent<MeshLerpToObject>().SetPointerMesh(this.PointerMeshIndex, (this.PointerTypeIndex < 0) ? 0 : this.PointerTypeIndex, false);
		}
		this.ThisCollider.isTrigger = (this.PointerMeshIndex != 3);
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06001906 RID: 6406 RVA: 0x000A7FDB File Offset: 0x000A61DB
	// (set) Token: 0x06001907 RID: 6407 RVA: 0x000A7FE4 File Offset: 0x000A61E4
	public GameObject FirstGrabbedObject
	{
		get
		{
			return this.firstGrabbedObject;
		}
		set
		{
			this.firstGrabbedObject = value;
			if (this.firstGrabbedObject)
			{
				this.FirstGrabbedNPO = this.firstGrabbedObject.GetComponent<NetworkPhysicsObject>();
				if (this.FirstGrabbedNPO)
				{
					this.pickedUpMeasuredObject = this.FirstGrabbedNPO.ShowRulerWhenHeld;
					if (Network.isClient)
					{
						this.FirstGrabbedNPO.PickedUpPosition = this.FirstGrabbedNPO.transform.position;
						this.FirstGrabbedNPO.PickedUpRotation = this.FirstGrabbedNPO.transform.eulerAngles;
						return;
					}
				}
			}
			else
			{
				this.FirstGrabbedNPO = null;
			}
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06001908 RID: 6408 RVA: 0x000A8079 File Offset: 0x000A6279
	// (set) Token: 0x06001909 RID: 6409 RVA: 0x000A8081 File Offset: 0x000A6281
	public NetworkPhysicsObject FirstGrabbedNPO { get; private set; }

	// Token: 0x0600190A RID: 6410 RVA: 0x000A808A File Offset: 0x000A628A
	public void UpdateFirstGrabbedNPO(NetworkPhysicsObject npo)
	{
		base.networkView.RPC<NetworkPhysicsObject>(Network.IdToNetworkPlayer(this.ID), new Action<NetworkPhysicsObject>(this.RPCUpdateFirstGrabbedNPO), npo);
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x000A80AF File Offset: 0x000A62AF
	[Remote(SendType.ReliableNoDelay)]
	private void RPCUpdateFirstGrabbedNPO(NetworkPhysicsObject npo)
	{
		this.FirstGrabbedObject = npo.gameObject;
		if (ObjectPositioningVisualizer.VisualizeGrabbedObjects && this.GrabbedObjects.Count <= 1)
		{
			Singleton<ObjectPositioningVisualizer>.Instance.AddVisualizer(this.FirstGrabbedNPO);
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x0600190C RID: 6412 RVA: 0x000A80E2 File Offset: 0x000A62E2
	// (set) Token: 0x0600190D RID: 6413 RVA: 0x000A80EA File Offset: 0x000A62EA
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int PointerMeshIndex
	{
		get
		{
			return this._PointerMeshIndex;
		}
		set
		{
			if (value != this._PointerMeshIndex)
			{
				this._PointerMeshIndex = value;
				base.DirtySync("PointerMeshIndex");
			}
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x0600190E RID: 6414 RVA: 0x000A8107 File Offset: 0x000A6307
	// (set) Token: 0x0600190F RID: 6415 RVA: 0x000A810F File Offset: 0x000A630F
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	private int PointerTypeIndex
	{
		get
		{
			return this._PointerTypeIndex;
		}
		set
		{
			if (value != this._PointerTypeIndex)
			{
				this._PointerTypeIndex = value;
				base.DirtySync("PointerTypeIndex");
			}
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06001910 RID: 6416 RVA: 0x000A812C File Offset: 0x000A632C
	// (set) Token: 0x06001911 RID: 6417 RVA: 0x000A8134 File Offset: 0x000A6334
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public NetworkView HoverView
	{
		get
		{
			return this._HoverView;
		}
		private set
		{
			if (value != this._HoverView)
			{
				this._HoverView = value;
				base.DirtySync("HoverView");
			}
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06001912 RID: 6418 RVA: 0x000A8156 File Offset: 0x000A6356
	// (set) Token: 0x06001913 RID: 6419 RVA: 0x000A815E File Offset: 0x000A635E
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public Vector3 StartLineVector
	{
		get
		{
			return this._StartLineVector;
		}
		set
		{
			if (value != this._StartLineVector)
			{
				this._StartLineVector = value;
				base.DirtySync("StartLineVector");
			}
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06001914 RID: 6420 RVA: 0x000A8180 File Offset: 0x000A6380
	// (set) Token: 0x06001915 RID: 6421 RVA: 0x000A8188 File Offset: 0x000A6388
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public Vector3 EndLineVector
	{
		get
		{
			return this._EndLineVector;
		}
		set
		{
			if (value != this._EndLineVector)
			{
				this._EndLineVector = value;
				base.DirtySync("EndLineVector");
			}
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06001916 RID: 6422 RVA: 0x000A81AA File Offset: 0x000A63AA
	// (set) Token: 0x06001917 RID: 6423 RVA: 0x000A81B2 File Offset: 0x000A63B2
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool LineVectorInComponents
	{
		get
		{
			return this._LineVectorInComponents;
		}
		set
		{
			if (value != this._LineVectorInComponents)
			{
				this._LineVectorInComponents = value;
				base.DirtySync("LineVectorInComponents");
			}
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06001918 RID: 6424 RVA: 0x000A81CF File Offset: 0x000A63CF
	// (set) Token: 0x06001919 RID: 6425 RVA: 0x000A81D7 File Offset: 0x000A63D7
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public bool MeasuringXZ
	{
		get
		{
			return this._MeasuringXZ;
		}
		set
		{
			if (value != this._MeasuringXZ)
			{
				this._MeasuringXZ = value;
				base.DirtySync("MeasuringXZ");
			}
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x0600191A RID: 6426 RVA: 0x000A81F4 File Offset: 0x000A63F4
	// (set) Token: 0x0600191B RID: 6427 RVA: 0x000A81FC File Offset: 0x000A63FC
	public List<NetworkPhysicsObject> HighLightedObjects { get; set; } = new List<NetworkPhysicsObject>();

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x0600191C RID: 6428 RVA: 0x000A8205 File Offset: 0x000A6405
	// (set) Token: 0x0600191D RID: 6429 RVA: 0x000A820D File Offset: 0x000A640D
	public GameObject InfoObject { get; private set; }

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x0600191E RID: 6430 RVA: 0x000A8216 File Offset: 0x000A6416
	// (set) Token: 0x0600191F RID: 6431 RVA: 0x000A821E File Offset: 0x000A641E
	public GameObject InfoHiddenZoneGO { get; private set; }

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06001920 RID: 6432 RVA: 0x000A8227 File Offset: 0x000A6427
	// (set) Token: 0x06001921 RID: 6433 RVA: 0x000A822F File Offset: 0x000A642F
	public GameObject InfoRandomizeZoneGO { get; private set; }

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06001922 RID: 6434 RVA: 0x000A8238 File Offset: 0x000A6438
	// (set) Token: 0x06001923 RID: 6435 RVA: 0x000A8240 File Offset: 0x000A6440
	public GameObject InfoHandObject { get; private set; }

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06001924 RID: 6436 RVA: 0x000A8249 File Offset: 0x000A6449
	// (set) Token: 0x06001925 RID: 6437 RVA: 0x000A8251 File Offset: 0x000A6451
	public GameObject InfoFogOfWarZoneGO { get; private set; }

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06001926 RID: 6438 RVA: 0x000A825A File Offset: 0x000A645A
	// (set) Token: 0x06001927 RID: 6439 RVA: 0x000A8262 File Offset: 0x000A6462
	public GameObject InfoLayoutZoneGO { get; private set; }

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06001928 RID: 6440 RVA: 0x000A826C File Offset: 0x000A646C
	// (set) Token: 0x06001929 RID: 6441 RVA: 0x000A82E2 File Offset: 0x000A64E2
	private GameObject HoverObject
	{
		get
		{
			if (VRHMD.isVR)
			{
				if (VRTrackedController.rightVRTrackedController && VRTrackedController.rightVRTrackedController.HoverObject != null)
				{
					return VRTrackedController.rightVRTrackedController.HoverObject.gameObject;
				}
				if (VRTrackedController.leftVRTrackedController && VRTrackedController.leftVRTrackedController.HoverObject != null)
				{
					return VRTrackedController.leftVRTrackedController.HoverObject.gameObject;
				}
			}
			return this._HoverObject;
		}
		set
		{
			this._HoverObject = value;
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x0600192A RID: 6442 RVA: 0x000A82EB File Offset: 0x000A64EB
	// (set) Token: 0x0600192B RID: 6443 RVA: 0x000A82F4 File Offset: 0x000A64F4
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public PointerMode CurrentPointerMode
	{
		get
		{
			return this._currentPointerMode;
		}
		set
		{
			if (PermissionsOptions.CheckIfAllowedInMode(value, this.ID))
			{
				if (this._currentPointerMode != value)
				{
					base.DirtySync("CurrentPointerMode");
				}
				if (base.networkView.isMine)
				{
					if (this._currentPointerMode != this.previousPointerMode)
					{
						this.previousPointerMode = this._currentPointerMode;
					}
					this.previousPointerModeWithDup = this._currentPointerMode;
					this._currentPointerMode = value;
					EventManager.TriggerChangePointerMode(this._currentPointerMode);
					return;
				}
				this._currentPointerMode = value;
			}
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x0600192C RID: 6444 RVA: 0x000A8370 File Offset: 0x000A6570
	public PointerMode PreviousPointerMode
	{
		get
		{
			return this.previousPointerMode;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x0600192D RID: 6445 RVA: 0x000A8378 File Offset: 0x000A6578
	public PointerMode PreviousPointerModeWithDup
	{
		get
		{
			return this.previousPointerModeWithDup;
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x0600192E RID: 6446 RVA: 0x000A8380 File Offset: 0x000A6580
	// (set) Token: 0x0600192F RID: 6447 RVA: 0x000A8388 File Offset: 0x000A6588
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public int RotationSnap
	{
		get
		{
			return this.rotationSnap;
		}
		set
		{
			if (value != this.rotationSnap)
			{
				this.rotationSnap = value;
				if (Network.sender == Network.player && base.networkView.isMine)
				{
					PlayerPrefs.SetInt("PointerRotationSnap", value);
				}
				base.DirtySync("RotationSnap");
			}
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06001930 RID: 6448 RVA: 0x000A83D9 File Offset: 0x000A65D9
	// (set) Token: 0x06001931 RID: 6449 RVA: 0x000A83E4 File Offset: 0x000A65E4
	[Sync(Permission.Owner, null, SerializationMethod.Default, false)]
	public float RaiseHeight
	{
		get
		{
			return this.raiseHeight;
		}
		set
		{
			if (value != this.raiseHeight)
			{
				this.raiseHeight = value;
				if (Network.sender == Network.player && base.networkView.isMine)
				{
					PlayerPrefs.SetFloat("PointerRaiseHeight", value);
				}
				base.DirtySync("RaiseHeight");
			}
		}
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x000A8438 File Offset: 0x000A6638
	private void Awake()
	{
		this.PointerColourFlag = Colour.FlagFromLabel(this.PointerColorLabel);
		this.PointerColour = Colour.ColourFromLabel(this.PointerColorLabel);
		this.PointerDarkColour = Colour.DarkenedFromColour(this.PointerColour);
		this.ReferenceFollower = UnityEngine.Object.Instantiate<GameObject>(this.PointerMeshFollower);
		this.ReferenceFollower.GetComponent<MeshLerpToObject>().FollowObject = base.gameObject;
		this.ReferenceFollower.GetComponent<MeshLerpToObject>().PointerColor = this.PointerColour;
		this.ThisCollider = base.GetComponent<CapsuleCollider>();
		this.rigidbody = base.GetComponent<Rigidbody>();
		this.ID = (int)base.networkView.owner.id;
		this.pointerSyncs = base.GetComponentInChildren<PointerSyncs>();
		if (base.networkView.isMine)
		{
			if (PlayerPrefs.HasKey("PointerRotationSnap"))
			{
				this.RotationSnap = PlayerPrefs.GetInt("PointerRotationSnap");
			}
			if (PlayerPrefs.HasKey("PointerRaiseHeight"))
			{
				this.RaiseHeight = PlayerPrefs.GetFloat("PointerRaiseHeight");
			}
		}
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x000A853C File Offset: 0x000A673C
	private void OnEnable()
	{
		if (base.networkView.isMine)
		{
			PlayerScript.Pointer = base.gameObject;
			PlayerScript.PointerScript = this;
		}
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000A855C File Offset: 0x000A675C
	private void OnDisable()
	{
		if (base.networkView.isMine)
		{
			if (PlayerScript.Pointer == base.gameObject)
			{
				PlayerScript.Pointer = null;
			}
			if (PlayerScript.PointerScript == this)
			{
				PlayerScript.PointerScript = null;
			}
		}
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000A8598 File Offset: 0x000A6798
	private void Start()
	{
		this.MainCamera = HoverScript.MainCamera;
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("LineObject"), Vector3.zero, Quaternion.identity);
			LineRenderer component = gameObject.GetComponent<LineRenderer>();
			component.material.color = this.PointerColour;
			component.numCapVertices = 6;
			component.enabled = false;
			gameObject.transform.parent = base.gameObject.transform;
			this.LineInstances.Add(component);
		}
		if (base.networkView.isMine)
		{
			this.UICameraObject = GameObject.Find("UICamera");
			this.HoverOverFacade = base.transform.Find("HoverOverFacade").gameObject;
			this.HoverOverFacadeMesh = this.HoverOverFacade.GetComponent<MeshFilter>();
			if (!this.bShowHardwareCursor)
			{
				Cursor.visible = false;
			}
			if (!NetworkSingleton<NetworkUI>.Instance.bHotseat)
			{
				Singleton<CameraController>.Instance.ResetCameraRotation();
			}
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.RegisterPointer(this);
		this.LastArrowSpawnTime = Time.time;
		this.NetworkInstance = NetworkSingleton<NetworkUI>.Instance;
		if (Network.isServer)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.ShakeDetectorObject, base.transform.position, Quaternion.identity).transform.parent = base.transform;
		}
		if (base.networkView.isMine)
		{
			NetworkSingleton<NetworkUI>.Instance.EnableGUIEndHandRevealButtonIfApplicableForColor(this.PointerColorLabel);
		}
		Wait.Frames(delegate
		{
			EventManager.TriggerChangePlayerColor(Colour.ColourFromLabel(this.PointerColorLabel), this.ID);
		}, 1);
		PermissionsOptions.OnPermissionUpdated += this.OnPermissionUpdated;
		EventManager.OnNetworkObjectDestroy += this.OnNetworkObjectDestroy;
		this.LineLabel = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.AddChild(NetworkSingleton<NetworkUI>.Instance.GUITooltipPrefab).GetComponentInChildren<UILabel>();
		this.LineLabel.gameObject.transform.parent.name = this.PointerColorLabel + " Line Text";
		this.LineLabelX = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.AddChild(NetworkSingleton<NetworkUI>.Instance.GUITooltipPrefab).GetComponentInChildren<UILabel>();
		this.LineLabelX.gameObject.transform.parent.name = this.PointerColorLabel + " Line Text X";
		this.LineLabelZ = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.AddChild(NetworkSingleton<NetworkUI>.Instance.GUITooltipPrefab).GetComponentInChildren<UILabel>();
		this.LineLabelZ.gameObject.transform.parent.name = this.PointerColorLabel + " Line Text Z";
		this.OnThemeChange();
		EventManager.OnUIThemeChange += this.OnThemeChange;
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x000A883C File Offset: 0x000A6A3C
	private void OnDestroy()
	{
		if (Network.isServer)
		{
			this.Release(this.ID, -1, -1, -1);
		}
		if (NetworkSingleton<ManagerPhysicsObject>.Instance)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.UnRegisterPointer(this);
		}
		if (NetworkSingleton<NetworkUI>.Instance)
		{
			if (base.networkView.isMine)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIEndHandRevealButton.SetActive(false);
			}
			EventManager.TriggerChangePlayerColor(Colour.Grey, this.ID);
		}
		this.ID = -1;
		this.ResetHighlight();
		Cursor.visible = true;
		foreach (LineRenderer obj in this.LineInstances)
		{
			UnityEngine.Object.Destroy(obj);
		}
		if (base.networkView.isMine)
		{
			this.ResetHiddenZoneObject();
			this.ResetRandomizeObject();
			this.ResetHandObject();
			this.ResetLayoutZoneObject();
			this.ResetFogOfWarObject();
			NetworkSingleton<NetworkUI>.Instance.GUIInventory.gameObject.SetActive(false);
			EventManager.TriggerChangePointerMode(PointerMode.None);
		}
		if (Singleton<UIObjectIndicatorManager>.Instance)
		{
			Singleton<UIObjectIndicatorManager>.Instance.CancelColor(this.PointerColorLabel);
		}
		PermissionsOptions.OnPermissionUpdated -= this.OnPermissionUpdated;
		EventManager.OnNetworkObjectDestroy -= this.OnNetworkObjectDestroy;
		EventManager.OnUIThemeChange -= this.OnThemeChange;
		if (this.LineLabel)
		{
			UnityEngine.Object.Destroy(this.LineLabel.transform.parent.gameObject);
		}
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000A89C8 File Offset: 0x000A6BC8
	private void OnPermissionUpdated()
	{
		if (!PermissionsOptions.CheckIfAllowedInMode(this.CurrentPointerMode, -1))
		{
			this.CurrentPointerMode = PointerMode.Grab;
		}
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x000A89E0 File Offset: 0x000A6BE0
	public void CheckFirstGrabbed()
	{
		if (base.networkView.isMine && !Network.isServer && zInput.GetButton("Grab", ControlType.All) && this.FirstGrabbedNPO == null)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
			{
				if (networkPhysicsObject.HeldByPlayerID == NetworkID.ID)
				{
					this.FirstGrabbedObject = networkPhysicsObject.gameObject;
					break;
				}
			}
		}
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x000A8A7C File Offset: 0x000A6C7C
	private void FixedUpdate()
	{
		this.PrevAutoRaise = this.AutoRaise;
		this.AutoRaise = 0f;
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x000A8A98 File Offset: 0x000A6C98
	private void Update()
	{
		this.bRotating = false;
		if (this.PointerMeshIndex == 0 || this.PointerMeshIndex == 1)
		{
			this.num_grabbed = 0;
		}
		if (Network.isServer && !this.ThisCollider.isTrigger && ServerOptions.isPhysicsSemi)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
			{
				if (Vector3.Distance(networkPhysicsObject.transform.position, base.transform.position) < 4f)
				{
					networkPhysicsObject.ResetIdleFreeze();
				}
			}
		}
		bool flag = this.StartLineVector != Vector3.zero && !NetworkSingleton<PlayerManager>.Instance.IsBlinded();
		bool flag2 = flag && this.CurrentPointerMode == PointerMode.Line;
		Vector3 vector = base.networkView.isMine ? HoverScript.PointerPosition : base.transform.position;
		this.LineLabel.gameObject.SetActive(flag2);
		this.LineLabelX.gameObject.SetActive(false);
		this.LineLabelZ.gameObject.SetActive(false);
		if (flag2)
		{
			Vector3 vector2 = this.MainCamera.WorldToScreenPoint(vector);
			if (vector2.z > 0f)
			{
				vector2.z = 0f;
			}
			vector2.y += 10f;
			this.LineLabel.transform.position = UICamera.mainCamera.ScreenToWorldPoint(vector2);
			this.LineLabel.transform.RoundLocalPosition();
			this.LineLabel.fontSize = Pointer.LineToolFontSize;
			float num2;
			float num3;
			float num4;
			float num = this.calculateMeasuredDistance(this.EndLineVector - this.StartLineVector, out num2, out num3, out num4);
			string str;
			if (LineScript.GRID_MEASUREMENTS)
			{
				str = "";
			}
			else if (LineScript.METRIC_MEASUREMENTS)
			{
				str = "cm";
			}
			else if (LineScript.MEASURE_MULTIPLIER != 1f)
			{
				str = "^";
			}
			else
			{
				str = "\"";
			}
			this.LineLabel.text = num.ToString("f1") + str;
			if (this.LineVectorInComponents)
			{
				this.LineLabel.gameObject.SetActive(false);
				this.LineLabelX.fontSize = Pointer.LineToolFontSize;
				Vector3 vector3;
				if (this.MeasuringXZ)
				{
					vector3 = this.MainCamera.WorldToScreenPoint(Vector3.Lerp(this.StartLineVector, new Vector3(this.EndLineVector.x, this.StartLineVector.y, this.StartLineVector.z), 0.5f));
					this.LineLabelX.text = num2.ToString("f1") + str;
					vector3.y += 10f;
				}
				else
				{
					vector3 = this.MainCamera.WorldToScreenPoint(Vector3.Lerp(this.StartLineVector, new Vector3(this.EndLineVector.x, this.StartLineVector.y, this.EndLineVector.z), 0.5f));
					this.LineLabelX.text = Mathf.Sqrt(num2 * num2 + num4 * num4).ToString("f1") + str;
				}
				if (vector3.z > 0f)
				{
					vector3.z = 0f;
				}
				this.LineLabelX.transform.position = UICamera.mainCamera.ScreenToWorldPoint(vector3);
				this.LineLabelX.transform.RoundLocalPosition();
				this.LineLabelX.gameObject.SetActive(true);
				this.LineLabelZ.fontSize = Pointer.LineToolFontSize;
				Vector3 vector4;
				if (this.MeasuringXZ)
				{
					vector4 = this.MainCamera.WorldToScreenPoint(Vector3.Lerp(this.EndLineVector, new Vector3(this.EndLineVector.x, this.StartLineVector.y, this.StartLineVector.z), 0.5f));
					this.LineLabelZ.text = num4.ToString("f1") + str;
					vector4.y += 10f;
				}
				else
				{
					vector4 = this.MainCamera.WorldToScreenPoint(Vector3.Lerp(this.EndLineVector, new Vector3(this.EndLineVector.x, this.StartLineVector.y, this.EndLineVector.z), 0.5f));
					this.LineLabelZ.text = num3.ToString("f1") + str;
					vector4.x += 30f;
				}
				if (vector4.z > 0f)
				{
					vector4.z = 0f;
				}
				this.LineLabelZ.transform.position = UICamera.mainCamera.ScreenToWorldPoint(vector4);
				this.LineLabelZ.transform.RoundLocalPosition();
				this.LineLabelZ.gameObject.SetActive(true);
			}
			if (VRHMD.isVR)
			{
				VRTrackedController.SetBothHoverText(this.LineLabel.text);
			}
		}
		if (Network.isServer)
		{
			if (this.HoverView != this.PrevHoverView)
			{
				if (this.HoverView == null)
				{
					EventManager.TriggerObjectHover(null, this.PointerColour);
				}
				else
				{
					EventManager.TriggerObjectHover(this.HoverView.gameObject, this.PointerColour);
				}
			}
			this.PrevHoverView = this.HoverView;
		}
		if (!base.networkView.isMine)
		{
			this.CalculateLineInstances(flag);
			this.ReferenceFollower.SetActive(base.transform.position != Vector3.zero && !flag);
			return;
		}
		this.PointerMeshIndex = 0;
		this.PointerTypeIndex = PlayerScript.PointerInt;
		if (this.HoverLockObject != HoverScript.HoverLockObject)
		{
			this.HoverView = (HoverScript.HoverLockObject ? HoverScript.HoverLockObject.GetComponent<NetworkView>() : null);
		}
		this.PrevHoverObject = this.HoverObject;
		this.HoverObject = null;
		this.HoverLockObject = null;
		HoverScript.GrabbedStackObject = null;
		this.HoverObject = HoverScript.HoverObject;
		this.HoverLockObject = HoverScript.HoverLockObject;
		Vector3 pointerPosition = HoverScript.PointerPosition;
		if (this.HoverObject && this.CurrentPointerMode == PointerMode.Grab && !UICamera.HoveredUIObject)
		{
			this.PointerMeshIndex = 1;
		}
		bool flag3 = !NetworkSingleton<Turns>.Instance.CanInteract(this.PointerColorLabel);
		if (flag3)
		{
			if (zInput.GetButtonDown("Grab", ControlType.All) && !UICamera.HoveredUIObject)
			{
				Chat.LogError("Cannot interact with objects because it's not your turn.", true);
			}
			Utilities.SetCursor(this.TextDefault, NetworkUI.HardwareCursorOffest, CursorMode.Auto);
			if (this.CurrentPointerMode != PointerMode.None)
			{
				this.bShowHardwareCursor = true;
				this.prevInteractivePointerMode = this.CurrentPointerMode;
				this.CurrentPointerMode = PointerMode.None;
				this.Release(this.ID, -1, -1, -1);
			}
		}
		else if (this.prevInteractivePointerMode != PointerMode.None)
		{
			this.bShowHardwareCursor = false;
			this.CurrentPointerMode = this.prevInteractivePointerMode;
			this.prevInteractivePointerMode = PointerMode.None;
		}
		if (!flag3 && (TTSInput.GetKeyDown(KeyCode.Mouse0) || TTSInput.GetKeyDown(KeyCode.Escape)))
		{
			try
			{
				TabletScript.TabletHasFocus = false;
				this.bShowHardwareCursor = false;
				foreach (NetworkPhysicsObject networkPhysicsObject2 in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
				{
					if (networkPhysicsObject2.tabletScript)
					{
						networkPhysicsObject2.tabletScript.HasFocus = false;
					}
				}
				if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || PermissionsOptions.options.Tablets)
				{
					if (TTSInput.GetKeyDown(KeyCode.Mouse0) && this.HoverLockObject && this.HoverLockObject.GetComponent<TabletScript>() && this.HoverLockObject.GetComponent<TabletScript>().CheckHit() && !UICamera.HoveredUIObject)
					{
						this.bShowHardwareCursor = true;
						TabletScript.TabletHasFocus = true;
						this.HoverLockObject.GetComponent<TabletScript>().HasFocus = true;
					}
					else if (TTSInput.GetKeyDown(KeyCode.Mouse0) && UICamera.HoveredUIObject && UICamera.HoveredUIObject.transform.root.gameObject.GetComponent<TabletScript>() && !UICamera.HoveredUIObject.GetComponent<UIInput>())
					{
						this.bShowHardwareCursor = true;
						TabletScript.TabletHasFocus = true;
						UICamera.HoveredUIObject.transform.root.gameObject.GetComponent<TabletScript>().HasFocus = true;
					}
					else if (TTSInput.GetKeyDown(KeyCode.Mouse0) && UICamera.HoveredUIObject && !UICamera.HoveredUIObject.GetComponent<UIInput>() && (UICamera.HoveredUIObject == NetworkSingleton<NetworkUI>.Instance.GUITabletWindow || UICamera.HoveredUIObject == NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().ThisUITextureView.gameObject || UICamera.HoveredUIObject.transform.parent.name == "TableTablet"))
					{
						this.bShowHardwareCursor = true;
						TabletScript.TabletHasFocus = true;
						NetworkSingleton<NetworkUI>.Instance.GUITabletWindow.GetComponent<UITabletWindow>().CurrentTablet.HasFocus = true;
					}
				}
			}
			catch (Exception)
			{
				TabletScript.TabletHasFocus = false;
				this.bShowHardwareCursor = false;
			}
		}
		if (pointerPosition != Vector3.zero)
		{
			bool flag4 = HoverScript.bAltZooming && AltZoomCamera.AltZoomFollowsPointer;
			Cursor.visible = ((this.bShowHardwareCursor || this.PointerTypeIndex < 0) && !flag4);
			this.ReferenceFollower.GetComponent<Renderer>().enabled = (!this.bShowHardwareCursor && !flag4);
			this.rigidbody.position = pointerPosition;
			this.rigidbody.rotation = Quaternion.Euler(new Vector3(0f, this.MainCamera.transform.eulerAngles.y - 180f, 0f));
			this.ReferenceFollower.transform.position = pointerPosition;
			this.ReferenceFollower.transform.rotation = Quaternion.Euler(new Vector3(this.MainCamera.transform.eulerAngles.x * -1f + 90f, base.transform.eulerAngles.y, base.transform.eulerAngles.z));
		}
		else
		{
			Cursor.visible = true;
		}
		if (UICamera.HoveredUIObject)
		{
			this.ReferenceFollower.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f) * Vector3.Distance(base.transform.position, this.MainCamera.transform.position);
		}
		else
		{
			this.ReferenceFollower.transform.localScale = Vector3.one;
		}
		if (TabletScript.TabletHasFocus || flag3)
		{
			return;
		}
		if (this.InteractiveSpawning)
		{
			if (zInput.GetButtonDown("Tap", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.TapHeldTime = 0f;
				this.InfoStartVector = base.transform.position;
			}
			else if (zInput.GetButton("Tap", ControlType.All))
			{
				this.TapHeldTime += Time.deltaTime;
			}
			if ((ObjectPositioningVisualizer.VisualizerSpawnEndOnRightClick && zInput.GetButtonUp("Tap", ControlType.All) && !UICamera.SelectIsInput() && Vector3.Distance(this.InfoStartVector, base.transform.position) < HoverScript.DISTANCE_CHECK && this.TapHeldTime < 0.3f) || TTSInput.GetKeyDown(KeyCode.Escape) || (UICamera.HoveredUIObject && zInput.GetButtonDown("Grab", ControlType.All)))
			{
				this.InteractiveSpawning = false;
				Singleton<ObjectPositioningVisualizer>.Instance.EndSpawnVisualizer();
			}
			else if (zInput.GetButton("Grab", ControlType.All) && Singleton<ObjectPositioningVisualizer>.Instance.SpawnOK && !UICamera.HoveredUIObject && !this.bSelecting)
			{
				Vector3 vector5 = Singleton<ObjectPositioningVisualizer>.Instance.SpawnLocation;
				bool flag5 = ObjectPositioningVisualizer.VisualizerSpawnAboveTable;
				if (zInput.GetButton("Ctrl", ControlType.All))
				{
					flag5 = !flag5;
				}
				if (flag5 && !Singleton<ObjectPositioningVisualizer>.Instance.ForceSpawnInPlace)
				{
					vector5 = this.GetSpawnPosition(vector5, false);
				}
				if (zInput.GetButtonDown("Grab", ControlType.All) || Mathf.Abs(vector5.x - this.lastSpawnPosition.x) > Singleton<ObjectPositioningVisualizer>.Instance.SpawnBounds.size.x || Mathf.Abs(vector5.z - this.lastSpawnPosition.z) > Singleton<ObjectPositioningVisualizer>.Instance.SpawnBounds.size.z)
				{
					this.lastSpawnPosition = vector5;
					this.spawnInPlace(vector5);
				}
			}
		}
		else
		{
			this.ClickingWhileInHandSelectMode = (zInput.GetButtonDown("Grab", ControlType.All) || (Singleton<HandSelectMode>.Instance.IsActive && this.lastClickedNPO && Vector3.Distance(vector, this.clickPosition) < HoverScript.DISTANCE_CHECK));
			if (zInput.GetButtonUp("Grab", ControlType.All) && this.CurrentPointerMode == PointerMode.Grab)
			{
				this.clickReleaseTime = Time.time;
				if (this.ClickingWhileInHandSelectMode)
				{
					Singleton<HandSelectMode>.Instance.ToggleNPOSelection(this.lastClickedNPO);
				}
				this.lastClickedNPO = null;
			}
			if (zInput.GetButton("Grab", ControlType.All) && !ServerOptions.isPhysicsLock && !UICamera.HoveredUIObject && !this.bSelecting && (this.CurrentPointerMode == PointerMode.Grab || Pointer.IsCombineTool(this.CurrentPointerMode)))
			{
				Vector3 origin = new Vector3(pointerPosition.x, pointerPosition.y + 5f, pointerPosition.z);
				Ray ray = new Ray(origin, Vector3.down);
				int num5 = 0;
				Pointer.raycastHits = HoverScript.RaySphereCast(ray, out num5);
				for (int i = 0; i < num5; i++)
				{
					RaycastHit raycastHit = Pointer.raycastHits[i];
					GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider);
					if (gameObject.layer != 18)
					{
						NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
						if (component && component.IsGrabbable && !component.CachedIsInvisible && component.HeldByPlayerID != this.ID && gameObject.layer != 2 && !component.IsLocked)
						{
							Highlighter highlighter = component.highlighter;
							if (highlighter)
							{
								this.HoverObject = gameObject;
								if (!this.bHideHoverHighlight)
								{
									highlighter.On(this.HoverColor);
									break;
								}
								break;
							}
						}
					}
				}
			}
			bool flag6 = this.CurrentPointerMode == PointerMode.Line || this.CurrentPointerMode == PointerMode.Flick || Pointer.IsCombineTool(this.CurrentPointerMode);
			bool flag7 = zInput.GetButtonDown("Line Tool", ControlType.All) && !this.measuringPickedUpObject;
			if (!UICamera.HoveredUIObject && !UICamera.SelectIsInput() && (flag7 || (flag6 && zInput.GetButtonDown("Grab", ControlType.All)) || this.IsMeasureMovement()))
			{
				this.StartLine(pointerPosition, (this.PrevHoverObject != null) ? this.PrevHoverObject.GetComponent<NetworkPhysicsObject>() : null);
			}
			flag7 = (zInput.GetButton("Line Tool", ControlType.All) && !this.measuringPickedUpObject);
			if (this.StartLineVector != Vector3.zero && (flag7 || (flag6 && zInput.GetButton("Grab", ControlType.All))))
			{
				this.UpdateLine(pointerPosition);
			}
			flag7 = (zInput.GetButtonUp("Line Tool", ControlType.All) && !this.measuringPickedUpObject);
			if (this.StartLineVector != Vector3.zero && (flag7 || (flag6 && zInput.GetButtonUp("Grab", ControlType.All))))
			{
				this.EndLine(pointerPosition, this.HoverObject);
			}
			this.pickedUpMeasuredObject = false;
			if ((zInput.GetButtonDown("Grab", ControlType.All) || zInput.GetButtonUp("Grab", ControlType.All)) && !UICamera.HoveredUIObject)
			{
				this.FirstGrabbedObject = null;
			}
			if (zInput.GetButton("Ctrl", ControlType.All))
			{
				if (zInput.GetButtonDown("Grab", ControlType.All) && this.HoverLockObject)
				{
					NetworkPhysicsObject component2 = this.HoverLockObject.GetComponent<NetworkPhysicsObject>();
					if (!this.HighLightedObjects.Contains(component2))
					{
						this.AddHighlight(component2, true);
					}
					else
					{
						this.RemoveHighlight(component2, true);
					}
				}
			}
			else if (zInput.GetButton("Grab", ControlType.All) && this.CurrentPointerMode == PointerMode.Grab && !UICamera.HoveredUIObject && GUIUtility.hotControl == 0)
			{
				Ray pointerRay = HoverScript.GetPointerRay();
				int num6 = 0;
				Pointer.raycastHits = HoverScript.RaySphereCast(pointerRay, out num6);
				int num7 = -1;
				for (int j = 0; j < num6; j++)
				{
					RaycastHit raycastHit2 = Pointer.raycastHits[j];
					GameObject gameObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit2.collider);
					NetworkPhysicsObject networkPhysicsObject3 = null;
					if (gameObject2.layer != 18)
					{
						networkPhysicsObject3 = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject2);
					}
					if (networkPhysicsObject3 && networkPhysicsObject3.IsGrabbable && !networkPhysicsObject3.CachedIsInvisible && networkPhysicsObject3.HeldByPlayerID != this.ID && (!networkPhysicsObject3.IsLocked || networkPhysicsObject3.deckScript || networkPhysicsObject3.stackObject || networkPhysicsObject3.clockScript) && gameObject2.layer != 2)
					{
						if (networkPhysicsObject3.stackObject || networkPhysicsObject3.deckScript || networkPhysicsObject3.clockScript)
						{
							num7 = networkPhysicsObject3.ID;
							HoverScript.GrabbedStackObject = networkPhysicsObject3.gameObject;
						}
						if (networkPhysicsObject3.ID == num7 && this.containerHeldTime > 0f && !this.bStopGrab)
						{
							this.containerHeldTime += Time.deltaTime;
							if (networkPhysicsObject3.InsideALayoutZone || (!Pointer.DOUBLE_CLICK_CONTAINER_PICKUP && this.containerHeldTime > 0.4f) || (Pointer.DOUBLE_CLICK_CONTAINER_PICKUP && Time.time - this.clickReleaseTime < 0.4f) || (this.bDeselectOnGrab && this.HighLightedObjects.Contains(networkPhysicsObject3)))
							{
								this.Grab(networkPhysicsObject3.ID, new Vector3?(base.transform.position), -1, true, null);
								this.lastClickedNPO = networkPhysicsObject3;
								this.clickPosition = vector;
								if (!this.HighLightedObjects.Contains(networkPhysicsObject3))
								{
									this.ResetHighlight();
								}
								this.bStopGrab = true;
								break;
							}
							break;
						}
						else if (zInput.GetButtonDown("Grab", ControlType.All))
						{
							if (networkPhysicsObject3.ID == num7)
							{
								this.containerHeldTime = Time.deltaTime;
								this.containerHeldID = networkPhysicsObject3.ID;
								break;
							}
							this.Grab(networkPhysicsObject3.ID, new Vector3?(base.transform.position), -1, true, null);
							this.lastClickedNPO = networkPhysicsObject3;
							this.clickPosition = vector;
							if (!this.HighLightedObjects.Contains(networkPhysicsObject3))
							{
								this.ResetHighlight();
								break;
							}
							break;
						}
					}
				}
				if (num7 != this.containerHeldID && this.containerHeldTime > 0f && !this.bStopGrab)
				{
					GameObject gameObject3 = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(this.containerHeldID);
					if (gameObject3)
					{
						if (gameObject3.GetComponent<StackObject>())
						{
							base.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(this.TellObjectManagerAboutObjectTake), this.containerHeldID, this.ID, -1);
						}
						else if (gameObject3.GetComponent<DeckScript>())
						{
							base.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(this.TellObjectManagerAboutCardPeel), this.containerHeldID, this.ID, -1);
						}
						else if (gameObject3.GetComponent<ClockScript>())
						{
							this.Grab(gameObject3.GetComponent<NetworkPhysicsObject>().ID, new Vector3?(base.transform.position), -1, true, null);
						}
					}
					this.ResetHighlight();
					this.bStopMarquee = true;
					this.containerHeldTime = 0f;
				}
			}
			if (PermissionsOptions.CheckIfAllowedInMode(PointerMode.Text, -1) && zInput.GetButtonDown("Grab", ControlType.All) && this.CurrentPointerMode == PointerMode.Text && UICamera.HoveredUIObject == null)
			{
				Vector3 zero = Vector3.zero;
				zero.x = 90f;
				zero.y = this.MainCamera.transform.eulerAngles.y;
				int objectId = -1;
				if (HoverScript.HoverLockLock != null)
				{
					objectId = HoverScript.HoverLockLock.GetComponent<NetworkPhysicsObject>().ID;
				}
				Vector3 vector6 = pointerPosition;
				vector6.x += this.MainCamera.transform.forward.normalized.x * 1f;
				vector6.z += this.MainCamera.transform.forward.normalized.z * 1f;
				this.SpawnText(vector6, zero, objectId);
			}
			this.justStartedZoneMenu = false;
			if (PermissionsOptions.CheckIfAllowedInMode(PointerMode.Hidden, -1))
			{
				if (zInput.GetButtonDown("Grab", ControlType.All) && !UICamera.HoveredUIObject)
				{
					this.StartZone(this.CurrentPointerMode, pointerPosition, Input.mousePosition, null);
				}
				if (zInput.GetButton("Grab", ControlType.All))
				{
					this.UpdateZone(this.CurrentPointerMode, pointerPosition, Input.mousePosition, false);
				}
				if (zInput.GetButtonUp("Grab", ControlType.All))
				{
					this.EndZone(this.CurrentPointerMode);
				}
			}
			if (zInput.GetButtonDown("Grab", ControlType.All))
			{
				if (this.containerHeldTime == 0f && !UICamera.HoveredUIObject && !UICamera.SelectIsInput() && this.CurrentPointerMode != PointerMode.Flick && !Pointer.IsCombineTool(this.CurrentPointerMode))
				{
					this.CtrlHighLightedObjects.Clear();
					if (!zInput.GetButton("Ctrl", ControlType.All))
					{
						if (this.bDeselectOnGrab || (!this.bStopMarquee && !this.HoverObject) || (this.HoverObject && !this.HighLightedObjects.Contains(this.HoverObject.GetComponent<NetworkPhysicsObject>())))
						{
							this.ResetHighlight();
							Singleton<UITagEditor>.Instance.SelectionChanged();
						}
					}
					else
					{
						foreach (NetworkPhysicsObject networkPhysicsObject4 in this.HighLightedObjects)
						{
							this.CtrlHighLightedObjects.Add(networkPhysicsObject4.gameObject);
						}
					}
				}
				this.StartMarqueeVector = Vector3.zero;
				if (this.containerHeldTime == 0f && !this.bStopMarquee && GUIUtility.hotControl == 0 && !UICamera.HoveredUIObject && !UICamera.SelectIsInput() && !this.HoverObject && (!zInput.GetButton("Ctrl", ControlType.All) || !this.HoverLockObject) && this.CurrentPointerMode == PointerMode.Grab)
				{
					this.StartMarqueeVector = pointerPosition;
					if (this.containerHeldTime == 0f && !this.bStopMarquee && GUIUtility.hotControl == 0 && this.StartMarqueeVector != Vector3.zero && !zInput.GetButton("Line Tool", ControlType.All))
					{
						this.bSelecting = true;
						Singleton<RectangleSelection>.Instance.StartSelection(this.CtrlHighLightedObjects, new Action<GameObject>(this.AddSelection), new Action<GameObject>(this.RemoveSelection), 128);
					}
				}
			}
			else if (zInput.GetButtonUp("Grab", ControlType.All) && this.selectionWasChanged)
			{
				this.selectionWasChanged = false;
				Singleton<UITagEditor>.Instance.SelectionChanged();
			}
			if (zInput.GetButtonUp("Grab", ControlType.All))
			{
				if (this.containerHeldTime == 0f)
				{
					this.clickReleaseTime = 0f;
				}
				if (!zInput.GetButton("Line Tool", ControlType.All))
				{
					this.CalculateLineInstances(false);
				}
				if (!this.bStopGrab && this.containerHeldTime != 0f)
				{
					GameObject gameObject4 = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(this.containerHeldID);
					if (gameObject4 && gameObject4.GetComponent<ClockScript>())
					{
						if (PermissionsOptions.CheckAllow(PermissionsOptions.options.Digital, -1))
						{
							gameObject4.GetComponent<ClockScript>().PauseStart();
						}
						else
						{
							PermissionsOptions.BroadcastPermissionWarning("interact with the Clock (Digital)");
						}
					}
				}
				if (this.bSelecting)
				{
					Singleton<RectangleSelection>.Instance.EndSelection();
				}
				this.bSelecting = false;
				this.bStopGrab = false;
				this.bStopMarquee = false;
				this.containerHeldTime = 0f;
				this.Release(this.ID, -1, -1, -1);
			}
			if (zInput.GetButtonDown("Tap", ControlType.All) && zInput.GetButton("Alt", ControlType.All))
			{
				this.Release(this.ID, this.GetLastGrabbedId(), -1, -1);
			}
			if (zInput.GetButton("Grab", ControlType.All))
			{
				this.PointerMeshIndex = 2;
				if ((Input.GetAxis("Mouse Wheel") != 0f && zInput.GetAxis("Rotate", ControlType.Keyboard, false) != 0f) || (zInput.GetAxis("Rotate", ControlType.Controller, false) != 0f && Time.time > this.LastRotateTime + 0.1f / Mathf.Abs(zInput.GetAxis("Rotate", ControlType.Controller, false))))
				{
					float num8 = 0f;
					if (zInput.GetAxis("Rotate", ControlType.Controller, false) != 0f)
					{
						num8 = zInput.GetAxis("Rotate", ControlType.Controller, false);
						this.LastRotateTime = Time.time;
					}
					if (zInput.GetAxis("Rotate", ControlType.Keyboard, false) != 0f)
					{
						num8 = zInput.GetAxis("Rotate", ControlType.All, false);
					}
					bool flag8 = num8 > 0f;
					if (!zInput.GetButton("Alt", ControlType.All))
					{
						if (flag8)
						{
							this.ChangeHeldSpinRotationIndex(this.RotationSnap / 15, -1);
						}
						else
						{
							this.ChangeHeldSpinRotationIndex(24 - this.RotationSnap / 15, -1);
						}
					}
					else if (!zInput.GetButton("Shift", ControlType.All))
					{
						if (flag8)
						{
							this.ChangeHeldFlipRotationIndex(this.RotationSnap / 15, -1);
						}
						else
						{
							this.ChangeHeldFlipRotationIndex(24 - this.RotationSnap / 15, -1);
						}
					}
				}
				this.bRaise = zInput.GetButton("Raise", ControlType.All);
				this.bTap = (zInput.GetButton("Tap", ControlType.All) && !zInput.GetButton("Alt", ControlType.All));
				if (zInput.GetButtonDown("Tap", ControlType.All) && !zInput.GetButton("Alt", ControlType.All) && this.CurrentPointerMode == PointerMode.Grab && !ServerOptions.isPhysicsLock)
				{
					Vector3 origin2 = new Vector3(pointerPosition.x, pointerPosition.y + 5f, pointerPosition.z);
					Ray ray2 = new Ray(origin2, Vector3.down);
					int num9 = 0;
					Pointer.raycastHits = HoverScript.RaySphereCast(ray2, out num9);
					int k = 0;
					while (k < num9)
					{
						RaycastHit raycastHit3 = Pointer.raycastHits[k];
						GameObject gameObject5 = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit3.collider);
						NetworkPhysicsObject component3 = gameObject5.GetComponent<NetworkPhysicsObject>();
						if (component3 && component3.IsGrabbable && !component3.CachedIsInvisible && component3.HeldByPlayerID != this.ID && (!gameObject5.GetComponent<CardScript>() || !gameObject5.GetComponent<CardScript>().RootHeldByMe()) && gameObject5.layer != 2)
						{
							if (component3.stackObject)
							{
								base.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(this.TellObjectManagerAboutObjectTake), component3.ID, this.ID, -1);
								break;
							}
							if (component3.deckScript)
							{
								base.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(this.TellObjectManagerAboutCardPeel), component3.ID, this.ID, -1);
								break;
							}
							this.Grab(component3.ID, new Vector3?(Vector3.zero), -1, true, null);
							break;
						}
						else
						{
							k++;
						}
					}
				}
			}
			if (zInput.GetButtonDown("Flip", ControlType.All) && !UICamera.SelectIsInput())
			{
				if (zInput.GetButton("Grab", ControlType.All))
				{
					if (!zInput.GetButton("Alt", ControlType.All))
					{
						this.ChangeHeldFlipRotationIndex(12, -1);
					}
					else if (!zInput.GetButton("Shift", ControlType.All))
					{
						this.ChangeHeldSpinRotationIndex(12, -1);
					}
				}
				else if (!TTSInput.GetKey("mouse 2"))
				{
					this.Rotation(this.GetHoverID(), 0, 12, false);
				}
			}
			if ((zInput.GetButton("Rotate Right", ControlType.All) || zInput.GetButton("Rotate Left", ControlType.All)) && !UICamera.SelectIsInput() && Input.GetAxis("Mouse Wheel") == 0f)
			{
				bool button = zInput.GetButton("Alt", ControlType.All);
				if ((zInput.GetButton("Grab", ControlType.All) && Time.time > this.LastRotateTime + 0.1f) || Time.time > this.LastRotateTime + 0.15f)
				{
					this.LastRotateTime = Time.time;
					bool button2 = zInput.GetButton("Rotate Left", ControlType.All);
					bool button3 = zInput.GetButton("Grab", ControlType.All);
					int num10 = this.RotationSnap / 15;
					if (button2)
					{
						num10 = 24 - num10;
					}
					if (button3)
					{
						if (!button)
						{
							this.ChangeHeldSpinRotationIndex(num10, -1);
						}
						else if (!zInput.GetButton("Shift", ControlType.All))
						{
							this.ChangeHeldFlipRotationIndex(num10, -1);
						}
					}
					else if (!button && (zInput.GetButton("Rotate Right", ControlType.Keyboard) || zInput.GetButton("Rotate Left", ControlType.Keyboard)))
					{
						this.Rotation(this.GetHoverID(), num10, 0, false);
					}
				}
			}
			if (zInput.GetButtonDown("Under", ControlType.All) && !UICamera.SelectIsInput() && !zInput.GetButton("Grab", ControlType.All))
			{
				this.Under(this.GetHoverID());
			}
			if ((zInput.GetButton("Scale Down", ControlType.All) || zInput.GetButton("Scale Up", ControlType.All)) && !UICamera.SelectIsInput() && !Pointer.IsVectorTool(this.CurrentPointerMode))
			{
				float num11 = 0.2f / Mathf.Sqrt((float)this.ScaleCount);
				num11 = Mathf.Max(0.025f, num11);
				if (Time.time > this.LastScaleTime + num11)
				{
					this.LastScaleTime = Time.time;
					this.ScaleCount++;
					bool button4 = zInput.GetButton("Scale Up", ControlType.All);
					this.Scale(this.GetHoverID(), button4);
				}
			}
			else
			{
				this.ScaleCount = 1;
			}
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				if ((TTSInput.GetKeyDown("delete") || TTSInput.GetKeyDown("backspace")) && !UICamera.SelectIsInput())
				{
					this.Delete(this.GetHoverID());
				}
				if (zInput.GetButtonDown("Cut", ControlType.All) && !UICamera.SelectIsInput() && (zInput.GetButton("Ctrl", ControlType.All) || Application.isEditor))
				{
					this.Copy(this.GetHoverID(), true, false);
				}
				if (zInput.GetButtonDown("Copy", ControlType.All) && !UICamera.SelectIsInput() && (zInput.GetButton("Ctrl", ControlType.All) || Application.isEditor))
				{
					this.Copy(this.GetHoverID(), false, false);
				}
				if (zInput.GetButtonDown("Paste", ControlType.All) && !UICamera.SelectIsInput() && (zInput.GetButton("Ctrl", ControlType.All) || Application.isEditor))
				{
					this.Paste(base.transform.position, false, true, true);
				}
			}
			if (zInput.GetButtonDown("Group", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.Group(this.GetHoverID());
			}
			if (zInput.GetButtonDown("Previous State", ControlType.All) && !UICamera.SelectIsInput())
			{
				foreach (GameObject gameObject6 in this.GetSelectedObjects(this.GetHoverID(), true, true))
				{
					NetworkPhysicsObject component4 = gameObject6.GetComponent<NetworkPhysicsObject>();
					if (component4)
					{
						if (component4.HasStates())
						{
							component4.PrevState();
						}
						else if (component4.customPDF != null)
						{
							component4.customPDF.PrevPage();
						}
					}
				}
			}
			if (zInput.GetButtonDown("Next State", ControlType.All) && !UICamera.SelectIsInput())
			{
				foreach (GameObject gameObject7 in this.GetSelectedObjects(this.GetHoverID(), true, true))
				{
					NetworkPhysicsObject component5 = gameObject7.GetComponent<NetworkPhysicsObject>();
					if (component5)
					{
						if (component5.HasStates())
						{
							component5.NextState();
						}
						else if (component5.customPDF != null)
						{
							component5.customPDF.NextPage();
						}
					}
				}
			}
			if (zInput.GetButtonDown("RPG Mode", ControlType.All) && !UICamera.SelectIsInput() && !zInput.GetButton("Alt", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All))
			{
				if (!zInput.GetButton("Grab", ControlType.All))
				{
					GameObject gameObject8 = this.CheckForTag("rpgFigurine");
					if (gameObject8)
					{
						gameObject8.GetComponent<RPGFigurines>().ChangeMode();
					}
				}
				else if (this.FirstGrabbedObject && this.FirstGrabbedObject.CompareTag("rpgFigurine"))
				{
					this.FirstGrabbedObject.GetComponent<RPGFigurines>().ChangeMode();
				}
			}
			if (zInput.GetButtonDown("RPG Attack", ControlType.All) && !UICamera.SelectIsInput() && !zInput.GetButton("Alt", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All))
			{
				if (!zInput.GetButton("Grab", ControlType.All))
				{
					GameObject gameObject9 = this.CheckForTag("rpgFigurine");
					if (gameObject9)
					{
						gameObject9.GetComponent<RPGFigurines>().Attack();
					}
				}
				else if (this.FirstGrabbedObject && this.FirstGrabbedObject.CompareTag("rpgFigurine"))
				{
					this.FirstGrabbedObject.GetComponent<RPGFigurines>().Attack();
				}
			}
			if (zInput.GetButtonDown("RPG Die", ControlType.All) && !UICamera.SelectIsInput() && !zInput.GetButton("Alt", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All))
			{
				if (!zInput.GetButton("Grab", ControlType.All))
				{
					GameObject gameObject10 = this.CheckForTag("rpgFigurine");
					if (gameObject10)
					{
						gameObject10.GetComponent<RPGFigurines>().Die();
					}
				}
				else if (this.FirstGrabbedObject && this.FirstGrabbedObject.CompareTag("rpgFigurine"))
				{
					this.FirstGrabbedObject.GetComponent<RPGFigurines>().Die();
				}
			}
			if (zInput.GetButtonDown("Raise", ControlType.All) && !zInput.GetButton("Grab", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.Randomize(this.GetHoverID(), this.ID);
			}
			if (zInput.GetButton("Grab", ControlType.All) && !UICamera.SelectIsInput())
			{
				for (int l = 0; l < this.numberKeys.Length; l++)
				{
					if (TTSInput.GetKeyDown(this.numberKeys[l]))
					{
						this.ChangeHeldGridLayout(l);
					}
				}
			}
			NetworkPhysicsObject networkPhysicsObject5 = this.HoverLockObject ? this.HoverLockObject.GetComponent<NetworkPhysicsObject>() : null;
			int num12 = Pointer.TypedNumberIntercept ? -2 : (networkPhysicsObject5 ? networkPhysicsObject5.ID : -1);
			bool flag9 = false;
			if (Pointer.TypedNumberIntercept)
			{
				if (Pointer.typedNumberInterceptColor != "" && Pointer.TypedNumberIntercept != Pointer.prevTypedNumberIntercept)
				{
					flag9 = true;
				}
				if (!Pointer.prevTypedNumberIntercept)
				{
					List<NetworkPhysicsObject> selectedNPOs = this.GetSelectedNPOs(-1, true, false);
					for (int m = 0; m < selectedNPOs.Count; m++)
					{
						NetworkPhysicsObject networkPhysicsObject6 = selectedNPOs[m];
						if (networkPhysicsObject6 && networkPhysicsObject6.deckScript)
						{
							Pointer.typedNumberInterceptMaxNumber = Mathf.Max(Pointer.typedNumberInterceptMaxNumber, networkPhysicsObject6.deckScript.num_cards_);
						}
					}
				}
			}
			Pointer.prevTypedNumberIntercept = Pointer.TypedNumberIntercept;
			if ((networkPhysicsObject5 || Pointer.TypedNumberIntercept) && !zInput.GetButton("Grab", ControlType.All))
			{
				if (this.typedNumberTargetID != -1 && (this.typedNumberTargetID != num12 || flag9))
				{
					if (this.typedNumberMagnitude != -1)
					{
						if (this.typedNumberTargetID == -2)
						{
							this.HandleInterceptedTypedNumber(this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
						}
						else
						{
							NetworkPhysicsObject npo = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromID(this.typedNumberTargetID);
							NetworkSingleton<ManagerPhysicsObject>.Instance.HandleTypedNumberWrapper(npo, this.ID, this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
						}
					}
					this.typedNumberMagnitude = -1;
					this.typedNumberTargetID = -1;
					this.typedNumberDigitCount = 0;
					this.typedNumberIsNegative = false;
				}
				int num13 = Pointer.TypedNumberIntercept ? Pointer.typedNumberInterceptMaxNumber : networkPhysicsObject5.MaxTypedNumber();
				if (num13 > 0 && !UICamera.SelectIsInput() && !zInput.GetButton("Shift", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All))
				{
					if (this.typedNumberMagnitude == -1 && NetworkPhysicsObject.AllowTypingNegativeNumbers && TTSInput.GetKeyDown("-"))
					{
						this.typedNumberTargetID = num12;
						this.typedNumberMagnitude = 0;
						this.typedNumberMaxDigits = 1 + (int)Mathf.Log10((float)num13);
						this.typedNumberIsNegative = true;
						this.typedNumberLastKeypressAt = Time.time;
					}
					else
					{
						for (int n = 0; n < this.numberKeys.Length; n++)
						{
							if (TTSInput.GetKeyDown(this.numberKeys[n]))
							{
								if (this.typedNumberTargetID == -1)
								{
									this.typedNumberTargetID = num12;
									this.typedNumberMagnitude = n;
									if (Pointer.DEFAULT_TYPED_NUMBER_IS_SINGLE_DIGIT && n == 0)
									{
										this.typedNumberMaxDigits = int.MaxValue;
									}
									else
									{
										this.typedNumberMaxDigits = 1 + (int)Mathf.Log10((float)num13);
									}
								}
								else
								{
									this.typedNumberMagnitude = this.typedNumberMagnitude * 10 + n;
								}
								this.typedNumberDigitCount++;
								if (Pointer.TypedNumberIntercept && Pointer.typedNumberInterceptColor == "")
								{
									Pointer.typedNumberInterceptColor = Pointer.TypedNumberIntercept.name.Split(new char[]
									{
										' '
									})[0];
								}
								if (this.typedNumberDigitCount >= this.typedNumberMaxDigits)
								{
									if (this.typedNumberTargetID == -2)
									{
										this.HandleInterceptedTypedNumber(this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
									}
									else
									{
										NetworkSingleton<ManagerPhysicsObject>.Instance.HandleTypedNumberWrapper(networkPhysicsObject5, this.ID, this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
									}
									this.typedNumberMagnitude = -1;
									this.typedNumberTargetID = -1;
									this.typedNumberDigitCount = 0;
									this.typedNumberIsNegative = false;
								}
								else
								{
									this.typedNumberLastKeypressAt = Time.time;
								}
							}
						}
					}
					if (this.typedNumberMagnitude != -1 && Time.time > this.typedNumberLastKeypressAt + 1f)
					{
						if (this.typedNumberTargetID == -2)
						{
							this.HandleInterceptedTypedNumber(this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
						}
						else
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.HandleTypedNumberWrapper(networkPhysicsObject5, this.ID, this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
						}
						this.typedNumberMagnitude = -1;
						this.typedNumberTargetID = -1;
						this.typedNumberDigitCount = 0;
						this.typedNumberIsNegative = false;
					}
				}
			}
			else if (this.typedNumberMagnitude != -1 && this.typedNumberTargetID != -1)
			{
				if (this.typedNumberTargetID == -2)
				{
					this.HandleInterceptedTypedNumber(this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
				}
				else
				{
					NetworkPhysicsObject npo2 = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromID(this.typedNumberTargetID);
					NetworkSingleton<ManagerPhysicsObject>.Instance.HandleTypedNumberWrapper(npo2, this.ID, this.typedNumberIsNegative ? (-this.typedNumberMagnitude) : this.typedNumberMagnitude);
				}
				this.typedNumberMagnitude = -1;
				this.typedNumberTargetID = -1;
				this.typedNumberDigitCount = 0;
				this.typedNumberIsNegative = false;
			}
			if (zInput.GetButton("Grab", ControlType.All) && zInput.GetButton("Alt", ControlType.All) && zInput.GetButton("Shift", ControlType.All) && !UICamera.SelectIsInput())
			{
				if (zInput.GetButtonDown("Rotate Right", ControlType.All))
				{
					this.Scale(this.GetHoverID(), true);
				}
				if (zInput.GetButtonDown("Rotate Left", ControlType.All))
				{
					this.Scale(this.GetHoverID(), false);
				}
			}
			if (zInput.GetButtonDown("Lock", ControlType.All) && !UICamera.SelectIsInput())
			{
				this.Lock(this.GetHoverLockID(), -1);
			}
			if (zInput.GetButtonDown("Grab", ControlType.All) && GUIUtility.hotControl == 0 && !UICamera.HoveredUIObject && !UICamera.SelectIsInput() && !this.justStartedZoneMenu)
			{
				this.ResetAllObjects();
			}
			if (zInput.GetButtonDown("Tap", ControlType.All) && !UICamera.HoveredUIObject && !UICamera.SelectIsInput())
			{
				this.TapHeldTime = 0f;
				this.ResetAllObjects();
				this.InfoStartVector = base.transform.position;
			}
			if (zInput.GetButton("Tap", ControlType.All))
			{
				this.TapHeldTime += Time.deltaTime;
			}
			if (zInput.GetButtonDown("Tap", ControlType.All) && UICamera.HoveredUIObject && UICamera.HoveredUIObject.GetComponent<TextTool>())
			{
				TTSUtilities.CopyToClipboard(UICamera.HoveredUIObject.GetComponent<NetworkPhysicsObject>().GUID);
			}
			if (zInput.GetButtonUp("Tap", ControlType.All) && !zInput.GetButton("Grab", ControlType.All) && !UICamera.SelectIsInput() && !UICamera.HoveredUIObject && Vector3.Distance(this.InfoStartVector, base.transform.position) < HoverScript.DISTANCE_CHECK && this.TapHeldTime < 0.3f)
			{
				if (Pointer.IsZoneTool(this.CurrentPointerMode))
				{
					this.CheckStartContextualZones(Input.mousePosition);
				}
				else if (Pointer.IsSnapTool(this.CurrentPointerMode))
				{
					NetworkSingleton<SnapPointManager>.Instance.CheckSnapPointMenuClick(HoverScript.PointerPosition);
				}
				else if (this.HoverLockObject)
				{
					this.StartContextual(this.HoverLockObject, true);
				}
				else if (!this.InfoObject && !this.InfoHiddenZoneGO && !this.InfoRandomizeZoneGO && !this.InfoHandObject && !this.InfoFogOfWarZoneGO && !this.InfoLayoutZoneGO)
				{
					this.StartGlobalContextual(Input.mousePosition);
				}
			}
			if (!this.InfoObject && this.NetworkInstance.GUIContextualMenu.activeSelf)
			{
				this.NetworkInstance.GUIContextualMenu.SetActive(false);
			}
			if ((!this.InfoHiddenZoneGO || this.CurrentPointerMode != PointerMode.Hidden) && this.NetworkInstance.GUIFogColor.activeSelf)
			{
				this.NetworkInstance.GUIFogColor.SetActive(false);
			}
			if ((!this.InfoRandomizeZoneGO || this.CurrentPointerMode != PointerMode.Randomize) && this.NetworkInstance.GUIRandomizeZone.activeSelf)
			{
				this.NetworkInstance.GUIRandomizeZone.SetActive(false);
			}
			if ((!this.InfoHandObject || this.CurrentPointerMode != PointerMode.Hands) && this.NetworkInstance.GUIHandColor.activeSelf)
			{
				this.NetworkInstance.GUIHandColor.SetActive(this.InfoHandObject);
			}
			if ((!this.InfoFogOfWarZoneGO || this.CurrentPointerMode != PointerMode.FogOfWar) && this.NetworkInstance.GUIFogOfWar.activeSelf)
			{
				this.NetworkInstance.GUIFogOfWar.SetActive(this.InfoFogOfWarZoneGO);
			}
			if ((!this.InfoLayoutZoneGO || this.CurrentPointerMode != PointerMode.LayoutZone) && this.NetworkInstance.GUILayoutZone.activeSelf)
			{
				this.NetworkInstance.GUILayoutZone.SetActive(this.InfoLayoutZoneGO);
			}
			if (!UICamera.SelectIsInput())
			{
				UserDefinedHotkeyManager instance = NetworkSingleton<UserDefinedHotkeyManager>.Instance;
				Vector3? position = null;
				bool flag10 = true;
				for (int num14 = 0; num14 < instance.Hotkeys.Count; num14++)
				{
					UserDefinedHotkeyManager.HotkeyIdentifier hotkeyIdentifier = instance.Hotkeys[num14];
					if (!(hotkeyIdentifier.label == ""))
					{
						bool buttonDown = zInput.GetButtonDown(hotkeyIdentifier.cInputID, ControlType.All);
						bool flag11 = hotkeyIdentifier.triggerOnKeyUp && zInput.GetButtonUp(hotkeyIdentifier.cInputID, ControlType.All);
						if (buttonDown || flag11)
						{
							if (flag10)
							{
								flag10 = false;
								Ray pointerRay2 = HoverScript.GetPointerRay();
								int num15 = 0;
								Pointer.raycastHits = HoverScript.RaySphereCast(pointerRay2, out num15);
								float num16 = 0f;
								for (int num17 = 0; num17 < num15; num17++)
								{
									RaycastHit raycastHit4 = Pointer.raycastHits[num17];
									GameObject gameObject11 = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit4.collider);
									if (!gameObject11.name.StartsWith("Bounds") && (num16 == 0f || raycastHit4.distance < num16))
									{
										position = new Vector3?(raycastHit4.point);
										num16 = raycastHit4.distance;
									}
								}
							}
							if (buttonDown)
							{
								instance.DoHotkey(NetworkID.ID, hotkeyIdentifier.index, networkPhysicsObject5, position, false);
							}
							if (flag11)
							{
								instance.DoHotkey(NetworkID.ID, hotkeyIdentifier.index, networkPhysicsObject5, position, true);
							}
						}
					}
				}
			}
			if (zInput.GetButton("Grab", ControlType.All) && Pointer.IsGizmoTool(this.CurrentPointerMode) && NetworkSingleton<NetworkUI>.Instance.GUIGrid.activeSelf && NetworkSingleton<GridOptions>.Instance.gridState.Lines)
			{
				int count = MonoSingletonBase<EditorObjectSelection>.Instance.SelectedGameObjects.Count;
				if (count > 0 && count <= 2)
				{
					List<GameObject> list = new List<GameObject>(MonoSingletonBase<EditorObjectSelection>.Instance.SelectedGameObjects);
					if (count == 1)
					{
						NetworkSingleton<GridOptions>.Instance.UpdateSettingsFromSelectedNPOs(list[0].GetComponent<NetworkPhysicsObject>(), null);
					}
					else
					{
						NetworkSingleton<GridOptions>.Instance.UpdateSettingsFromSelectedNPOs(list[0].GetComponent<NetworkPhysicsObject>(), list[1].GetComponent<NetworkPhysicsObject>());
					}
				}
			}
			if (!TTSInput.GetKey(KeyCode.LeftControl) && !TTSInput.GetKey(KeyCode.RightControl))
			{
				if (TTSInput.GetKeyDown(KeyCode.F1))
				{
					this.CurrentPointerMode = PointerMode.Grab;
				}
				if (TTSInput.GetKeyDown(KeyCode.F2))
				{
					this.ToggleBetweenModes(Pointer.VectorTools);
				}
				if (TTSInput.GetKeyDown(KeyCode.F3))
				{
					this.ToggleBetweenModes(Pointer.ZoneTools);
				}
				if (TTSInput.GetKeyDown(KeyCode.F4))
				{
					this.CurrentPointerMode = PointerMode.Line;
				}
				if (TTSInput.GetKeyDown(KeyCode.F5))
				{
					this.CurrentPointerMode = PointerMode.Flick;
				}
				if (TTSInput.GetKeyDown(KeyCode.F6))
				{
					this.ToggleBetweenModes(Pointer.CombineTools);
				}
				if (TTSInput.GetKeyDown(KeyCode.F7) && this.CurrentPointerMode != PointerMode.Text)
				{
					this.CurrentPointerMode = PointerMode.Text;
				}
				if (TTSInput.GetKeyDown(KeyCode.F8))
				{
					this.ToggleBetweenModes(Pointer.GizmoTools);
				}
				if (TTSInput.GetKeyDown(KeyCode.F9))
				{
					this.CurrentPointerMode = PointerMode.Decal;
				}
				if (TTSInput.GetKeyDown(KeyCode.F10))
				{
					this.ToggleBetweenModes(Pointer.SnapTools);
				}
			}
			if (this.CurrentPointerMode != PointerMode.VectorErase && Pointer.IsVectorTool(this.CurrentPointerMode))
			{
				this.PointerMeshIndex = 4;
			}
			else if (this.CurrentPointerMode == PointerMode.VectorErase)
			{
				this.PointerMeshIndex = 5;
			}
			else if (this.CurrentPointerMode == PointerMode.Line)
			{
				this.PointerMeshIndex = 6;
			}
			else if (Pointer.IsCombineTool(this.CurrentPointerMode))
			{
				this.PointerMeshIndex = 7;
			}
			else if (this.CurrentPointerMode == PointerMode.Text)
			{
				this.PointerMeshIndex = 8;
			}
			if (zInput.GetButton("Nudge", ControlType.All) && !zInput.GetButton("Alt", ControlType.All) && !UICamera.SelectIsInput() && !zInput.GetButton("Ctrl", ControlType.All) && this.CurrentPointerMode == PointerMode.Grab)
			{
				if (PermissionsOptions.CheckAllow(PermissionsOptions.options.Nudging, this.ID))
				{
					this.PointerMeshIndex = 3;
					if (Network.isServer)
					{
						this.ThisCollider.isTrigger = false;
					}
				}
				else
				{
					PermissionsOptions.BroadcastPermissionWarning("Nudge");
				}
			}
			else if (Network.isServer)
			{
				this.ThisCollider.isTrigger = true;
			}
		}
		if (Singleton<SpectatorCamera>.Instance.active && HoverScript.HoverCamera != Singleton<CameraManager>.Instance.SpectatorCamera && !VRHMD.isVR && this.PointerTypeIndex < 0)
		{
			this.ReferenceFollower.GetComponent<MeshLerpToObject>().SetPointerMesh(this.PointerMeshIndex, 0, true);
		}
		else
		{
			this.ReferenceFollower.GetComponent<MeshLerpToObject>().SetPointerMesh(this.PointerMeshIndex, this.PointerTypeIndex, false);
		}
		if (this.InteractiveSpawning)
		{
			Utilities.SetCursor(this.TextSpawn, NetworkUI.HardwareCursorOffest, CursorMode.Auto);
			return;
		}
		if (this.PointerTypeIndex < 0)
		{
			Texture2D texture2D = null;
			switch (this.PointerMeshIndex)
			{
			case 0:
				texture2D = this.TextDefault;
				break;
			case 1:
				texture2D = this.TextOpen;
				break;
			case 2:
			case 3:
				texture2D = this.TextGrab;
				break;
			case 4:
				texture2D = this.TextVector;
				break;
			case 5:
				texture2D = this.TextErase;
				break;
			case 6:
				texture2D = this.TextLine;
				break;
			case 7:
				texture2D = this.TextJoint;
				break;
			case 8:
				texture2D = this.TextText;
				break;
			}
			if (this.CurrentPointerMode == PointerMode.Flick)
			{
				texture2D = this.TextFlick;
			}
			else if (Pointer.IsGizmoTool(this.CurrentPointerMode))
			{
				texture2D = this.TextGizmo;
			}
			else if (Pointer.IsSnapTool(this.CurrentPointerMode))
			{
				texture2D = this.TextSnap;
			}
			else if (this.CurrentPointerMode == PointerMode.Decal)
			{
				texture2D = this.TextDecal;
			}
			else if (this.CurrentPointerMode == PointerMode.Vector)
			{
				texture2D = this.TextVector;
			}
			else if (this.CurrentPointerMode == PointerMode.Paint)
			{
				texture2D = this.TextPaint;
			}
			else if (Pointer.IsZoneTool(this.CurrentPointerMode))
			{
				texture2D = this.TextZone;
			}
			if (texture2D != null)
			{
				Utilities.SetCursor(texture2D, NetworkUI.HardwareCursorOffest, CursorMode.Auto);
			}
		}
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000ABBF8 File Offset: 0x000A9DF8
	private void HandleInterceptedTypedNumber(int count)
	{
		List<NetworkPhysicsObject> selectedNPOs = this.GetSelectedNPOs(-1, true, false);
		for (int i = 0; i < selectedNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = selectedNPOs[i];
			if (networkPhysicsObject && networkPhysicsObject.deckScript)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(networkPhysicsObject.ID, Pointer.typedNumberInterceptColor, count, 0);
			}
		}
		Pointer.typedNumberInterceptColor = "";
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x000ABC5E File Offset: 0x000A9E5E
	private void OnNetworkObjectDestroy(NetworkPhysicsObject NPO)
	{
		if (this.HighLightedObjects.Contains(NPO))
		{
			this.HighLightedObjects.Remove(NPO);
		}
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x000ABC7C File Offset: 0x000A9E7C
	private void OnThemeChange()
	{
		if (this.LineLabel)
		{
			this.LineLabel.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.MeasurementInner];
			this.LineLabel.effectColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.MeasurementOuter];
		}
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000ABCD8 File Offset: 0x000A9ED8
	private float calculateMeasuredDistance(Vector3 distanceVector, out float x, out float y, out float z)
	{
		x = (y = (z = 0f));
		if (this.zeroMeasurement)
		{
			return 0f;
		}
		Vector3 vector;
		if (LineScript.GRID_MEASUREMENTS)
		{
			vector = NetworkSingleton<GridOptions>.Instance.ScaledVector(distanceVector, out x, out y, out z);
		}
		else
		{
			vector = distanceVector;
			x = vector.x;
			y = vector.y;
			z = vector.z;
		}
		x = Mathf.Abs(x);
		y = Mathf.Abs(y);
		z = Mathf.Abs(z);
		if (LineScript.GRID_MEASUREMENTS)
		{
			return vector.magnitude;
		}
		if (LineScript.METRIC_MEASUREMENTS)
		{
			return vector.magnitude * 2.54f;
		}
		return vector.magnitude * LineScript.MEASURE_MULTIPLIER;
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x000ABD88 File Offset: 0x000A9F88
	public void AddSelection(GameObject GO)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(GO);
		if (!networkPhysicsObject)
		{
			return;
		}
		this.AddHighlight(networkPhysicsObject, false);
		this.selectionWasChanged = true;
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x000ABDB9 File Offset: 0x000A9FB9
	public void RemoveSelection(GameObject GO)
	{
		this.RemoveHighlight(NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(GO), false);
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x000ABDCD File Offset: 0x000A9FCD
	public void AddHighlight(NetworkPhysicsObject npo, bool withCtrl)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<NetworkPhysicsObject, bool>(RPCTarget.Server, new Action<NetworkPhysicsObject, bool>(this.RPCAddHighlight), npo, withCtrl);
			return;
		}
		this.RPCAddHighlight(npo, withCtrl);
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000ABDFC File Offset: 0x000A9FFC
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCAddHighlight(NetworkPhysicsObject npo, bool withCtrl)
	{
		if (npo && this.HighLightedObjects.Count < 128 && !this.HighLightedObjects.Contains(npo))
		{
			GameObject gameObject = npo.gameObject;
			if (!withCtrl)
			{
				LuaGameObjectScript component = gameObject.GetComponent<LuaGameObjectScript>();
				if (component != null && !EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Select, component))
				{
					return;
				}
			}
			if (withCtrl && !this.CtrlHighLightedObjects.Contains(gameObject))
			{
				this.CtrlHighLightedObjects.Add(gameObject);
			}
			if (base.networkView.isMine)
			{
				gameObject.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
			}
			this.HighLightedObjects.Add(npo);
			if (Network.isServer && base.networkView.owner != Network.player)
			{
				base.networkView.RPC<NetworkPhysicsObject, bool>(base.networkView.owner, new Action<NetworkPhysicsObject, bool>(this.RPCAddHighlight), npo, withCtrl);
			}
		}
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x000ABEF4 File Offset: 0x000AA0F4
	public void RemoveHighlight(NetworkPhysicsObject npo, bool withCtrl)
	{
		if (Network.isServer)
		{
			if (base.networkView.owner != Network.player)
			{
				base.networkView.RPC<NetworkPhysicsObject, bool>(base.networkView.owner, new Action<NetworkPhysicsObject, bool>(this.RPCRemoveHighlight), npo, withCtrl);
			}
		}
		else
		{
			base.networkView.RPC<NetworkPhysicsObject, bool>(RPCTarget.Server, new Action<NetworkPhysicsObject, bool>(this.RPCRemoveHighlight), npo, withCtrl);
		}
		this.RPCRemoveHighlight(npo, withCtrl);
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x000ABF68 File Offset: 0x000AA168
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCRemoveHighlight(NetworkPhysicsObject npo, bool withCtrl)
	{
		if (npo && this.HighLightedObjects.Contains(npo))
		{
			GameObject gameObject = npo.gameObject;
			if (withCtrl && this.CtrlHighLightedObjects.Contains(gameObject))
			{
				this.CtrlHighLightedObjects.Remove(gameObject);
			}
			if (base.networkView.isMine)
			{
				gameObject.GetComponent<Highlighter>().ConstantOff(0.25f);
			}
			this.HighLightedObjects.Remove(npo);
		}
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x000ABFDC File Offset: 0x000AA1DC
	public void ResetHighlight()
	{
		if (this.HighLightedObjects.Count == 0)
		{
			return;
		}
		if (Network.isServer)
		{
			if (base.networkView.owner != Network.player)
			{
				base.networkView.RPC(base.networkView.owner, new Action(this.RPCResetHighlight));
			}
		}
		else
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.RPCResetHighlight));
		}
		this.RPCResetHighlight();
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x000AC058 File Offset: 0x000AA258
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void RPCResetHighlight()
	{
		if (base.networkView.isMine)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in this.HighLightedObjects)
			{
				if (networkPhysicsObject)
				{
					networkPhysicsObject.GetComponent<Highlighter>().ConstantOff(0.25f);
				}
			}
		}
		this.HighLightedObjects.Clear();
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x000AC0D4 File Offset: 0x000AA2D4
	public IEnumerator AddRecentlyDropped(NetworkPhysicsObject npo)
	{
		this.RecentlyDropped.Add(npo);
		yield return new WaitForSeconds(1f);
		this.CleanupRecentlyDropped();
		yield break;
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x000AC0EA File Offset: 0x000AA2EA
	private void CleanupRecentlyDropped()
	{
		this.RecentlyDropped.Clear();
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000AC0F7 File Offset: 0x000AA2F7
	public void ResetAllObjects()
	{
		this.ResetInfoObject();
		this.ResetHiddenZoneObject();
		this.ResetRandomizeObject();
		this.ResetHandObject();
		this.ResetLayoutZoneObject();
		this.ResetFogOfWarObject();
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x000AC120 File Offset: 0x000AA320
	public void ResetInfoObject()
	{
		this.NetworkInstance.GUIContextualName.GetComponent<UIInput>().RemoveFocus();
		this.NetworkInstance.GUIContextualDesc.GetComponent<UIInput>().RemoveFocus();
		this.NetworkInstance.GUIContextualValue.GetComponent<UIInput>().RemoveFocus();
		this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIInput>().RemoveFocus();
		UICamera.selectedObject = null;
		if (this.InfoObject)
		{
			if (!this.HighLightedObjects.Contains(this.InfoObject.GetComponent<NetworkPhysicsObject>()))
			{
				this.InfoObject.GetComponent<Highlighter>().ConstantOff(0.25f);
			}
			this.InfoObject = null;
			GUIUtility.hotControl = 0;
		}
		NetworkSingleton<NetworkUI>.Instance.GUIContextualGlobalMenu.SetActive(false);
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x000AC1DE File Offset: 0x000AA3DE
	public void ResetHiddenZoneObject()
	{
		if (this.InfoHiddenZoneGO)
		{
			this.InfoHiddenZoneGO.GetComponent<Highlighter>().ConstantOff(0.25f);
			this.InfoHiddenZoneGO = null;
		}
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x000AC209 File Offset: 0x000AA409
	public void ResetHandObject()
	{
		if (this.InfoHandObject)
		{
			this.InfoHandObject.GetComponent<Highlighter>().ConstantOff(0.25f);
			this.InfoHandObject = null;
		}
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x000AC234 File Offset: 0x000AA434
	public void ResetRandomizeObject()
	{
		if (this.InfoRandomizeZoneGO)
		{
			this.InfoRandomizeZoneGO.GetComponent<Highlighter>().ConstantOff(0.25f);
			this.InfoRandomizeZoneGO = null;
		}
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000AC25F File Offset: 0x000AA45F
	public void ResetFogOfWarObject()
	{
		if (this.InfoFogOfWarZoneGO)
		{
			this.InfoFogOfWarZoneGO.GetComponent<Highlighter>().ConstantOff(0.25f);
			this.InfoFogOfWarZoneGO = null;
		}
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000AC28C File Offset: 0x000AA48C
	public void ResetLayoutZoneObject()
	{
		if (this.NetworkInstance)
		{
			this.NetworkInstance.GUILayoutZoneSettings.Cancel();
		}
		if (this.InfoLayoutZoneGO)
		{
			this.InfoLayoutZoneGO.GetComponent<Highlighter>().ConstantOff(0.25f);
			this.InfoLayoutZoneGO = null;
		}
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x000AC2E0 File Offset: 0x000AA4E0
	public void CheckDeckReset(GameObject GO)
	{
		if (GO.CompareTag("Card"))
		{
			NetworkSingleton<CardManagerScript>.Instance.SetupCard(GO, GO.GetComponent<CardScript>().card_id_, -1, false);
		}
		if (GO.CompareTag("Deck"))
		{
			NetworkSingleton<CardManagerScript>.Instance.SetupCard(GO, GO.GetComponent<DeckScript>().get_bottom_card_id(), GO.GetComponent<DeckScript>().get_top_card_id(), false);
		}
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x000AC344 File Offset: 0x000AA544
	private Vector3 ScreenPointToWorldPoint(Vector3 ScreenPoint)
	{
		int num = Physics.RaycastNonAlloc(this.MainCamera.ScreenPointToRay(ScreenPoint), Pointer.raycastHits, 1000f, HoverScript.GrabbableLayerMask);
		Array.Sort<RaycastHit>(Pointer.raycastHits, 0, num, new RaycastHitComparator());
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = Pointer.raycastHits[i];
			if (raycastHit.collider.gameObject.CompareTag("Surface"))
			{
				return raycastHit.point;
			}
		}
		return Vector3.zero;
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x000AC3C0 File Offset: 0x000AA5C0
	private GameObject CheckForTag(string Tag)
	{
		if (string.IsNullOrEmpty(Tag))
		{
			return this.HoverObject;
		}
		if (this.HoverObject && this.HoverObject.CompareTag(Tag))
		{
			return this.HoverObject;
		}
		return null;
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x000AC3F4 File Offset: 0x000AA5F4
	public void Shake(int touch_id = -1)
	{
		if (Network.isClient)
		{
			return;
		}
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			GameObject gameObject = networkPhysicsObject.gameObject;
			if (networkPhysicsObject && networkPhysicsObject.HeldByPlayerID == this.ID && (touch_id == -1 || networkPhysicsObject.HeldByTouchID == touch_id))
			{
				if (this.num_grabbed <= 64 && !ServerOptions.isPhysicsLock)
				{
					networkPhysicsObject.HeldOffset = Vector3.zero;
					networkPhysicsObject.HeldByControllerPickupOffset = new Vector3(0f, 0.8f, 0.5f);
					if (this.FirstGrabbedNPO)
					{
						networkPhysicsObject.HeldSpinRotationIndex = this.FirstGrabbedNPO.HeldSpinRotationIndex;
						networkPhysicsObject.HeldFlipRotationIndex = this.FirstGrabbedNPO.HeldFlipRotationIndex;
						networkPhysicsObject.HeldRotationOffset = Vector3.zero;
					}
					else
					{
						this.FirstGrabbedObject = base.gameObject;
					}
				}
				if ((gameObject.CompareTag("Dice") || gameObject.CompareTag("Deck") || gameObject.CompareTag("Coin") || gameObject.CompareTag("Bag") || gameObject.CompareTag("Infinite") || gameObject.CompareTag("Superfight") || (gameObject.CompareTag("GoPiece") && gameObject.GetComponentInParent<SoundMaterial>().soundMaterialType == SoundMaterialType.Wood)) && NetworkSingleton<ManagerPhysicsObject>.Instance.CheckLuaTryRandomize(networkPhysicsObject, this.ID))
				{
					if (gameObject.CompareTag("Dice") || gameObject.CompareTag("Coin"))
					{
						if (networkPhysicsObject.soundScript)
						{
							networkPhysicsObject.soundScript.ShakeSound();
						}
						networkPhysicsObject.rigidbody.rotation = UnityEngine.Random.rotation;
						EventManager.TriggerObjectRandomize(networkPhysicsObject, this.PointerColorLabel);
					}
					else if (networkPhysicsObject.deckScript)
					{
						networkPhysicsObject.deckScript.RandomizeDeck();
						EventManager.TriggerObjectRandomize(networkPhysicsObject, this.PointerColorLabel);
					}
					else if (networkPhysicsObject.stackObject)
					{
						networkPhysicsObject.stackObject.ShuffleBag();
						EventManager.TriggerObjectRandomize(networkPhysicsObject, this.PointerColorLabel);
					}
				}
			}
		}
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000AC634 File Offset: 0x000AA834
	public bool CheckForZoneMenu(Vector2 ScreenPos, string Tag)
	{
		if (!PermissionsOptions.CheckIfAllowedInMode(PointerMode.Hidden, -1))
		{
			return false;
		}
		RaycastHit[] array = Physics.RaycastAll(this.MainCamera.ScreenPointToRay(ScreenPos), 1000f, HoverScript.GrabbableLayerMask);
		Array.Sort<RaycastHit>(array, new RaycastHitComparator());
		foreach (RaycastHit raycastHit in array)
		{
			if (NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider).CompareTag(Tag))
			{
				this.CheckStartZones(Input.mousePosition);
				return true;
			}
		}
		foreach (RaycastHit raycastHit2 in array)
		{
			if (raycastHit2.collider.gameObject.CompareTag(Tag))
			{
				this.CheckStartZones(Input.mousePosition);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x000AC6F4 File Offset: 0x000AA8F4
	public bool CheckDeleteTag(Vector2 ScreenPos, string Tag)
	{
		RaycastHit[] array = Physics.RaycastAll(this.MainCamera.ScreenPointToRay(ScreenPos), 1000f, HoverScript.GrabbableLayerMask);
		Array.Sort<RaycastHit>(array, new RaycastHitComparator());
		foreach (RaycastHit raycastHit in array)
		{
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider);
			if (gameObject.CompareTag(Tag))
			{
				this.DestroyZone(gameObject);
				return true;
			}
		}
		foreach (RaycastHit raycastHit2 in array)
		{
			GameObject gameObject2 = raycastHit2.collider.gameObject;
			if (gameObject2.CompareTag(Tag))
			{
				this.DestroyZone(gameObject2);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x000AC7A8 File Offset: 0x000AA9A8
	private void DestroyZone(GameObject gameObject)
	{
		if (!gameObject)
		{
			return;
		}
		NetworkView component = gameObject.GetComponent<NetworkView>();
		if (component)
		{
			base.networkView.RPC<NetworkView>(RPCTarget.Server, new Action<NetworkView>(this.RPCDestroyZone), component);
			return;
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000AC7F0 File Offset: 0x000AA9F0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Zones", SerializationMethod.Default)]
	private void RPCDestroyZone(NetworkView networkView)
	{
		GameObject gameObject = networkView.gameObject;
		if (!gameObject || !networkView.GetComponent<Zone>())
		{
			return;
		}
		LuaGameObjectScript component = gameObject.GetComponent<LuaGameObjectScript>();
		if (component && !EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Delete, component))
		{
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(gameObject);
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000AC844 File Offset: 0x000AAA44
	private bool CleanupZone(PointerMode mode)
	{
		if (mode != this.currentZoneMode && this.currentZoneMode != PointerMode.None)
		{
			this.EndZone(this.currentZoneMode);
			return true;
		}
		return !Pointer.IsZoneTool(mode);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000AC870 File Offset: 0x000AAA70
	public static string PointerModeToTag(PointerMode mode)
	{
		if (mode <= PointerMode.Randomize)
		{
			if (mode == PointerMode.Hidden)
			{
				return "Fog";
			}
			if (mode == PointerMode.Randomize)
			{
				return "Randomize";
			}
		}
		else
		{
			switch (mode)
			{
			case PointerMode.Hands:
				return "Hand";
			case PointerMode.Scripting:
				return "Scripting";
			case PointerMode.VolumeScale:
				break;
			case PointerMode.Decal:
				return "Decal";
			default:
				if (mode == PointerMode.FogOfWar)
				{
					return "FogOfWar";
				}
				if (mode == PointerMode.LayoutZone)
				{
					return "Layout";
				}
				break;
			}
		}
		return "";
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000AC8E0 File Offset: 0x000AAAE0
	public void StartZone(PointerMode mode, Vector3 WorldPos, Vector2 ScreenPos, GameObject DeleteZone = null)
	{
		if (this.CleanupZone(mode))
		{
			return;
		}
		UnityEngine.Object.Destroy(this.ZoneObject);
		string text = Pointer.PointerModeToTag(mode);
		if (text == "")
		{
			return;
		}
		if (DeleteZone)
		{
			this.DestroyZone(DeleteZone);
			return;
		}
		if (!this.CheckForZoneMenu(ScreenPos, text))
		{
			this.ZoneObject = null;
			if (mode <= PointerMode.Hands)
			{
				if (mode != PointerMode.Hidden)
				{
					if (mode != PointerMode.Randomize)
					{
						if (mode == PointerMode.Hands)
						{
							this.ZoneObject = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.HandTrigger, WorldPos, Quaternion.identity);
							this.ZoneObject.GetComponent<HandZone>().TriggerLabel = this.PointerColorLabel;
						}
					}
					else
					{
						this.ZoneObject = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.RandomizeTrigger, WorldPos, Quaternion.identity);
					}
				}
				else
				{
					this.ZoneObject = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.FogOfWarTrigger, WorldPos, Quaternion.identity);
					this.ZoneObject.GetComponent<HiddenZone>().SetOwningColor(this.PointerColorLabel);
				}
			}
			else if (mode != PointerMode.Scripting)
			{
				if (mode != PointerMode.FogOfWar)
				{
					if (mode == PointerMode.LayoutZone)
					{
						this.ZoneObject = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.LayoutZone, WorldPos, Quaternion.identity);
					}
				}
				else
				{
					this.ZoneObject = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.FogOfWar, WorldPos, Quaternion.identity);
				}
			}
			else
			{
				this.ZoneObject = UnityEngine.Object.Instantiate<GameObject>(NetworkSingleton<GameMode>.Instance.ScriptingTrigger, WorldPos, Quaternion.identity);
			}
			this.StartZoneVector = WorldPos;
			UnityEngine.Object.Destroy(this.ZoneObject.GetComponent<NetworkPhysicsObject>());
			this.ZoneObject.GetComponent<BoxCollider>().enabled = false;
			this.currentZoneMode = mode;
			return;
		}
		this.justStartedZoneMenu = true;
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000ACA88 File Offset: 0x000AAC88
	public void UpdateZone(PointerMode mode, Vector3 WorldPos, Vector2 ScreenPos, bool SelectionUsingLaser = false)
	{
		if (this.CleanupZone(mode))
		{
			return;
		}
		if (this.StartZoneVector == Vector3.zero || !this.ZoneObject)
		{
			return;
		}
		float num = 5.1f;
		if (VRHMD.isVR && VRTrackedController.ControllerStyle == TrackedControllerStyle.New)
		{
			num = VRTrackedController.SELECTION_HEIGHT;
			if (!SelectionUsingLaser)
			{
				if (VRTrackedController.VRSelectionMode == VRTrackedController.VRSelectionStyle.Exact)
				{
					num = WorldPos.y - this.StartZoneVector.y;
				}
				else if (VRTrackedController.VRSelectionMode == VRTrackedController.VRSelectionStyle.Anchored)
				{
					num = -this.StartZoneVector.y;
				}
				else
				{
					num = -num;
				}
			}
		}
		else if (mode == PointerMode.Hands)
		{
			num = 7f;
		}
		else if (mode == PointerMode.LayoutZone)
		{
			num = 14f;
		}
		Vector3 normalized = this.MainCamera.transform.right.normalized;
		float num2 = WorldPos.x - this.StartZoneVector.x;
		float num3 = WorldPos.z - this.StartZoneVector.z;
		float num4 = normalized.x * num2 + normalized.z * num3;
		float num5 = normalized.x * num3 - normalized.z * num2;
		num4 = Mathf.Abs(num4);
		num5 = Mathf.Abs(num5);
		Vector3 localScale = new Vector3(num4, num, num5);
		this.ZoneObject.transform.localScale = localScale;
		this.ZoneObject.AddMissingComponent<Highlighter>().On(this.SelectColor);
		this.ZoneObject.transform.eulerAngles = new Vector3(0f, this.MainCamera.transform.eulerAngles.y, 0f);
		this.ZoneObject.transform.position = new Vector3((WorldPos.x + this.StartZoneVector.x) / 2f, this.StartZoneVector.y + num / 2f, (WorldPos.z + this.StartZoneVector.z) / 2f);
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x000ACC68 File Offset: 0x000AAE68
	public void EndZone(PointerMode mode)
	{
		if (this.StartZoneVector == Vector3.zero || !this.ZoneObject)
		{
			return;
		}
		this.StartZoneVector = Vector3.zero;
		this.currentZoneMode = PointerMode.None;
		if ((this.ZoneObject.GetComponent<BoxCollider>().size.x > 1f && this.ZoneObject.GetComponent<BoxCollider>().size.z > 1f) || (this.ZoneObject.transform.localScale.x > 1f && this.ZoneObject.transform.localScale.z > 1f))
		{
			if (mode <= PointerMode.Hands)
			{
				if (mode != PointerMode.Hidden)
				{
					if (mode != PointerMode.Randomize)
					{
						if (mode == PointerMode.Hands)
						{
							TableScript tableScript = NetworkSingleton<ManagerPhysicsObject>.Instance.TableScript;
							if (tableScript)
							{
								HandTransformState handTransform = tableScript.GetHandTransform(this.ZoneObject.transform);
								tableScript.LoadHand(handTransform);
							}
						}
					}
					else
					{
						this.SpawnRandomizeZone(this.ZoneObject.transform.position, this.ZoneObject.transform.rotation, this.ZoneObject.transform.localScale);
					}
				}
				else
				{
					this.SpawnHiddenZone(this.ZoneObject.transform.position, this.ZoneObject.transform.rotation, this.ZoneObject.transform.localScale);
				}
			}
			else if (mode != PointerMode.Scripting)
			{
				if (mode != PointerMode.FogOfWar)
				{
					if (mode == PointerMode.LayoutZone)
					{
						this.SpawnLayoutZone(this.ZoneObject.transform.position, this.ZoneObject.transform.rotation, this.ZoneObject.transform.localScale);
					}
				}
				else
				{
					this.SpawnFogOfWarZone(this.ZoneObject.transform.position, this.ZoneObject.transform.rotation, this.ZoneObject.transform.localScale);
				}
			}
			else
			{
				this.SpawnScriptingZone(this.ZoneObject.transform.position, this.ZoneObject.transform.rotation, this.ZoneObject.transform.localScale);
			}
		}
		UnityEngine.Object.Destroy(this.ZoneObject);
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x000ACEB0 File Offset: 0x000AB0B0
	private Vector3 level(Vector3 position, Vector3 to, bool force = false)
	{
		if (!this.MeasuringXZ && !force)
		{
			return position;
		}
		Vector3 result = position;
		result.y = to.y;
		return result;
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x000ACEDC File Offset: 0x000AB0DC
	private Vector3 set_y(Vector3 position, float y)
	{
		Vector3 result = position;
		result.y = y;
		return result;
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x000ACEF4 File Offset: 0x000AB0F4
	public bool IsMeasureMovement()
	{
		return LineScript.MEASURE_OBJECTS && this.pickedUpMeasuredObject;
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x000ACF08 File Offset: 0x000AB108
	public void StartLine(Vector3 pos, NetworkPhysicsObject hoverObject)
	{
		if (!PermissionsOptions.CheckAllowSender(PermissionsOptions.options.Line))
		{
			return;
		}
		this.PrevDisablePointerMode = this.CurrentPointerMode;
		this.startedLineWhileHolding = null;
		this.startedLineWhileHovering = null;
		if (zInput.GetButtonDown("Line Tool", ControlType.All) || this.IsMeasureMovement())
		{
			this.CurrentPointerMode = PointerMode.Line;
		}
		if (this.CurrentPointerMode == PointerMode.Line && LineScript.MEASURE_OBJECTS)
		{
			if (LineScript.GRID_MEASUREMENTS && !zInput.GetButton("Ctrl", ControlType.All))
			{
				pos = NetworkSingleton<ManagerPhysicsObject>.Instance.GridSnapPosition(pos);
			}
			if (this.FirstGrabbedNPO != null)
			{
				this.startedLineWhileHolding = this.FirstGrabbedNPO;
				pos = this.level(this.FirstGrabbedNPO.PickedUpPosition, pos, false);
			}
			else if (hoverObject != null && hoverObject != null && !zInput.GetButton("Ctrl", ControlType.All))
			{
				this.startedLineWhileHovering = hoverObject;
				pos = this.level(hoverObject.transform.position, pos, LineScript.MEASURE_HOVERED_FROM_EDGE);
				if (!LineScript.MEASURE_FLAT)
				{
					float num = hoverObject.transform.position.y - hoverObject.GetBounds().extents.y;
					if (pos.y < num)
					{
						pos.y = num;
					}
				}
			}
		}
		this.LineHeldTime = 0f;
		this.hasLoggedMeasurementStart = false;
		if (base.networkView.isMine)
		{
			this.StartLineVector = pos;
			this.EndLineVector = this.StartLineVector;
			this.LineVectorInComponents = (this.CurrentPointerMode == PointerMode.Line && zInput.GetButton("Shift", ControlType.All));
		}
		if (this.CurrentPointerMode == PointerMode.Flick || Pointer.IsCombineTool(this.CurrentPointerMode))
		{
			if (hoverObject)
			{
				this.AddHighlight(hoverObject, false);
			}
			if (this.HighLightedObjects.Count == 0)
			{
				this.StartLineVector = Vector3.zero;
				this.EndLineVector = Vector3.zero;
			}
		}
		this.CalculateLineInstances(true);
		if (VRHMD.isVR)
		{
			VRTrackedController.UpdateBothHoverText(true);
		}
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x000AD0F0 File Offset: 0x000AB2F0
	private void CalculateLineInstances(bool draw = true)
	{
		Vector3 startLineVector = this.StartLineVector;
		Vector3 endLineVector = this.EndLineVector;
		for (int i = 0; i < 5; i++)
		{
			this.LineInstances[i].enabled = false;
		}
		if (!draw || (startLineVector == endLineVector && startLineVector == Vector3.zero))
		{
			return;
		}
		this.LineInstances[0].enabled = true;
		this.LineInstances[0].positionCount = 2;
		this.LineInstances[0].SetPosition(0, startLineVector);
		this.LineInstances[0].SetPosition(1, endLineVector);
		if (Vector3.Distance(startLineVector, endLineVector) < 0.25f)
		{
			return;
		}
		if (this.CurrentPointerMode == PointerMode.Flick)
		{
			Vector3 point = (endLineVector - startLineVector).normalized * Pointer.LineToolArrowHeadLength;
			this.LineInstances[1].enabled = true;
			this.LineInstances[1].positionCount = 3;
			this.LineInstances[1].SetPosition(0, startLineVector + Quaternion.Euler(0f, -20f, 0f) * point);
			this.LineInstances[1].SetPosition(1, startLineVector);
			this.LineInstances[1].SetPosition(2, startLineVector + Quaternion.Euler(0f, 20f, 0f) * point);
			return;
		}
		if (Pointer.IsCombineTool(this.CurrentPointerMode))
		{
			Vector3 point = (startLineVector - endLineVector).normalized * Pointer.LineToolArrowHeadLength;
			this.LineInstances[1].enabled = true;
			this.LineInstances[1].positionCount = 3;
			this.LineInstances[1].SetPosition(0, endLineVector + Quaternion.Euler(0f, -45f, 0f) * point);
			this.LineInstances[1].SetPosition(1, endLineVector);
			this.LineInstances[1].SetPosition(2, endLineVector + Quaternion.Euler(0f, 45f, 0f) * point);
			return;
		}
		if (this.CurrentPointerMode == PointerMode.Line)
		{
			if (this.LineVectorInComponents)
			{
				Vector3 vector = new Vector3(endLineVector.x, startLineVector.y, this.MeasuringXZ ? startLineVector.z : endLineVector.z);
				this.LineInstances[0].positionCount = 3;
				this.LineInstances[0].SetPosition(1, vector);
				this.LineInstances[0].SetPosition(2, endLineVector);
				if (Pointer.MeasureToolArrowHeadAngle > 0f)
				{
					Vector3 point = (vector - startLineVector).normalized * Pointer.LineToolArrowHeadLength;
					this.LineInstances[1].enabled = true;
					this.LineInstances[1].positionCount = 3;
					this.LineInstances[1].SetPosition(1, startLineVector);
					this.LineInstances[1].SetPosition(0, startLineVector + Quaternion.Euler(0f, -Pointer.MeasureToolArrowHeadAngle, 0f) * point);
					this.LineInstances[1].SetPosition(2, startLineVector + Quaternion.Euler(0f, Pointer.MeasureToolArrowHeadAngle, 0f) * point);
					point = (vector - endLineVector).normalized * Pointer.LineToolArrowHeadLength;
					this.LineInstances[2].enabled = true;
					this.LineInstances[2].positionCount = 3;
					this.LineInstances[2].SetPosition(1, endLineVector);
					if (this.MeasuringXZ)
					{
						this.LineInstances[2].SetPosition(0, endLineVector + Quaternion.Euler(0f, -Pointer.MeasureToolArrowHeadAngle, 0f) * point);
						this.LineInstances[2].SetPosition(2, endLineVector + Quaternion.Euler(0f, Pointer.MeasureToolArrowHeadAngle, 0f) * point);
						return;
					}
					float num = Mathf.Atan2((endLineVector - startLineVector).z, (endLineVector - startLineVector).x) * 57.29578f;
					this.LineInstances[2].SetPosition(0, endLineVector + Quaternion.Euler(-Pointer.MeasureToolArrowHeadAngle, -num, 0f) * point);
					this.LineInstances[2].SetPosition(2, endLineVector + Quaternion.Euler(Pointer.MeasureToolArrowHeadAngle, -num, 0f) * point);
					return;
				}
			}
			else if (Pointer.MeasureToolArrowHeadAngle > 0f)
			{
				float num2 = 0f;
				if (endLineVector.y != startLineVector.y)
				{
					num2 = (endLineVector.y - startLineVector.y) * (Mathf.Cos(Pointer.MeasureToolArrowHeadAngle * 0.017453292f) * Pointer.LineToolArrowHeadLength) / (endLineVector - startLineVector).magnitude;
				}
				Vector3 point = (endLineVector - startLineVector).normalized * Pointer.LineToolArrowHeadLength;
				this.LineInstances[1].enabled = true;
				this.LineInstances[1].positionCount = 3;
				this.LineInstances[1].SetPosition(0, this.set_y(startLineVector + Quaternion.Euler(0f, -Pointer.MeasureToolArrowHeadAngle, 0f) * point, startLineVector.y + num2));
				this.LineInstances[1].SetPosition(1, startLineVector);
				this.LineInstances[1].SetPosition(2, this.set_y(startLineVector + Quaternion.Euler(0f, Pointer.MeasureToolArrowHeadAngle, 0f) * point, startLineVector.y + num2));
				point = (startLineVector - endLineVector).normalized * Pointer.LineToolArrowHeadLength;
				this.LineInstances[2].enabled = true;
				this.LineInstances[2].positionCount = 3;
				this.LineInstances[2].SetPosition(0, this.set_y(endLineVector + Quaternion.Euler(0f, -Pointer.MeasureToolArrowHeadAngle, 0f) * point, endLineVector.y - num2));
				this.LineInstances[2].SetPosition(1, endLineVector);
				this.LineInstances[2].SetPosition(2, this.set_y(endLineVector + Quaternion.Euler(0f, Pointer.MeasureToolArrowHeadAngle, 0f) * point, endLineVector.y - num2));
			}
		}
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x000AD7B4 File Offset: 0x000AB9B4
	public void UpdateLine(Vector3 pos)
	{
		if (!PermissionsOptions.CheckAllowSender(PermissionsOptions.options.Line))
		{
			return;
		}
		if (base.networkView.isMine)
		{
			this.LineVectorInComponents = (this.CurrentPointerMode == PointerMode.Line && zInput.GetButton("Shift", ControlType.All));
			bool flag = zInput.GetButton("Ctrl", ControlType.All) && !this.LineVectorInComponents;
			bool flag2 = LineScript.MEASURE_FLAT;
			if (zInput.GetButton("Ctrl", ControlType.All) && this.LineVectorInComponents)
			{
				flag2 = !flag2;
			}
			this.MeasuringXZ = flag2;
			this.LineHeldTime += Time.deltaTime;
			if (this.CurrentPointerMode == PointerMode.Line)
			{
				this.zeroMeasurement = false;
				Vector3 endLineVector;
				if (LineScript.MEASURE_OBJECTS && LineScript.GRID_MEASUREMENTS && !flag)
				{
					endLineVector = NetworkSingleton<ManagerPhysicsObject>.Instance.GridSnapPosition(pos);
				}
				else
				{
					endLineVector = pos;
				}
				if (!this.hasLoggedMeasurementStart && this.LineHeldTime > 0.3f)
				{
					this.hasLoggedMeasurementStart = true;
				}
				if (this.startedLineWhileHolding)
				{
					Vector3 position;
					if (Singleton<ObjectPositioningVisualizer>.Instance.npoToVisualize == this.startedLineWhileHolding && Singleton<ObjectPositioningVisualizer>.Instance.SpawnOK)
					{
						endLineVector = this.level(Singleton<ObjectPositioningVisualizer>.Instance.SpawnLocation, this.StartLineVector, false);
					}
					else if (NetworkSingleton<ManagerPhysicsObject>.Instance.CheckGridSnap(this.startedLineWhileHolding.gameObject, out position, 6.5f))
					{
						endLineVector = this.level(position, this.StartLineVector, false);
					}
					else
					{
						endLineVector = this.level(this.startedLineWhileHolding.transform.position, this.StartLineVector, false);
					}
				}
				else if (this.startedLineWhileHovering)
				{
					if (this.HoverObject && !flag)
					{
						NetworkPhysicsObject component = this.HoverObject.GetComponent<NetworkPhysicsObject>();
						if (component)
						{
							if (LineScript.MEASURE_HOVERED_FROM_EDGE)
							{
								Vector3 vector = this.level(LibVector.nearestPointOnNPO(this.startedLineWhileHovering.transform.position, component), this.StartLineVector, false);
								if (vector != Vector3.zero)
								{
									pos = vector;
									vector = this.level(LibVector.nearestPointOnNPO(pos, this.startedLineWhileHovering), this.StartLineVector, true);
									if (vector != Vector3.zero)
									{
										this.StartLineVector = this.level(pos, this.StartLineVector, true);
										vector = LibVector.nearestPointOnNPO(this.StartLineVector, component);
										if (vector != Vector3.zero)
										{
											pos = this.level(vector, this.StartLineVector, false);
										}
									}
									endLineVector = pos;
								}
							}
							else
							{
								pos = this.level(component.transform.position, pos, false);
								endLineVector = pos;
							}
						}
					}
					if (LineScript.MEASURE_HOVERED_FROM_EDGE)
					{
						Vector3 vector2 = LibVector.nearestPointOnNPO(pos, this.startedLineWhileHovering);
						if (vector2 == Vector3.zero)
						{
							this.StartLineVector = pos;
							this.zeroMeasurement = true;
						}
						else
						{
							this.StartLineVector = this.level(vector2, this.StartLineVector, true);
						}
					}
					else
					{
						this.StartLineVector = this.level(this.startedLineWhileHovering.transform.position, this.StartLineVector, true);
					}
				}
				this.EndLineVector = endLineVector;
			}
			else
			{
				this.EndLineVector = pos;
			}
			this.EndLineVector = this.level(this.EndLineVector, this.StartLineVector, false);
		}
		this.CalculateLineInstances(true);
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x000ADAF0 File Offset: 0x000ABCF0
	public void EndLine(Vector3 pos, GameObject hoverObject)
	{
		this.EndLine(pos, (hoverObject != null) ? hoverObject.GetComponent<NetworkPhysicsObject>() : null);
	}

	// Token: 0x06001964 RID: 6500 RVA: 0x000ADB0C File Offset: 0x000ABD0C
	public void EndLine(Vector3 pos, NetworkPhysicsObject hoverObject = null)
	{
		if (this.CurrentPointerMode == PointerMode.Line)
		{
			this.measuringPickedUpObject = false;
			if (this.StartLineVector != Vector3.zero && this.LineHeldTime < 0.3f && Time.time > this.LastArrowSpawnTime + this.ArrowCooldown)
			{
				Vector3 pos2 = new Vector3(pos.x, pos.y + 0.75f, pos.z);
				if (hoverObject)
				{
					pos2.y = hoverObject.transform.position.y + hoverObject.GetBounds().extents.y + 0.75f;
					hoverObject.HighlightOn(this.PointerColour, 4f, 3);
				}
				this.SpawnArrow(pos2);
			}
			else if (this.hasLoggedMeasurementStart)
			{
				base.networkView.RPC<Vector3, NetworkPhysicsObject>(RPCTarget.All, new Action<Vector3, NetworkPhysicsObject>(this.LogMeasurement), this.EndLineVector - this.StartLineVector, this.startedLineWhileHolding ? this.startedLineWhileHolding : this.startedLineWhileHovering);
			}
			if (VRHMD.isVR)
			{
				VRTrackedController.SetBothHoverText("");
				VRTrackedController.UpdateBothHoverText(false);
			}
			this.startedLineWhileHovering = null;
		}
		if (this.CurrentPointerMode == PointerMode.Flick)
		{
			Vector3 b = new Vector3(pos.x, this.StartLineVector.y, pos.z);
			Vector3 force = this.StartLineVector - b;
			this.Flick(force);
			if (!zInput.GetButton("Ctrl", ControlType.All))
			{
				this.ResetHighlight();
			}
		}
		else if (Pointer.IsCombineTool(this.CurrentPointerMode))
		{
			List<GameObject> selectedObjects = this.GetSelectedObjects(-1, true, false);
			if (selectedObjects.Count > 0)
			{
				GameObject gameObject = selectedObjects[selectedObjects.Count - 1];
				if (gameObject)
				{
					switch (this.CurrentPointerMode)
					{
					case PointerMode.Attach:
						this.Combine((hoverObject == null) ? this.GetHoverID() : hoverObject.ID, gameObject.GetComponent<NetworkPhysicsObject>().ID);
						break;
					case PointerMode.FixedJoint:
						this.JointFixed((hoverObject == null) ? this.GetHoverID() : hoverObject.ID, gameObject.GetComponent<NetworkPhysicsObject>().ID, new JointFixedState());
						break;
					case PointerMode.HingeJoint:
						this.JointHinge((hoverObject == null) ? this.GetHoverID() : hoverObject.ID, gameObject.GetComponent<NetworkPhysicsObject>().ID, new JointHingeState());
						break;
					case PointerMode.SpringJoint:
						this.JointSpring((hoverObject == null) ? this.GetHoverID() : hoverObject.ID, gameObject.GetComponent<NetworkPhysicsObject>().ID, new JointSpringState());
						break;
					}
				}
			}
			this.ResetHighlight();
		}
		this.StartLineVector = (this.EndLineVector = Vector3.zero);
		this.CalculateLineInstances(false);
		if (zInput.GetButtonUp("Line Tool", ControlType.All) || this.startedLineWhileHolding)
		{
			this.CurrentPointerMode = this.PrevDisablePointerMode;
		}
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x000ADE10 File Offset: 0x000AC010
	[Remote(Permission.Owner, validationFunction = "Permissions/Line")]
	public void LogMeasurement(Vector3 distanceVector, NetworkPhysicsObject npo)
	{
		if (!LineScript.LOG_MEASUREMENTS)
		{
			return;
		}
		Colour colour = Colour.ColourFromLabel(this.PointerColorLabel);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Colour.HexFromLabel(this.PointerColorLabel));
		stringBuilder.Append(NetworkSingleton<PlayerManager>.Instance.NameFromColour(colour));
		stringBuilder.Append("[DDDDDD] measured [FFFFFF]");
		if (npo)
		{
			if (!npo.IsHidden)
			{
				stringBuilder.Append(npo.tag);
				stringBuilder.Append(": ");
				stringBuilder.Append(npo.Name);
			}
			else
			{
				stringBuilder.Append("?");
			}
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				stringBuilder.Append(" {");
				stringBuilder.Append(npo.GUID);
				stringBuilder.Append("}");
			}
			stringBuilder.Append(" =");
		}
		stringBuilder.Append(" ");
		string value = "";
		if (!LineScript.GRID_MEASUREMENTS)
		{
			if (LineScript.METRIC_MEASUREMENTS)
			{
				value = "[DDDDDD]cm";
			}
			else if (LineScript.MEASURE_MULTIPLIER == 1f)
			{
				value = "[DDDDDD]\"";
			}
			else
			{
				value = "[DDDDDD]^";
			}
		}
		float num;
		float num2;
		float num3;
		stringBuilder.Append(this.calculateMeasuredDistance(distanceVector, out num, out num2, out num3).ToString("f2"));
		stringBuilder.Append(value);
		if (this.LineVectorInComponents)
		{
			stringBuilder.Append(" (");
			if (this.MeasuringXZ)
			{
				stringBuilder.Append(num.ToString("f2"));
			}
			else
			{
				stringBuilder.Append(Mathf.Sqrt(num * num + num3 * num3).ToString("f2"));
			}
			stringBuilder.Append(value);
			stringBuilder.Append(", ");
			if (this.MeasuringXZ)
			{
				stringBuilder.Append(num3.ToString("f2"));
			}
			else
			{
				stringBuilder.Append(num2.ToString("f2"));
			}
			stringBuilder.Append(value);
			stringBuilder.Append(")");
		}
		Chat.Log(stringBuilder.ToString(), colour, ChatMessageType.Game, false);
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x000AE012 File Offset: 0x000AC212
	public void LogInventoryDrag(string deckName)
	{
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.LogInventoryDragRPC), deckName);
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x000AE030 File Offset: 0x000AC230
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void LogInventoryDragRPC(string deckName)
	{
		if (!Pointer.LOG_INVENTORY)
		{
			return;
		}
		Colour colour = Colour.ColourFromLabel(this.PointerColorLabel);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Colour.HexFromLabel(this.PointerColorLabel));
		stringBuilder.Append(NetworkSingleton<PlayerManager>.Instance.NameFromColour(colour));
		stringBuilder.Append("[DDDDDD] ");
		stringBuilder.Append(Language.Translate("MANIPULATED_THE_DECK"));
		if (deckName != "")
		{
			stringBuilder.Append(": [FFFFFF]");
			stringBuilder.Append(deckName);
			stringBuilder.Append("[DDDDDD]");
		}
		Chat.Log(stringBuilder.ToString(), colour, ChatMessageType.Game, false);
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000AE0D4 File Offset: 0x000AC2D4
	public void ReStartContextual()
	{
		Vector3 position = this.NetworkInstance.GUIContextualMenu.transform.position;
		this.StartContextual(this.InfoObject, true);
		this.NetworkInstance.GUIContextualMenu.transform.position = position;
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x000AE11A File Offset: 0x000AC31A
	private void SetActive(GameObject item, bool enabled)
	{
		item.SetActive(enabled);
		if (enabled)
		{
			this.ActiveItems.Add(item);
		}
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000AE134 File Offset: 0x000AC334
	public void StartContextual(GameObject ContextualObject, bool bResetHighlight = true)
	{
		NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
		this.ActiveItems.Clear();
		this.InfoObject = ContextualObject;
		NetworkPhysicsObject component = this.InfoObject.GetComponent<NetworkPhysicsObject>();
		if (!this.HighLightedObjects.Contains(component))
		{
			if (!zInput.GetButton("Ctrl", ControlType.All) && bResetHighlight)
			{
				this.ResetHighlight();
			}
			this.AddHighlight(component, false);
		}
		bool canSeeName = component.CanSeeName;
		bool flag = !component.GetComponent<Zone>();
		if (canSeeName)
		{
			if (this.NetworkInstance.GUIContextualName.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualName.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
			}
			if (this.NetworkInstance.GUIContextualDesc.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualDesc.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
			}
			if (this.NetworkInstance.GUIContextualValue.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualValue.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
			}
			if (this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
			}
			this.NetworkInstance.GUIContextualName.GetComponent<UIInput>().value = component.Name;
			this.NetworkInstance.GUIContextualDesc.GetComponent<UIInput>().value = "1";
			this.NetworkInstance.GUIContextualDesc.GetComponent<UIInput>().value = component.Description;
			this.NetworkInstance.GUIContextualValue.GetComponent<UIInput>().value = "1";
			this.NetworkInstance.GUIContextualValue.GetComponent<UIInput>().value = component.Value.ToString();
			this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIInput>().value = "1";
			this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIInput>().value = component.GMNotes;
		}
		else
		{
			if (this.NetworkInstance.GUIContextualName.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualName.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			}
			if (this.NetworkInstance.GUIContextualDesc.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualDesc.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			}
			if (this.NetworkInstance.GUIContextualValue.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualValue.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			}
			if (this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIButton>())
			{
				this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNeutral];
			}
			this.NetworkInstance.GUIContextualName.GetComponent<UIInput>().value = "";
			this.NetworkInstance.GUIContextualDesc.GetComponent<UIInput>().value = "";
			this.NetworkInstance.GUIContextualValue.GetComponent<UIInput>().value = "";
			this.NetworkInstance.GUIContextualGMNotes.GetComponent<UIInput>().value = "";
		}
		this.SetActive(this.NetworkInstance.GUIContextualToggles, flag);
		this.SetActive(this.NetworkInstance.GUIContextualComponentTags, true);
		this.SetActive(this.NetworkInstance.GUIContextualSaveToChest, flag);
		this.SetActive(this.NetworkInstance.GUIContextualColorTint, flag);
		this.SetActive(this.NetworkInstance.GUIContextualFlip, flag);
		this.SetActive(this.NetworkInstance.GUIContextualRotateMenu, flag);
		this.SetActive(this.NetworkInstance.GUIContextualScaleMenu, flag);
		this.SetActive(this.NetworkInstance.GUIContextualClone, flag);
		this.SetActive(this.NetworkInstance.GUIContextualCopy, true);
		this.SetActive(this.NetworkInstance.GUIContextualDelete, true);
		this.NetworkInstance.GUIContextualMovementSpacer.SetActive(flag);
		if (flag)
		{
			this.NetworkInstance.GUIContextualLockBool.GetComponent<UIToggle>().value = component.IsLocked;
			this.NetworkInstance.GUIContextualDragSelectableBool.GetComponent<UIToggle>().value = component.IsDragSelectable;
			this.NetworkInstance.GUIContextualGridBool.GetComponent<UIToggle>().value = !component.IgnoresGrid;
			this.NetworkInstance.GUIContextualSnapBool.GetComponent<UIToggle>().value = !component.IgnoresSnap;
			this.NetworkInstance.GUIContextualDestroyableBool.GetComponent<UIToggle>().value = !component.DoesNotPersist;
			this.NetworkInstance.GUIContextualAutoraiseBool.GetComponent<UIToggle>().value = component.DoAutoRaise;
			this.NetworkInstance.GUIContextualStickyBool.GetComponent<UIToggle>().value = component.IsSticky;
			this.NetworkInstance.GUIContextualTooltipBool.GetComponent<UIToggle>().value = component.ShowTooltip;
			this.NetworkInstance.GUIContextualGridProjectionBool.GetComponent<UIToggle>().value = component.ShowGridProjection;
			this.NetworkInstance.GUIContextualHandsBool.GetComponent<UIToggle>().value = component.CanBeHeldInHand;
			this.NetworkInstance.GUIContextualHideFaceDownBool.GetComponent<UIToggle>().value = component.IsHiddenWhenFaceDown;
			this.NetworkInstance.GUIContextualIgnoreFogOfWarBool.GetComponent<UIToggle>().value = component.IgnoresFogOfWar;
			this.NetworkInstance.GUIContextualMeasureMovementBool.GetComponent<UIToggle>().value = component.ShowRulerWhenHeld;
			this.NetworkInstance.GUIContextualRevealActive.GetComponent<UIToggle>().value = this.InfoObject.GetComponent<FogOfWarRevealer>().Active;
		}
		this.SetActive(this.NetworkInstance.GUIContextualDeckDeal, this.InfoObject.GetComponent<DeckScript>() || this.InfoObject.GetComponent<StackObject>() || component.CanBeHeldInHand);
		this.SetActive(this.NetworkInstance.GUIContextualDraw, this.InfoObject.GetComponent<DeckScript>() || this.InfoObject.GetComponent<StackObject>() || component.CanBeHeldInHand);
		NetworkSingleton<NetworkUI>.Instance.handZoneToReveal = component.GetContainingHandZone();
		this.SetActive(this.NetworkInstance.GUIContextualHandReveal, NetworkSingleton<NetworkUI>.Instance.handZoneToReveal != null && NetworkSingleton<NetworkUI>.Instance.handZoneToReveal.TriggerLabel == this.PointerColorLabel);
		this.SetActive(this.NetworkInstance.GUIContextualDeckCut, this.InfoObject.tag == "Deck" || (this.InfoObject.GetComponent<StackObject>() && !this.InfoObject.GetComponent<StackObject>().bBag && !this.InfoObject.GetComponent<StackObject>().IsInfiniteStack));
		if (this.NetworkInstance.GUIContextualDeckCut.activeSelf)
		{
			UISlider componentInChildren = this.NetworkInstance.GUIContextualDeckCut.GetComponentInChildren<UISlider>(true);
			componentInChildren.value = 0.5f;
			int num = 2;
			int num2;
			if (this.InfoObject.tag == "Deck")
			{
				num2 = component.deckScript.num_cards_ - 2;
			}
			else
			{
				num2 = component.stackObject.num_objects_ - 2;
			}
			UILabel[] componentsInChildren = componentInChildren.GetComponentsInChildren<UILabel>(true);
			for (int i = 0; i < 2; i++)
			{
				UISliderRange component2 = componentsInChildren[i].gameObject.GetComponent<UISliderRange>();
				component2.Min = (float)num;
				component2.Max = (float)num2;
			}
		}
		this.SetActive(this.NetworkInstance.GUIContextualSplit, this.InfoObject.tag == "Deck" || (this.InfoObject.GetComponent<StackObject>() && !this.InfoObject.GetComponent<StackObject>().bBag && !this.InfoObject.GetComponent<StackObject>().IsInfiniteStack));
		if (this.NetworkInstance.GUIContextualSplit.activeSelf)
		{
			Transform transform = this.NetworkInstance.GUIContextualSplit.GetComponentsInChildren<UITable>(true)[0].transform;
			for (int j = 0; j < transform.childCount; j++)
			{
				GameObject gameObject = transform.GetChild(j).gameObject;
				int num3 = int.Parse(gameObject.name);
				int num4;
				if (this.InfoObject.tag == "Deck")
				{
					num4 = component.deckScript.num_cards_ / 2;
				}
				else
				{
					num4 = component.stackObject.num_objects_ / 2;
				}
				gameObject.SetActive(num3 <= num4 && num3 >= 2 && num3 <= 10);
			}
			transform.GetComponent<UITable>().repositionNow = true;
		}
		this.SetActive(this.NetworkInstance.GUIContextualDeckReset, this.InfoObject.CompareTag("Deck") || (this.InfoObject.GetComponent<StackObject>() && this.InfoObject.GetComponent<StackObject>().IsInfiniteStack && this.InfoObject.GetComponent<StackObject>().ObjectsHolder.Count > 0) || (component.GetSelectedStateId() != -1 && NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1)));
		bool flag2 = this.InfoObject.GetComponent<StackObject>() && this.InfoObject.GetComponent<StackObject>().bBag;
		bool flag3 = this.InfoObject.CompareTag("Deck");
		this.SetActive(this.NetworkInstance.GUIContextualSearch, flag3 || flag2);
		this.NetworkInstance.GUIContextualSearch.transform.Find("Arrow").gameObject.SetActive(flag3);
		this.NetworkInstance.GUIContextualSearch.GetComponent<UIHoverEnableObjects>().enabled = flag3;
		if (flag3)
		{
			Transform transform2 = this.NetworkInstance.GUIContextualSearch.GetComponentsInChildren<UITable>(true)[0].transform;
			int num5 = Mathf.Min(20, component.deckScript.num_cards_);
			for (int k = 0; k < transform2.childCount; k++)
			{
				GameObject gameObject2 = transform2.GetChild(k).gameObject;
				int num6 = int.Parse(gameObject2.name);
				gameObject2.SetActive(num6 >= 1 && num6 <= num5);
			}
			transform2.GetComponent<UITable>().repositionNow = true;
		}
		if (flag3 || (this.InfoObject.GetComponent<StackObject>() && this.InfoObject.GetComponent<StackObject>().bBag) || (component.GetSelectedStateId() != -1 || (this.InfoObject.CompareTag("Jigsaw Box") && NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))) || (component.CurrentPlayerHand && this.GetSelectedObjectCount(-1, true, false) > 1))
		{
			this.SetActive(this.NetworkInstance.GUIContextualShuffle, true);
		}
		else
		{
			this.SetActive(this.NetworkInstance.GUIContextualShuffle, false);
		}
		this.SetActive(this.NetworkInstance.GUIContextualSpread, this.InfoObject.CompareTag("Deck"));
		this.SetActive(this.NetworkInstance.GUIContextualStopSearch, false);
		this.SetActive(this.NetworkInstance.GUIContextualCheck, this.InfoObject.CompareTag("Jigsaw Box"));
		this.SetActive(this.NetworkInstance.GUIContextualSolve, this.InfoObject.CompareTag("Jigsaw Box") && NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) && NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject == this.InfoObject);
		this.SetActive(this.NetworkInstance.GUIContextualJoint, this.InfoObject.GetComponent<Joint>());
		this.SetActive(this.NetworkInstance.GUIContextualPhysics, Network.isServer && flag);
		this.SetActive(this.NetworkInstance.GUIContextualCustom, Network.isServer && this.InfoObject.GetComponent<CustomObject>());
		bool enabled = this.InfoObject.CompareTag("Clock");
		this.SetActive(this.NetworkInstance.GUIContextualClockCurrentTime, enabled);
		this.SetActive(this.NetworkInstance.GUIContextualClockStopwatch, enabled);
		this.SetActive(this.NetworkInstance.GUIContextualClockTimer, enabled);
		this.SetActive(this.NetworkInstance.GUIContextualScripting, Network.isServer && !string.IsNullOrEmpty(component.GUID));
		if (this.NetworkInstance.GUIContextualScripting.activeSelf)
		{
			this.NetworkInstance.GUIContextualGUID.GetComponent<UILabel>().text = "            GUID: " + component.GUID;
			this.NetworkInstance.GUIContextualGUID.AddMissingComponent<UICopyToClipboard>().CopyText = component.GUID;
			UITooltipScript uitooltipScript = this.NetworkInstance.GUIContextualGUID.AddMissingComponent<UITooltipScript>();
			uitooltipScript.Tooltip = "Click to copy to clipboard.";
			uitooltipScript.QuestionMark = false;
			this.NetworkInstance.GUIContextualGUID.GetComponent<BoxCollider2D>().enabled = true;
		}
		this.SetActive(this.NetworkInstance.GUIContextualRoll, this.InfoObject.CompareTag("Dice") || this.InfoObject.CompareTag("Coin"));
		this.SetActive(this.NetworkInstance.GUIContextualSetNumber, component.HasRotationsValues());
		if (this.NetworkInstance.GUIContextualSetNumber.activeSelf)
		{
			Transform transform3 = this.NetworkInstance.GUIContextualSetNumber.GetComponentsInChildren<UITable>(true)[0].transform;
			for (int l = 0; l < transform3.childCount; l++)
			{
				GameObject gameObject3 = transform3.GetChild(l).gameObject;
				int num7 = int.Parse(gameObject3.name);
				gameObject3.SetActive(num7 <= component.RotationValues.Count && num7 >= 1);
			}
			transform3.GetComponent<UITable>().repositionNow = true;
		}
		this.SetActive(this.NetworkInstance.GUIContextualLayout, LayoutZone.IsNPOInLayoutZone(component, LayoutZone.PotentialZoneCheck.Both));
		this.SetActive(this.NetworkInstance.GUIContextualCreateStates, this.GetSelectedObjects(-1, true, false).Count > 1 && NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1));
		this.SetActive(this.NetworkInstance.GUIContextualSetState, component.GetSelectedStateId() != -1);
		if (this.NetworkInstance.GUIContextualSetState.activeSelf)
		{
			Transform transform4 = this.NetworkInstance.GUIContextualSetState.GetComponentsInChildren<UITable>(true)[0].transform;
			int selectedStateId = component.GetSelectedStateId();
			for (int m = 0; m < transform4.childCount; m++)
			{
				GameObject gameObject4 = transform4.GetChild(m).gameObject;
				int num8 = int.Parse(gameObject4.name);
				gameObject4.SetActive(num8 <= component.GetStatesCount() && num8 >= 1);
				if (gameObject4.activeSelf)
				{
					UITooltipScript uitooltipScript2 = gameObject4.GetComponent<UITooltipScript>();
					if (!uitooltipScript2)
					{
						uitooltipScript2 = gameObject4.AddComponent<UITooltipScript>();
						uitooltipScript2.QuestionMark = false;
					}
					if (num8 == selectedStateId)
					{
						uitooltipScript2.Tooltip = component.Name;
					}
					else if (component.States != null)
					{
						ObjectState objectState = null;
						if (component.States.TryGetValue(num8, out objectState))
						{
							uitooltipScript2.Tooltip = objectState.Nickname;
						}
						else
						{
							uitooltipScript2.Tooltip = "";
						}
					}
				}
			}
			transform4.GetComponent<UITable>().repositionNow = true;
		}
		this.SetActive(this.NetworkInstance.GUIContextualLoopingEffect, this.InfoObject.GetComponent<CustomAssetbundle>() && this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.LoopingEffects != null && this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.LoopingEffects.Count > 0);
		if (this.NetworkInstance.GUIContextualLoopingEffect.activeSelf)
		{
			Transform transform5 = this.NetworkInstance.GUIContextualLoopingEffect.GetComponentsInChildren<UITable>(true)[0].transform;
			for (int n = 0; n < transform5.childCount; n++)
			{
				GameObject gameObject5 = transform5.GetChild(n).gameObject;
				int num9 = int.Parse(gameObject5.name);
				gameObject5.SetActive(num9 <= this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.LoopingEffects.Count && num9 != 0);
				if (gameObject5.activeSelf)
				{
					UITooltipScript uitooltipScript3 = gameObject5.GetComponent<UITooltipScript>();
					if (!uitooltipScript3)
					{
						uitooltipScript3 = gameObject5.AddComponent<UITooltipScript>();
						uitooltipScript3.QuestionMark = false;
					}
					uitooltipScript3.Tooltip = this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.LoopingEffects[num9 - 1].Name;
				}
			}
			transform5.GetComponent<UITable>().repositionNow = true;
		}
		this.SetActive(this.NetworkInstance.GUIContextualTriggerEffect, this.InfoObject.GetComponent<CustomAssetbundle>() && this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.TriggerEffects != null && this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.TriggerEffects.Count > 0);
		if (this.NetworkInstance.GUIContextualTriggerEffect.activeSelf)
		{
			Transform transform6 = this.NetworkInstance.GUIContextualTriggerEffect.GetComponentsInChildren<UITable>(true)[0].transform;
			for (int num10 = 0; num10 < transform6.childCount; num10++)
			{
				GameObject gameObject6 = transform6.GetChild(num10).gameObject;
				int num11 = int.Parse(gameObject6.name);
				gameObject6.SetActive(num11 <= this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.TriggerEffects.Count && num11 != 0);
				if (gameObject6.activeSelf)
				{
					UITooltipScript uitooltipScript4 = gameObject6.GetComponent<UITooltipScript>();
					if (!uitooltipScript4)
					{
						uitooltipScript4 = gameObject6.AddComponent<UITooltipScript>();
						uitooltipScript4.QuestionMark = false;
					}
					uitooltipScript4.Tooltip = this.InfoObject.GetComponent<CustomAssetbundle>().assetBundleEffects.TriggerEffects[num11 - 1].Name;
				}
			}
			transform6.GetComponent<UITable>().repositionNow = true;
		}
		GameObject guicontextualPopoutPDFPage = this.NetworkInstance.GUIContextualPopoutPDFPage;
		bool enabled2;
		if (this.InfoObject.GetComponent<CustomPDF>())
		{
			CustomPDF component3 = this.InfoObject.GetComponent<CustomPDF>();
			enabled2 = (component3 != null && component3.PageCount > 0);
		}
		else
		{
			enabled2 = false;
		}
		this.SetActive(guicontextualPopoutPDFPage, enabled2);
		GameObject guicontextualSetPDFPage = this.NetworkInstance.GUIContextualSetPDFPage;
		bool enabled3;
		if (this.InfoObject.GetComponent<CustomPDF>())
		{
			CustomPDF component4 = this.InfoObject.GetComponent<CustomPDF>();
			enabled3 = (component4 != null && component4.PageCount > 0);
		}
		else
		{
			enabled3 = false;
		}
		this.SetActive(guicontextualSetPDFPage, enabled3);
		if (this.NetworkInstance.GUIContextualSetPDFPage.activeSelf)
		{
			Transform transform7 = this.NetworkInstance.GUIContextualSetPDFPage.GetComponentsInChildren<UITable>(true)[0].transform;
			int pageDisplayOffset = this.InfoObject.GetComponent<CustomPDF>().PageDisplayOffset;
			for (int num12 = 0; num12 < transform7.childCount; num12++)
			{
				GameObject gameObject7 = transform7.GetChild(num12).gameObject;
				int num13 = int.Parse(gameObject7.name);
				gameObject7.SetActive(num13 <= this.InfoObject.GetComponent<CustomPDF>().PageCount + pageDisplayOffset && num13 >= 1);
				if (gameObject7.activeSelf)
				{
					UITooltipScript uitooltipScript5 = gameObject7.GetComponent<UITooltipScript>();
					if (!uitooltipScript5)
					{
						uitooltipScript5 = gameObject7.AddComponent<UITooltipScript>();
						uitooltipScript5.QuestionMark = false;
					}
					if (num13 == this.InfoObject.GetComponent<CustomPDF>().CurrentPDFPage)
					{
						uitooltipScript5.Tooltip = component.Name;
					}
				}
			}
			transform7.GetComponent<UITable>().repositionNow = true;
		}
		this.SetActive(this.NetworkInstance.GUIContextualFogOfWarRevealPanel, false);
		if (this.InfoObject.GetComponent<FogOfWarRevealer>() != null && this.InfoObject.GetComponent<FogOfWarRevealer>().Active)
		{
			this.SetActive(this.NetworkInstance.GUIContextualFogOfWarRevealPanel, true);
			this.NetworkInstance.GUIContextualFogOfWarRevealColorSprite.color = this.InfoObject.GetComponent<FogOfWarRevealer>().AColor;
			this.NetworkInstance.GUIContextualFogOfWarRevealRange.GetComponentInChildren<UISliderRange>(true).floatValue = this.InfoObject.GetComponent<FogOfWarRevealer>().Range;
			this.NetworkInstance.GUIContextualFogOfWarRevealHeight.GetComponentInChildren<UISliderRange>(true).floatValue = this.InfoObject.GetComponent<FogOfWarRevealer>().Height;
			this.NetworkInstance.GUIContextualFogOfWarRevealFoV.GetComponentInChildren<UISliderRange>(true).floatValue = this.InfoObject.GetComponent<FogOfWarRevealer>().FoV;
			this.NetworkInstance.GUIContextualFogOfWarRevealFoVOffset.GetComponentInChildren<UISliderRange>(true).floatValue = this.InfoObject.GetComponent<FogOfWarRevealer>().FoVOffset;
			this.NetworkInstance.GUIContextualFogOfWarRevealOutline.GetComponentInChildren<UIToggle>(true).Set(this.InfoObject.GetComponent<FogOfWarRevealer>().ShowFoWOutline, true);
		}
		this.SetActive(this.NetworkInstance.GUIContextualRPGMode, this.InfoObject.CompareTag("rpgFigurine"));
		this.SetActive(this.NetworkInstance.GUIContextualRPGAttack, this.InfoObject.CompareTag("rpgFigurine"));
		this.SetActive(this.NetworkInstance.GUIContextualRPGDeath, this.InfoObject.CompareTag("rpgFigurine"));
		this.SetActive(this.NetworkInstance.GUIContextualGroup, this.CheckGroup());
		if ((this.InfoObject.tag == "Dice" || this.InfoObject.tag == "Domino" || this.InfoObject.tag == "Chess") && this.InfoObject.name != "Die_6_Rounded(Clone)" && this.InfoObject.GetComponent<MaterialSyncScript>())
		{
			this.SetActive(this.NetworkInstance.GUIContextualMaterial, true);
			int material = this.InfoObject.GetComponent<MaterialSyncScript>().GetMaterial();
			this.SetActive(this.NetworkInstance.GUIContextualGoldBool.transform.parent.gameObject, Network.isServer && SteamManager.bKickstarterGold);
			int group = this.NetworkInstance.GUIContextualWoodBool.GetComponent<UIToggle>().group;
			this.NetworkInstance.GUIContextualWoodBool.GetComponent<UIToggle>().group = 0;
			this.NetworkInstance.GUIContextualPlasticBool.GetComponent<UIToggle>().group = 0;
			this.NetworkInstance.GUIContextualMetalBool.GetComponent<UIToggle>().group = 0;
			this.NetworkInstance.GUIContextualGoldBool.GetComponent<UIToggle>().group = 0;
			if (this.InfoObject.tag != "Chess")
			{
				this.NetworkInstance.GUIContextualWoodBool.transform.parent.gameObject.SetActive(false);
				this.NetworkInstance.GUIContextualPlasticBool.transform.parent.gameObject.SetActive(true);
				this.NetworkInstance.GUIContextualWoodBool.GetComponent<UIToggle>().value = false;
				this.NetworkInstance.GUIContextualPlasticBool.GetComponent<UIToggle>().value = (material == 0);
				this.NetworkInstance.GUIContextualMetalBool.GetComponent<UIToggle>().value = (material == 1);
				this.NetworkInstance.GUIContextualGoldBool.GetComponent<UIToggle>().value = (material == 2);
			}
			else
			{
				this.NetworkInstance.GUIContextualPlasticBool.transform.parent.gameObject.SetActive(false);
				this.NetworkInstance.GUIContextualWoodBool.transform.parent.gameObject.SetActive(true);
				this.NetworkInstance.GUIContextualPlasticBool.GetComponent<UIToggle>().value = false;
				this.NetworkInstance.GUIContextualWoodBool.GetComponent<UIToggle>().value = (material == 2 || material == 3);
				this.NetworkInstance.GUIContextualMetalBool.GetComponent<UIToggle>().value = (material == 0 || material == 1);
				this.NetworkInstance.GUIContextualGoldBool.GetComponent<UIToggle>().value = (material == 4);
			}
			this.NetworkInstance.GUIContextualWoodBool.GetComponent<UIToggle>().group = group;
			this.NetworkInstance.GUIContextualPlasticBool.GetComponent<UIToggle>().group = group;
			this.NetworkInstance.GUIContextualMetalBool.GetComponent<UIToggle>().group = group;
			this.NetworkInstance.GUIContextualGoldBool.GetComponent<UIToggle>().group = group;
		}
		else
		{
			this.SetActive(this.NetworkInstance.GUIContextualMaterial, false);
		}
		this.SetActive(this.NetworkInstance.GUIContextualOrder, this.InfoObject.GetComponent<StackObject>() && this.InfoObject.GetComponent<StackObject>().bBag);
		if (this.NetworkInstance.GUIContextualOrder.activeSelf)
		{
			int group2 = this.NetworkInstance.GUIContextualOrderLIFO.group;
			this.NetworkInstance.GUIContextualOrderLIFO.group = 0;
			this.NetworkInstance.GUIContextualOrderFIFO.group = 0;
			this.NetworkInstance.GUIContextualOrderRandom.group = 0;
			OrderType order = this.InfoObject.GetComponent<StackObject>().Order;
			this.NetworkInstance.GUIContextualOrderLIFO.value = (order == OrderType.LIFO);
			this.NetworkInstance.GUIContextualOrderFIFO.value = (order == OrderType.FILO);
			this.NetworkInstance.GUIContextualOrderRandom.value = (order == OrderType.Random);
			this.NetworkInstance.GUIContextualOrderLIFO.group = group2;
			this.NetworkInstance.GUIContextualOrderFIFO.group = group2;
			this.NetworkInstance.GUIContextualOrderRandom.group = group2;
		}
		bool flag4 = !PermissionsOptions.options.Contextual && !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		bool flag5 = this.InfoObject.GetComponent<StackObject>() && this.InfoObject.GetComponent<StackObject>().num_objects_ <= 0;
		this.NetworkInstance.GUIContextualDeckDeal.GetComponent<BoxCollider2D>().enabled = (!flag4 && !flag5);
		this.NetworkInstance.GUIContextualDraw.GetComponent<BoxCollider2D>().enabled = !flag5;
		this.NetworkInstance.GUIContextualDeckCut.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualSplit.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualDeckReset.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualColorTint.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualName.GetComponent<BoxCollider2D>().enabled = (!flag4 && canSeeName);
		this.NetworkInstance.GUIContextualDesc.GetComponent<BoxCollider2D>().enabled = (!flag4 && canSeeName);
		this.NetworkInstance.GUIContextualValue.GetComponent<BoxCollider2D>().enabled = (!flag4 && canSeeName);
		this.NetworkInstance.GUIContextualGMNotes.GetComponent<BoxCollider2D>().enabled = (!flag4 && canSeeName);
		this.NetworkInstance.GUIContextualWoodBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualPlasticBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualMetalBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualGoldBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualOrderLIFO.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualOrderFIFO.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualOrderRandom.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualLockBool.GetComponent<BoxCollider2D>().enabled = (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || PermissionsOptions.options.Locking);
		this.NetworkInstance.GUIContextualDragSelectableBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualGridBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualSnapBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualAutoraiseBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualStickyBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualRevealActive.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualTooltipBool.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualDestroyableBool.GetComponent<BoxCollider2D>().enabled = NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		this.NetworkInstance.GUIContextualSolve.GetComponent<BoxCollider2D>().enabled = NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		this.NetworkInstance.GUIContextualCheck.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualSearch.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualShuffle.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualSpread.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualStopSearch.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualClockCurrentTime.GetComponent<BoxCollider2D>().enabled = (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || PermissionsOptions.options.Digital);
		this.NetworkInstance.GUIContextualClockStopwatch.GetComponent<BoxCollider2D>().enabled = (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || PermissionsOptions.options.Digital);
		this.NetworkInstance.GUIContextualClockTimer.GetComponent<BoxCollider2D>().enabled = (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || PermissionsOptions.options.Digital);
		this.NetworkInstance.GUIContextualRoll.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualSetNumber.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualComponentTags.GetComponent<BoxCollider2D>().enabled = NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		this.NetworkInstance.GUIContextualSaveToChest.GetComponent<BoxCollider2D>().enabled = (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) || PermissionsOptions.options.Saving);
		this.NetworkInstance.GUIContextualJoint.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualPhysics.GetComponent<BoxCollider2D>().enabled = !flag4;
		this.NetworkInstance.GUIContextualDelete.GetComponent<BoxCollider2D>().enabled = NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		this.NetworkInstance.GUIContextualCopy.GetComponent<BoxCollider2D>().enabled = NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		this.NetworkInstance.GUIContextualClone.GetComponent<BoxCollider2D>().enabled = NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1);
		NetworkSingleton<UserDefinedContextualManager>.Instance.CleanUpObjectMenuGOs();
		if (component.CustomContextMenus.Count == 0 || UserDefinedContextualManager.PROHIBIT_USER_CONTEXT_MENUS)
		{
			this.SetActive(this.NetworkInstance.GUIContextualUserDefinedSpacer, false);
		}
		else
		{
			this.SetActive(this.NetworkInstance.GUIContextualUserDefinedSpacer, true);
			Transform parent = this.NetworkInstance.GUIContextualUserDefinedTemplate.transform.parent;
			for (int num14 = 0; num14 < component.CustomContextMenus.Count; num14++)
			{
				GameObject gameObject8 = UnityEngine.Object.Instantiate<GameObject>(this.NetworkInstance.GUIContextualUserDefinedTemplate, parent);
				int num15 = component.CustomContextMenus[num14];
				UserDefinedContextualManager.UDCIdentifier udcidentifier = NetworkSingleton<UserDefinedContextualManager>.Instance.ObjectEntries[num15];
				gameObject8.GetComponent<UserDefinedContextual>().Init(udcidentifier.label, num15, component, null);
				if (udcidentifier.keepOpen)
				{
					UnityEngine.Object.Destroy(gameObject8.GetComponent<UIButtonActivate>());
				}
				gameObject8.name = string.Format("00 00 {0:D4}", num14);
				NetworkSingleton<UserDefinedContextualManager>.Instance.AddObjectMenuGO(gameObject8);
				gameObject8.SetActive(true);
			}
		}
		this.SetActive(this.NetworkInstance.GUIContextualValue, false);
		this.SetActive(this.NetworkInstance.GUIContextualGMNotes, Pointer.SHOW_GM_NOTES && Colour.MyColorLabel() == "Black");
		UIContextMenuSpacer.UpdateContextualSpacerChildren();
		this.NetworkInstance.GUIContextualGrid.GetComponent<UIGrid>().repositionNow = true;
		if (this.MainCamera.enabled)
		{
			Vector3 position = this.MainCamera.WorldToViewportPoint(base.transform.position);
			Vector3 vector = this.UICameraObject.GetComponent<Camera>().ViewportToWorldPoint(position);
			vector = new Vector3(vector.x + 0.01f, vector.y - 0.01f, 0f);
			this.NetworkInstance.GUIContextualMenu.transform.position = vector;
		}
		else
		{
			this.NetworkInstance.GUIContextualMenu.transform.position = UICamera.mainCamera.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2 + 100), 0f));
		}
		this.NetworkInstance.GUIContextualMenu.SetActive(true);
		this.NetworkInstance.GUIDropDown.SetActive(true);
		this.NetworkInstance.GUIContextualDealColor.SetActive(false);
		this.NetworkInstance.GUIContextualHandRevealColors.SetActive(false);
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x000B0270 File Offset: 0x000AE470
	public void CheckStartContextualZones(Vector3 ScreenPos)
	{
		RaycastHit[] array = Physics.RaycastAll(this.MainCamera.ScreenPointToRay(ScreenPos), 1000f, HoverScript.GrabbableLayerMask);
		Array.Sort<RaycastHit>(array, new RaycastHitComparator());
		foreach (RaycastHit raycastHit in array)
		{
			if (raycastHit.collider.gameObject.CompareTag("Fog"))
			{
				this.ResetHiddenZoneObject();
				this.InfoHiddenZoneGO = raycastHit.collider.gameObject;
				this.InfoHiddenZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.StartContextual(this.InfoHiddenZoneGO, true);
				return;
			}
			if (raycastHit.collider.gameObject.CompareTag("Randomize"))
			{
				this.ResetRandomizeObject();
				this.InfoRandomizeZoneGO = raycastHit.collider.gameObject;
				this.InfoRandomizeZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.StartContextual(this.InfoRandomizeZoneGO, true);
				return;
			}
			if (raycastHit.collider.gameObject.CompareTag("Hand"))
			{
				this.ResetHandObject();
				this.InfoHandObject = raycastHit.collider.gameObject;
				this.InfoHandObject.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.StartContextual(this.InfoHandObject, true);
				return;
			}
			if (raycastHit.collider.gameObject.CompareTag("Scripting"))
			{
				this.StartContextual(raycastHit.collider.gameObject, true);
				return;
			}
			if (raycastHit.collider.gameObject.CompareTag("FogOfWar"))
			{
				this.ResetFogOfWarObject();
				this.InfoFogOfWarZoneGO = raycastHit.collider.gameObject;
				this.InfoFogOfWarZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.StartContextual(this.InfoFogOfWarZoneGO, true);
				return;
			}
			if (raycastHit.collider.gameObject.CompareTag("Layout"))
			{
				this.ResetLayoutZoneObject();
				this.InfoLayoutZoneGO = raycastHit.collider.gameObject;
				this.InfoLayoutZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.StartContextual(this.InfoLayoutZoneGO, true);
				return;
			}
		}
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x000B04AC File Offset: 0x000AE6AC
	public void CheckStartZones(Vector3 screenPos)
	{
		RaycastHit[] array = Physics.RaycastAll(this.MainCamera.ScreenPointToRay(screenPos), 1000f, HoverScript.GrabbableLayerMask);
		Array.Sort<RaycastHit>(array, new RaycastHitComparator());
		RaycastHit[] array2 = array;
		int i = 0;
		while (i < array2.Length)
		{
			RaycastHit raycastHit = array2[i];
			if (raycastHit.collider.gameObject.CompareTag("Fog"))
			{
				if (this.NetworkInstance.GUIFogColor.activeSelf && this.InfoHiddenZoneGO == raycastHit.collider.gameObject)
				{
					this.NetworkInstance.GUIFogColor.SetActive(false);
					this.ResetHiddenZoneObject();
					return;
				}
				NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
				this.ResetHiddenZoneObject();
				this.InfoHiddenZoneGO = raycastHit.collider.gameObject;
				this.InfoHiddenZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				Vector3 vector = this.MainCamera.WorldToViewportPoint(base.transform.position);
				vector = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector, 160f, 160f, SpriteAlignment.Center, true);
				Vector3 vector2 = this.UICameraObject.GetComponent<Camera>().ViewportToWorldPoint(vector);
				vector2 = new Vector3(vector2.x + 0.03f, vector2.y - 0.03f, 0f);
				this.NetworkInstance.GUIFogColor.transform.position = vector2;
				this.NetworkInstance.GUIFogColor.transform.RoundLocalPosition();
				this.NetworkInstance.GUIFogColor.SetActive(true);
				return;
			}
			else if (raycastHit.collider.gameObject.CompareTag("Randomize") && this.CurrentPointerMode == PointerMode.Randomize)
			{
				if (this.NetworkInstance.GUIRandomizeZone.activeSelf && this.InfoRandomizeZoneGO == raycastHit.collider.gameObject)
				{
					this.NetworkInstance.GUIRandomizeZone.SetActive(false);
					this.ResetRandomizeObject();
					return;
				}
				NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
				this.ResetRandomizeObject();
				this.InfoRandomizeZoneGO = raycastHit.collider.gameObject;
				this.InfoRandomizeZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.ShowRandomizeZonePopupMenu(screenPos);
				return;
			}
			else if (raycastHit.collider.gameObject.CompareTag("Hand") && this.CurrentPointerMode == PointerMode.Hands)
			{
				if (this.NetworkInstance.GUIHandColor.activeSelf && this.InfoHandObject == raycastHit.collider.gameObject)
				{
					this.NetworkInstance.GUIHandColor.SetActive(false);
					this.ResetHandObject();
					return;
				}
				NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
				this.ResetHandObject();
				this.InfoHandObject = raycastHit.collider.gameObject;
				this.InfoHandObject.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				Vector3 vector3 = this.MainCamera.WorldToViewportPoint(base.transform.position);
				vector3 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, 160f, 160f, SpriteAlignment.Center, true);
				Vector3 vector4 = this.UICameraObject.GetComponent<Camera>().ViewportToWorldPoint(vector3);
				vector4 = new Vector3(vector4.x, vector4.y, 0f);
				this.NetworkInstance.GUIHandColor.transform.position = vector4;
				this.NetworkInstance.GUIHandColor.transform.RoundLocalPosition();
				this.NetworkInstance.GUIHandColor.SetActive(true);
				return;
			}
			else if (raycastHit.collider.gameObject.CompareTag("Scripting") && this.CurrentPointerMode == PointerMode.Scripting)
			{
				if (NetworkSingleton<PlayerManager>.Instance.IsHost(-1))
				{
					TTSUtilities.CopyToClipboard(raycastHit.collider.gameObject.GetComponent<NetworkPhysicsObject>().GUID);
					return;
				}
				break;
			}
			else if (raycastHit.collider.gameObject.CompareTag("FogOfWar") && this.CurrentPointerMode == PointerMode.FogOfWar)
			{
				if (this.NetworkInstance.GUIFogOfWar.activeSelf && this.InfoFogOfWarZoneGO == raycastHit.collider.gameObject)
				{
					this.NetworkInstance.GUIFogOfWar.SetActive(false);
					this.ResetFogOfWarObject();
					return;
				}
				NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
				this.ResetFogOfWarObject();
				this.InfoFogOfWarZoneGO = raycastHit.collider.gameObject;
				this.InfoFogOfWarZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.NetworkInstance.GUIFogOfWar.SetActive(true);
				Vector3 position = this.MainCamera.ScreenToViewportPoint(screenPos);
				Vector3 vector5 = this.UICameraObject.GetComponent<Camera>().ViewportToWorldPoint(position);
				vector5 = new Vector3(vector5.x, vector5.y, 0f);
				this.NetworkInstance.GUIFogOfWar.transform.position = vector5;
				Vector3 localPosition = this.NetworkInstance.GUIFogOfWar.transform.localPosition;
				localPosition = new Vector3(localPosition.x + 100f, localPosition.y - 97.5f, localPosition.z);
				this.NetworkInstance.GUIFogOfWar.transform.localPosition = localPosition;
				this.NetworkInstance.GUIFogOfWar.transform.RoundLocalPosition();
				this.NetworkInstance.GUIFogOfWar.SetActive(true);
				return;
			}
			else if (raycastHit.collider.gameObject.CompareTag("Layout") && this.CurrentPointerMode == PointerMode.LayoutZone)
			{
				if (this.NetworkInstance.GUILayoutZone.activeSelf && this.InfoLayoutZoneGO == raycastHit.collider.gameObject)
				{
					this.ResetLayoutZoneObject();
					return;
				}
				NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.SetActive(false);
				this.ResetLayoutZoneObject();
				this.InfoLayoutZoneGO = raycastHit.collider.gameObject;
				this.InfoLayoutZoneGO.GetComponent<Highlighter>().ConstantOn(this.SelectColor, 0.25f);
				this.ShowLayoutZonePopupMenu(screenPos);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x000B0AC3 File Offset: 0x000AECC3
	public void ShowRandomizeZonePopupMenu(GameObject go)
	{
		this.InfoRandomizeZoneGO = go;
		this.ShowRandomizeZonePopupMenu(Input.mousePosition);
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x000B0AD8 File Offset: 0x000AECD8
	public void ShowCenteredZonePopupMenu(GameObject popupMenu, Vector3 screenPos)
	{
		Vector3 position = this.MainCamera.ScreenToViewportPoint(screenPos);
		Vector3 vector = this.UICameraObject.GetComponent<Camera>().ViewportToWorldPoint(position);
		vector = new Vector3(vector.x, vector.y, 0f);
		popupMenu.transform.position = vector;
		Vector3 localPosition = popupMenu.transform.localPosition;
		localPosition = new Vector3(localPosition.x + 100f, localPosition.y - 97.5f, localPosition.z);
		popupMenu.transform.localPosition = localPosition;
		popupMenu.transform.RoundLocalPosition();
		popupMenu.SetActive(true);
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x000B0B77 File Offset: 0x000AED77
	public void ShowRandomizeZonePopupMenu(Vector3 screenPos)
	{
		this.ShowCenteredZonePopupMenu(this.NetworkInstance.GUIRandomizeZone, screenPos);
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x000B0B8B File Offset: 0x000AED8B
	public void ShowLayoutZonePopupMenu(Vector3 screenPos)
	{
		this.ShowCenteredZonePopupMenu(this.NetworkInstance.GUILayoutZone, screenPos);
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x000B0BA0 File Offset: 0x000AEDA0
	public void StartGlobalContextual(Vector3 ScreenPos)
	{
		NetworkSingleton<NetworkUI>.Instance.GUIContextualGlobalMenu.SetActive(false);
		this.StartGlobalPointerPos = base.transform.position;
		Vector3 position = this.MainCamera.ScreenToViewportPoint(ScreenPos);
		Vector3 vector = this.UICameraObject.GetComponent<Camera>().ViewportToWorldPoint(position);
		vector = new Vector3(vector.x + 0.01f, vector.y - 0.01f, 0f);
		this.NetworkInstance.GUIContextualGlobalMenu.transform.position = vector;
		this.NetworkInstance.GUIContextualPaste.GetComponent<Collider2D>().enabled = (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1) && this.bCopied);
		this.NetworkInstance.GUIPictureInPicture.SetActive(!Singleton<SpectatorCamera>.Instance.active);
		UserDefinedContextualManager instance = NetworkSingleton<UserDefinedContextualManager>.Instance;
		instance.CleanUpGlobalMenuGOs();
		if (instance.GlobalItems.Count == 0 || UserDefinedContextualManager.PROHIBIT_USER_CONTEXT_MENUS)
		{
			this.NetworkInstance.GUIUserDefinedGlobalSpacer.SetActive(false);
		}
		else
		{
			Transform parent = this.NetworkInstance.GUIUserDefinedGlobalTemplate.transform.parent;
			Ray pointerRay = HoverScript.GetPointerRay();
			int num = 0;
			Pointer.raycastHits = HoverScript.RaySphereCast(pointerRay, out num);
			bool flag = false;
			float num2 = 0f;
			Vector3? location = null;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = Pointer.raycastHits[i];
				GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider);
				if (!gameObject.name.StartsWith("Bounds"))
				{
					flag = true;
					if (num2 == 0f || raycastHit.distance < num2)
					{
						location = new Vector3?(raycastHit.point);
						num2 = raycastHit.distance;
					}
				}
			}
			int num3 = 0;
			for (int j = 0; j < instance.GlobalItems.Count; j++)
			{
				int num4 = instance.GlobalItems[j];
				UserDefinedContextualManager.UDCGlobalIdentifier udcglobalIdentifier = instance.GlobalEntries[num4];
				if (!udcglobalIdentifier.requireTable || flag)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.NetworkInstance.GUIUserDefinedGlobalTemplate, parent);
					gameObject2.GetComponent<UserDefinedContextual>().Init(udcglobalIdentifier.label, num4, null, location);
					if (udcglobalIdentifier.keepOpen)
					{
						UnityEngine.Object.Destroy(gameObject2.GetComponent<UIButtonActivate>());
					}
					gameObject2.name = string.Format("00 00 {0:D4}", j);
					instance.AddGlobalMenuGO(gameObject2);
					gameObject2.SetActive(true);
					num3++;
				}
			}
			this.NetworkInstance.GUIUserDefinedGlobalSpacer.SetActive(num3 > 0);
		}
		NetworkSingleton<NetworkUI>.Instance.GUIContextualGlobalMenu.SetActive(true);
		NetworkSingleton<NetworkUI>.Instance.GUIContextualGlobalMenu.SetActive(false);
		Wait.Frames(delegate
		{
			NetworkSingleton<NetworkUI>.Instance.GUIContextualGlobalMenu.SetActive(true);
		}, 1);
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x000B0E6C File Offset: 0x000AF06C
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void SpawnText(Vector3 pos, Vector3 rot, int objectId)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, Vector3, int>(RPCTarget.Server, new Action<Vector3, Vector3, int>(this.SpawnText), pos, rot, objectId);
			return;
		}
		TextTool component = Network.Instantiate(NetworkSingleton<GameMode>.Instance.TextLabel, pos, Quaternion.Euler(rot), default(NetworkPlayer)).GetComponent<TextTool>();
		component.playerId = this.ID;
		component.input.activeTextColor = this.PointerColour;
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x000B0EE1 File Offset: 0x000AF0E1
	public IEnumerator SelectText(UIInput input)
	{
		yield return null;
		input.isSelected = true;
		yield break;
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000B0EF0 File Offset: 0x000AF0F0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void SpawnScriptingZone(Vector3 pos, Quaternion rot, Vector3 scale)
	{
		if (!PermissionsOptions.CheckIfAllowedInMode(PointerMode.Scripting, this.ID))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, Quaternion, Vector3>(RPCTarget.Server, new Action<Vector3, Quaternion, Vector3>(this.SpawnScriptingZone), pos, rot, scale);
			return;
		}
		Network.Instantiate(NetworkSingleton<GameMode>.Instance.ScriptingTrigger, pos, rot, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetScale(scale, false);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000B0F58 File Offset: 0x000AF158
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void SpawnHiddenZone(Vector3 pos, Quaternion rot, Vector3 scale)
	{
		if (!PermissionsOptions.CheckIfAllowedInMode(PointerMode.Hidden, this.ID))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, Quaternion, Vector3>(RPCTarget.Server, new Action<Vector3, Quaternion, Vector3>(this.SpawnHiddenZone), pos, rot, scale);
			return;
		}
		GameObject gameObject = Network.Instantiate(NetworkSingleton<GameMode>.Instance.FogOfWarTrigger, pos, rot, default(NetworkPlayer));
		gameObject.GetComponent<NetworkPhysicsObject>().SetScale(scale, false);
		gameObject.GetComponent<HiddenZone>().SetOwningColor(this.PointerColorLabel);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000B0FD0 File Offset: 0x000AF1D0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void SpawnFogOfWarZone(Vector3 pos, Quaternion rot, Vector3 scale)
	{
		if (!PermissionsOptions.CheckIfAllowedInMode(PointerMode.FogOfWar, this.ID))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, Quaternion, Vector3>(RPCTarget.Server, new Action<Vector3, Quaternion, Vector3>(this.SpawnFogOfWarZone), pos, rot, scale);
			return;
		}
		Network.Instantiate(NetworkSingleton<GameMode>.Instance.FogOfWar, pos, rot, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetScale(scale, false);
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000B1038 File Offset: 0x000AF238
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void SpawnLayoutZone(Vector3 pos, Quaternion rot, Vector3 scale)
	{
		if (!PermissionsOptions.CheckIfAllowedInMode(PointerMode.LayoutZone, this.ID))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, Quaternion, Vector3>(RPCTarget.Server, new Action<Vector3, Quaternion, Vector3>(this.SpawnLayoutZone), pos, rot, scale);
			return;
		}
		Network.Instantiate(NetworkSingleton<GameMode>.Instance.LayoutZone, pos, rot, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetScale(scale, false);
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x000B10A0 File Offset: 0x000AF2A0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void SpawnRandomizeZone(Vector3 pos, Quaternion rot, Vector3 scale)
	{
		if (!PermissionsOptions.CheckIfAllowedInMode(PointerMode.Randomize, this.ID))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, Quaternion, Vector3>(RPCTarget.Server, new Action<Vector3, Quaternion, Vector3>(this.SpawnRandomizeZone), pos, rot, scale);
			return;
		}
		GameObject gameObject = Network.Instantiate(NetworkSingleton<GameMode>.Instance.RandomizeTrigger, pos, rot, default(NetworkPlayer));
		gameObject.GetComponent<NetworkPhysicsObject>().SetScale(scale, false);
		gameObject.GetComponentInChildren<RandomizeZone>().SpawnId_ = this.ID;
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x000B1118 File Offset: 0x000AF318
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Delete(int hoverObjectId)
	{
		if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(this.ID))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.Delete), hoverObjectId);
			return;
		}
		List<LuaGameObjectScript> selectedLuaObjects = this.GetSelectedLuaObjects(hoverObjectId, true, true);
		NetworkPhysicsObject networkPhysicsObject = HoverScript.HoverToolObject ? HoverScript.HoverToolObject.GetComponent<NetworkPhysicsObject>() : null;
		LuaGameObjectScript luaGameObjectScript = networkPhysicsObject ? networkPhysicsObject.GetComponent<LuaGameObjectScript>() : null;
		if (luaGameObjectScript != null)
		{
			selectedLuaObjects.Add(luaGameObjectScript);
		}
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Delete, selectedLuaObjects, false))
		{
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(hoverObjectId, true, true))
		{
			if (gameObject)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(gameObject);
			}
		}
		if (networkPhysicsObject)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(networkPhysicsObject);
		}
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000B121C File Offset: 0x000AF41C
	public void Copy(List<LuaGameObjectScript> luaObjects)
	{
		this.ObjectCopyStates.Clear();
		foreach (LuaGameObjectScript luaGameObjectScript in luaObjects)
		{
			NetworkPhysicsObject npo = luaGameObjectScript.NPO;
			if (npo.IsSaved)
			{
				this.ObjectCopyStates.Add(NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(npo));
			}
		}
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x000B1294 File Offset: 0x000AF494
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Copy(int hoverObjectId, bool destroyAfterCopy = false, bool clone = false)
	{
		Debug.Log("Copy");
		if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(this.ID))
		{
			return;
		}
		this.bCopied = true;
		if (Network.isClient)
		{
			base.networkView.RPC<int, bool, bool>(RPCTarget.Server, new Action<int, bool, bool>(this.Copy), hoverObjectId, destroyAfterCopy, clone);
			return;
		}
		List<LuaGameObjectScript> list;
		if (clone)
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromID(hoverObjectId);
			LuaGameObjectScript luaGameObjectScript = networkPhysicsObject ? networkPhysicsObject.GetComponent<LuaGameObjectScript>() : null;
			if (!luaGameObjectScript)
			{
				return;
			}
			list = new List<LuaGameObjectScript>
			{
				luaGameObjectScript
			};
		}
		else
		{
			list = this.GetSelectedLuaObjects(hoverObjectId, true, true);
		}
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, destroyAfterCopy ? PlayerAction.Cut : PlayerAction.Copy, list, false))
		{
			return;
		}
		List<ObjectState> list2;
		if (clone)
		{
			list2 = new List<ObjectState>
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(list[0].NPO)
			};
			if (destroyAfterCopy)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(list[0].NPO);
			}
		}
		else
		{
			list2 = this.GetObjectStates(hoverObjectId, true, true, destroyAfterCopy, true);
		}
		if (list2.Count > 0)
		{
			this.ObjectCopyStates.Clear();
			this.ObjectCopyStates = list2;
		}
		if (clone)
		{
			base.networkView.RPC<NetworkView>(base.networkView.owner, new Action<NetworkView>(this.RPCStartCloning), list[0].networkView);
		}
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x000B13DA File Offset: 0x000AF5DA
	public void StartCloning()
	{
		if (this.InfoObject)
		{
			this.Copy(this.InfoObject.GetComponent<NetworkPhysicsObject>().ID, false, true);
		}
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000B1404 File Offset: 0x000AF604
	[Remote(Permission.Server, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void RPCStartCloning(NetworkView view)
	{
		GameObject gameObject = view.gameObject;
		NetworkPhysicsObject networkPhysicsObject = gameObject ? gameObject.GetComponent<NetworkPhysicsObject>() : null;
		if (!networkPhysicsObject)
		{
			return;
		}
		this.InteractiveSpawn(new SpawnDelegate(this.CloneSpawnInPlace), new ObjectVisualizer(networkPhysicsObject, true));
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x000B144C File Offset: 0x000AF64C
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Paste(Vector3 pos, bool positionIsPrecise = false, bool playSound = true, bool checkAction = true)
	{
		Debug.Log("Paste");
		if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(this.ID))
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3, bool, bool, bool>(RPCTarget.Server, new Action<Vector3, bool, bool, bool>(this.Paste), pos, positionIsPrecise, playSound, true);
			return;
		}
		checkAction = (checkAction || Network.sender.isClient);
		Vector3 spawnPos = positionIsPrecise ? pos : this.GetSpawnPosition(pos, true);
		List<GameObject> list = NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectStatesOffset(this.ObjectCopyStates, spawnPos, true, false, !positionIsPrecise, false);
		List<LuaGameObjectScript> list2 = new List<LuaGameObjectScript>(list.Count);
		foreach (GameObject gameObject in list)
		{
			LuaGameObjectScript component = gameObject.GetComponent<LuaGameObjectScript>();
			if (component)
			{
				list2.Add(component);
			}
		}
		if (list2.Count > 0 && checkAction && !EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Paste, list2, false))
		{
			foreach (GameObject go in list)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(go);
			}
			return;
		}
		if (playSound)
		{
			SoundScript soundScript = null;
			foreach (GameObject gameObject2 in list)
			{
				soundScript = gameObject2.GetComponent<SoundScript>();
				if (soundScript)
				{
					break;
				}
			}
			if (soundScript != null)
			{
				soundScript.CopyPasteSound();
			}
		}
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x000B15F4 File Offset: 0x000AF7F4
	private void CloneSpawnInPlace(Vector3 position)
	{
		this.Paste(position, true, false, true);
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x000B1600 File Offset: 0x000AF800
	public Vector3 GetSpawnPosition()
	{
		return this.GetSpawnPosition(base.transform.position, false);
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x000B1614 File Offset: 0x000AF814
	public Vector3 GetSpawnPosition(Vector3 Pos, bool SubtractFloor = true)
	{
		return Pointer.GetSpawnPosition(Pos, SubtractFloor, this.GetLiftHeightOffset());
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x000B1624 File Offset: 0x000AF824
	public static Vector3 GetSpawnPosition(Vector3 Pos, bool SubtractFloor, float LiftHeightOffset)
	{
		float num = Pos.y;
		if (SubtractFloor)
		{
			num -= 0.96f;
		}
		return new Vector3(Pos.x, num + LiftHeightOffset, Pos.z);
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x000B1657 File Offset: 0x000AF857
	public float GetLiftHeightOffset()
	{
		return 2.5f * this.RaiseHeight * 2f;
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x000B166B File Offset: 0x000AF86B
	public int GetSelectedObjectCount(int HoverObjectId = -1, bool bAlwaysIncludeSelected = true, bool bIncludeHeld = false)
	{
		return this.GetSelectedObjects(HoverObjectId, bAlwaysIncludeSelected, bIncludeHeld).Count;
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x000B167C File Offset: 0x000AF87C
	public List<GameObject> GetSelectedObjects(int HoverObjectId = -1, bool bAlwaysIncludeSelected = true, bool bIncludeHeld = false)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (NetworkPhysicsObject networkPhysicsObject in this.GetSelectedNPOs(HoverObjectId, bAlwaysIncludeSelected, bIncludeHeld))
		{
			list.Add(networkPhysicsObject.gameObject);
		}
		return list;
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x000B16E0 File Offset: 0x000AF8E0
	public List<NetworkPhysicsObject> GetSelectedNPOs(int HoverObjectId = -1, bool bAlwaysIncludeSelected = true, bool bIncludeHeld = false)
	{
		NetworkPhysicsObject networkPhysicsObject = null;
		List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
		if (HoverObjectId != -1)
		{
			networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromID(HoverObjectId);
			if (networkPhysicsObject)
			{
				list.Add(networkPhysicsObject);
			}
		}
		if (bAlwaysIncludeSelected || !networkPhysicsObject || this.HighLightedObjects.Contains(networkPhysicsObject))
		{
			foreach (NetworkPhysicsObject networkPhysicsObject2 in this.HighLightedObjects)
			{
				if (networkPhysicsObject2 && networkPhysicsObject2 != networkPhysicsObject)
				{
					list.Add(networkPhysicsObject2);
				}
			}
		}
		if (bIncludeHeld)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject3 in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
			{
				if (networkPhysicsObject3.HeldByPlayerID == this.ID && !list.Contains(networkPhysicsObject3))
				{
					list.Add(networkPhysicsObject3);
				}
			}
		}
		return list;
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x000B17EC File Offset: 0x000AF9EC
	public List<LuaGameObjectScript> GetSelectedLuaObjects(int HoverObjectId = -1, bool bAlwaysIncludeSelected = true, bool bIncludeHeld = false)
	{
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		foreach (NetworkPhysicsObject networkPhysicsObject in this.GetSelectedNPOs(HoverObjectId, bAlwaysIncludeSelected, bIncludeHeld))
		{
			LuaGameObjectScript component = networkPhysicsObject.GetComponent<LuaGameObjectScript>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x000B1858 File Offset: 0x000AFA58
	public List<LuaGameObjectScript> GetGrabbedLuaObjects(int touchId = -1)
	{
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>();
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			if (networkPhysicsObject.HeldByPlayerID == this.ID && networkPhysicsObject.HeldByTouchID == touchId)
			{
				LuaGameObjectScript component = networkPhysicsObject.GetComponent<LuaGameObjectScript>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		return list;
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x000B18E0 File Offset: 0x000AFAE0
	public List<ObjectState> GetObjectStates(int HoverId = -1, bool bAlwaysIncludeSelected = true, bool bIncludeHeld = true, bool bDestroyAfterCopy = false, bool bOffest = true)
	{
		List<GameObject> selectedObjects = this.GetSelectedObjects(HoverId, bAlwaysIncludeSelected, bIncludeHeld);
		List<ObjectState> list = new List<ObjectState>();
		Vector3 zero = Vector3.zero;
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component.IsSaved)
				{
					ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(component);
					if (bOffest && list.Count > 0)
					{
						objectState.Transform.posX -= list[0].Transform.posX;
						objectState.Transform.posY -= list[0].Transform.posY;
						objectState.Transform.posZ -= list[0].Transform.posZ;
					}
					list.Add(objectState);
					if (bDestroyAfterCopy)
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(gameObject);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x0600198A RID: 6538 RVA: 0x000B19FC File Offset: 0x000AFBFC
	public bool AcceptingEscape
	{
		get
		{
			return (this.FirstGrabbedNPO != null && this.FirstGrabbedNPO.HeldByPlayerID == this.ID) || this.InteractiveSpawning;
		}
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x000B1A27 File Offset: 0x000AFC27
	public void HandleEscape()
	{
		this.ResetHeldObjectsToPickupPosition();
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x000B1A30 File Offset: 0x000AFC30
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	private void ResetHeldObjectsToPickupPosition()
	{
		if (Network.isClient)
		{
			base.networkView.RPC(RPCTarget.Server, new Action(this.ResetHeldObjectsToPickupPosition));
			return;
		}
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			if (networkPhysicsObject.HeldByPlayerID == this.ID)
			{
				networkPhysicsObject.SetRotation(networkPhysicsObject.PickedUpRotation);
				networkPhysicsObject.SetSmoothPosition(networkPhysicsObject.PickedUpPosition, false, true, false, true, null, false, true, null);
			}
		}
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x000B1AD8 File Offset: 0x000AFCD8
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Grab(int HoverObjectId, Vector3? PointerPosition, int TouchId = -1, bool bOffsetFromSurface = true, Vector3? PointerRot = null)
	{
		this.bStopMarquee = true;
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(HoverObjectId);
		if (!this.FirstGrabbedObject || TouchId != -1)
		{
			this.FirstGrabbedObject = gameObject;
		}
		if (!this.FirstGrabbedObject)
		{
			return;
		}
		Vector3 vector = gameObject.transform.position;
		if (PointerPosition != null)
		{
			vector = PointerPosition.Value;
		}
		else
		{
			vector = this.FirstGrabbedObject.transform.position;
		}
		List<GameObject> selectedObjects = this.GetSelectedObjects(HoverObjectId, false, false);
		if (base.networkView.isMine)
		{
			foreach (GameObject gameObject2 in selectedObjects)
			{
				if (gameObject2 && (!(gameObject2 != this.FirstGrabbedObject) || !gameObject2.CompareTag("Card") || !gameObject2.GetComponent<Joint>()) && !this.GrabbedObjects.Contains(gameObject2))
				{
					this.GrabbedObjects.Add(gameObject2);
				}
			}
			if (ObjectPositioningVisualizer.VisualizeGrabbedObjects && this.GrabbedObjects.Count == 1)
			{
				Singleton<ObjectPositioningVisualizer>.Instance.AddVisualizer(this.FirstGrabbedNPO);
			}
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int, Vector3?, int, bool, Vector3?>(RPCTarget.Server, new NetworkView.Action<int, Vector3?, int, bool, Vector3?>(this.Grab), HoverObjectId, PointerPosition, TouchId, bOffsetFromSurface, PointerRot);
			return;
		}
		List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
		if (selectedObjects.Count == 1 && this.FirstGrabbedObject != selectedObjects[0] && vector == Vector3.zero && TouchId == -1 && bOffsetFromSurface)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in grabbableNPOs)
			{
				if (networkPhysicsObject.HeldByPlayerID == this.ID)
				{
					networkPhysicsObject.HeldOffset = Vector3.zero;
				}
			}
		}
		float num = float.MaxValue;
		if (bOffsetFromSurface)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject2 in grabbableNPOs)
			{
				if (networkPhysicsObject2 && (selectedObjects.Contains(networkPhysicsObject2.gameObject) || (networkPhysicsObject2.joint && networkPhysicsObject2.joint.connectedBody && selectedObjects.Contains(networkPhysicsObject2.joint.connectedBody.gameObject))))
				{
					Bounds boundsNotNormalized = networkPhysicsObject2.GetBoundsNotNormalized();
					Vector3 vector2 = new Vector3(boundsNotNormalized.center.x, boundsNotNormalized.center.y - boundsNotNormalized.extents.y, boundsNotNormalized.center.z);
					if (vector2.y < num)
					{
						num = vector2.y;
					}
					if (networkPhysicsObject2.joint && networkPhysicsObject2.joint.connectedBody)
					{
						NetworkPhysicsObject component = networkPhysicsObject2.joint.connectedBody.GetComponent<NetworkPhysicsObject>();
						if (component)
						{
							boundsNotNormalized = component.GetBoundsNotNormalized();
							vector2 = new Vector3(boundsNotNormalized.center.x, boundsNotNormalized.center.y - boundsNotNormalized.extents.y, boundsNotNormalized.center.z);
							if (vector2.y < num)
							{
								num = vector2.y;
							}
						}
					}
				}
			}
		}
		if (num == 3.4028235E+38f || num < 0f)
		{
			num = vector.y;
		}
		foreach (GameObject gameObject3 in selectedObjects)
		{
			if (gameObject3 && (!(gameObject3 != this.FirstGrabbedObject) || !gameObject3.CompareTag("Card") || !gameObject3.GetComponent<Joint>()))
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.PopulateGrabbedObjects(this.pendingGrabbedNpos, gameObject3.GetComponent<NetworkPhysicsObject>().ID, this.ID, TouchId);
				if (this.pendingGrabbedNpos.Count >= 128)
				{
					break;
				}
			}
		}
		foreach (NetworkPhysicsObject networkPhysicsObject3 in this.pendingGrabbedNpos)
		{
			LuaGameObjectScript component2 = networkPhysicsObject3.GetComponent<LuaGameObjectScript>();
			if (component2 != null)
			{
				this.pendingGrabbedLuaObjects.Add(component2);
			}
		}
		if (EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.PickUp, this.pendingGrabbedLuaObjects, false))
		{
			foreach (NetworkPhysicsObject networkPhysicsObject4 in this.pendingGrabbedNpos)
			{
				GameObject gameObject4 = networkPhysicsObject4.gameObject;
				Vector3 holdOffset = (vector != Vector3.zero) ? new Vector3(gameObject4.transform.position.x - vector.x, gameObject4.transform.position.y - num, gameObject4.transform.position.z - vector.z) : Vector3.zero;
				NetworkSingleton<ManagerPhysicsObject>.Instance.ClientGrab(networkPhysicsObject4.ID, this.ID, holdOffset, PointerRot ?? Vector3.zero, TouchId);
			}
		}
		this.pendingGrabbedNpos.Clear();
		this.pendingGrabbedLuaObjects.Clear();
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x000B20D4 File Offset: 0x000B02D4
	public void Release(int playerID, int dropID, int touchID = -1, int hoverID = -1)
	{
		if (base.networkView.isMine)
		{
			Singleton<ObjectPositioningVisualizer>.Instance.RemoveVisualizer();
		}
		if (dropID == -1)
		{
			this.GrabbedObjects.Clear();
		}
		if (hoverID == -1 && this.HoverObject)
		{
			NetworkPhysicsObject component = this.HoverObject.GetComponent<NetworkPhysicsObject>();
			if (component != null)
			{
				hoverID = component.ID;
			}
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, int, int>(RPCTarget.Server, new Action<int, int, int, int>(this.TellManagerPhysicsObjectAboutRelease), playerID, dropID, touchID, hoverID);
			return;
		}
		this.TellManagerPhysicsObjectAboutRelease(playerID, dropID, touchID, hoverID);
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x000B2168 File Offset: 0x000B0368
	private int GetLastGrabbedId()
	{
		for (int i = 0; i < this.GrabbedObjects.Count; i++)
		{
			GameObject gameObject = this.GrabbedObjects[this.GrabbedObjects.Count - 1 - i];
			if (gameObject && gameObject.GetComponent<NetworkPhysicsObject>().HeldByPlayerID == this.ID)
			{
				return gameObject.GetComponent<NetworkPhysicsObject>().ID;
			}
		}
		if (this.FirstGrabbedNPO)
		{
			return this.FirstGrabbedNPO.ID;
		}
		return -1;
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000B21E7 File Offset: 0x000B03E7
	public bool tapping()
	{
		return this.bTap;
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x000B21EF File Offset: 0x000B03EF
	public bool raising()
	{
		return this.bRaise;
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x000B21F7 File Offset: 0x000B03F7
	public void OverrideRaising(bool braise)
	{
		this.bRaise = braise;
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000B2200 File Offset: 0x000B0400
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Under(int HoverObjectId)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.Under), HoverObjectId);
			return;
		}
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Under, this.GetSelectedLuaObjects(HoverObjectId, true, false), false))
		{
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(HoverObjectId, true, false))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component)
				{
					component.ResetIdleFreeze();
					component.ResetIdleFreezeAroundObject();
					if (component.cardScript)
					{
						component.cardScript.ResetCard();
					}
					Bounds boundsNotNormalized = component.GetBoundsNotNormalized();
					List<NetworkPhysicsObject> overlapObjects = component.GetOverlapObjects();
					List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
					for (int i = 0; i < overlapObjects.Count; i++)
					{
						NetworkPhysicsObject networkPhysicsObject = overlapObjects[i];
						List<NetworkPhysicsObject> overlapObjects2 = networkPhysicsObject.GetOverlapObjects();
						overlapObjects2.Add(networkPhysicsObject);
						for (int j = 0; j < overlapObjects2.Count; j++)
						{
							NetworkPhysicsObject networkPhysicsObject2 = overlapObjects2[j];
							if (!list.Contains(networkPhysicsObject2) && networkPhysicsObject2 != component)
							{
								list.Add(networkPhysicsObject2);
							}
						}
					}
					for (int k = 0; k < list.Count; k++)
					{
						NetworkPhysicsObject networkPhysicsObject3 = list[k];
						networkPhysicsObject3.ResetIdleFreeze();
						networkPhysicsObject3.rigidbody.position = new Vector3(networkPhysicsObject3.rigidbody.position.x, networkPhysicsObject3.rigidbody.position.y + boundsNotNormalized.size.y + 0.1f, networkPhysicsObject3.rigidbody.position.z);
					}
					component.HighlightNotify(this.PointerDarkColour);
				}
			}
		}
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000B23F8 File Offset: 0x000B05F8
	public void SpawnArrow(Vector3 pos)
	{
		this.LastArrowSpawnTime = Time.time;
		base.networkView.RPC<Vector3>(RPCTarget.All, new Action<Vector3>(this.RPCSpawnArrow), pos);
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x000B2420 File Offset: 0x000B0620
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Line", SerializationMethod.Default)]
	public void RPCSpawnArrow(Vector3 pos)
	{
		UnityEngine.Object.Instantiate<GameObject>(this.ArrowObject, pos, this.ArrowObject.transform.rotation).GetComponent<Renderer>().material.color = this.PointerColour;
		if (Network.isServer)
		{
			LuaGameObjectScript hoveredObject = this.HoverObject ? this.HoverObject.GetComponent<LuaGameObjectScript>() : null;
			EventManager.TriggerPlayerPing(this.PointerColorLabel, pos, hoveredObject);
		}
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x000B2494 File Offset: 0x000B0694
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Order(int HoverObjectId, OrderType orderType)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, OrderType>(RPCTarget.Server, new Action<int, OrderType>(this.Order), HoverObjectId, orderType);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(HoverObjectId, true, false))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component && component.stackObject && component.stackObject.bBag)
				{
					component.stackObject.Order = orderType;
					component.HighlightNotify(this.PointerDarkColour);
				}
			}
		}
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x000B2554 File Offset: 0x000B0754
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Randomize(int HoverObjectId, int id)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int>(RPCTarget.Server, new Action<int, int>(this.Randomize), HoverObjectId, id);
			return;
		}
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Randomize, this.GetSelectedLuaObjects(HoverObjectId, true, false), false))
		{
			return;
		}
		List<GameObject> selectedObjects = this.GetSelectedObjects(HoverObjectId, true, false);
		if (selectedObjects.Count == 1)
		{
			NetworkPhysicsObject component = selectedObjects[0].GetComponent<NetworkPhysicsObject>();
			if (component && component.CurrentPlayerHand)
			{
				selectedObjects.Clear();
				for (int i = 0; i < component.CurrentPlayerHand.ContainedNPOs.Count; i++)
				{
					selectedObjects.Add(component.CurrentPlayerHand.ContainedNPOs[i].gameObject);
				}
			}
		}
		Dictionary<string, List<NetworkPhysicsObject>> dictionary = new Dictionary<string, List<NetworkPhysicsObject>>();
		foreach (string key in Colour.HandPlayerLabels)
		{
			dictionary[key] = new List<NetworkPhysicsObject>();
		}
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject)
			{
				NetworkPhysicsObject component2 = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component2)
				{
					if (component2.CurrentPlayerHand)
					{
						dictionary[component2.CurrentPlayerHand.TriggerLabel].Add(component2);
						component2.HighlightNotify(this.PointerDarkColour);
						EventManager.TriggerObjectRandomize(component2, this.PointerColorLabel);
					}
					else
					{
						component2.HighlightNotify(this.PointerDarkColour);
						if (NetworkSingleton<ManagerPhysicsObject>.Instance.Randomize(component2, id))
						{
							EventManager.TriggerObjectRandomize(component2, this.PointerColorLabel);
						}
					}
				}
			}
		}
		foreach (KeyValuePair<string, List<NetworkPhysicsObject>> keyValuePair in dictionary)
		{
			if (keyValuePair.Value.Count > 1)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.RandomizeObjectsInHand(keyValuePair.Value);
			}
		}
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x000B2774 File Offset: 0x000B0974
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Rotation(int hoverObjectId, int spinRotationIndex, int flipRotationIndex, bool includeHeld = false)
	{
		if (spinRotationIndex == 0 && flipRotationIndex == 0)
		{
			return;
		}
		this.bRotating = true;
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, int, bool>(RPCTarget.Server, new Action<int, int, int, bool>(this.Rotation), hoverObjectId, spinRotationIndex, flipRotationIndex, includeHeld);
			return;
		}
		PlayerAction playerAction = PlayerAction.Select;
		if (spinRotationIndex == 0)
		{
			playerAction = ((flipRotationIndex == 12) ? PlayerAction.FlipOver : ((flipRotationIndex < 12 && flipRotationIndex > 0) ? PlayerAction.FlipIncrementalRight : PlayerAction.FlipIncrementalLeft));
		}
		else if (flipRotationIndex == 0)
		{
			playerAction = ((spinRotationIndex == 12) ? PlayerAction.RotateOver : ((spinRotationIndex < 12 && spinRotationIndex > 0) ? PlayerAction.RotateIncrementalRight : PlayerAction.RotateIncrementalLeft));
		}
		if (playerAction != PlayerAction.Select && !EventManager.CheckPlayerAction(this.PointerColorLabel, playerAction, this.GetSelectedLuaObjects(hoverObjectId, true, includeHeld), false))
		{
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(hoverObjectId, true, includeHeld))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				NetworkSingleton<ManagerPhysicsObject>.Instance.SetObjectRotation(component, spinRotationIndex, flipRotationIndex, this.ID);
			}
		}
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x000B2874 File Offset: 0x000B0A74
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Scaling", SerializationMethod.Default)]
	public void Scale(int HoverObjectId, bool bScaleUp)
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Scaling, this.ID))
		{
			PermissionsOptions.BroadcastPermissionWarning("Scale");
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int, bool>(RPCTarget.Server, new Action<int, bool>(this.Scale), HoverObjectId, bScaleUp);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(HoverObjectId, true, true))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				float num = component.ScaleAverage;
				float num2 = num;
				float num3 = 0.05f;
				if (bScaleUp)
				{
					component.rigidbody.AddForce(Vector3.up);
					if (num >= 1f)
					{
						num = Mathf.Min(15f, num + num3);
					}
					else
					{
						num = Mathf.Min(15f, num + num3 / 2f);
					}
				}
				else if (num > 1f)
				{
					num = Mathf.Max(0.25f, num - num3);
				}
				else
				{
					num = Mathf.Max(0.25f, num - num3 / 2f);
				}
				float num4 = num / num2;
				component.SetScale(new Vector3(component.Scale.x * num4, component.Scale.y * num4, component.Scale.z * num4), true);
				component.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x000B29F0 File Offset: 0x000B0BF0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Locking", SerializationMethod.Default)]
	public void Lock(int HoverObjectId, int OptionalLockValue = -1)
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Locking, this.ID))
		{
			PermissionsOptions.BroadcastPermissionWarning("Lock");
			return;
		}
		List<GameObject> selectedObjects = this.GetSelectedObjects(HoverObjectId, true, true);
		if (this.ID == NetworkID.ID)
		{
			Stats.INT_LOCK_ITEMS += selectedObjects.Count;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int, int>(RPCTarget.Server, new Action<int, int>(this.Lock), HoverObjectId, OptionalLockValue);
			return;
		}
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (OptionalLockValue == -1)
				{
					component.IsLocked = !component.IsLocked;
				}
				else
				{
					component.IsLocked = (OptionalLockValue == 1);
				}
				if (component.IsLocked)
				{
					component.Drop();
				}
				component.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000B2AF0 File Offset: 0x000B0CF0
	public void Lock(NetworkPhysicsObject npo, bool lockState)
	{
		List<NetworkPhysicsObject> npos = new List<NetworkPhysicsObject>
		{
			npo
		};
		this.Lock(npos, lockState);
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000B2B14 File Offset: 0x000B0D14
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Locking", SerializationMethod.Default)]
	public void Lock(List<NetworkPhysicsObject> npos, bool lockState)
	{
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Locking, this.ID))
		{
			PermissionsOptions.BroadcastPermissionWarning("Lock");
			return;
		}
		if (this.ID == NetworkID.ID)
		{
			Stats.INT_LOCK_ITEMS += npos.Count;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<List<NetworkPhysicsObject>, bool>(RPCTarget.Server, new Action<List<NetworkPhysicsObject>, bool>(this.Lock), npos, lockState);
			return;
		}
		foreach (NetworkPhysicsObject networkPhysicsObject in npos)
		{
			if (networkPhysicsObject)
			{
				networkPhysicsObject.IsLocked = lockState;
				if (networkPhysicsObject.IsLocked)
				{
					networkPhysicsObject.Drop();
				}
				networkPhysicsObject.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x000B2BEC File Offset: 0x000B0DEC
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Flick", SerializationMethod.Default)]
	private void Flick(Vector3 Force)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<Vector3>(RPCTarget.Server, new Action<Vector3>(this.Flick), Force);
			return;
		}
		List<GameObject> selectedObjects = this.GetSelectedObjects(-1, true, false);
		Force *= 7f;
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject && !gameObject.GetComponent<NetworkPhysicsObject>().IsLocked)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				EventManager.TriggerObjectFlick(component, this.PointerColorLabel, Force);
				component.rigidbody.isKinematic = false;
				component.ResetRigidbody();
				component.SetCollision(true);
				component.ResetIdleFreeze();
				component.ResetIdleFreezeAroundObject();
				component.rigidbody.AddForce(Force, ForceMode.Impulse);
				if (component.soundScript)
				{
					component.soundScript.PickUpSound();
				}
				component.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x000B2CF4 File Offset: 0x000B0EF4
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Combining", SerializationMethod.Default)]
	public void Combine(int HoverObjectId, int SourceObjectId)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int>(RPCTarget.Server, new Action<int, int>(this.Combine), HoverObjectId, SourceObjectId);
			return;
		}
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(HoverObjectId);
		foreach (GameObject gameObject2 in this.GetSelectedObjects(SourceObjectId, true, true))
		{
			if (!gameObject2)
			{
				break;
			}
			if (gameObject && gameObject != gameObject2)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				NetworkPhysicsObject component2 = gameObject2.GetComponent<NetworkPhysicsObject>();
				component.childSpawner.AddChild(component2);
				component.HighlightNotify(this.PointerDarkColour);
			}
			if (!gameObject)
			{
				gameObject2.GetComponent<NetworkPhysicsObject>().childSpawner.RemoveChildren();
			}
		}
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x000B2DD0 File Offset: 0x000B0FD0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Combining", SerializationMethod.Default)]
	public void JointFixed(int HoverObjectId, int SourceObjectId, JointFixedState jointState)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, JointFixedState>(RPCTarget.Server, new Action<int, int, JointFixedState>(this.JointFixed), HoverObjectId, SourceObjectId, jointState);
			return;
		}
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(HoverObjectId);
		foreach (GameObject gameObject2 in this.GetSelectedObjects(SourceObjectId, true, true))
		{
			if (!gameObject2)
			{
				break;
			}
			if (gameObject2.GetComponent<Joint>())
			{
				UnityEngine.Object.Destroy(gameObject2.GetComponent<Joint>());
			}
			if (gameObject && gameObject != gameObject2)
			{
				FixedJoint fixedJoint = gameObject2.AddComponent<FixedJoint>();
				NetworkPhysicsObject component = gameObject2.GetComponent<NetworkPhysicsObject>();
				component.AddJoint(fixedJoint);
				Joint joint = fixedJoint;
				joint.connectedBody = gameObject.GetComponent<Rigidbody>();
				joint.enableCollision = jointState.EnableCollision;
				if (jointState.Axis.ToVector() != Vector3.zero)
				{
					joint.axis = jointState.Axis.ToVector();
				}
				joint.anchor = jointState.Anchor.ToVector();
				if (jointState.ConnectedAnchor.ToVector() != Vector3.zero)
				{
					joint.connectedAnchor = jointState.ConnectedAnchor.ToVector();
				}
				if (jointState.BreakForce != 0f)
				{
					joint.breakForce = jointState.BreakForce;
				}
				if (jointState.BreakTorgue != 0f)
				{
					joint.breakTorque = jointState.BreakTorgue;
				}
				if (gameObject2.GetComponent<SoundScript>())
				{
					gameObject2.GetComponent<SoundScript>().PickUpSound();
				}
				component.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x000B2F90 File Offset: 0x000B1190
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Combining", SerializationMethod.Default)]
	public void JointHinge(int HoverObjectId, int SourceObjectId, JointHingeState jointState)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, JointHingeState>(RPCTarget.Server, new Action<int, int, JointHingeState>(this.JointHinge), HoverObjectId, SourceObjectId, jointState);
			return;
		}
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(HoverObjectId);
		foreach (GameObject gameObject2 in this.GetSelectedObjects(SourceObjectId, true, true))
		{
			if (!gameObject2)
			{
				break;
			}
			if (gameObject2.GetComponent<Joint>())
			{
				UnityEngine.Object.Destroy(gameObject2.GetComponent<Joint>());
			}
			if (gameObject && gameObject != gameObject2)
			{
				HingeJoint hingeJoint = gameObject2.AddComponent<HingeJoint>();
				NetworkPhysicsObject component = gameObject2.GetComponent<NetworkPhysicsObject>();
				component.AddJoint(hingeJoint);
				Joint joint = hingeJoint;
				joint.connectedBody = gameObject.GetComponent<Rigidbody>();
				joint.enableCollision = jointState.EnableCollision;
				if (jointState.Axis.ToVector() != Vector3.zero)
				{
					joint.axis = jointState.Axis.ToVector();
				}
				joint.anchor = jointState.Anchor.ToVector();
				if (jointState.ConnectedAnchor.ToVector() != Vector3.zero)
				{
					joint.connectedAnchor = jointState.ConnectedAnchor.ToVector();
				}
				if (jointState.BreakForce != 0f)
				{
					joint.breakForce = jointState.BreakForce;
				}
				if (jointState.BreakTorgue != 0f)
				{
					joint.breakTorque = jointState.BreakTorgue;
				}
				if (jointState.Motor.force != 0f || jointState.Motor.targetVelocity != 0f)
				{
					hingeJoint.useMotor = true;
					hingeJoint.motor = new JointMotor
					{
						force = jointState.Motor.force,
						targetVelocity = jointState.Motor.targetVelocity,
						freeSpin = jointState.Motor.freeSpin
					};
				}
				if (gameObject2.GetComponent<SoundScript>())
				{
					gameObject2.GetComponent<SoundScript>().PickUpSound();
				}
				component.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A1 RID: 6561 RVA: 0x000B31C0 File Offset: 0x000B13C0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Combining", SerializationMethod.Default)]
	public void JointSpring(int HoverObjectId, int SourceObjectId, JointSpringState jointState)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, int, JointSpringState>(RPCTarget.Server, new Action<int, int, JointSpringState>(this.JointSpring), HoverObjectId, SourceObjectId, jointState);
			return;
		}
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromID(HoverObjectId);
		foreach (GameObject gameObject2 in this.GetSelectedObjects(SourceObjectId, true, true))
		{
			if (!gameObject2)
			{
				break;
			}
			if (gameObject2.GetComponent<Joint>())
			{
				UnityEngine.Object.Destroy(gameObject2.GetComponent<Joint>());
			}
			if (gameObject && gameObject != gameObject2)
			{
				SpringJoint springJoint = gameObject2.AddComponent<SpringJoint>();
				NetworkPhysicsObject component = gameObject2.GetComponent<NetworkPhysicsObject>();
				component.AddJoint(springJoint);
				Joint joint = springJoint;
				joint.connectedBody = gameObject.GetComponent<Rigidbody>();
				joint.enableCollision = jointState.EnableCollision;
				if (jointState.Axis.ToVector() != Vector3.zero)
				{
					joint.axis = jointState.Axis.ToVector();
				}
				joint.anchor = jointState.Anchor.ToVector();
				if (jointState.ConnectedAnchor.ToVector() != Vector3.zero)
				{
					joint.connectedAnchor = jointState.ConnectedAnchor.ToVector();
				}
				if (jointState.BreakForce != 0f)
				{
					joint.breakForce = jointState.BreakForce;
				}
				if (jointState.BreakTorgue != 0f)
				{
					joint.breakTorque = jointState.BreakTorgue;
				}
				if (jointState.Spring != 0f)
				{
					springJoint.spring = jointState.Spring;
				}
				if (jointState.Damper != 0f)
				{
					springJoint.damper = jointState.Damper;
				}
				if (jointState.MaxDistance != 0f)
				{
					springJoint.maxDistance = jointState.MaxDistance;
				}
				if (jointState.MinDistance != 0f)
				{
					springJoint.minDistance = jointState.MinDistance;
				}
				if (gameObject2.GetComponent<SoundScript>())
				{
					gameObject2.GetComponent<SoundScript>().PickUpSound();
				}
				component.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x000B33E4 File Offset: 0x000B15E4
	[Remote(Permission.Server)]
	public void SetPhysics(int HoverObjectId, RigidbodyState rigidbodyState, PhysicsMaterialState physicsMaterialState)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int, RigidbodyState, PhysicsMaterialState>(RPCTarget.Server, new Action<int, RigidbodyState, PhysicsMaterialState>(this.SetPhysics), HoverObjectId, rigidbodyState, physicsMaterialState);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(HoverObjectId, true, true))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				component.OverrideRigidbody = rigidbodyState;
				component.OverridePhysicsMaterial = physicsMaterialState;
				component.ResetObject();
				component.ResetPhysicsMaterial();
				component.HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x000B3490 File Offset: 0x000B1690
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void DragSelectable(bool bDragSelectable)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.DragSelectable), bDragSelectable);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().IsDragSelectable = bDragSelectable;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x000B3530 File Offset: 0x000B1730
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void MeasureMovement(bool bMeasureMovement)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.MeasureMovement), bMeasureMovement);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().ShowRulerWhenHeld = bMeasureMovement;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000B35D0 File Offset: 0x000B17D0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Grid(bool bGrid)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Grid), bGrid);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().IgnoresGrid = !bGrid;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000B3674 File Offset: 0x000B1874
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Snap(bool bSnap)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Snap), bSnap);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().IgnoresSnap = !bSnap;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x000B3718 File Offset: 0x000B1918
	[Remote(Permission.Admin, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Destroyable(bool bDestroyable)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Destroyable), bDestroyable);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().DoesNotPersist = bDestroyable;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x000B37B8 File Offset: 0x000B19B8
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Autoraise(bool bAutoraise)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Autoraise), bAutoraise);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().DoAutoRaise = bAutoraise;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x000B3858 File Offset: 0x000B1A58
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Sticky(bool bSticky)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Sticky), bSticky);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().IsSticky = bSticky;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000B38F8 File Offset: 0x000B1AF8
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Tooltip(bool bTooltip)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Tooltip), bTooltip);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().ShowTooltip = bTooltip;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x000B3994 File Offset: 0x000B1B94
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void FogOfWarReveal(bool bFogOfWarReveal)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.FogOfWarReveal), bFogOfWarReveal);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<FogOfWarRevealer>().Active = bFogOfWarReveal;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x000B3A30 File Offset: 0x000B1C30
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void GridProjection(bool bGridProjection)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.GridProjection), bGridProjection);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().ShowGridProjection = bGridProjection;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x000B3ACC File Offset: 0x000B1CCC
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Hands(bool bHands)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.Hands), bHands);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().CanBeHeldInHand = bHands;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x000B3B68 File Offset: 0x000B1D68
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void HideFaceDown(bool bHideWhenFaceDown)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.HideFaceDown), bHideWhenFaceDown);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().IsHiddenWhenFaceDown = bHideWhenFaceDown;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000B3C04 File Offset: 0x000B1E04
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void IgnoreFogOfWar(bool bIgnoreFogOfWar)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<bool>(RPCTarget.Server, new Action<bool>(this.IgnoreFogOfWar), bIgnoreFogOfWar);
			return;
		}
		foreach (GameObject gameObject in this.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().IgnoresFogOfWar = bIgnoreFogOfWar;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x000B3CA0 File Offset: 0x000B1EA0
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Material(int MatInt)
	{
		if (MatInt == -1)
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.Material), MatInt);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject && (gameObject.tag == "Dice" || gameObject.tag == "Domino" || gameObject.tag == "Chess") && gameObject.name != "Die_6_Rounded(Clone)" && gameObject.GetComponent<MaterialSyncScript>())
			{
				bool flag = MatInt == 0;
				bool flag2 = MatInt == 1;
				bool flag3 = MatInt == 2;
				int num = gameObject.GetComponent<MaterialSyncScript>().GetMaterial();
				if (gameObject.tag != "Chess")
				{
					if (flag && num != 0)
					{
						gameObject.GetComponent<NetworkPhysicsObject>().SetObject(false, -1, 0);
					}
					else if (flag2 && num != 1)
					{
						gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
					}
					else if (flag3 && num != 2)
					{
						gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
					}
				}
				else if (flag && num != 2 && num != 3)
				{
					if (num == 4)
					{
						num = 0;
					}
					gameObject.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, num + 2);
				}
				else if (flag2 && num != 0 && num != 1)
				{
					if (num == 4)
					{
						num = 2;
					}
					gameObject.GetComponent<NetworkPhysicsObject>().SetObject(false, -1, num - 2);
				}
				else if (flag3)
				{
					gameObject.GetComponent<NetworkPhysicsObject>().SetObject(false, -1, 4);
				}
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000B3E88 File Offset: 0x000B2088
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void Info(string Name, string Desc, int Value)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string, string, int>(RPCTarget.Server, new Action<string, string, int>(this.Info), Name, Desc, Value);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				component.Name = Name;
				component.Description = Desc;
				component.Value = Value;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x000B3F38 File Offset: 0x000B2138
	[Remote(Permission.Admin, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void GMInfo(string Notes)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.GMInfo), Notes);
			return;
		}
		foreach (GameObject gameObject in base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().GMNotes = Notes;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x000B3FD8 File Offset: 0x000B21D8
	[Remote(Permission.Owner, SendType.ReliableNoDelay, "Permissions/Contextual", SerializationMethod.Default)]
	public void DiffuseColor(Color NewColor)
	{
		List<GameObject> selectedObjects = base.GetComponent<Pointer>().GetSelectedObjects(-1, true, false);
		if (this.ID == NetworkID.ID)
		{
			Stats.INT_TINT_OBJECTS += selectedObjects.Count;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<Color>(RPCTarget.Server, new Action<Color>(this.DiffuseColor), NewColor);
			return;
		}
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().DiffuseColor = NewColor;
				gameObject.GetComponent<NetworkPhysicsObject>().HighlightNotify(this.PointerDarkColour);
			}
		}
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x000B4098 File Offset: 0x000B2298
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Group(int HoverObjectId)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.Group), HoverObjectId);
			return;
		}
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Group, this.GetSelectedLuaObjects(HoverObjectId, true, true), false))
		{
			return;
		}
		List<GameObject> selectedObjects = this.GetSelectedObjects(HoverObjectId, true, true);
		NetworkSingleton<ManagerPhysicsObject>.Instance.Group(selectedObjects);
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000B40F8 File Offset: 0x000B22F8
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void Group(List<NetworkPhysicsObject> npos)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<List<NetworkPhysicsObject>>(RPCTarget.Server, new Action<List<NetworkPhysicsObject>>(this.Group), npos);
			return;
		}
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>(npos.Count);
		foreach (NetworkPhysicsObject networkPhysicsObject in npos)
		{
			LuaGameObjectScript component = networkPhysicsObject.GetComponent<LuaGameObjectScript>();
			if (component)
			{
				list.Add(component);
			}
		}
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, PlayerAction.Group, list, false))
		{
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.Group(npos);
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x000B41A0 File Offset: 0x000B23A0
	public bool CheckGroup()
	{
		return NetworkSingleton<ManagerPhysicsObject>.Instance.CheckGroup(this.GetSelectedObjects(-1, true, false));
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x000B41B5 File Offset: 0x000B23B5
	private int GetHoverID()
	{
		if (this.HoverObject && !zInput.GetButton("Grab", ControlType.All))
		{
			return this.HoverObject.GetComponent<NetworkPhysicsObject>().ID;
		}
		return -1;
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x000B41E3 File Offset: 0x000B23E3
	public int GetHoverLockID()
	{
		if (this.HoverLockObject && !zInput.GetButton("Grab", ControlType.All))
		{
			return this.HoverLockObject.GetComponent<NetworkPhysicsObject>().ID;
		}
		return -1;
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x000B4214 File Offset: 0x000B2414
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void ChangeHeldGridLayout(int number)
	{
		List<NetworkPhysicsObject> heldObjects = this.HeldObjects;
		for (int i = this.HeldObjects.Count - 1; i >= 0; i--)
		{
			if (this.HeldObjects[i] == null)
			{
				this.HeldObjects.RemoveAt(i);
			}
		}
		if (heldObjects.Count == 0)
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int>(RPCTarget.Server, new Action<int>(this.ChangeHeldGridLayout), number);
			return;
		}
		float num = 0f;
		float num2 = 0f;
		foreach (NetworkPhysicsObject networkPhysicsObject in heldObjects)
		{
			Bounds bounds = networkPhysicsObject.GetBounds();
			if (bounds.size.x > num)
			{
				num = bounds.size.x;
			}
			if (bounds.size.z > num2)
			{
				num2 = bounds.size.z;
			}
		}
		num += 0.1f;
		num2 += 0.1f;
		int num3 = number;
		int num4 = Mathf.CeilToInt((float)heldObjects.Count / (float)num3);
		if (number == 0)
		{
			num3 = heldObjects.Count;
			num4 = 1;
		}
		Vector3 eulerAngles = new Vector3(0f, base.transform.eulerAngles.y - 180f, 0f);
		int num5 = NetworkSingleton<ManagerPhysicsObject>.Instance.SpinRotationIndexFromGrabbable(base.gameObject, (float)this.RotationSnap);
		eulerAngles = NetworkSingleton<ManagerPhysicsObject>.Instance.RotationFromIndex(num5).eulerAngles;
		eulerAngles.y -= 180f;
		eulerAngles.y = (float)(Mathf.RoundToInt(eulerAngles.y / 5f) * 5);
		int num6 = 0;
		for (int j = 0; j < num3; j++)
		{
			for (int k = 0; k < num4; k++)
			{
				Vector3 vector = new Vector3((float)k * num, 0f, (float)j * num2);
				vector = Utilities.RotatePointAroundPivot(vector, Vector3.zero, eulerAngles);
				NetworkPhysicsObject networkPhysicsObject2 = heldObjects[num6];
				networkPhysicsObject2.HeldSpinRotationIndex = num5;
				networkPhysicsObject2.HeldOffset = vector;
				networkPhysicsObject2.HeldRotationOffset = Vector3.zero;
				networkPhysicsObject2.bReduceForce = false;
				num6++;
				if (num6 >= heldObjects.Count)
				{
					return;
				}
			}
		}
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x000B444C File Offset: 0x000B264C
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void ChangeHeldSpinRotationIndex(int spinRotationDelta, int touchId = -1)
	{
		if (spinRotationDelta == 0)
		{
			return;
		}
		this.bRotating = true;
		if (Network.isClient)
		{
			base.networkView.RPC<int, int>(RPCTarget.Server, new Action<int, int>(this.ChangeHeldSpinRotationIndex), spinRotationDelta, touchId);
			return;
		}
		PlayerAction action = (spinRotationDelta == 12) ? PlayerAction.RotateOver : ((spinRotationDelta < 12 && spinRotationDelta > 0) ? PlayerAction.RotateIncrementalRight : PlayerAction.RotateIncrementalLeft);
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, action, this.GetGrabbedLuaObjects(touchId), false))
		{
			return;
		}
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			if (networkPhysicsObject.HeldByPlayerID == this.ID && networkPhysicsObject.HeldByTouchID == touchId)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SetHeldObjectSpinRotationIndex(networkPhysicsObject, spinRotationDelta, this.ID, this.FirstGrabbedNPO);
			}
		}
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x000B4528 File Offset: 0x000B2728
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void ChangeHeldFlipRotationIndex(int flipRotationDelta, int touchId = -1)
	{
		if (flipRotationDelta == 0)
		{
			return;
		}
		if (Network.isClient)
		{
			base.networkView.RPC<int, int>(RPCTarget.Server, new Action<int, int>(this.ChangeHeldFlipRotationIndex), flipRotationDelta, touchId);
			return;
		}
		PlayerAction action = (flipRotationDelta == 12) ? PlayerAction.FlipOver : ((flipRotationDelta < 12 && flipRotationDelta > 0) ? PlayerAction.FlipIncrementalRight : PlayerAction.FlipIncrementalLeft);
		if (!EventManager.CheckPlayerAction(this.PointerColorLabel, action, this.GetGrabbedLuaObjects(touchId), false))
		{
			return;
		}
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			if (networkPhysicsObject.HeldByPlayerID == this.ID && networkPhysicsObject.HeldByTouchID == touchId)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SetHeldObjectFlipRotationIndex(networkPhysicsObject, flipRotationDelta, this.ID);
			}
		}
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000B45F4 File Offset: 0x000B27F4
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void TellManagerPhysicsObjectAboutRelease(int player_id, int drop_id, int touch_id, int hover_id)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.ClientRelease(player_id, drop_id, touch_id, hover_id);
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x000B4605 File Offset: 0x000B2805
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void TellObjectManagerAboutCardPeel(int grab_id, int player_id, int touch_id)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.ClientCardPeel(grab_id, player_id, touch_id);
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x000B4614 File Offset: 0x000B2814
	[Remote(Permission.Owner, SendType.ReliableNoDelay, null, SerializationMethod.Default)]
	public void TellObjectManagerAboutObjectTake(int grab_id, int player_id, int touch_id)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.ClientTakeObject(grab_id, player_id, touch_id);
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x000B4624 File Offset: 0x000B2824
	public bool StartInteract(NetworkPhysicsObject InteractNPO, Vector3 Position, Quaternion Rotation, int SyncId)
	{
		PointerMode currentPointerMode = this.CurrentPointerMode;
		return false;
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000B463C File Offset: 0x000B283C
	public bool UpdateInteract(NetworkPhysicsObject InteractNPO, Vector3 Position, Quaternion Rotation, int SyncId)
	{
		PointerMode currentPointerMode = this.CurrentPointerMode;
		return false;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x000B4654 File Offset: 0x000B2854
	public bool EndInteract(NetworkPhysicsObject InteractNPO, Vector3 Position, Quaternion Rotation, int SyncId)
	{
		PointerMode currentPointerMode = this.CurrentPointerMode;
		return false;
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000B466C File Offset: 0x000B286C
	public static bool ContainsPointerMode(List<PointerMode> list, PointerMode mode)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == mode)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x000B4697 File Offset: 0x000B2897
	public static bool IsVectorTool(PointerMode mode)
	{
		return Pointer.ContainsPointerMode(Pointer.VectorTools, mode);
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000B46A4 File Offset: 0x000B28A4
	public static bool IsZoneTool(PointerMode mode)
	{
		return Pointer.ContainsPointerMode(Pointer.ZoneTools, mode);
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000B46B1 File Offset: 0x000B28B1
	public static bool IsGizmoTool(PointerMode mode)
	{
		return Pointer.ContainsPointerMode(Pointer.GizmoTools, mode);
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000B46BE File Offset: 0x000B28BE
	public static bool IsSnapTool(PointerMode mode)
	{
		return Pointer.ContainsPointerMode(Pointer.SnapTools, mode);
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x000B46CB File Offset: 0x000B28CB
	public static bool IsCombineTool(PointerMode mode)
	{
		return Pointer.ContainsPointerMode(Pointer.CombineTools, mode);
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x000B46D8 File Offset: 0x000B28D8
	public void ToggleBetweenModes(List<PointerMode> tools)
	{
		if (tools.Contains(this.CurrentPointerMode))
		{
			int num = tools.IndexOf(this.CurrentPointerMode);
			num += (zInput.GetButton("Shift", ControlType.All) ? -1 : 1);
			if (num >= tools.Count)
			{
				num = 0;
			}
			if (num < 0)
			{
				num = tools.Count - 1;
			}
			this.CurrentPointerMode = tools[num];
			return;
		}
		this.CurrentPointerMode = tools[zInput.GetButton("Shift", ControlType.All) ? (tools.Count - 1) : 0];
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x000B475E File Offset: 0x000B295E
	public void InteractiveSpawn(SpawnDelegate spawnInPlace, ObjectVisualizer objectVisualizer)
	{
		this.InteractiveSpawning = true;
		this.spawnInPlace = spawnInPlace;
		Singleton<ObjectPositioningVisualizer>.Instance.SetSpawnVisualizer(objectVisualizer);
	}

	// Token: 0x04000EF1 RID: 3825
	public static bool DEFAULT_TYPED_NUMBER_IS_SINGLE_DIGIT = false;

	// Token: 0x04000EF2 RID: 3826
	public static bool DOUBLE_CLICK_CONTAINER_PICKUP = false;

	// Token: 0x04000EF3 RID: 3827
	public static bool LOG_INVENTORY = true;

	// Token: 0x04000EF4 RID: 3828
	public static float SPREAD_ACTION_DISTANCE = 0.6f;

	// Token: 0x04000EF5 RID: 3829
	public static float SPREAD_ACTION_ROW_DISTANCE = 3.4f;

	// Token: 0x04000EF6 RID: 3830
	public static int SPREAD_ACTION_CARDS_PER_ROW = 52;

	// Token: 0x04000EF7 RID: 3831
	private const int LINE_INSTANCE_COUNT = 5;

	// Token: 0x04000EF8 RID: 3832
	public static int LineToolFontSize = 16;

	// Token: 0x04000EF9 RID: 3833
	public static float LineToolArrowHeadLength = 0.4f;

	// Token: 0x04000EFA RID: 3834
	public static float MeasureToolArrowHeadAngle = 0f;

	// Token: 0x04000EFB RID: 3835
	private const float FlickToolArrowHeadAngle = 20f;

	// Token: 0x04000EFC RID: 3836
	private const float CombineToolArrowHeadAngle = 45f;

	// Token: 0x04000EFD RID: 3837
	private int _ID = -1;

	// Token: 0x04000EFE RID: 3838
	private bool _bTap;

	// Token: 0x04000EFF RID: 3839
	private bool _bRaise;

	// Token: 0x04000F00 RID: 3840
	[NonSerialized]
	public bool InteractiveSpawning;

	// Token: 0x04000F01 RID: 3841
	private Vector3 lastSpawnPosition;

	// Token: 0x04000F02 RID: 3842
	private SpawnDelegate spawnInPlace;

	// Token: 0x04000F03 RID: 3843
	public string playerName = "";

	// Token: 0x04000F04 RID: 3844
	private Vector3 refcurPosition;

	// Token: 0x04000F05 RID: 3845
	public string PointerColorLabel = "White";

	// Token: 0x04000F08 RID: 3848
	public uint PointerColourFlag;

	// Token: 0x04000F09 RID: 3849
	public const float STACK_HOLD_THRESHOLD = 0.4f;

	// Token: 0x04000F0A RID: 3850
	public const float DOUBLE_CLICK_THRESHOLD = 0.4f;

	// Token: 0x04000F0B RID: 3851
	private float containerHeldTime;

	// Token: 0x04000F0C RID: 3852
	private float clickReleaseTime;

	// Token: 0x04000F0D RID: 3853
	private int containerHeldID = -1;

	// Token: 0x04000F0E RID: 3854
	private bool bStopGrab;

	// Token: 0x04000F0F RID: 3855
	private Vector3 clickPosition;

	// Token: 0x04000F10 RID: 3856
	[NonSerialized]
	public bool ClickingWhileInHandSelectMode;

	// Token: 0x04000F11 RID: 3857
	private NetworkPhysicsObject lastClickedNPO;

	// Token: 0x04000F13 RID: 3859
	public uint interpolatePackedId;

	// Token: 0x04000F14 RID: 3860
	public Texture2D TextDefault;

	// Token: 0x04000F15 RID: 3861
	public Texture2D TextOpen;

	// Token: 0x04000F16 RID: 3862
	public Texture2D TextGrab;

	// Token: 0x04000F17 RID: 3863
	public Texture2D TextPaint;

	// Token: 0x04000F18 RID: 3864
	public Texture2D TextErase;

	// Token: 0x04000F19 RID: 3865
	public Texture2D TextLine;

	// Token: 0x04000F1A RID: 3866
	public Texture2D TextFlick;

	// Token: 0x04000F1B RID: 3867
	public Texture2D TextJoint;

	// Token: 0x04000F1C RID: 3868
	public Texture2D TextArrow;

	// Token: 0x04000F1D RID: 3869
	public Texture2D TextText;

	// Token: 0x04000F1E RID: 3870
	public Texture2D TextGizmo;

	// Token: 0x04000F1F RID: 3871
	public Texture2D TextSnap;

	// Token: 0x04000F20 RID: 3872
	public Texture2D TextDecal;

	// Token: 0x04000F21 RID: 3873
	public Texture2D TextVector;

	// Token: 0x04000F22 RID: 3874
	public Texture2D TextZone;

	// Token: 0x04000F23 RID: 3875
	public Texture2D TextSpawn;

	// Token: 0x04000F24 RID: 3876
	private GameObject firstGrabbedObject;

	// Token: 0x04000F26 RID: 3878
	private List<ObjectState> ObjectCopyStates = new List<ObjectState>();

	// Token: 0x04000F27 RID: 3879
	public int num_grabbed;

	// Token: 0x04000F28 RID: 3880
	private int _PointerMeshIndex = -1;

	// Token: 0x04000F29 RID: 3881
	private int _PointerTypeIndex = -1;

	// Token: 0x04000F2A RID: 3882
	public bool bShowHardwareCursor;

	// Token: 0x04000F2B RID: 3883
	private NetworkView _HoverView;

	// Token: 0x04000F2C RID: 3884
	public NetworkView PrevHoverView;

	// Token: 0x04000F2D RID: 3885
	public GameObject PointerMeshFollower;

	// Token: 0x04000F2E RID: 3886
	public GameObject ReferenceFollower;

	// Token: 0x04000F2F RID: 3887
	private GameObject HoverOverFacade;

	// Token: 0x04000F30 RID: 3888
	public MeshFilter HoverOverFacadeMesh;

	// Token: 0x04000F31 RID: 3889
	private PhysicsState HoverOverState;

	// Token: 0x04000F32 RID: 3890
	private int typedNumberTargetID = -1;

	// Token: 0x04000F33 RID: 3891
	private int typedNumberMagnitude = -1;

	// Token: 0x04000F34 RID: 3892
	private bool typedNumberIsNegative;

	// Token: 0x04000F35 RID: 3893
	private float typedNumberLastKeypressAt;

	// Token: 0x04000F36 RID: 3894
	private int typedNumberDigitCount;

	// Token: 0x04000F37 RID: 3895
	private int typedNumberMaxDigits;

	// Token: 0x04000F38 RID: 3896
	public static Transform TypedNumberIntercept;

	// Token: 0x04000F39 RID: 3897
	private static Transform prevTypedNumberIntercept;

	// Token: 0x04000F3A RID: 3898
	private static string typedNumberInterceptColor;

	// Token: 0x04000F3B RID: 3899
	private static int typedNumberInterceptMaxNumber = 0;

	// Token: 0x04000F3C RID: 3900
	private List<LineRenderer> LineInstances = new List<LineRenderer>();

	// Token: 0x04000F3D RID: 3901
	private Vector3 _StartLineVector = Vector3.zero;

	// Token: 0x04000F3E RID: 3902
	private Vector3 _EndLineVector = Vector3.zero;

	// Token: 0x04000F3F RID: 3903
	private bool _LineVectorInComponents;

	// Token: 0x04000F40 RID: 3904
	private bool _MeasuringXZ = true;

	// Token: 0x04000F41 RID: 3905
	private Vector3 StartMarqueeVector = Vector3.zero;

	// Token: 0x04000F43 RID: 3907
	private List<GameObject> CtrlHighLightedObjects = new List<GameObject>();

	// Token: 0x04000F44 RID: 3908
	private bool bStopMarquee;

	// Token: 0x04000F45 RID: 3909
	public bool bHideHoverHighlight;

	// Token: 0x04000F46 RID: 3910
	public List<NetworkPhysicsObject> RecentlyDropped = new List<NetworkPhysicsObject>();

	// Token: 0x04000F47 RID: 3911
	private float LineHeldTime;

	// Token: 0x04000F48 RID: 3912
	private float LastArrowSpawnTime;

	// Token: 0x04000F49 RID: 3913
	private float ArrowCooldown = 0.5f;

	// Token: 0x04000F4A RID: 3914
	public GameObject ArrowObject;

	// Token: 0x04000F4B RID: 3915
	public GameObject ShakeDetectorObject;

	// Token: 0x04000F4C RID: 3916
	public bool bShaking;

	// Token: 0x04000F4D RID: 3917
	private Vector3 InfoStartVector = Vector3.zero;

	// Token: 0x04000F4E RID: 3918
	private bool justStartedZoneMenu;

	// Token: 0x04000F55 RID: 3925
	private float TapHeldTime;

	// Token: 0x04000F56 RID: 3926
	private GameObject _HoverObject;

	// Token: 0x04000F57 RID: 3927
	private GameObject HoverLockObject;

	// Token: 0x04000F58 RID: 3928
	private GameObject PrevHoverObject;

	// Token: 0x04000F59 RID: 3929
	private GameObject UICameraObject;

	// Token: 0x04000F5A RID: 3930
	private NetworkUI NetworkInstance;

	// Token: 0x04000F5B RID: 3931
	private float LastRotateTime;

	// Token: 0x04000F5C RID: 3932
	private float LastScaleTime;

	// Token: 0x04000F5D RID: 3933
	private int ScaleCount;

	// Token: 0x04000F5E RID: 3934
	private Vector3 StartZoneVector = Vector3.zero;

	// Token: 0x04000F5F RID: 3935
	private GameObject ZoneObject;

	// Token: 0x04000F60 RID: 3936
	public bool bRotating;

	// Token: 0x04000F61 RID: 3937
	private static RaycastHit[] raycastHits = new RaycastHit[20];

	// Token: 0x04000F62 RID: 3938
	public PointerState CurrentPointerState;

	// Token: 0x04000F63 RID: 3939
	private PointerMode _currentPointerMode;

	// Token: 0x04000F64 RID: 3940
	private PointerMode previousPointerMode;

	// Token: 0x04000F65 RID: 3941
	private PointerMode previousPointerModeWithDup;

	// Token: 0x04000F66 RID: 3942
	private PointerMode PrevDisablePointerMode;

	// Token: 0x04000F67 RID: 3943
	private NetworkPhysicsObject startedLineWhileHolding;

	// Token: 0x04000F68 RID: 3944
	public NetworkPhysicsObject startedLineWhileHovering;

	// Token: 0x04000F69 RID: 3945
	private bool pickedUpMeasuredObject;

	// Token: 0x04000F6A RID: 3946
	private bool zeroMeasurement;

	// Token: 0x04000F6B RID: 3947
	private bool hasLoggedMeasurementStart;

	// Token: 0x04000F6C RID: 3948
	private bool measuringPickedUpObject;

	// Token: 0x04000F6D RID: 3949
	private int rotationSnap = 15;

	// Token: 0x04000F6E RID: 3950
	private float raiseHeight = 0.2f;

	// Token: 0x04000F6F RID: 3951
	private CapsuleCollider ThisCollider;

	// Token: 0x04000F70 RID: 3952
	public Color HoverColor = Color.white;

	// Token: 0x04000F71 RID: 3953
	public Color SelectColor = Color.yellow;

	// Token: 0x04000F72 RID: 3954
	public Rigidbody rigidbody;

	// Token: 0x04000F73 RID: 3955
	public float AutoRaise;

	// Token: 0x04000F74 RID: 3956
	public float PrevAutoRaise;

	// Token: 0x04000F75 RID: 3957
	public PointerSyncs pointerSyncs;

	// Token: 0x04000F76 RID: 3958
	private Camera MainCamera;

	// Token: 0x04000F77 RID: 3959
	private bool bDeselectOnGrab;

	// Token: 0x04000F78 RID: 3960
	private PointerMode prevInteractivePointerMode = PointerMode.None;

	// Token: 0x04000F79 RID: 3961
	private string[] numberKeys = new string[]
	{
		"0",
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8",
		"9"
	};

	// Token: 0x04000F7A RID: 3962
	public List<NetworkPhysicsObject> HeldObjects = new List<NetworkPhysicsObject>();

	// Token: 0x04000F7B RID: 3963
	private readonly List<NetworkPhysicsObject> pendingGrabbedNpos = new List<NetworkPhysicsObject>(128);

	// Token: 0x04000F7C RID: 3964
	private readonly List<LuaGameObjectScript> pendingGrabbedLuaObjects = new List<LuaGameObjectScript>(128);

	// Token: 0x04000F7D RID: 3965
	private UILabel LineLabel;

	// Token: 0x04000F7E RID: 3966
	private UILabel LineLabelX;

	// Token: 0x04000F7F RID: 3967
	private UILabel LineLabelZ;

	// Token: 0x04000F80 RID: 3968
	private bool selectionWasChanged;

	// Token: 0x04000F81 RID: 3969
	private PointerMode currentZoneMode = PointerMode.None;

	// Token: 0x04000F82 RID: 3970
	public List<GameObject> ActiveItems = new List<GameObject>();

	// Token: 0x04000F83 RID: 3971
	public static bool SHOW_GM_NOTES = true;

	// Token: 0x04000F84 RID: 3972
	public Vector3 StartGlobalPointerPos = Vector3.zero;

	// Token: 0x04000F85 RID: 3973
	private bool bCopied;

	// Token: 0x04000F86 RID: 3974
	public List<GameObject> GrabbedObjects = new List<GameObject>();

	// Token: 0x04000F87 RID: 3975
	public static readonly List<PointerMode> VectorTools = new List<PointerMode>
	{
		PointerMode.Vector,
		PointerMode.VectorLine,
		PointerMode.VectorBox,
		PointerMode.VectorCircle,
		PointerMode.VectorErase
	};

	// Token: 0x04000F88 RID: 3976
	public static readonly List<PointerMode> ZoneTools = new List<PointerMode>
	{
		PointerMode.Hidden,
		PointerMode.Randomize,
		PointerMode.Hands,
		PointerMode.LayoutZone,
		PointerMode.FogOfWar,
		PointerMode.Scripting
	};

	// Token: 0x04000F89 RID: 3977
	public static readonly List<PointerMode> GizmoTools = new List<PointerMode>
	{
		PointerMode.Move,
		PointerMode.Rotate,
		PointerMode.Scale,
		PointerMode.VolumeScale,
		PointerMode.RotationValue
	};

	// Token: 0x04000F8A RID: 3978
	public static readonly List<PointerMode> SnapTools = new List<PointerMode>
	{
		PointerMode.Snap,
		PointerMode.SnapRotate
	};

	// Token: 0x04000F8B RID: 3979
	public static readonly List<PointerMode> CombineTools = new List<PointerMode>
	{
		PointerMode.Attach,
		PointerMode.FixedJoint,
		PointerMode.HingeJoint,
		PointerMode.SpringJoint
	};
}
