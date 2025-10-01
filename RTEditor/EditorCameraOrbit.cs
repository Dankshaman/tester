using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003BE RID: 958
	public static class EditorCameraOrbit
	{
		// Token: 0x06002D3C RID: 11580 RVA: 0x0013B37C File Offset: 0x0013957C
		public static void OrbitCamera(Camera camera, float degreesCameraRight, float degreesGlobalUp, Vector3 orbitPoint)
		{
			Transform transform = camera.transform;
			if ((transform.position - orbitPoint).magnitude < 1E-05f)
			{
				return;
			}
			transform.RotateAround(orbitPoint, Vector3.up, degreesGlobalUp);
			transform.RotateAround(orbitPoint, transform.right, degreesCameraRight);
			transform.LookAt(orbitPoint, transform.up);
		}
	}
}
