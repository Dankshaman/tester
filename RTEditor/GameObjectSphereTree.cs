using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200044A RID: 1098
	public class GameObjectSphereTree
	{
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x001529DC File Offset: 0x00150BDC
		public int NumberOfGameObjects
		{
			get
			{
				return this._gameObjectToNode.Count;
			}
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x001529E9 File Offset: 0x00150BE9
		public GameObjectSphereTree(int numberOfChildNodesPerNode)
		{
			this._sphereTree = new SphereTree<GameObject>(numberOfChildNodesPerNode);
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x00152A14 File Offset: 0x00150C14
		public List<GameObject> OverlapSphere(Sphere3D sphere, ObjectOverlapPrecision objectOverlapPrecision = ObjectOverlapPrecision.ObjectBox)
		{
			List<SphereTreeNode<GameObject>> list = this._sphereTree.OverlapSphere(sphere);
			if (list.Count == 0)
			{
				return new List<GameObject>();
			}
			List<GameObject> list2 = new List<GameObject>();
			foreach (SphereTreeNode<GameObject> sphereTreeNode in list)
			{
				GameObject data = sphereTreeNode.Data;
				if (!(data == null) && data.activeSelf)
				{
					OrientedBox worldOrientedBox = data.GetWorldOrientedBox();
					if (sphere.OverlapsFullyOrPartially(worldOrientedBox))
					{
						list2.Add(data);
					}
				}
			}
			return list2;
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x00152AB0 File Offset: 0x00150CB0
		public List<GameObject> OverlapBox(OrientedBox box, ObjectOverlapPrecision objectOverlapPrecision = ObjectOverlapPrecision.ObjectBox)
		{
			List<SphereTreeNode<GameObject>> list = this._sphereTree.OverlapBox(box);
			if (list.Count == 0)
			{
				return new List<GameObject>();
			}
			List<GameObject> list2 = new List<GameObject>();
			foreach (SphereTreeNode<GameObject> sphereTreeNode in list)
			{
				GameObject data = sphereTreeNode.Data;
				if (!(data == null) && data.activeSelf)
				{
					OrientedBox worldOrientedBox = data.GetWorldOrientedBox();
					if (box.Intersects(worldOrientedBox))
					{
						list2.Add(data);
					}
				}
			}
			return list2;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x00152B48 File Offset: 0x00150D48
		public List<GameObject> OverlapBox(Box box, ObjectOverlapPrecision objectOverlapPrecision = ObjectOverlapPrecision.ObjectBox)
		{
			List<SphereTreeNode<GameObject>> list = this._sphereTree.OverlapBox(box);
			if (list.Count == 0)
			{
				return new List<GameObject>();
			}
			OrientedBox orientedBox = box.ToOrientedBox();
			List<GameObject> list2 = new List<GameObject>();
			foreach (SphereTreeNode<GameObject> sphereTreeNode in list)
			{
				GameObject data = sphereTreeNode.Data;
				if (!(data == null) && data.activeSelf)
				{
					OrientedBox worldOrientedBox = data.GetWorldOrientedBox();
					if (orientedBox.Intersects(worldOrientedBox))
					{
						list2.Add(data);
					}
				}
			}
			return list2;
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x00152BF0 File Offset: 0x00150DF0
		public List<GameObjectRayHit> RaycastAllBox(Ray ray)
		{
			List<SphereTreeNodeRayHit<GameObject>> list = this._sphereTree.RaycastAll(ray);
			if (list.Count == 0)
			{
				return new List<GameObjectRayHit>();
			}
			List<GameObjectRayHit> list2 = new List<GameObjectRayHit>();
			foreach (SphereTreeNodeRayHit<GameObject> sphereTreeNodeRayHit in list)
			{
				GameObject data = sphereTreeNodeRayHit.HitNode.Data;
				if (!(data == null) && data.activeSelf)
				{
					GameObjectRayHit item = null;
					if (data.RaycastBox(ray, out item))
					{
						list2.Add(item);
					}
				}
			}
			return list2;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x00152C94 File Offset: 0x00150E94
		public List<GameObjectRayHit> RaycastAllSprite(Ray ray)
		{
			List<SphereTreeNodeRayHit<GameObject>> list = this._sphereTree.RaycastAll(ray);
			if (list.Count == 0)
			{
				return new List<GameObjectRayHit>();
			}
			List<GameObjectRayHit> list2 = new List<GameObjectRayHit>();
			foreach (SphereTreeNodeRayHit<GameObject> sphereTreeNodeRayHit in list)
			{
				GameObject data = sphereTreeNodeRayHit.HitNode.Data;
				if (!(data == null) && data.HasSpriteRendererWithSprite())
				{
					GameObjectRayHit item = null;
					if (data.RaycastSprite(ray, out item))
					{
						list2.Add(item);
					}
				}
			}
			return list2;
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x00152D38 File Offset: 0x00150F38
		public List<GameObjectRayHit> RaycastAllMesh(Ray ray)
		{
			List<GameObjectRayHit> list = this.RaycastAllBox(ray);
			if (list.Count == 0)
			{
				return new List<GameObjectRayHit>();
			}
			List<GameObjectRayHit> list2 = new List<GameObjectRayHit>(list.Count);
			foreach (GameObjectRayHit gameObjectRayHit in list)
			{
				GameObject hitObject = gameObjectRayHit.HitObject;
				if (!(hitObject == null) && hitObject.activeSelf)
				{
					GameObjectRayHit item = null;
					if (hitObject.RaycastMesh(ray, out item))
					{
						list2.Add(item);
					}
				}
			}
			return list2;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x00152DD0 File Offset: 0x00150FD0
		public void Update()
		{
			this._nullCleanupTime += Time.deltaTime;
			if (this._nullCleanupTime >= 1f)
			{
				this.RemoveNullObjectNodes();
				this._nullCleanupTime = 0f;
			}
			foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType<GameObject>())
			{
				if (this.IsGameObjectRegistered(gameObject) || this.RegisterGameObject(gameObject))
				{
					Transform transform = gameObject.transform;
					GameObjectSphereTree.GameObjectTransformData gameObjectTransformData = this._gameObjectToTransformData[gameObject];
					if (gameObjectTransformData.Position != transform.position || gameObjectTransformData.Rotation != transform.rotation || gameObjectTransformData.Scale != transform.lossyScale)
					{
						this.HandleObjectTransformChange(transform);
						this._gameObjectToTransformData[gameObject] = this.GetGameObjectTransformData(gameObject);
					}
				}
			}
			this._sphereTree.PerformPendingUpdates();
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x00152EB3 File Offset: 0x001510B3
		public bool IsGameObjectRegistered(GameObject gameObject)
		{
			return this._gameObjectToNode.ContainsKey(gameObject);
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x00152EC4 File Offset: 0x001510C4
		private void RegisterGameObjectHierarchies(List<GameObject> roots)
		{
			foreach (GameObject root in roots)
			{
				this.RegisterGameObjectHierarchy(root);
			}
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x00152F14 File Offset: 0x00151114
		private void RegisterGameObjectHierarchy(GameObject root)
		{
			foreach (GameObject gameObject in root.GetAllChildrenIncludingSelf())
			{
				this.RegisterGameObject(gameObject);
			}
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x00152F68 File Offset: 0x00151168
		private bool RegisterGameObject(GameObject gameObject)
		{
			if (!this.CanGameObjectBeRegisteredWithTree(gameObject))
			{
				return false;
			}
			Sphere3D encapsulatingSphere = gameObject.GetWorldBox().GetEncapsulatingSphere();
			SphereTreeNode<GameObject> value = this._sphereTree.AddTerminalNode(encapsulatingSphere, gameObject);
			this._gameObjectToNode.Add(gameObject, value);
			this._gameObjectToTransformData.Add(gameObject, this.GetGameObjectTransformData(gameObject));
			if (gameObject.HasMesh())
			{
				EditorMesh editorMesh = MonoSingletonBase<EditorMeshDatabase>.Instance.CreateEditorMesh(gameObject.GetMesh());
				if (editorMesh != null)
				{
					MonoSingletonBase<EditorMeshDatabase>.Instance.AddMeshToSilentBuild(editorMesh);
				}
			}
			MonoSingletonBase<EditorCamera>.Instance.AdjustObjectVisibility(gameObject);
			return true;
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x00152FF4 File Offset: 0x001511F4
		private GameObjectSphereTree.GameObjectTransformData GetGameObjectTransformData(GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			return new GameObjectSphereTree.GameObjectTransformData
			{
				Position = transform.position,
				Rotation = transform.rotation,
				Scale = transform.lossyScale
			};
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x00153034 File Offset: 0x00151234
		private bool CanGameObjectBeRegisteredWithTree(GameObject gameObject)
		{
			return !(gameObject == null) && !this._gameObjectToNode.ContainsKey(gameObject) && gameObject != MonoSingletonBase<EditorCamera>.Instance.Background.IsSameAs(gameObject) && !gameObject.HasTerrain() && !gameObject.HasCamera() && !gameObject.IsRTEditorSystemObject();
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x00153094 File Offset: 0x00151294
		private void HandleObjectTransformChange(Transform gameObjectTransform)
		{
			GameObject gameObject = gameObjectTransform.gameObject;
			if (!this.IsGameObjectRegistered(gameObject))
			{
				return;
			}
			SphereTreeNode<GameObject> sphereTreeNode = this._gameObjectToNode[gameObject];
			bool flag = false;
			bool flag2 = false;
			Sphere3D sphere = sphereTreeNode.Sphere;
			Sphere3D encapsulatingSphere = gameObject.GetWorldBox().GetEncapsulatingSphere();
			if (sphere.Center != encapsulatingSphere.Center)
			{
				flag = true;
			}
			if (sphere.Radius != encapsulatingSphere.Radius)
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				this._sphereTree.UpdateTerminalNodeCenterAndRadius(sphereTreeNode, encapsulatingSphere.Center, encapsulatingSphere.Radius);
			}
			else if (flag)
			{
				this._sphereTree.UpdateTerminalNodeCenter(sphereTreeNode, encapsulatingSphere.Center);
			}
			else if (flag2)
			{
				this._sphereTree.UpdateTerminalNodeRadius(sphereTreeNode, encapsulatingSphere.Radius);
			}
			MonoSingletonBase<EditorCamera>.Instance.AdjustObjectVisibility(gameObject);
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x00153160 File Offset: 0x00151360
		private void RemoveNullObjectNodes()
		{
			Dictionary<GameObject, SphereTreeNode<GameObject>> dictionary = new Dictionary<GameObject, SphereTreeNode<GameObject>>();
			foreach (KeyValuePair<GameObject, SphereTreeNode<GameObject>> keyValuePair in this._gameObjectToNode)
			{
				if (keyValuePair.Key == null)
				{
					this._sphereTree.RemoveNode(keyValuePair.Value);
				}
				else
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			this._gameObjectToNode = dictionary;
		}

		// Token: 0x04002060 RID: 8288
		private float _nullCleanupTime;

		// Token: 0x04002061 RID: 8289
		private SphereTree<GameObject> _sphereTree;

		// Token: 0x04002062 RID: 8290
		private Dictionary<GameObject, SphereTreeNode<GameObject>> _gameObjectToNode = new Dictionary<GameObject, SphereTreeNode<GameObject>>();

		// Token: 0x04002063 RID: 8291
		private Dictionary<GameObject, GameObjectSphereTree.GameObjectTransformData> _gameObjectToTransformData = new Dictionary<GameObject, GameObjectSphereTree.GameObjectTransformData>();

		// Token: 0x0200080D RID: 2061
		private class GameObjectTransformData
		{
			// Token: 0x04002E1B RID: 11803
			public Vector3 Position;

			// Token: 0x04002E1C RID: 11804
			public Quaternion Rotation;

			// Token: 0x04002E1D RID: 11805
			public Vector3 Scale;
		}
	}
}
