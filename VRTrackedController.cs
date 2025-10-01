using System;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using NewNet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000379 RID: 889
public class VRTrackedController : MonoBehaviour
{
	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06002997 RID: 10647 RVA: 0x00123953 File Offset: 0x00121B53
	// (set) Token: 0x06002998 RID: 10648 RVA: 0x0012395B File Offset: 0x00121B5B
	public TrackedControllerMode currentMode { get; private set; }

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06002999 RID: 10649 RVA: 0x00123964 File Offset: 0x00121B64
	// (set) Token: 0x0600299A RID: 10650 RVA: 0x0012396B File Offset: 0x00121B6B
	public static TrackedControllerStyle ControllerStyle
	{
		get
		{
			return VRTrackedController._controllerStyle;
		}
		set
		{
			VRTrackedController._controllerStyle = value;
			VRTrackedController.controllerStyleChanged();
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x0600299B RID: 10651 RVA: 0x00123978 File Offset: 0x00121B78
	// (set) Token: 0x0600299C RID: 10652 RVA: 0x0012397F File Offset: 0x00121B7F
	public static float LASER_ANGLE
	{
		get
		{
			return VRTrackedController._LASER_ANGLE;
		}
		set
		{
			VRTrackedController._LASER_ANGLE = value;
			VRTrackedController.controllerStyleChanged();
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x0600299D RID: 10653 RVA: 0x0012398C File Offset: 0x00121B8C
	public int Index
	{
		get
		{
			return (int)this.trackedObject.index;
		}
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x0012399C File Offset: 0x00121B9C
	private static void controllerStyleChanged()
	{
		if (VRTrackedController._controllerStyle == TrackedControllerStyle.Old)
		{
			Quaternion identity = Quaternion.identity;
			VRTrackedController.GrabSphereOriginXRotation = VRTrackedController.LASER_ANGLE_ORIGINAL;
			identity.eulerAngles = new Vector3(VRTrackedController.GrabSphereOriginXRotation, 0f, 0f);
			if (VRTrackedController.leftVRTrackedController)
			{
				if (VRTrackedController.LASER_ANGLE == 0f)
				{
					VRTrackedController.leftVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.leftVRTrackedController.GrabSpherePositionStraight;
				}
				else
				{
					VRTrackedController.leftVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.leftVRTrackedController.GrabSpherePositionAngled;
				}
				VRLaserPointer vrlaserPointer = VRTrackedController.leftVRTrackedController.laserPointer;
				vrlaserPointer.turnedOn = true;
				vrlaserPointer.LaserTransform.rotation = VRTrackedController.leftVRTrackedController.transform.rotation * identity;
				vrlaserPointer.LaserTransform.localPosition = VRTrackedController.leftVRTrackedController.GrabSphere.transform.localPosition;
			}
			if (VRTrackedController.rightVRTrackedController)
			{
				if (VRTrackedController.LASER_ANGLE == 0f)
				{
					VRTrackedController.rightVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.rightVRTrackedController.GrabSpherePositionStraight;
				}
				else
				{
					VRTrackedController.rightVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.rightVRTrackedController.GrabSpherePositionAngled;
				}
				VRLaserPointer vrlaserPointer2 = VRTrackedController.rightVRTrackedController.laserPointer;
				vrlaserPointer2.turnedOn = true;
				vrlaserPointer2.LaserTransform.rotation = VRTrackedController.rightVRTrackedController.transform.rotation * identity;
				vrlaserPointer2.LaserTransform.localPosition = VRTrackedController.rightVRTrackedController.GrabSphere.transform.localPosition;
			}
		}
		else
		{
			VRTrackedController.GrabSphereOriginXRotation = VRTrackedController.LASER_ANGLE;
			Quaternion identity2 = Quaternion.identity;
			identity2.eulerAngles = new Vector3(VRTrackedController.GrabSphereOriginXRotation, 0f, 0f);
			if (VRTrackedController.leftVRTrackedController)
			{
				if (VRTrackedController.LASER_ANGLE == 0f)
				{
					VRTrackedController.leftVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.leftVRTrackedController.GrabSpherePositionStraight;
				}
				else
				{
					VRTrackedController.leftVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.leftVRTrackedController.GrabSpherePositionAngled;
				}
				VRLaserPointer vrlaserPointer3 = VRTrackedController.leftVRTrackedController.laserPointer;
				vrlaserPointer3.turnedOn = false;
				vrlaserPointer3.LaserTransform.rotation = VRTrackedController.leftVRTrackedController.transform.rotation * identity2;
				vrlaserPointer3.LaserTransform.localPosition = VRTrackedController.leftVRTrackedController.GrabSphere.transform.localPosition;
			}
			if (VRTrackedController.rightVRTrackedController)
			{
				if (VRTrackedController.LASER_ANGLE == 0f)
				{
					VRTrackedController.rightVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.rightVRTrackedController.GrabSpherePositionStraight;
				}
				else
				{
					VRTrackedController.rightVRTrackedController.GrabSphere.transform.localPosition = VRTrackedController.rightVRTrackedController.GrabSpherePositionAngled;
				}
				VRLaserPointer vrlaserPointer4 = VRTrackedController.rightVRTrackedController.laserPointer;
				vrlaserPointer4.turnedOn = false;
				vrlaserPointer4.LaserTransform.rotation = VRTrackedController.rightVRTrackedController.transform.rotation * identity2;
				vrlaserPointer4.LaserTransform.localPosition = VRTrackedController.rightVRTrackedController.GrabSphere.transform.localPosition;
			}
		}
		VRTrackedController.RefreshTouchpadIcons();
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x00123CB4 File Offset: 0x00121EB4
	private void Awake()
	{
		if (this.controller == null)
		{
			this.controller = base.GetComponent<VRSteamControllerDevice>();
		}
		if (this.trackedObject == null)
		{
			this.trackedObject = base.GetComponent<SteamVR_TrackedObject>();
		}
		if (this.laserPointer == null)
		{
			this.laserPointer = base.GetComponent<VRLaserPointer>();
		}
		if (this.handZone == null)
		{
			this.handZone = new HandZone();
		}
		this.HMD = Singleton<VRHMD>.Instance.trackedObject;
		this.GrabSphereMaterial = this.GrabSphere.GetComponent<Renderer>().material;
		this.laserPointer.LaserTransform.position = this.GrabSphere.transform.position;
		this.Tip = base.transform.Find("Grab/Tip");
		if (this.Tip == null)
		{
			this.Tip = this.GrabSphere.transform;
		}
		this.SelectionTrigger = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("SelectionTrigger"), Vector3.zero, Quaternion.identity);
		this.SelectionTrigger.SetActive(false);
		this.SelectionMaterial = this.SelectionTrigger.GetComponentInChildren<Renderer>().material;
		this.ZoomObject = new GameObject();
		this.ZoomObject.name = "Zoom Object";
		this.ZoomObject.transform.parent = base.transform;
		this.ZoomObject.transform.Reset();
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.New)
		{
			this.ZoomObject.transform.localScale *= 0.75f;
		}
		this.ZoomObject.SetActive(false);
		Material material = new Material(Shader.Find("Unlit/Color"));
		material.SetColor("_Color", Colour.Purple);
		this.TeleportLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.TeleportLine.layer = base.gameObject.layer;
		this.TeleportLine.transform.parent = base.transform.parent;
		this.TeleportLine.transform.localScale = new Vector3(0.01f, 100f, 0.01f);
		this.TeleportLine.GetComponent<MeshRenderer>().material = material;
		UnityEngine.Object.Destroy(this.TeleportLine.GetComponent<Collider>());
		this.TeleportLine.SetActive(false);
		this.sphereCollider = base.GetComponent<SphereCollider>();
		this.sphereCollider.enabled = false;
		this.renderModel = this.Model.GetComponent<VRRenderModel>();
		VRTrackedController.HasAnalogStick = this.renderModel.hasAnalogStick;
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.Old)
		{
			Singleton<VRVirtualHand>.Instance.SetVirtualHandAttachment(VRControllerAttachment.Detached);
		}
		if (NetworkSingleton<NetworkUI>.Instance.VRControllerManager.left == base.gameObject)
		{
			this.otherController = NetworkSingleton<NetworkUI>.Instance.VRControllerManager.right.GetComponent<VRTrackedController>();
			VRTrackedController.leftVRTrackedController = this;
			this.touchpadDownMode = VRTrackedController.TouchpadDownMode.Zoom;
			return;
		}
		this.otherController = NetworkSingleton<NetworkUI>.Instance.VRControllerManager.left.GetComponent<VRTrackedController>();
		VRTrackedController.rightVRTrackedController = this;
		this.touchpadDownMode = VRTrackedController.TouchpadDownMode.Zoom;
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x00123FC8 File Offset: 0x001221C8
	private void OldAwake()
	{
		if (this.trackedObject == null)
		{
			this.trackedObject = base.GetComponent<SteamVR_TrackedObject>();
		}
		if (this.laserPointer == null)
		{
			this.laserPointer = base.GetComponent<VRLaserPointer>();
		}
		this.HMD = Singleton<VRHMD>.Instance.trackedObject;
		this.GrabSphereMaterial = this.GrabSphere.GetComponent<Renderer>().material;
		this.SelectionTrigger = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("SelectionTrigger"), Vector3.zero, Quaternion.identity);
		this.SelectionTrigger.SetActive(false);
		this.SelectionMaterial = this.SelectionTrigger.GetComponentInChildren<Renderer>().material;
		this.ZoomObject = new GameObject();
		this.ZoomObject.name = "Zoom Object";
		this.ZoomObject.transform.parent = base.transform;
		this.ZoomObject.transform.Reset();
		this.ZoomObject.SetActive(false);
		Material material = new Material(Shader.Find("Unlit/Color"));
		material.SetColor("_Color", Colour.Purple);
		this.TeleportLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.TeleportLine.layer = base.gameObject.layer;
		this.TeleportLine.transform.parent = base.transform.parent;
		this.TeleportLine.transform.localScale = new Vector3(0.01f, 100f, 0.01f);
		this.TeleportLine.GetComponent<MeshRenderer>().material = material;
		UnityEngine.Object.Destroy(this.TeleportLine.GetComponent<Collider>());
		this.TeleportLine.SetActive(false);
		this.sphereCollider = base.GetComponent<SphereCollider>();
		this.sphereCollider.enabled = false;
		if (NetworkSingleton<NetworkUI>.Instance.VRControllerManager.left == base.gameObject)
		{
			this.otherController = NetworkSingleton<NetworkUI>.Instance.VRControllerManager.right.GetComponent<VRTrackedController>();
		}
		else
		{
			this.otherController = NetworkSingleton<NetworkUI>.Instance.VRControllerManager.left.GetComponent<VRTrackedController>();
		}
		this.renderModel = this.Model.GetComponent<VRRenderModel>();
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x001241F0 File Offset: 0x001223F0
	private void Start()
	{
		this.uiCamera = UICamera.mainCamera.GetComponent<UICamera>();
		this.uiCamera.useTouch = true;
		EventManager.OnDummyObjectFinish += this.EventManager_OnDummyObjectFinish;
		EventManager.OnPlayerChangeColor += this.OnPlayerChangeColor;
		EventManager.OnChangePointerMode += this.OnChangePointerMode;
		NetworkEvents.OnServerInitialized += this.OnServerStart;
		NetworkEvents.OnConnectedToServer += this.OnServerStart;
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x00124270 File Offset: 0x00122470
	private void OnDestroy()
	{
		EventManager.OnDummyObjectFinish -= this.EventManager_OnDummyObjectFinish;
		EventManager.OnPlayerChangeColor -= this.OnPlayerChangeColor;
		EventManager.OnChangePointerMode -= this.OnChangePointerMode;
		NetworkEvents.OnServerInitialized -= this.OnServerStart;
		NetworkEvents.OnConnectedToServer -= this.OnServerStart;
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x060029A3 RID: 10659 RVA: 0x001242D2 File Offset: 0x001224D2
	// (set) Token: 0x060029A4 RID: 10660 RVA: 0x001242DA File Offset: 0x001224DA
	public bool initialized { get; private set; }

	// Token: 0x060029A5 RID: 10661 RVA: 0x001242E4 File Offset: 0x001224E4
	private void Init()
	{
		this.initialized = true;
		if (this.renderModel.modelType == VRRenderModel.RenderModelType.vr_controller_vive_1_5)
		{
			this.CurrentTool = base.transform.Find("UI/Vive/CurrentTool").GetComponent<VRTouchpadIcon>();
			this.TouchpadUp = base.transform.Find("UI/Vive/TouchPad/Up").GetComponent<VRTouchpadIcon>();
			this.TouchpadDown = base.transform.Find("UI/Vive/TouchPad/Down").GetComponent<VRTouchpadIcon>();
			this.TouchpadLeft = base.transform.Find("UI/Vive/TouchPad/Left").GetComponent<VRTouchpadIcon>();
			this.TouchpadRight = base.transform.Find("UI/Vive/TouchPad/Right").GetComponent<VRTouchpadIcon>();
			this.TouchpadCenter = base.transform.Find("UI/Vive/TouchPad/Center").GetComponent<VRTouchpadIcon>();
			this.UIHoverText = base.transform.Find("UI/Vive/UIHoverText").GetComponent<TextMesh>();
			this.GrabSpherePositionStraight = this.GrabSphere.transform.localPosition;
			this.GrabSpherePositionAngled = this.GrabSphere.transform.localPosition;
			VRTrackedController.LASER_ANGLE_ORIGINAL = 58f;
			this.Tooltips = new GameObject[this.ViveTooltips.Length];
			for (int i = 0; i < this.Tooltips.Length; i++)
			{
				this.Tooltips[i] = this.ViveTooltips[i];
				this.Tooltips[i].GetComponent<Renderer>().material = new Material(this.Tooltips[i].GetComponent<Renderer>().material);
			}
			this.shouldDisplayClickTooltip = true;
		}
		else if (this.renderModel.modelType == VRRenderModel.RenderModelType.valve_controller_knu_left || this.renderModel.modelType == VRRenderModel.RenderModelType.valve_controller_knu_right)
		{
			this.IsIndexKnucklesController = true;
			this.CurrentTool = base.transform.Find("UI/Oculus/CurrentTool").GetComponent<VRTouchpadIcon>();
			this.TouchpadIconContainer = base.transform.Find("UI/Oculus/TouchPad").gameObject;
			this.TouchpadUp = base.transform.Find("UI/Oculus/TouchPad/Up").GetComponent<VRTouchpadIcon>();
			this.TouchpadDown = base.transform.Find("UI/Oculus/TouchPad/Down").GetComponent<VRTouchpadIcon>();
			this.TouchpadLeft = base.transform.Find("UI/Oculus/TouchPad/Left").GetComponent<VRTouchpadIcon>();
			this.TouchpadRight = base.transform.Find("UI/Oculus/TouchPad/Right").GetComponent<VRTouchpadIcon>();
			this.TouchpadCenter = base.transform.Find("UI/Oculus/TouchPad/Center").GetComponent<VRTouchpadIcon>();
			this.UIHoverText = base.transform.Find("UI/Oculus/UIHoverText").GetComponent<TextMesh>();
			this.GrabSpherePositionStraight = base.transform.Find("RiftGrabPosition").localPosition;
			this.GrabSpherePositionAngled = base.transform.Find("RiftGrabPositionAngled").localPosition;
			VRTrackedController.LASER_ANGLE_ORIGINAL = 23f;
			VRTrackedController.ViveHaptics = false;
			VRTrackedController.HasAnalogStick = true;
			this.Tooltips = new GameObject[this.OculusTooltips.Length];
			for (int j = 0; j < this.Tooltips.Length; j++)
			{
				this.Tooltips[j] = this.OculusTooltips[j];
				this.Tooltips[j].GetComponent<Renderer>().material = new Material(this.Tooltips[j].GetComponent<Renderer>().material);
			}
			if (this.renderModel.modelType == VRRenderModel.RenderModelType.valve_controller_knu_right)
			{
				Transform parent = this.CurrentTool.transform.parent;
				parent.localPosition = new Vector3(0.0112f, 0.019f, -0.0005f);
				parent.Rotate(-15f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsRight/Menu").Translate(-0.5f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsRight/TriggerText").GetComponent<TextMesh>().text = "Grab";
				Transform transform = base.transform.Find("UI/Oculus/TooltipsRight/Grip");
				Vector3 localScale = transform.localScale;
				localScale.y = -localScale.y;
				transform.localScale = localScale;
				transform.Translate(2.8f, -2.5f, -1f);
				transform.Rotate(0f, 0f, 90f);
				Transform transform2 = base.transform.Find("UI/Oculus/TooltipsRight/GripText");
				transform2.GetComponent<TextMesh>().text = "Laser + Click";
				transform2.position = transform.position;
				transform2.Translate(-2f, -3f, 0f);
				base.transform.Find("UI/Oculus/TooltipsRight/Middle").Translate(0f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsRight/MenuTapText").Translate(-0.6f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsRight/MenuHoldText").Translate(-0.6f, 0f, 0f);
			}
			else
			{
				Transform parent2 = this.CurrentTool.transform.parent;
				parent2.localPosition = new Vector3(-0.0112f, 0.019f, -0.0005f);
				parent2.Rotate(-15f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsLeft/Menu").Translate(0.4f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsLeft/Lower").transform.Translate(0.7f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsLeft/TriggerText").transform.Translate(0.7f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsLeft/TriggerText").GetComponent<TextMesh>().text = "Grab";
				Transform transform3 = base.transform.Find("UI/Oculus/TooltipsLeft/Grip");
				transform3.Translate(-5.4f, -3f, -1f);
				transform3.Rotate(0f, 0f, 90f);
				transform3.Translate(0f, -2.5f, 0f);
				Transform transform4 = base.transform.Find("UI/Oculus/TooltipsLeft/GripText");
				transform4.GetComponent<TextMesh>().text = "Laser + Click";
				transform4.position = transform3.position;
				transform4.Translate(-1.5f, -3f, 0f);
				Transform transform5 = base.transform.Find("UI/Oculus/TooltipsLeft/Middle");
				transform5.Translate(-2.5f, 0f, 0f);
				Vector3 localScale2 = transform5.localScale;
				localScale2.x = -localScale2.x;
				transform5.localScale = localScale2;
				base.transform.Find("UI/Oculus/TooltipsLeft/MoveText").Translate(-5f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsLeft/RotateText").Translate(-7.6f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsLeft/MenuTapText").Translate(0.4f, 0f, 0f);
				base.transform.Find("UI/Oculus/TooltipsLeft/MenuHoldText").Translate(0.4f, 0f, 0f);
			}
		}
		else
		{
			this.CurrentTool = base.transform.Find("UI/Oculus/CurrentTool").GetComponent<VRTouchpadIcon>();
			this.TouchpadIconContainer = base.transform.Find("UI/Oculus/TouchPad").gameObject;
			this.TouchpadUp = base.transform.Find("UI/Oculus/TouchPad/Up").GetComponent<VRTouchpadIcon>();
			this.TouchpadDown = base.transform.Find("UI/Oculus/TouchPad/Down").GetComponent<VRTouchpadIcon>();
			this.TouchpadLeft = base.transform.Find("UI/Oculus/TouchPad/Left").GetComponent<VRTouchpadIcon>();
			this.TouchpadRight = base.transform.Find("UI/Oculus/TouchPad/Right").GetComponent<VRTouchpadIcon>();
			this.TouchpadCenter = base.transform.Find("UI/Oculus/TouchPad/Center").GetComponent<VRTouchpadIcon>();
			this.UIHoverText = base.transform.Find("UI/Oculus/UIHoverText").GetComponent<TextMesh>();
			this.GrabSpherePositionStraight = base.transform.Find("RiftGrabPosition").localPosition;
			this.GrabSpherePositionAngled = base.transform.Find("RiftGrabPositionAngled").localPosition;
			VRTrackedController.LASER_ANGLE_ORIGINAL = 40f;
			VRTrackedController.ViveHaptics = false;
			VRTrackedController.HasAnalogStick = true;
			this.Tooltips = new GameObject[this.OculusTooltips.Length];
			for (int k = 0; k < this.Tooltips.Length; k++)
			{
				this.Tooltips[k] = this.OculusTooltips[k];
				this.Tooltips[k].GetComponent<Renderer>().material = new Material(this.Tooltips[k].GetComponent<Renderer>().material);
			}
		}
		this.CurrentTool.SetFlarePosition(new Vector3(0f, 0f, 0f));
		this.CurrentTool.FlareDuration = 0f;
		VRTrackedController.UpdateToolIconColor();
		Singleton<SystemConsole>.Instance.RecheckTouchpadBindings();
		if (this.renderModel.controllerType == VRControllerType.oculus_touch || this.renderModel.controllerType == VRControllerType.index_knuckles)
		{
			VRTrackedController.LASER_ANGLE = VRTrackedController.LASER_ANGLE_ORIGINAL;
			VRTrackedController.ALWAYS_ANGLE_LASER = true;
		}
		VRTrackedController.controllerStyleChanged();
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.New)
		{
			this.DisplayTooltips(true, true);
			if (!VRTrackedController.ALWAYS_DISPLAY_TOOLTIPS)
			{
				this.startedDisplayingtTooltipsAt = Time.time;
			}
		}
		if (!this.BindingsInitialized)
		{
			this.BindingsInitialized = true;
			this.BindPadHotkeys(false);
		}
		this.UpdateHoverText(false);
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x00124C24 File Offset: 0x00122E24
	public void BindPadHotkeys(bool clean = false)
	{
		string[] array = new string[]
		{
			"grab",
			"flick",
			"line"
		};
		string str;
		if (this == VRTrackedController.leftVRTrackedController)
		{
			str = "Left";
			this.ToolBindingsAttached = VRTrackedController.LEFT_CONTROLLER_HOTKEYS;
		}
		else
		{
			if (!(this == VRTrackedController.rightVRTrackedController))
			{
				return;
			}
			str = "Right";
			this.ToolBindingsAttached = VRTrackedController.RIGHT_CONTROLLER_HOTKEYS;
		}
		string[] array2 = new string[]
		{
			"West",
			"East",
			"North"
		};
		int num = 0;
		foreach (string str2 in array2)
		{
			VRTrackedController.VRKeyCode vrkeyCode = (VRTrackedController.VRKeyCode)Enum.Parse(typeof(VRTrackedController.VRKeyCode), "VR" + str + "Pad" + str2);
			if (this.ToolBindingsAttached)
			{
				if (VRTrackedController.HasAnalogStick)
				{
					Singleton<SystemConsole>.Instance.VRKeyLongPressBinds[vrkeyCode] = "exec tool_revert -d; bind +" + vrkeyCode + " tool_current <tool_current>";
					Singleton<SystemConsole>.Instance.VRKeyPressBinds[vrkeyCode] = "tool_current " + array[num++];
				}
				else
				{
					Singleton<SystemConsole>.Instance.VRKeyLongPressBinds[vrkeyCode] = "bind -" + vrkeyCode + " tool_current <<tool_current>>";
					Singleton<SystemConsole>.Instance.VRKeyReleaseBinds[vrkeyCode] = "tool_current " + array[num++];
				}
			}
			else if (clean)
			{
				Singleton<SystemConsole>.Instance.VRKeyLongPressBinds.Remove(vrkeyCode);
				Singleton<SystemConsole>.Instance.VRKeyPressBinds.Remove(vrkeyCode);
				Singleton<SystemConsole>.Instance.VRKeyReleaseBinds.Remove(vrkeyCode);
			}
		}
		Singleton<SystemConsole>.Instance.RecheckTouchpadBindings();
		this.UpdateTouchpadIcons(VRTrackedController.VRKeyCode.None);
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x00124DED File Offset: 0x00122FED
	private void OnPlayerChangeColor(PlayerState playerState)
	{
		if (playerState.id == NetworkID.ID)
		{
			this.handZone.TriggerLabel = playerState.stringColor;
		}
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x00124E0D File Offset: 0x0012300D
	private void OnChangePointerMode(PointerMode mode)
	{
		if (this.CurrentTool && this.IconMode != mode)
		{
			this.CurrentTool.SetIcon(mode);
			this.CurrentTool.FakeTouch(0.1f);
			this.IconMode = mode;
		}
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x00124E48 File Offset: 0x00123048
	private void Update()
	{
		if (this.controller == null)
		{
			return;
		}
		if (!this.renderModel.loaded)
		{
			return;
		}
		if (!this.initialized && this.renderModel.loaded)
		{
			this.Init();
		}
		this.Velocity = (this.WorldPosition() - this.LastPosition) / Time.deltaTime;
		this.LastPosition = this.WorldPosition();
		this.LastRotation = base.transform.rotation;
		float num = Mathf.Abs(this.Velocity.x);
		float num2 = Mathf.Abs(this.Velocity.y);
		float num3 = Mathf.Abs(this.Velocity.z);
		if (num > num2 && num > num3)
		{
			this.GrabSphereRotation += 0.1f * this.Velocity.x;
		}
		else if (num2 > num3)
		{
			this.GrabSphereRotation += 0.1f * this.Velocity.y;
		}
		else
		{
			this.GrabSphereRotation += 0.1f * this.Velocity.z;
		}
		while (this.GrabSphereRotation > 360f)
		{
			this.GrabSphereRotation -= 360f;
		}
		while (this.GrabSphereRotation < 0f)
		{
			this.GrabSphereRotation += 360f;
		}
		this.GrabSphereTranslationRotation.eulerAngles = new Vector3(VRTrackedController.GrabSphereOriginXRotation, 0f, this.GrabSphereRotation);
		this.GrabSphere.transform.rotation = this.LastRotation * this.GrabSphereTranslationRotation;
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.Old)
		{
			this.OldUpdate();
			return;
		}
		this.NewUpdate();
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x00125000 File Offset: 0x00123200
	private void OldUpdate()
	{
		if (this.displayingTooltips)
		{
			this.DisplayTooltips(false, true);
			this.displayTooltipsOnCodebaseChange = true;
		}
		this.currentPointerMode = ((PlayerScript.Pointer != null) ? PlayerScript.PointerScript.CurrentPointerMode : PointerMode.None);
		Color color = (PlayerScript.Pointer != null) ? PlayerScript.PointerScript.HoverColor : Color.white;
		Highlighter highlighter = null;
		bool pressTrigger = this.controller.GetPressTrigger();
		this.prevHoverObject = this.HoverObject;
		this.prevHoverNonNetwork = this.HoverNonNetwork;
		this.HoverObject = null;
		this.HoverUIObject = null;
		this.HoverLockObject = null;
		this.HoverNonNetwork = null;
		this.LaserHover = false;
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		int num3 = Physics.OverlapSphereNonAlloc(base.transform.position, this.sphereCollider.radius * base.transform.lossyScale.x, this.grabbabaleColliders);
		for (int i = 0; i < num3; i++)
		{
			Collider collider = this.grabbabaleColliders[i];
			if (!this.HoverNonNetwork && Singleton<VRHMD>.Instance.floatingUI && collider.gameObject.CompareTag("VR UI"))
			{
				this.HoverNonNetwork = collider.gameObject;
			}
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(collider);
			if (networkPhysicsObject && networkPhysicsObject.IsGrabbable)
			{
				float num4 = Vector3.Distance(base.transform.position, networkPhysicsObject.rigidbody.position);
				if (!networkPhysicsObject.IsLocked && num4 < num)
				{
					num = num4;
					this.HoverObject = networkPhysicsObject;
				}
				else if (networkPhysicsObject.IsLocked && num4 < num2)
				{
					num2 = num4;
					this.HoverLockObject = networkPhysicsObject;
				}
			}
		}
		if (num2 > num)
		{
			this.HoverLockObject = this.HoverObject;
		}
		if ((this.currentMode == TrackedControllerMode.Grabbed && !this.MovePositionAtLaser) || this.currentMode == TrackedControllerMode.GrabbedNonNetwork)
		{
			this.SetGrabSphereColor(Colour.Blue);
		}
		else if ((this.HoverObject && this.currentMode != TrackedControllerMode.Grabbed) || (this.HoverNonNetwork && this.currentMode != TrackedControllerMode.GrabbedNonNetwork))
		{
			this.SetGrabSphereColor(Colour.Green);
		}
		else
		{
			this.SetGrabSphereColor(Colour.Red);
		}
		if (!this.HoverObject && (this.laserPointer.HitNetwork != null || this.laserPointer.HitObject != null))
		{
			GameObject gameObject = this.laserPointer.HitObject.gameObject;
			NetworkPhysicsObject hitNetwork = this.laserPointer.HitNetwork;
			if (hitNetwork && hitNetwork.IsGrabbable && !hitNetwork.IsLocked)
			{
				this.HoverObject = hitNetwork;
			}
			else if (gameObject.CompareTag("VR UI"))
			{
				this.HoverUIObject = gameObject;
			}
			if (this.HoverLockObject == null)
			{
				this.HoverLockObject = this.laserPointer.HitLockNetwork;
				this.LaserHover = true;
			}
		}
		NetworkPhysicsObject hoverLockObject = this.HoverLockObject;
		bool flag = this.currentPointerMode == PointerMode.Grab || this.currentPointerMode == PointerMode.Flick || Pointer.IsCombineTool(this.currentPointerMode) || this.currentPointerMode == PointerMode.Move || this.currentPointerMode == PointerMode.Scale || this.currentPointerMode == PointerMode.VolumeScale || this.currentPointerMode == PointerMode.Rotate || this.currentPointerMode == PointerMode.Snap || this.currentPointerMode == PointerMode.SnapRotate;
		bool flag2 = this.currentPointerMode == PointerMode.Grab || this.currentPointerMode == PointerMode.Flick || Pointer.IsCombineTool(this.currentPointerMode);
		if (!flag || this.laserPointer.UI3DHitObject != null)
		{
			this.HoverObject = null;
			this.HoverLockObject = null;
		}
		string text = Pointer.PointerModeToTag(this.currentPointerMode);
		if (text != "" && this.laserPointer.HitTrigger && this.laserPointer.HitTrigger.CompareTag(text))
		{
			this.HoverTrigger = this.laserPointer.HitTrigger;
			highlighter = this.laserPointer.HitTrigger.GetComponent<Highlighter>();
		}
		if (this.HoverObject && this.HoverObject.highlighter && (!pressTrigger || Pointer.IsCombineTool(this.currentPointerMode)))
		{
			highlighter = this.HoverObject.highlighter;
		}
		if (this.HoverNonNetwork)
		{
			highlighter = this.HoverNonNetwork.GetComponent<Highlighter>();
		}
		if (highlighter)
		{
			highlighter.On(color);
		}
		string text2 = "";
		if (this.HoverLockObject && this.currentMode == TrackedControllerMode.Default)
		{
			HoverIcons hoverIcons;
			HoverScript.ObjectToTooltip(this.HoverLockObject, ref this.PrevTooltipObject, ref this.TooltipTimeHolder, out text2, out hoverIcons);
			Vector3 position = this.HoverLockObject.transform.position;
			Vector3 vector;
			Bounds boundsNotNormalized = this.HoverLockObject.GetBoundsNotNormalized(out vector);
			position.y += boundsNotNormalized.extents.y + 0.5f - vector.y;
			this.TextTooltip.transform.position = position;
			Vector3 worldPosition = new Vector3(Singleton<VRHMD>.Instance.Pivot.transform.position.x, this.HoverLockObject.transform.position.y, Singleton<VRHMD>.Instance.Pivot.transform.position.z);
			this.TextTooltip.transform.LookAt(worldPosition);
			this.TextTooltip.transform.Rotate(new Vector3(45f, 180f, 0f));
		}
		else
		{
			this.PrevTooltipObject = null;
		}
		bool flag3 = text2 != "";
		if (this.TextTooltip.gameObject.activeSelf != flag3)
		{
			this.TextTooltip.gameObject.SetActive(flag3);
		}
		this.TextTooltip.text = text2;
		if (this.controller.GetPressTriggerDown())
		{
			this.controller.TriggerHapticPulse(1f);
			this.ResetUIHover();
			this.ToggleZoomObject(null);
			TrackedControllerMode currentMode = this.currentMode;
			if (currentMode != TrackedControllerMode.Default)
			{
				if (currentMode == TrackedControllerMode.Teleport)
				{
					this.currentMode = TrackedControllerMode.Default;
				}
			}
			else if (this.HoverNonNetwork)
			{
				if (!this.HoverNonNetwork.transform.parent.GetComponent<VRTrackedController>())
				{
					this.GrabbedNonNetwork = this.HoverNonNetwork;
					this.GrabbedNonNetworkParent = this.GrabbedNonNetwork.transform.parent;
					this.GrabbedNonNetwork.transform.parent = base.transform;
					this.currentMode = TrackedControllerMode.GrabbedNonNetwork;
				}
			}
			else if (this.laserPointer.UI3DHitObject != null)
			{
				this.currentMode = TrackedControllerMode.UI3D;
				this.Click3DUI(this.laserPointer.UI3DHitObject);
			}
			else if (this.Grab(false))
			{
				this.currentMode = TrackedControllerMode.Grabbed;
			}
			else if (this.HoverUIObject)
			{
				this.currentMode = TrackedControllerMode.UI;
				this.TouchChanged(TouchPhase.Began, false);
			}
			else if (this.StartTool())
			{
				this.currentMode = TrackedControllerMode.Tool;
			}
			else if (PlayerScript.Pointer && flag2)
			{
				PlayerScript.PointerScript.ResetAllObjects();
				PlayerScript.PointerScript.ResetHighlight();
				this.currentMode = TrackedControllerMode.Selection;
				this.StartSelection();
			}
		}
		if (this.controller.GetPressTrigger())
		{
			TrackedControllerMode currentMode = this.currentMode;
			if (currentMode != TrackedControllerMode.Selection)
			{
				if (currentMode != TrackedControllerMode.UI)
				{
					if (currentMode == TrackedControllerMode.Tool)
					{
						this.UpdateTool();
					}
				}
				else
				{
					this.TouchChanged(TouchPhase.Moved, false);
				}
			}
			else
			{
				this.UpdateSelection();
			}
		}
		if (this.controller.GetPressTriggerUp())
		{
			this.controller.TriggerHapticPulse(0.4f);
			switch (this.currentMode)
			{
			case TrackedControllerMode.Grabbed:
				this.Release();
				break;
			case TrackedControllerMode.Selection:
				this.EndSelection();
				break;
			case TrackedControllerMode.UI:
				this.TouchChanged(TouchPhase.Ended, false);
				break;
			case TrackedControllerMode.Tool:
				this.EndTool();
				break;
			case TrackedControllerMode.GrabbedNonNetwork:
				this.GrabbedNonNetwork.transform.parent = this.GrabbedNonNetworkParent;
				this.GrabbedNonNetwork = null;
				break;
			}
			if (this.currentMode != TrackedControllerMode.Teleport && this.currentMode != TrackedControllerMode.Zoom)
			{
				this.currentMode = TrackedControllerMode.Default;
			}
			if (this.grabStackCoroutine != null)
			{
				base.StopCoroutine(this.grabStackCoroutine);
			}
		}
		if (this.HoverUIObject && this.currentMode == TrackedControllerMode.Default && this.otherController.currentMode != TrackedControllerMode.UI)
		{
			if (!VRTrackedController.currentUITracked || VRTrackedController.currentUITracked == this)
			{
				this.TouchChanged(TouchPhase.Moved, true);
				VRTrackedController.currentUITracked = this;
			}
		}
		else
		{
			this.ResetUIHover();
		}
		if (this.controller.GetPressGripDown())
		{
			this.ToggleZoomObject(null);
			TrackedControllerMode currentMode = this.currentMode;
			if (currentMode != TrackedControllerMode.Default)
			{
				if (currentMode == TrackedControllerMode.Grabbed)
				{
					this.controller.TriggerHapticPulse(0.5f);
					this.MovePositionAtLaser = !this.MovePositionAtLaser;
				}
			}
			else
			{
				this.ToggleZoomObject(hoverLockObject);
			}
		}
		if (this.controller.GetPressMenuDown())
		{
			if (this.HoverLockObject && PlayerScript.Pointer)
			{
				PlayerScript.PointerScript.StartContextual(this.HoverLockObject.gameObject, true);
			}
			else
			{
				NetworkSingleton<NetworkUI>.Instance.EscapeMenu(NetworkUI.EscapeMenuActivation.AUTO);
				if (PlayerScript.Pointer)
				{
					PlayerScript.PointerScript.ResetAllObjects();
				}
			}
		}
		this.controller.GetPressMenuUp();
		if (this.currentMode == TrackedControllerMode.Teleport)
		{
			this.laserPointer.SetColor(Colour.Purple);
		}
		else if (this.currentMode == TrackedControllerMode.Grabbed && (this.MovePositionAtLaser || this.HoverUIObject))
		{
			this.laserPointer.SetColor(Colour.Blue);
		}
		else if (this.HoverObject && this.currentMode != TrackedControllerMode.Grabbed && this.LaserHover)
		{
			this.laserPointer.SetColor(Colour.Green);
		}
		else
		{
			this.laserPointer.SetColor(Colour.Red);
		}
		this.TeleportLine.SetActive(this.currentMode == TrackedControllerMode.Teleport);
		if (this.TeleportLine.activeSelf)
		{
			this.TeleportLine.transform.position = this.laserPointer.HitPoint;
		}
		bool flag4 = this.controller.GetPressTouchpad();
		bool flag5 = this.controller.GetPressTouchpadDown();
		bool flag6 = this.controller.GetPressTouchpadUp();
		Vector2 touchpadAxis = this.controller.GetTouchpadAxis();
		bool flag7 = false;
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		float num5 = 0.6f;
		if (this.renderModel.hasAnalogStick)
		{
			flag4 = true;
			num5 = 0.75f;
		}
		if (flag4)
		{
			if (Mathf.Abs(touchpadAxis.y) > Mathf.Abs(touchpadAxis.x))
			{
				if (touchpadAxis.y > num5)
				{
					flag7 = true;
				}
				else if (touchpadAxis.y < -num5)
				{
					flag8 = true;
				}
			}
			else if (touchpadAxis.x > num5)
			{
				flag10 = true;
			}
			else if (touchpadAxis.x < -num5)
			{
				flag9 = true;
			}
			if (touchpadAxis.x < 1f - num5 && touchpadAxis.x > num5 - 1f && touchpadAxis.y < 1f - num5 && touchpadAxis.y > num5 - 1f)
			{
				flag11 = true;
			}
		}
		if (this.renderModel.hasAnalogStick)
		{
			bool flag12 = flag7 || flag8 || flag10 || flag9;
			if (this.analogPress)
			{
				if (!flag12)
				{
					flag6 = true;
					this.analogPress = false;
				}
			}
			else if (flag12)
			{
				flag5 = true;
				this.analogPress = true;
			}
		}
		if (flag6 && PlayerScript.Pointer)
		{
			PlayerScript.PointerScript.OverrideRaising(flag7);
		}
		if (flag11 && flag5 && this.currentMode == TrackedControllerMode.Default)
		{
			this.currentMode = TrackedControllerMode.Teleport;
			this.controller.TriggerHapticPulse(0.4f);
		}
		if (flag6 && this.currentMode == TrackedControllerMode.Teleport)
		{
			this.currentMode = TrackedControllerMode.Default;
			this.Teleport(this.laserPointer.HitPoint);
		}
		this.pointerColor = NetworkSingleton<NetworkUI>.Instance.playerColour;
		this.SetColor(this.pointerColor);
		if (this.currentMode == TrackedControllerMode.Zoom)
		{
			bool flag13 = Vector3.Dot((base.transform.position - this.HMD.transform.position).normalized, -base.transform.up) > 0.05f;
			if (!flag13)
			{
				if (PermissionsOptions.CheckAllow(PermissionsOptions.options.Peeking, -1))
				{
					if (this.currentZoomObject)
					{
						this.currentZoomObject.AddPeekIndicator();
					}
				}
				else
				{
					PermissionsOptions.BroadcastPermissionWarning("Peek");
				}
			}
			if (this.ZoomObject)
			{
				this.ZoomObject.SetActive(flag13 || PermissionsOptions.CheckAllow(PermissionsOptions.options.Peeking, -1));
			}
		}
		if (this.Model.activeSelf == this.ZoomObject.activeSelf)
		{
			this.Model.SetActive(!this.ZoomObject.activeSelf);
			this.GrabSphere.SetActive(this.Model.activeSelf);
			this.UI.SetActive(this.Model.activeSelf);
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		int num6 = PlayerScript.PointerScript.RotationSnap / 15;
		float intensity = 0.5f;
		if (flag8 && flag5)
		{
			switch (this.currentMode)
			{
			case TrackedControllerMode.Default:
				if (this.HoverObject)
				{
					this.controller.TriggerHapticPulse(intensity);
					PlayerScript.PointerScript.Rotation(this.HoverObject.ID, 0, 12, false);
				}
				break;
			case TrackedControllerMode.Grabbed:
				this.controller.TriggerHapticPulse(intensity);
				PlayerScript.PointerScript.ChangeHeldFlipRotationIndex(12, this.Index);
				break;
			case TrackedControllerMode.Zoom:
				this.controller.TriggerHapticPulse(intensity);
				this.ZoomObject.transform.localScale = this.ZoomObject.transform.localScale / 1.1f;
				break;
			case TrackedControllerMode.World:
				this.controller.TriggerHapticPulse(intensity);
				this.VRCameraRig.localScale = this.VRCameraRig.localScale * 1.1f;
				this.VRCameraRig.position = new Vector3(this.VRCameraRig.position.x, this.VRCameraRig.position.y * 1.1f, this.VRCameraRig.position.z);
				break;
			}
		}
		if (flag7 && flag5)
		{
			switch (this.currentMode)
			{
			case TrackedControllerMode.Default:
				if (this.HoverLockObject)
				{
					this.controller.TriggerHapticPulse(intensity);
					PlayerScript.PointerScript.Randomize(this.HoverLockObject.ID, PlayerScript.PointerScript.ID);
				}
				break;
			case TrackedControllerMode.Grabbed:
				if (PlayerScript.Pointer)
				{
					PlayerScript.PointerScript.OverrideRaising(true);
				}
				break;
			case TrackedControllerMode.Zoom:
				this.controller.TriggerHapticPulse(intensity);
				this.ZoomObject.transform.localScale = this.ZoomObject.transform.localScale * 1.1f;
				break;
			case TrackedControllerMode.World:
				this.controller.TriggerHapticPulse(intensity);
				this.VRCameraRig.localScale = this.VRCameraRig.localScale / 1.1f;
				this.VRCameraRig.position = new Vector3(this.VRCameraRig.position.x, this.VRCameraRig.position.y / 1.1f, this.VRCameraRig.position.z);
				break;
			}
		}
		if (flag10 && Time.time > this.RotateTimeHolder + 0.15f)
		{
			switch (this.currentMode)
			{
			case TrackedControllerMode.Default:
				if (this.HoverObject)
				{
					this.controller.TriggerHapticPulse(intensity);
					PlayerScript.PointerScript.Rotation(this.HoverObject.ID, num6, 0, false);
				}
				break;
			case TrackedControllerMode.Grabbed:
				this.controller.TriggerHapticPulse(intensity);
				PlayerScript.PointerScript.ChangeHeldSpinRotationIndex(num6, this.Index);
				break;
			case TrackedControllerMode.World:
				this.controller.TriggerHapticPulse(intensity);
				this.VRCameraRig.eulerAngles = new Vector3(0f, this.VRCameraRig.eulerAngles.y - 15f, 0f);
				break;
			}
			this.RotateTimeHolder = Time.time;
		}
		if (flag9 && Time.time > this.RotateTimeHolder + 0.15f)
		{
			switch (this.currentMode)
			{
			case TrackedControllerMode.Default:
				if (this.HoverObject)
				{
					this.controller.TriggerHapticPulse(intensity);
					PlayerScript.PointerScript.Rotation(this.HoverObject.ID, -num6, 0, false);
				}
				break;
			case TrackedControllerMode.Grabbed:
				this.controller.TriggerHapticPulse(intensity);
				PlayerScript.PointerScript.ChangeHeldSpinRotationIndex(-num6, this.Index);
				break;
			case TrackedControllerMode.World:
				this.controller.TriggerHapticPulse(intensity);
				this.VRCameraRig.eulerAngles = new Vector3(0f, this.VRCameraRig.eulerAngles.y + 15f, 0f);
				break;
			}
			this.RotateTimeHolder = Time.time;
		}
		this.HapticsUpdate();
		PlayerScript.PointerScript.pointerSyncs.SetTrackedTransform(base.gameObject.transform.position, base.gameObject.transform.rotation, Singleton<VRHMD>.Instance.VRCameraRig.localScale.x, TrackedType.Controller, this.trackedObject, this.MovePositionAtLaser && this.laserPointer.HitPoint != Vector3.zero, this.Index);
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x001261EC File Offset: 0x001243EC
	private void NewUpdate()
	{
		int id = NetworkID.ID;
		if (VRTrackedController.LastToolController == this)
		{
			VRTrackedController.ToolActionUp = false;
			VRTrackedController.ToolActionDown = false;
		}
		if (this.displayingTooltipsFadeStartTime != 0f)
		{
			bool flag = Time.time > this.displayingTooltipsFadeStartTime + 0.5f;
			float num;
			if (flag)
			{
				num = this.displayingTooltipsFadeToAlpha;
			}
			else
			{
				num = Mathf.Lerp(this.displayingTooltipsFadeFromAlpha, this.displayingTooltipsFadeToAlpha, (Time.time - this.displayingTooltipsFadeStartTime) / 0.5f);
			}
			for (int i = 0; i < this.Tooltips.Length; i++)
			{
				Material material = this.Tooltips[i].GetComponent<Renderer>().material;
				Color color = material.color;
				if (Network.peerType == NetworkPeerMode.Disconnected)
				{
					if (i == 0 || i == 8 || i == 9)
					{
						color.a = 0f;
					}
					else if (i == 3 || i == 13)
					{
						if (VRTrackedController.DISPLAY_CLICK_TOOLTIP_ON_MENU && this.shouldDisplayClickTooltip)
						{
							color.a = 1f;
						}
						else
						{
							color.a = num;
						}
					}
					else
					{
						color.a = num;
					}
				}
				else
				{
					color.a = num;
				}
				material.color = color;
				if (flag && color.a == 0f)
				{
					this.Tooltips[i].SetActive(false);
				}
			}
			if (flag)
			{
				this.displayingTooltipsFadeStartTime = 0f;
				if (num == 0f)
				{
					this.displayingTooltips = false;
				}
			}
		}
		if (!this.isPushing)
		{
			if (this.wantsToMove)
			{
				this.isPushing = true;
				this.startPushPosition = this.GrabSphere.transform.position;
			}
			else if (this.pushVelocity.magnitude > 0.01f)
			{
				this.VRCameraRig.transform.position -= this.pushVelocity * Time.deltaTime;
				this.pushVelocity *= VRTrackedController.THROW_FRICTION;
			}
		}
		else if (this.wantsToMove)
		{
			if (this.otherController.wantsToMove)
			{
				this.otherController.wantsToMove = false;
				this.otherController.isPushing = false;
				this.wantsToMove = false;
				this.isPushing = false;
			}
			else
			{
				this.VRCameraRig.transform.position -= this.GrabSphere.transform.position - this.startPushPosition;
			}
		}
		else
		{
			this.isPushing = false;
			if (VRTrackedController.THROW_MOVE)
			{
				this.pushVelocity = Singleton<VRHMD>.Instance.VRCameraRig.rotation * this.controller.GetTrackedObjectVelocity() * (Singleton<VRHMD>.Instance.transform.localScale.x / 38f);
				this.pushVelocity /= Time.deltaTime;
			}
		}
		this.DoOrientHeldObject = false;
		if (VRTrackedController.STICKY_GRAB)
		{
			this.GrabStarted = false;
			this.GrabReleased = false;
			if (this.WaitForGrabRelease)
			{
				if (!this.controller.IsGrabbing())
				{
					this.WaitForGrabRelease = false;
					this.GrabReleased = true;
					this.Grabbing = false;
				}
			}
			else if (this.currentMode == TrackedControllerMode.Grabbed || this.currentMode == TrackedControllerMode.GrabbedNonNetwork)
			{
				if (this.controller.StartedGrab())
				{
					this.WaitForGrabRelease = true;
				}
			}
			else
			{
				this.Grabbing = this.controller.IsGrabbing();
				this.GrabStarted = this.controller.StartedGrab();
				this.GrabReleased = this.controller.ReleasedGrab();
			}
		}
		else
		{
			this.Grabbing = this.controller.IsGrabbing();
			this.GrabStarted = this.controller.StartedGrab();
			this.GrabReleased = this.controller.ReleasedGrab();
		}
		if (this.controller.OrientHeldObject())
		{
			this.DoOrientHeldObject = true;
		}
		if (this.EmulateTriggerClick)
		{
			this.DoOrientHeldObject = true;
			this.EmulateTriggerClick = false;
		}
		GameObject gameObject = null;
		NetworkPhysicsObject networkPhysicsObject = null;
		bool flag2 = false;
		bool flag3 = false;
		if (this.currentMode == TrackedControllerMode.Grabbed)
		{
			gameObject = PlayerScript.PointerScript.FirstGrabbedObject;
			networkPhysicsObject = PlayerScript.PointerScript.FirstGrabbedNPO;
			bool flag4 = networkPhysicsObject && networkPhysicsObject.HeldByPlayerID == id;
			flag2 = (networkPhysicsObject && networkPhysicsObject.cardScript);
			flag3 = (networkPhysicsObject && networkPhysicsObject.deckScript);
			if (flag4 && this.DoOrientHeldObject && Time.time > this.lastGrabTime + VRTrackedController.ORIENT_OBJECT_DELAY)
			{
				if (networkPhysicsObject.CanBeHeldInHand)
				{
					this.objectHeldClose = !this.objectHeldClose;
					networkPhysicsObject.SetHeldClose(this.objectHeldClose ? id : 0, false);
				}
				else if (networkPhysicsObject.GetComponent<JigsawPiece>())
				{
					networkPhysicsObject.DisableFastDragWhileAnimating();
					networkPhysicsObject.HeldByControllerPickupRotation = base.transform.rotation;
					if (networkPhysicsObject.HeldSpinRotationIndex % 6 == 0)
					{
						networkPhysicsObject.HeldSpinRotationIndex = (networkPhysicsObject.HeldSpinRotationIndex + 6) % 24;
					}
					else
					{
						networkPhysicsObject.HeldSpinRotationIndex = ((networkPhysicsObject.HeldSpinRotationIndex - 3) / 6 + 1) * 6;
					}
				}
			}
		}
		if (this.stifleLaserPointer)
		{
			this.laserPointer.turnedOn = false;
		}
		else
		{
			this.laserPointer.turnedOn = (VRTrackedController.LASER_ALWAYS_ON || this.controller.ActivateLaser());
		}
		this.currentPointerMode = ((PlayerScript.Pointer != null) ? PlayerScript.PointerScript.CurrentPointerMode : PointerMode.None);
		Color color2 = (PlayerScript.Pointer != null) ? PlayerScript.PointerScript.HoverColor : Color.white;
		Highlighter highlighter = null;
		bool grabbing = this.Grabbing;
		this.prevHoverObject = this.HoverObject;
		this.prevHoverLockObject = this.HoverLockObject;
		this.prevHoverNonNetwork = this.HoverNonNetwork;
		this.HoverObject = null;
		this.HoverUIObject = null;
		this.HoverLockObject = null;
		this.HoverNonNetwork = null;
		this.LaserHover = false;
		float num2 = float.MaxValue;
		float num3 = float.MaxValue;
		this.UI3DHitObject = null;
		bool flag5 = false;
		bool flag6 = false;
		this.previouslyInVirtualHand = this.inVirtualHand;
		this.inVirtualHand = false;
		this.HoverTrigger = null;
		this.SurfaceObject = null;
		while (!flag5 || flag6)
		{
			Vector3 vector;
			if (flag6)
			{
				vector = Singleton<VRVirtualHandObject>.Instance.ActualPositionFromVirtualHandPosition(this.WorldPosition());
			}
			else
			{
				vector = this.WorldPosition();
			}
			int num4 = Physics.OverlapSphereNonAlloc(vector, this.sphereCollider.radius * base.transform.lossyScale.x, this.grabbabaleColliders);
			for (int j = 0; j < num4; j++)
			{
				Collider collider = this.grabbabaleColliders[j];
				if (collider == Singleton<VRVirtualHandObject>.Instance.VirtualHandCollider)
				{
					if ((this == VRTrackedController.rightVRTrackedController && Singleton<VRVirtualHand>.Instance.Attach == VRControllerAttachment.OnLeft) || (this == VRTrackedController.leftVRTrackedController && Singleton<VRVirtualHand>.Instance.Attach == VRControllerAttachment.OnRight))
					{
						flag6 = true;
					}
				}
				else
				{
					if (!this.HoverNonNetwork && Singleton<VRHMD>.Instance.floatingUI && collider.gameObject.CompareTag("VR UI"))
					{
						this.HoverNonNetwork = collider.gameObject;
					}
					if (Layers.IsUI3D(collider.gameObject))
					{
						float num5 = Vector3.Distance(vector, collider.ClosestPoint(vector));
						if (num5 < num2 && num5 < 0.5f)
						{
							num2 = num5;
							this.UI3DHitObject = collider.gameObject;
							this.HoverObject = null;
						}
					}
					else
					{
						if (this.HoverTrigger == null && ((this.currentPointerMode == PointerMode.FogOfWar && collider.gameObject.tag == "FogOfWar") || (this.currentPointerMode == PointerMode.LayoutZone && collider.gameObject.tag == "Layout") || (this.currentPointerMode == PointerMode.Hands && collider.gameObject.tag == "Hand") || (this.currentPointerMode == PointerMode.Hidden && collider.gameObject.tag == "Fog") || (this.currentPointerMode == PointerMode.Randomize && collider.gameObject.tag == "Randomize") || (this.currentPointerMode == PointerMode.Scripting && collider.gameObject.tag == "Scripting")))
						{
							this.HoverTrigger = collider.gameObject;
						}
						if ((this.SurfaceObject == null || this.SurfaceObject.GetComponent<NetworkPhysicsObject>() == null) && collider.gameObject.tag == "Surface")
						{
							this.SurfaceObject = collider.gameObject;
						}
						NetworkPhysicsObject networkPhysicsObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(collider);
						if (networkPhysicsObject2)
						{
							float num5 = Vector3.Distance(vector, collider.ClosestPoint(networkPhysicsObject2.rigidbody.position));
							if (networkPhysicsObject2.IsGrabbable)
							{
								if (!networkPhysicsObject2.IsLocked && num5 < num2)
								{
									num2 = num5;
									this.HoverObject = networkPhysicsObject2;
								}
								else if (networkPhysicsObject2.IsLocked && num5 < num3)
								{
									num3 = num5;
									this.HoverLockObject = networkPhysicsObject2;
								}
							}
						}
					}
				}
			}
			if (!flag5)
			{
				if (this.HoverObject && this.currentMode == TrackedControllerMode.Default)
				{
					flag6 = false;
				}
			}
			else
			{
				flag6 = false;
				this.inVirtualHand = true;
			}
			flag5 = true;
		}
		if (num3 > num2)
		{
			this.HoverLockObject = this.HoverObject;
		}
		if ((this.currentMode == TrackedControllerMode.Grabbed && !this.MovePositionAtLaser) || this.currentMode == TrackedControllerMode.GrabbedNonNetwork)
		{
			this.SetGrabSphereColor(Colour.Blue);
		}
		else if ((this.HoverObject && this.currentMode != TrackedControllerMode.Grabbed) || (this.HoverNonNetwork && this.currentMode != TrackedControllerMode.GrabbedNonNetwork))
		{
			this.SetGrabSphereColor(Colour.Green);
		}
		else
		{
			this.SetGrabSphereColor(Colour.Red);
		}
		if (this.currentMode == TrackedControllerMode.Grabbed && gameObject && PlayerScript.Pointer)
		{
			if (this.previouslyInVirtualHand && !this.inVirtualHand)
			{
				NetworkPhysicsObject networkPhysicsObject3 = networkPhysicsObject;
				if (networkPhysicsObject3 && networkPhysicsObject3.CanBeHeldInHand)
				{
					Singleton<VRVirtualHandObject>.Instance.ReplaceVirtualObjectWithNPO(networkPhysicsObject3);
					this.controller.TriggerHapticPulse(0.4f);
				}
				using (List<GameObject>.Enumerator enumerator = PlayerScript.PointerScript.GrabbedObjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject2 = enumerator.Current;
						networkPhysicsObject3 = gameObject2.GetComponent<NetworkPhysicsObject>();
						if (networkPhysicsObject3 && networkPhysicsObject3.CanBeHeldInHand && networkPhysicsObject3 != networkPhysicsObject)
						{
							Singleton<VRVirtualHandObject>.Instance.ReplaceVirtualObjectWithNPO(networkPhysicsObject3);
							this.controller.TriggerHapticPulse(0.4f);
						}
					}
					goto IL_BCC;
				}
			}
			if (!this.previouslyInVirtualHand && this.inVirtualHand)
			{
				NetworkPhysicsObject networkPhysicsObject4 = networkPhysicsObject;
				if (networkPhysicsObject4 && networkPhysicsObject4.CanBeHeldInHand)
				{
					networkPhysicsObject4.SetPositionHeld(Singleton<VRVirtualHandObject>.Instance.ActualPositionFromVirtualHandPosition(networkPhysicsObject4.transform.position));
					networkPhysicsObject4.SetRotationHeld(Singleton<VRVirtualHandObject>.Instance.ActualRotationFromVirtualHandRotation(networkPhysicsObject4.transform.rotation).eulerAngles);
					Singleton<VRVirtualHandObject>.Instance.ReplaceNPOWithVirtualObject(networkPhysicsObject4);
					Singleton<VRVirtualHand>.Instance.TriggerHapticPulse(0.4f);
				}
				foreach (GameObject gameObject3 in PlayerScript.PointerScript.GrabbedObjects)
				{
					networkPhysicsObject4 = gameObject3.GetComponent<NetworkPhysicsObject>();
					if (networkPhysicsObject4 && networkPhysicsObject4.CanBeHeldInHand && networkPhysicsObject4 != networkPhysicsObject)
					{
						networkPhysicsObject4.SetPositionHeld(Singleton<VRVirtualHandObject>.Instance.ActualPositionFromVirtualHandPosition(networkPhysicsObject4.transform.position));
						networkPhysicsObject4.SetRotationHeld(Singleton<VRVirtualHandObject>.Instance.ActualRotationFromVirtualHandRotation(networkPhysicsObject4.transform.rotation).eulerAngles);
						Singleton<VRVirtualHandObject>.Instance.ReplaceNPOWithVirtualObject(networkPhysicsObject4);
						Singleton<VRVirtualHand>.Instance.TriggerHapticPulse(0.4f);
					}
				}
			}
		}
		IL_BCC:
		if (this.laserPointer.turnedOn && !this.HoverObject && (this.laserPointer.HitNetwork != null || this.laserPointer.HitObject != null))
		{
			GameObject gameObject4 = this.laserPointer.HitObject.gameObject;
			NetworkPhysicsObject hitNetwork = this.laserPointer.HitNetwork;
			if (hitNetwork && hitNetwork.IsGrabbable && !hitNetwork.IsLocked)
			{
				this.HoverObject = hitNetwork;
			}
			else if (gameObject4.CompareTag("VR UI"))
			{
				this.HoverUIObject = gameObject4;
			}
			else if (Layers.IsUI3D(gameObject4))
			{
				this.UI3DHitObject = gameObject4;
			}
			if (this.HoverLockObject == null)
			{
				this.HoverLockObject = this.laserPointer.HitLockNetwork;
				this.LaserHover = true;
			}
		}
		if (this.HoverLockObject && this.HoverLockObject != VRTrackedController.LatestZoomableObject)
		{
			VRTrackedController.LatestZoomableObject = this.HoverLockObject;
		}
		bool flag7 = this.currentPointerMode == PointerMode.Grab || this.currentPointerMode == PointerMode.Flick || Pointer.IsCombineTool(this.currentPointerMode) || this.currentPointerMode == PointerMode.Move || this.currentPointerMode == PointerMode.Scale || this.currentPointerMode == PointerMode.VolumeScale || this.currentPointerMode == PointerMode.Rotate || this.currentPointerMode == PointerMode.Snap || this.currentPointerMode == PointerMode.SnapRotate;
		bool flag8 = this.currentPointerMode == PointerMode.Grab || this.currentPointerMode == PointerMode.Flick || Pointer.IsCombineTool(this.currentPointerMode);
		if (!flag7 || this.laserPointer.UI3DHitObject != null || this.laserPointer.UIHitFirst)
		{
			this.HoverObject = null;
			this.HoverLockObject = null;
		}
		string text = Pointer.PointerModeToTag(this.currentPointerMode);
		if (text != "" && this.laserPointer.turnedOn && this.laserPointer.HitTrigger && this.laserPointer.HitTrigger.CompareTag(text))
		{
			this.HoverTrigger = this.laserPointer.HitTrigger;
		}
		if (this.HoverTrigger)
		{
			highlighter = this.HoverTrigger.GetComponent<Highlighter>();
		}
		if (this.HoverObject && this.HoverObject.highlighter && (!grabbing || Pointer.IsCombineTool(this.currentPointerMode)))
		{
			highlighter = this.HoverObject.highlighter;
		}
		if (this.HoverNonNetwork)
		{
			highlighter = this.HoverNonNetwork.GetComponent<Highlighter>();
		}
		if (highlighter)
		{
			highlighter.On(color2);
		}
		if (this.HoverLockObject == null && this.HoverTrigger)
		{
			this.HoverLockObject = this.HoverTrigger.GetComponent<NetworkPhysicsObject>();
		}
		string text2 = "";
		if (this.HoverLockObject && this.currentMode == TrackedControllerMode.Default)
		{
			HoverIcons hoverIcons;
			HoverScript.ObjectToTooltip(this.HoverLockObject, ref this.PrevTooltipObject, ref this.TooltipTimeHolder, out text2, out hoverIcons);
			Vector3 position = this.HoverLockObject.transform.position;
			Vector3 vector2;
			Bounds boundsNotNormalized = this.HoverLockObject.GetBoundsNotNormalized(out vector2);
			position.y += boundsNotNormalized.extents.y + 0.5f - vector2.y;
			this.TextTooltip.transform.position = position;
			Vector3 worldPosition = new Vector3(Singleton<VRHMD>.Instance.Pivot.transform.position.x, this.HoverLockObject.transform.position.y, Singleton<VRHMD>.Instance.Pivot.transform.position.z);
			this.TextTooltip.transform.LookAt(worldPosition);
			this.TextTooltip.transform.Rotate(new Vector3(45f, 180f, 0f));
		}
		else
		{
			this.PrevTooltipObject = null;
		}
		bool flag9 = text2 != "";
		if (this.TextTooltip.gameObject.activeSelf != flag9)
		{
			this.TextTooltip.gameObject.SetActive(flag9);
		}
		this.TextTooltip.text = text2;
		if (this.grabNextObjectUntil != 0f)
		{
			if (Time.time > this.grabNextObjectUntil)
			{
				this.grabNextObjectUntil = 0f;
			}
			else if (this.HoverObject && this.currentMode == TrackedControllerMode.Default)
			{
				this.GrabStarted = true;
				this.grabbingFromSearch = true;
				this.grabNextObjectUntil = 0f;
			}
		}
		this.newlyGrabbedObject = false;
		if (this.GrabStarted)
		{
			this.controller.TriggerHapticPulse(1f);
			this.ResetUIHover();
			if (this.currentMode == TrackedControllerMode.Default)
			{
				if (this.HoverNonNetwork)
				{
					if (!this.HoverNonNetwork.transform.parent.GetComponent<VRTrackedController>() && Singleton<VRHMD>.Instance.VRUIScript.VRUIAttached == VRControllerAttachment.Detached)
					{
						this.GrabbedNonNetwork = this.HoverNonNetwork;
						this.GrabbedNonNetworkParent = this.GrabbedNonNetwork.transform.parent;
						this.GrabbedNonNetwork.transform.parent = base.transform;
						this.currentMode = TrackedControllerMode.GrabbedNonNetwork;
					}
				}
				else if (this.UI3DHitObject != null)
				{
					this.currentMode = TrackedControllerMode.UI3D;
					this.Click3DUI(this.UI3DHitObject);
				}
				else if (this.Grab(this.inVirtualHand))
				{
					this.currentMode = TrackedControllerMode.Grabbed;
					this.newlyGrabbedObject = true;
					this.objectHeldClose = false;
					this.lastGrabTime = Time.time;
					if (VRTrackedController.HIDE_VIRTUAL_HAND_WHEN_GRABBING)
					{
						Singleton<VRVirtualHand>.Instance.ToggleVirtualHand(false, this);
					}
					if (VRTrackedController.HIDE_GEM_ON_GRAB)
					{
						this.GrabSphere.GetComponent<MeshRenderer>().enabled = false;
					}
				}
				else if (this.StartTool())
				{
					if (this.doingGizmoSelect)
					{
						if (PlayerScript.Pointer)
						{
							PlayerScript.PointerScript.ResetAllObjects();
							PlayerScript.PointerScript.ResetHighlight();
							this.currentMode = TrackedControllerMode.Selection;
							this.StartSelection();
						}
						else
						{
							this.doingGizmoSelect = false;
						}
					}
					else
					{
						this.currentMode = TrackedControllerMode.Tool;
					}
				}
				else if (PlayerScript.Pointer && flag8)
				{
					PlayerScript.PointerScript.ResetAllObjects();
					PlayerScript.PointerScript.ResetHighlight();
					this.currentMode = TrackedControllerMode.Selection;
					this.StartSelection();
				}
			}
		}
		if (this.Grabbing)
		{
			TrackedControllerMode currentMode = this.currentMode;
			if (currentMode != TrackedControllerMode.Selection)
			{
				if (currentMode == TrackedControllerMode.Tool)
				{
					this.UpdateTool();
				}
			}
			else
			{
				this.UpdateSelection();
			}
		}
		if (this.GrabReleased)
		{
			this.controller.TriggerHapticPulse(0.4f);
			if (VRTrackedController.HIDE_GEM_ON_GRAB)
			{
				this.GrabSphere.GetComponent<MeshRenderer>().enabled = true;
			}
			TrackedControllerMode currentMode = this.currentMode;
			if (currentMode <= TrackedControllerMode.Selection)
			{
				if (currentMode != TrackedControllerMode.Grabbed)
				{
					if (currentMode == TrackedControllerMode.Selection)
					{
						this.EndSelection();
						if (this.doingGizmoSelect)
						{
							this.currentMode = TrackedControllerMode.Tool;
							this.EndTool();
							this.doingGizmoSelect = false;
						}
					}
				}
				else
				{
					this.Release();
				}
			}
			else if (currentMode != TrackedControllerMode.Tool)
			{
				if (currentMode == TrackedControllerMode.GrabbedNonNetwork)
				{
					this.GrabbedNonNetwork.transform.parent = this.GrabbedNonNetworkParent;
					this.GrabbedNonNetwork = null;
				}
			}
			else
			{
				this.EndTool();
			}
			if (this.currentMode != TrackedControllerMode.Teleport && this.currentMode != TrackedControllerMode.Zoom)
			{
				this.currentMode = TrackedControllerMode.Default;
			}
			if (this.grabStackCoroutine != null)
			{
				base.StopCoroutine(this.grabStackCoroutine);
			}
		}
		this.previouslyPeeking = this.peeking;
		this.peeking = this.controller.Peek();
		if (this.currentMode == TrackedControllerMode.Zoom && this.peeking != this.previouslyPeeking)
		{
			this.ToggleZoomObject(null);
			this.ToggleZoomObject(VRTrackedController.LatestZoomableObject);
		}
		this.wantsToMove = this.controller.IsMoveEnabled();
		if (VRTrackedController.ENABLE_TOOLTIP_ACTION && this.controller.DisplayTooltips())
		{
			if (!VRTrackedController.ALWAYS_DISPLAY_TOOLTIPS && !this.wantsToDisplayTooltips)
			{
				this.wantsToDisplayTooltips = true;
				this.DisplayTooltips(true, false);
			}
		}
		else if (this.wantsToDisplayTooltips)
		{
			this.wantsToDisplayTooltips = false;
			if (!VRTrackedController.ALWAYS_DISPLAY_TOOLTIPS)
			{
				this.DisplayTooltips(false, false);
			}
		}
		if (this.controller.ToggleMainMenu())
		{
			NetworkSingleton<NetworkUI>.Instance.EscapeMenu(NetworkUI.EscapeMenuActivation.AUTO);
			if (PlayerScript.Pointer)
			{
				PlayerScript.PointerScript.ResetAllObjects();
			}
		}
		if (this.controller.ResetPosition())
		{
			Singleton<VRHMD>.Instance.ResetToInitialPosition();
		}
		if (this.displayTooltipsOnCodebaseChange)
		{
			this.DisplayTooltips(true, true);
			this.displayTooltipsOnCodebaseChange = false;
		}
		if (this.startedDisplayingtTooltipsAt != 0f && Time.time > this.startedDisplayingtTooltipsAt + VRTrackedController.HIDE_TOOLTIPS_DELAY)
		{
			if (!VRTrackedController.ALWAYS_DISPLAY_TOOLTIPS)
			{
				this.DisplayTooltips(false, false);
			}
			this.startedDisplayingtTooltipsAt = 0f;
		}
		if (this.currentMode == TrackedControllerMode.Teleport)
		{
			this.laserPointer.SetColor(Colour.Purple);
		}
		else if (this.currentMode == TrackedControllerMode.Grabbed && (this.MovePositionAtLaser || this.HoverUIObject))
		{
			this.laserPointer.SetColor(Colour.Blue);
		}
		else if (this.HoverObject && this.currentMode != TrackedControllerMode.Grabbed && this.LaserHover)
		{
			this.laserPointer.SetColor(Colour.Green);
		}
		else if ((this.controller.Action(VRAction.CENTER_TOUCH) && this.currentMode != TrackedControllerMode.Grabbed) || this.currentMode == TrackedControllerMode.UI)
		{
			this.laserPointer.SetColor(Colour.Red);
		}
		else
		{
			this.laserPointer.SetColor(Colour.Orange);
		}
		this.stifleLaserPointer = false;
		this.TeleportLine.SetActive(this.currentMode == TrackedControllerMode.Teleport);
		if (this.TeleportLine.activeSelf)
		{
			this.TeleportLine.transform.position = this.laserPointer.HitPoint;
		}
		this.controller.GetTouchpadAxis();
		int num6 = 0;
		if (PlayerScript.Pointer)
		{
			num6 = PlayerScript.PointerScript.RotationSnap / 15;
		}
		float intensity = 0.5f;
		if (this.renderModel.hasAnalogStick)
		{
			this.TouchpadIconContainer.SetActive(this.controller.PadTouched() || VRTrackedController.TOUCHPAD_ICONS_ALWAYS_ON);
		}
		if (this.controller.NewPadTouched())
		{
			this.controller.TriggerHapticPulse((float)((ushort)(this.controller.Action(VRAction.CENTER_TOUCH) ? 1f : 0.4f)));
		}
		this.TouchpadUp.Touching = this.controller.Action(VRAction.NORTH_TOUCH);
		this.TouchpadDown.Touching = this.controller.Action(VRAction.SOUTH_TOUCH);
		this.TouchpadLeft.Touching = this.controller.Action(VRAction.WEST_TOUCH);
		this.TouchpadRight.Touching = this.controller.Action(VRAction.EAST_TOUCH);
		this.TouchpadCenter.Touching = this.controller.Action(VRAction.CENTER_TOUCH);
		this.interfaceForMenu = false;
		if (this.controller.ActivateLaser() && ((this.currentMode == TrackedControllerMode.Grabbed && this.potentialStackOrDeckGrabNPO == null) || (this.currentMode == TrackedControllerMode.Default && (this.HoverObject || this.HoverLockObject) && !this.LaserHover)))
		{
			this.laserPointer.turnedOn = false;
			this.stifleLaserPointer = true;
			this.interfaceForMenu = true;
		}
		bool flag10;
		bool flag11;
		bool flag12;
		if (this.laserPointer.turnedOn || this.interfaceForMenu)
		{
			flag10 = this.controller.Action(VRAction.INTERFACE_CLICK);
			flag11 = this.controller.ActionPress(VRAction.INTERFACE_CLICK);
			flag12 = this.controller.ActionRelease(VRAction.INTERFACE_CLICK);
			if (flag11)
			{
				this.controller.TriggerHapticPulse(0.4f);
			}
			if (this.currentMode == TrackedControllerMode.Default && this.laserPointer.UI3DHitObject != null)
			{
				this.currentMode = TrackedControllerMode.UI3D;
			}
			else if (this.currentMode == TrackedControllerMode.UI3D && this.laserPointer.UI3DHitObject == null)
			{
				this.currentMode = TrackedControllerMode.Default;
			}
			if (this.currentMode == TrackedControllerMode.Default && flag11 && this.HoverLockObject && PlayerScript.Pointer)
			{
				PlayerScript.PointerScript.StartContextual(this.HoverLockObject.gameObject, true);
			}
			if (this.HoverUIObject && this.currentMode == TrackedControllerMode.Default && this.otherController.currentMode != TrackedControllerMode.UI)
			{
				if (!VRTrackedController.currentUITracked)
				{
					VRTrackedController.currentUITracked = this;
				}
				if (VRTrackedController.currentUITracked == this)
				{
					this.TouchChanged(TouchPhase.Moved, true);
					if (flag10)
					{
						this.ResetUIHover();
						this.TouchChanged(TouchPhase.Began, false);
						this.currentMode = TrackedControllerMode.UI;
					}
				}
			}
			else if (VRTrackedController.currentUITracked == this && !this.HoverUIObject)
			{
				this.TouchChanged(TouchPhase.Canceled, false);
				this.ResetUIHover();
			}
			else if (this.currentMode == TrackedControllerMode.UI)
			{
				if (this.HoverUIObject)
				{
					if (flag12)
					{
						this.TouchChanged(TouchPhase.Ended, false);
						this.ResetUIHover();
						this.currentMode = TrackedControllerMode.Default;
					}
					else
					{
						this.TouchChanged(TouchPhase.Moved, false);
					}
				}
			}
			else if (this.currentMode == TrackedControllerMode.UI3D)
			{
				if (flag11 && this.laserPointer.UI3DHitObject != null)
				{
					this.Click3DUI(this.laserPointer.UI3DHitObject);
				}
			}
			else if (this.currentMode == TrackedControllerMode.Zoom && flag10 && this.isZoomLocked)
			{
				this.isZoomLocked = false;
				this.ToggleZoomObject(null);
				this.awaitingZoomRelease = true;
			}
		}
		else
		{
			if (this.currentMode == TrackedControllerMode.UI || VRTrackedController.currentUITracked == this)
			{
				this.TouchChanged(TouchPhase.Canceled, false);
				this.ResetUIHover();
				this.currentMode = TrackedControllerMode.Default;
			}
			if (this.currentMode == TrackedControllerMode.UI3D)
			{
				this.currentMode = TrackedControllerMode.Default;
			}
		}
		if (this.laserPointer.turnedOn && this.UI3DHitObject == null && this.laserPointer.UI3DHitObject != null && this.currentMode == TrackedControllerMode.UI3D)
		{
			this.UI3DHitObject = this.laserPointer.UI3DHitObject;
		}
		if (this.PreviousUI3DHitObject != null && (this.UI3DHitObject != this.PreviousUI3DHitObject || (this.currentMode != TrackedControllerMode.Default && this.currentMode != TrackedControllerMode.UI3D)))
		{
			this.PreviousUI3DHitObject.SendMessage("OnHover", false, SendMessageOptions.DontRequireReceiver);
			this.PreviousUI3DHitObject = null;
		}
		if (this.UI3DHitObject != null && this.UI3DHitObject != this.PreviousUI3DHitObject && (this.currentMode == TrackedControllerMode.Default || this.currentMode == TrackedControllerMode.UI3D))
		{
			this.UI3DHitObject.SendMessage("OnHover", true, SendMessageOptions.DontRequireReceiver);
			this.PreviousUI3DHitObject = this.UI3DHitObject;
			this.controller.TriggerHapticPulse(intensity);
		}
		flag10 = this.controller.Action(VRAction.NORTH_CLICK);
		flag11 = this.controller.ActionPress(VRAction.NORTH_CLICK);
		flag12 = this.controller.ActionRelease(VRAction.NORTH_CLICK);
		if (this.controller.Action(VRAction.NORTH_TOUCH) || flag10)
		{
			if (this.currentMode == TrackedControllerMode.Default)
			{
				if (this.HoverObject)
				{
					Pointer pointerScript = PlayerScript.PointerScript;
					if (pointerScript && flag11)
					{
						this.controller.TriggerHapticPulse(intensity);
						pointerScript.Rotation(this.HoverObject.ID, 0, 12, true);
						this.justFlipped = true;
					}
				}
				else if (VRTrackedController.UP_IS_TELEPORT && !this.justFlipped)
				{
					this.currentMode = TrackedControllerMode.Teleport;
				}
				else if (flag11)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadNorth, VRTrackedController.VRKeyEvent.Pressed);
				}
				else if (flag12)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadNorth, VRTrackedController.VRKeyEvent.Released);
				}
				else if (flag10)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadNorth, VRTrackedController.VRKeyEvent.Held);
				}
			}
			if (this.currentMode == TrackedControllerMode.Teleport)
			{
				if (flag11)
				{
					this.Teleport(this.laserPointer.HitPoint);
				}
			}
			else if (this.currentMode == TrackedControllerMode.Grabbed && gameObject != null)
			{
				if (PlayerScript.PointerScript && flag11)
				{
					this.controller.TriggerHapticPulse(intensity);
					PlayerScript.PointerScript.ChangeHeldFlipRotationIndex(12, this.Index);
				}
			}
			else if (this.currentMode == TrackedControllerMode.Zoom && flag11 && this.isZoomLocked)
			{
				this.isZoomLocked = false;
				this.ToggleZoomObject(null);
				this.awaitingZoomRelease = true;
			}
		}
		else
		{
			this.justFlipped = false;
			if (this.currentMode == TrackedControllerMode.Teleport)
			{
				this.currentMode = TrackedControllerMode.Default;
			}
		}
		flag10 = this.controller.Action(VRAction.SOUTH_CLICK);
		flag11 = this.controller.ActionPress(VRAction.SOUTH_CLICK);
		flag12 = this.controller.ActionRelease(VRAction.SOUTH_CLICK);
		if (this.controller.Action(VRAction.SOUTH_TOUCH) || flag10)
		{
			if (this.currentMode == TrackedControllerMode.Zoom)
			{
				if (VRTrackedController.LatestZoomableObject != this.currentZoomObject)
				{
					this.ToggleZoomObject(null);
					this.ToggleZoomObject(VRTrackedController.LatestZoomableObject);
				}
				if (this.controller.ActionPress(VRAction.PAD_CLICK))
				{
					this.isZoomLocked = !this.isZoomLocked;
					if (!this.isZoomLocked)
					{
						this.ToggleZoomObject(null);
						this.awaitingZoomRelease = true;
					}
				}
			}
			else if (this.currentMode != TrackedControllerMode.Grabbed || !flag3)
			{
				if (this.currentMode == TrackedControllerMode.Default)
				{
					if (this.touchpadDownMode == VRTrackedController.TouchpadDownMode.ToolSelect)
					{
						this.laserPointer.turnedOn = false;
						this.stifleLaserPointer = true;
					}
					else if (this.touchpadDownMode == VRTrackedController.TouchpadDownMode.Zoom)
					{
						this.laserPointer.turnedOn = false;
						this.stifleLaserPointer = true;
						if (VRTrackedController.LatestZoomableObject && !this.awaitingZoomRelease)
						{
							this.ToggleZoomObject(VRTrackedController.LatestZoomableObject);
						}
					}
				}
				if (this.touchpadDownMode == VRTrackedController.TouchpadDownMode.Bindable)
				{
					if (flag11)
					{
						this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadSouth, VRTrackedController.VRKeyEvent.Pressed);
					}
					else if (flag12)
					{
						this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadSouth, VRTrackedController.VRKeyEvent.Released);
					}
					else if (flag10)
					{
						this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadSouth, VRTrackedController.VRKeyEvent.Held);
					}
				}
			}
		}
		else if (this.currentMode == TrackedControllerMode.Zoom)
		{
			if (this.controller.ActionPress(VRAction.PAD_CLICK) && this.isZoomLocked)
			{
				this.isZoomLocked = false;
			}
			if (!this.isZoomLocked)
			{
				this.ToggleZoomObject(null);
			}
		}
		else if (this.awaitingZoomRelease)
		{
			this.awaitingZoomRelease = false;
		}
		flag10 = this.controller.Action(VRAction.WEST_CLICK);
		flag11 = this.controller.ActionPress(VRAction.WEST_CLICK);
		flag12 = this.controller.ActionRelease(VRAction.WEST_CLICK);
		bool flag13 = this.controller.ActionRepeated(VRAction.WEST_CLICK);
		if (this.controller.Action(VRAction.WEST_TOUCH) || flag10)
		{
			if (this.currentMode == TrackedControllerMode.Default)
			{
				if (this.HoverObject && PlayerScript.PointerScript)
				{
					if (this.HoverObject.deckScript)
					{
						if (flag11 || flag13)
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(this.HoverObject.ID, PlayerScript.PointerScript.PointerColorLabel, 1, 0);
						}
					}
					else if (this.HoverObject.CanBeHeldInHand && flag11)
					{
						this.ActOnSelectedObjects(delegate(NetworkPhysicsObject npo)
						{
							if (npo.CanBeHeldInHand)
							{
								NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(npo.ID, PlayerScript.PointerScript.PointerColorLabel, 1, 0);
							}
						});
					}
				}
				else if (flag11)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadWest, VRTrackedController.VRKeyEvent.Pressed);
				}
				else if (flag12)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadWest, VRTrackedController.VRKeyEvent.Released);
				}
				else if (flag10)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadWest, VRTrackedController.VRKeyEvent.Held);
				}
			}
			else if (this.currentMode == TrackedControllerMode.Grabbed && gameObject != null && PlayerScript.Pointer)
			{
				Pointer pointerScript2 = PlayerScript.PointerScript;
				if (flag11 || flag13)
				{
					if (flag2 || flag3)
					{
						PlayerScript.PointerScript.ChangeHeldSpinRotationIndex(-6, this.Index);
					}
					else
					{
						PlayerScript.PointerScript.ChangeHeldSpinRotationIndex(-num6, this.Index);
					}
				}
			}
			else if (this.currentMode == TrackedControllerMode.Zoom)
			{
				this.ZoomObject.transform.localScale = this.ZoomObject.transform.localScale / (1f + 1.75f * Time.deltaTime);
				if (flag11 && this.isZoomLocked)
				{
					this.isZoomLocked = false;
					this.ToggleZoomObject(null);
					this.awaitingZoomRelease = true;
				}
			}
		}
		flag10 = this.controller.Action(VRAction.EAST_CLICK);
		flag11 = this.controller.ActionPress(VRAction.EAST_CLICK);
		flag12 = this.controller.ActionRelease(VRAction.EAST_CLICK);
		flag13 = this.controller.ActionRepeated(VRAction.EAST_CLICK);
		if (this.controller.Action(VRAction.EAST_TOUCH) || flag10)
		{
			if (this.currentMode == TrackedControllerMode.Default)
			{
				if (NetworkSingleton<NetworkUI>.Instance.InventoryActive())
				{
					if (flag11)
					{
						NetworkSingleton<NetworkUI>.Instance.EscapeMenu(NetworkUI.EscapeMenuActivation.AUTO);
						this.UpdateTouchpadIcons(VRTrackedController.VRKeyCode.VRPadEast);
					}
				}
				else if (this.HoverObject && PlayerScript.PointerScript)
				{
					if (ManagerPhysicsObject.InventoryTypeFromObject(this.HoverObject.gameObject) != InventoryTypes.None)
					{
						if (flag11)
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.SearchInventory(this.HoverObject.ID, id, -1);
						}
					}
					else if (this.HoverObject.CompareTag("Dice") || this.HoverObject.CompareTag("Coin"))
					{
						if (flag11)
						{
							this.ActOnSelectedObjects(delegate(NetworkPhysicsObject npo)
							{
								if (npo.CompareTag("Dice") || npo.CompareTag("Coin"))
								{
									PlayerScript.PointerScript.Randomize(npo.ID, PlayerScript.PointerScript.ID);
								}
							});
						}
					}
					else if (this.CanGroupSelectedObjects() && this.SelectedObjectCountWhenOnStackable > 1 && flag11)
					{
						PlayerScript.PointerScript.Group(this.HoverObject.ID);
						this.SelectedObjectCountWhenOnStackable = 0;
					}
				}
				else if (this.HoverLockObject && PlayerScript.PointerScript && ManagerPhysicsObject.InventoryTypeFromObject(this.HoverLockObject.gameObject) != InventoryTypes.None)
				{
					if (flag11)
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.SearchInventory(this.HoverLockObject.ID, id, -1);
					}
				}
				else if (flag11)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadEast, VRTrackedController.VRKeyEvent.Pressed);
				}
				else if (flag12)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadEast, VRTrackedController.VRKeyEvent.Released);
				}
				else if (flag10)
				{
					this.TouchpadEvent(VRTrackedController.VRKeyCode.VRPadEast, VRTrackedController.VRKeyEvent.Held);
				}
			}
			else if (this.currentMode == TrackedControllerMode.Grabbed && gameObject != null && PlayerScript.Pointer)
			{
				if (flag11 || flag13)
				{
					if (flag2 || flag3)
					{
						PlayerScript.PointerScript.ChangeHeldSpinRotationIndex(6, this.Index);
					}
					else
					{
						PlayerScript.PointerScript.ChangeHeldSpinRotationIndex(num6, this.Index);
					}
				}
			}
			else if (this.currentMode == TrackedControllerMode.Zoom)
			{
				this.ZoomObject.transform.localScale = this.ZoomObject.transform.localScale * (1f + 1.75f * Time.deltaTime);
				if (flag11 && this.isZoomLocked)
				{
					this.isZoomLocked = false;
					this.ToggleZoomObject(null);
					this.awaitingZoomRelease = true;
				}
			}
		}
		if (this.controller.Action(VRAction.TELEPORT_START))
		{
			if (this.currentMode == TrackedControllerMode.Default)
			{
				this.currentMode = TrackedControllerMode.Teleport;
			}
			if (this.currentMode == TrackedControllerMode.Teleport && this.controller.ActionPress(VRAction.TELEPORT_CONFIRM))
			{
				this.Teleport(this.laserPointer.HitPoint);
			}
		}
		else if (this.currentMode == TrackedControllerMode.Teleport)
		{
			this.currentMode = TrackedControllerMode.Default;
		}
		if (this.pointerColor != NetworkSingleton<NetworkUI>.Instance.playerColour)
		{
			this.pointerColor = NetworkSingleton<NetworkUI>.Instance.playerColour;
			this.SetColor(this.pointerColor);
		}
		if (this.currentMode == TrackedControllerMode.Zoom)
		{
			Vector3 normalized = (this.ZoomObject.transform.position - this.HMD.transform.position).normalized;
			bool flag14 = this.ZoomObjectFacingMultiplier == 0 || Vector3.Dot(normalized, (float)this.ZoomObjectFacingMultiplier * this.ZoomObject.transform.up) > 0.05f;
			if (!flag14)
			{
				if (PermissionsOptions.CheckAllow(PermissionsOptions.options.Peeking, -1))
				{
					if (this.currentZoomObject)
					{
						this.currentZoomObject.AddPeekIndicator();
					}
				}
				else
				{
					PermissionsOptions.BroadcastPermissionWarning("Peek");
				}
			}
			this.laserPointer.turnedOn = false;
			this.stifleLaserPointer = true;
			if (this.ZoomObject)
			{
				this.ZoomObject.SetActive(flag14 || PermissionsOptions.CheckAllow(PermissionsOptions.options.Peeking, -1));
			}
		}
		if (this.Model.activeSelf == this.ZoomObject.activeSelf)
		{
			this.Model.SetActive(!this.ZoomObject.activeSelf);
			this.GrabSphere.SetActive(this.Model.activeSelf);
			this.UI.SetActive(this.Model.activeSelf);
		}
		if (this.currentMode != this.previousMode || this.HoverObject != this.prevHoverObject || this.HoverLockObject != this.prevHoverLockObject)
		{
			if (this.HoverObject && this.CanGroupSelectedObjects())
			{
				this.SelectedObjectCountWhenOnStackable = PlayerScript.PointerScript.GetSelectedObjectCount(-1, true, false);
			}
			this.UpdateTouchpadIcons(VRTrackedController.VRKeyCode.None);
			this.previousMode = this.currentMode;
		}
		if (!PlayerScript.Pointer)
		{
			return;
		}
		this.HapticsUpdate();
		if (this.inVirtualHand && (this.currentMode == TrackedControllerMode.Default || (this.currentMode == TrackedControllerMode.Grabbed && ((networkPhysicsObject && networkPhysicsObject.CanBeHeldInHand) || this.newlyGrabbedObject))))
		{
			PlayerScript.PointerScript.pointerSyncs.SetTrackedTransform(Singleton<VRVirtualHandObject>.Instance.ActualPositionFromVirtualHandPosition(this.WorldPosition()), Singleton<VRVirtualHandObject>.Instance.ActualRotationFromVirtualHandRotation(base.gameObject.transform.rotation), Singleton<VRHMD>.Instance.VRCameraRig.localScale.x, TrackedType.Controller, this.trackedObject, this.MovePositionAtLaser && this.laserPointer.HitPoint != Vector3.zero, this.Index);
			return;
		}
		PlayerScript.PointerScript.pointerSyncs.SetTrackedTransform(this.WorldPosition(), base.gameObject.transform.rotation, Singleton<VRHMD>.Instance.VRCameraRig.localScale.x, TrackedType.Controller, this.trackedObject, this.MovePositionAtLaser && this.laserPointer.HitPoint != Vector3.zero, this.Index);
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x00128768 File Offset: 0x00126968
	private void SetAction(TrackedControllerButton button, VRTouchpadIcon padDisplay, PointerMode pointerMode, string tooltip = null)
	{
		this.SetAction(button, padDisplay, null, new PointerMode?(pointerMode), tooltip);
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x00128790 File Offset: 0x00126990
	private void SetAction(TrackedControllerButton button, VRTouchpadIcon padDisplay, VRControlIcon controlIcon, string tooltip = null)
	{
		this.SetAction(button, padDisplay, new VRControlIcon?(controlIcon), null, tooltip);
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x001287B8 File Offset: 0x001269B8
	private void SetAction(TrackedControllerButton button, VRTouchpadIcon padDisplay, VRControlIcon? controlIcon = null, PointerMode? pointerMode = null, string tooltip = null)
	{
		if (VRRenderModel.HideTouchpad)
		{
			padDisplay.HideIcon();
		}
		else if (controlIcon != null)
		{
			padDisplay.SetIcon(controlIcon.Value);
		}
		else if (pointerMode != null)
		{
			padDisplay.SetIcon(pointerMode.Value);
		}
		if (tooltip != null)
		{
			this.UpdateTooltip(button, tooltip);
		}
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x0012880F File Offset: 0x00126A0F
	private void UpdateTooltip(TrackedControllerButton button, string tooltip)
	{
		this.Tooltips[(int)button].GetComponent<TextMesh>().text = tooltip;
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x00128824 File Offset: 0x00126A24
	private void OnServerStart()
	{
		this.UpdateTouchpadIcons(VRTrackedController.VRKeyCode.None);
		this.DisplayTooltips(false, true);
	}

	// Token: 0x060029B1 RID: 10673 RVA: 0x00128838 File Offset: 0x00126A38
	private void UpdateTouchpadIcons(VRTrackedController.VRKeyCode vrKeyCode = VRTrackedController.VRKeyCode.None)
	{
		if (!this.initialized)
		{
			return;
		}
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.Old)
		{
			if (PlayerScript.PointerScript)
			{
				this.OnChangePointerMode(PlayerScript.PointerScript.CurrentPointerMode);
			}
			this.SetAction(TrackedControllerButton.PadCenter, this.TouchpadCenter, VRControlIcon.Teleport, "Teleport");
			this.SetAction(TrackedControllerButton.PadUp, this.TouchpadUp, VRControlIcon.Raise, "Raise");
			this.SetAction(TrackedControllerButton.PadDown, this.TouchpadDown, VRControlIcon.Flip, "Flip");
			this.SetAction(TrackedControllerButton.PadLeft, this.TouchpadLeft, VRControlIcon.RotateLeft, "Rotate Left");
			this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.RotateRight, "Rotate Right");
			return;
		}
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			this.SetAction(TrackedControllerButton.PadCenter, this.TouchpadCenter, VRControlIcon.Laser, "Click");
			this.SetAction(TrackedControllerButton.PadUp, this.TouchpadUp, VRControlIcon.None, "");
			this.SetAction(TrackedControllerButton.PadDown, this.TouchpadDown, VRControlIcon.None, "");
			this.SetAction(TrackedControllerButton.PadLeft, this.TouchpadLeft, VRControlIcon.None, "");
			this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.None, "");
			return;
		}
		if (vrKeyCode == VRTrackedController.VRKeyCode.None && PlayerScript.PointerScript)
		{
			this.OnChangePointerMode(PlayerScript.PointerScript.CurrentPointerMode);
		}
		TrackedControllerMode currentMode = this.currentMode;
		if (currentMode == TrackedControllerMode.Grabbed)
		{
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadCenter)
			{
				this.SetAction(TrackedControllerButton.PadCenter, this.TouchpadCenter, VRControlIcon.None, "");
			}
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadNorth)
			{
				this.SetAction(TrackedControllerButton.PadUp, this.TouchpadUp, VRControlIcon.Flip, "Flip");
			}
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadWest)
			{
				this.SetAction(TrackedControllerButton.PadLeft, this.TouchpadLeft, VRControlIcon.RotateLeft, "Rotate Left");
			}
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadEast)
			{
				this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.RotateRight, "Rotate Right");
			}
		}
		else
		{
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadCenter)
			{
				this.SetAction(TrackedControllerButton.PadCenter, this.TouchpadCenter, VRControlIcon.Laser, "Click");
			}
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadNorth)
			{
				if (this.HoverObject)
				{
					this.SetAction(TrackedControllerButton.PadUp, this.TouchpadUp, VRControlIcon.Flip, "Flip");
				}
				else if (VRTrackedController.UP_IS_TELEPORT)
				{
					this.SetAction(TrackedControllerButton.PadUp, this.TouchpadUp, VRControlIcon.Teleport, "Teleport");
				}
				else if (this.ToolBindingsAttached)
				{
					this.SetAction(TrackedControllerButton.PadUp, this.TouchpadUp, this.TouchpadUp.AttachedTool, LibString.CamelCaseFromUnderscore(this.TouchpadUp.AttachedTool.ToString(), true, true));
				}
				else
				{
					this.SetAction(TrackedControllerButton.PadUp, this.TouchpadUp, VRControlIcon.None, "");
				}
			}
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadWest)
			{
				if (this.HoverObject)
				{
					if (this.HoverObject.deckScript || this.HoverObject.CanBeHeldInHand)
					{
						this.SetAction(TrackedControllerButton.PadLeft, this.TouchpadLeft, VRControlIcon.Draw, "Draw");
					}
					else
					{
						this.SetAction(TrackedControllerButton.PadLeft, this.TouchpadLeft, VRControlIcon.None, "");
					}
				}
				else if (this.ToolBindingsAttached)
				{
					this.SetAction(TrackedControllerButton.PadLeft, this.TouchpadLeft, this.TouchpadLeft.AttachedTool, LibString.CamelCaseFromUnderscore(this.TouchpadLeft.AttachedTool.ToString(), true, true));
				}
				else
				{
					this.SetAction(TrackedControllerButton.PadLeft, this.TouchpadLeft, VRControlIcon.None, "");
				}
			}
			if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadEast)
			{
				if (NetworkSingleton<NetworkUI>.Instance.InventoryActive())
				{
					this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.Search, "Close Search");
				}
				else if (this.HoverObject)
				{
					if (ManagerPhysicsObject.InventoryTypeFromObject(this.HoverObject.gameObject) != InventoryTypes.None)
					{
						this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.Search, "Search");
					}
					else if (this.HoverObject.CompareTag("Dice") || this.HoverObject.CompareTag("Coin"))
					{
						this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.Roll, "Roll");
					}
					else if (this.CanGroupSelectedObjects() && this.SelectedObjectCountWhenOnStackable > 1)
					{
						this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.Group, "Group");
					}
					else
					{
						this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.None, "");
					}
				}
				else if (this.HoverLockObject && ManagerPhysicsObject.InventoryTypeFromObject(this.HoverLockObject.gameObject) != InventoryTypes.None)
				{
					this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.Search, "Search");
				}
				else if (this.ToolBindingsAttached)
				{
					this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, this.TouchpadRight.AttachedTool, LibString.CamelCaseFromUnderscore(this.TouchpadRight.AttachedTool.ToString(), true, true));
				}
				else
				{
					this.SetAction(TrackedControllerButton.PadRight, this.TouchpadRight, VRControlIcon.None, "");
				}
			}
		}
		if (vrKeyCode == VRTrackedController.VRKeyCode.None || vrKeyCode == VRTrackedController.VRKeyCode.VRPadSouth)
		{
			if (this.touchpadDownMode == VRTrackedController.TouchpadDownMode.Bindable)
			{
				this.SetAction(TrackedControllerButton.PadDown, this.TouchpadDown, this.TouchpadDown.AttachedTool, LibString.CamelCaseFromUnderscore(this.TouchpadDown.AttachedTool.ToString(), true, true));
				return;
			}
			if (this.touchpadDownMode == VRTrackedController.TouchpadDownMode.ToolSelect)
			{
				this.SetAction(TrackedControllerButton.PadDown, this.TouchpadDown, VRControlIcon.Tool, "Tool Select");
				return;
			}
			if (this.currentMode != TrackedControllerMode.Grabbed)
			{
				this.SetAction(TrackedControllerButton.PadDown, this.TouchpadDown, VRControlIcon.Zoom, "Examine Touched Object");
				return;
			}
			this.SetAction(TrackedControllerButton.PadDown, this.TouchpadDown, VRControlIcon.None, "");
		}
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x00128D4E File Offset: 0x00126F4E
	public static void RefreshTouchpadIcons()
	{
		if (VRTrackedController.leftVRTrackedController)
		{
			VRTrackedController.leftVRTrackedController.UpdateTouchpadIcons(VRTrackedController.VRKeyCode.None);
		}
		if (VRTrackedController.rightVRTrackedController)
		{
			VRTrackedController.rightVRTrackedController.UpdateTouchpadIcons(VRTrackedController.VRKeyCode.None);
		}
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x00128D80 File Offset: 0x00126F80
	public static void ProcessTouchpadBinding(VRTrackedController.VRKeyCode keyCode, string command)
	{
		if (keyCode > (VRTrackedController.VRKeyCode)32)
		{
			PointerMode pointerMode = PointerMode.None;
			string text = "";
			while (text != null && text != "tool_current")
			{
				text = LibString.bite(ref command, true, ' ', false, false, '\0');
				if (text == "tool_current")
				{
					string text2 = LibString.bite(ref command, false, ' ', false, false, '\0');
					text2 = LibString.CamelCaseFromUnderscore(text2, true, false);
					try
					{
						pointerMode = (PointerMode)Enum.Parse(typeof(PointerMode), text2);
					}
					catch (ArgumentException)
					{
						return;
					}
				}
			}
			if (pointerMode == PointerMode.None)
			{
				return;
			}
			VRTrackedController vrtrackedController = null;
			if (keyCode < (VRTrackedController.VRKeyCode)64)
			{
				if (VRTrackedController.leftVRTrackedController)
				{
					vrtrackedController = VRTrackedController.leftVRTrackedController;
				}
				keyCode -= 32;
			}
			else
			{
				if (VRTrackedController.rightVRTrackedController)
				{
					vrtrackedController = VRTrackedController.rightVRTrackedController;
				}
				keyCode -= 64;
			}
			if (vrtrackedController && vrtrackedController.initialized)
			{
				switch (keyCode)
				{
				case VRTrackedController.VRKeyCode.VRPadWest:
					vrtrackedController.TouchpadLeft.AttachedTool = pointerMode;
					break;
				case VRTrackedController.VRKeyCode.VRPadEast:
					vrtrackedController.TouchpadRight.AttachedTool = pointerMode;
					break;
				case (VRTrackedController.VRKeyCode)3:
					break;
				case VRTrackedController.VRKeyCode.VRPadNorth:
					vrtrackedController.TouchpadUp.AttachedTool = pointerMode;
					break;
				default:
					if (keyCode == VRTrackedController.VRKeyCode.VRPadSouth)
					{
						vrtrackedController.TouchpadDown.AttachedTool = pointerMode;
					}
					break;
				}
				vrtrackedController.UpdateTouchpadIcons(keyCode);
			}
		}
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x00128EB8 File Offset: 0x001270B8
	private void HapticsUpdate()
	{
		TrackedControllerMode currentMode = this.currentMode;
		if (currentMode == TrackedControllerMode.Default)
		{
			if ((this.HoverObject && this.prevHoverObject != this.HoverObject) || (this.HoverNonNetwork && this.prevHoverNonNetwork != this.HoverNonNetwork))
			{
				if (this.LaserHover)
				{
					this.controller.TriggerHapticPulse(0.4f);
				}
				else
				{
					this.controller.TriggerHapticPulse(0.6f);
				}
			}
			this.prevHapticsPoint = Vector3.zero;
			return;
		}
		if (currentMode != TrackedControllerMode.Teleport)
		{
			return;
		}
		if (this.prevHapticsPoint == Vector3.zero)
		{
			this.prevHapticsPoint = this.GetObjectPoint();
		}
		if (Vector3.Distance(this.prevHapticsPoint, this.GetObjectPoint()) > 1f)
		{
			this.prevHapticsPoint = this.GetObjectPoint();
			this.controller.TriggerHapticPulse(0.2f);
		}
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x00128F9C File Offset: 0x0012719C
	private void ResetUIHover()
	{
		if (VRTrackedController.currentUITracked == this)
		{
			UICamera.RemoveTouch(this.Index);
			UICamera.currentTouch = null;
			VRTrackedController.currentUITracked = null;
		}
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x00128FC7 File Offset: 0x001271C7
	public void Teleport(Vector3 Position)
	{
		this.controller.TriggerHapticPulse(2f);
		Singleton<VRHMD>.Instance.Teleport(Position);
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x00128FE4 File Offset: 0x001271E4
	public Vector3 GetObjectPoint()
	{
		if (!this.MovePositionAtLaser)
		{
			return this.WorldPosition();
		}
		return this.laserPointer.HitPoint;
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x00129000 File Offset: 0x00127200
	public Vector3 GetToolPoint()
	{
		if (!this.laserPointer.turnedOn || this.laserPointer.HitPoint.y <= -3f)
		{
			return this.WorldPosition();
		}
		return this.laserPointer.HitPoint;
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x00129038 File Offset: 0x00127238
	private void SetHoverPosition()
	{
		if (this.laserPointer.turnedOn)
		{
			VRTrackedController.HoverPosition = this.laserPointer.HitPoint;
			return;
		}
		VRTrackedController.HoverPosition = this.WorldPosition();
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x00129064 File Offset: 0x00127264
	private bool StartTool()
	{
		if (this.currentPointerMode == PointerMode.Grab || this.currentPointerMode == PointerMode.None)
		{
			return false;
		}
		if (this.currentPointerMode == PointerMode.Flick && !this.HoverObject)
		{
			return false;
		}
		if (Pointer.IsZoneTool(this.currentPointerMode))
		{
			this.SelectionUsingLaser = this.laserPointer.turnedOn;
			PlayerScript.PointerScript.StartZone(PlayerScript.PointerScript.CurrentPointerMode, this.GetToolPoint(), Vector2.zero, this.HoverTrigger);
		}
		else if (Pointer.IsVectorTool(this.currentPointerMode))
		{
			VRTrackedController.ToolAction = true;
			VRTrackedController.ToolActionDown = true;
			VRTrackedController.LastToolController = this;
		}
		else if (Pointer.IsGizmoTool(this.currentPointerMode))
		{
			this.doingGizmoSelect = true;
			VRTrackedController.LastToolController = this;
		}
		else if (Pointer.IsCombineTool(this.currentPointerMode) || this.currentPointerMode == PointerMode.Flick)
		{
			if (!this.HoverObject)
			{
				return false;
			}
			PlayerScript.PointerScript.StartLine(this.GetToolPoint(), this.HoverObject);
		}
		else if (Pointer.IsSnapTool(this.currentPointerMode))
		{
			VRTrackedController.StaticHoverObject = (this.HoverObject ? this.HoverObject.gameObject : null);
			VRTrackedController.StaticSurfaceObject = (this.SurfaceObject ? this.SurfaceObject : null);
			NetworkSingleton<SnapPointManager>.Instance.CreateSnapPoint(this.laserPointer.turnedOn ? this.laserPointer.HitPoint : this.WorldPosition());
		}
		PointerMode pointerMode = this.currentPointerMode;
		if (pointerMode != PointerMode.Line)
		{
			if (pointerMode == PointerMode.Text)
			{
				int objectId = -1;
				if (this.HoverLockObject != null)
				{
					objectId = this.HoverLockObject.GetComponent<NetworkPhysicsObject>().ID;
				}
				Vector3 vector = this.laserPointer.turnedOn ? this.laserPointer.HitPoint : this.WorldPosition();
				Vector3 eulerAngles = Quaternion.LookRotation(vector - this.HMD.transform.position).eulerAngles;
				eulerAngles.x = 90f;
				PlayerScript.PointerScript.SpawnText(vector, eulerAngles, objectId);
				return false;
			}
			if (pointerMode == PointerMode.Decal)
			{
				VRTrackedController.ToolActionDown = true;
				VRTrackedController.ToolAction = true;
				VRTrackedController.LastToolController = this;
				VRTrackedController.StaticHoverLockObject = (this.HoverLockObject ? this.HoverLockObject.gameObject : null);
			}
		}
		else
		{
			PlayerScript.PointerScript.StartLine(this.GetToolPoint(), this.HoverObject);
		}
		this.SetHoverPosition();
		return true;
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x001292CC File Offset: 0x001274CC
	private bool UpdateTool()
	{
		if (this.currentPointerMode == PointerMode.Grab || this.currentPointerMode == PointerMode.None)
		{
			return false;
		}
		if (Pointer.IsCombineTool(this.currentPointerMode) || this.currentPointerMode == PointerMode.Flick || this.currentPointerMode == PointerMode.Line)
		{
			PlayerScript.PointerScript.UpdateLine(this.GetToolPoint());
		}
		else if (Pointer.IsZoneTool(this.currentPointerMode))
		{
			PlayerScript.PointerScript.UpdateZone(PlayerScript.PointerScript.CurrentPointerMode, this.GetToolPoint(), Vector2.zero, this.SelectionUsingLaser);
		}
		this.SetHoverPosition();
		return true;
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x00129358 File Offset: 0x00127558
	private bool EndTool()
	{
		if (this.currentPointerMode == PointerMode.Grab || this.currentPointerMode == PointerMode.None)
		{
			return false;
		}
		if (Pointer.IsCombineTool(this.currentPointerMode) || this.currentPointerMode == PointerMode.Flick || this.currentPointerMode == PointerMode.Line)
		{
			PlayerScript.PointerScript.EndLine(this.GetToolPoint(), this.HoverObject);
		}
		else if (Pointer.IsZoneTool(this.currentPointerMode))
		{
			PlayerScript.PointerScript.EndZone(PlayerScript.PointerScript.CurrentPointerMode);
		}
		else if (Pointer.IsGizmoTool(this.currentPointerMode) || Pointer.IsVectorTool(this.currentPointerMode) || this.currentPointerMode == PointerMode.Decal)
		{
			VRTrackedController.ToolActionUp = true;
			VRTrackedController.ToolAction = false;
		}
		this.SetHoverPosition();
		return true;
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x0012940C File Offset: 0x0012760C
	private bool Grab(bool virtualCollision = false)
	{
		if (!PlayerScript.Pointer || this.currentPointerMode != PointerMode.Grab)
		{
			return false;
		}
		if (!this.HoverLockObject || this.HoverLockObject.HeldByPlayerID == NetworkID.ID)
		{
			return false;
		}
		this.MovePositionAtLaser = this.LaserHover;
		if (this.HoverLockObject.stackObject || this.HoverLockObject.deckScript)
		{
			this.grabStackCoroutine = base.StartCoroutine(this.CheckGrabStack());
			return true;
		}
		return this.pointerGrab(this.HoverObject, virtualCollision);
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x001294A4 File Offset: 0x001276A4
	private bool pointerGrab(NetworkPhysicsObject grabObject, bool virtualCollision = false)
	{
		if (!grabObject || grabObject.IsLocked)
		{
			return false;
		}
		if (!PlayerScript.PointerScript.HighLightedObjects.Contains(grabObject))
		{
			PlayerScript.PointerScript.ResetHighlight();
		}
		if (virtualCollision)
		{
			PlayerScript.PointerScript.Grab(grabObject.ID, new Vector3?(Singleton<VRVirtualHandObject>.Instance.ActualPositionFromVirtualHandPosition(this.GetObjectPoint())), this.Index, this.MovePositionAtLaser, new Vector3?(Singleton<VRVirtualHandObject>.Instance.ActualRotationFromVirtualHandRotation(base.gameObject.transform.rotation).eulerAngles));
		}
		else if (this.grabbingFromSearch)
		{
			PlayerScript.PointerScript.Grab(grabObject.ID, null, this.Index, this.MovePositionAtLaser, null);
			this.grabbingFromSearch = false;
		}
		else
		{
			PlayerScript.PointerScript.Grab(grabObject.ID, new Vector3?(this.GetObjectPoint()), this.Index, this.MovePositionAtLaser, null);
		}
		return true;
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x001295AC File Offset: 0x001277AC
	private IEnumerator CheckGrabStack()
	{
		float HoverSameObjectTime = 0f;
		this.potentialStackOrDeckGrabNPO = this.HoverLockObject;
		while (this.prevHoverObject == this.potentialStackOrDeckGrabNPO && HoverSameObjectTime < 0.4f)
		{
			yield return null;
			HoverSameObjectTime += Time.deltaTime;
		}
		if (!this.potentialStackOrDeckGrabNPO)
		{
			yield break;
		}
		if (this.prevHoverObject != this.potentialStackOrDeckGrabNPO)
		{
			if (this.potentialStackOrDeckGrabNPO.stackObject)
			{
				if (!Network.isServer)
				{
					PlayerScript.PointerScript.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(PlayerScript.PointerScript.TellObjectManagerAboutObjectTake), this.potentialStackOrDeckGrabNPO.ID, PlayerScript.PointerScript.ID, this.Index);
				}
				else
				{
					PlayerScript.PointerScript.TellObjectManagerAboutObjectTake(this.potentialStackOrDeckGrabNPO.ID, PlayerScript.PointerScript.ID, this.Index);
				}
			}
			else if (this.potentialStackOrDeckGrabNPO.deckScript)
			{
				if (!Network.isServer)
				{
					PlayerScript.PointerScript.networkView.RPC<int, int, int>(RPCTarget.Server, new Action<int, int, int>(PlayerScript.PointerScript.TellObjectManagerAboutCardPeel), this.potentialStackOrDeckGrabNPO.ID, PlayerScript.PointerScript.ID, this.Index);
				}
				else
				{
					PlayerScript.PointerScript.TellObjectManagerAboutCardPeel(this.potentialStackOrDeckGrabNPO.ID, NetworkID.ID, this.Index);
				}
			}
		}
		else
		{
			this.pointerGrab(this.potentialStackOrDeckGrabNPO, false);
		}
		this.potentialStackOrDeckGrabNPO = null;
		yield break;
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x001295BB File Offset: 0x001277BB
	private void Release()
	{
		if (!PlayerScript.Pointer)
		{
			return;
		}
		PlayerScript.PointerScript.Release(NetworkID.ID, -1, this.Index, -1);
		this.MovePositionAtLaser = false;
		if (VRTrackedController.HIDE_VIRTUAL_HAND_WHEN_GRABBING)
		{
			Singleton<VRVirtualHand>.Instance.ToggleVirtualHand(true, this);
		}
	}

	// Token: 0x060029C1 RID: 10689 RVA: 0x001295FC File Offset: 0x001277FC
	private void StartSelection()
	{
		if (this.laserPointer.turnedOn)
		{
			this.StartSelectionVector = this.laserPointer.HitPoint;
			this.SelectionUsingLaser = true;
		}
		else
		{
			this.StartSelectionVector = this.WorldPosition();
			this.SelectionUsingLaser = false;
		}
		Transform transform = this.HMD.transform;
		Quaternion rotation = transform.rotation;
		transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
		this.StartSelectionCameraRightNorm = transform.right;
		this.StartSelectionCameraRotation = transform.eulerAngles;
		transform.rotation = rotation;
		this.UpdateSelection();
		this.SelectionTrigger.SetActive(true);
		this.SelectionMaterial.color = new Color(this.pointerColor.r, this.pointerColor.g, this.pointerColor.b, this.SelectionMaterial.color.a);
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x001296E8 File Offset: 0x001278E8
	private void UpdateSelection()
	{
		Vector3 startSelectionCameraRightNorm = this.StartSelectionCameraRightNorm;
		Vector3 vector;
		if (this.SelectionUsingLaser)
		{
			vector = this.laserPointer.HitPoint;
		}
		else
		{
			vector = this.WorldPosition();
		}
		float num = vector.x - this.StartSelectionVector.x;
		float num2 = vector.z - this.StartSelectionVector.z;
		float x = startSelectionCameraRightNorm.x * num + startSelectionCameraRightNorm.z * num2;
		float z = startSelectionCameraRightNorm.x * num2 - startSelectionCameraRightNorm.z * num;
		float num3 = VRTrackedController.SELECTION_HEIGHT;
		if (!this.SelectionUsingLaser)
		{
			if (VRTrackedController.VRSelectionMode == VRTrackedController.VRSelectionStyle.Exact)
			{
				num3 = vector.y - this.StartSelectionVector.y;
			}
			else if (VRTrackedController.VRSelectionMode == VRTrackedController.VRSelectionStyle.Anchored)
			{
				num3 = -this.StartSelectionVector.y;
			}
			else
			{
				num3 = -num3;
			}
		}
		Vector3 localScale = new Vector3(x, num3, z);
		this.SelectionTrigger.transform.localScale = localScale;
		this.SelectionTrigger.transform.eulerAngles = new Vector3(0f, this.StartSelectionCameraRotation.y, 0f);
		this.SelectionTrigger.transform.position = new Vector3((vector.x + this.StartSelectionVector.x) / 2f, this.StartSelectionVector.y + num3 / 2f, (vector.z + this.StartSelectionVector.z) / 2f);
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x00129851 File Offset: 0x00127A51
	private void EndSelection()
	{
		this.SelectionTrigger.SetActive(false);
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x00129860 File Offset: 0x00127A60
	private void ToggleZoomObject(NetworkPhysicsObject objectToMagnify)
	{
		UnityEngine.Object.Destroy(this.DummySpawnObject);
		bool flag = objectToMagnify && !objectToMagnify.CanBePeeked;
		if (objectToMagnify == null || objectToMagnify.CompareTag("Board") || flag)
		{
			this.ZoomObject.SetActive(false);
			if (this.currentMode == TrackedControllerMode.Zoom)
			{
				this.currentMode = TrackedControllerMode.Default;
			}
			Singleton<VRVirtualHand>.Instance.ToggleVirtualHand(true, this);
			NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject = null;
			return;
		}
		this.currentZoomObject = objectToMagnify;
		NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject = objectToMagnify.gameObject;
		this.currentMode = TrackedControllerMode.Zoom;
		this.ZoomObjectFacingMultiplier = ((this.currentZoomObject.transform.up.normalized.y <= 0f) ? 1 : -1);
		if (VRTrackedController.ALIGN_ZOOMED_OBJECT && VRTrackedController.ControllerStyle == TrackedControllerStyle.New)
		{
			if (objectToMagnify.CanBeHeldInHand || objectToMagnify.deckScript)
			{
				this.ZoomObject.transform.LookAt(this.HMD.transform);
				if (this.ZoomObjectFacingMultiplier == 1)
				{
					this.ZoomObject.transform.Rotate(Vector3.right, -90f);
					this.ZoomObject.transform.Rotate(Vector3.up, 180f);
				}
				else
				{
					this.ZoomObject.transform.Rotate(Vector3.right, 90f);
				}
				if (this.peeking)
				{
					this.ZoomObject.transform.Rotate(Vector3.forward, 180f);
				}
				if ((objectToMagnify.deckScript && objectToMagnify.deckScript.bSideways) || (objectToMagnify.cardScript && objectToMagnify.cardScript.bSideways))
				{
					this.ZoomObject.transform.Rotate(Vector3.up, -90f);
				}
			}
			else if (objectToMagnify.HasRotationsValues())
			{
				this.ZoomObject.transform.eulerAngles = objectToMagnify.GetRotationValue(false).rotation;
				this.ZoomObjectFacingMultiplier = 0;
			}
			else
			{
				this.ZoomObject.transform.rotation = objectToMagnify.transform.rotation;
				if (this.peeking)
				{
					this.ZoomObject.transform.Rotate(Vector3.forward, 180f);
				}
			}
		}
		this.ZoomObject.SetActive(true);
		ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(objectToMagnify);
		objectState.Transform.posY = 10000f;
		this.DummySpawnObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(objectState, true, false);
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.New)
		{
			this.DummyRotation = Vector3.zero;
		}
		else
		{
			bool flag2 = (objectToMagnify.deckScript && objectToMagnify.deckScript.bSideways) || (objectToMagnify.cardScript && objectToMagnify.cardScript.bSideways);
			this.DummyRotation = new Vector3(objectToMagnify.transform.eulerAngles.x, (float)(flag2 ? 90 : 180), objectToMagnify.transform.eulerAngles.z);
			this.ZoomObjectFacingMultiplier = -1;
		}
		this.DummyScale = this.DummySpawnObject.transform.localScale;
		this.EventManager_OnDummyObjectFinish(this.DummySpawnObject);
		Singleton<VRVirtualHand>.Instance.ToggleVirtualHand(false, this);
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x00129BA4 File Offset: 0x00127DA4
	private void EventManager_OnDummyObjectFinish(GameObject dummyGameObject)
	{
		if (dummyGameObject != this.DummySpawnObject)
		{
			return;
		}
		Collider[] componentsInChildren = dummyGameObject.GetComponentsInChildren<Collider>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.DestroyImmediate(componentsInChildren[i]);
		}
		this.DummySpawnObject.transform.parent = null;
		this.DummySpawnObject.transform.eulerAngles = this.DummyRotation;
		this.DummySpawnObject.transform.localScale = this.DummyScale;
		Vector3 a;
		Bounds combinedBounds = Utilities.GetCombinedBounds(this.DummySpawnObject, out a);
		float num = combinedBounds.size.x;
		if (num < combinedBounds.size.z)
		{
			num = combinedBounds.size.z;
		}
		if (num < combinedBounds.size.y * 2f)
		{
			num = combinedBounds.size.y * 2f;
		}
		float d = 20f / num;
		this.DummySpawnObject.transform.localScale = this.DummySpawnObject.transform.localScale * d * this.ZoomObject.transform.localScale.x;
		this.DummySpawnObject.transform.parent = this.ZoomObject.transform;
		this.DummySpawnObject.transform.localPosition = a / base.transform.lossyScale.x * d;
		this.DummySpawnObject.transform.localEulerAngles = this.DummyRotation;
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x00129D27 File Offset: 0x00127F27
	private void SetGrabSphereColor(Colour colour)
	{
		colour = colour.WithAlpha(VRTrackedController.GrabSphereAlpha);
		if (this.GrabSphereMaterial.color != colour)
		{
			this.GrabSphereMaterial.color = colour;
		}
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x00129D5C File Offset: 0x00127F5C
	private void TouchChanged(TouchPhase touchPhase, bool OverridePressFalse = false)
	{
		UICamera.currentScheme = UICamera.ControlScheme.Touch;
		UICamera.currentTouchID = this.Index;
		UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID, true);
		bool flag = touchPhase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
		bool flag2 = touchPhase == TouchPhase.Canceled || touchPhase == TouchPhase.Ended;
		UICamera.currentTouch.touchBegan = false;
		if (OverridePressFalse)
		{
			flag = false;
			flag2 = false;
		}
		VRTrackedController.lastTouchController = this;
		Vector2 uihitPoint = this.laserPointer.UIHitPoint;
		Touch touch = new Touch
		{
			fingerId = this.Index,
			position = uihitPoint,
			deltaPosition = uihitPoint - this.prevUIPosition,
			deltaTime = Time.time - this.lastUITime,
			phase = touchPhase
		};
		StandaloneInputModuleV2.VirtualTouch virtualTouch = new StandaloneInputModuleV2.VirtualTouch
		{
			forceNoPress = OverridePressFalse,
			touch = touch
		};
		EventSystem.current.pixelDragThreshold = 40;
		StandaloneInputModuleV2 standaloneInputModuleV = EventSystem.current.currentInputModule as StandaloneInputModuleV2;
		if (standaloneInputModuleV)
		{
			standaloneInputModuleV.ProcessVirtualTouchEvent(virtualTouch);
		}
		if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
		{
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			return;
		}
		if (flag)
		{
			UICamera.currentTouch.delta = Vector2.zero;
		}
		else
		{
			UICamera.currentTouch.delta = uihitPoint - this.prevUIPosition;
		}
		UICamera.currentTouch.pos = uihitPoint;
		this.prevUIPosition = uihitPoint;
		this.lastUITime = Time.time;
		UICamera.CalculateHoveredUIObject();
		bool flag3 = UICamera.Raycast(UICamera.currentTouch.pos);
		UICamera.currentTouch.current = (flag3 ? UICamera.HoveredUIObject : UICamera.fallThrough);
		UICamera.hoveredObject = (flag3 ? UICamera.HoveredUIObject : UICamera.fallThrough);
		if (UICamera.hoveredObject == null)
		{
			UICamera.hoveredObject = UICamera.genericEventHandler;
		}
		UICamera.currentTouch.current = UICamera.hoveredObject;
		UICamera.lastEventPosition = UICamera.currentTouch.pos;
		if (touchPhase == TouchPhase.Canceled && UICamera.hoveredObject != null)
		{
			UICamera.Notify(UICamera.hoveredObject, "OnHover", false);
		}
		if (flag)
		{
			UICamera.currentTouch.pressedCam = UICamera.currentCamera;
		}
		else if (UICamera.currentTouch.pressed != null)
		{
			UICamera.currentCamera = UICamera.currentTouch.pressedCam;
		}
		this.uiCamera.ProcessTouch(flag, flag2, 0);
		if (flag2)
		{
			UICamera.RemoveTouch(UICamera.currentTouchID);
		}
		UICamera.currentTouch.touchBegan = false;
		UICamera.currentTouch.last = null;
		UICamera.currentTouch = null;
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x00129FF0 File Offset: 0x001281F0
	private void SetColor(Color color)
	{
		if (this.controllerColor != color)
		{
			this.controllerColor = color;
			Renderer[] componentsInChildren = this.Model.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.color = color;
			}
		}
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x0012A03A File Offset: 0x0012823A
	public Vector3 WorldPosition()
	{
		return this.Tip.position;
	}

	// Token: 0x060029CA RID: 10698 RVA: 0x0012A047 File Offset: 0x00128247
	public Vector3 InteractionPoint()
	{
		if (this.laserPointer.turnedOn)
		{
			return this.laserPointer.HitPoint;
		}
		return this.WorldPosition();
	}

	// Token: 0x060029CB RID: 10699 RVA: 0x0012A068 File Offset: 0x00128268
	public static VRTrackedController VRTrackedControllerFromIndex(int index)
	{
		if (!XRSettings.enabled)
		{
			return null;
		}
		if (VRTrackedController.leftVRTrackedController.Index == index)
		{
			return VRTrackedController.leftVRTrackedController;
		}
		if (VRTrackedController.rightVRTrackedController.Index == index)
		{
			return VRTrackedController.rightVRTrackedController;
		}
		return null;
	}

	// Token: 0x060029CC RID: 10700 RVA: 0x0012A09C File Offset: 0x0012829C
	public void DisplayTooltips(bool visible, bool now = false)
	{
		if (!this.initialized)
		{
			return;
		}
		if (now)
		{
			float a;
			if (visible)
			{
				a = 1f;
			}
			else
			{
				a = 0f;
			}
			for (int i = 0; i < this.Tooltips.Length; i++)
			{
				Material material = this.Tooltips[i].GetComponent<Renderer>().material;
				Color color = material.color;
				if (Network.peerType == NetworkPeerMode.Disconnected)
				{
					if (i == 0 || i == 8 || i == 9)
					{
						color.a = 0f;
					}
					else if (i == 3 || i == 13)
					{
						if (VRTrackedController.DISPLAY_CLICK_TOOLTIP_ON_MENU && this.shouldDisplayClickTooltip)
						{
							color.a = 1f;
						}
						else
						{
							color.a = a;
						}
					}
					else
					{
						color.a = a;
					}
				}
				else
				{
					color.a = a;
				}
				material.color = color;
				this.Tooltips[i].SetActive(color.a != 0f);
			}
			this.displayingTooltips = visible;
			return;
		}
		if (visible && !this.displayingTooltips)
		{
			for (int j = 0; j < this.Tooltips.Length; j++)
			{
				this.Tooltips[j].SetActive(true);
			}
		}
		if (this.displayingTooltipsFadeStartTime == 0f)
		{
			if (visible != this.displayingTooltips)
			{
				this.displayingTooltipsFadeStartTime = Time.time;
				if (visible)
				{
					this.displayingTooltipsFadeFromAlpha = 0f;
					this.displayingTooltipsFadeToAlpha = 1f;
				}
				else
				{
					this.displayingTooltipsFadeFromAlpha = 1f;
					this.displayingTooltipsFadeToAlpha = 0f;
				}
			}
		}
		else if ((visible && this.displayingTooltipsFadeToAlpha == 0f) || (!visible && this.displayingTooltipsFadeFromAlpha == 0f))
		{
			float num = this.displayingTooltipsFadeFromAlpha;
			this.displayingTooltipsFadeFromAlpha = this.displayingTooltipsFadeToAlpha;
			this.displayingTooltipsFadeToAlpha = num;
			num = Time.time + (Time.time - this.displayingTooltipsFadeStartTime);
			this.displayingTooltipsFadeStartTime = num - 0.5f;
		}
		if (visible)
		{
			this.displayingTooltips = true;
		}
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x0012A274 File Offset: 0x00128474
	private void TouchpadEvent(VRTrackedController.VRKeyCode keycode, VRTrackedController.VRKeyEvent padevent)
	{
		if (keycode < (VRTrackedController.VRKeyCode)32)
		{
			if (this == VRTrackedController.leftVRTrackedController)
			{
				keycode = (VRTrackedController.VRKeyCode)32 + (int)keycode;
			}
			else
			{
				keycode = (VRTrackedController.VRKeyCode)64 + (int)keycode;
			}
		}
		if (padevent == VRTrackedController.VRKeyEvent.Pressed)
		{
			this.lastPadPress = keycode;
			this.lastPadPressAt = Time.time;
			Singleton<SystemConsole>.Instance.VRPadEvent(keycode, VRTrackedController.VRKeyEvent.Pressed);
			return;
		}
		if (padevent == VRTrackedController.VRKeyEvent.Released)
		{
			if (keycode == this.lastPadPress)
			{
				Singleton<SystemConsole>.Instance.VRPadEvent(keycode, VRTrackedController.VRKeyEvent.Released);
			}
			this.lastPadPress = VRTrackedController.VRKeyCode.None;
			return;
		}
		if (padevent == VRTrackedController.VRKeyEvent.LongPress)
		{
			if (keycode == this.lastPadPress)
			{
				if (Singleton<SystemConsole>.Instance.VRPadEvent(keycode, VRTrackedController.VRKeyEvent.LongPress))
				{
					this.lastPadPress = VRTrackedController.VRKeyCode.None;
					this.controller.TriggerHapticPulse(0.6f);
				}
				this.lastPadPressAt = 0f;
				return;
			}
		}
		else if (padevent == VRTrackedController.VRKeyEvent.Held)
		{
			if (keycode != this.lastPadPress)
			{
				this.lastPadPress = VRTrackedController.VRKeyCode.None;
				return;
			}
			if (this.lastPadPressAt != 0f && Time.time > this.lastPadPressAt + 1f)
			{
				this.TouchpadEvent(keycode, VRTrackedController.VRKeyEvent.LongPress);
			}
		}
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x0012A361 File Offset: 0x00128561
	public static void LaserBeamVisible(bool visible)
	{
		VRTrackedController.LASER_BEAM_VISIBLE = visible;
		if (VRTrackedController.leftVRTrackedController)
		{
			VRTrackedController.leftVRTrackedController.laserPointer.UpdateLaserVisiblity();
		}
		if (VRTrackedController.rightVRTrackedController)
		{
			VRTrackedController.rightVRTrackedController.laserPointer.UpdateLaserVisiblity();
		}
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x0012A3A0 File Offset: 0x001285A0
	public static void UpdateToolIconColor()
	{
		Colour colour = Colour.UnityBlack;
		if (VRTrackedController.TOOL_ICON_COLORED)
		{
			colour = Colour.UnityWhite;
		}
		if (VRTrackedController.leftVRTrackedController && VRTrackedController.leftVRTrackedController.CurrentTool)
		{
			VRTrackedController.leftVRTrackedController.CurrentTool.SetInitialColour(colour, null);
		}
		if (VRTrackedController.rightVRTrackedController && VRTrackedController.rightVRTrackedController.CurrentTool)
		{
			VRTrackedController.rightVRTrackedController.CurrentTool.SetInitialColour(colour, null);
		}
	}

	// Token: 0x060029D0 RID: 10704 RVA: 0x0012A42C File Offset: 0x0012862C
	public void SetZoomScale(float scale)
	{
		this.ZoomObject.transform.Reset();
		this.ZoomObject.transform.localScale *= scale;
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x0012A45A File Offset: 0x0012865A
	public float GetZoomScale()
	{
		return this.ZoomObject.transform.localScale.x;
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x0012A471 File Offset: 0x00128671
	private void Click3DUI(GameObject ui)
	{
		ui.SendMessage("OnClick");
		if (ui.GetComponent<Selectable>())
		{
			ExecuteEvents.Execute<IPointerClickHandler>(ui, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
		}
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x0012A4A4 File Offset: 0x001286A4
	private void ActOnSelectedObjects(VRTrackedController.SelectedObjectFunction function)
	{
		List<GameObject> selectedObjects = PlayerScript.PointerScript.GetSelectedObjects(-1, true, true);
		bool flag = false;
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component)
				{
					function(component);
					if (component == this.HoverObject)
					{
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			function(this.HoverObject);
		}
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x0012A538 File Offset: 0x00128738
	private bool CanGroupSelectedObjects()
	{
		return this.HoverObject && (this.HoverObject.cardScript || this.HoverObject.GetComponent<CheckStackObject>());
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x0012A56D File Offset: 0x0012876D
	public void GrabSearchInventoryObject()
	{
		this.grabNextObjectUntil = Time.time + 2f;
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x0012A580 File Offset: 0x00128780
	public void SetHoverText(string text)
	{
		if (this.UIHoverText)
		{
			this.UIHoverText.text = Utilities.NewlineBreakupString(text, VRTrackedController.TOOLTIP_NEWLINE_CHARS);
		}
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x0012A5A5 File Offset: 0x001287A5
	public void UpdateHoverText(bool force = false)
	{
		if (this.UIHoverText)
		{
			this.UIHoverText.GetComponent<Renderer>().enabled = (VRTrackedController.UI_HOVER_TOOLTIPS || force);
		}
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x0012A5CB File Offset: 0x001287CB
	public static void SetBothHoverText(string text)
	{
		if (VRTrackedController.leftVRTrackedController)
		{
			VRTrackedController.leftVRTrackedController.SetHoverText(text);
		}
		if (VRTrackedController.rightVRTrackedController)
		{
			VRTrackedController.rightVRTrackedController.SetHoverText(text);
		}
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x0012A5FB File Offset: 0x001287FB
	public static void UpdateBothHoverText(bool force = false)
	{
		if (VRTrackedController.leftVRTrackedController)
		{
			VRTrackedController.leftVRTrackedController.UpdateHoverText(force);
		}
		if (VRTrackedController.rightVRTrackedController)
		{
			VRTrackedController.rightVRTrackedController.UpdateHoverText(force);
		}
	}

	// Token: 0x04001BC7 RID: 7111
	public static float HIDE_TOOLTIPS_DELAY = 5f;

	// Token: 0x04001BC8 RID: 7112
	public static bool HIDE_GEM_ON_GRAB = true;

	// Token: 0x04001BC9 RID: 7113
	public static bool UI_HOVER_TOOLTIPS = true;

	// Token: 0x04001BCA RID: 7114
	public static bool ENABLE_TOOLTIP_ACTION = true;

	// Token: 0x04001BCB RID: 7115
	public static bool ALWAYS_DISPLAY_TOOLTIPS = false;

	// Token: 0x04001BCC RID: 7116
	public static bool DISPLAY_CLICK_TOOLTIP_ON_MENU = true;

	// Token: 0x04001BCD RID: 7117
	public static bool STICKY_GRAB = false;

	// Token: 0x04001BCE RID: 7118
	public static bool TOUCHPAD_ICONS_ALWAYS_ON = false;

	// Token: 0x04001BCF RID: 7119
	public static bool LASER_BEAM_VISIBLE = true;

	// Token: 0x04001BD0 RID: 7120
	public static float LASER_BEAM_ALPHA = 1f;

	// Token: 0x04001BD1 RID: 7121
	public static float REPEAT_DURATION = 0.25f;

	// Token: 0x04001BD2 RID: 7122
	public static bool HIDE_VIRTUAL_HAND_WHEN_GRABBING = true;

	// Token: 0x04001BD3 RID: 7123
	public static float ORIENT_OBJECT_DELAY = 0.2f;

	// Token: 0x04001BD4 RID: 7124
	public static bool ALIGN_ZOOMED_OBJECT = true;

	// Token: 0x04001BD5 RID: 7125
	public static bool RIGHT_CONTROLLER_HOTKEYS = true;

	// Token: 0x04001BD6 RID: 7126
	public static bool LEFT_CONTROLLER_HOTKEYS = true;

	// Token: 0x04001BD7 RID: 7127
	public static float SELECTION_HEIGHT = 7f;

	// Token: 0x04001BD8 RID: 7128
	public static bool THROW_MOVE = true;

	// Token: 0x04001BD9 RID: 7129
	public static float THROW_FRICTION = 0.9f;

	// Token: 0x04001BDA RID: 7130
	private const float GEM_ROTATION_RATE = 0.1f;

	// Token: 0x04001BDB RID: 7131
	private const float TOOLTIP_DISPLAY_ALPHA = 1f;

	// Token: 0x04001BDC RID: 7132
	private const float TOOLTIP_DISPLAY_FADE_DURATION = 0.5f;

	// Token: 0x04001BDD RID: 7133
	private const float DELAY_FOR_HOLD_EFFECT = 1f;

	// Token: 0x04001BDE RID: 7134
	private const float BUTTON_PRESS_RADIUS = 0.5f;

	// Token: 0x04001BDF RID: 7135
	private const float ZOOM_SCALE_MULTIPLIER = 1.75f;

	// Token: 0x04001BE0 RID: 7136
	private const float GRAB_NEXT_OBJECT_TIMEOUT = 2f;

	// Token: 0x04001BE1 RID: 7137
	public static VRTrackedController.VRSelectionStyle VRSelectionMode = VRTrackedController.VRSelectionStyle.Fixed;

	// Token: 0x04001BE3 RID: 7139
	private TrackedControllerMode previousMode;

	// Token: 0x04001BE4 RID: 7140
	private static TrackedControllerStyle _controllerStyle = TrackedControllerStyle.New;

	// Token: 0x04001BE5 RID: 7141
	public static bool ViveHaptics = true;

	// Token: 0x04001BE6 RID: 7142
	public static float LASER_ANGLE_ORIGINAL = 58f;

	// Token: 0x04001BE7 RID: 7143
	private static float _LASER_ANGLE = 0f;

	// Token: 0x04001BE8 RID: 7144
	public static bool LASER_ALWAYS_ON = false;

	// Token: 0x04001BE9 RID: 7145
	public static bool TOOL_ICON_COLORED = true;

	// Token: 0x04001BEA RID: 7146
	public static bool ALWAYS_ANGLE_LASER = false;

	// Token: 0x04001BEB RID: 7147
	public static bool UP_IS_TELEPORT = false;

	// Token: 0x04001BEC RID: 7148
	public static bool ToolAction = false;

	// Token: 0x04001BED RID: 7149
	public static bool ToolActionDown = false;

	// Token: 0x04001BEE RID: 7150
	public static bool ToolActionUp = false;

	// Token: 0x04001BEF RID: 7151
	public static VRTrackedController LastToolController = null;

	// Token: 0x04001BF0 RID: 7152
	public static Vector3 HoverPosition = Vector3.zero;

	// Token: 0x04001BF1 RID: 7153
	public static GameObject StaticHoverObject;

	// Token: 0x04001BF2 RID: 7154
	public static GameObject StaticHoverLockObject;

	// Token: 0x04001BF3 RID: 7155
	public static GameObject StaticSurfaceObject;

	// Token: 0x04001BF4 RID: 7156
	private GameObject SurfaceObject;

	// Token: 0x04001BF5 RID: 7157
	public VRSteamControllerDevice controller;

	// Token: 0x04001BF6 RID: 7158
	public SteamVR_TrackedObject trackedObject;

	// Token: 0x04001BF7 RID: 7159
	public VRLaserPointer laserPointer;

	// Token: 0x04001BF8 RID: 7160
	public GameObject GrabSphere;

	// Token: 0x04001BF9 RID: 7161
	public Transform Tip;

	// Token: 0x04001BFA RID: 7162
	public Vector3 GrabSpherePositionStraight;

	// Token: 0x04001BFB RID: 7163
	public Vector3 GrabSpherePositionAngled;

	// Token: 0x04001BFC RID: 7164
	public Vector3 Velocity;

	// Token: 0x04001BFD RID: 7165
	public Vector3 LastPosition;

	// Token: 0x04001BFE RID: 7166
	public Quaternion AngularVelocity;

	// Token: 0x04001BFF RID: 7167
	public Quaternion LastRotation;

	// Token: 0x04001C00 RID: 7168
	private bool inVirtualHand;

	// Token: 0x04001C01 RID: 7169
	private bool previouslyInVirtualHand;

	// Token: 0x04001C02 RID: 7170
	private bool peeking;

	// Token: 0x04001C03 RID: 7171
	private bool previouslyPeeking;

	// Token: 0x04001C04 RID: 7172
	private UICamera uiCamera;

	// Token: 0x04001C05 RID: 7173
	public GameObject UI;

	// Token: 0x04001C06 RID: 7174
	public UILabel TextTooltip;

	// Token: 0x04001C07 RID: 7175
	public Transform VRCameraRig;

	// Token: 0x04001C08 RID: 7176
	public SteamVR_TrackedObject HMD;

	// Token: 0x04001C09 RID: 7177
	private float RotateTimeHolder;

	// Token: 0x04001C0A RID: 7178
	private Material GrabSphereMaterial;

	// Token: 0x04001C0B RID: 7179
	public NetworkPhysicsObject HoverObject;

	// Token: 0x04001C0C RID: 7180
	public NetworkPhysicsObject HoverLockObject;

	// Token: 0x04001C0D RID: 7181
	private NetworkPhysicsObject prevHoverObject;

	// Token: 0x04001C0E RID: 7182
	private NetworkPhysicsObject prevHoverLockObject;

	// Token: 0x04001C0F RID: 7183
	private NetworkPhysicsObject potentialStackOrDeckGrabNPO;

	// Token: 0x04001C10 RID: 7184
	public GameObject HoverTrigger;

	// Token: 0x04001C11 RID: 7185
	private int SelectedObjectCountWhenOnStackable;

	// Token: 0x04001C12 RID: 7186
	private GameObject prevHoverUIObject;

	// Token: 0x04001C13 RID: 7187
	public GameObject HoverUIObject;

	// Token: 0x04001C14 RID: 7188
	private GameObject PreviousUI3DHitObject;

	// Token: 0x04001C15 RID: 7189
	public GameObject UI3DHitObject;

	// Token: 0x04001C16 RID: 7190
	private GameObject prevHoverNonNetwork;

	// Token: 0x04001C17 RID: 7191
	private GameObject HoverNonNetwork;

	// Token: 0x04001C18 RID: 7192
	private GameObject GrabbedNonNetwork;

	// Token: 0x04001C19 RID: 7193
	private Transform GrabbedNonNetworkParent;

	// Token: 0x04001C1A RID: 7194
	private bool justFlipped;

	// Token: 0x04001C1B RID: 7195
	private float lastGrabTime;

	// Token: 0x04001C1C RID: 7196
	private bool MovePositionAtLaser;

	// Token: 0x04001C1D RID: 7197
	public static float RenderAlpha = 1f;

	// Token: 0x04001C1E RID: 7198
	public static float IconAlpha = 1f;

	// Token: 0x04001C1F RID: 7199
	public static float GrabSphereAlpha = 1f;

	// Token: 0x04001C20 RID: 7200
	private Vector2 prevUIPosition = Vector3.zero;

	// Token: 0x04001C21 RID: 7201
	private GameObject SelectionTrigger;

	// Token: 0x04001C22 RID: 7202
	private Material SelectionMaterial;

	// Token: 0x04001C23 RID: 7203
	private bool doingGizmoSelect;

	// Token: 0x04001C24 RID: 7204
	private bool LaserHover;

	// Token: 0x04001C25 RID: 7205
	public GameObject ZoomObject;

	// Token: 0x04001C26 RID: 7206
	private int ZoomObjectFacingMultiplier;

	// Token: 0x04001C27 RID: 7207
	private GameObject TeleportLine;

	// Token: 0x04001C28 RID: 7208
	public GameObject Model;

	// Token: 0x04001C29 RID: 7209
	private VRRenderModel renderModel;

	// Token: 0x04001C2A RID: 7210
	public static bool HasAnalogStick = false;

	// Token: 0x04001C2B RID: 7211
	public PointerMode currentPointerMode;

	// Token: 0x04001C2C RID: 7212
	private SphereCollider sphereCollider;

	// Token: 0x04001C2D RID: 7213
	public static VRTrackedController currentUITracked = null;

	// Token: 0x04001C2E RID: 7214
	private VRTrackedController otherController;

	// Token: 0x04001C2F RID: 7215
	public static VRTrackedController leftVRTrackedController;

	// Token: 0x04001C30 RID: 7216
	public static VRTrackedController rightVRTrackedController;

	// Token: 0x04001C31 RID: 7217
	public static VRTrackedController lastTouchController;

	// Token: 0x04001C32 RID: 7218
	public static NetworkPhysicsObject LatestZoomableObject;

	// Token: 0x04001C33 RID: 7219
	private Color pointerColor = Colour.White;

	// Token: 0x04001C34 RID: 7220
	private GameObject PrevTooltipObject;

	// Token: 0x04001C35 RID: 7221
	private float TooltipTimeHolder;

	// Token: 0x04001C36 RID: 7222
	private bool shouldDisplayClickTooltip;

	// Token: 0x04001C37 RID: 7223
	public bool Grabbing;

	// Token: 0x04001C38 RID: 7224
	public bool Grabbing_Previous;

	// Token: 0x04001C39 RID: 7225
	public bool GrabStarted;

	// Token: 0x04001C3A RID: 7226
	public bool GrabReleased;

	// Token: 0x04001C3B RID: 7227
	public bool WaitForGrabRelease;

	// Token: 0x04001C3C RID: 7228
	public bool DoOrientHeldObject;

	// Token: 0x04001C3D RID: 7229
	public bool EmulateTriggerClick;

	// Token: 0x04001C3E RID: 7230
	private float grabNextObjectUntil;

	// Token: 0x04001C3F RID: 7231
	private bool grabbingFromSearch;

	// Token: 0x04001C40 RID: 7232
	private float startedDisplayingtTooltipsAt;

	// Token: 0x04001C41 RID: 7233
	public GameObject[] ViveTooltips;

	// Token: 0x04001C42 RID: 7234
	public GameObject[] OculusTooltips;

	// Token: 0x04001C43 RID: 7235
	private GameObject[] Tooltips;

	// Token: 0x04001C44 RID: 7236
	private const int LEFT_CONTROLLER_OFFSET = 32;

	// Token: 0x04001C45 RID: 7237
	private const int RIGHT_CONTROLLER_OFFSET = 64;

	// Token: 0x04001C46 RID: 7238
	public const int MINIMUM_VALID_VR_KEYCODE = 32;

	// Token: 0x04001C47 RID: 7239
	private VRTrackedController.VRKeyCode lastPadPress;

	// Token: 0x04001C48 RID: 7240
	private float lastPadPressAt;

	// Token: 0x04001C49 RID: 7241
	private float directionPressedAt;

	// Token: 0x04001C4A RID: 7242
	private bool newlyGrabbedObject;

	// Token: 0x04001C4B RID: 7243
	private HandZone handZone;

	// Token: 0x04001C4C RID: 7244
	private bool objectHeldClose;

	// Token: 0x04001C4D RID: 7245
	private GameObject TouchpadIconContainer;

	// Token: 0x04001C4E RID: 7246
	private VRTouchpadIcon CurrentTool;

	// Token: 0x04001C4F RID: 7247
	private VRTouchpadIcon TouchpadUp;

	// Token: 0x04001C50 RID: 7248
	private VRTouchpadIcon TouchpadDown;

	// Token: 0x04001C51 RID: 7249
	private VRTouchpadIcon TouchpadLeft;

	// Token: 0x04001C52 RID: 7250
	private VRTouchpadIcon TouchpadRight;

	// Token: 0x04001C53 RID: 7251
	private VRTouchpadIcon TouchpadCenter;

	// Token: 0x04001C54 RID: 7252
	public TextMesh UIHoverText;

	// Token: 0x04001C55 RID: 7253
	public static int TOOLTIP_NEWLINE_CHARS = 20;

	// Token: 0x04001C56 RID: 7254
	private PointerMode IconMode;

	// Token: 0x04001C57 RID: 7255
	private bool displayingTooltips;

	// Token: 0x04001C58 RID: 7256
	private float displayingTooltipsFadeStartTime;

	// Token: 0x04001C59 RID: 7257
	private float displayingTooltipsFadeFromAlpha;

	// Token: 0x04001C5A RID: 7258
	private float displayingTooltipsFadeToAlpha;

	// Token: 0x04001C5B RID: 7259
	private bool displayTooltipsOnCodebaseChange;

	// Token: 0x04001C5C RID: 7260
	public VRTrackedController.TouchpadDownMode touchpadDownMode;

	// Token: 0x04001C5D RID: 7261
	private bool wantsToMove;

	// Token: 0x04001C5E RID: 7262
	private bool isPushing;

	// Token: 0x04001C5F RID: 7263
	private bool wantsToDisplayTooltips;

	// Token: 0x04001C60 RID: 7264
	private bool isZoomLocked;

	// Token: 0x04001C61 RID: 7265
	private bool awaitingZoomRelease;

	// Token: 0x04001C62 RID: 7266
	private bool stifleLaserPointer;

	// Token: 0x04001C63 RID: 7267
	private bool interfaceForMenu;

	// Token: 0x04001C64 RID: 7268
	private Vector3 startPushPosition;

	// Token: 0x04001C65 RID: 7269
	private Vector3 pushVelocity;

	// Token: 0x04001C66 RID: 7270
	private static float GrabSphereOriginXRotation = 0f;

	// Token: 0x04001C67 RID: 7271
	private float GrabSphereRotation;

	// Token: 0x04001C68 RID: 7272
	private Quaternion GrabSphereTranslationRotation;

	// Token: 0x04001C69 RID: 7273
	private bool BindingsInitialized;

	// Token: 0x04001C6A RID: 7274
	private bool ToolBindingsAttached = true;

	// Token: 0x04001C6B RID: 7275
	private bool IsIndexKnucklesController;

	// Token: 0x04001C6D RID: 7277
	private Collider[] grabbabaleColliders = new Collider[20];

	// Token: 0x04001C6E RID: 7278
	private bool analogPress;

	// Token: 0x04001C6F RID: 7279
	private Vector3 prevHapticsPoint = Vector3.zero;

	// Token: 0x04001C70 RID: 7280
	private Coroutine grabStackCoroutine;

	// Token: 0x04001C71 RID: 7281
	private Vector3 StartSelectionVector = Vector3.zero;

	// Token: 0x04001C72 RID: 7282
	private Vector3 StartSelectionCameraRightNorm = Vector3.zero;

	// Token: 0x04001C73 RID: 7283
	private Vector3 StartSelectionCameraRotation = Vector3.zero;

	// Token: 0x04001C74 RID: 7284
	private bool SelectionUsingLaser;

	// Token: 0x04001C75 RID: 7285
	private NetworkPhysicsObject currentZoomObject;

	// Token: 0x04001C76 RID: 7286
	private GameObject DummySpawnObject;

	// Token: 0x04001C77 RID: 7287
	private Vector3 DummyRotation = Vector3.zero;

	// Token: 0x04001C78 RID: 7288
	private Vector3 DummyScale = Vector3.one;

	// Token: 0x04001C79 RID: 7289
	private float lastUITime;

	// Token: 0x04001C7A RID: 7290
	private Color controllerColor = Color.white;

	// Token: 0x020007AF RID: 1967
	public enum VRSelectionStyle
	{
		// Token: 0x04002D0A RID: 11530
		Fixed,
		// Token: 0x04002D0B RID: 11531
		Exact,
		// Token: 0x04002D0C RID: 11532
		Anchored
	}

	// Token: 0x020007B0 RID: 1968
	// (Invoke) Token: 0x06003F98 RID: 16280
	public delegate void SelectedObjectFunction(NetworkPhysicsObject npo);

	// Token: 0x020007B1 RID: 1969
	public enum VRKeyCode
	{
		// Token: 0x04002D0E RID: 11534
		None,
		// Token: 0x04002D0F RID: 11535
		VRPadWest,
		// Token: 0x04002D10 RID: 11536
		VRPadEast,
		// Token: 0x04002D11 RID: 11537
		VRPadNorth = 4,
		// Token: 0x04002D12 RID: 11538
		VRPadSouth = 8,
		// Token: 0x04002D13 RID: 11539
		VRPadCenter = 16,
		// Token: 0x04002D14 RID: 11540
		VRLeftPadWest = 33,
		// Token: 0x04002D15 RID: 11541
		VRLeftPadEast,
		// Token: 0x04002D16 RID: 11542
		VRLeftPadNorth = 36,
		// Token: 0x04002D17 RID: 11543
		VRLeftPadSouth = 40,
		// Token: 0x04002D18 RID: 11544
		VRLeftPadCenter = 48,
		// Token: 0x04002D19 RID: 11545
		VRRightPadWest = 65,
		// Token: 0x04002D1A RID: 11546
		VRRightPadEast,
		// Token: 0x04002D1B RID: 11547
		VRRightPadNorth = 68,
		// Token: 0x04002D1C RID: 11548
		VRRightPadSouth = 72,
		// Token: 0x04002D1D RID: 11549
		VRRightPadCenter = 80
	}

	// Token: 0x020007B2 RID: 1970
	public enum VRKeyEvent
	{
		// Token: 0x04002D1F RID: 11551
		Held,
		// Token: 0x04002D20 RID: 11552
		Pressed,
		// Token: 0x04002D21 RID: 11553
		Released,
		// Token: 0x04002D22 RID: 11554
		LongPress
	}

	// Token: 0x020007B3 RID: 1971
	public enum TouchpadDownMode
	{
		// Token: 0x04002D24 RID: 11556
		Bindable,
		// Token: 0x04002D25 RID: 11557
		ToolSelect,
		// Token: 0x04002D26 RID: 11558
		Zoom
	}
}
