using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200037F RID: 895
public class ViewCamera : MonoBehaviour
{
	// Token: 0x06002A00 RID: 10752 RVA: 0x0012BE26 File Offset: 0x0012A026
	protected virtual void Awake()
	{
		ViewCamera.ViewCameras.Add(this);
		if (this.camera == null)
		{
			this.camera = base.GetComponent<Camera>();
		}
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x0012BE4D File Offset: 0x0012A04D
	protected virtual void OnDestroy()
	{
		ViewCamera.ViewCameras.Remove(this);
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x0012BE5B File Offset: 0x0012A05B
	protected virtual Rect GetScreenRect()
	{
		return this.camera.pixelRect;
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x0012BE68 File Offset: 0x0012A068
	public virtual bool Contains(Vector2 screenPos)
	{
		return this.camera.gameObject.activeSelf && this.GetScreenRect().Contains(screenPos);
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x0012BE98 File Offset: 0x0012A098
	public static ViewCamera GetViewCamera(Vector2 screenPos)
	{
		for (int i = 0; i < ViewCamera.ViewCameras.Count; i++)
		{
			ViewCamera viewCamera = ViewCamera.ViewCameras[i];
			if (viewCamera.Contains(screenPos))
			{
				return viewCamera;
			}
		}
		return null;
	}

	// Token: 0x04001CAB RID: 7339
	public static List<ViewCamera> ViewCameras = new List<ViewCamera>();

	// Token: 0x04001CAC RID: 7340
	public Camera camera;
}
