using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x0200036D RID: 877
public class VRRenderModel : MonoBehaviour
{
	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06002947 RID: 10567 RVA: 0x00122772 File Offset: 0x00120972
	// (set) Token: 0x06002948 RID: 10568 RVA: 0x0012277A File Offset: 0x0012097A
	[HideInInspector]
	public VRRenderModel.RenderModelType modelType { get; private set; }

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06002949 RID: 10569 RVA: 0x00122783 File Offset: 0x00120983
	// (set) Token: 0x0600294A RID: 10570 RVA: 0x0012278B File Offset: 0x0012098B
	[HideInInspector]
	public VRControllerType controllerType { get; private set; }

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x0600294B RID: 10571 RVA: 0x00122794 File Offset: 0x00120994
	// (set) Token: 0x0600294C RID: 10572 RVA: 0x0012279C File Offset: 0x0012099C
	[HideInInspector]
	public bool hasAnalogStick { get; private set; }

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x0600294D RID: 10573 RVA: 0x001227A5 File Offset: 0x001209A5
	// (set) Token: 0x0600294E RID: 10574 RVA: 0x001227AD File Offset: 0x001209AD
	[HideInInspector]
	public SteamVR_RenderModel steamVR_RenderModel { get; private set; }

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x0600294F RID: 10575 RVA: 0x001227B6 File Offset: 0x001209B6
	// (set) Token: 0x06002950 RID: 10576 RVA: 0x001227BE File Offset: 0x001209BE
	[HideInInspector]
	public bool loaded { get; private set; }

	// Token: 0x06002951 RID: 10577 RVA: 0x001227C7 File Offset: 0x001209C7
	private void Awake()
	{
		this.steamVR_RenderModel = base.GetComponent<SteamVR_RenderModel>();
		SteamVR_Events.RenderModelLoaded.Listen(new UnityAction<SteamVR_RenderModel, bool>(this.RenderModelLoaded));
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x001227EC File Offset: 0x001209EC
	private void RenderModelLoaded(SteamVR_RenderModel renderModel, bool success)
	{
		if (renderModel != this.steamVR_RenderModel)
		{
			return;
		}
		this.modelType = VRRenderModel.GetRenderModelType(renderModel.renderModelName);
		this.controllerType = VRRenderModel.RenderModelToController(this.modelType);
		this.hasAnalogStick = VRRenderModel.HasAnalogStick(this.controllerType);
		Texture2D texture2D = null;
		switch (this.modelType)
		{
		case VRRenderModel.RenderModelType.vr_controller_vive_1_5:
			this.ViveObject.SetActive(true);
			texture2D = this.ViveTexture;
			break;
		case VRRenderModel.RenderModelType.oculus_cv1_controller_left:
			this.RiftLeftObject.SetActive(true);
			texture2D = this.RiftLeftTexture;
			break;
		case VRRenderModel.RenderModelType.oculus_cv1_controller_right:
			this.RiftRightObject.SetActive(true);
			texture2D = this.RiftRightTexture;
			break;
		case VRRenderModel.RenderModelType.valve_controller_knu_left:
			this.RiftLeftObject.SetActive(true);
			break;
		case VRRenderModel.RenderModelType.valve_controller_knu_right:
			this.RiftRightObject.SetActive(true);
			break;
		}
		if (texture2D)
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.mainTexture = texture2D;
			}
		}
		this.loaded = true;
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x001228EC File Offset: 0x00120AEC
	public static VRRenderModel.RenderModelType GetRenderModelType(string renderModelName)
	{
		if (!renderModelName.StartsWith("{indexcontroller}valve_controller_knu_"))
		{
			VRRenderModel.RenderModelType result;
			try
			{
				result = (VRRenderModel.RenderModelType)Enum.Parse(typeof(VRRenderModel.RenderModelType), renderModelName);
			}
			catch (Exception)
			{
				result = VRRenderModel.RenderModelType.Unknown;
			}
			return result;
		}
		if (renderModelName.EndsWith("left"))
		{
			return VRRenderModel.RenderModelType.valve_controller_knu_left;
		}
		return VRRenderModel.RenderModelType.valve_controller_knu_right;
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x00122948 File Offset: 0x00120B48
	public static VRControllerType GetControllerType(string renderModelName)
	{
		return VRRenderModel.RenderModelToController(VRRenderModel.GetRenderModelType(renderModelName));
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x00122955 File Offset: 0x00120B55
	public static VRControllerType RenderModelToController(VRRenderModel.RenderModelType type)
	{
		switch (type)
		{
		case VRRenderModel.RenderModelType.vr_controller_vive_1_5:
			return VRControllerType.vive_wand;
		case VRRenderModel.RenderModelType.oculus_cv1_controller_left:
		case VRRenderModel.RenderModelType.oculus_cv1_controller_right:
			return VRControllerType.oculus_touch;
		case VRRenderModel.RenderModelType.valve_controller_knu_left:
		case VRRenderModel.RenderModelType.valve_controller_knu_right:
			return VRControllerType.index_knuckles;
		default:
			return VRControllerType.Unknown;
		}
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x0012297A File Offset: 0x00120B7A
	public static bool HasAnalogStick(VRControllerType type)
	{
		return type == VRControllerType.oculus_touch || type == VRControllerType.index_knuckles;
	}

	// Token: 0x04001B5F RID: 7007
	public Texture2D ViveTexture;

	// Token: 0x04001B60 RID: 7008
	public Texture2D RiftLeftTexture;

	// Token: 0x04001B61 RID: 7009
	public Texture2D RiftRightTexture;

	// Token: 0x04001B62 RID: 7010
	public GameObject ViveObject;

	// Token: 0x04001B63 RID: 7011
	public GameObject RiftLeftObject;

	// Token: 0x04001B64 RID: 7012
	public GameObject RiftRightObject;

	// Token: 0x04001B6A RID: 7018
	[HideInInspector]
	public static bool HideTouchpad;

	// Token: 0x020007AB RID: 1963
	public enum RenderModelType
	{
		// Token: 0x04002CF6 RID: 11510
		Unknown = -1,
		// Token: 0x04002CF7 RID: 11511
		vr_controller_vive_1_5,
		// Token: 0x04002CF8 RID: 11512
		oculus_cv1_controller_left,
		// Token: 0x04002CF9 RID: 11513
		oculus_cv1_controller_right,
		// Token: 0x04002CFA RID: 11514
		valve_controller_knu_left,
		// Token: 0x04002CFB RID: 11515
		valve_controller_knu_right
	}
}
