using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003C4 RID: 964
	public static class EditorCameraZoom
	{
		// Token: 0x06002D4F RID: 11599 RVA: 0x0013B4F8 File Offset: 0x001396F8
		public static void ZoomCamera(Camera camera, float zoomAmount)
		{
			float d = EditorCameraZoom.ZoomOrthoCameraViewVolume(camera, zoomAmount);
			if (!camera.orthographic)
			{
				d = 1f;
			}
			Transform transform = camera.transform;
			transform.position += transform.forward * zoomAmount * d;
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x0013B54C File Offset: 0x0013974C
		public static float ZoomOrthoCameraViewVolume(Camera camera, float zoomAmount)
		{
			float result = 1f;
			float num = camera.orthographicSize - zoomAmount * 0.5f;
			if (num < 0.001f)
			{
				float num2 = (0.001f - num) / zoomAmount;
				num = 0.001f;
				result = 1f - num2;
			}
			camera.orthographicSize = Mathf.Max(0.001f, num);
			return result;
		}
	}
}
