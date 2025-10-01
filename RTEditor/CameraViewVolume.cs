using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003C7 RID: 967
	public class CameraViewVolume
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06002D65 RID: 11621 RVA: 0x0013B6D4 File Offset: 0x001398D4
		public Ray3D[] WorldSpaceVolumeEdgeRays
		{
			get
			{
				return this._worldSpaceVolumeEdgeRays.Clone() as Ray3D[];
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06002D66 RID: 11622 RVA: 0x0013B6E6 File Offset: 0x001398E6
		public Vector3 TopLeftPointOnNearPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[0];
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002D67 RID: 11623 RVA: 0x0013B6F4 File Offset: 0x001398F4
		public Vector3 TopRightPointOnNearPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[1];
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002D68 RID: 11624 RVA: 0x0013B702 File Offset: 0x00139902
		public Vector3 BottomRightPointOnNearPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[2];
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002D69 RID: 11625 RVA: 0x0013B710 File Offset: 0x00139910
		public Vector3 BottomLeftPointOnNearPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[3];
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002D6A RID: 11626 RVA: 0x0013B71E File Offset: 0x0013991E
		public Vector3 TopLeftPointOnFarPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[4];
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06002D6B RID: 11627 RVA: 0x0013B72C File Offset: 0x0013992C
		public Vector3 TopRightPointOnFarPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[5];
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06002D6C RID: 11628 RVA: 0x0013B73A File Offset: 0x0013993A
		public Vector3 BottomRightPointOnFarPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[6];
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002D6D RID: 11629 RVA: 0x0013B748 File Offset: 0x00139948
		public Vector3 BottomLeftPointOnFarPlane
		{
			get
			{
				return this._worldSpaceVolumePoints[7];
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002D6E RID: 11630 RVA: 0x0013B756 File Offset: 0x00139956
		public float FarClipPlaneDistance
		{
			get
			{
				return this._farClipPlaneDistance;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06002D6F RID: 11631 RVA: 0x0013B75E File Offset: 0x0013995E
		public float NearClipPlaneDistance
		{
			get
			{
				return this._nearClipPlaneDistance;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002D70 RID: 11632 RVA: 0x0013B766 File Offset: 0x00139966
		public Vector2 NearPlaneSize
		{
			get
			{
				return this._nearPlaneSize;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002D71 RID: 11633 RVA: 0x0013B76E File Offset: 0x0013996E
		public Vector2 FarPlaneSize
		{
			get
			{
				return this._farPlaneSize;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06002D72 RID: 11634 RVA: 0x0013B776 File Offset: 0x00139976
		public Bounds AABB
		{
			get
			{
				return this._aabb;
			}
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x00002594 File Offset: 0x00000794
		public CameraViewVolume()
		{
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x0013B77E File Offset: 0x0013997E
		public CameraViewVolume(Camera camera, float desiredCameraFarClipPlane)
		{
			this.BuildForCamera(camera, desiredCameraFarClipPlane);
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0013B790 File Offset: 0x00139990
		public void BuildForCamera(Camera camera, float desiredCameraFarClipPlane)
		{
			float farClipPlane = camera.farClipPlane;
			this.AdjustCameraFarClipPlane(camera, desiredCameraFarClipPlane);
			this.CalculateWorldSpacePoints(camera);
			this.CalculateWorldSpacePlanes(camera);
			this.CalculateWorldSpaceVolumeEdgeRays();
			camera.farClipPlane = farClipPlane;
			this._farClipPlaneDistance = desiredCameraFarClipPlane;
			this._nearClipPlaneDistance = camera.nearClipPlane;
			Transform transform = camera.transform;
			Vector3 lhs = this.TopLeftPointOnNearPlane - this.BottomRightPointOnNearPlane;
			float x = Mathf.Abs(Vector3.Dot(lhs, transform.right));
			float y = Mathf.Abs(Vector3.Dot(lhs, transform.up));
			this._nearPlaneSize = new Vector2(x, y);
			Vector3 lhs2 = this.TopLeftPointOnFarPlane - this.BottomRightPointOnFarPlane;
			x = Mathf.Abs(Vector3.Dot(lhs2, transform.right));
			y = Mathf.Abs(Vector3.Dot(lhs2, transform.up));
			this._farPlaneSize = new Vector2(x, y);
			this._aabb = BoundsExtensions.FromPointCloud(new List<Vector3>(this._worldSpaceVolumePoints));
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x0013B879 File Offset: 0x00139A79
		public bool ContainsWorldSpaceAABB(Bounds worldSpaceAABB)
		{
			return GeometryUtility.TestPlanesAABB(this._worldSpacePlanes, worldSpaceAABB);
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x0013B888 File Offset: 0x00139A88
		private void CalculateWorldSpacePoints(Camera camera)
		{
			CameraViewVolumePointsCalculator cameraViewVolumePointsCalculator = CameraViewVolumePointsCalculatorFactory.Create(camera);
			this._worldSpaceVolumePoints = cameraViewVolumePointsCalculator.CalculateWorldSpaceVolumePoints(camera);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x0013B8A9 File Offset: 0x00139AA9
		private void CalculateWorldSpacePlanes(Camera camera)
		{
			this._worldSpacePlanes = GeometryUtility.CalculateFrustumPlanes(camera);
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x0013B8B7 File Offset: 0x00139AB7
		private void CalculateWorldSpaceVolumeEdgeRays()
		{
			this._worldSpaceVolumeEdgeRays = CameraViewVolumeEdgeRaysCalculator.CalculateWorldSpaceVolumeEdgeRays(this);
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x0013B8C5 File Offset: 0x00139AC5
		private void AdjustCameraFarClipPlane(Camera camera, float desiredFarClipPlane)
		{
			camera.farClipPlane = desiredFarClipPlane;
			this.EnsureFarClipPlaneSitsInFrontOfNearPlane(camera);
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x0013B8D5 File Offset: 0x00139AD5
		private void EnsureFarClipPlaneSitsInFrontOfNearPlane(Camera camera)
		{
			if (camera.farClipPlane <= camera.nearClipPlane)
			{
				camera.farClipPlane = camera.nearClipPlane + 0.1f;
			}
		}

		// Token: 0x04001E62 RID: 7778
		private Vector3[] _worldSpaceVolumePoints;

		// Token: 0x04001E63 RID: 7779
		private Plane[] _worldSpacePlanes;

		// Token: 0x04001E64 RID: 7780
		private Ray3D[] _worldSpaceVolumeEdgeRays;

		// Token: 0x04001E65 RID: 7781
		private Vector2 _nearPlaneSize;

		// Token: 0x04001E66 RID: 7782
		private Vector2 _farPlaneSize;

		// Token: 0x04001E67 RID: 7783
		private float _farClipPlaneDistance;

		// Token: 0x04001E68 RID: 7784
		private float _nearClipPlaneDistance;

		// Token: 0x04001E69 RID: 7785
		private Bounds _aabb;
	}
}
