using System;
using UnityEngine;

// Token: 0x02000239 RID: 569
public class SpectatorViewCamera : ViewCamera
{
	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06001C29 RID: 7209 RVA: 0x000C1EDE File Offset: 0x000C00DE
	public Vector3 Offset
	{
		get
		{
			return this.ScreenCamera.WorldToScreenPoint(this.texture.worldCorners[0]);
		}
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x000C1EFC File Offset: 0x000C00FC
	protected override void Awake()
	{
		base.Awake();
		this.texture = base.GetComponent<UITexture>();
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x000C1F10 File Offset: 0x000C0110
	protected override Rect GetScreenRect()
	{
		Vector2 vector = this.ScreenCamera.WorldToScreenPoint(this.texture.worldCorners[0]);
		Vector2 size = this.ScreenCamera.WorldToScreenPoint(this.texture.worldCorners[2]) - vector;
		return new Rect(vector, size);
	}

	// Token: 0x040011DD RID: 4573
	public Camera ScreenCamera;

	// Token: 0x040011DE RID: 4574
	private UITexture texture;
}
