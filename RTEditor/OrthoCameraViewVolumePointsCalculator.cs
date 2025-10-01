using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003CD RID: 973
	public class OrthoCameraViewVolumePointsCalculator : CameraViewVolumePointsCalculator
	{
		// Token: 0x06002D80 RID: 11648 RVA: 0x0013BAC8 File Offset: 0x00139CC8
		public override Vector3[] CalculateWorldSpaceVolumePoints(Camera camera)
		{
			Transform transform = camera.transform;
			Vector3 position = transform.position;
			Vector3 right = transform.right;
			Vector3 up = transform.up;
			Vector3 forward = transform.forward;
			float orthographicSize = camera.orthographicSize;
			float d = orthographicSize * camera.aspect;
			return new Vector3[]
			{
				position + forward * camera.nearClipPlane - right * d + up * orthographicSize,
				position + forward * camera.nearClipPlane + right * d + up * orthographicSize,
				position + forward * camera.nearClipPlane + right * d - up * orthographicSize,
				position + forward * camera.nearClipPlane - right * d - up * orthographicSize,
				position + forward * camera.farClipPlane - right * d + up * orthographicSize,
				position + forward * camera.farClipPlane + right * d + up * orthographicSize,
				position + forward * camera.farClipPlane + right * d - up * orthographicSize,
				position + forward * camera.farClipPlane - right * d - up * orthographicSize
			};
		}
	}
}
