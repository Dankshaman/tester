using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class CurrentControlType : MonoBehaviour
{
	// Token: 0x06000A00 RID: 2560 RVA: 0x000463D0 File Offset: 0x000445D0
	private void Update()
	{
		if (VRHMD.isVR)
		{
			zInput.CurrentControlType = ControlType.VR;
			return;
		}
		if (zInput.bTouching)
		{
			zInput.CurrentControlType = ControlType.Touch;
			return;
		}
		if (zInput.GetButtonDown("Grab", ControlType.Keyboard))
		{
			zInput.CurrentControlType = ControlType.Keyboard;
			return;
		}
		if (zInput.GetButtonDown("Grab", ControlType.Controller) || zInput.GetButtonDown("Tap", ControlType.Controller))
		{
			zInput.CurrentControlType = ControlType.Controller;
			return;
		}
	}
}
