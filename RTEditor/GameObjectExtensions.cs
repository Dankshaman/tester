using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F5 RID: 1013
	public static class GameObjectExtensions
	{
		// Token: 0x06002E6D RID: 11885 RVA: 0x0013EDE0 File Offset: 0x0013CFE0
		public static bool RaycastBox(this GameObject gameObject, Ray ray, out GameObjectRayHit objectRayHit)
		{
			objectRayHit = null;
			OrientedBoxRayHit objectBoxHit;
			if (gameObject.GetWorldOrientedBox().Raycast(ray, out objectBoxHit))
			{
				objectRayHit = new GameObjectRayHit(ray, gameObject, objectBoxHit, null, null, null);
			}
			return objectRayHit != null;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x0013EE14 File Offset: 0x0013D014
		public static bool RaycastSprite(this GameObject gameObject, Ray ray, out GameObjectRayHit objectRayHit)
		{
			objectRayHit = null;
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			if (component == null)
			{
				return false;
			}
			OrientedBoxRayHit orientedBoxRayHit;
			if (gameObject.GetWorldOrientedBox().Raycast(ray, out orientedBoxRayHit))
			{
				SpriteRayHit objectSpriteHit = new SpriteRayHit(ray, orientedBoxRayHit.HitEnter, component, orientedBoxRayHit.HitPoint, orientedBoxRayHit.HitNormal);
				objectRayHit = new GameObjectRayHit(ray, gameObject, null, null, null, objectSpriteHit);
			}
			return objectRayHit != null;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x0013EE74 File Offset: 0x0013D074
		public static bool RaycastMesh(this GameObject gameObject, Ray ray, out GameObjectRayHit objectRayHit)
		{
			objectRayHit = null;
			Mesh meshFromFilterOrSkinnedMeshRenderer = gameObject.GetMeshFromFilterOrSkinnedMeshRenderer();
			if (meshFromFilterOrSkinnedMeshRenderer == null)
			{
				return false;
			}
			EditorMesh editorMesh = MonoSingletonBase<EditorMeshDatabase>.Instance.GetEditorMesh(meshFromFilterOrSkinnedMeshRenderer);
			if (editorMesh != null)
			{
				MeshRayHit meshRayHit = editorMesh.Raycast(ray, gameObject.transform.GetWorldMatrix());
				if (meshRayHit == null)
				{
					return false;
				}
				objectRayHit = new GameObjectRayHit(ray, gameObject, null, meshRayHit, null, null);
				return true;
			}
			else
			{
				MeshCollider component = gameObject.GetComponent<MeshCollider>();
				if (!(component != null))
				{
					return gameObject.RaycastBox(ray, out objectRayHit);
				}
				RaycastHit raycastHit;
				if (component.Raycast(ray, out raycastHit, 3.4028235E+38f))
				{
					MeshRayHit objectMeshHit = new MeshRayHit(ray, raycastHit.distance, raycastHit.triangleIndex, raycastHit.point, raycastHit.normal);
					objectRayHit = new GameObjectRayHit(ray, gameObject, null, objectMeshHit, null, null);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x0013EF2C File Offset: 0x0013D12C
		public static bool RaycastTerrain(this GameObject gameObject, Ray ray, out GameObjectRayHit objectRayHit)
		{
			objectRayHit = null;
			if (!gameObject.HasTerrain())
			{
				return false;
			}
			TerrainCollider component = gameObject.GetComponent<TerrainCollider>();
			if (component == null)
			{
				return false;
			}
			RaycastHit raycastHit;
			if (component.Raycast(ray, out raycastHit, 3.4028235E+38f))
			{
				TerrainRayHit objectTerrainHit = new TerrainRayHit(ray, raycastHit);
				objectRayHit = new GameObjectRayHit(ray, gameObject, null, null, objectTerrainHit, null);
			}
			return objectRayHit != null;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x0013EF82 File Offset: 0x0013D182
		public static GameObject CloneAbsTRS(this GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, position, rotation);
			gameObject2.name = gameObject.name;
			gameObject2.layer = gameObject.layer;
			Transform transform = gameObject2.transform;
			transform.localScale = scale;
			transform.parent = parent;
			return gameObject2;
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x0013EFB8 File Offset: 0x0013D1B8
		public static GameObject CloneLocalTRS(this GameObject gameObject, Vector3 localPos, Quaternion localRot, Vector3 localScale, Transform parent = null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.name = gameObject.name;
			gameObject2.layer = gameObject.layer;
			Transform transform = gameObject2.transform;
			transform.localPosition = localPos;
			transform.localRotation = localRot;
			transform.localScale = localScale;
			transform.parent = parent;
			return gameObject2;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x0013F005 File Offset: 0x0013D205
		public static GameObject Clone(this GameObject gameObject, Transform parent = null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.name = gameObject.name;
			gameObject2.layer = gameObject.layer;
			gameObject2.transform.parent = parent;
			return gameObject2;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x0013F034 File Offset: 0x0013D234
		public static void PlaceHierarchyOnPlane(this GameObject root, Vector3 ptOnPlane, Vector3 planeNormal, int alignAxisIndex)
		{
			Transform transform = root.transform;
			Box hierarchyModelSpaceBox = root.GetHierarchyModelSpaceBox();
			Vector3 vector = Vector3.Scale(transform.lossyScale, hierarchyModelSpaceBox.Size);
			planeNormal.Normalize();
			Plane plane = new Plane(planeNormal, ptOnPlane);
			if (alignAxisIndex >= 0 && alignAxisIndex < 3)
			{
				root.AlignAxis(planeNormal, alignAxisIndex);
				Vector3 vector2 = transform.TransformPoint(hierarchyModelSpaceBox.Center);
				float d = vector[alignAxisIndex];
				Vector3 b = vector2 - plane.normal * d * 0.5f;
				Vector3 b2 = transform.position - b;
				Vector3 vector3 = vector2 - plane.normal * plane.GetDistanceToPoint(vector2);
				vector3 += ptOnPlane - vector3;
				transform.position = vector3 + b2;
				return;
			}
			transform.position = ptOnPlane;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x0013F118 File Offset: 0x0013D318
		public static void AlignAxis(this GameObject gameObject, Vector3 destAxis, int srcAxisIndex)
		{
			Transform transform = gameObject.transform;
			Vector3 vector = transform.right;
			if (srcAxisIndex == 1)
			{
				vector = transform.up;
			}
			if (srcAxisIndex == 2)
			{
				vector = transform.forward;
			}
			destAxis.Normalize();
			float num = Vector3.Dot(destAxis, vector);
			if (num > 0f && Mathf.Abs(num - 1f) < 1E-05f)
			{
				return;
			}
			Vector3 axis = Vector3.Cross(vector, destAxis);
			axis.Normalize();
			float angle = MathHelper.SafeAcos(num) * 57.29578f;
			transform.Rotate(axis, angle, Space.World);
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x0013F19C File Offset: 0x0013D39C
		public static List<GameObject> GetRootObjectsFromObjectCollection(List<GameObject> gameObjects)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in gameObjects)
			{
				list.Add(gameObject.transform.root.gameObject);
			}
			return list;
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x0013F200 File Offset: 0x0013D400
		public static List<GameObject> GetAllRootsAndChildren(List<GameObject> gameObjects)
		{
			List<GameObject> rootObjectsFromObjectCollection = GameObjectExtensions.GetRootObjectsFromObjectCollection(gameObjects);
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in rootObjectsFromObjectCollection)
			{
				list.AddRange(gameObject.GetAllChildrenIncludingSelf());
			}
			return list;
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x0013F260 File Offset: 0x0013D460
		public static List<GameObject> GetParentsFromObjectCollection(IEnumerable<GameObject> gameObjects)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in gameObjects)
			{
				Transform transform = gameObject.transform;
				bool flag = false;
				foreach (GameObject gameObject2 in gameObjects)
				{
					if (gameObject2 != gameObject && transform.IsChildOf(gameObject2.transform))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(gameObject);
				}
			}
			return list;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x0013F314 File Offset: 0x0013D514
		public static HashSet<GameObject> GetRootObjectsFromObjectCollection(HashSet<GameObject> gameObjects)
		{
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			foreach (GameObject gameObject in gameObjects)
			{
				hashSet.Add(gameObject.transform.root.gameObject);
			}
			return hashSet;
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x0013F37C File Offset: 0x0013D57C
		public static List<GameObject> GetAllChildren(this GameObject gameObject)
		{
			Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
			List<GameObject> list = new List<GameObject>(componentsInChildren.Length);
			Transform transform = gameObject.transform;
			foreach (Transform transform2 in componentsInChildren)
			{
				if (!(transform2 == transform))
				{
					list.Add(transform2.gameObject);
				}
			}
			return list;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x0013F3CC File Offset: 0x0013D5CC
		public static List<GameObject> GetAllChildrenIncludingSelf(this GameObject gameObject)
		{
			Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
			List<GameObject> list = new List<GameObject>(componentsInChildren.Length);
			foreach (Transform transform in componentsInChildren)
			{
				list.Add(transform.gameObject);
			}
			return list;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x0013F408 File Offset: 0x0013D608
		public static GameObject GetFirstEmptyParent(this GameObject gameObject)
		{
			Transform parent = gameObject.transform.parent;
			while (parent != null)
			{
				if (parent.gameObject.IsEmpty())
				{
					return parent.gameObject;
				}
				parent = parent.parent;
			}
			return null;
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x0013F448 File Offset: 0x0013D648
		public static GameObject GetFirstParentOfType<T>(this GameObject gameObject) where T : Component
		{
			Transform parent = gameObject.transform.parent;
			while (parent != null)
			{
				if (parent.GetComponent<T>() != null)
				{
					return parent.gameObject;
				}
				parent = parent.parent;
			}
			return null;
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x0013F490 File Offset: 0x0013D690
		public static void SetAbsoluteScale(this GameObject gameObject, Vector3 absoluteScale)
		{
			Transform transform = gameObject.transform;
			Transform parent = transform.parent;
			if (Mathf.Abs(absoluteScale.x) < 0.0001f)
			{
				absoluteScale.x = 0.0001f * Mathf.Sign(absoluteScale.x);
			}
			if (Mathf.Abs(absoluteScale.y) < 0.0001f)
			{
				absoluteScale.y = 0.0001f * Mathf.Sign(absoluteScale.y);
			}
			if (Mathf.Abs(absoluteScale.z) < 0.0001f)
			{
				absoluteScale.z = 0.0001f * Mathf.Sign(absoluteScale.z);
			}
			transform.parent = null;
			transform.localScale = absoluteScale;
			transform.parent = parent;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x0013F53C File Offset: 0x0013D73C
		public static void Rotate(this GameObject gameObject, Vector3 rotationAxis, float angleInDegrees, Vector3 pivotPoint)
		{
			Transform transform = gameObject.transform;
			Vector3 vector = transform.position - pivotPoint;
			vector = Quaternion.AngleAxis(angleInDegrees, rotationAxis) * vector;
			transform.Rotate(rotationAxis, angleInDegrees, Space.World);
			transform.position = pivotPoint + vector;
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x0013F580 File Offset: 0x0013D780
		public static Rect GetScreenRectangle(this GameObject gameObject, Camera camera)
		{
			Bounds bounds = gameObject.GetWorldBox().ToBounds();
			if (!bounds.IsValid())
			{
				return new Rect(0f, 0f, 0f, 0f);
			}
			return bounds.GetScreenRectangle(camera);
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x0013F5C8 File Offset: 0x0013D7C8
		public static OrientedBox GetWorldOrientedBox(this GameObject gameObject)
		{
			OrientedBox meshWorldOrientedBox = gameObject.GetMeshWorldOrientedBox();
			if (meshWorldOrientedBox.IsValid())
			{
				return meshWorldOrientedBox;
			}
			return gameObject.GetNonMeshWorldOrientedBox();
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x0013F5EC File Offset: 0x0013D7EC
		public static Box GetWorldBox(this GameObject gameObject)
		{
			Box meshWorldBox = gameObject.GetMeshWorldBox();
			if (meshWorldBox.IsValid())
			{
				return meshWorldBox;
			}
			return gameObject.GetNonMeshWorldBox();
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x0013F614 File Offset: 0x0013D814
		public static OrientedBox GetModelSpaceOrientedBox(this GameObject gameObject)
		{
			OrientedBox meshModelSpaceOrientedBox = gameObject.GetMeshModelSpaceOrientedBox();
			if (meshModelSpaceOrientedBox.IsValid())
			{
				return meshModelSpaceOrientedBox;
			}
			return gameObject.GetNonMeshModelSpaceOrientedBox();
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x0013F638 File Offset: 0x0013D838
		public static Box GetModelSpaceBox(this GameObject gameObject)
		{
			Box meshModelSpaceBox = gameObject.GetMeshModelSpaceBox();
			if (meshModelSpaceBox.IsValid())
			{
				return meshModelSpaceBox;
			}
			return gameObject.GetNonMeshModelSpaceBox();
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x0013F660 File Offset: 0x0013D860
		public static OrientedBox GetMeshWorldOrientedBox(this GameObject gameObject)
		{
			Mesh mesh = gameObject.GetMeshFromMeshFilter();
			if (mesh != null)
			{
				return new OrientedBox(new Box(mesh.bounds), gameObject.transform);
			}
			mesh = gameObject.GetMeshFromSkinnedMeshRenderer();
			if (mesh != null)
			{
				return new OrientedBox(new Box(mesh.bounds), gameObject.transform);
			}
			return OrientedBox.GetInvalid();
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x0013F6C0 File Offset: 0x0013D8C0
		public static Box GetMeshWorldBox(this GameObject gameObject)
		{
			Mesh mesh = gameObject.GetMeshFromMeshFilter();
			if (mesh != null)
			{
				return new Box(mesh.bounds).Transform(gameObject.transform.GetWorldMatrix());
			}
			mesh = gameObject.GetMeshFromSkinnedMeshRenderer();
			if (mesh != null)
			{
				return new Box(mesh.bounds).Transform(gameObject.transform.GetWorldMatrix());
			}
			return Box.GetInvalid();
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x0013F730 File Offset: 0x0013D930
		public static OrientedBox GetNonMeshWorldOrientedBox(this GameObject gameObject)
		{
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				return new OrientedBox(Box.FromBounds(component.GetModelSpaceBounds()), gameObject.transform);
			}
			OrientedBox nonMeshModelSpaceOrientedBox = gameObject.GetNonMeshModelSpaceOrientedBox();
			if (!nonMeshModelSpaceOrientedBox.IsValid())
			{
				return nonMeshModelSpaceOrientedBox;
			}
			OrientedBox orientedBox = new OrientedBox(nonMeshModelSpaceOrientedBox);
			Transform transform = gameObject.transform;
			orientedBox.Center = transform.position;
			orientedBox.Rotation = transform.rotation;
			orientedBox.Scale = transform.lossyScale;
			return orientedBox;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x0013F7A8 File Offset: 0x0013D9A8
		public static Box GetNonMeshWorldBox(this GameObject gameObject)
		{
			Box nonMeshModelSpaceBox = gameObject.GetNonMeshModelSpaceBox();
			if (!nonMeshModelSpaceBox.IsValid())
			{
				return nonMeshModelSpaceBox;
			}
			return nonMeshModelSpaceBox.Transform(gameObject.transform.GetWorldMatrix());
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x0013F7DC File Offset: 0x0013D9DC
		public static OrientedBox GetMeshModelSpaceOrientedBox(this GameObject gameObject)
		{
			Mesh mesh = gameObject.GetMeshFromMeshFilter();
			if (mesh != null)
			{
				return new OrientedBox(new Box(mesh.bounds), Quaternion.identity);
			}
			mesh = gameObject.GetMeshFromSkinnedMeshRenderer();
			if (mesh != null)
			{
				return new OrientedBox(new Box(mesh.bounds), Quaternion.identity);
			}
			return OrientedBox.GetInvalid();
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x0013F83C File Offset: 0x0013DA3C
		public static Box GetMeshModelSpaceBox(this GameObject gameObject)
		{
			Mesh mesh = gameObject.GetMeshFromMeshFilter();
			if (mesh != null)
			{
				return new Box(mesh.bounds);
			}
			mesh = gameObject.GetMeshFromSkinnedMeshRenderer();
			if (mesh != null)
			{
				return new Box(mesh.bounds);
			}
			return Box.GetInvalid();
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x0013F886 File Offset: 0x0013DA86
		public static OrientedBox GetNonMeshModelSpaceOrientedBox(this GameObject gameObject)
		{
			return new OrientedBox(gameObject.GetNonMeshModelSpaceBox());
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x0013F894 File Offset: 0x0013DA94
		public static Box GetNonMeshModelSpaceBox(this GameObject gameObject)
		{
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			if (component != null && component.sprite != null)
			{
				return Box.FromBounds(component.GetModelSpaceBounds());
			}
			if (gameObject.HasTerrain())
			{
				TerrainData terrainData = gameObject.GetComponent<Terrain>().terrainData;
				Vector3 vector = new Vector3(terrainData.size.x, 1f, terrainData.size.z);
				if (terrainData != null)
				{
					return new Box(vector * 0.5f, vector);
				}
			}
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseUnityColliders)
			{
				Box colliderModelSpaceBox = gameObject.GetColliderModelSpaceBox();
				if (colliderModelSpaceBox.IsValid())
				{
					return colliderModelSpaceBox;
				}
			}
			if (gameObject.HasLight())
			{
				return new Box(Vector3.zero, MonoSingletonBase<RuntimeEditorApplication>.Instance.VolumeSizeForLightObjects);
			}
			if (gameObject.HasParticleSystem())
			{
				return new Box(Vector3.zero, MonoSingletonBase<RuntimeEditorApplication>.Instance.VolumeSizeForParticleSystemObjects);
			}
			return new Box(Vector3.zero, MonoSingletonBase<RuntimeEditorApplication>.Instance.VolumeSizeForEmptyObjects);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x0013F988 File Offset: 0x0013DB88
		public static Box GetColliderModelSpaceBox(this GameObject gameObject)
		{
			Box result = Box.GetInvalid();
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			if (component != null && component.enabled)
			{
				result = new Box(component.center, component.size);
			}
			SphereCollider component2 = gameObject.GetComponent<SphereCollider>();
			if (component2 != null && component2.enabled)
			{
				Vector3 size = Vector3.one * component2.radius * 2f;
				Box box = new Box(component2.center, size);
				if (result.IsValid())
				{
					result.Encapsulate(box);
				}
				else
				{
					result = box;
				}
			}
			CapsuleCollider component3 = gameObject.GetComponent<CapsuleCollider>();
			if (component3 != null && component3.enabled)
			{
				float num = component3.radius * 2f;
				Vector3 zero = Vector3.zero;
				if (component3.direction == 0)
				{
					zero = new Vector3(component3.height, num, num);
				}
				else if (component3.direction == 1)
				{
					zero = new Vector3(num, component3.height, num);
				}
				else
				{
					zero = new Vector3(num, num, component3.height);
				}
				Box box2 = new Box(component3.center, zero);
				if (result.IsValid())
				{
					result.Encapsulate(box2);
				}
				else
				{
					result = box2;
				}
			}
			return result;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x0013FAC2 File Offset: 0x0013DCC2
		public static bool IsEmpty(this GameObject gameObject)
		{
			return !gameObject.HasMesh() && !gameObject.HasTerrain() && !gameObject.HasLight() && !gameObject.HasParticleSystem() && !gameObject.IsSprite();
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x0013FAF8 File Offset: 0x0013DCF8
		public static void SetLayerForEntireHierarchy(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x0013FB30 File Offset: 0x0013DD30
		public static Box GetHierarchyModelSpaceBox(this GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
			Box result = gameObject.GetModelSpaceBox();
			foreach (Transform transform2 in componentsInChildren)
			{
				if (transform != transform2)
				{
					Box box = transform2.gameObject.GetModelSpaceBox();
					if (box.IsValid())
					{
						Matrix4x4 relativeTransform = transform2.GetRelativeTransform(transform);
						box = box.Transform(relativeTransform);
						if (result.IsValid())
						{
							result.Encapsulate(box);
						}
						else
						{
							result = box;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x0013FBB4 File Offset: 0x0013DDB4
		public static Mesh GetMesh(this GameObject gameObject)
		{
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh != null)
			{
				return component.sharedMesh;
			}
			SkinnedMeshRenderer component2 = gameObject.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null && component2.sharedMesh != null)
			{
				return component2.sharedMesh;
			}
			return null;
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x0013FC0C File Offset: 0x0013DE0C
		public static Renderer GetRenderer(this GameObject gameObject)
		{
			return gameObject.GetComponent<Renderer>();
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x0013FC14 File Offset: 0x0013DE14
		public static bool IsSprite(this GameObject gameObject)
		{
			if (gameObject.HasMesh())
			{
				return false;
			}
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			return component != null && component.sprite != null;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x0013FC4C File Offset: 0x0013DE4C
		public static bool IsRTEditorSystemObject(this GameObject gameObject)
		{
			RuntimeEditorApplication instance = MonoSingletonBase<RuntimeEditorApplication>.Instance;
			return instance.gameObject == gameObject || gameObject.transform.IsChildOf(instance.transform);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x0013FC88 File Offset: 0x0013DE88
		public static Mesh GetMeshFromFilterOrSkinnedMeshRenderer(this GameObject gameObject)
		{
			Mesh mesh = gameObject.GetMeshFromMeshFilter();
			if (mesh == null)
			{
				mesh = gameObject.GetMeshFromSkinnedMeshRenderer();
			}
			return mesh;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x0013FCB0 File Offset: 0x0013DEB0
		public static Mesh GetMeshFromMeshFilter(this GameObject gameObject)
		{
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh != null)
			{
				return component.sharedMesh;
			}
			return null;
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x0013FCE4 File Offset: 0x0013DEE4
		public static Mesh GetMeshFromSkinnedMeshRenderer(this GameObject gameObject)
		{
			SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
			if (component != null && component.sharedMesh != null)
			{
				return component.sharedMesh;
			}
			return null;
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x0013FD17 File Offset: 0x0013DF17
		public static bool HasMesh(this GameObject gameObject)
		{
			return gameObject.HasMeshFilterWithValidMesh() || gameObject.HasSkinnedMeshRendererWithValidMesh();
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x0013FD2C File Offset: 0x0013DF2C
		public static bool HasMeshFilterWithValidMesh(this GameObject gameObject)
		{
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			return component != null && component.sharedMesh != null;
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x0013FD58 File Offset: 0x0013DF58
		public static bool HasSkinnedMeshRendererWithValidMesh(this GameObject gameObject)
		{
			SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
			return component != null && component.sharedMesh != null;
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x0013FD83 File Offset: 0x0013DF83
		public static bool HasTerrain(this GameObject gameObject)
		{
			return gameObject.GetComponent<Terrain>() != null;
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x0013FD91 File Offset: 0x0013DF91
		public static bool HasLight(this GameObject gameObject)
		{
			return gameObject.GetComponent<Light>() != null;
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x0013FD9F File Offset: 0x0013DF9F
		public static bool HasParticleSystem(this GameObject gameObject)
		{
			return gameObject.GetComponent<ParticleSystem>() != null;
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x0013FDAD File Offset: 0x0013DFAD
		public static bool HasSpriteRenderer(this GameObject gameObject)
		{
			return gameObject.GetComponent<SpriteRenderer>() != null;
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x0013FDBB File Offset: 0x0013DFBB
		public static bool HasCamera(this GameObject gameObject)
		{
			return gameObject.GetComponent<Camera>() != null;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x0013FDCC File Offset: 0x0013DFCC
		public static bool HasSpriteRendererWithSprite(this GameObject gameObject)
		{
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			return !(component == null) && component.sprite != null;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x0013FDF8 File Offset: 0x0013DFF8
		public static void RemoveAllColliders(this GameObject gameObject)
		{
			Collider[] components = gameObject.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
			Collider2D[] components2 = gameObject.GetComponents<Collider2D>();
			for (int i = 0; i < components2.Length; i++)
			{
				UnityEngine.Object.Destroy(components2[i]);
			}
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x0013FE40 File Offset: 0x0013E040
		public static void DestroyAllChildren(this GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			foreach (Transform transform2 in gameObject.GetComponentsInChildren<Transform>())
			{
				if (!(transform == transform2))
				{
					UnityEngine.Object.Destroy(transform2.gameObject);
				}
			}
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x0013FE81 File Offset: 0x0013E081
		public static Light[] GetAllLightComponents(this GameObject gameObject)
		{
			return gameObject.GetComponents<Light>();
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x0013FE8C File Offset: 0x0013E08C
		public static Light GetFirstLightComponent(this GameObject gameObject)
		{
			Light[] allLightComponents = gameObject.GetAllLightComponents();
			if (allLightComponents.Length != 0)
			{
				return allLightComponents[0];
			}
			return null;
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x0013FEA9 File Offset: 0x0013E0A9
		public static ParticleSystem[] GetAllParticleSystemComponents(this GameObject gameObject)
		{
			return gameObject.GetComponents<ParticleSystem>();
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x0013FEB4 File Offset: 0x0013E0B4
		public static ParticleSystem GetFirstParticleSystemComponent(this GameObject gameObject)
		{
			ParticleSystem[] allParticleSystemComponents = gameObject.GetAllParticleSystemComponents();
			if (allParticleSystemComponents.Length != 0)
			{
				return allParticleSystemComponents[0];
			}
			return null;
		}
	}
}
