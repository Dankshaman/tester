using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000413 RID: 1043
	public class VolumeScaleGizmo : Gizmo
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600305D RID: 12381 RVA: 0x0014A995 File Offset: 0x00148B95
		// (set) Token: 0x0600305E RID: 12382 RVA: 0x0014A99D File Offset: 0x00148B9D
		public Color LineColor
		{
			get
			{
				return this._lineColor;
			}
			set
			{
				this._lineColor = value;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x0014A9A6 File Offset: 0x00148BA6
		// (set) Token: 0x06003060 RID: 12384 RVA: 0x0014A9AE File Offset: 0x00148BAE
		public int DragHandleSizeInPixels
		{
			get
			{
				return this._dragHandleSizeInPixels;
			}
			set
			{
				this._dragHandleSizeInPixels = Mathf.Clamp(value, 2, 50);
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x0014A9BF File Offset: 0x00148BBF
		// (set) Token: 0x06003062 RID: 12386 RVA: 0x0014A9C7 File Offset: 0x00148BC7
		public float SnapStepInWorldUnits
		{
			get
			{
				return this._snapStepInWorldUnits;
			}
			set
			{
				this._snapStepInWorldUnits = Mathf.Max(value, 0.1f);
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x0014A9DA File Offset: 0x00148BDA
		public ShortcutKeys EnableScaleFromCenterShortcut
		{
			get
			{
				return this._enableScaleFromCenterShortcut;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06003064 RID: 12388 RVA: 0x0014A9E2 File Offset: 0x00148BE2
		public ShortcutKeys EnableStepSnappingShortcut
		{
			get
			{
				return this._enableStepSnappingShortcut;
			}
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x0014A9EA File Offset: 0x00148BEA
		public override GizmoType GetGizmoType()
		{
			return GizmoType.VolumeScale;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x0014A9ED File Offset: 0x00148BED
		public override bool IsReadyForObjectManipulation()
		{
			return this.DetectHoveredComponents(false);
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x0014A9F6 File Offset: 0x00148BF6
		public void RefreshTargets()
		{
			this._targetObject = this.GetTargetObject();
			this.UpdateTargetOOBB();
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x0014AA0C File Offset: 0x00148C0C
		protected override bool DetectHoveredComponents(bool updateCompStates)
		{
			if (updateCompStates && !this._isDragging)
			{
				this._hoveredDragHandle = this.GetIndexOfHoveredDragHandle();
				int num = this._hoveredDragHandle / 2;
				if (num == 0)
				{
					this._selectedAxis = GizmoAxis.X;
				}
				else if (num == 1)
				{
					this._selectedAxis = GizmoAxis.Y;
				}
				else if (num == 2)
				{
					this._selectedAxis = GizmoAxis.Z;
				}
				else
				{
					this._selectedAxis = GizmoAxis.None;
				}
				return this._hoveredDragHandle != -1;
			}
			return this.GetIndexOfHoveredDragHandle() != -1;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x0014AA7F File Offset: 0x00148C7F
		protected override void Update()
		{
			base.Update();
			this._targetObject = this.GetTargetObject();
			this.UpdateTargetOOBB();
			this.UpdateDragHandles();
			this.DetectHoveredComponents(true);
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x0014AAA8 File Offset: 0x00148CA8
		protected override void OnInputDeviceFirstButtonDown()
		{
			base.OnInputDeviceFirstButtonDown();
			if (this._hoveredDragHandle != -1 && this._targetOOBB != null && this._targetObject != null)
			{
				this._dragStartData.DragHandleFace = (BoxFace)this._hoveredDragHandle;
				this._dragStartData.TargetOOBB = new OrientedBox(this._targetOOBB);
				this._dragStartData.TargetObjectScale = this._targetObject.transform.lossyScale;
				this._dragStartData.ScaleFromCenter = this._enableScaleFromCenterShortcut.IsActive();
				this._dragStartData.DragHandlePlane = this._dragStartData.TargetOOBB.GetBoxFacePlane(this._dragStartData.DragHandleFace);
				this._dragStartData.ScaleAxisIndex = -1;
				if (this._dragStartData.DragHandleFace == BoxFace.Left || this._dragStartData.DragHandleFace == BoxFace.Right)
				{
					this._dragStartData.ScaleAxisIndex = 0;
				}
				else if (this._dragStartData.DragHandleFace == BoxFace.Bottom || this._dragStartData.DragHandleFace == BoxFace.Top)
				{
					this._dragStartData.ScaleAxisIndex = 1;
				}
				else
				{
					this._dragStartData.ScaleAxisIndex = 2;
				}
				if (this._dragStartData.ScaleFromCenter)
				{
					this._dragStartData.ScalePivot = this._dragStartData.TargetOOBB.Center;
				}
				else
				{
					this._dragStartData.ScalePivot = this._dragStartData.TargetOOBB.GetBoxFaceCenter(BoxFaces.GetOpposite(this._dragStartData.DragHandleFace));
				}
				this._dragStartData.PivotPlane = new Plane(this._dragStartData.DragHandlePlane.normal, this._dragStartData.ScalePivot);
				this._dragStartData.DragPlane = this.CalculateDragPlane();
				this._dragStartData.FromPivotToObjectPos = this._targetObject.transform.position - this._dragStartData.ScalePivot;
			}
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x0014AC89 File Offset: 0x00148E89
		protected override void OnInputDeviceFirstButtonUp()
		{
			base.OnInputDeviceFirstButtonUp();
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x0014AC94 File Offset: 0x00148E94
		protected override void OnInputDeviceMoved()
		{
			base.OnInputDeviceMoved();
			if (this._isDragging && this._targetObject != null && this._targetOOBB != null)
			{
				Vector3 lossyScale = this._targetObject.transform.lossyScale;
				Camera camera = MonoSingletonBase<EditorCamera>.Instance.Camera;
				int scaleAxisIndex = this._dragStartData.ScaleAxisIndex;
				Ray ray;
				if (!MonoSingletonBase<InputDevice>.Instance.GetPickRay(camera, out ray))
				{
					return;
				}
				float distance;
				if (this._dragStartData.DragPlane.Raycast(ray, out distance))
				{
					Vector3 point = ray.GetPoint(distance);
					float distanceToPoint = this._dragStartData.PivotPlane.GetDistanceToPoint(point);
					float num = this._dragStartData.TargetOOBB.ScaledSize[this._dragStartData.ScaleAxisIndex];
					float num2 = Mathf.Abs((!this._dragStartData.ScaleFromCenter) ? distanceToPoint : (2f * distanceToPoint));
					if (this._enableStepSnappingShortcut.IsActive())
					{
						num2 = (float)((int)(num2 / this._snapStepInWorldUnits)) * this._snapStepInWorldUnits;
					}
					float num3 = (distanceToPoint < 0f) ? (-Mathf.Sign(this._dragStartData.TargetObjectScale[scaleAxisIndex])) : Mathf.Sign(this._dragStartData.TargetObjectScale[scaleAxisIndex]);
					float num4 = num2 / num;
					lossyScale[scaleAxisIndex] = this._dragStartData.TargetObjectScale[scaleAxisIndex] * num4;
					if (Mathf.Sign(lossyScale[scaleAxisIndex]) != num3)
					{
						ref Vector3 ptr = ref lossyScale;
						int index = scaleAxisIndex;
						ptr[index] *= -1f;
					}
					this._targetObject.SetAbsoluteScale(lossyScale);
					Transform transform = this._targetObject.transform;
					Vector3 one = Vector3.one;
					one[scaleAxisIndex] = num4 * ((distanceToPoint < 0f) ? -1f : 1f);
					float d = Vector3.Dot(transform.right, this._dragStartData.FromPivotToObjectPos);
					float d2 = Vector3.Dot(transform.up, this._dragStartData.FromPivotToObjectPos);
					float d3 = Vector3.Dot(transform.forward, this._dragStartData.FromPivotToObjectPos);
					this._targetObject.transform.position = this._dragStartData.ScalePivot + transform.right * d * one[0] + transform.up * d2 * one[1] + transform.forward * d3 * one[2];
					this._objectsWereTransformedSinceLeftMouseButtonWasPressed = true;
					this.UpdateTargetOOBB();
				}
			}
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x0014AF44 File Offset: 0x00149144
		protected override void OnRenderObject()
		{
			if (Camera.current != MonoSingletonBase<EditorCamera>.Instance.Camera)
			{
				return;
			}
			base.OnRenderObject();
			if (this._targetObject == null)
			{
				return;
			}
			GLPrimitives.DrawWireOOBB(this._targetOOBB, this._lineColor, SingletonBase<MaterialPool>.Instance.GLLine);
			foreach (VolumeScaleGizmo.DragHandle dragHandle in this._dragHandles)
			{
				if (dragHandle.IsVisible && this._axesVisibilityMask[(int)dragHandle.Axis])
				{
					Color rectangleColor = (this._hoveredDragHandle >= 0 && dragHandle.VolumeFace == (BoxFace)this._hoveredDragHandle) ? this._selectedAxisColor : this._axesColors[(int)dragHandle.Axis];
					GLPrimitives.Draw2DFilledRectangle(dragHandle.ScreenRect, rectangleColor, SingletonBase<MaterialPool>.Instance.Geometry2D, MonoSingletonBase<EditorCamera>.Instance.Camera);
				}
			}
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x0014B020 File Offset: 0x00149220
		private GameObject GetTargetObject()
		{
			if (base.ControlledObjects == null)
			{
				return null;
			}
			List<GameObject> list = new List<GameObject>(base.ControlledObjects);
			if (list.Count != 1)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x0014B058 File Offset: 0x00149258
		private void UpdateTargetOOBB()
		{
			this._targetOOBB = null;
			if (this._targetObject != null)
			{
				ObjectSelectionBoxRenderSettings objectSelectionBoxRenderSettings = MonoSingletonBase<EditorObjectSelection>.Instance.ObjectSelectionSettings.ObjectSelectionBoxRenderSettings;
				if (objectSelectionBoxRenderSettings.SelectionBoxRenderMode == ObjectSelectionBoxRenderMode.PerObject)
				{
					this._targetOOBB = this._targetObject.GetWorldOrientedBox();
				}
				else if (objectSelectionBoxRenderSettings.SelectionBoxRenderMode == ObjectSelectionBoxRenderMode.FromParentToBottom)
				{
					Box hierarchyModelSpaceBox = this._targetObject.GetHierarchyModelSpaceBox();
					this._targetOOBB = new OrientedBox(hierarchyModelSpaceBox, this._targetObject.transform);
				}
				this._targetOOBB.Scale = this._targetOOBB.Scale.GetVectorWithAbsComponents();
			}
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x0014B0EC File Offset: 0x001492EC
		private void UpdateDragHandles()
		{
			if (this._targetOOBB == null)
			{
				return;
			}
			float num = (float)this._dragHandleSizeInPixels * 0.5f;
			Camera camera = MonoSingletonBase<EditorCamera>.Instance.Camera;
			Plane nearPlane = camera.GetNearPlane();
			foreach (BoxFace boxFace in BoxFaces.GetAll())
			{
				Vector3 boxFaceCenter = this._targetOOBB.GetBoxFaceCenter(boxFace);
				Vector2 vector = camera.WorldToScreenPoint(boxFaceCenter);
				this._dragHandles[(int)boxFace].ScreenRect = new Rect(vector.x - num, vector.y - num, (float)this._dragHandleSizeInPixels, (float)this._dragHandleSizeInPixels);
				this._dragHandles[(int)boxFace].IsVisible = true;
				if (nearPlane.GetDistanceToPoint(boxFaceCenter) < 0f)
				{
					this._dragHandles[(int)boxFace].IsVisible = false;
				}
			}
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x0014B1F4 File Offset: 0x001493F4
		private int GetIndexOfHoveredDragHandle()
		{
			Vector2 v;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out v))
			{
				return -1;
			}
			foreach (BoxFace boxFace in BoxFaces.GetAll())
			{
				VolumeScaleGizmo.DragHandle dragHandle = this._dragHandles[(int)boxFace];
				if (this._axesVisibilityMask[(int)dragHandle.Axis] && dragHandle.ScreenRect.Contains(v, true))
				{
					return (int)boxFace;
				}
			}
			return -1;
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x0014B28C File Offset: 0x0014948C
		private Plane CalculateDragPlane()
		{
			if (this._targetOOBB == null)
			{
				return default(Plane);
			}
			Matrix4x4 transformMatrix = this._targetOOBB.TransformMatrix;
			Vector3 center = this._targetOOBB.Center;
			Camera camera = MonoSingletonBase<EditorCamera>.Instance.Camera;
			Vector3 inNormal = Vector3.zero;
			if (this._dragStartData.DragHandleFace == BoxFace.Front || this._dragStartData.DragHandleFace == BoxFace.Back)
			{
				inNormal = Vector3Extensions.GetMostAlignedVector(new List<Vector3>
				{
					transformMatrix.GetAxis(1)
				}, camera.transform.forward);
			}
			else if (this._dragStartData.DragHandleFace == BoxFace.Left || this._dragStartData.DragHandleFace == BoxFace.Right)
			{
				inNormal = Vector3Extensions.GetMostAlignedVector(new List<Vector3>
				{
					transformMatrix.GetAxis(1)
				}, camera.transform.forward);
			}
			else
			{
				inNormal = Vector3Extensions.GetMostAlignedVector(new List<Vector3>
				{
					transformMatrix.GetAxis(2),
					transformMatrix.GetAxis(0)
				}, camera.transform.forward);
			}
			return new Plane(inNormal, center);
		}

		// Token: 0x04001F96 RID: 8086
		[SerializeField]
		private ShortcutKeys _enableScaleFromCenterShortcut = new ShortcutKeys("Scale from center", 0)
		{
			LShift = true,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F97 RID: 8087
		[SerializeField]
		private ShortcutKeys _enableStepSnappingShortcut = new ShortcutKeys("Enable step snapping", 0)
		{
			LCtrl = true,
			UseMouseButtons = false,
			UseStrictModifierCheck = true
		};

		// Token: 0x04001F98 RID: 8088
		[SerializeField]
		private Color _lineColor = new Color(1f, 1f, 1f, 0.211f);

		// Token: 0x04001F99 RID: 8089
		[SerializeField]
		private int _dragHandleSizeInPixels = 8;

		// Token: 0x04001F9A RID: 8090
		[SerializeField]
		private float _snapStepInWorldUnits = 1f;

		// Token: 0x04001F9B RID: 8091
		private VolumeScaleGizmo.DragHandle[] _dragHandles = new VolumeScaleGizmo.DragHandle[]
		{
			new VolumeScaleGizmo.DragHandle
			{
				Axis = GizmoAxis.Z,
				VolumeFace = BoxFace.Front
			},
			new VolumeScaleGizmo.DragHandle
			{
				Axis = GizmoAxis.Z,
				VolumeFace = BoxFace.Back
			},
			new VolumeScaleGizmo.DragHandle
			{
				Axis = GizmoAxis.Y,
				VolumeFace = BoxFace.Top
			},
			new VolumeScaleGizmo.DragHandle
			{
				Axis = GizmoAxis.Y,
				VolumeFace = BoxFace.Bottom
			},
			new VolumeScaleGizmo.DragHandle
			{
				Axis = GizmoAxis.X,
				VolumeFace = BoxFace.Left
			},
			new VolumeScaleGizmo.DragHandle
			{
				Axis = GizmoAxis.X,
				VolumeFace = BoxFace.Right
			}
		};

		// Token: 0x04001F9C RID: 8092
		private int _hoveredDragHandle = -1;

		// Token: 0x04001F9D RID: 8093
		private GameObject _targetObject;

		// Token: 0x04001F9E RID: 8094
		private OrientedBox _targetOOBB;

		// Token: 0x04001F9F RID: 8095
		private VolumeScaleGizmo.DragStartData _dragStartData;

		// Token: 0x02000804 RID: 2052
		private struct DragHandle
		{
			// Token: 0x04002DFF RID: 11775
			public Rect ScreenRect;

			// Token: 0x04002E00 RID: 11776
			public BoxFace VolumeFace;

			// Token: 0x04002E01 RID: 11777
			public GizmoAxis Axis;

			// Token: 0x04002E02 RID: 11778
			public bool IsVisible;
		}

		// Token: 0x02000805 RID: 2053
		private struct DragStartData
		{
			// Token: 0x04002E03 RID: 11779
			public BoxFace DragHandleFace;

			// Token: 0x04002E04 RID: 11780
			public Plane DragHandlePlane;

			// Token: 0x04002E05 RID: 11781
			public OrientedBox TargetOOBB;

			// Token: 0x04002E06 RID: 11782
			public Vector3 TargetObjectScale;

			// Token: 0x04002E07 RID: 11783
			public Vector3 ScalePivot;

			// Token: 0x04002E08 RID: 11784
			public Plane PivotPlane;

			// Token: 0x04002E09 RID: 11785
			public Vector3 FromPivotToObjectPos;

			// Token: 0x04002E0A RID: 11786
			public bool ScaleFromCenter;

			// Token: 0x04002E0B RID: 11787
			public int ScaleAxisIndex;

			// Token: 0x04002E0C RID: 11788
			public Plane DragPlane;
		}
	}
}
