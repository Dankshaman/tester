using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
	// Token: 0x06000153 RID: 339 RVA: 0x00008F4C File Offset: 0x0000714C
	private void OnDragStart()
	{
		if (this.target != null)
		{
			Vector3[] worldCorners = this.target.worldCorners;
			this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				this.mRayPos = currentRay.GetPoint(distance);
				this.mLocalPos = this.target.cachedTransform.localPosition;
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00008FFC File Offset: 0x000071FC
	private void OnDrag(Vector2 delta)
	{
		if (this.mDragging && this.target != null)
		{
			Ray currentRay = UICamera.currentRay;
			float distance;
			if (this.mPlane.Raycast(currentRay, out distance))
			{
				Transform cachedTransform = this.target.cachedTransform;
				cachedTransform.localPosition = this.mLocalPos;
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 b = currentRay.GetPoint(distance) - this.mRayPos;
				cachedTransform.position += b;
				Vector3 vector = Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos);
				cachedTransform.localPosition = this.mLocalPos;
				NGUIMath.ResizeWidget(this.target, this.pivot, vector.x, vector.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
				if (this.updateAnchors)
				{
					this.target.BroadcastMessage("UpdateAnchors");
				}
				if (this.target.GetComponent<UIDragObject>())
				{
					this.target.GetComponent<UIDragObject>().UpdateDropShadow();
				}
			}
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000913E File Offset: 0x0000733E
	private void OnDragEnd()
	{
		this.mDragging = false;
	}

	// Token: 0x04000119 RID: 281
	public UIWidget target;

	// Token: 0x0400011A RID: 282
	public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;

	// Token: 0x0400011B RID: 283
	public int minWidth = 100;

	// Token: 0x0400011C RID: 284
	public int minHeight = 100;

	// Token: 0x0400011D RID: 285
	public int maxWidth = 100000;

	// Token: 0x0400011E RID: 286
	public int maxHeight = 100000;

	// Token: 0x0400011F RID: 287
	public bool updateAnchors;

	// Token: 0x04000120 RID: 288
	private Plane mPlane;

	// Token: 0x04000121 RID: 289
	private Vector3 mRayPos;

	// Token: 0x04000122 RID: 290
	private Vector3 mLocalPos;

	// Token: 0x04000123 RID: 291
	private int mWidth;

	// Token: 0x04000124 RID: 292
	private int mHeight;

	// Token: 0x04000125 RID: 293
	private bool mDragging;
}
