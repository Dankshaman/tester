using System;
using NewNet;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A8 RID: 424
public class MagnifyCamera : Singleton<MagnifyCamera>
{
	// Token: 0x06001532 RID: 5426 RVA: 0x00089F70 File Offset: 0x00088170
	private void Start()
	{
		this.cameraObject = this.magnifyCamera.gameObject;
		this.parentCanvas = this.magnifyUI.GetComponentInParent<Canvas>();
		this.magnifyUI.raycastTarget = false;
		EventManager.OnScreenDimensionsChange += this.OnScreenDimensionsChange;
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x00089FBC File Offset: 0x000881BC
	private void OnDestroy()
	{
		EventManager.OnScreenDimensionsChange -= this.OnScreenDimensionsChange;
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x00089FCF File Offset: 0x000881CF
	private void OnScreenDimensionsChange()
	{
		this.Reset();
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x00089FD7 File Offset: 0x000881D7
	public void Reset()
	{
		MagnifyCamera.SCREEN_PERCENT = Mathf.Clamp(MagnifyCamera.SCREEN_PERCENT, 0.1f, 1f);
		this.inited = false;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x00089FFC File Offset: 0x000881FC
	private void Update()
	{
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			return;
		}
		bool flag = zInput.GetButton("Magnify", ControlType.All) && !UICamera.SelectIsInput() && !NetworkSingleton<PlayerManager>.Instance.IsBlinded();
		this.cameraObject.SetActive(flag);
		this.magnifyUI.gameObject.SetActive(flag);
		if (flag)
		{
			this.Init();
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentCanvas.transform as RectTransform, Input.mousePosition, this.parentCanvas.worldCamera, out v);
			this.magnifyUI.transform.position = this.parentCanvas.transform.TransformPoint(v);
			Vector3 point = new Vector3(0f, 0f, -Vector3.Distance(HoverScript.PointerPosition, HoverScript.MainCamera.transform.position) * 0.16f / Singleton<CameraController>.Instance.MagnifyZoom);
			Vector3 position = HoverScript.MainCamera.transform.rotation * point + HoverScript.PointerPosition;
			this.cameraObject.transform.position = position;
		}
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x0008A120 File Offset: 0x00088320
	private void Init()
	{
		if (this.inited)
		{
			return;
		}
		this.inited = true;
		if (this.renderTexture != null)
		{
			this.renderTexture.Release();
		}
		int num = Mathf.RoundToInt((float)Screen.height * MagnifyCamera.SCREEN_PERCENT);
		this.renderTexture = new RenderTexture(num, num, 0);
		this.magnifyCamera.targetTexture = this.renderTexture;
		this.magnifyUI.texture = this.renderTexture;
		(this.magnifyUI.transform as RectTransform).sizeDelta = new Vector2((float)Mathf.RoundToInt(1080f * MagnifyCamera.SCREEN_PERCENT), (float)Mathf.RoundToInt(1080f * MagnifyCamera.SCREEN_PERCENT));
	}

	// Token: 0x04000BF9 RID: 3065
	public Camera magnifyCamera;

	// Token: 0x04000BFA RID: 3066
	public RawImage magnifyUI;

	// Token: 0x04000BFB RID: 3067
	private GameObject cameraObject;

	// Token: 0x04000BFC RID: 3068
	private Canvas parentCanvas;

	// Token: 0x04000BFD RID: 3069
	private bool inited;

	// Token: 0x04000BFE RID: 3070
	private RenderTexture renderTexture;

	// Token: 0x04000BFF RID: 3071
	public static float SCREEN_PERCENT = 0.7f;
}
