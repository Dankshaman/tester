using System;
using UnityEngine;

// Token: 0x02000248 RID: 584
public class StopControllerSpin : MonoBehaviour
{
	// Token: 0x06001D33 RID: 7475 RVA: 0x000C8CAC File Offset: 0x000C6EAC
	private void Update()
	{
		if (!this.bFirstUpdate && !(this.ControllerPointer != Vector2.zero) && !(this.ControllerCamera != Vector2.zero) && !(this.ControllerMove != Vector2.zero))
		{
			Debug.Log("Destroy Stop Controller Spin");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Vector2 vector = new Vector2(zInput.GetAxis("Pointer Horizontal", ControlType.Controller, true), zInput.GetAxis("Pointer Vertical", ControlType.Controller, true));
		Vector2 vector2 = new Vector2(zInput.GetAxis("Move Horizontal", ControlType.Controller, true), zInput.GetAxis("Move Vertical", ControlType.Controller, true));
		Vector2 vector3 = new Vector2(zInput.GetAxis("Camera Horizontal", ControlType.Controller, true), zInput.GetAxis("Camera Vertical", ControlType.Controller, true));
		if (this.ControllerPointer == Vector2.zero)
		{
			if (zInput.bController)
			{
				this.ControllerPointer = vector;
			}
		}
		else if (vector != Vector2.zero)
		{
			zInput.bController = false;
		}
		else
		{
			zInput.bController = true;
			this.ControllerPointer = Vector2.zero;
		}
		if (this.ControllerMove == Vector2.zero)
		{
			if (zInput.bController)
			{
				this.ControllerMove = vector2;
			}
		}
		else if (vector2 != Vector2.zero)
		{
			zInput.bController = false;
		}
		else
		{
			zInput.bController = true;
			this.ControllerMove = Vector2.zero;
		}
		if (this.ControllerCamera == Vector2.zero)
		{
			if (zInput.bController)
			{
				this.ControllerCamera = vector3;
			}
		}
		else if (vector3 != Vector2.zero)
		{
			zInput.bController = false;
		}
		else
		{
			zInput.bController = true;
			this.ControllerCamera = Vector2.zero;
		}
		if (this.FrameCount > 10)
		{
			this.bFirstUpdate = false;
			return;
		}
		this.FrameCount++;
	}

	// Token: 0x040012AA RID: 4778
	private bool bFirstUpdate = true;

	// Token: 0x040012AB RID: 4779
	private const int CheckFrames = 10;

	// Token: 0x040012AC RID: 4780
	private int FrameCount = 1;

	// Token: 0x040012AD RID: 4781
	private Vector2 ControllerPointer = Vector2.zero;

	// Token: 0x040012AE RID: 4782
	private Vector2 ControllerMove = Vector2.zero;

	// Token: 0x040012AF RID: 4783
	private Vector2 ControllerCamera = Vector2.zero;
}
