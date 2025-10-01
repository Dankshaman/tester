using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Camera")]
public class UIDragCamera : MonoBehaviour
{
	// Token: 0x0600011B RID: 283 RVA: 0x000075F8 File Offset: 0x000057F8
	private void Awake()
	{
		if (this.draggableCamera == null)
		{
			this.draggableCamera = NGUITools.FindInParents<UIDraggableCamera>(base.gameObject);
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00007619 File Offset: 0x00005819
	private void OnPress(bool isPressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Press(isPressed);
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000764A File Offset: 0x0000584A
	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Drag(delta);
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x0000767B File Offset: 0x0000587B
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.draggableCamera != null)
		{
			this.draggableCamera.Scroll(delta);
		}
	}

	// Token: 0x040000E4 RID: 228
	public UIDraggableCamera draggableCamera;
}
