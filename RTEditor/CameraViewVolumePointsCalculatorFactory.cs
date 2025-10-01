using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003CC RID: 972
	public static class CameraViewVolumePointsCalculatorFactory
	{
		// Token: 0x06002D7F RID: 11647 RVA: 0x0013BAB3 File Offset: 0x00139CB3
		public static CameraViewVolumePointsCalculator Create(Camera camera)
		{
			if (camera.orthographic)
			{
				return new OrthoCameraViewVolumePointsCalculator();
			}
			return new PerspectiveCameraViewVolumePointsCalculator();
		}
	}
}
