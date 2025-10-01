using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/UI/Viewport Camera")]
public class UIViewport : MonoBehaviour
{
	// Token: 0x06000800 RID: 2048 RVA: 0x00038206 File Offset: 0x00036406
	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.main;
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00038230 File Offset: 0x00036430
	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			if (this.topLeft.gameObject.activeInHierarchy)
			{
				Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
				Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
				Rect rect = new Rect(vector.x / (float)Screen.width, vector2.y / (float)Screen.height, (vector2.x - vector.x) / (float)Screen.width, (vector.y - vector2.y) / (float)Screen.height);
				float num = this.fullSize * rect.height;
				if (rect != this.mCam.rect)
				{
					this.mCam.rect = rect;
				}
				if (this.mCam.orthographicSize != num)
				{
					this.mCam.orthographicSize = num;
				}
				this.mCam.enabled = true;
				return;
			}
			this.mCam.enabled = false;
		}
	}

	// Token: 0x0400058B RID: 1419
	public Camera sourceCamera;

	// Token: 0x0400058C RID: 1420
	public Transform topLeft;

	// Token: 0x0400058D RID: 1421
	public Transform bottomRight;

	// Token: 0x0400058E RID: 1422
	public float fullSize = 1f;

	// Token: 0x0400058F RID: 1423
	private Camera mCam;
}
