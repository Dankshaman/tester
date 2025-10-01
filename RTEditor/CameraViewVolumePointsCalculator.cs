using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003CB RID: 971
	public abstract class CameraViewVolumePointsCalculator
	{
		// Token: 0x06002D7D RID: 11645
		public abstract Vector3[] CalculateWorldSpaceVolumePoints(Camera camera);
	}
}
