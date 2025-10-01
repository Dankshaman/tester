using System;

namespace RTEditor
{
	// Token: 0x020003C8 RID: 968
	public static class CameraViewVolumeEdgeRaysCalculator
	{
		// Token: 0x06002D7C RID: 11644 RVA: 0x0013B8F8 File Offset: 0x00139AF8
		public static Ray3D[] CalculateWorldSpaceVolumeEdgeRays(CameraViewVolume cameraViewVolume)
		{
			return new Ray3D[]
			{
				new Ray3D(cameraViewVolume.TopLeftPointOnNearPlane, cameraViewVolume.TopRightPointOnNearPlane - cameraViewVolume.TopLeftPointOnNearPlane),
				new Ray3D(cameraViewVolume.TopRightPointOnNearPlane, cameraViewVolume.BottomRightPointOnNearPlane - cameraViewVolume.TopRightPointOnNearPlane),
				new Ray3D(cameraViewVolume.BottomRightPointOnNearPlane, cameraViewVolume.BottomLeftPointOnNearPlane - cameraViewVolume.BottomRightPointOnNearPlane),
				new Ray3D(cameraViewVolume.BottomLeftPointOnNearPlane, cameraViewVolume.TopLeftPointOnNearPlane - cameraViewVolume.BottomLeftPointOnNearPlane),
				new Ray3D(cameraViewVolume.TopLeftPointOnFarPlane, cameraViewVolume.TopRightPointOnFarPlane - cameraViewVolume.TopLeftPointOnFarPlane),
				new Ray3D(cameraViewVolume.TopRightPointOnFarPlane, cameraViewVolume.BottomRightPointOnFarPlane - cameraViewVolume.TopRightPointOnFarPlane),
				new Ray3D(cameraViewVolume.BottomRightPointOnFarPlane, cameraViewVolume.BottomLeftPointOnFarPlane - cameraViewVolume.BottomRightPointOnFarPlane),
				new Ray3D(cameraViewVolume.BottomLeftPointOnFarPlane, cameraViewVolume.TopLeftPointOnFarPlane - cameraViewVolume.BottomLeftPointOnFarPlane),
				new Ray3D(cameraViewVolume.TopLeftPointOnNearPlane, cameraViewVolume.TopLeftPointOnFarPlane - cameraViewVolume.TopLeftPointOnNearPlane),
				new Ray3D(cameraViewVolume.TopRightPointOnNearPlane, cameraViewVolume.TopRightPointOnFarPlane - cameraViewVolume.TopRightPointOnNearPlane),
				new Ray3D(cameraViewVolume.BottomRightPointOnNearPlane, cameraViewVolume.BottomRightPointOnFarPlane - cameraViewVolume.BottomRightPointOnNearPlane),
				new Ray3D(cameraViewVolume.BottomLeftPointOnNearPlane, cameraViewVolume.BottomLeftPointOnFarPlane - cameraViewVolume.BottomLeftPointOnNearPlane)
			};
		}
	}
}
