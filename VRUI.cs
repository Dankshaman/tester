using System;
using UnityEngine;

// Token: 0x0200037C RID: 892
public class VRUI : MonoBehaviour
{
	// Token: 0x060029E2 RID: 10722 RVA: 0x0012B304 File Offset: 0x00129504
	public void SetAttachment(VRControllerAttachment attachment)
	{
		if (this.VRUIAttached != attachment)
		{
			this.VRUIAttached = attachment;
			if (Singleton<VRHMD>.Instance.floatingUI && this.VRUIAttached == VRControllerAttachment.Detached && Singleton<VRHMD>.Instance.Initialized)
			{
				Singleton<VRHMD>.Instance.floatingUI = false;
				Singleton<VRHMD>.Instance.ResetUITransform();
				Singleton<VRHMD>.Instance.floatingUI = true;
				Singleton<VRHMD>.Instance.ResetUITransform();
			}
		}
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x0012B36B File Offset: 0x0012956B
	private void Start()
	{
		this.SetAttachment(this.VRUIAttached);
		base.GetComponent<Collider>().enabled = false;
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x0012B388 File Offset: 0x00129588
	private void LateUpdate()
	{
		this.Active = true;
		this.lookingAtUI = false;
		if (Singleton<VRHMD>.Instance.floatingUI && this.VRUIAttached != VRControllerAttachment.Detached)
		{
			this.Active = false;
			if (!VRUI.SUPPRESS_SCREEN)
			{
				this.controller = ((this.VRUIAttached == VRControllerAttachment.OnLeft) ? VRTrackedController.leftVRTrackedController : VRTrackedController.rightVRTrackedController);
				if (this.controller)
				{
					Vector3 normalized = (this.controller.WorldPosition() - this.controller.HMD.transform.position).normalized;
					this.lookingAtUI = (Vector3.Dot(normalized, (this.VRUIAttached == VRControllerAttachment.OnLeft) ? this.controller.transform.right : (-this.controller.transform.right)) > 0.6f);
					if (this.lookingAtUI)
					{
						this.Active = true;
						base.transform.position = this.controller.transform.position;
						base.transform.rotation = this.controller.transform.rotation;
						base.transform.Rotate(0f, 0f, (float)((this.VRUIAttached == VRControllerAttachment.OnLeft) ? -90 : 90));
						base.transform.Rotate(180f, (float)((this.VRUIAttached == VRControllerAttachment.OnLeft) ? 90 : -90), 0f);
						base.transform.Translate(Vector3.up * 3f, Space.Self);
						base.transform.localScale = new Vector3(this.ActiveScale * Singleton<VRHMD>.Instance.AspectRatio, 0.1f, this.ActiveScale);
					}
				}
			}
		}
		base.GetComponent<Renderer>().enabled = this.Active;
		if (this.lookingAtUI != this.previousLookingAtUI && this.controller)
		{
			if (this.VRUIAttached == Singleton<VRVirtualHand>.Instance.Attach)
			{
				Singleton<VRVirtualHand>.Instance.ToggleVirtualHand(!this.lookingAtUI, this.controller);
			}
			this.previousLookingAtUI = this.lookingAtUI;
		}
	}

	// Token: 0x04001C9B RID: 7323
	public static bool SUPPRESS_SCREEN;

	// Token: 0x04001C9C RID: 7324
	public VRControllerAttachment VRUIAttached;

	// Token: 0x04001C9D RID: 7325
	[NonSerialized]
	public float ActiveScale = 0.03f;

	// Token: 0x04001C9E RID: 7326
	[NonSerialized]
	public bool Active;

	// Token: 0x04001C9F RID: 7327
	private bool lookingAtUI;

	// Token: 0x04001CA0 RID: 7328
	private bool previousLookingAtUI;

	// Token: 0x04001CA1 RID: 7329
	private VRTrackedController controller;
}
