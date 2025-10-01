using System;
using UnityEngine;

// Token: 0x02000285 RID: 645
public class UICameraModeToggle : MonoBehaviour
{
	// Token: 0x0600215A RID: 8538 RVA: 0x000F0846 File Offset: 0x000EEA46
	private void Awake()
	{
		this.cameraController = Singleton<CameraController>.Instance;
		this.uiToggle = base.GetComponent<UIToggle>();
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000F085F File Offset: 0x000EEA5F
	private void Update()
	{
		this.uiToggle.value = (this.cameraController.CurrentMode == this.CameraMode);
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000F087F File Offset: 0x000EEA7F
	private void OnClick()
	{
		this.cameraController.StartCameraMode(this.CameraMode);
	}

	// Token: 0x040014A7 RID: 5287
	public CameraController.CameraMode CameraMode;

	// Token: 0x040014A8 RID: 5288
	private CameraController cameraController;

	// Token: 0x040014A9 RID: 5289
	private UIToggle uiToggle;
}
