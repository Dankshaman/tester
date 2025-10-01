using System;
using System.Collections.Generic;
using System.Text;
using HighlightingSystem;
using NewNet;
using TouchScript;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012B RID: 299
public class HoverScript : MonoBehaviour
{
	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0006B3A5 File Offset: 0x000695A5
	// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x0006B3B9 File Offset: 0x000695B9
	public static GameObject HoverObject
	{
		get
		{
			if (VRHMD.isVR)
			{
				return VRTrackedController.StaticHoverObject;
			}
			return HoverScript._HoverObject;
		}
		private set
		{
			HoverScript._HoverObject = value;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0006B3C1 File Offset: 0x000695C1
	// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x0006B3D5 File Offset: 0x000695D5
	public static GameObject HoverLockObject
	{
		get
		{
			if (VRHMD.isVR)
			{
				return VRTrackedController.StaticHoverLockObject;
			}
			return HoverScript._HoverLockObject;
		}
		private set
		{
			HoverScript._HoverLockObject = value;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0006B3DD File Offset: 0x000695DD
	// (set) Token: 0x06000FC8 RID: 4040 RVA: 0x0006B3E4 File Offset: 0x000695E4
	public static GameObject HoverGizmoObject { get; private set; }

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0006B3EC File Offset: 0x000695EC
	// (set) Token: 0x06000FCA RID: 4042 RVA: 0x0006B3F3 File Offset: 0x000695F3
	public static GameObject TooltipObject { get; private set; }

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06000FCB RID: 4043 RVA: 0x0006B3FB File Offset: 0x000695FB
	// (set) Token: 0x06000FCC RID: 4044 RVA: 0x0006B402 File Offset: 0x00069602
	public static GameObject PrevTooltipObject { get; private set; }

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06000FCD RID: 4045 RVA: 0x0006B40A File Offset: 0x0006960A
	// (set) Token: 0x06000FCE RID: 4046 RVA: 0x0006B411 File Offset: 0x00069611
	public static GameObject PrevHoverObject { get; private set; }

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06000FCF RID: 4047 RVA: 0x0006B419 File Offset: 0x00069619
	// (set) Token: 0x06000FD0 RID: 4048 RVA: 0x0006B42D File Offset: 0x0006962D
	public static GameObject SurfaceObject
	{
		get
		{
			if (VRHMD.isVR)
			{
				return VRTrackedController.StaticSurfaceObject;
			}
			return HoverScript._SurfaceObject;
		}
		private set
		{
			HoverScript._SurfaceObject = value;
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0006B435 File Offset: 0x00069635
	// (set) Token: 0x06000FD2 RID: 4050 RVA: 0x0006B43C File Offset: 0x0006963C
	public static GameObject HoverLockLock { get; private set; }

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0006B444 File Offset: 0x00069644
	// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x0006B44B File Offset: 0x0006964B
	public static GameObject HoverToolObject { get; private set; }

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0006B453 File Offset: 0x00069653
	// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x0006B467 File Offset: 0x00069667
	public static Vector3 PointerPosition
	{
		get
		{
			if (VRHMD.isVR)
			{
				return VRTrackedController.HoverPosition;
			}
			return HoverScript._PointerPosition;
		}
		private set
		{
			HoverScript._PointerPosition = value;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x0006B46F File Offset: 0x0006966F
	// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x0006B476 File Offset: 0x00069676
	public static Vector3 PrevPointerPosition { get; private set; }

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0006B480 File Offset: 0x00069680
	public static Vector3 PointerPositionDelta
	{
		get
		{
			return new Vector3(HoverScript.PointerPosition.x - HoverScript.PrevPointerPosition.x, HoverScript.PointerPosition.y - HoverScript.PrevPointerPosition.y, HoverScript.PointerPosition.z - HoverScript.PrevPointerPosition.z);
		}
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0006B4D4 File Offset: 0x000696D4
	public static Vector3 PointerPositionDirection
	{
		get
		{
			return HoverScript.PointerPositionDelta.normalized;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06000FDB RID: 4059 RVA: 0x0006B4EE File Offset: 0x000696EE
	// (set) Token: 0x06000FDC RID: 4060 RVA: 0x0006B4F5 File Offset: 0x000696F5
	public static RaycastHit FirstHit { get; private set; }

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0006B4FD File Offset: 0x000696FD
	// (set) Token: 0x06000FDE RID: 4062 RVA: 0x0006B504 File Offset: 0x00069704
	public static RaycastHit ObjectHit { get; private set; }

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06000FDF RID: 4063 RVA: 0x0006B50C File Offset: 0x0006970C
	// (set) Token: 0x06000FE0 RID: 4064 RVA: 0x0006B513 File Offset: 0x00069713
	public static RaycastHit ObjectLockHit { get; private set; }

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x0006B51B File Offset: 0x0006971B
	// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x0006B522 File Offset: 0x00069722
	public static Vector3 PointerPixel { get; private set; }

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0006B52A File Offset: 0x0006972A
	// (set) Token: 0x06000FE4 RID: 4068 RVA: 0x0006B531 File Offset: 0x00069731
	public static Vector3 PrevPointerPixel { get; private set; }

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0006B539 File Offset: 0x00069739
	public static bool IsMouseMoving
	{
		get
		{
			return Input.mousePosition.x != HoverScript.PrevPointerPixel.x || Input.mousePosition.y != HoverScript.PrevPointerPixel.y;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0006B56C File Offset: 0x0006976C
	// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0006B573 File Offset: 0x00069773
	public static string currentToolTipText { get; private set; }

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0006B57B File Offset: 0x0006977B
	// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0006B582 File Offset: 0x00069782
	public static string prevCurrentToolTipText { get; private set; }

	// Token: 0x06000FEA RID: 4074 RVA: 0x0006B58C File Offset: 0x0006978C
	public static Vector3 InputScreenPosition(int TouchIndex = 0)
	{
		if (TouchManager.Instance.NumberOfTouches == 0)
		{
			Vector3 vector = Display.RelativeMouseAt(Input.mousePosition);
			if (vector == Vector3.zero)
			{
				return Input.mousePosition;
			}
			return vector;
		}
		else
		{
			if (TouchManager.Instance.NumberOfTouches - 1 >= TouchIndex)
			{
				return TouchManager.Instance.ActiveTouches[TouchIndex].Position;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0006B5F4 File Offset: 0x000697F4
	// (set) Token: 0x06000FEC RID: 4076 RVA: 0x0006B5FB File Offset: 0x000697FB
	public static float DISTANCE_CHECK { get; private set; }

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06000FED RID: 4077 RVA: 0x0006B603 File Offset: 0x00069803
	// (set) Token: 0x06000FEE RID: 4078 RVA: 0x0006B60A File Offset: 0x0006980A
	public static Vector3 CameraPosition { get; private set; }

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0006B612 File Offset: 0x00069812
	// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0006B619 File Offset: 0x00069819
	public static float CameraDistance { get; private set; }

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0006B621 File Offset: 0x00069821
	// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x0006B628 File Offset: 0x00069828
	public static Camera PrevHoverCamera { get; private set; }

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0006B630 File Offset: 0x00069830
	// (set) Token: 0x06000FF4 RID: 4084 RVA: 0x0006B637 File Offset: 0x00069837
	public static Camera HoverCamera { get; private set; }

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0006B640 File Offset: 0x00069840
	private void Awake()
	{
		HoverScript.ZoomCameraCamera = this.ZoomCamera.GetComponent<Camera>();
		HoverScript.MainCamera = Camera.main;
		HoverScript.raycastHitComparator = new RaycastHitComparator();
		HoverScript.raySpherecastHitComparator = new RaySpherecastHitComparator();
		HoverScript.GrabbableLayerMask = Layers.Mask(new int[]
		{
			10,
			9,
			18,
			17
		});
		HoverScript.NonHeldLayerMask = Layers.Mask(new int[]
		{
			10
		});
		HoverScript.HeldLayerMask = Layers.Mask(new int[]
		{
			9
		});
		NetworkEvents.OnDisconnectedFromServer += this.NetworkEvents_OnDisconnectedFromServer;
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0006B6D1 File Offset: 0x000698D1
	private void OnDestroy()
	{
		NetworkEvents.OnDisconnectedFromServer -= this.NetworkEvents_OnDisconnectedFromServer;
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0006B6E4 File Offset: 0x000698E4
	private void NetworkEvents_OnDisconnectedFromServer(DisconnectInfo info)
	{
		this.ZoomCamera.SetActive(false);
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0006B6F4 File Offset: 0x000698F4
	private void Update()
	{
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			return;
		}
		if (VRHMD.isVR)
		{
			HoverScript.DISTANCE_CHECK = 0.3f;
			return;
		}
		HoverScript.PrevHoverCamera = HoverScript.HoverCamera;
		HoverScript.HoverCamera = HoverScript.GetHoverCamera(HoverScript.InputScreenPosition(0));
		HoverScript.PrevHoverObject = HoverScript.HoverObject;
		HoverScript.PrevTooltipObject = HoverScript.TooltipObject;
		HoverScript.PrevPointerPosition = HoverScript.PointerPosition;
		HoverScript.PrevPointerPixel = HoverScript.PointerPixel;
		HoverScript.CameraPosition = HoverScript.MainCamera.transform.position;
		HoverScript.CameraDistance = Singleton<CameraController>.Instance.distance;
		HoverScript.DISTANCE_CHECK = Vector3.Distance(HoverScript.CameraPosition, HoverScript.PointerPosition) / 200f;
		UICamera.CalculateHoveredUIObject();
		this.overUIObject = UICamera.HoverOverUI();
		HoverScript.HoverObject = null;
		HoverScript.TooltipObject = null;
		HoverScript.HoverLockObject = null;
		HoverScript.HoverLockLock = null;
		HoverScript.SurfaceObject = null;
		HoverScript.HoverGizmoObject = null;
		HoverScript.HoverToolObject = null;
		HoverScript.bAltZooming = false;
		HoverScript.PointerPosition = Vector3.zero;
		HoverScript.FirstHit = default(RaycastHit);
		HoverScript.ObjectHit = default(RaycastHit);
		HoverScript.ObjectLockHit = default(RaycastHit);
		this.CurrentPointerMode = (PlayerScript.PointerScript ? PlayerScript.PointerScript.CurrentPointerMode : PointerMode.Grab);
		this.newHoverIcon = HoverIcons.None;
		HoverScript.currentToolTipText = "";
		this.bHighlight = false;
		if (zInput.GetButton("Alt", ControlType.All) && this.overUIObject && UICamera.HoveredUIObject.GetComponent<UIGridMenuButton>() && UICamera.HoveredUIObject.GetComponent<UIGridMenuButton>().SpawnedGameObject)
		{
			HoverScript.bAltZooming = true;
			GameObject spawnedGameObject = UICamera.HoveredUIObject.GetComponent<UIGridMenuButton>().SpawnedGameObject;
			Singleton<AltZoomCamera>.Instance.ZoomObject = spawnedGameObject;
		}
		Ray pointerRay = HoverScript.GetPointerRay();
		int rayCastCount;
		this.CheckHits(HoverScript.RaySphereCast(pointerRay, out rayCastCount), rayCastCount);
		LayoutZone layoutZone;
		int num;
		if (this.CurrentPointerMode == PointerMode.Grab && PlayerScript.PointerScript && PlayerScript.PointerScript.FirstGrabbedNPO != null && LayoutZone.TryNPOInLayoutZone(PlayerScript.PointerScript.FirstGrabbedNPO, out layoutZone, out num, LayoutZone.PotentialZoneCheck.Auto))
		{
			this.newHoverIcon = HoverIcons.Layout;
			HoverScript.currentToolTipText = layoutZone.NPO.Name;
		}
		else if (this.overUIObject && Layers.IsUI3D(UICamera.HoveredUIObject))
		{
			if (UICamera.HoveredUIObject.GetComponent<UIInput>() || UICamera.HoveredUIObject.GetComponent<InputField>())
			{
				this.newHoverIcon = HoverIcons.Input;
			}
			else if (UICamera.HoveredUIObject.GetComponent<UIButton>() || UICamera.HoveredUIObject.GetComponent<Graphic>())
			{
				this.newHoverIcon = HoverIcons.Button;
			}
		}
		UIHoverIcon.icon = this.newHoverIcon;
		HoverScript.PointerPixel = Input.mousePosition;
		if (HoverScript.prevCurrentToolTipText != HoverScript.currentToolTipText && (!this.overUIObject || !UICamera.HoveredUIObject.GetComponent<UITooltipScript>()))
		{
			UIHoverText.text = HoverScript.currentToolTipText;
			HoverScript.prevCurrentToolTipText = HoverScript.currentToolTipText;
		}
		if ((zInput.GetButton("Rotate Right", ControlType.All) || zInput.GetButton("Rotate Left", ControlType.All)) && !UICamera.SelectIsInput() && Time.time > this.LastRotateTime + 0.1f && Input.GetAxis("Mouse Wheel") == 0f)
		{
			this.LastRotateTime = Time.time;
			if (zInput.GetButton("Rotate Left", ControlType.All) && HoverScript.bAltZooming)
			{
				HoverScript.AltZoomRotate = (HoverScript.AltZoomRotate - (float)(PlayerScript.PointerScript ? PlayerScript.PointerScript.RotationSnap : 15)) % 360f;
			}
			if (zInput.GetButton("Rotate Right", ControlType.All) && HoverScript.bAltZooming)
			{
				HoverScript.AltZoomRotate = (HoverScript.AltZoomRotate + (float)(PlayerScript.PointerScript ? PlayerScript.PointerScript.RotationSnap : 15)) % 360f;
			}
		}
		if (zInput.GetButtonDown("Grab", ControlType.All) && !PlayerScript.Pointer && !this.overUIObject && HoverScript.HoverLockObject)
		{
			Chat.LogError("Grey (Spectator) cannot interact. Click your name in the top right -> Change Color, then click a colored circle.", true);
		}
		if (PlayerScript.Pointer && TouchManager.Instance.NumberOfTouches > 0)
		{
			PlayerScript.PointerScript.pointerSyncs.SetTouchPosition(0, HoverScript.PointerPosition, TouchManager.Instance.ActiveTouches[0].Id);
			if (!ProMouse.Instance.MouseBusy && Application.platform != RuntimePlatform.WindowsEditor)
			{
				ProMouse.Instance.SetCursorPosition((int)TouchManager.Instance.ActiveTouches[0].Position.x, (int)TouchManager.Instance.ActiveTouches[0].Position.y);
			}
			for (int i = 1; i < TouchManager.Instance.NumberOfTouches; i++)
			{
				TouchPoint touchPoint = TouchManager.Instance.ActiveTouches[i];
				Vector3 worldPositionFromScreenPos = HoverScript.GetWorldPositionFromScreenPos(touchPoint.Position);
				PlayerScript.PointerScript.pointerSyncs.SetTouchPosition(i, worldPositionFromScreenPos, touchPoint.Id);
			}
		}
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0006BBD4 File Offset: 0x00069DD4
	public static Ray GetPointerRay()
	{
		Camera camera = HoverScript.GetHoverCamera();
		Vector3 vector = HoverScript.InputScreenPosition(0);
		if (vector.z != 0f)
		{
			camera = Singleton<CameraManager>.Instance.SpectatorCamera;
		}
		if (camera == Singleton<CameraManager>.Instance.SpectatorCamera && !Singleton<SpectatorCamera>.Instance.DisplayingFullscreen)
		{
			return camera.ScreenPointToRay(vector - Singleton<CameraManager>.Instance.SpectatorViewCamera.Offset);
		}
		return camera.ScreenPointToRay(vector);
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0006BC47 File Offset: 0x00069E47
	public static Camera GetHoverCamera()
	{
		return HoverScript.HoverCamera;
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0006BC50 File Offset: 0x00069E50
	public static Camera GetHoverCamera(Vector3 ScreenPos)
	{
		Camera result = HoverScript.MainCamera;
		ViewCamera viewCamera = ViewCamera.GetViewCamera(ScreenPos);
		if (viewCamera)
		{
			result = viewCamera.camera;
		}
		return result;
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0006BC80 File Offset: 0x00069E80
	private void HoverHighlightTool(GameObject checkObject, PointerMode targetMode)
	{
		if (this.CurrentPointerMode == targetMode && checkObject.CompareTag(Pointer.PointerModeToTag(targetMode)) && !HoverScript.HoverToolObject)
		{
			Highlighter component = checkObject.GetComponent<Highlighter>();
			if (component)
			{
				this.bHighlight = true;
				if (!PlayerScript.PointerScript.bHideHoverHighlight && !zInput.GetButton("Grab", ControlType.All))
				{
					component.On(PlayerScript.PointerScript.HoverColor);
				}
			}
			HoverScript.HoverToolObject = checkObject;
		}
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0006BCF8 File Offset: 0x00069EF8
	private void CheckHits(RaycastHit[] raycastHits, int rayCastCount)
	{
		bool flag = this.CurrentPointerMode == PointerMode.Line && LineScript.MEASURE_OBJECTS;
		for (int i = 0; i < rayCastCount; i++)
		{
			RaycastHit raycastHit = raycastHits[i];
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider);
			NetworkPhysicsObject networkPhysicsObject = null;
			NetworkPhysicsObject networkPhysicsObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject);
			if (gameObject.layer != 18)
			{
				networkPhysicsObject = networkPhysicsObject2;
			}
			if (HoverScript.FirstHit.collider == null)
			{
				HoverScript.FirstHit = raycastHit;
			}
			if (!this.overUIObject && !this.bHighlight && PlayerScript.PointerScript && raycastHit.barycentricCoordinate.y == 1f)
			{
				foreach (PointerMode targetMode in Pointer.ZoneTools)
				{
					this.HoverHighlightTool(gameObject, targetMode);
				}
				if (HoverScript.HoverLockObject == null)
				{
					this.HoverHighlightTool(raycastHit.collider.gameObject, PointerMode.Decal);
				}
			}
			if (networkPhysicsObject && networkPhysicsObject.IsGrabbable && !networkPhysicsObject.IsInvisible)
			{
				if ((!zInput.GetButton("Grab", ControlType.All) || flag || Pointer.IsCombineTool(this.CurrentPointerMode) || this.CurrentPointerMode == PointerMode.Snap || this.CurrentPointerMode == PointerMode.SnapRotate) && !this.bHighlight && networkPhysicsObject.HeldByPlayerID != NetworkID.ID && (!networkPhysicsObject.IsLocked || Pointer.IsCombineTool(this.CurrentPointerMode)) && (this.CurrentPointerMode == PointerMode.Grab || this.CurrentPointerMode == PointerMode.Flick || this.CurrentPointerMode == PointerMode.Move || this.CurrentPointerMode == PointerMode.Scale || this.CurrentPointerMode == PointerMode.VolumeScale || this.CurrentPointerMode == PointerMode.Rotate || this.CurrentPointerMode == PointerMode.Snap || this.CurrentPointerMode == PointerMode.SnapRotate || Pointer.IsCombineTool(this.CurrentPointerMode) || flag))
				{
					if (!HoverScript.HoverObject)
					{
						HoverScript.HoverObject = gameObject;
						HoverScript.HoverObjectDistance = raycastHit.distance;
						UICamera.CalculateHoveredUIObject();
						this.overUIObject = UICamera.HoverOverUI();
					}
					if (!this.overUIObject && PlayerScript.PointerScript && !PlayerScript.PointerScript.bHideHoverHighlight)
					{
						Highlighter highlighter = networkPhysicsObject.highlighter;
						if (highlighter)
						{
							this.bHighlight = true;
							if (!gameObject.CompareTag("Tablet") || !gameObject.GetComponent<TabletScript>() || !gameObject.GetComponent<TabletScript>().CheckHit())
							{
								highlighter.On(PlayerScript.PointerScript.HoverColor);
							}
							if (gameObject.CompareTag("Card") && networkPhysicsObject.cardScript)
							{
								foreach (GameObject gameObject2 in networkPhysicsObject.cardScript.CardsAttachedToThis())
								{
									gameObject2.GetComponent<Highlighter>().On(PlayerScript.PointerScript.HoverColor);
								}
							}
						}
					}
				}
				if (!this.overUIObject && ((AltZoomCamera.AltZoomAlwaysOn && !AltZoomCamera.AltZoomFollowsPointer) || zInput.GetButton("Alt", ControlType.All) || TouchHandlerScript.bAltZoom) && !zInput.GetButton("Grab", ControlType.All) && !HoverScript.bAltZooming && !gameObject.CompareTag("Board") && !networkPhysicsObject.IsInvisible && !NetworkSingleton<PlayerManager>.Instance.IsBlinded())
				{
					HoverScript.bAltZooming = true;
					GameObject zoomObject = networkPhysicsObject.gameObject;
					if (networkPhysicsObject.stackObject && networkPhysicsObject.stackObject.IsInfiniteStack && networkPhysicsObject.stackObject.InfiniteObject && !networkPhysicsObject.customObject)
					{
						zoomObject = networkPhysicsObject.stackObject.InfiniteObject;
					}
					Singleton<AltZoomCamera>.Instance.ZoomObject = zoomObject;
				}
			}
			if (HoverScript.PointerPosition == Vector3.zero)
			{
				if (raycastHit.collider.gameObject.CompareTag("Surface"))
				{
					HoverScript.PointerPosition = raycastHit.point;
					HoverScript.SurfaceObject = gameObject;
				}
				if (networkPhysicsObject && networkPhysicsObject.IsLocked)
				{
					HoverScript.PointerPosition = raycastHit.point;
					HoverScript.SurfaceObject = gameObject;
				}
			}
			if (networkPhysicsObject && networkPhysicsObject.IsGrabbable && !networkPhysicsObject.CachedIsInvisible)
			{
				if (!HoverScript.HoverLockObject && (this.CurrentPointerMode == PointerMode.Grab || this.CurrentPointerMode == PointerMode.Flick || Pointer.IsCombineTool(this.CurrentPointerMode) || this.CurrentPointerMode == PointerMode.Move || this.CurrentPointerMode == PointerMode.Scale || this.CurrentPointerMode == PointerMode.VolumeScale || this.CurrentPointerMode == PointerMode.Rotate || this.CurrentPointerMode == PointerMode.Decal))
				{
					HoverScript.HoverLockObject = gameObject;
					HoverScript.ObjectHit = raycastHit;
				}
				if (!HoverScript.HoverLockLock && networkPhysicsObject.IsLocked)
				{
					HoverScript.HoverLockLock = gameObject;
					HoverScript.ObjectLockHit = raycastHit;
				}
				if ((!HoverScript.bAltZooming || !AltZoomCamera.AltZoomFollowsPointer) && HoverScript.HoverLockObject == gameObject && HoverScript.currentToolTipText == "" && networkPhysicsObject.HeldByPlayerID != NetworkID.ID && (networkPhysicsObject.CanSeeName || networkPhysicsObject.CanBePeeked) && !this.overUIObject && !NetworkSingleton<PlayerManager>.Instance.IsBlinded())
				{
					GameObject prevTooltipObject = HoverScript.PrevTooltipObject;
					string currentToolTipText;
					HoverScript.ObjectToTooltip(networkPhysicsObject, ref prevTooltipObject, ref this.TooltipTimeHolder, out currentToolTipText, out this.newHoverIcon);
					HoverScript.currentToolTipText = currentToolTipText;
					HoverScript.TooltipObject = prevTooltipObject;
				}
			}
			if (!HoverScript.HoverGizmoObject)
			{
				if (networkPhysicsObject2)
				{
					HoverScript.HoverGizmoObject = networkPhysicsObject2.gameObject;
				}
				else if (raycastHit.collider.gameObject.CompareTag("Hand"))
				{
					HoverScript.HoverGizmoObject = raycastHit.collider.gameObject;
				}
				else if (raycastHit.collider.gameObject.CompareTag("3D Text"))
				{
					HoverScript.HoverGizmoObject = raycastHit.collider.gameObject.transform.parent.gameObject;
				}
			}
		}
		if (!HoverScript.bAltZooming)
		{
			Singleton<AltZoomCamera>.Instance.ZoomObject = null;
			if (!PlayerScript.Pointer)
			{
				Cursor.visible = true;
				return;
			}
		}
		else if (!PlayerScript.Pointer)
		{
			Cursor.visible = false;
		}
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0006C34C File Offset: 0x0006A54C
	public static RaycastHit[] RaySphereCast(Ray ray, out int count)
	{
		return HoverScript.RaySphereCast(ray, out count, HoverScript.DISTANCE_CHECK * 1.6f, 1000f, HoverScript.GrabbableLayerMask);
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x0006C36A File Offset: 0x0006A56A
	public static RaycastHit[] RaySphereCast(Ray ray, out int count, int layerMask)
	{
		return HoverScript.RaySphereCast(ray, out count, HoverScript.DISTANCE_CHECK * 1.6f, 1000f, layerMask);
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0006C384 File Offset: 0x0006A584
	public static RaycastHit[] RaySphereCast(Ray ray, out int count, float sphereRadius, float maxDistance, int layerMask)
	{
		int num = Physics.RaycastNonAlloc(ray, HoverScript.raycast_hits, maxDistance, layerMask);
		int num2 = Physics.SphereCastNonAlloc(ray, sphereRadius, HoverScript.spherecast_hits, maxDistance, layerMask);
		count = 0;
		for (int i = 0; i < num; i++)
		{
			HoverScript.rayspherecast_hits[count] = HoverScript.raycast_hits[i];
			HoverScript.rayspherecast_hits[count].barycentricCoordinate = Vector3.one;
			count++;
		}
		for (int j = 0; j < num2; j++)
		{
			bool flag = true;
			for (int k = 0; k < num; k++)
			{
				if (HoverScript.spherecast_hits[j].collider == HoverScript.rayspherecast_hits[k].collider)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				HoverScript.rayspherecast_hits[count] = HoverScript.spherecast_hits[j];
				HoverScript.rayspherecast_hits[count].barycentricCoordinate = Vector3.zero;
				count++;
			}
		}
		Array.Sort<RaycastHit>(HoverScript.rayspherecast_hits, 0, count, HoverScript.raySpherecastHitComparator);
		return HoverScript.rayspherecast_hits;
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x0006C48C File Offset: 0x0006A68C
	public static Vector3 GetWorldPositionFromScreenPos(Vector2 ScreenPos)
	{
		return HoverScript.GetWorldPositionFromScreenPos(new Vector3(ScreenPos.x, ScreenPos.y, 0f));
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x0006C4AC File Offset: 0x0006A6AC
	public static Vector3 GetWorldPositionFromScreenPos(Vector3 ScreenPos)
	{
		int num = Physics.RaycastNonAlloc(HoverScript.MainCamera.ScreenPointToRay(ScreenPos), HoverScript.raycast_hits, 1000f, HoverScript.GrabbableLayerMask);
		Array.Sort<RaycastHit>(HoverScript.raycast_hits, 0, num, HoverScript.raycastHitComparator);
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = HoverScript.raycast_hits[i];
			GameObject grabbable = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider);
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(grabbable);
			if (raycastHit.collider.gameObject.CompareTag("Surface") || (networkPhysicsObject && networkPhysicsObject.IsGrabbable && networkPhysicsObject.IsLocked))
			{
				return raycastHit.point;
			}
		}
		return Vector3.zero;
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x0006C564 File Offset: 0x0006A764
	public static void ObjectToTooltip(NetworkPhysicsObject npoTarget, ref GameObject prevGO, ref float tooltipTimeHolder, out string currentToolTipText, out HoverIcons newHoverIcon)
	{
		newHoverIcon = HoverIcons.None;
		HoverScript.toolTipBuilder.Length = 0;
		if (!npoTarget.ShowTooltip)
		{
			currentToolTipText = "";
			return;
		}
		GameObject gameObject = npoTarget.gameObject;
		if (gameObject.CompareTag("Dice"))
		{
			newHoverIcon = HoverIcons.Dice;
		}
		if (npoTarget.customAssetbundle && (npoTarget.customAssetbundle.assetBundleEffects.LoopingEffects != null || npoTarget.customAssetbundle.assetBundleEffects.TriggerEffects != null))
		{
			newHoverIcon = HoverIcons.Effects;
		}
		bool flag = npoTarget.ValueFlags != 0;
		int num = 0;
		int num2 = 0;
		if (npoTarget.CanSeeName && PlayerScript.Pointer && (flag || npoTarget.hasTags))
		{
			List<GameObject> selectedObjects = PlayerScript.PointerScript.GetSelectedObjects(-1, false, false);
			if (selectedObjects.Contains(gameObject))
			{
				foreach (GameObject gameObject2 in selectedObjects)
				{
					if (!(gameObject2 == gameObject))
					{
						NetworkPhysicsObject component = gameObject2.GetComponent<NetworkPhysicsObject>();
						if (component && component.CanSeeName && (flag ? ((component.ValueFlags & npoTarget.ValueFlags) != 0) : npoTarget.TagsAllowActingUpon(component)))
						{
							num += ((component.stackObject && !component.stackObject.IsInfiniteStack && !component.stackObject.bBag) ? (component.stackObject.num_objects_ * component.Value) : component.Value);
							num2++;
						}
					}
				}
			}
		}
		if (num != 0 || (npoTarget.CanSeeName && (npoTarget.Value != 0 || flag)))
		{
			bool flag2 = npoTarget.stackObject && !npoTarget.stackObject.IsInfiniteStack && !npoTarget.stackObject.bBag;
			int num3 = flag2 ? (npoTarget.stackObject.num_objects_ * npoTarget.Value) : npoTarget.Value;
			HoverScript.toolTipBuilder.Append(num3);
			if (flag2)
			{
				newHoverIcon = HoverIcons.Chips;
			}
			if (num != 0)
			{
				HoverScript.toolTipBuilder.Append(" + ");
				if (num2 > 1)
				{
					HoverScript.toolTipBuilder.Append("(");
				}
				HoverScript.toolTipBuilder.Append(num);
				if (num2 > 1)
				{
					HoverScript.toolTipBuilder.Append(")");
				}
				HoverScript.toolTipBuilder.Append(" = ");
				HoverScript.toolTipBuilder.Append(num3 + num);
			}
		}
		else if (npoTarget.HasRotationsValues())
		{
			string tag = gameObject.tag;
			RotationValue rotationValue = npoTarget.GetRotationValue(false);
			if (!rotationValue.value.StartsWith("#"))
			{
				HoverScript.toolTipBuilder.Append(rotationValue.value);
				if (rotationValue.floatValue != null && PlayerScript.Pointer)
				{
					List<GameObject> selectedObjects2 = PlayerScript.PointerScript.GetSelectedObjects(-1, false, false);
					if (selectedObjects2.Contains(gameObject))
					{
						float value = rotationValue.floatValue.Value;
						float num4 = rotationValue.floatValue.Value;
						bool flag3 = false;
						for (int i = 0; i < selectedObjects2.Count; i++)
						{
							GameObject gameObject3 = selectedObjects2[i];
							if (gameObject3 && gameObject3 != gameObject && gameObject3.CompareTag(tag))
							{
								NetworkPhysicsObject component2 = gameObject3.GetComponent<NetworkPhysicsObject>();
								if (component2 && component2.HasRotationsValues())
								{
									RotationValue rotationValue2 = component2.GetRotationValue(false);
									if (rotationValue2.floatValue != null)
									{
										num4 += rotationValue2.floatValue.Value;
										flag3 = true;
										HoverScript.toolTipBuilder.Append(" + ");
										HoverScript.toolTipBuilder.Append(rotationValue2.value);
									}
								}
							}
						}
						if (flag3)
						{
							HoverScript.toolTipBuilder.Append(" = ");
							HoverScript.toolTipBuilder.Append(num4);
						}
					}
				}
			}
		}
		else if (gameObject.CompareTag("Deck") && npoTarget.deckScript)
		{
			newHoverIcon = HoverIcons.Deck;
			HoverScript.toolTipBuilder.Append(npoTarget.deckScript.num_cards_);
		}
		else if (gameObject.CompareTag("Chip") && npoTarget.GetComponent<MeshFilter>() && npoTarget.GetComponent<MeshFilter>().sharedMesh.name.Contains("_chip"))
		{
			string name = npoTarget.GetComponent<MeshFilter>().sharedMesh.name;
			int num5 = 0;
			int num6 = 0;
			if (name.StartsWith("1000"))
			{
				num6 += 1000;
			}
			else if (name.StartsWith("500"))
			{
				num6 += 500;
			}
			else if (name.StartsWith("100"))
			{
				num6 += 100;
			}
			else if (name.StartsWith("50"))
			{
				num6 += 50;
			}
			else if (name.StartsWith("10"))
			{
				num6 += 10;
			}
			HoverScript.toolTipBuilder.Append("$");
			HoverScript.toolTipBuilder.Append(num6);
			bool flag4 = false;
			if (npoTarget.stackObject)
			{
				newHoverIcon = HoverIcons.Chips;
				flag4 = true;
				num6 *= npoTarget.stackObject.num_objects_;
				HoverScript.toolTipBuilder.Append("*");
				HoverScript.toolTipBuilder.Append(npoTarget.stackObject.num_objects_);
			}
			num5 += num6;
			if (PlayerScript.Pointer)
			{
				List<GameObject> selectedObjects3 = PlayerScript.PointerScript.GetSelectedObjects(-1, false, false);
				if (selectedObjects3.Contains(gameObject))
				{
					for (int j = 0; j < selectedObjects3.Count; j++)
					{
						GameObject gameObject4 = selectedObjects3[j];
						if (gameObject4 && gameObject4 != gameObject && gameObject4.CompareTag("Chip") && gameObject4.GetComponent<MeshFilter>() && gameObject4.GetComponent<MeshFilter>().sharedMesh.name.Contains("_chip"))
						{
							flag4 = true;
							num6 = 0;
							name = gameObject4.GetComponent<MeshFilter>().sharedMesh.name;
							if (name.StartsWith("1000"))
							{
								num6 += 1000;
							}
							else if (name.StartsWith("500"))
							{
								num6 += 500;
							}
							else if (name.StartsWith("100"))
							{
								num6 += 100;
							}
							else if (name.StartsWith("50"))
							{
								num6 += 50;
							}
							else if (name.StartsWith("10"))
							{
								num6 += 10;
							}
							HoverScript.toolTipBuilder.Append(" + $");
							HoverScript.toolTipBuilder.Append(num6);
							if (gameObject4.GetComponent<StackObject>())
							{
								num6 *= gameObject4.GetComponent<StackObject>().num_objects_;
								HoverScript.toolTipBuilder.Append("*");
								HoverScript.toolTipBuilder.Append(gameObject4.GetComponent<StackObject>().num_objects_);
							}
							num5 += num6;
						}
					}
				}
			}
			if (flag4)
			{
				HoverScript.toolTipBuilder.Append(" = $");
				HoverScript.toolTipBuilder.Append(num5);
			}
		}
		else if (npoTarget.stackObject)
		{
			if (!npoTarget.stackObject.IsInfiniteStack)
			{
				if (!npoTarget.stackObject.bBag)
				{
					newHoverIcon = HoverIcons.Chips;
				}
				else
				{
					newHoverIcon = HoverIcons.Bag;
				}
				HoverScript.toolTipBuilder.Append(npoTarget.stackObject.num_objects_);
			}
			else
			{
				newHoverIcon = HoverIcons.Infinite;
			}
		}
		else if (gameObject.CompareTag("Clock") && npoTarget.clockScript && npoTarget.clockScript.Label)
		{
			newHoverIcon = HoverIcons.Clock;
			HoverScript.toolTipBuilder.Append(npoTarget.clockScript.Label.text);
		}
		else if (gameObject.CompareTag("Counter") && npoTarget.counterScript)
		{
			HoverScript.toolTipBuilder.Append(npoTarget.counterScript.GetValue());
		}
		else if (gameObject.CompareTag("Calculator") && npoTarget.calculatorScript)
		{
			HoverScript.toolTipBuilder.Append(npoTarget.calculatorScript.screen.text);
		}
		else if (npoTarget.mp3PlayerScript)
		{
			string text = npoTarget.mp3PlayerScript.PlayingSongScript.songName;
			text = text.Replace("MP3_", "");
			text = text.Replace("_", " ");
			HoverScript.toolTipBuilder.Append(text);
		}
		else if (npoTarget.rpgFigurines)
		{
			newHoverIcon = HoverIcons.RPG;
		}
		else if (gameObject.CompareTag("Coin"))
		{
			newHoverIcon = HoverIcons.Dice;
		}
		if (npoTarget.CanSeeName)
		{
			if (!string.IsNullOrEmpty(npoTarget.Name))
			{
				if (HoverScript.toolTipBuilder.Length > 0)
				{
					HoverScript.toolTipBuilder.Append(" ");
				}
				string name2 = npoTarget.Name;
				TextCode.LocalizeUIText(ref name2);
				HoverScript.toolTipBuilder.Append(name2);
			}
			if (!string.IsNullOrEmpty(npoTarget.Description) && gameObject == prevGO)
			{
				if (Time.time > tooltipTimeHolder + UIHoverText.NPODelayTooltipTime && tooltipTimeHolder != 0f)
				{
					if (HoverScript.toolTipBuilder.Length > 0)
					{
						HoverScript.toolTipBuilder.Append("\n----------\n");
					}
					string description = npoTarget.Description;
					TextCode.LocalizeUIText(ref description);
					HoverScript.toolTipBuilder.Append(description);
				}
				else if (tooltipTimeHolder == 0f)
				{
					tooltipTimeHolder = Time.time;
				}
			}
			else
			{
				tooltipTimeHolder = 0f;
			}
			prevGO = gameObject;
			if (npoTarget.GetSelectedStateId() != -1)
			{
				newHoverIcon = HoverIcons.States;
				if (HoverScript.toolTipBuilder.Length > 0)
				{
					HoverScript.toolTipBuilder.Insert(0, string.Concat(new string[]
					{
						"(",
						npoTarget.GetSelectedStateId().ToString(),
						"/",
						npoTarget.GetStatesCount().ToString(),
						") "
					}));
				}
				else
				{
					HoverScript.toolTipBuilder.Append(string.Concat(new string[]
					{
						"(",
						npoTarget.GetSelectedStateId().ToString(),
						"/",
						npoTarget.GetStatesCount().ToString(),
						")"
					}));
				}
			}
		}
		currentToolTipText = HoverScript.toolTipBuilder.ToString();
	}

	// Token: 0x040009C0 RID: 2496
	private static GameObject _HoverObject;

	// Token: 0x040009C1 RID: 2497
	public static float HoverObjectDistance;

	// Token: 0x040009C2 RID: 2498
	private static GameObject _HoverLockObject;

	// Token: 0x040009C3 RID: 2499
	public static GameObject GrabbedStackObject;

	// Token: 0x040009C8 RID: 2504
	private static GameObject _SurfaceObject;

	// Token: 0x040009CB RID: 2507
	private static Vector3 _PointerPosition;

	// Token: 0x040009D5 RID: 2517
	public const float TIME_THRESHOLD = 0.3f;

	// Token: 0x040009D8 RID: 2520
	private PointerMode CurrentPointerMode;

	// Token: 0x040009D9 RID: 2521
	public GameObject ZoomCamera;

	// Token: 0x040009DA RID: 2522
	public static Camera ZoomCameraCamera;

	// Token: 0x040009DB RID: 2523
	public static Camera MainCamera;

	// Token: 0x040009DC RID: 2524
	private float LastRotateTime;

	// Token: 0x040009DD RID: 2525
	public static float AltZoomRotate = 180f;

	// Token: 0x040009DE RID: 2526
	public static int GrabbableLayerMask;

	// Token: 0x040009DF RID: 2527
	public static int NonHeldLayerMask;

	// Token: 0x040009E0 RID: 2528
	public static int HeldLayerMask;

	// Token: 0x040009E1 RID: 2529
	public static bool bAltZooming = false;

	// Token: 0x040009E2 RID: 2530
	private static RaycastHitComparator raycastHitComparator;

	// Token: 0x040009E3 RID: 2531
	private static RaySpherecastHitComparator raySpherecastHitComparator;

	// Token: 0x040009E4 RID: 2532
	private float TooltipTimeHolder;

	// Token: 0x040009E5 RID: 2533
	private HoverIcons newHoverIcon;

	// Token: 0x040009E6 RID: 2534
	public static RaycastHit[] raycast_hits = new RaycastHit[20];

	// Token: 0x040009E7 RID: 2535
	public static RaycastHit[] spherecast_hits = new RaycastHit[20];

	// Token: 0x040009E8 RID: 2536
	public static RaycastHit[] rayspherecast_hits = new RaycastHit[40];

	// Token: 0x040009E9 RID: 2537
	private bool bHighlight;

	// Token: 0x040009EA RID: 2538
	private bool overUIObject;

	// Token: 0x040009ED RID: 2541
	private static StringBuilder toolTipBuilder = new StringBuilder();
}
