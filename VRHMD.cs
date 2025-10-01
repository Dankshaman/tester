using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;
using Valve.VR;

// Token: 0x02000368 RID: 872
public class VRHMD : Singleton<VRHMD>
{
	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x0600291B RID: 10523 RVA: 0x00121054 File Offset: 0x0011F254
	public int Index
	{
		get
		{
			return (int)this.trackedObject.index;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x0600291C RID: 10524 RVA: 0x00121061 File Offset: 0x0011F261
	// (set) Token: 0x0600291D RID: 10525 RVA: 0x00121068 File Offset: 0x0011F268
	public static bool isVR
	{
		get
		{
			return VRHMD._isVR;
		}
		private set
		{
			if (value != VRHMD._isVR)
			{
				VRHMD._isVR = value;
				NetworkSingleton<PlayerManager>.Instance.SetVR(NetworkID.ID, value);
				EventManager.TriggerVR(value);
			}
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x0600291E RID: 10526 RVA: 0x0012108E File Offset: 0x0011F28E
	// (set) Token: 0x0600291F RID: 10527 RVA: 0x00121096 File Offset: 0x0011F296
	public float Scale
	{
		get
		{
			return this.scale;
		}
		set
		{
			value = Mathf.Clamp(value, 0.025f, 4f);
			if (value != this.scale)
			{
				this.scale = value;
				this.Fade<float>(new Action<float>(this.SetScale), this.scale);
			}
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06002920 RID: 10528 RVA: 0x001210D2 File Offset: 0x0011F2D2
	// (set) Token: 0x06002921 RID: 10529 RVA: 0x001210DC File Offset: 0x0011F2DC
	public float Floor
	{
		get
		{
			return this.floor;
		}
		set
		{
			value = Mathf.Clamp(value, -50f, this.floorTableSurface + 30f);
			if ((this.floor < this.floorTableSurface && value > this.floorTableSurface) || (this.floor > this.floorTableSurface && value < this.floorTableSurface))
			{
				value = this.floorTableSurface;
			}
			if ((this.floor < this.initialCameraFloor && value > this.initialCameraFloor) || (this.floor > this.initialCameraFloor && value < this.initialCameraFloor))
			{
				value = this.initialCameraFloor;
			}
			if (value != this.floor)
			{
				this.floor = value;
				this.Fade<float>(new Action<float>(this.SetFloor), this.floor);
			}
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06002922 RID: 10530 RVA: 0x00121196 File Offset: 0x0011F396
	// (set) Token: 0x06002923 RID: 10531 RVA: 0x0012119E File Offset: 0x0011F39E
	public float Spin
	{
		get
		{
			return this.spin;
		}
		set
		{
			if (value != this.spin)
			{
				this.spin = value;
				this.Fade<float>(new Action<float>(this.SetSpin), this.spin);
			}
		}
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x001211C8 File Offset: 0x0011F3C8
	public void CheckUISetting()
	{
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			if (this.floatingUI)
			{
				this.ToggleUIMode();
				return;
			}
		}
		else if (VRHMD.FLOATING != this.floatingUI)
		{
			this.ToggleUIMode();
		}
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x001211F4 File Offset: 0x0011F3F4
	public void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.SetupRenderTexture));
		NetworkEvents.OnSettingsChange -= this.CheckUISetting;
		NetworkEvents.OnConnectedToServer -= this.CheckUISetting;
		NetworkEvents.OnServerInitialized -= this.CheckUISetting;
		EventManager.OnChangePlayerColor -= this.EventManager_OnChangePlayerColor;
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x00121268 File Offset: 0x0011F468
	protected override void Awake()
	{
		base.Awake();
		NetworkEvents.OnSettingsChange += this.CheckUISetting;
		NetworkEvents.OnConnectedToServer += this.CheckUISetting;
		NetworkEvents.OnServerInitialized += this.CheckUISetting;
		VRHMD.isVR = true;
		foreach (GameObject gameObject in this.EnableObjects)
		{
			gameObject.SetActive(true);
		}
		this.initialScale = this.VRCameraRig.transform.localScale;
		this.initialCameraFloor = this.VRCameraRig.transform.position.y;
		this.initialFloor = this.FloorObject.transform.position.y;
		this.floorTableSurface = Mathf.Abs(this.initialCameraFloor) + 0.9f;
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x0012135C File Offset: 0x0011F55C
	private void Start()
	{
		this.uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
		this.uiCamerauGUI = GameObject.Find("uGUI Camera").GetComponent<Camera>();
		Screen.SetResolution(1920, 1080, true);
		this.VRUIScript = this.VRUI.GetComponent<VRUI>();
		this.UIAnchor = this.VRUI.transform.parent.gameObject;
		this.InitialUIAnchorScale = this.UIAnchor.transform.localScale;
		this.InitialUILocalPosition = this.VRUI.transform.localPosition;
		this.InitialUILocalRotation = this.VRUI.transform.localRotation;
		base.StartCoroutine(this.DelaySetup());
		SteamVR_Camera.sceneResolutionScale = 1.25f;
		this.SkyboxTexture = (Resources.Load("VR_Loading_Texture") as Texture2D);
		this.BlackTexture = Texture2D.blackTexture;
		SteamVR_Skybox.SetOverride(this.BlackTexture, null, null, null, null, null);
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.SetupRenderTexture));
		EventManager.OnChangePlayerColor += this.EventManager_OnChangePlayerColor;
		this.StoreInitialPosition(true);
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x00121490 File Offset: 0x0011F690
	private IEnumerator DelaySetup()
	{
		yield return null;
		base.gameObject.tag = "MainCamera";
		this.VRCamera.tag = "MainCamera";
		this.Initialized = true;
		this.CheckUISetting();
		yield break;
	}

	// Token: 0x06002929 RID: 10537 RVA: 0x001214A0 File Offset: 0x0011F6A0
	private void SetupRenderTexture()
	{
		if (this.floatingUI)
		{
			this.ToggleUIMode();
		}
		Transform transform = this.UIAnchor.transform;
		transform.localScale = new Vector3(transform.localScale.x, this.InitialUIAnchorScale.y * (float)Screen.height / (float)Screen.width, transform.localScale.z);
		if (this.UIRenderTexture != null)
		{
			UnityEngine.Object.Destroy(this.UIRenderTexture);
		}
		this.UIRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		this.UIRenderTexture.useMipMap = true;
		this.UIRenderTexture.anisoLevel = 9;
		this.VRUI.GetComponent<Renderer>().material.mainTexture = this.UIRenderTexture;
		this.uiCamera.transform.Reset();
		this.uiCamera.targetTexture = this.UIRenderTexture;
		this.uiCamera.allowHDR = true;
		this.uiCamera.clearFlags = CameraClearFlags.Color;
		this.uiCamera.backgroundColor = Colour.UnityClear;
		if (this.UIuGUIRenderTexture != null)
		{
			UnityEngine.Object.Destroy(this.UIuGUIRenderTexture);
		}
		this.UIuGUIRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		this.UIuGUIRenderTexture.useMipMap = true;
		this.UIuGUIRenderTexture.anisoLevel = 9;
		this.VRUIuGUI.GetComponent<Renderer>().material.mainTexture = this.UIuGUIRenderTexture;
		this.uiCamerauGUI.transform.Reset();
		this.uiCamerauGUI.targetTexture = this.UIuGUIRenderTexture;
		this.uiCamerauGUI.allowHDR = true;
		this.uiCamerauGUI.clearFlags = CameraClearFlags.Color;
		this.uiCamerauGUI.backgroundColor = Colour.UnityClear;
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x000025B8 File Offset: 0x000007B8
	private void EventManager_OnChangePlayerColor(Color newColor, int id)
	{
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x0600292B RID: 10539 RVA: 0x00121667 File Offset: 0x0011F867
	// (set) Token: 0x0600292C RID: 10540 RVA: 0x0012166F File Offset: 0x0011F86F
	public bool floatingUI { get; set; }

	// Token: 0x0600292D RID: 10541 RVA: 0x00121678 File Offset: 0x0011F878
	public void ResetUITransform()
	{
		if (this.floatingUI)
		{
			Vector3 lossyScale = this.VRUI.transform.lossyScale;
			this.VRUI.transform.parent = this.VRCameraRig.transform;
			float num = 35f;
			this.AppliedScale = lossyScale / 130f;
			this.AspectRatio = this.AppliedScale.x / this.AppliedScale.z;
			this.VRUI.transform.localScale = this.AppliedScale;
			this.VRUI.transform.position = new Vector3(this.Head.transform.position.x + num * this.UIAnchor.transform.forward.normalized.x, this.VRUI.transform.position.y, this.Head.transform.position.z + num * this.UIAnchor.transform.forward.normalized.z);
			return;
		}
		this.VRUI.transform.parent = this.UIAnchor.transform;
		this.VRUI.transform.localScale = Vector3.one;
		this.VRUI.transform.localPosition = this.InitialUILocalPosition;
		this.VRUI.transform.localRotation = this.InitialUILocalRotation;
		this.VRUI.GetComponent<Collider>().enabled = false;
		this.VRUI.GetComponent<Collider>().enabled = true;
	}

	// Token: 0x0600292E RID: 10542 RVA: 0x00121819 File Offset: 0x0011FA19
	public void ToggleUIMode()
	{
		this.floatingUI = !this.floatingUI;
		this.ResetUITransform();
		if (this.floatingUI)
		{
			this.VRUIScript.SetAttachment(this.VRUIScript.VRUIAttached);
		}
	}

	// Token: 0x0600292F RID: 10543 RVA: 0x00121850 File Offset: 0x0011FA50
	private void Update()
	{
		if (this.uiCamera.targetTexture == null)
		{
			this.SetupRenderTexture();
		}
		if (UILoading.IsLoading && (float)Singleton<UILoading>.Instance.NumLoading > 5f && !this.fading)
		{
			SteamVR_Skybox.SetOverride(this.SkyboxTexture, this.SkyboxTexture, this.SkyboxTexture, this.SkyboxTexture, this.BlackTexture, this.BlackTexture);
			this.fading = true;
			SteamVR_Events.Loading.Send(true);
			SteamVR_Events.LoadingFadeOut.Send(1f);
			SteamVR_Render.pauseRendering = true;
		}
		else if (!UILoading.IsLoading && this.fading)
		{
			SteamVR_Skybox.SetOverride(this.BlackTexture, null, null, null, null, null);
			this.fading = false;
			SteamVR_Render.pauseRendering = false;
			SteamVR_Events.Loading.Send(false);
			SteamVR_Events.LoadingFadeIn.Send(1f);
		}
		float y = this.prevClosetAngle;
		float num = float.MaxValue;
		float num2 = Quaternion.Angle(Quaternion.Euler(new Vector3(0f, 0f, 0f)), Quaternion.Euler(new Vector3(0f, this.Head.transform.eulerAngles.y, 0f)));
		if (num2 < num && num2 < this.DeadZone)
		{
			y = 0f;
			num = num2;
		}
		num2 = Quaternion.Angle(Quaternion.Euler(new Vector3(0f, 90f, 0f)), Quaternion.Euler(new Vector3(0f, this.Head.transform.eulerAngles.y, 0f)));
		if (num2 < num && num2 < this.DeadZone)
		{
			y = 90f;
			num = num2;
		}
		num2 = Quaternion.Angle(Quaternion.Euler(new Vector3(0f, 180f, 0f)), Quaternion.Euler(new Vector3(0f, this.Head.transform.eulerAngles.y, 0f)));
		if (num2 < num && num2 < this.DeadZone)
		{
			y = 180f;
			num = num2;
		}
		num2 = Quaternion.Angle(Quaternion.Euler(new Vector3(0f, -90f, 0f)), Quaternion.Euler(new Vector3(0f, this.Head.transform.eulerAngles.y, 0f)));
		if (num2 < num && num2 < this.DeadZone)
		{
			y = -90f;
		}
		this.prevClosetAngle = y;
		this.UIAnchor.transform.eulerAngles = new Vector3(0f, y, 0f);
		if (PlayerScript.Pointer)
		{
			PlayerScript.PointerScript.pointerSyncs.SetTrackedTransform(this.Head.transform.position, this.Head.transform.rotation, this.VRCameraRig.transform.localScale.x, TrackedType.Headset, this.trackedObject, false, this.Index);
		}
	}

	// Token: 0x06002930 RID: 10544 RVA: 0x00121B3B File Offset: 0x0011FD3B
	private void Fade<T>(Action<T> Func, T Value)
	{
		base.StartCoroutine(this.FadeCoroutine<T>(Func, Value));
	}

	// Token: 0x06002931 RID: 10545 RVA: 0x00121B4C File Offset: 0x0011FD4C
	private IEnumerator FadeCoroutine<T>(Action<T> Func, T Value)
	{
		SteamVR_Fade.Start(Colour.UnityBlack, VRHMD.FadeTime, false);
		yield return this.fadeWaitTime;
		yield return null;
		Func(Value);
		yield return null;
		SteamVR_Fade.Start(Colour.UnityClear, VRHMD.FadeTime, false);
		if (this.StoreAfterFade > 0)
		{
			int num = this.StoreAfterFade - 1;
			this.StoreAfterFade = num;
			if (num == 0)
			{
				this.StoreInitialPosition(false);
			}
		}
		yield break;
	}

	// Token: 0x06002932 RID: 10546 RVA: 0x00121B6C File Offset: 0x0011FD6C
	private void SetFloor(float floor)
	{
		this.VRCameraRig.transform.position = new Vector3(this.VRCameraRig.transform.position.x, this.initialCameraFloor + floor, this.VRCameraRig.transform.position.z);
		this.FloorObject.transform.position = new Vector3(this.FloorObject.transform.position.x, this.initialFloor + floor, this.FloorObject.transform.position.z);
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x00121C08 File Offset: 0x0011FE08
	private void SetScale(float scale)
	{
		Vector3 position = this.Head.transform.position;
		this.VRCameraRig.transform.localScale = this.initialScale * scale;
		this.SetTeleport(position);
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x00121C4C File Offset: 0x0011FE4C
	private void SetSpin(float spin)
	{
		Vector3 position = this.Head.transform.position;
		this.VRCameraRig.transform.eulerAngles = new Vector3(this.VRCameraRig.transform.eulerAngles.x, spin, this.VRCameraRig.transform.eulerAngles.z);
		this.SetTeleport(position);
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x00121CB1 File Offset: 0x0011FEB1
	public void Teleport(Vector3 Position)
	{
		this.Fade<Vector3>(new Action<Vector3>(this.SetTeleport), Position);
	}

	// Token: 0x06002936 RID: 10550 RVA: 0x00121CC8 File Offset: 0x0011FEC8
	private void SetTeleport(Vector3 Position)
	{
		Vector3 vector = new Vector3(this.VRCameraRig.transform.position.x - this.Head.transform.position.x, 0f, this.VRCameraRig.transform.position.z - this.Head.transform.position.z);
		this.VRCameraRig.position = new Vector3(Position.x + vector.x, this.VRCameraRig.position.y, Position.z + vector.z);
		this.prevTeleportPosition = Position;
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x00121D78 File Offset: 0x0011FF78
	public void StoreInitialPosition(bool storeScale = false)
	{
		this.VRCameraRigInitialPosition = this.VRCameraRig.position;
		this.VRCameraRigInitialRotation = this.VRCameraRig.rotation;
		if (storeScale)
		{
			this.VRCameraRigInitialScale = this.VRCameraRig.localScale;
		}
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x00121DB0 File Offset: 0x0011FFB0
	public void ResetToInitialPosition()
	{
		this.VRCameraRig.position = this.VRCameraRigInitialPosition;
		this.VRCameraRig.localScale = this.VRCameraRigInitialScale;
		this.VRCameraRig.rotation = this.VRCameraRigInitialRotation;
	}

	// Token: 0x04001AFC RID: 6908
	public static float WALL_ANGLE = 90f;

	// Token: 0x04001AFD RID: 6909
	public GameObject Head;

	// Token: 0x04001AFE RID: 6910
	public GameObject VRUI;

	// Token: 0x04001AFF RID: 6911
	public VRUI VRUIScript;

	// Token: 0x04001B00 RID: 6912
	public GameObject VRUIuGUI;

	// Token: 0x04001B01 RID: 6913
	public GameObject FloorObject;

	// Token: 0x04001B02 RID: 6914
	public Transform Pivot;

	// Token: 0x04001B03 RID: 6915
	public List<GameObject> EnableObjects;

	// Token: 0x04001B04 RID: 6916
	private GameObject UIAnchor;

	// Token: 0x04001B05 RID: 6917
	private RenderTexture UIRenderTexture;

	// Token: 0x04001B06 RID: 6918
	private RenderTexture UIuGUIRenderTexture;

	// Token: 0x04001B07 RID: 6919
	public Camera VRCamera;

	// Token: 0x04001B08 RID: 6920
	public static bool WALL_MODE;

	// Token: 0x04001B09 RID: 6921
	public static bool FLOATING = false;

	// Token: 0x04001B0A RID: 6922
	private Camera uiCamera;

	// Token: 0x04001B0B RID: 6923
	private Camera uiCamerauGUI;

	// Token: 0x04001B0C RID: 6924
	private float prevClosetAngle;

	// Token: 0x04001B0D RID: 6925
	private float DeadZone = 20f;

	// Token: 0x04001B0E RID: 6926
	public Vector3 VRCameraRigInitialPosition;

	// Token: 0x04001B0F RID: 6927
	public Vector3 VRCameraRigInitialScale;

	// Token: 0x04001B10 RID: 6928
	[NonSerialized]
	public float AspectRatio = 1.7777778f;

	// Token: 0x04001B11 RID: 6929
	public Quaternion VRCameraRigInitialRotation;

	// Token: 0x04001B12 RID: 6930
	public int StoreAfterFade;

	// Token: 0x04001B13 RID: 6931
	public Transform VRCameraRig;

	// Token: 0x04001B14 RID: 6932
	public SteamVR_TrackedObject trackedObject;

	// Token: 0x04001B15 RID: 6933
	private static bool _isVR = false;

	// Token: 0x04001B16 RID: 6934
	private Vector3 initialScale;

	// Token: 0x04001B17 RID: 6935
	private float scale = 1f;

	// Token: 0x04001B18 RID: 6936
	private float initialFloor;

	// Token: 0x04001B19 RID: 6937
	private float initialCameraFloor;

	// Token: 0x04001B1A RID: 6938
	private float floorTableSurface;

	// Token: 0x04001B1B RID: 6939
	private float floor;

	// Token: 0x04001B1C RID: 6940
	private float spin;

	// Token: 0x04001B1D RID: 6941
	private Vector3 InitialUIAnchorScale;

	// Token: 0x04001B1E RID: 6942
	private Vector3 InitialUILocalPosition;

	// Token: 0x04001B1F RID: 6943
	private Quaternion InitialUILocalRotation;

	// Token: 0x04001B20 RID: 6944
	private Texture2D SkyboxTexture;

	// Token: 0x04001B21 RID: 6945
	private Texture2D BlackTexture;

	// Token: 0x04001B22 RID: 6946
	public bool Initialized;

	// Token: 0x04001B24 RID: 6948
	public Vector3 AppliedScale;

	// Token: 0x04001B25 RID: 6949
	private bool fading;

	// Token: 0x04001B26 RID: 6950
	private static float FadeTime = 0.33f;

	// Token: 0x04001B27 RID: 6951
	private WaitForSeconds fadeWaitTime = new WaitForSeconds(VRHMD.FadeTime);

	// Token: 0x04001B28 RID: 6952
	public Vector3 prevTeleportPosition = Vector3.zero;
}
