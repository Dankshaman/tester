using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003CE RID: 974
	public class PerspectiveCameraViewVolumePointsCalculator : CameraViewVolumePointsCalculator
	{
		// Token: 0x06002D82 RID: 11650 RVA: 0x0013BCB0 File Offset: 0x00139EB0
		public override Vector3[] CalculateWorldSpaceVolumePoints(Camera camera)
		{
			Transform transform = camera.transform;
			Vector3 position = transform.position;
			Vector3 right = transform.right;
			Vector3 up = transform.up;
			Vector3 forward = transform.forward;
			float num = Mathf.Tan(camera.fieldOfView * 0.5f * 0.017453292f);
			float num2 = num * camera.aspect;
			float d = num2 * camera.nearClipPlane;
			float d2 = num2 * camera.farClipPlane;
			float d3 = num * camera.nearClipPlane;
			float d4 = num * camera.farClipPlane;
			return new Vector3[]
			{
				position + forward * camera.nearClipPlane - right * d + up * d3,
				position + forward * camera.nearClipPlane + right * d + up * d3,
				position + forward * camera.nearClipPlane + right * d - up * d3,
				position + forward * camera.nearClipPlane - right * d - up * d3,
				position + forward * camera.farClipPlane - right * d2 + up * d4,
				position + forward * camera.farClipPlane + right * d2 + up * d4,
				position + forward * camera.farClipPlane + right * d2 - up * d4,
				position + forward * camera.farClipPlane - right * d2 - up * d4
			};
		}
	}
}
