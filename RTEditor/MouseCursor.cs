using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200041E RID: 1054
	public class MouseCursor : SingletonBase<MouseCursor>
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060030BD RID: 12477 RVA: 0x0014D3A3 File Offset: 0x0014B5A3
		public Vector2 PreviousPosition
		{
			get
			{
				return this._previousPosition;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x0014D3AB File Offset: 0x0014B5AB
		public Vector2 OffsetSinceLastMouseMove
		{
			get
			{
				return this._offsetSinceLastMouseMove;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060030BF RID: 12479 RVA: 0x0014D3B3 File Offset: 0x0014B5B3
		public Vector2 Position
		{
			get
			{
				return Event.current.mousePosition;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x0014D3BF File Offset: 0x0014B5BF
		public MouseCursorObjectPickFlags ObjectPickMaskFlags
		{
			get
			{
				if (this._objectPickMaskFlagsStack.Count == 0)
				{
					return MouseCursorObjectPickFlags.None;
				}
				return this._objectPickMaskFlagsStack.Peek();
			}
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x0014D3DB File Offset: 0x0014B5DB
		public bool IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags flag)
		{
			return (this.ObjectPickMaskFlags & flag) > MouseCursorObjectPickFlags.None;
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x0014D3E8 File Offset: 0x0014B5E8
		public void PushObjectPickMaskFlags(MouseCursorObjectPickFlags flags)
		{
			this._objectPickMaskFlagsStack.Push(flags);
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x0014D3F8 File Offset: 0x0014B5F8
		public MouseCursorRayHit GetRayHit()
		{
			this.GetObjectRayHitInstances().RemoveAll((GameObjectRayHit item) => !item.HitObject.activeSelf);
			MouseCursorRayHit result = new MouseCursorRayHit(this.GetGridCellRayHit(), this.GetObjectRayHitInstances());
			this.PopObjectPickMaskFlags();
			return result;
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x0014D448 File Offset: 0x0014B648
		public MouseCursorRayHit GetRayHit(int layerMask)
		{
			List<GameObjectRayHit> objectRayHitInstances = this.GetObjectRayHitInstances();
			objectRayHitInstances.RemoveAll((GameObjectRayHit item) => !item.HitObject.activeSelf);
			objectRayHitInstances.RemoveAll((GameObjectRayHit item) => !LayerHelper.IsLayerBitSet(layerMask, item.HitObject.layer));
			MouseCursorRayHit result = new MouseCursorRayHit(this.GetGridCellRayHit(), objectRayHitInstances);
			this.PopObjectPickMaskFlags();
			return result;
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x0014D4B8 File Offset: 0x0014B6B8
		public MouseCursorRayHit GetCursorRayHitForGridCell()
		{
			GridCellRayHit gridCellRayHit = this.GetGridCellRayHit();
			if (gridCellRayHit == null)
			{
				return null;
			}
			return new MouseCursorRayHit(gridCellRayHit, new List<GameObjectRayHit>());
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x0014D4DC File Offset: 0x0014B6DC
		public GridCellRayHit GetGridCellRayHit()
		{
			Ray ray;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPickRay(MonoSingletonBase<EditorCamera>.Instance.Camera, out ray))
			{
				return null;
			}
			float t;
			XZGrid closestHitGridAndMinT = this.GetClosestHitGridAndMinT(new List<XZGrid>
			{
				MonoSingletonBase<RuntimeEditorApplication>.Instance.XZGrid
			}, ray, out t);
			if (closestHitGridAndMinT != null)
			{
				return this.GetGridCellHit(closestHitGridAndMinT, ray, t);
			}
			return null;
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x0014D530 File Offset: 0x0014B730
		public bool IntersectsPlane(Plane plane, out Vector3 intersectionPoint)
		{
			intersectionPoint = Vector3.zero;
			Ray ray;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPickRay(MonoSingletonBase<EditorCamera>.Instance.Camera, out ray))
			{
				return false;
			}
			float distance;
			if (plane.Raycast(ray, out distance))
			{
				intersectionPoint = ray.GetPoint(distance);
				return true;
			}
			return false;
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x0014D57F File Offset: 0x0014B77F
		public void HandleMouseMoveEvent(Event e)
		{
			this._offsetSinceLastMouseMove = e.mousePosition - this._previousPosition;
			this._previousPosition = e.mousePosition;
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x0014D5A4 File Offset: 0x0014B7A4
		private MouseCursorObjectPickFlags PopObjectPickMaskFlags()
		{
			if (this._objectPickMaskFlagsStack.Count != 0)
			{
				return this._objectPickMaskFlagsStack.Pop();
			}
			return MouseCursorObjectPickFlags.None;
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x0014D5C0 File Offset: 0x0014B7C0
		private List<GameObjectRayHit> GetObjectRayHitInstances()
		{
			Ray ray;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPickRay(MonoSingletonBase<EditorCamera>.Instance.Camera, out ray))
			{
				return new List<GameObjectRayHit>();
			}
			List<GameObjectRayHit> list = new List<GameObjectRayHit>();
			this.RaycastAllTerrainObjects(ray, list);
			this.RaycastAllObjectsNoTerrains(ray, list);
			this.SortObjectRayHitListByHitDistanceFromCamera(list);
			return list;
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x0014D60C File Offset: 0x0014B80C
		private void RaycastAllTerrainObjects(Ray ray, List<GameObjectRayHit> terrainHits)
		{
			if (!this.IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags.ObjectTerrain))
			{
				RaycastHit[] array = Physics.RaycastAll(ray);
				if (array.Length != 0)
				{
					foreach (RaycastHit raycastHit in array)
					{
						if (raycastHit.collider.GetType() == typeof(TerrainCollider))
						{
							TerrainRayHit objectTerrainHit = new TerrainRayHit(ray, raycastHit);
							GameObjectRayHit item = new GameObjectRayHit(ray, raycastHit.collider.gameObject, null, null, objectTerrainHit, null);
							terrainHits.Add(item);
						}
					}
				}
			}
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x0014D68C File Offset: 0x0014B88C
		private void RaycastAllObjectsNoTerrains(Ray ray, List<GameObjectRayHit> objectHits)
		{
			bool flag = !this.IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags.ObjectMesh);
			bool flag2 = !this.IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags.ObjectBox);
			bool flag3 = !this.IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags.ObjectSprite);
			if (flag && flag2 && flag3)
			{
				List<GameObjectRayHit> list = SingletonBase<EditorScene>.Instance.RaycastAllMesh(ray);
				if (list.Count != 0)
				{
					objectHits.AddRange(list);
				}
				List<GameObjectRayHit> list2 = SingletonBase<EditorScene>.Instance.RaycastAllBox(ray);
				list2.RemoveAll((GameObjectRayHit item) => item.HitObject.HasMesh() || item.HitObject.HasSpriteRendererWithSprite());
				if (list2.Count != 0)
				{
					objectHits.AddRange(list2);
				}
				List<GameObjectRayHit> list3 = SingletonBase<EditorScene>.Instance.RaycastAllSprite(ray);
				list3.RemoveAll((GameObjectRayHit item) => item.HitObject.HasMesh() || item.HitObject.GetComponent<SpriteRenderer>().IsPixelFullyTransparent(item.HitPoint));
				if (list3.Count != 0)
				{
					objectHits.AddRange(list3);
					return;
				}
			}
			else
			{
				if (!this.IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags.ObjectMesh))
				{
					List<GameObjectRayHit> list4 = SingletonBase<EditorScene>.Instance.RaycastAllMesh(ray);
					if (list4.Count != 0)
					{
						objectHits.AddRange(list4);
					}
				}
				if (!this.IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags.ObjectSprite))
				{
					List<GameObjectRayHit> list5 = SingletonBase<EditorScene>.Instance.RaycastAllSprite(ray);
					list5.RemoveAll((GameObjectRayHit item) => objectHits.Contains(item) || item.HitObject.GetComponent<SpriteRenderer>().IsPixelFullyTransparent(item.HitPoint));
					if (list5.Count != 0)
					{
						objectHits.AddRange(list5);
					}
				}
				if (!this.IsObjectPickMaskFlagSet(MouseCursorObjectPickFlags.ObjectBox))
				{
					List<GameObjectRayHit> list6 = SingletonBase<EditorScene>.Instance.RaycastAllBox(ray);
					list6.RemoveAll((GameObjectRayHit item) => objectHits.Contains(item));
					if (list6.Count != 0)
					{
						objectHits.AddRange(list6);
					}
				}
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x0014D834 File Offset: 0x0014BA34
		private XZGrid GetClosestHitGridAndMinT(List<XZGrid> allSnapGrids, Ray ray, out float minT)
		{
			minT = float.MaxValue;
			XZGrid result = null;
			foreach (XZGrid xzgrid in allSnapGrids)
			{
				float num;
				if (xzgrid.Plane.Raycast(ray, out num) & num < minT)
				{
					minT = num;
					result = xzgrid;
				}
			}
			return result;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x0014D8A4 File Offset: 0x0014BAA4
		private GridCellRayHit GetGridCellHit(XZGrid hitGrid, Ray ray, float t)
		{
			XZGridCell cellFromWorldPoint = hitGrid.GetCellFromWorldPoint(ray.GetPoint(t));
			return new GridCellRayHit(ray, t, cellFromWorldPoint);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x0014D8C8 File Offset: 0x0014BAC8
		private void SortObjectRayHitListByHitDistanceFromCamera(List<GameObjectRayHit> objectRayHitInstances)
		{
			Vector3 sceneCameraPosition = MonoSingletonBase<EditorCamera>.Instance.Camera.transform.position;
			objectRayHitInstances.Sort(delegate(GameObjectRayHit firstObjectHit, GameObjectRayHit secondObjectHit)
			{
				float magnitude = (firstObjectHit.HitPoint - sceneCameraPosition).magnitude;
				float magnitude2 = (secondObjectHit.HitPoint - sceneCameraPosition).magnitude;
				return magnitude.CompareTo(magnitude2);
			});
		}

		// Token: 0x04001FB6 RID: 8118
		private Vector2 _previousPosition;

		// Token: 0x04001FB7 RID: 8119
		private Vector2 _offsetSinceLastMouseMove;

		// Token: 0x04001FB8 RID: 8120
		private Stack<MouseCursorObjectPickFlags> _objectPickMaskFlagsStack = new Stack<MouseCursorObjectPickFlags>();
	}
}
