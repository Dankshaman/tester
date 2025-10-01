using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class CameraManager : Singleton<CameraManager>
{
	// Token: 0x060008A1 RID: 2209 RVA: 0x0003D9C8 File Offset: 0x0003BBC8
	protected override void Awake()
	{
		base.Awake();
		this.PureSkyboxMaterial = new Material(this.PureSkyboxMaterial);
		this.AllCameras = new Camera[6];
		this.AllCameras[0] = Camera.main;
		this.AllCameras[1] = this.UICamera;
		this.AllCameras[2] = this.AltZoomCamera;
		this.AllCameras[3] = this.SpectatorCamera;
		this.AllCameras[4] = this.SpectatorAltZoomCamera;
		this.AllCameras[5] = this.HandCamera;
	}

	// Token: 0x0400062C RID: 1580
	public Camera UICamera;

	// Token: 0x0400062D RID: 1581
	public Camera AltZoomCamera;

	// Token: 0x0400062E RID: 1582
	public Camera SpectatorCamera;

	// Token: 0x0400062F RID: 1583
	public Camera SpectatorAltZoomCamera;

	// Token: 0x04000630 RID: 1584
	public Camera HandCamera;

	// Token: 0x04000631 RID: 1585
	public HandCamera HandViewCamera;

	// Token: 0x04000632 RID: 1586
	public SpectatorViewCamera SpectatorViewCamera;

	// Token: 0x04000633 RID: 1587
	public Material DefaultSkyboxMaterial;

	// Token: 0x04000634 RID: 1588
	public Material PureSkyboxMaterial;

	// Token: 0x04000635 RID: 1589
	public Camera[] AllCameras;
}
