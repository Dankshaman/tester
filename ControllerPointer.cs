using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class ControllerPointer : MonoBehaviour
{
	// Token: 0x060009E6 RID: 2534 RVA: 0x00045C74 File Offset: 0x00043E74
	private void FixedUpdate()
	{
		if ((zInput.GetAxis("Pointer Horizontal", ControlType.Controller, false) != 0f || zInput.GetAxis("Pointer Vertical", ControlType.Controller, false) != 0f) && !ProMouse.Instance.MouseBusy && zInput.bController)
		{
			float num = ControllerPointer.Speed * (float)(Screen.width / 1920);
			if (num == 0f)
			{
				num = ControllerPointer.Speed;
			}
			Vector2 vector = new Vector2(zInput.GetAxis("Pointer Horizontal", ControlType.Controller, false), zInput.GetAxis("Pointer Vertical", ControlType.Controller, false));
			vector = vector.normalized * ((vector.magnitude - ControllerPointer.Deadzone) / (1f - ControllerPointer.Deadzone));
			float x = vector.x;
			float y = vector.y;
			ProMouse.Instance.SetCursorPosition((int)Mathf.Round(Input.mousePosition.x + num * x), (int)Mathf.Round(Input.mousePosition.y + num * y));
		}
	}

	// Token: 0x0400070D RID: 1805
	public static float Speed = 28f;

	// Token: 0x0400070E RID: 1806
	public static float Deadzone = 0.1f;
}
