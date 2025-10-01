using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200010C RID: 268
public class FastDrag : MonoBehaviour
{
	// Token: 0x06000DDE RID: 3550 RVA: 0x0005955C File Offset: 0x0005775C
	private void Awake()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x0005956C File Offset: 0x0005776C
	private void OnWillRenderObject()
	{
		if (this.NPO.HeldByPlayerID == NetworkID.ID && Time.time >= this.NPO.DisableFastDragUntil)
		{
			if (XRSettings.enabled)
			{
				if (this.NPO.DesiredPosition != null)
				{
					this.actualPosition = new Vector3?(base.transform.position);
					base.transform.position = this.NPO.DesiredPosition.Value;
				}
				if (this.NPO.DesiredRotation != null)
				{
					this.actualRotation = new Quaternion?(base.transform.rotation);
					if (this.NPO.DesiredRotationStartTime == 0f)
					{
						base.transform.rotation = this.NPO.DesiredRotation.Value;
						return;
					}
					if (Time.time > this.NPO.DesiredRotationStartTime + 1f)
					{
						base.transform.rotation = this.NPO.DesiredRotation.Value;
						this.NPO.DesiredRotationStartTime = 0f;
						return;
					}
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.NPO.DesiredRotation.Value, (Time.time - this.NPO.DesiredRotationStartTime) / 1f);
					return;
				}
			}
			else if (this.NPO.DesiredPosition != null)
			{
				this.actualPosition = new Vector3?(base.transform.position);
				Vector3 value = this.NPO.DesiredPosition.Value;
				base.transform.position = new Vector3(value.x, base.transform.position.y, value.z);
			}
		}
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x00059738 File Offset: 0x00057938
	private void OnPostRender()
	{
		if (this.actualPosition != null)
		{
			base.transform.position = this.actualPosition.Value;
			this.actualPosition = null;
			this.NPO.DesiredPosition = null;
		}
		if (this.actualRotation != null)
		{
			base.transform.rotation = this.actualRotation.Value;
			this.actualRotation = null;
			this.NPO.DesiredRotation = null;
		}
	}

	// Token: 0x040008FE RID: 2302
	public static bool Enabled;

	// Token: 0x040008FF RID: 2303
	public const float PICKUP_DISABLE_DURATION = 0.5f;

	// Token: 0x04000900 RID: 2304
	public const float ANIMATION_DISABLE_DURATION = 0.5f;

	// Token: 0x04000901 RID: 2305
	private const float ROTATION_DISABLE_DURATION = 1f;

	// Token: 0x04000902 RID: 2306
	private NetworkPhysicsObject NPO;

	// Token: 0x04000903 RID: 2307
	private Vector3? actualPosition;

	// Token: 0x04000904 RID: 2308
	private Quaternion? actualRotation;
}
