using System;
using UnityEngine;

// Token: 0x02000385 RID: 901
public class ZoomCamera : MonoBehaviour
{
	// Token: 0x06002A70 RID: 10864 RVA: 0x0012E65E File Offset: 0x0012C85E
	private void Start()
	{
		this.ZoomCameraCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x0012E66C File Offset: 0x0012C86C
	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(this.ZoomCameraCamera.pixelRect.xMin, (float)this.ZoomCameraCamera.pixelHeight * this.yPixelMulti - this.ZoomCameraCamera.pixelRect.yMax, (float)(this.ZoomCameraCamera.pixelWidth + 4), (float)(this.ZoomCameraCamera.pixelHeight + 4)), this.Border);
	}

	// Token: 0x04001CD5 RID: 7381
	public Texture2D Border;

	// Token: 0x04001CD6 RID: 7382
	[SerializeField]
	private float yPixelMulti = 2.5f;

	// Token: 0x04001CD7 RID: 7383
	private Camera ZoomCameraCamera;
}
