using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000416 RID: 1046
	public static class InputHelper
	{
		// Token: 0x06003085 RID: 12421 RVA: 0x0014CBE6 File Offset: 0x0014ADE6
		public static bool IsAnyCtrlOrCommandKeyPressed()
		{
			return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x0014CC18 File Offset: 0x0014AE18
		public static bool IsAnyShiftKeyPressed()
		{
			return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x0014CC32 File Offset: 0x0014AE32
		public static bool WasLeftMouseButtonPressedInCurrentFrame()
		{
			return Input.GetMouseButtonDown(0);
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x0014CC3A File Offset: 0x0014AE3A
		public static bool WasLeftMouseButtonReleasedInCurrentFrame()
		{
			return Input.GetMouseButtonUp(0);
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x0014CC42 File Offset: 0x0014AE42
		public static bool WasRightMouseButtonPressedInCurrentFrame()
		{
			return Input.GetMouseButtonDown(1);
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x0014CC4A File Offset: 0x0014AE4A
		public static bool WasRightMouseButtonReleasedInCurrentFrame()
		{
			return Input.GetMouseButtonUp(1);
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x0014CC52 File Offset: 0x0014AE52
		public static bool WasMiddleMouseButtonPressedInCurrentFrame()
		{
			return Input.GetMouseButtonDown(2);
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x0014CC5A File Offset: 0x0014AE5A
		public static bool WasMiddleMouseButtonReleasedInCurrentFrame()
		{
			return Input.GetMouseButtonUp(2);
		}
	}
}
