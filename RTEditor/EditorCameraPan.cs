using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003BF RID: 959
	public static class EditorCameraPan
	{
		// Token: 0x06002D3D RID: 11581 RVA: 0x0013B3D4 File Offset: 0x001395D4
		public static void PanCamera(Camera camera, float panAmountRight, float panAmountUp)
		{
			Transform transform = camera.transform;
			transform.position += transform.right * panAmountRight + transform.up * panAmountUp;
		}
	}
}
