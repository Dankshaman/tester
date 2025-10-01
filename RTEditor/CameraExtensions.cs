using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F3 RID: 1011
	public static class CameraExtensions
	{
		// Token: 0x06002E66 RID: 11878 RVA: 0x0013EBF4 File Offset: 0x0013CDF4
		public static Plane GetNearPlane(this Camera camera)
		{
			Transform transform = camera.transform;
			return new Plane(transform.forward, transform.position + transform.forward * camera.nearClipPlane);
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x0013EC2F File Offset: 0x0013CE2F
		public static CameraViewVolume GetViewVolume(this Camera camera)
		{
			CameraViewVolume cameraViewVolume = new CameraViewVolume();
			cameraViewVolume.BuildForCamera(camera, camera.farClipPlane);
			return cameraViewVolume;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x0013EC43 File Offset: 0x0013CE43
		public static CameraViewVolume GetViewVolume(this Camera camera, float desiredFarClipPlane)
		{
			CameraViewVolume cameraViewVolume = new CameraViewVolume();
			cameraViewVolume.BuildForCamera(camera, desiredFarClipPlane);
			return cameraViewVolume;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x0013EC54 File Offset: 0x0013CE54
		public static List<GameObject> GetVisibleGameObjects(this Camera camera)
		{
			CameraViewVolume cameraViewVolume = new CameraViewVolume();
			cameraViewVolume.BuildForCamera(camera, camera.farClipPlane);
			List<GameObject> pottentiallyVisibleGameObjects = camera.GetPottentiallyVisibleGameObjects();
			List<GameObject> list = new List<GameObject>(pottentiallyVisibleGameObjects.Count);
			foreach (GameObject gameObject in pottentiallyVisibleGameObjects)
			{
				Box worldBox = gameObject.GetWorldBox();
				if (!worldBox.IsInvalid() && cameraViewVolume.ContainsWorldSpaceAABB(worldBox.ToBounds()))
				{
					list.Add(gameObject);
				}
			}
			return list;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x0013ECE8 File Offset: 0x0013CEE8
		public static bool IsGameObjectVisible(this Camera camera, GameObject gameObject)
		{
			CameraViewVolume cameraViewVolume = new CameraViewVolume();
			cameraViewVolume.BuildForCamera(camera, camera.farClipPlane);
			Box worldBox = gameObject.GetWorldBox();
			return !worldBox.IsInvalid() && cameraViewVolume.ContainsWorldSpaceAABB(worldBox.ToBounds());
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x0013ED28 File Offset: 0x0013CF28
		private static List<GameObject> GetPottentiallyVisibleGameObjects(this Camera camera)
		{
			Box box = Box.FromBounds(camera.GetViewVolume().AABB);
			return SingletonBase<EditorScene>.Instance.OverlapBox(box, ObjectOverlapPrecision.ObjectBox);
		}
	}
}
