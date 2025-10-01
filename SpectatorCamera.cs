using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class SpectatorCamera : Singleton<SpectatorCamera>
{
	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000C1630 File Offset: 0x000BF830
	// (set) Token: 0x06001C17 RID: 7191 RVA: 0x000C1638 File Offset: 0x000BF838
	public bool ShowUI3D
	{
		get
		{
			return this._ShowUI3D;
		}
		set
		{
			this._ShowUI3D = value;
			if (this._ShowUI3D)
			{
				base.GetComponent<Camera>().cullingMask |= 131072;
				return;
			}
			base.GetComponent<Camera>().cullingMask &= -131073;
		}
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x000C1678 File Offset: 0x000BF878
	public void LoadState(CameraState state)
	{
		if (state.AbsolutePosition != null)
		{
			this.targetPosition = state.AbsolutePosition.Value.ToVector();
			this.targetRotation = Quaternion.Euler(state.Rotation.ToVector());
			if (!SpectatorCamera.SMOOTH_SET_POSITION)
			{
				base.transform.position = this.targetPosition;
				base.transform.rotation = this.targetRotation;
			}
		}
		this.lastState = state;
		this.lockedToPlayer = false;
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x000C16F8 File Offset: 0x000BF8F8
	public void SwitchToFree()
	{
		this.lockOn = SpectatorCamera.LockOn.Free;
		if (this.lastState != null)
		{
			this.LoadState(this.lastState);
		}
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x000C171C File Offset: 0x000BF91C
	public void Update()
	{
		if (this.lockedToPlayer)
		{
			this.lockOn = SpectatorCamera.LockOn.Player;
		}
		else if (this.lockedToObject)
		{
			this.lockOn = SpectatorCamera.LockOn.Object;
		}
		SpectatorCamera.LockOn lockOn = this.lockOn;
		if (lockOn != SpectatorCamera.LockOn.Player)
		{
			if (lockOn == SpectatorCamera.LockOn.Object)
			{
				if (!this.lockedToObject)
				{
					this.SwitchToFree();
				}
				else
				{
					if (this.lockObject == null)
					{
						this.lockObject = this.objectFromColourLabelOrGUID(this.lockObjectIdentifier);
					}
					if (this.lockObject != null)
					{
						this.targetPosition = this.lockObject.transform.position + this.lockObjectOffsetPosition;
						if (this.lockObject.GetComponent<Pointer>())
						{
							this.targetRotation = Quaternion.Euler(this.lockObjectOffsetRotation);
						}
						else
						{
							this.targetRotation = this.lockObject.transform.rotation * Quaternion.Euler(this.lockObjectOffsetRotation);
						}
					}
				}
			}
		}
		else
		{
			this.targetPosition = Camera.main.transform.position;
			this.targetRotation = Camera.main.transform.rotation;
			if (!this.lockedToPlayer)
			{
				if (this.lockedToObject)
				{
					this.lockOn = SpectatorCamera.LockOn.Object;
				}
				else
				{
					this.SwitchToFree();
				}
			}
		}
		base.transform.position = Vector3.Lerp(base.transform.position, this.targetPosition, SpectatorCamera.LERP_RATE);
		if (this.lockOn != SpectatorCamera.LockOn.Player || this.trackOverridesPlayer)
		{
			Quaternion rotation = base.transform.rotation;
			if (this.glanceIdentifier != null)
			{
				this.LookAt(this.glanceIdentifier, null);
				this.targetRotation = base.transform.rotation;
				base.transform.rotation = rotation;
			}
			else if (this.tracking)
			{
				this.trackedObject = this.LookAt(this.trackedIdentifier, this.trackedObject);
				this.targetRotation = base.transform.rotation;
				base.transform.rotation = rotation;
			}
			else if (this.prevTracking && this.lockOn == SpectatorCamera.LockOn.Free && this.lastState != null)
			{
				this.LoadState(this.lastState);
			}
		}
		this.prevTracking = this.tracking;
		this.glanceIdentifier = null;
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.targetRotation, SpectatorCamera.SLERP_RATE);
		if (this.lockOn == SpectatorCamera.LockOn.Object && this.lockObject)
		{
			base.transform.Translate(Vector3.forward * this.lockObjectOffset);
			if (this.stayUpright)
			{
				base.transform.LookAt(base.transform.position + base.transform.forward);
			}
		}
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x000C19DC File Offset: 0x000BFBDC
	private GameObject objectFromColourLabelOrGUID(string identifier)
	{
		GameObject result = null;
		Colour colour;
		if (Colour.TryColourFromLabel(identifier, out colour))
		{
			if (NetworkSingleton<PlayerManager>.Instance.ColourInUse(colour))
			{
				result = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromColorLabel(identifier).gameObject;
			}
		}
		else
		{
			result = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromGUID(identifier);
		}
		return result;
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x000C1A22 File Offset: 0x000BFC22
	private GameObject LookAt(string identifier, GameObject target = null)
	{
		if (target == null)
		{
			target = this.objectFromColourLabelOrGUID(identifier);
		}
		if (target != null)
		{
			base.transform.LookAt(target.transform.position);
		}
		return target;
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x000C1A56 File Offset: 0x000BFC56
	public void CopyPlayerCamera()
	{
		this.targetPosition = Camera.main.transform.position;
		this.targetRotation = Camera.main.transform.rotation;
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x000C1A84 File Offset: 0x000BFC84
	public void TurnOn(int width = 0, int height = 0, int rate = 0, int display = 1, bool panel = false, int x = 0, int y = 0, bool overlayButtons = true, bool sizingCorners = true, bool movable = true, bool hideOnHover = false)
	{
		if (this.active)
		{
			return;
		}
		this.active = true;
		base.gameObject.SetActive(true);
		this.UpdateUICamera();
		if (VRHMD.isVR && SpectatorCamera.VR_OVERRIDES_MAIN_WINDOW)
		{
			base.GetComponent<Camera>().targetDisplay = 0;
			this.SpectatorUICamera.GetComponent<Camera>().targetDisplay = 0;
			this.SpectatorRenderTopCamera.GetComponent<Camera>().targetDisplay = 0;
			return;
		}
		if (panel)
		{
			this.displayedInPanel = true;
			RenderTexture targetTexture = Singleton<UISpectatorView>.Instance.Init(x, y, width, height, overlayButtons, sizingCorners, movable, hideOnHover);
			base.GetComponent<Camera>().targetTexture = targetTexture;
			this.SpectatorUICamera.GetComponent<Camera>().targetTexture = targetTexture;
			this.SpectatorRenderTopCamera.GetComponent<Camera>().targetTexture = targetTexture;
			return;
		}
		try
		{
			if (width == 0)
			{
				Display.displays[display].Activate();
			}
			else
			{
				Display.displays[display].Activate(width, height, rate);
			}
		}
		catch
		{
		}
		base.GetComponent<Camera>().targetDisplay = display;
		this.SpectatorUICamera.GetComponent<Camera>().targetDisplay = display;
		this.SpectatorRenderTopCamera.GetComponent<Camera>().targetDisplay = display;
		Singleton<SpectatorAltZoomCamera>.Instance.GetComponent<Camera>().targetDisplay = display;
		this.DisplayingFullscreen = true;
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x000C1BC4 File Offset: 0x000BFDC4
	private void releaseTexture(Camera cam)
	{
		if (cam && cam.targetTexture)
		{
			cam.targetTexture.Release();
		}
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x000C1BE8 File Offset: 0x000BFDE8
	public void TurnOff()
	{
		if (!this.active || !this.displayedInPanel)
		{
			return;
		}
		this.releaseTexture(base.GetComponent<Camera>());
		this.releaseTexture(this.SpectatorUICamera.GetComponent<Camera>());
		this.releaseTexture(this.SpectatorRenderTopCamera.GetComponent<Camera>());
		Singleton<UISpectatorView>.Instance.gameObject.SetActive(false);
		Singleton<UISpectatorView>.Instance.SpectatorRenderTexture.Release();
		this.active = false;
		this.displayedInPanel = false;
		base.gameObject.SetActive(false);
		this.UpdateUICamera();
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x000C1C74 File Offset: 0x000BFE74
	public void ReassignRenderTexture(RenderTexture newRenderTexture)
	{
		if (!this.active || !this.displayedInPanel)
		{
			return;
		}
		base.GetComponent<Camera>().targetTexture.Release();
		this.SpectatorUICamera.GetComponent<Camera>().targetTexture.Release();
		this.SpectatorRenderTopCamera.GetComponent<Camera>().targetTexture.Release();
		base.GetComponent<Camera>().targetTexture = newRenderTexture;
		this.SpectatorUICamera.GetComponent<Camera>().targetTexture = newRenderTexture;
		this.SpectatorRenderTopCamera.GetComponent<Camera>().targetTexture = newRenderTexture;
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x000C1CFA File Offset: 0x000BFEFA
	public void UpdateGrid()
	{
		base.GetComponent<GridCameraRender>().enabled = this.showGrid;
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x000C1D0D File Offset: 0x000BFF0D
	public void OverrideUICamera(bool enable)
	{
		this.overrideUI = enable;
		this.UpdateUICamera();
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x000C1D1C File Offset: 0x000BFF1C
	public void UpdateUICamera()
	{
		this.SpectatorUICamera.SetActive(this.overrideUI || (this.active && this.showUI));
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x000C1D48 File Offset: 0x000BFF48
	private void OnPreCull()
	{
		if (!this.active || Colour.MyColorLabel() == "Grey")
		{
			return;
		}
		this.ZoomObjectHidden = false;
		if (this.RestrictView)
		{
			List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
			NetworkPhysicsObject y = null;
			if (NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject)
			{
				y = NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject.GetComponent<NetworkPhysicsObject>();
			}
			int i = 0;
			while (i < grabbableNPOs.Count)
			{
				NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
				if (networkPhysicsObject.MakeInvisibleToSpectator())
				{
					networkPhysicsObject.ForceInvisible(true);
					goto IL_83;
				}
				if (networkPhysicsObject.MakeObscuredToSpectator())
				{
					networkPhysicsObject.ForceObscured(true);
					goto IL_83;
				}
				IL_93:
				i++;
				continue;
				IL_83:
				if (networkPhysicsObject == y)
				{
					this.ZoomObjectHidden = true;
					goto IL_93;
				}
				goto IL_93;
			}
		}
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x000C1DF8 File Offset: 0x000BFFF8
	private void OnPostRender()
	{
		if (!this.active || Colour.MyColorLabel() == "Grey")
		{
			return;
		}
		if (this.RestrictView)
		{
			List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
			for (int i = 0; i < grabbableNPOs.Count; i++)
			{
				NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[i];
				if (networkPhysicsObject.OverrideIsInvisible)
				{
					networkPhysicsObject.ForceInvisible(false);
				}
				else if (networkPhysicsObject.OverrideIsObscured)
				{
					networkPhysicsObject.ForceObscured(false);
				}
			}
		}
	}

	// Token: 0x040011BB RID: 4539
	public static float LERP_RATE = 0.2f;

	// Token: 0x040011BC RID: 4540
	public static float SLERP_RATE = 0.2f;

	// Token: 0x040011BD RID: 4541
	public static bool VR_OVERRIDES_MAIN_WINDOW = true;

	// Token: 0x040011BE RID: 4542
	public static bool SMOOTH_SET_POSITION = false;

	// Token: 0x040011BF RID: 4543
	public bool DisplayingFullscreen;

	// Token: 0x040011C0 RID: 4544
	public bool ZoomObjectHidden;

	// Token: 0x040011C1 RID: 4545
	public bool RestrictView;

	// Token: 0x040011C2 RID: 4546
	private bool _ShowUI3D = true;

	// Token: 0x040011C3 RID: 4547
	public bool active;

	// Token: 0x040011C4 RID: 4548
	public bool displayedInPanel;

	// Token: 0x040011C5 RID: 4549
	public bool lockedToPlayer = true;

	// Token: 0x040011C6 RID: 4550
	public bool lockedToObject;

	// Token: 0x040011C7 RID: 4551
	public string lockObjectIdentifier;

	// Token: 0x040011C8 RID: 4552
	public GameObject lockObject;

	// Token: 0x040011C9 RID: 4553
	public float lockObjectOffset;

	// Token: 0x040011CA RID: 4554
	public Vector3 lockObjectOffsetPosition = Vector3.zero;

	// Token: 0x040011CB RID: 4555
	public Vector3 lockObjectOffsetRotation = Vector3.zero;

	// Token: 0x040011CC RID: 4556
	public GameObject trackedObject;

	// Token: 0x040011CD RID: 4557
	public string trackedIdentifier = "";

	// Token: 0x040011CE RID: 4558
	public string glanceIdentifier;

	// Token: 0x040011CF RID: 4559
	public bool tracking;

	// Token: 0x040011D0 RID: 4560
	private bool prevTracking;

	// Token: 0x040011D1 RID: 4561
	public bool trackOverridesPlayer;

	// Token: 0x040011D2 RID: 4562
	public bool showUI;

	// Token: 0x040011D3 RID: 4563
	private bool overrideUI;

	// Token: 0x040011D4 RID: 4564
	public bool showGrid = true;

	// Token: 0x040011D5 RID: 4565
	public bool stayUpright;

	// Token: 0x040011D6 RID: 4566
	public int lastSetPosition;

	// Token: 0x040011D7 RID: 4567
	public CameraState lastState;

	// Token: 0x040011D8 RID: 4568
	public GameObject SpectatorUICamera;

	// Token: 0x040011D9 RID: 4569
	public GameObject SpectatorRenderTopCamera;

	// Token: 0x040011DA RID: 4570
	private Vector3 targetPosition;

	// Token: 0x040011DB RID: 4571
	private Quaternion targetRotation;

	// Token: 0x040011DC RID: 4572
	private SpectatorCamera.LockOn lockOn = SpectatorCamera.LockOn.Player;

	// Token: 0x020006C3 RID: 1731
	private enum LockOn
	{
		// Token: 0x04002939 RID: 10553
		Free,
		// Token: 0x0400293A RID: 10554
		Player,
		// Token: 0x0400293B RID: 10555
		Object
	}
}
