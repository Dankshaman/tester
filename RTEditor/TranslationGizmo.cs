using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200040E RID: 1038
	public class TranslationGizmo : Gizmo
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x0014749E File Offset: 0x0014569E
		private bool IsTranslateAlongScreenAxesShActive
		{
			get
			{
				return this._translateAlongScreenAxesShortcut.IsActive();
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x001474AB File Offset: 0x001456AB
		private bool IsStepSnappingShActive
		{
			get
			{
				return this._enableStepSnappingShortcut.IsActive();
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x001474B8 File Offset: 0x001456B8
		private bool IsVertexSnappingShActive
		{
			get
			{
				return this._enableVertexSnappingShortcut.IsActive();
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x001474C5 File Offset: 0x001456C5
		private bool IsSurfacePlacementShActive
		{
			get
			{
				return this.IsSurfacePlacementAlignYShActive || this.IsSurfacePlacementAlignXShActive || this.IsSurfacePlacementAlignZShActive || this.IsSurfacePlacementNoAlignShActive;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x001474E7 File Offset: 0x001456E7
		private bool IsSurfacePlacementAlignXShActive
		{
			get
			{
				return this._enableSurfacePlacementWithXAlignment.IsActive();
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x001474F4 File Offset: 0x001456F4
		private bool IsSurfacePlacementAlignYShActive
		{
			get
			{
				return this._enableSurfacePlacementWithYAlignment.IsActive();
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x00147501 File Offset: 0x00145701
		private bool IsSurfacePlacementAlignZShActive
		{
			get
			{
				return this._enableSurfacePlacementWithZAlignment.IsActive();
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x0014750E File Offset: 0x0014570E
		private bool IsSurfacePlacementNoAlignShActive
		{
			get
			{
				return this._enableSurfacePlacementNoAxisAlignment.IsActive();
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x0600300D RID: 12301 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinAxisLength
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinArrowConeRadius
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x0600300F RID: 12303 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinArrowConeLength
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinMultiAxisSquareSize
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x00143F8C File Offset: 0x0014218C
		public static float MinScreenSizeOfCameraAxesTranslationSquare
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x00143F8C File Offset: 0x0014218C
		public static float MinScreenSizeOfVertexSnappingSquare
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x0014751B File Offset: 0x0014571B
		public ShortcutKeys TranslateAlongScreenAxesShortcut
		{
			get
			{
				return this._translateAlongScreenAxesShortcut;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x00147523 File Offset: 0x00145723
		public ShortcutKeys EnableStepSnappingShortcut
		{
			get
			{
				return this._enableStepSnappingShortcut;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x0014752B File Offset: 0x0014572B
		public ShortcutKeys EnableVertexSnappingShortcut
		{
			get
			{
				return this._enableVertexSnappingShortcut;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06003016 RID: 12310 RVA: 0x00147533 File Offset: 0x00145733
		public ShortcutKeys EnableSurfacePlacementWithXAlignment
		{
			get
			{
				return this._enableSurfacePlacementWithXAlignment;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x0014753B File Offset: 0x0014573B
		public ShortcutKeys EnableSurfacePlacementWithYAlignment
		{
			get
			{
				return this._enableSurfacePlacementWithYAlignment;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x00147543 File Offset: 0x00145743
		public ShortcutKeys EnableSurfacePlacementWithZAlignment
		{
			get
			{
				return this._enableSurfacePlacementWithZAlignment;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x0014754B File Offset: 0x0014574B
		public ShortcutKeys EnableSurfacePlacementWithNoAxisAlignment
		{
			get
			{
				return this._enableSurfacePlacementNoAxisAlignment;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x00147553 File Offset: 0x00145753
		// (set) Token: 0x0600301B RID: 12315 RVA: 0x0014755B File Offset: 0x0014575B
		public float AxisLength
		{
			get
			{
				return this._axisLength;
			}
			set
			{
				this._axisLength = Mathf.Max(TranslationGizmo.MinAxisLength, value);
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x0014756E File Offset: 0x0014576E
		// (set) Token: 0x0600301D RID: 12317 RVA: 0x00147576 File Offset: 0x00145776
		public float ArrowConeRadius
		{
			get
			{
				return this._arrowConeRadius;
			}
			set
			{
				this._arrowConeRadius = Mathf.Max(TranslationGizmo.MinArrowConeRadius, value);
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x00147589 File Offset: 0x00145789
		// (set) Token: 0x0600301F RID: 12319 RVA: 0x00147591 File Offset: 0x00145791
		public float ArrowConeLength
		{
			get
			{
				return this._arrowConeLength;
			}
			set
			{
				this._arrowConeLength = Mathf.Max(TranslationGizmo.MinArrowConeLength, value);
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x001475A4 File Offset: 0x001457A4
		// (set) Token: 0x06003021 RID: 12321 RVA: 0x001475AC File Offset: 0x001457AC
		public float MultiAxisSquareSize
		{
			get
			{
				return this._multiAxisSquareSize;
			}
			set
			{
				this._multiAxisSquareSize = Mathf.Max(TranslationGizmo.MinMultiAxisSquareSize, value);
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x001475BF File Offset: 0x001457BF
		// (set) Token: 0x06003023 RID: 12323 RVA: 0x001475C7 File Offset: 0x001457C7
		public bool AdjustMultiAxisForBetterVisibility
		{
			get
			{
				return this._adjustMultiAxisForBetterVisibility;
			}
			set
			{
				this._adjustMultiAxisForBetterVisibility = value;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x001475D0 File Offset: 0x001457D0
		// (set) Token: 0x06003025 RID: 12325 RVA: 0x001475D8 File Offset: 0x001457D8
		public float MultiAxisSquareAlpha
		{
			get
			{
				return this._multiAxisSquareAlpha;
			}
			set
			{
				this._multiAxisSquareAlpha = Mathf.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x001475F0 File Offset: 0x001457F0
		// (set) Token: 0x06003027 RID: 12327 RVA: 0x001475F8 File Offset: 0x001457F8
		public bool AreArrowConesLit
		{
			get
			{
				return this._areArrowConesLit;
			}
			set
			{
				this._areArrowConesLit = value;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x00147601 File Offset: 0x00145801
		// (set) Token: 0x06003029 RID: 12329 RVA: 0x00147609 File Offset: 0x00145809
		public float ScreenSizeOfSpecialOpSquare
		{
			get
			{
				return this._screenSizeOfSpecialOpSquare;
			}
			set
			{
				this._screenSizeOfSpecialOpSquare = Mathf.Max(value, TranslationGizmo.MinScreenSizeOfVertexSnappingSquare);
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x0014761C File Offset: 0x0014581C
		// (set) Token: 0x0600302B RID: 12331 RVA: 0x00147624 File Offset: 0x00145824
		public Color SpecialOpSquareColor
		{
			get
			{
				return this._specialOpSquareColor;
			}
			set
			{
				this._specialOpSquareColor = value;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x0014762D File Offset: 0x0014582D
		// (set) Token: 0x0600302D RID: 12333 RVA: 0x00147635 File Offset: 0x00145835
		public Color SpecialOpSquareColorWhenSelected
		{
			get
			{
				return this._specialOpSquareColorWhenSelected;
			}
			set
			{
				this._specialOpSquareColorWhenSelected = value;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x0014763E File Offset: 0x0014583E
		public TranslationGizmoSnapSettings SnapSettings
		{
			get
			{
				return this._snapSettings;
			}
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x00147646 File Offset: 0x00145846
		public override bool IsReadyForObjectManipulation()
		{
			return this._selectedAxis != GizmoAxis.None || this._selectedMultiAxisSquare != MultiAxisSquare.None || this._isCameraAxesTranslationSquareSelected || this.IsVertexSnappingShActive || this.IsSurfacePlacementShActive || this.DetectHoveredComponents(false);
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public override GizmoType GetGizmoType()
		{
			return GizmoType.Translation;
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x00144137 File Offset: 0x00142337
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x0014767C File Offset: 0x0014587C
		protected override void Update()
		{
			base.Update();
			bool isVertexSnappingShActive = this.IsVertexSnappingShActive;
			if (isVertexSnappingShActive && !this._isVertexSnapping)
			{
				this._isVertexSnapping = true;
			}
			else if (!isVertexSnappingShActive && this._isVertexSnapping)
			{
				this._isVertexSnapping = false;
				VertexSnappingDisabledMessage.SendToInterestedListeners();
			}
			if (this._isVertexSnapping)
			{
				if (!MonoSingletonBase<InputDevice>.Instance.IsPressed(0) && !this.IsSurfacePlacementShActive)
				{
					this._selectedAxis = GizmoAxis.None;
					this._selectedMultiAxisSquare = MultiAxisSquare.None;
					this._isCameraAxesTranslationSquareSelected = false;
					List<GameObject> objectsForClosestVertexSelection = this.GetObjectsForClosestVertexSelection(base.ControlledObjects, false);
					if (objectsForClosestVertexSelection.Count != 0)
					{
						this._gizmoTransform.position = this.GetWorldPositionClosestToMouseCursorForVertexSnapping(objectsForClosestVertexSelection, false);
						return;
					}
				}
				else if (this.IsSurfacePlacementShActive)
				{
					this._selectedAxis = GizmoAxis.None;
					this._selectedMultiAxisSquare = MultiAxisSquare.None;
				}
				return;
			}
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0))
			{
				return;
			}
			this.DetectHoveredComponents(true);
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x0014774C File Offset: 0x0014594C
		private List<GameObject> GetObjectsForClosestVertexSelection(IEnumerable<GameObject> gameObjects, bool onlyHoveredByCursor = false)
		{
			if (gameObjects == null)
			{
				return new List<GameObject>();
			}
			Vector2 vector;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out vector))
			{
				return new List<GameObject>();
			}
			float num = float.MaxValue;
			GameObject gameObject = null;
			Ray ray;
			bool pickRay = MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray);
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject2 in gameObjects)
			{
				if ((!(gameObject2.GetMesh() == null) || !(gameObject2.GetComponent<SpriteRenderer>() == null)) && (!onlyHoveredByCursor || (pickRay && gameObject2.GetWorldBox().ToBounds().IntersectRay(ray))))
				{
					Rect screenRectangle = gameObject2.GetScreenRectangle(this._camera);
					if (screenRectangle.Contains(vector, true))
					{
						list.Add(gameObject2);
					}
					else if (list.Count == 0)
					{
						float magnitude = (screenRectangle.GetClosestPointToPoint(vector) - vector).magnitude;
						if (magnitude < num)
						{
							num = magnitude;
							gameObject = gameObject2;
						}
					}
				}
			}
			if (list.Count == 0 && gameObject == null)
			{
				return new List<GameObject>();
			}
			if (list.Count == 0)
			{
				list.Add(gameObject);
			}
			return list;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x0014789C File Offset: 0x00145A9C
		private Vector3 GetWorldPositionClosestToMouseCursorForVertexSnapping(List<GameObject> gameObjects, bool considerOnlyMeshAndSpriteObjects)
		{
			Vector2 vector;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out vector))
			{
				return Vector3.zero;
			}
			List<GameObject> list = gameObjects.FindAll((GameObject item) => item.GetMesh() != null);
			List<GameObject> list2 = gameObjects.FindAll((GameObject item) => item.GetComponent<SpriteRenderer>() != null);
			if (list.Count != 0)
			{
				MeshVertexGroupMappings instance = SingletonBase<MeshVertexGroupMappings>.Instance;
				Dictionary<GameObject, List<MeshVertexGroup>> dictionary = new Dictionary<GameObject, List<MeshVertexGroup>>();
				foreach (GameObject gameObject in gameObjects)
				{
					Mesh mesh = gameObject.GetMesh();
					List<MeshVertexGroup> meshVertexGroups = instance.GetMeshVertexGroups(mesh);
					if (meshVertexGroups.Count != 0)
					{
						dictionary.Add(gameObject, meshVertexGroups);
					}
				}
				Vector3 result = Vector3.zero;
				float num = float.MaxValue;
				float num2 = float.MaxValue;
				MeshVertexGroup meshVertexGroup = null;
				GameObject gameObject2 = null;
				bool flag = false;
				foreach (KeyValuePair<GameObject, List<MeshVertexGroup>> keyValuePair in dictionary)
				{
					GameObject key = keyValuePair.Key;
					List<MeshVertexGroup> value = keyValuePair.Value;
					Matrix4x4 localToWorldMatrix = key.transform.localToWorldMatrix;
					foreach (MeshVertexGroup meshVertexGroup2 in value)
					{
						Rect screenRectangle = meshVertexGroup2.GroupAABB.Transform(localToWorldMatrix).GetScreenRectangle(this._camera);
						if (screenRectangle.Contains(vector, true))
						{
							flag = true;
							foreach (Vector3 point in meshVertexGroup2.ModelSpaceVertices)
							{
								Vector3 vector2 = localToWorldMatrix.MultiplyPoint(point);
								float magnitude = (this._camera.WorldToScreenPoint(vector2) - vector).magnitude;
								if (magnitude < num)
								{
									num = magnitude;
									result = vector2;
								}
							}
						}
						if (!flag)
						{
							float magnitude2 = (screenRectangle.GetClosestPointToPoint(vector) - vector).magnitude;
							if (magnitude2 < num2)
							{
								num2 = magnitude2;
								meshVertexGroup = meshVertexGroup2;
								gameObject2 = key;
							}
						}
					}
				}
				if (!flag && meshVertexGroup != null)
				{
					num = float.MaxValue;
					Matrix4x4 localToWorldMatrix2 = gameObject2.transform.localToWorldMatrix;
					foreach (Vector3 point2 in meshVertexGroup.ModelSpaceVertices)
					{
						Vector3 vector3 = localToWorldMatrix2.MultiplyPoint(point2);
						float magnitude3 = (this._camera.WorldToScreenPoint(vector3) - vector).magnitude;
						if (magnitude3 < num)
						{
							num = magnitude3;
							result = vector3;
						}
					}
				}
				return result;
			}
			if (list2.Count != 0)
			{
				Vector3 result2 = Vector3.zero;
				float num3 = float.MaxValue;
				foreach (GameObject gameObject3 in list2)
				{
					SpriteRenderer component = gameObject3.GetComponent<SpriteRenderer>();
					if (!(component.sprite == null))
					{
						foreach (Vector3 vector4 in component.GetWorldCenterAndCornerPoints())
						{
							float magnitude4 = (this._camera.WorldToScreenPoint(vector4) - vector).magnitude;
							if (magnitude4 < num3)
							{
								num3 = magnitude4;
								result2 = vector4;
							}
						}
					}
				}
				return result2;
			}
			if (!considerOnlyMeshAndSpriteObjects)
			{
				Vector3 result3 = Vector3.zero;
				float num4 = float.MaxValue;
				foreach (GameObject gameObject4 in gameObjects)
				{
					Vector3 position = gameObject4.transform.position;
					float magnitude5 = (this._camera.WorldToScreenPoint(position) - vector).magnitude;
					if (magnitude5 < num4)
					{
						num4 = magnitude5;
						result3 = position;
					}
				}
				return result3;
			}
			return Vector3.zero;
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x00147D7C File Offset: 0x00145F7C
		protected override void OnInputDeviceFirstButtonDown()
		{
			base.OnInputDeviceFirstButtonDown();
			if (MonoSingletonBase<InputDevice>.Instance.UsingMobile)
			{
				this.DetectHoveredComponents(true);
			}
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0) && (this._selectedAxis != GizmoAxis.None || this._selectedMultiAxisSquare != MultiAxisSquare.None || this._isCameraAxesTranslationSquareSelected))
			{
				Plane plane;
				if (this._selectedAxis != GizmoAxis.None)
				{
					plane = base.GetCoordinateSystemPlaneFromSelectedAxis();
				}
				else if (this._selectedMultiAxisSquare != MultiAxisSquare.None)
				{
					plane = this.GetPlaneFromSelectedMultiAxisSquare();
				}
				else
				{
					plane = this.GetCameraAxesTranslationSquarePlane();
				}
				Ray ray;
				float d;
				if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray) && plane.Raycast(ray, out d))
				{
					this._lastGizmoPickPoint = ray.origin + ray.direction * d;
				}
			}
			this._accumulatedTranslation = Vector3.zero;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x00147E42 File Offset: 0x00146042
		protected override void OnInputDeviceFirstButtonUp()
		{
			base.OnInputDeviceFirstButtonUp();
			this._objectOffsetsFromGizmo.Clear();
			if (MonoSingletonBase<InputDevice>.Instance.UsingMobile)
			{
				this._selectedAxis = GizmoAxis.None;
				this._selectedMultiAxisSquare = MultiAxisSquare.None;
				this._isCameraAxesTranslationSquareSelected = false;
				this._isSpecialOpSquareSelected = false;
			}
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x00147E80 File Offset: 0x00146080
		protected override void OnInputDeviceMoved()
		{
			base.OnInputDeviceMoved();
			if (!base.CanAnyControlledObjectBeManipulated())
			{
				return;
			}
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0))
			{
				if (this._isVertexSnapping)
				{
					List<GameObject> visibleGameObjects = this._camera.GetVisibleGameObjects();
					if (base.ControlledObjects != null)
					{
						HashSet<GameObject> controlledObjects = new HashSet<GameObject>(base.ControlledObjects);
						List<GameObject> parents = GameObjectExtensions.GetParentsFromObjectCollection(controlledObjects);
						visibleGameObjects.RemoveAll((GameObject item) => controlledObjects.Contains(item) || parents.FindAll((GameObject parentItem) => item.transform.IsChildOf(parentItem.transform)).Count != 0);
					}
					List<GameObject> objectsForClosestVertexSelection = this.GetObjectsForClosestVertexSelection(visibleGameObjects, true);
					if (objectsForClosestVertexSelection.Count != 0)
					{
						Vector3 vector = this.GetWorldPositionClosestToMouseCursorForVertexSnapping(objectsForClosestVertexSelection, true) - this._gizmoTransform.position;
						this._gizmoTransform.position += vector;
						this.TranslateControlledObjects(vector);
						return;
					}
					Vector3 vector2 = MonoSingletonBase<RuntimeEditorApplication>.Instance.XZGrid.GetCellCornerPointClosestToInputDevPos() - this._gizmoTransform.position;
					this._gizmoTransform.position += vector2;
					this.TranslateControlledObjects(vector2);
					return;
				}
				else if (this._selectedAxis != GizmoAxis.None)
				{
					Vector3 vector3;
					if (this._selectedAxis == GizmoAxis.X)
					{
						vector3 = this._gizmoTransform.right;
					}
					else if (this._selectedAxis == GizmoAxis.Y)
					{
						vector3 = this._gizmoTransform.up;
					}
					else
					{
						vector3 = this._gizmoTransform.forward;
					}
					Plane coordinateSystemPlaneFromSelectedAxis = base.GetCoordinateSystemPlaneFromSelectedAxis();
					Ray ray;
					float d;
					if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray) && coordinateSystemPlaneFromSelectedAxis.Raycast(ray, out d))
					{
						Vector3 vector4 = ray.origin + ray.direction * d;
						float num = Vector3.Dot(vector4 - this._lastGizmoPickPoint, vector3);
						this._lastGizmoPickPoint = vector4;
						if (!this.IsStepSnappingShActive)
						{
							Vector3 vector5 = vector3 * num;
							this._gizmoTransform.position += vector5;
							this.TranslateControlledObjects(vector5);
							return;
						}
						int selectedAxis = (int)this._selectedAxis;
						ref Vector3 ptr = ref this._accumulatedTranslation;
						int index = selectedAxis;
						ptr[index] += num;
						if (Mathf.Abs(this._accumulatedTranslation[selectedAxis]) >= this._snapSettings.StepValueInWorldUnits)
						{
							float num2 = (float)((int)Mathf.Abs(this._accumulatedTranslation[selectedAxis] / this._snapSettings.StepValueInWorldUnits));
							float d2 = this._snapSettings.StepValueInWorldUnits * num2 * Mathf.Sign(this._accumulatedTranslation[selectedAxis]);
							Vector3 vector6 = vector3 * d2;
							this._gizmoTransform.position += vector6;
							this.TranslateControlledObjects(vector6);
							if (this._accumulatedTranslation[selectedAxis] > 0f)
							{
								ptr = ref this._accumulatedTranslation;
								index = selectedAxis;
								ptr[index] -= this._snapSettings.StepValueInWorldUnits * num2;
								return;
							}
							if (this._accumulatedTranslation[selectedAxis] < 0f)
							{
								ptr = ref this._accumulatedTranslation;
								index = selectedAxis;
								ptr[index] += this._snapSettings.StepValueInWorldUnits * num2;
								return;
							}
						}
					}
				}
				else if (this._selectedMultiAxisSquare != MultiAxisSquare.None)
				{
					float[] multiAxisExtensionSigns = base.GetMultiAxisExtensionSigns(this._adjustMultiAxisForBetterVisibility);
					Vector3 vector7;
					Vector3 vector8;
					int num3;
					int num4;
					if (this._selectedMultiAxisSquare == MultiAxisSquare.XY)
					{
						vector7 = this._gizmoTransform.right * multiAxisExtensionSigns[0];
						vector8 = this._gizmoTransform.up * multiAxisExtensionSigns[1];
						num3 = 0;
						num4 = 1;
					}
					else if (this._selectedMultiAxisSquare == MultiAxisSquare.XZ)
					{
						vector7 = this._gizmoTransform.right * multiAxisExtensionSigns[0];
						vector8 = this._gizmoTransform.forward * multiAxisExtensionSigns[2];
						num3 = 0;
						num4 = 2;
					}
					else
					{
						vector7 = this._gizmoTransform.up * multiAxisExtensionSigns[1];
						vector8 = this._gizmoTransform.forward * multiAxisExtensionSigns[2];
						num3 = 1;
						num4 = 2;
					}
					Plane planeFromSelectedMultiAxisSquare = this.GetPlaneFromSelectedMultiAxisSquare();
					Ray ray2;
					float d3;
					if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray2) && planeFromSelectedMultiAxisSquare.Raycast(ray2, out d3))
					{
						Vector3 vector9 = ray2.origin + ray2.direction * d3;
						Vector3 lhs = vector9 - this._lastGizmoPickPoint;
						float num5 = Vector3.Dot(lhs, vector7);
						float num6 = Vector3.Dot(lhs, vector8);
						this._lastGizmoPickPoint = vector9;
						if (this.IsStepSnappingShActive)
						{
							ref Vector3 ptr = ref this._accumulatedTranslation;
							int index = num3;
							ptr[index] += num5;
							ptr = ref this._accumulatedTranslation;
							index = num4;
							ptr[index] += num6;
							Vector3 vector10 = Vector3.zero;
							if (Mathf.Abs(this._accumulatedTranslation[num3]) >= this._snapSettings.StepValueInWorldUnits)
							{
								float num7 = (float)((int)Mathf.Abs(this._accumulatedTranslation[num3] / this._snapSettings.StepValueInWorldUnits));
								float d4 = this._snapSettings.StepValueInWorldUnits * num7 * Mathf.Sign(this._accumulatedTranslation[num3]);
								vector10 += vector7 * d4;
								if (this._accumulatedTranslation[num3] > 0f)
								{
									ptr = ref this._accumulatedTranslation;
									index = num3;
									ptr[index] -= this._snapSettings.StepValueInWorldUnits * num7;
								}
								else if (this._accumulatedTranslation[num3] < 0f)
								{
									ptr = ref this._accumulatedTranslation;
									index = num3;
									ptr[index] += this._snapSettings.StepValueInWorldUnits * num7;
								}
							}
							if (Mathf.Abs(this._accumulatedTranslation[num4]) >= this._snapSettings.StepValueInWorldUnits)
							{
								float num8 = (float)((int)Mathf.Abs(this._accumulatedTranslation[num4] / this._snapSettings.StepValueInWorldUnits));
								float d5 = this._snapSettings.StepValueInWorldUnits * num8 * Mathf.Sign(this._accumulatedTranslation[num4]);
								vector10 += vector8 * d5;
								if (this._accumulatedTranslation[num4] > 0f)
								{
									ptr = ref this._accumulatedTranslation;
									index = num4;
									ptr[index] -= this._snapSettings.StepValueInWorldUnits * num8;
								}
								else if (this._accumulatedTranslation[num4] < 0f)
								{
									ptr = ref this._accumulatedTranslation;
									index = num4;
									ptr[index] += this._snapSettings.StepValueInWorldUnits * num8;
								}
							}
							this._gizmoTransform.position += vector10;
							this.TranslateControlledObjects(vector10);
							return;
						}
						Vector3 vector11 = num5 * vector7 + num6 * vector8;
						this._gizmoTransform.position += vector11;
						this.TranslateControlledObjects(vector11);
						return;
					}
				}
				else if (this.IsTranslateAlongScreenAxesShActive && this._isCameraAxesTranslationSquareSelected)
				{
					Plane cameraAxesTranslationSquarePlane = this.GetCameraAxesTranslationSquarePlane();
					Ray ray3;
					float d6;
					if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray3) && cameraAxesTranslationSquarePlane.Raycast(ray3, out d6))
					{
						Vector3 vector12 = ray3.origin + ray3.direction * d6;
						Vector3 lhs2 = vector12 - this._lastGizmoPickPoint;
						float num9 = Vector3.Dot(lhs2, this._cameraTransform.right);
						float num10 = Vector3.Dot(lhs2, this._cameraTransform.up);
						this._lastGizmoPickPoint = vector12;
						if (this.IsStepSnappingShActive)
						{
							ref Vector3 ptr = ref this._accumulatedTranslation;
							ptr[0] = ptr[0] + num9;
							ptr = ref this._accumulatedTranslation;
							ptr[1] = ptr[1] + num10;
							Vector3 vector13 = Vector3.zero;
							if (Mathf.Abs(this._accumulatedTranslation[0]) >= this._snapSettings.StepValueInWorldUnits)
							{
								float num11 = (float)((int)Mathf.Abs(this._accumulatedTranslation[0] / this._snapSettings.StepValueInWorldUnits));
								float d7 = this._snapSettings.StepValueInWorldUnits * num11 * Mathf.Sign(this._accumulatedTranslation[0]);
								vector13 += this._cameraTransform.right * d7;
								if (this._accumulatedTranslation[0] > 0f)
								{
									ptr = ref this._accumulatedTranslation;
									ptr[0] = ptr[0] - this._snapSettings.StepValueInWorldUnits * num11;
								}
								else if (this._accumulatedTranslation[0] < 0f)
								{
									ptr = ref this._accumulatedTranslation;
									ptr[0] = ptr[0] + this._snapSettings.StepValueInWorldUnits * num11;
								}
							}
							if (Mathf.Abs(this._accumulatedTranslation[1]) >= this._snapSettings.StepValueInWorldUnits)
							{
								float num12 = (float)((int)Mathf.Abs(this._accumulatedTranslation[1] / this._snapSettings.StepValueInWorldUnits));
								float d8 = this._snapSettings.StepValueInWorldUnits * num12 * Mathf.Sign(this._accumulatedTranslation[1]);
								vector13 += this._cameraTransform.up * d8;
								if (this._accumulatedTranslation[1] > 0f)
								{
									ptr = ref this._accumulatedTranslation;
									ptr[1] = ptr[1] - this._snapSettings.StepValueInWorldUnits * num12;
								}
								else if (this._accumulatedTranslation[1] < 0f)
								{
									ptr = ref this._accumulatedTranslation;
									ptr[1] = ptr[1] + this._snapSettings.StepValueInWorldUnits * num12;
								}
							}
							this._gizmoTransform.position += vector13;
							this.TranslateControlledObjects(vector13);
							return;
						}
						Vector3 vector14 = this._cameraTransform.right * num9 + this._cameraTransform.up * num10;
						this._gizmoTransform.position += vector14;
						this.TranslateControlledObjects(vector14);
						return;
					}
				}
				else if (this.IsSurfacePlacementShActive && base.ControlledObjects != null && this._isSpecialOpSquareSelected)
				{
					if (this._objectOffsetsFromGizmo.Count == 0)
					{
						foreach (GameObject gameObject in GameObjectExtensions.GetParentsFromObjectCollection(base.ControlledObjects))
						{
							this._objectOffsetsFromGizmo.Add(gameObject, gameObject.transform.position - this._gizmoTransform.position);
						}
					}
					bool isSurfacePlacementAlignXShActive = this.IsSurfacePlacementAlignXShActive;
					bool isSurfacePlacementAlignZShActive = this.IsSurfacePlacementAlignZShActive;
					bool flag = !this.IsSurfacePlacementNoAlignShActive;
					Axis axis = Axis.Y;
					if (isSurfacePlacementAlignXShActive)
					{
						axis = Axis.X;
					}
					else if (isSurfacePlacementAlignZShActive)
					{
						axis = Axis.Z;
					}
					SingletonBase<MouseCursor>.Instance.PushObjectPickMaskFlags(MouseCursorObjectPickFlags.ObjectBox | MouseCursorObjectPickFlags.ObjectSprite);
					MouseCursorRayHit rayHit = SingletonBase<MouseCursor>.Instance.GetRayHit();
					if (rayHit == null && (!rayHit.WasAnObjectHit || !rayHit.WasACellHit))
					{
						return;
					}
					List<GameObject> ignoreObjects = new List<GameObject>();
					foreach (GameObject gameObject2 in base.ControlledObjects)
					{
						List<GameObject> allChildrenIncludingSelf = gameObject2.GetAllChildrenIncludingSelf();
						ignoreObjects.AddRange(allChildrenIncludingSelf);
					}
					rayHit.SortedObjectRayHits.RemoveAll((GameObjectRayHit item) => ignoreObjects.Contains(item.HitObject));
					if (rayHit.WasAnObjectHit && rayHit.ClosestObjectRayHit.WasTerrainHit)
					{
						Vector3 hitPoint = rayHit.ClosestObjectRayHit.HitPoint;
						TerrainCollider component = rayHit.ClosestObjectRayHit.HitObject.GetComponent<TerrainCollider>();
						if (component != null)
						{
							List<GameObject> parentsFromObjectCollection = GameObjectExtensions.GetParentsFromObjectCollection(base.ControlledObjects);
							if (parentsFromObjectCollection.Count != 0)
							{
								foreach (GameObject gameObject3 in parentsFromObjectCollection)
								{
									Transform transform = gameObject3.transform;
									transform.position = hitPoint + this._objectOffsetsFromGizmo[gameObject3];
									Ray ray4 = new Ray(transform.position, -Vector3.up);
									RaycastHit raycastHit;
									if (component.RaycastReverseIfFail(ray4, out raycastHit))
									{
										gameObject3.PlaceHierarchyOnPlane(raycastHit.point, raycastHit.normal, (int)(flag ? axis : ((Axis)(-1))));
										IRTEditorEventListener component2 = gameObject3.GetComponent<IRTEditorEventListener>();
										if (component2 != null)
										{
											component2.OnAlteredByTransformGizmo(this);
										}
										this._objectsWereTransformedSinceLeftMouseButtonWasPressed = true;
									}
								}
								this._gizmoTransform.position = hitPoint;
								return;
							}
						}
					}
					else if (rayHit.WasAnObjectHit && rayHit.ClosestObjectRayHit.WasMeshHit)
					{
						Vector3 hitPoint2 = rayHit.ClosestObjectRayHit.HitPoint;
						GameObject hitObject = rayHit.ClosestObjectRayHit.HitObject;
						Vector3 hitNormal = rayHit.ClosestObjectRayHit.HitNormal;
						if (hitObject != null)
						{
							List<GameObject> parentsFromObjectCollection2 = GameObjectExtensions.GetParentsFromObjectCollection(base.ControlledObjects);
							if (parentsFromObjectCollection2.Count != 0)
							{
								foreach (GameObject gameObject4 in parentsFromObjectCollection2)
								{
									Transform transform2 = gameObject4.transform;
									transform2.position = hitPoint2 + this._objectOffsetsFromGizmo[gameObject4];
									GameObjectRayHit gameObjectRayHit = null;
									Ray ray5 = new Ray(transform2.position + hitNormal, -hitNormal);
									if (hitObject.RaycastMesh(ray5, out gameObjectRayHit))
									{
										gameObject4.PlaceHierarchyOnPlane(gameObjectRayHit.HitPoint, gameObjectRayHit.HitNormal, (int)(flag ? axis : ((Axis)(-1))));
										IRTEditorEventListener component3 = gameObject4.GetComponent<IRTEditorEventListener>();
										if (component3 != null)
										{
											component3.OnAlteredByTransformGizmo(this);
										}
										this._objectsWereTransformedSinceLeftMouseButtonWasPressed = true;
									}
								}
								this._gizmoTransform.position = hitPoint2;
								return;
							}
						}
					}
					else if (rayHit.WasACellHit)
					{
						Plane plane = rayHit.GridCellRayHit.HitCell.ParentGrid.Plane;
						Vector3 hitPoint3 = rayHit.GridCellRayHit.HitPoint;
						List<GameObject> parentsFromObjectCollection3 = GameObjectExtensions.GetParentsFromObjectCollection(base.ControlledObjects);
						if (parentsFromObjectCollection3.Count != 0)
						{
							foreach (GameObject gameObject5 in parentsFromObjectCollection3)
							{
								Transform transform3 = gameObject5.transform;
								transform3.position = hitPoint3 + this._objectOffsetsFromGizmo[gameObject5];
								float distanceToPoint = plane.GetDistanceToPoint(transform3.position);
								Vector3 ptOnPlane = transform3.position - distanceToPoint * plane.normal;
								gameObject5.PlaceHierarchyOnPlane(ptOnPlane, plane.normal, (int)(flag ? axis : ((Axis)(-1))));
								IRTEditorEventListener component4 = gameObject5.GetComponent<IRTEditorEventListener>();
								if (component4 != null)
								{
									component4.OnAlteredByTransformGizmo(this);
								}
								this._objectsWereTransformedSinceLeftMouseButtonWasPressed = true;
							}
							this._gizmoTransform.position = hitPoint3;
						}
					}
				}
			}
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x00148DDC File Offset: 0x00146FDC
		protected override void OnRenderObject()
		{
			if (Camera.current != MonoSingletonBase<EditorCamera>.Instance.Camera)
			{
				return;
			}
			base.OnRenderObject();
			float num = base.CalculateGizmoScale();
			bool isSurfacePlacementShActive = this.IsSurfacePlacementShActive;
			Matrix4x4[] arrowConesWorldTransforms = this.GetArrowConesWorldTransforms();
			this.DrawArrowCones(arrowConesWorldTransforms);
			if (!this.IsTranslateAlongScreenAxesShActive && !this._isVertexSnapping && !isSurfacePlacementShActive)
			{
				Matrix4x4[] multiAxisSquaresWorldTransforms = this.GetMultiAxisSquaresWorldTransforms();
				this.DrawMultiAxisSquares(multiAxisSquaresWorldTransforms);
			}
			int[] sortedGizmoAxesIndices = base.GetSortedGizmoAxesIndices();
			Vector3[] gizmoLocalAxes = base.GetGizmoLocalAxes();
			Vector3 position = this._gizmoTransform.position;
			foreach (int num2 in sortedGizmoAxesIndices)
			{
				if (this._axesVisibilityMask[num2])
				{
					Color lineColor = (this._selectedAxis == (GizmoAxis)num2) ? this._selectedAxisColor : this._axesColors[num2];
					Vector3 vector = position + gizmoLocalAxes[num2] * this._axisLength * num;
					base.UpdateShaderStencilRefValuesForGizmoAxisLineDraw(num2, position, vector, num);
					GLPrimitives.Draw3DLine(position, vector, lineColor, SingletonBase<MaterialPool>.Instance.GizmoLine);
				}
			}
			SingletonBase<MaterialPool>.Instance.GizmoLine.SetInt("_StencilRefValue", this._doNotUseStencil);
			if (!this.IsTranslateAlongScreenAxesShActive && !this._isVertexSnapping && !isSurfacePlacementShActive)
			{
				Vector3[] linePoints;
				Color[] lineColors;
				this.GetMultiAxisSquaresLinePointsAndColors(num, out linePoints, out lineColors);
				GLPrimitives.Draw3DLines(linePoints, lineColors, false, SingletonBase<MaterialPool>.Instance.GizmoLine, false, Color.black);
			}
			if (this.IsTranslateAlongScreenAxesShActive)
			{
				Color borderLineColor = this._isCameraAxesTranslationSquareSelected ? this.SpecialOpSquareColorWhenSelected : this.SpecialOpSquareColor;
				GLPrimitives.Draw2DRectangleBorderLines(this.GetSpecialOpSquareScreenPoints(), borderLineColor, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
			}
			if (this._isVertexSnapping)
			{
				Color borderLineColor2 = MonoSingletonBase<InputDevice>.Instance.IsPressed(0) ? this._specialOpSquareColorWhenSelected : this._specialOpSquareColor;
				GLPrimitives.Draw2DRectangleBorderLines(this.GetSpecialOpSquareScreenPoints(), borderLineColor2, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
			}
			if (isSurfacePlacementShActive)
			{
				Color borderLineColor3 = this._isSpecialOpSquareSelected ? this._specialOpSquareColorWhenSelected : this._specialOpSquareColor;
				GLPrimitives.Draw2DRectangleBorderLines(this.GetSpecialOpSquareScreenPoints(), borderLineColor3, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
			}
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x00148FF8 File Offset: 0x001471F8
		protected override bool DetectHoveredComponents(bool updateCompStates)
		{
			if (updateCompStates)
			{
				bool isSurfacePlacementShActive = this.IsSurfacePlacementShActive;
				this._selectedAxis = GizmoAxis.None;
				this._selectedMultiAxisSquare = MultiAxisSquare.None;
				this._isCameraAxesTranslationSquareSelected = false;
				this._isSpecialOpSquareSelected = this.IsMouseCursorInsideSpecialOpSquare();
				if (this._isSpecialOpSquareSelected && isSurfacePlacementShActive)
				{
					return false;
				}
				if (this._camera == null)
				{
					return false;
				}
				Ray ray;
				bool pickRay = MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray);
				float num = float.MaxValue;
				float num2 = base.CalculateGizmoScale();
				float cylinderRadius = 0.2f * num2;
				Vector3 position = this._cameraTransform.position;
				Vector3 position2 = this._gizmoTransform.position;
				if (pickRay)
				{
					Matrix4x4[] arrowConesWorldTransforms = this.GetArrowConesWorldTransforms();
					Vector3[] gizmoLocalAxes = base.GetGizmoLocalAxes();
					Vector3 cylinderAxisFirstPoint = position2;
					for (int i = 0; i < 3; i++)
					{
						if (this._axesVisibilityMask[i])
						{
							bool flag = false;
							Vector3 cylinderAxisSecondPoint = position2 + gizmoLocalAxes[i] * this._axisLength * num2;
							float d;
							if (ray.IntersectsCylinder(cylinderAxisFirstPoint, cylinderAxisSecondPoint, cylinderRadius, out d))
							{
								float magnitude = (ray.origin + ray.direction * d - position).magnitude;
								if (magnitude < num)
								{
									num = magnitude;
									this._selectedAxis = (GizmoAxis)i;
									flag = true;
								}
							}
							if (!flag && ray.IntersectsCone(1f, 1f, arrowConesWorldTransforms[i], out d))
							{
								float magnitude2 = (ray.origin + ray.direction * d - position).magnitude;
								if (magnitude2 < num)
								{
									num = magnitude2;
									this._selectedAxis = (GizmoAxis)i;
								}
							}
						}
					}
					if (!this.IsTranslateAlongScreenAxesShActive)
					{
						Vector3[] array = new Vector3[]
						{
							this._gizmoTransform.forward,
							this._gizmoTransform.up,
							this._gizmoTransform.right
						};
						float[] multiAxisExtensionSigns = base.GetMultiAxisExtensionSigns(this._adjustMultiAxisForBetterVisibility);
						float num3 = Mathf.Sign(num2);
						Vector3[] array2 = new Vector3[]
						{
							this._gizmoTransform.right * multiAxisExtensionSigns[0],
							this._gizmoTransform.up * multiAxisExtensionSigns[1],
							this._gizmoTransform.right * multiAxisExtensionSigns[0],
							this._gizmoTransform.forward * multiAxisExtensionSigns[2],
							this._gizmoTransform.up * multiAxisExtensionSigns[1],
							this._gizmoTransform.forward * multiAxisExtensionSigns[2]
						};
						for (int j = 0; j < 3; j++)
						{
							if (this.IsMultiAxisSquareVisible(j))
							{
								Plane plane = new Plane(array[j], this._gizmoTransform.position);
								float d;
								if (plane.Raycast(ray, out d))
								{
									Vector3 a = ray.origin + ray.direction * d;
									Vector3 lhs = a - this._gizmoTransform.position;
									float num4 = Vector3.Dot(lhs, array2[j * 2]) * num3;
									float num5 = Vector3.Dot(lhs, array2[j * 2 + 1]) * num3;
									if (num4 >= 0f && num4 <= this._multiAxisSquareSize * Mathf.Abs(num2) && num5 >= 0f && num5 <= this._multiAxisSquareSize * Mathf.Abs(num2))
									{
										float magnitude3 = (a - position).magnitude;
										if (magnitude3 < num)
										{
											num = magnitude3;
											this._selectedMultiAxisSquare = (MultiAxisSquare)j;
											this._selectedAxis = GizmoAxis.None;
										}
									}
								}
							}
						}
					}
					if (this.IsTranslateAlongScreenAxesShActive && this.IsMouseCursorInsideSpecialOpSquare())
					{
						this._isCameraAxesTranslationSquareSelected = true;
						this._selectedAxis = GizmoAxis.None;
						this._selectedMultiAxisSquare = MultiAxisSquare.None;
					}
				}
				return this._selectedAxis != GizmoAxis.None || this._selectedMultiAxisSquare != MultiAxisSquare.None || this._isCameraAxesTranslationSquareSelected;
			}
			else
			{
				if (this.IsSurfacePlacementShActive)
				{
					return false;
				}
				if (this._camera == null)
				{
					return false;
				}
				Ray ray2;
				bool pickRay2 = MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray2);
				float num6 = base.CalculateGizmoScale();
				float cylinderRadius2 = 0.2f * num6;
				Vector3 position3 = this._gizmoTransform.position;
				if (pickRay2)
				{
					Matrix4x4[] arrowConesWorldTransforms2 = this.GetArrowConesWorldTransforms();
					Vector3[] gizmoLocalAxes2 = base.GetGizmoLocalAxes();
					Vector3 cylinderAxisFirstPoint2 = position3;
					for (int k = 0; k < 3; k++)
					{
						if (this._axesVisibilityMask[k])
						{
							Vector3 cylinderAxisSecondPoint2 = position3 + gizmoLocalAxes2[k] * this._axisLength * num6;
							float d2;
							if (ray2.IntersectsCylinder(cylinderAxisFirstPoint2, cylinderAxisSecondPoint2, cylinderRadius2, out d2))
							{
								return true;
							}
							if (ray2.IntersectsCone(1f, 1f, arrowConesWorldTransforms2[k], out d2))
							{
								return true;
							}
						}
					}
					if (!this.IsTranslateAlongScreenAxesShActive)
					{
						Vector3[] array3 = new Vector3[]
						{
							this._gizmoTransform.forward,
							this._gizmoTransform.up,
							this._gizmoTransform.right
						};
						float[] multiAxisExtensionSigns2 = base.GetMultiAxisExtensionSigns(this._adjustMultiAxisForBetterVisibility);
						float num7 = Mathf.Sign(num6);
						Vector3[] array4 = new Vector3[]
						{
							this._gizmoTransform.right * multiAxisExtensionSigns2[0],
							this._gizmoTransform.up * multiAxisExtensionSigns2[1],
							this._gizmoTransform.right * multiAxisExtensionSigns2[0],
							this._gizmoTransform.forward * multiAxisExtensionSigns2[2],
							this._gizmoTransform.up * multiAxisExtensionSigns2[1],
							this._gizmoTransform.forward * multiAxisExtensionSigns2[2]
						};
						for (int l = 0; l < 3; l++)
						{
							if (this.IsMultiAxisSquareVisible(l))
							{
								Plane plane2 = new Plane(array3[l], this._gizmoTransform.position);
								float d2;
								if (plane2.Raycast(ray2, out d2))
								{
									Vector3 lhs2 = ray2.origin + ray2.direction * d2 - this._gizmoTransform.position;
									float num8 = Vector3.Dot(lhs2, array4[l * 2]) * num7;
									float num9 = Vector3.Dot(lhs2, array4[l * 2 + 1]) * num7;
									if (num8 >= 0f && num8 <= this._multiAxisSquareSize * Mathf.Abs(num6) && num9 >= 0f && num9 <= this._multiAxisSquareSize * Mathf.Abs(num6))
									{
										return true;
									}
								}
							}
						}
					}
					if (this.IsTranslateAlongScreenAxesShActive && this.IsMouseCursorInsideSpecialOpSquare())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x001496C0 File Offset: 0x001478C0
		private bool IsMultiAxisSquareVisible(int multiAxisIndex)
		{
			if (multiAxisIndex == 0)
			{
				if (!this._axesVisibilityMask[0] || !this._axesVisibilityMask[1])
				{
					return false;
				}
			}
			else if (multiAxisIndex == 1)
			{
				if (!this._axesVisibilityMask[0] || !this._axesVisibilityMask[2])
				{
					return false;
				}
			}
			else if (!this._axesVisibilityMask[1] || !this._axesVisibilityMask[2])
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x0014971C File Offset: 0x0014791C
		private void GetMultiAxisSquaresLinePointsAndColors(float gizmoScale, out Vector3[] squareLinesPoints, out Color[] squareLinesColors)
		{
			float d = (this._multiAxisSquareSize + 0.001f) * gizmoScale;
			squareLinesPoints = new Vector3[24];
			squareLinesColors = new Color[12];
			Vector3[] worldAxesUsedToDrawMultiAxisSquareLines = this.GetWorldAxesUsedToDrawMultiAxisSquareLines();
			for (int i = 0; i < 3; i++)
			{
				Color multiAxisSquareLineColor = this.GetMultiAxisSquareLineColor((MultiAxisSquare)i, this._selectedMultiAxisSquare == (MultiAxisSquare)i);
				if (!this.IsMultiAxisSquareVisible(i))
				{
					multiAxisSquareLineColor.a = 0f;
				}
				int num = i * 4;
				squareLinesColors[num] = multiAxisSquareLineColor;
				squareLinesColors[num + 1] = multiAxisSquareLineColor;
				squareLinesColors[num + 2] = multiAxisSquareLineColor;
				squareLinesColors[num + 3] = multiAxisSquareLineColor;
				int num2 = i * 2;
				Vector3 position = this._gizmoTransform.position;
				Vector3 vector = position + worldAxesUsedToDrawMultiAxisSquareLines[num2 + 1] * d;
				Vector3 vector2 = vector + worldAxesUsedToDrawMultiAxisSquareLines[num2] * d;
				Vector3 vector3 = position + worldAxesUsedToDrawMultiAxisSquareLines[num2] * d;
				int num3 = i * 8;
				squareLinesPoints[num3] = position;
				squareLinesPoints[num3 + 1] = vector;
				squareLinesPoints[num3 + 2] = vector;
				squareLinesPoints[num3 + 3] = vector2;
				squareLinesPoints[num3 + 4] = vector2;
				squareLinesPoints[num3 + 5] = vector3;
				squareLinesPoints[num3 + 6] = vector3;
				squareLinesPoints[num3 + 7] = position;
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x00149880 File Offset: 0x00147A80
		private Vector2[] GetSpecialOpSquareScreenPoints()
		{
			Vector2 a = this._camera.WorldToScreenPoint(this._gizmoTransform.position);
			float d = this.ScreenSizeOfSpecialOpSquare * 0.5f;
			return new Vector2[]
			{
				a - (Vector2.right - Vector2.up) * d,
				a + (Vector2.right + Vector2.up) * d,
				a + (Vector2.right - Vector2.up) * d,
				a - (Vector2.right + Vector2.up) * d
			};
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x00149944 File Offset: 0x00147B44
		private bool IsMouseCursorInsideSpecialOpSquare()
		{
			Vector2 b = this._camera.WorldToScreenPoint(this._gizmoTransform.position);
			float num = this.ScreenSizeOfSpecialOpSquare * 0.5f;
			Vector2 a;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out a))
			{
				return false;
			}
			Vector2 vector = a - b;
			return Mathf.Abs(vector.x) <= num && Mathf.Abs(vector.y) <= num;
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x001499B4 File Offset: 0x00147BB4
		private void DrawMultiAxisSquares(Matrix4x4[] worldTransformMatrices)
		{
			Material gizmoSolidComponent = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
			gizmoSolidComponent.SetInt("_ZTest", 0);
			gizmoSolidComponent.SetInt("_ZWrite", 1);
			gizmoSolidComponent.SetVector("_LightDir", this._cameraTransform.forward);
			gizmoSolidComponent.SetInt("_IsLit", 0);
			int @int = gizmoSolidComponent.GetInt("_CullMode");
			gizmoSolidComponent.SetInt("_CullMode", 0);
			Mesh xysquareMesh = SingletonBase<MeshPool>.Instance.XYSquareMesh;
			for (int i = 0; i < 3; i++)
			{
				if (this.IsMultiAxisSquareVisible(i))
				{
					Color multiAxisSquareColor = this.GetMultiAxisSquareColor((MultiAxisSquare)i, this._selectedMultiAxisSquare == (MultiAxisSquare)i);
					gizmoSolidComponent.SetColor("_Color", multiAxisSquareColor);
					gizmoSolidComponent.SetPass(0);
					Graphics.DrawMeshNow(xysquareMesh, worldTransformMatrices[i]);
				}
			}
			gizmoSolidComponent.SetInt("_CullMode", @int);
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x00149A84 File Offset: 0x00147C84
		private Matrix4x4[] GetMultiAxisSquaresWorldTransforms()
		{
			Matrix4x4[] array = new Matrix4x4[3];
			Vector3[] multiAxisSquaresGizmoLocalPositions = this.GetMultiAxisSquaresGizmoLocalPositions();
			Quaternion[] multiAxisSquaresGizmoLocalRotations = this.GetMultiAxisSquaresGizmoLocalRotations();
			float d = base.CalculateGizmoScale();
			for (int i = 0; i < 3; i++)
			{
				Vector3 pos = this._gizmoTransform.position + this._gizmoTransform.rotation * multiAxisSquaresGizmoLocalPositions[i] * d;
				Quaternion q = this._gizmoTransform.rotation * multiAxisSquaresGizmoLocalRotations[i];
				array[i] = default(Matrix4x4);
				array[i].SetTRS(pos, q, Vector3.Scale(this._gizmoTransform.lossyScale, new Vector3(this._multiAxisSquareSize, this._multiAxisSquareSize, 1f)));
			}
			return array;
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x00149B54 File Offset: 0x00147D54
		private Quaternion[] GetMultiAxisSquaresGizmoLocalRotations()
		{
			return new Quaternion[]
			{
				Quaternion.identity,
				Quaternion.Euler(90f, 0f, 0f),
				Quaternion.Euler(0f, 90f, 0f)
			};
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x00149BAC File Offset: 0x00147DAC
		private Vector3[] GetMultiAxisSquaresGizmoLocalPositions()
		{
			float d = this._multiAxisSquareSize * 0.5f;
			float[] multiAxisExtensionSigns = base.GetMultiAxisExtensionSigns(this._adjustMultiAxisForBetterVisibility);
			return new Vector3[]
			{
				(Vector3.right * multiAxisExtensionSigns[0] + Vector3.up * multiAxisExtensionSigns[1]) * d,
				(Vector3.right * multiAxisExtensionSigns[0] + Vector3.forward * multiAxisExtensionSigns[2]) * d,
				(Vector3.up * multiAxisExtensionSigns[1] + Vector3.forward * multiAxisExtensionSigns[2]) * d
			};
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x00149C60 File Offset: 0x00147E60
		private Vector3[] GetWorldAxesUsedToDrawMultiAxisSquareLines()
		{
			float[] multiAxisExtensionSigns = base.GetMultiAxisExtensionSigns(this._adjustMultiAxisForBetterVisibility);
			return new Vector3[]
			{
				this._gizmoTransform.right * multiAxisExtensionSigns[0],
				this._gizmoTransform.up * multiAxisExtensionSigns[1],
				this._gizmoTransform.right * multiAxisExtensionSigns[0],
				this._gizmoTransform.forward * multiAxisExtensionSigns[2],
				this._gizmoTransform.up * multiAxisExtensionSigns[1],
				this._gizmoTransform.forward * multiAxisExtensionSigns[2]
			};
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x00149D1C File Offset: 0x00147F1C
		private Plane GetPlaneFromSelectedMultiAxisSquare()
		{
			switch (this._selectedMultiAxisSquare)
			{
			case MultiAxisSquare.XY:
				return new Plane(this._gizmoTransform.forward, this._gizmoTransform.position);
			case MultiAxisSquare.XZ:
				return new Plane(this._gizmoTransform.up, this._gizmoTransform.position);
			case MultiAxisSquare.YZ:
				return new Plane(this._gizmoTransform.right, this._gizmoTransform.position);
			default:
				return default(Plane);
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x00149DA1 File Offset: 0x00147FA1
		private Plane GetCameraAxesTranslationSquarePlane()
		{
			return new Plane(this._cameraTransform.forward, this._gizmoTransform.position);
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x00149DC0 File Offset: 0x00147FC0
		private void TranslateControlledObjects(Vector3 translationVector)
		{
			if (base.ControlledObjects != null)
			{
				List<GameObject> parentsFromControlledObjects = base.GetParentsFromControlledObjects(true);
				bool flag = !this._isCameraAxesTranslationSquareSelected && !this.IsSurfacePlacementShActive;
				if (parentsFromControlledObjects.Count != 0)
				{
					foreach (GameObject gameObject in parentsFromControlledObjects)
					{
						if (gameObject != null)
						{
							Vector3 b = translationVector;
							if (flag && this._objAxisMask.ContainsKey(gameObject))
							{
								bool[] array = this._objAxisMask[gameObject];
								for (int i = 0; i < 3; i++)
								{
									if (!array[i])
									{
										b[i] = 0f;
									}
								}
							}
							gameObject.transform.position += b;
							IRTEditorEventListener component = gameObject.GetComponent<IRTEditorEventListener>();
							if (component != null)
							{
								component.OnAlteredByTransformGizmo(this);
							}
							this._objectsWereTransformedSinceLeftMouseButtonWasPressed = true;
						}
					}
				}
			}
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x00149EC4 File Offset: 0x001480C4
		private void DrawArrowCones(Matrix4x4[] worldTransformMatrices)
		{
			Material gizmoSolidComponent = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
			gizmoSolidComponent.SetInt("_ZTest", 0);
			gizmoSolidComponent.SetInt("_ZWrite", 1);
			gizmoSolidComponent.SetVector("_LightDir", this._cameraTransform.forward);
			gizmoSolidComponent.SetInt("_IsLit", this._areArrowConesLit ? 1 : 0);
			gizmoSolidComponent.SetFloat("_LightIntensity", 1.5f);
			Mesh coneMesh = SingletonBase<MeshPool>.Instance.ConeMesh;
			for (int i = 0; i < 3; i++)
			{
				if (this._axesVisibilityMask[i])
				{
					Color value = (i == (int)this._selectedAxis) ? this._selectedAxisColor : this._axesColors[i];
					gizmoSolidComponent.SetInt("_StencilRefValue", this._axesStencilRefValues[i]);
					gizmoSolidComponent.SetColor("_Color", value);
					gizmoSolidComponent.SetPass(0);
					Graphics.DrawMeshNow(coneMesh, worldTransformMatrices[i]);
				}
			}
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x00149FAC File Offset: 0x001481AC
		private Matrix4x4[] GetArrowConesWorldTransforms()
		{
			Matrix4x4[] array = new Matrix4x4[3];
			Vector3[] arrowConesGizmoLocalPositions = this.GetArrowConesGizmoLocalPositions();
			Quaternion[] arrowConesGizmoLocalRotations = this.GetArrowConesGizmoLocalRotations();
			float d = base.CalculateGizmoScale();
			for (int i = 0; i < 3; i++)
			{
				Vector3 pos = this._gizmoTransform.position + this._gizmoTransform.rotation * arrowConesGizmoLocalPositions[i] * d;
				Quaternion q = this._gizmoTransform.rotation * arrowConesGizmoLocalRotations[i];
				array[i] = default(Matrix4x4);
				array[i].SetTRS(pos, q, Vector3.Scale(Vector3.one * d, new Vector3(this._arrowConeRadius, this._arrowConeLength, this._arrowConeRadius)));
			}
			return array;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x0014A07C File Offset: 0x0014827C
		private Vector3[] GetArrowConesGizmoLocalPositions()
		{
			return new Vector3[]
			{
				Vector3.right * this._axisLength,
				Vector3.up * this._axisLength,
				Vector3.forward * this._axisLength
			};
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x0014A0D4 File Offset: 0x001482D4
		private Quaternion[] GetArrowConesGizmoLocalRotations()
		{
			return new Quaternion[]
			{
				Quaternion.Euler(0f, 0f, -90f),
				Quaternion.identity,
				Quaternion.Euler(90f, 0f, 0f)
			};
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x0014A12C File Offset: 0x0014832C
		private Color GetMultiAxisSquareColor(MultiAxisSquare multiAxisSquare, bool isSelected)
		{
			if (multiAxisSquare == MultiAxisSquare.XY)
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._multiAxisSquareAlpha);
				}
				return new Color(this._axesColors[2].r, this._axesColors[2].g, this._axesColors[2].b, this._multiAxisSquareAlpha);
			}
			else if (multiAxisSquare == MultiAxisSquare.XZ)
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._multiAxisSquareAlpha);
				}
				return new Color(this._axesColors[1].r, this._axesColors[1].g, this._axesColors[1].b, this._multiAxisSquareAlpha);
			}
			else
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._multiAxisSquareAlpha);
				}
				return new Color(this._axesColors[0].r, this._axesColors[0].g, this._axesColors[0].b, this._multiAxisSquareAlpha);
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x0014A28C File Offset: 0x0014848C
		private Color GetMultiAxisSquareLineColor(MultiAxisSquare multiAxisSquare, bool isSelected)
		{
			if (multiAxisSquare == MultiAxisSquare.XY)
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._selectedAxisColor.a);
				}
				return new Color(this._axesColors[2].r, this._axesColors[2].g, this._axesColors[2].b, this._axesColors[2].a);
			}
			else if (multiAxisSquare == MultiAxisSquare.XZ)
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._selectedAxisColor.a);
				}
				return new Color(this._axesColors[1].r, this._axesColors[1].g, this._axesColors[1].b, this._axesColors[1].a);
			}
			else
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._selectedAxisColor.a);
				}
				return new Color(this._axesColors[0].r, this._axesColors[0].g, this._axesColors[0].b, this._axesColors[0].a);
			}
		}

		// Token: 0x04001F79 RID: 8057
		[SerializeField]
		private ShortcutKeys _translateAlongScreenAxesShortcut = new ShortcutKeys("Translate along screen axes", 0)
		{
			LShift = true,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F7A RID: 8058
		[SerializeField]
		private ShortcutKeys _enableStepSnappingShortcut = new ShortcutKeys("Enable step snapping", 0)
		{
			LCtrl = true,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F7B RID: 8059
		[SerializeField]
		private ShortcutKeys _enableVertexSnappingShortcut = new ShortcutKeys("Enable vertex snapping", 1)
		{
			Key0 = KeyCode.V,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F7C RID: 8060
		[SerializeField]
		private ShortcutKeys _enableSurfacePlacementWithYAlignment = new ShortcutKeys("Enable surface placement (with Y axis alignment)", 1)
		{
			Key0 = KeyCode.Space,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F7D RID: 8061
		[SerializeField]
		private ShortcutKeys _enableSurfacePlacementWithXAlignment = new ShortcutKeys("Enable surface placement (with X axis alignment)", 2)
		{
			Key0 = KeyCode.Space,
			Key1 = KeyCode.X,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F7E RID: 8062
		[SerializeField]
		private ShortcutKeys _enableSurfacePlacementWithZAlignment = new ShortcutKeys("Enable surface placement (with Z axis alignment)", 2)
		{
			Key0 = KeyCode.Space,
			Key1 = KeyCode.Z,
			UseModifiers = false,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F7F RID: 8063
		[SerializeField]
		private ShortcutKeys _enableSurfacePlacementNoAxisAlignment = new ShortcutKeys("Enable surface placement (no alignment", 1)
		{
			Key0 = KeyCode.Space,
			LCtrl = true,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F80 RID: 8064
		private bool _isVertexSnapping;

		// Token: 0x04001F81 RID: 8065
		[SerializeField]
		private float _axisLength = 5f;

		// Token: 0x04001F82 RID: 8066
		[SerializeField]
		private float _arrowConeRadius = 0.4f;

		// Token: 0x04001F83 RID: 8067
		[SerializeField]
		private float _arrowConeLength = 1.19f;

		// Token: 0x04001F84 RID: 8068
		[SerializeField]
		private float _multiAxisSquareSize = 1f;

		// Token: 0x04001F85 RID: 8069
		[SerializeField]
		private bool _adjustMultiAxisForBetterVisibility = true;

		// Token: 0x04001F86 RID: 8070
		[SerializeField]
		private float _multiAxisSquareAlpha = 0.2f;

		// Token: 0x04001F87 RID: 8071
		[SerializeField]
		private bool _areArrowConesLit = true;

		// Token: 0x04001F88 RID: 8072
		private Dictionary<GameObject, Vector3> _objectOffsetsFromGizmo = new Dictionary<GameObject, Vector3>();

		// Token: 0x04001F89 RID: 8073
		[SerializeField]
		private Color _specialOpSquareColor = Color.white;

		// Token: 0x04001F8A RID: 8074
		[SerializeField]
		private Color _specialOpSquareColorWhenSelected = Color.yellow;

		// Token: 0x04001F8B RID: 8075
		private bool _isSpecialOpSquareSelected;

		// Token: 0x04001F8C RID: 8076
		[SerializeField]
		private float _screenSizeOfSpecialOpSquare = 25f;

		// Token: 0x04001F8D RID: 8077
		private TranslationGizmoSnapSettings _snapSettings = new TranslationGizmoSnapSettings();

		// Token: 0x04001F8E RID: 8078
		private Vector3 _accumulatedTranslation;

		// Token: 0x04001F8F RID: 8079
		private MultiAxisSquare _selectedMultiAxisSquare = MultiAxisSquare.None;

		// Token: 0x04001F90 RID: 8080
		private bool _isCameraAxesTranslationSquareSelected;
	}
}
