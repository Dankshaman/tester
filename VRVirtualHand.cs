using System;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class VRVirtualHand : Singleton<VRVirtualHand>
{
	// Token: 0x060029E7 RID: 10727 RVA: 0x0012B5BC File Offset: 0x001297BC
	private void Update()
	{
		if (this.controller != this.controllerFromHandState(this.Attach))
		{
			this.Refresh();
		}
		if (this.controller)
		{
			this.VirtualHand.transform.position = this.controller.GrabSphere.transform.position;
			this.VirtualHand.transform.rotation = this.controller.transform.rotation;
			this.VirtualHand.transform.Rotate(new Vector3(VRVirtualHand.HAND_VIEW_ANGLE, 0f, 0f));
			this.VirtualHand.transform.Translate(new Vector3(0f, 2f, 3f));
		}
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x0012B685 File Offset: 0x00129885
	public void SetVirtualHandAttachment(VRControllerAttachment attachment)
	{
		this.Attach = attachment;
		this.controller = this.controllerFromHandState(this.Attach);
		this.VirtualHand.SetActive(this.Attach != VRControllerAttachment.Detached && VRHMD.isVR);
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x0012B6BB File Offset: 0x001298BB
	public void Refresh()
	{
		this.SetVirtualHandAttachment(this.Attach);
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x0012B6C9 File Offset: 0x001298C9
	public void TriggerHapticPulse(float intensity)
	{
		if (this.controller)
		{
			this.controller.controller.TriggerHapticPulse(intensity);
		}
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x0012B6EC File Offset: 0x001298EC
	public void ToggleVirtualHand(bool active, VRTrackedController ifThisController)
	{
		if (this.controller == ifThisController)
		{
			if (active)
			{
				if (this.suppressionCount > 0)
				{
					this.suppressionCount--;
				}
				if (this.suppressionCount == 0)
				{
					this.VirtualHand.SetActive(true);
					return;
				}
			}
			else
			{
				this.suppressionCount++;
				this.VirtualHand.SetActive(false);
			}
		}
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x0012B750 File Offset: 0x00129950
	private VRTrackedController controllerFromHandState(VRControllerAttachment vhs)
	{
		if (vhs == VRControllerAttachment.OnLeft)
		{
			return VRTrackedController.leftVRTrackedController;
		}
		if (vhs == VRControllerAttachment.OnRight)
		{
			return VRTrackedController.rightVRTrackedController;
		}
		return null;
	}

	// Token: 0x04001CA2 RID: 7330
	public static float HAND_VIEW_ANGLE = -4f;

	// Token: 0x04001CA3 RID: 7331
	public GameObject VirtualHand;

	// Token: 0x04001CA4 RID: 7332
	public VRControllerAttachment Attach = VRControllerAttachment.OnLeft;

	// Token: 0x04001CA5 RID: 7333
	private VRTrackedController controller;

	// Token: 0x04001CA6 RID: 7334
	private int suppressionCount;
}
