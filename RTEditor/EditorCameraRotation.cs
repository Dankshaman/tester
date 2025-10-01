using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003C2 RID: 962
	public static class EditorCameraRotation
	{
		// Token: 0x06002D4E RID: 11598 RVA: 0x0013B4D4 File Offset: 0x001396D4
		public static void RotateCamera(Camera camera, float degreesCameraRight, float degreesGlobalUp)
		{
			Transform transform = camera.transform;
			transform.Rotate(transform.right, degreesCameraRight, Space.World);
			transform.Rotate(Vector3.up, degreesGlobalUp, Space.World);
		}
	}
}
