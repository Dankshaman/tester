using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000452 RID: 1106
	public class EditorScene : SingletonBase<EditorScene>
	{
		// Token: 0x0600329B RID: 12955 RVA: 0x00153FD5 File Offset: 0x001521D5
		public void Update()
		{
			if (!MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				this._gameObjectSphereTree.Update();
			}
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00153FF0 File Offset: 0x001521F0
		public List<GameObjectRayHit> RaycastAllBox(Ray ray)
		{
			if (!MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return this._gameObjectSphereTree.RaycastAllBox(ray);
			}
			RaycastHit[] array = Physics.RaycastAll(ray);
			List<GameObjectRayHit> list = new List<GameObjectRayHit>();
			foreach (RaycastHit raycastHit in array)
			{
				GameObject gameObject = raycastHit.collider.gameObject;
				if (!(gameObject == null) && gameObject.activeSelf)
				{
					GameObjectRayHit item = null;
					if (gameObject.RaycastBox(ray, out item))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x00154072 File Offset: 0x00152272
		public List<GameObjectRayHit> RaycastAllSprite(Ray ray)
		{
			return this._gameObjectSphereTree.RaycastAllSprite(ray);
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x00154080 File Offset: 0x00152280
		public List<GameObjectRayHit> RaycastAllMesh(Ray ray)
		{
			if (!MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return this._gameObjectSphereTree.RaycastAllMesh(ray);
			}
			RaycastHit[] array = Physics.RaycastAll(ray);
			List<GameObjectRayHit> list = new List<GameObjectRayHit>();
			foreach (RaycastHit raycastHit in array)
			{
				GameObject gameObject = raycastHit.collider.gameObject;
				if (!(gameObject == null) && gameObject.activeSelf)
				{
					GameObjectRayHit item = null;
					if (gameObject.RaycastBox(ray, out item))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x00154104 File Offset: 0x00152304
		public List<GameObject> OverlapBox(Box box, ObjectOverlapPrecision overlapPrecision = ObjectOverlapPrecision.ObjectBox)
		{
			if (!MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				return this._gameObjectSphereTree.OverlapBox(box, overlapPrecision);
			}
			List<GameObject> list = new List<GameObject>();
			foreach (Collider collider in Physics.OverlapSphere(box.Center, box.Extents.magnitude))
			{
				list.Add(collider.gameObject);
			}
			return list;
		}

		// Token: 0x0400207F RID: 8319
		private GameObjectSphereTree _gameObjectSphereTree = new GameObjectSphereTree(2);
	}
}
