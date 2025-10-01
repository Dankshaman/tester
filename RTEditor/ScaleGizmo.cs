using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000406 RID: 1030
	public class ScaleGizmo : Gizmo
	{
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06002F6F RID: 12143 RVA: 0x00143F72 File Offset: 0x00142172
		private bool IsStepSnappingShActive
		{
			get
			{
				return this._enableStepSnappingShortcut.IsActive();
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06002F70 RID: 12144 RVA: 0x00143F7F File Offset: 0x0014217F
		private bool IsScaleAlongAllAxesShActive
		{
			get
			{
				return this._enableScaleAlongAllAxesShortcut.IsActive();
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002F71 RID: 12145 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinAxisLength
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002F72 RID: 12146 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinScaleBoxSize
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002F73 RID: 12147 RVA: 0x00143F8C File Offset: 0x0014218C
		public static float MinScreenSizeOfAllAxesSquare
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002F74 RID: 12148 RVA: 0x0013DECE File Offset: 0x0013C0CE
		public static float MinMultiAxisTriangleSideLength
		{
			get
			{
				return 0.001f;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002F75 RID: 12149 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinObjectsLocalAxesLength
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06002F76 RID: 12150 RVA: 0x00143F93 File Offset: 0x00142193
		public ShortcutKeys EnableStepSnappingShortcut
		{
			get
			{
				return this._enableStepSnappingShortcut;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002F77 RID: 12151 RVA: 0x00143F9B File Offset: 0x0014219B
		public ShortcutKeys EnableScaleAlongAllAxesShortcut
		{
			get
			{
				return this._enableScaleAlongAllAxesShortcut;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06002F78 RID: 12152 RVA: 0x00143FA3 File Offset: 0x001421A3
		// (set) Token: 0x06002F79 RID: 12153 RVA: 0x00143FAB File Offset: 0x001421AB
		public float AxisLength
		{
			get
			{
				return this._axisLength;
			}
			set
			{
				this._axisLength = Mathf.Max(ScaleGizmo.MinAxisLength, value);
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x00143FBE File Offset: 0x001421BE
		// (set) Token: 0x06002F7B RID: 12155 RVA: 0x00143FC6 File Offset: 0x001421C6
		public float ScaleBoxWidth
		{
			get
			{
				return this._scaleBoxWidth;
			}
			set
			{
				this._scaleBoxWidth = Mathf.Max(value, ScaleGizmo.MinScaleBoxSize);
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x00143FD9 File Offset: 0x001421D9
		// (set) Token: 0x06002F7D RID: 12157 RVA: 0x00143FE1 File Offset: 0x001421E1
		public float ScaleBoxHeight
		{
			get
			{
				return this._scaleBoxHeight;
			}
			set
			{
				this._scaleBoxHeight = Mathf.Max(value, ScaleGizmo.MinScaleBoxSize);
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002F7E RID: 12158 RVA: 0x00143FF4 File Offset: 0x001421F4
		// (set) Token: 0x06002F7F RID: 12159 RVA: 0x00143FFC File Offset: 0x001421FC
		public float ScaleBoxDepth
		{
			get
			{
				return this._scaleBoxDepth;
			}
			set
			{
				this._scaleBoxDepth = Mathf.Max(value, ScaleGizmo.MinScaleBoxSize);
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002F80 RID: 12160 RVA: 0x0014400F File Offset: 0x0014220F
		// (set) Token: 0x06002F81 RID: 12161 RVA: 0x00144017 File Offset: 0x00142217
		public bool AreScaleBoxesLit
		{
			get
			{
				return this._areScaleBoxesLit;
			}
			set
			{
				this._areScaleBoxesLit = value;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002F82 RID: 12162 RVA: 0x00144020 File Offset: 0x00142220
		// (set) Token: 0x06002F83 RID: 12163 RVA: 0x00144028 File Offset: 0x00142228
		public float ScreenSizeOfAllAxesSquare
		{
			get
			{
				return this._screenSizeOfAllAxesSquare;
			}
			set
			{
				this._screenSizeOfAllAxesSquare = Mathf.Max(ScaleGizmo.MinScreenSizeOfAllAxesSquare, value);
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06002F84 RID: 12164 RVA: 0x0014403B File Offset: 0x0014223B
		// (set) Token: 0x06002F85 RID: 12165 RVA: 0x00144043 File Offset: 0x00142243
		public Color ColorOfAllAxesSquareLines
		{
			get
			{
				return this._colorOfAllAxesSquareLines;
			}
			set
			{
				this._colorOfAllAxesSquareLines = value;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002F86 RID: 12166 RVA: 0x0014404C File Offset: 0x0014224C
		// (set) Token: 0x06002F87 RID: 12167 RVA: 0x00144054 File Offset: 0x00142254
		public Color ColorOfAllAxesSquareLinesWhenSelected
		{
			get
			{
				return this._colorOfAllAxesSquareLinesWhenSelected;
			}
			set
			{
				this._colorOfAllAxesSquareLinesWhenSelected = value;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002F88 RID: 12168 RVA: 0x0014405D File Offset: 0x0014225D
		// (set) Token: 0x06002F89 RID: 12169 RVA: 0x00144065 File Offset: 0x00142265
		public bool AdjustAllAxesScaleSquareWhileScalingObjects
		{
			get
			{
				return this._adjustAllAxesScaleSquareWhileScalingObjects;
			}
			set
			{
				this._adjustAllAxesScaleSquareWhileScalingObjects = value;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x0014406E File Offset: 0x0014226E
		// (set) Token: 0x06002F8B RID: 12171 RVA: 0x00144076 File Offset: 0x00142276
		public bool AdjustAxisLengthWhileScalingObjects
		{
			get
			{
				return this._adjustAxisLengthWhileScalingObjects;
			}
			set
			{
				this._adjustAxisLengthWhileScalingObjects = value;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x0014407F File Offset: 0x0014227F
		// (set) Token: 0x06002F8D RID: 12173 RVA: 0x00144087 File Offset: 0x00142287
		public bool AdjustMultiAxisTrianglesWhileScalingObjects
		{
			get
			{
				return this._adjustMultiAxisTrianglesWhileScalingObjects;
			}
			set
			{
				this._adjustMultiAxisTrianglesWhileScalingObjects = value;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x00144090 File Offset: 0x00142290
		// (set) Token: 0x06002F8F RID: 12175 RVA: 0x00144098 File Offset: 0x00142298
		public float MultiAxisTriangleSideLength
		{
			get
			{
				return this._multiAxisTriangleSideLength;
			}
			set
			{
				this._multiAxisTriangleSideLength = Mathf.Max(ScaleGizmo.MinMultiAxisTriangleSideLength, value);
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x001440AB File Offset: 0x001422AB
		// (set) Token: 0x06002F91 RID: 12177 RVA: 0x001440B3 File Offset: 0x001422B3
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

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002F92 RID: 12178 RVA: 0x001440BC File Offset: 0x001422BC
		// (set) Token: 0x06002F93 RID: 12179 RVA: 0x001440C4 File Offset: 0x001422C4
		public bool DrawObjectsLocalAxesWhileScaling
		{
			get
			{
				return this._drawObjectsLocalAxesWhileScaling;
			}
			set
			{
				this._drawObjectsLocalAxesWhileScaling = value;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002F94 RID: 12180 RVA: 0x001440CD File Offset: 0x001422CD
		// (set) Token: 0x06002F95 RID: 12181 RVA: 0x001440D5 File Offset: 0x001422D5
		public float ObjectsLocalAxesLength
		{
			get
			{
				return this._objectsLocalAxesLength;
			}
			set
			{
				this._objectsLocalAxesLength = Mathf.Max(ScaleGizmo.MinObjectsLocalAxesLength, value);
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002F96 RID: 12182 RVA: 0x001440E8 File Offset: 0x001422E8
		// (set) Token: 0x06002F97 RID: 12183 RVA: 0x001440F0 File Offset: 0x001422F0
		public bool PreserveObjectLocalAxesScreenSize
		{
			get
			{
				return this._preserveObjectLocalAxesScreenSize;
			}
			set
			{
				this._preserveObjectLocalAxesScreenSize = value;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002F98 RID: 12184 RVA: 0x001440F9 File Offset: 0x001422F9
		// (set) Token: 0x06002F99 RID: 12185 RVA: 0x00144101 File Offset: 0x00142301
		public bool AdjustObjectLocalAxesWhileScalingObjects
		{
			get
			{
				return this._adjustObjectLocalAxesWhileScalingObjects;
			}
			set
			{
				this._adjustObjectLocalAxesWhileScalingObjects = value;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002F9A RID: 12186 RVA: 0x0014410A File Offset: 0x0014230A
		public ScaleGizmoSnapSettings SnapSettings
		{
			get
			{
				return this._snapSettings;
			}
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x00144112 File Offset: 0x00142312
		public override bool IsReadyForObjectManipulation()
		{
			return this._selectedAxis != GizmoAxis.None || this._selectedMultiAxisTriangle != MultiAxisTriangle.None || this._isAllAxesSquareSelected || this.DetectHoveredComponents(false);
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00024D16 File Offset: 0x00022F16
		public override GizmoType GetGizmoType()
		{
			return GizmoType.Scale;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00144137 File Offset: 0x00142337
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x001427C4 File Offset: 0x001409C4
		protected override void Update()
		{
			base.Update();
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0))
			{
				return;
			}
			this.DetectHoveredComponents(true);
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x00144140 File Offset: 0x00142340
		protected override void OnRenderObject()
		{
			if (Camera.current != MonoSingletonBase<EditorCamera>.Instance.Camera)
			{
				return;
			}
			base.OnRenderObject();
			Matrix4x4[] scaleBoxesWorldTransforms = this.GetScaleBoxesWorldTransforms();
			this.DrawScaleBoxes(scaleBoxesWorldTransforms);
			Matrix4x4[] multiAxisTrianglesWorldTransforms = this.GetMultiAxisTrianglesWorldTransforms();
			if (!this.IsScaleAlongAllAxesShActive)
			{
				this.DrawMultiAxisTriangles(multiAxisTrianglesWorldTransforms);
			}
			this.DrawAxesLines();
			SingletonBase<MaterialPool>.Instance.GizmoLine.SetInt("_StencilRefValue", this._doNotUseStencil);
			this.DrawMultiAxisTrianglesLines();
			this.DrawAllAxesSquareLines();
			this.DrawObjectsLocalAxesDuringScaleOperation();
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x001441C0 File Offset: 0x001423C0
		protected override void OnInputDeviceFirstButtonDown()
		{
			base.OnInputDeviceFirstButtonDown();
			if (MonoSingletonBase<InputDevice>.Instance.UsingMobile)
			{
				this.DetectHoveredComponents(true);
			}
			Ray ray;
			if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray) && (this._selectedAxis != GizmoAxis.None || this._selectedMultiAxisTriangle != MultiAxisTriangle.None))
			{
				Plane plane;
				if (this._selectedAxis != GizmoAxis.None)
				{
					plane = base.GetCoordinateSystemPlaneFromSelectedAxis();
				}
				else
				{
					plane = this.GetMultiAxisTrianglePlane(this._selectedMultiAxisTriangle);
				}
				float d;
				if (plane.Raycast(ray, out d))
				{
					this._lastGizmoPickPoint = ray.origin + ray.direction * d;
				}
			}
			if (this.IsReadyForObjectManipulation())
			{
				foreach (GameObject gameObject in base.ControlledObjects)
				{
					this._gameObjectLocalScaleSnapshot.Add(gameObject, gameObject.transform.localScale);
				}
			}
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x001442B4 File Offset: 0x001424B4
		protected override void OnInputDeviceFirstButtonUp()
		{
			base.OnInputDeviceFirstButtonUp();
			this._accumulatedScaleAxisDrag = 0f;
			this._accumulatedMultiAxisTriangleDrag = 0f;
			this._accumulatedAllAxesSquareDragInScreenUnits = 0f;
			this._accumulatedAllAxesSquareDragInWorldUnits = 0f;
			this._gameObjectLocalScaleSnapshot.Clear();
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x001442F4 File Offset: 0x001424F4
		protected override void OnInputDeviceMoved()
		{
			base.OnInputDeviceMoved();
			if (!base.CanAnyControlledObjectBeManipulated())
			{
				return;
			}
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0))
			{
				if (this._selectedAxis != GizmoAxis.None)
				{
					Vector3 rhs;
					if (this._selectedAxis == GizmoAxis.X)
					{
						rhs = this._gizmoTransform.right;
					}
					else if (this._selectedAxis == GizmoAxis.Y)
					{
						rhs = this._gizmoTransform.up;
					}
					else
					{
						rhs = this._gizmoTransform.forward;
					}
					Plane coordinateSystemPlaneFromSelectedAxis = base.GetCoordinateSystemPlaneFromSelectedAxis();
					Ray ray;
					float d;
					if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray) && coordinateSystemPlaneFromSelectedAxis.Raycast(ray, out d))
					{
						Vector3 vector = ray.origin + ray.direction * d;
						float num = Vector3.Dot(vector - this._lastGizmoPickPoint, rhs);
						this._accumulatedScaleAxisDrag += num;
						bool[] array = new bool[3];
						array[(int)this._selectedAxis] = true;
						this.ScaleControlledObjects(array);
						this._lastGizmoPickPoint = vector;
						return;
					}
				}
				else if (this._selectedMultiAxisTriangle != MultiAxisTriangle.None)
				{
					Plane multiAxisTrianglePlane = this.GetMultiAxisTrianglePlane(this._selectedMultiAxisTriangle);
					Ray ray2;
					float d2;
					if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray2) && multiAxisTrianglePlane.Raycast(ray2, out d2))
					{
						Vector3 vector2 = ray2.origin + ray2.direction * d2;
						Vector3 rhs2 = vector2 - this._lastGizmoPickPoint;
						Vector3 multiAxisTriangleMedianVector = this.GetMultiAxisTriangleMedianVector(this._selectedMultiAxisTriangle);
						multiAxisTriangleMedianVector.Normalize();
						float num2 = Vector3.Dot(multiAxisTriangleMedianVector, rhs2);
						this._accumulatedMultiAxisTriangleDrag += num2;
						this.ScaleControlledObjects(this.GetScaleAxisBooleanFlagsForMultiAxisTriangle(this._selectedMultiAxisTriangle));
						this._lastGizmoPickPoint = vector2;
						return;
					}
				}
				else if (this.IsScaleAlongAllAxesShActive && this._isAllAxesSquareSelected)
				{
					Vector2 a;
					if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out a))
					{
						return;
					}
					Vector2 b = this._camera.WorldToScreenPoint(this._gizmoTransform.position);
					Vector2 vector3 = a - b;
					vector3.Normalize();
					float num3 = Mathf.Sign(Vector2.Dot(vector3, MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0)));
					if (vector3.y < 0f)
					{
						num3 *= -1f;
					}
					this._accumulatedAllAxesSquareDragInScreenUnits += MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0).magnitude * num3 * 0.45f;
					this.ScaleControlledObjects(new bool[]
					{
						true,
						true,
						true
					});
					Plane plane = new Plane(this._cameraTransform.forward, this._gizmoTransform.position);
					Ray ray3;
					float d3;
					if (MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray3) && plane.Raycast(ray3, out d3))
					{
						Vector3 b2 = ray3.origin + ray3.direction * d3;
						this._accumulatedAllAxesSquareDragInWorldUnits = (this._gizmoTransform.position - b2).magnitude;
						this._accumulatedAllAxesSquareDragInWorldUnits *= Mathf.Sign(this._accumulatedAllAxesSquareDragInScreenUnits);
					}
				}
			}
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x00144604 File Offset: 0x00142804
		protected override bool DetectHoveredComponents(bool updateCompStates)
		{
			if (updateCompStates)
			{
				this._selectedAxis = GizmoAxis.None;
				this._selectedMultiAxisTriangle = MultiAxisTriangle.None;
				this._isAllAxesSquareSelected = false;
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
							Matrix4x4[] scaleBoxesWorldTransforms = this.GetScaleBoxesWorldTransforms();
							if (!flag && ray.IntersectsBox(1f, 1f, 1f, scaleBoxesWorldTransforms[i], out d))
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
					if (!this.IsScaleAlongAllAxesShActive)
					{
						for (int j = 0; j < 3; j++)
						{
							if (this.IsMultiAxisTriangleVisible(j))
							{
								Vector3[] multiAxisTriangleWorldSpacePoints = this.GetMultiAxisTriangleWorldSpacePoints(j);
								Matrix4x4[] multiAxisTrianglesWorldTransforms = this.GetMultiAxisTrianglesWorldTransforms();
								this.ReorderTriangleVertsForClockwiseWindingOrder(multiAxisTriangleWorldSpacePoints, multiAxisTrianglesWorldTransforms[j]);
								Plane plane = new Plane(multiAxisTriangleWorldSpacePoints[0], multiAxisTriangleWorldSpacePoints[1], multiAxisTriangleWorldSpacePoints[2]);
								float d;
								if (plane.Raycast(ray, out d))
								{
									Vector3 vector = ray.origin + ray.direction * d;
									if (vector.IsInsideTriangle(multiAxisTriangleWorldSpacePoints))
									{
										float magnitude3 = (vector - position).magnitude;
										if (magnitude3 < num)
										{
											num = magnitude3;
											this._selectedMultiAxisTriangle = (MultiAxisTriangle)j;
											this._selectedAxis = GizmoAxis.None;
										}
									}
								}
							}
						}
					}
				}
				if (this.IsScaleAlongAllAxesShActive && this.IsMouseCursorInsideAllAxesScaleSquare())
				{
					this._isAllAxesSquareSelected = true;
					this._selectedAxis = GizmoAxis.None;
					this._selectedMultiAxisTriangle = MultiAxisTriangle.None;
				}
				return this._selectedAxis != GizmoAxis.None || this._selectedMultiAxisTriangle != MultiAxisTriangle.None || this._isAllAxesSquareSelected;
			}
			else
			{
				if (this._camera == null)
				{
					return false;
				}
				Ray ray2;
				bool pickRay2 = MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray2);
				float num3 = base.CalculateGizmoScale();
				float cylinderRadius2 = 0.2f * num3;
				Vector3 position3 = this._gizmoTransform.position;
				if (pickRay2)
				{
					Vector3[] gizmoLocalAxes2 = base.GetGizmoLocalAxes();
					Vector3 cylinderAxisFirstPoint2 = position3;
					for (int k = 0; k < 3; k++)
					{
						if (this._axesVisibilityMask[k])
						{
							Vector3 cylinderAxisSecondPoint2 = position3 + gizmoLocalAxes2[k] * this._axisLength * num3;
							float d2;
							if (ray2.IntersectsCylinder(cylinderAxisFirstPoint2, cylinderAxisSecondPoint2, cylinderRadius2, out d2))
							{
								return true;
							}
							Matrix4x4[] scaleBoxesWorldTransforms2 = this.GetScaleBoxesWorldTransforms();
							if (ray2.IntersectsBox(1f, 1f, 1f, scaleBoxesWorldTransforms2[k], out d2))
							{
								return true;
							}
						}
					}
					if (!this.IsScaleAlongAllAxesShActive)
					{
						for (int l = 0; l < 3; l++)
						{
							if (this.IsMultiAxisTriangleVisible(l))
							{
								Vector3[] multiAxisTriangleWorldSpacePoints2 = this.GetMultiAxisTriangleWorldSpacePoints(l);
								Matrix4x4[] multiAxisTrianglesWorldTransforms2 = this.GetMultiAxisTrianglesWorldTransforms();
								this.ReorderTriangleVertsForClockwiseWindingOrder(multiAxisTriangleWorldSpacePoints2, multiAxisTrianglesWorldTransforms2[l]);
								Plane plane2 = new Plane(multiAxisTriangleWorldSpacePoints2[0], multiAxisTriangleWorldSpacePoints2[1], multiAxisTriangleWorldSpacePoints2[2]);
								float d2;
								if (plane2.Raycast(ray2, out d2) && (ray2.origin + ray2.direction * d2).IsInsideTriangle(multiAxisTriangleWorldSpacePoints2))
								{
									return true;
								}
							}
						}
					}
				}
				return this.IsScaleAlongAllAxesShActive && this.IsMouseCursorInsideAllAxesScaleSquare();
			}
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x00144A24 File Offset: 0x00142C24
		private Vector2[] GetAllAxesScaleSquareScreenPoints()
		{
			float allAxesScaleSquareScaleFactorDuringScaleOperation = this.GetAllAxesScaleSquareScaleFactorDuringScaleOperation();
			Vector2 a = this._camera.WorldToScreenPoint(this._gizmoTransform.position);
			float d = this._screenSizeOfAllAxesSquare * 0.5f * allAxesScaleSquareScaleFactorDuringScaleOperation;
			return new Vector2[]
			{
				a - (Vector2.right - Vector2.up) * d,
				a + (Vector2.right + Vector2.up) * d,
				a + (Vector2.right - Vector2.up) * d,
				a - (Vector2.right + Vector2.up) * d
			};
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x00144AF4 File Offset: 0x00142CF4
		private bool IsMouseCursorInsideAllAxesScaleSquare()
		{
			Vector2 b = this._camera.WorldToScreenPoint(this._gizmoTransform.position);
			float num = this._screenSizeOfAllAxesSquare * 0.5f;
			Vector2 a;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPosition(out a))
			{
				return false;
			}
			Vector2 vector = a - b;
			return Mathf.Abs(vector.x) <= num && Mathf.Abs(vector.y) <= num;
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x00144B64 File Offset: 0x00142D64
		private bool[] GetScaleAxisBooleanFlagsForMultiAxisTriangle(MultiAxisTriangle multiAxisTriangle)
		{
			switch (multiAxisTriangle)
			{
			case MultiAxisTriangle.XY:
			{
				bool[] array = new bool[3];
				array[0] = true;
				array[1] = true;
				return array;
			}
			case MultiAxisTriangle.XZ:
				return new bool[]
				{
					true,
					default(bool),
					true
				};
			case MultiAxisTriangle.YZ:
				return new bool[]
				{
					default(bool),
					true,
					true
				};
			default:
				return null;
			}
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x00144BB4 File Offset: 0x00142DB4
		private Vector3 GetMultiAxisTriangleMedianVector(MultiAxisTriangle multiAxisTriangle)
		{
			Vector3[] worldAxesMultipliedByMultiAxisExtensionSigns = this.GetWorldAxesMultipliedByMultiAxisExtensionSigns();
			float multiAxisTriangleSideLength = this.GetMultiAxisTriangleSideLength();
			Vector3 a = worldAxesMultipliedByMultiAxisExtensionSigns[0];
			Vector3 vector = worldAxesMultipliedByMultiAxisExtensionSigns[1];
			Vector3 vector2 = worldAxesMultipliedByMultiAxisExtensionSigns[2];
			switch (multiAxisTriangle)
			{
			case MultiAxisTriangle.XY:
			{
				Vector3 a2 = (a - vector) * multiAxisTriangleSideLength;
				float magnitude = a2.magnitude;
				a2.Normalize();
				return vector * multiAxisTriangleSideLength + a2 * magnitude * 0.5f;
			}
			case MultiAxisTriangle.XZ:
			{
				Vector3 a2 = (a - vector2) * multiAxisTriangleSideLength;
				float magnitude = a2.magnitude;
				a2.Normalize();
				return vector2 * multiAxisTriangleSideLength + a2 * magnitude * 0.5f;
			}
			case MultiAxisTriangle.YZ:
			{
				Vector3 a2 = (vector2 - vector) * multiAxisTriangleSideLength;
				float magnitude = a2.magnitude;
				a2.Normalize();
				return vector * multiAxisTriangleSideLength + a2 * magnitude * 0.5f;
			}
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x00144CBB File Offset: 0x00142EBB
		private float GetMultiAxisTriangleSideLength()
		{
			return base.CalculateGizmoScale() * this._multiAxisTriangleSideLength;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x00144CCC File Offset: 0x00142ECC
		private Plane GetMultiAxisTrianglePlane(MultiAxisTriangle multiAxisTriangle)
		{
			switch (multiAxisTriangle)
			{
			case MultiAxisTriangle.XY:
				return new Plane(this._gizmoTransform.forward, this._gizmoTransform.position);
			case MultiAxisTriangle.XZ:
				return new Plane(this._gizmoTransform.up, this._gizmoTransform.position);
			case MultiAxisTriangle.YZ:
				return new Plane(this._gizmoTransform.right, this._gizmoTransform.position);
			default:
				return default(Plane);
			}
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x00144D4C File Offset: 0x00142F4C
		private void ReorderTriangleVertsForClockwiseWindingOrder(Vector3[] worldSpaceTriangleVerts, Matrix4x4 triangleWorldTransform)
		{
			Matrix4x4 inverse = triangleWorldTransform.inverse;
			Vector3[] array = worldSpaceTriangleVerts.Clone() as Vector3[];
			for (int i = 0; i < 3; i++)
			{
				array[i] = inverse.MultiplyPoint(worldSpaceTriangleVerts[i]);
			}
			Vector3 rhs = array[1] - array[0];
			if (Vector3.Cross(array[2] - array[0], rhs).z < 0f)
			{
				Vector3 vector = worldSpaceTriangleVerts[1];
				worldSpaceTriangleVerts[1] = worldSpaceTriangleVerts[2];
				worldSpaceTriangleVerts[2] = vector;
			}
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x00144DE8 File Offset: 0x00142FE8
		private Vector3[] GetMultiAxisTriangleWorldSpacePoints(int multiAxisTriangleIndex)
		{
			Vector3[] worldAxesUsedToDrawMultiAxisTriangleLines = this.GetWorldAxesUsedToDrawMultiAxisTriangleLines();
			float d = base.CalculateGizmoScale();
			int num = multiAxisTriangleIndex * 2;
			return new Vector3[]
			{
				this._gizmoTransform.position,
				this._gizmoTransform.position + worldAxesUsedToDrawMultiAxisTriangleLines[num + 1] * this._multiAxisTriangleSideLength * d,
				this._gizmoTransform.position + worldAxesUsedToDrawMultiAxisTriangleLines[num] * this._multiAxisTriangleSideLength * d
			};
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x00144E80 File Offset: 0x00143080
		private void DrawAxesLines()
		{
			int[] sortedGizmoAxesIndices = base.GetSortedGizmoAxesIndices();
			float gizmoScale = base.CalculateGizmoScale();
			Vector3[] gizmoLocalAxes = base.GetGizmoLocalAxes();
			Vector3 position = this._gizmoTransform.position;
			foreach (int num in sortedGizmoAxesIndices)
			{
				if (this._axesVisibilityMask[num])
				{
					Vector3 vector = position + gizmoLocalAxes[num] * this.GetAxisLength(num, gizmoScale);
					base.UpdateShaderStencilRefValuesForGizmoAxisLineDraw(num, position, vector, gizmoScale);
					GLPrimitives.Draw3DLine(position, vector, (this._selectedAxis == (GizmoAxis)num) ? this._selectedAxisColor : this._axesColors[num], SingletonBase<MaterialPool>.Instance.GizmoLine);
				}
			}
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x00144F2C File Offset: 0x0014312C
		private bool IsMultiAxisTriangleVisible(int multiAxisIndex)
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

		// Token: 0x06002FAE RID: 12206 RVA: 0x00144F88 File Offset: 0x00143188
		private float GetAxisLength(int axisIndex, float gizmoScale)
		{
			if (!this._adjustAxisLengthWhileScalingObjects || (axisIndex != (int)this._selectedAxis && !this.IsGizmoAxisSharedBySelectedMultiAxisTriangle(axisIndex) && !this._isAllAxesSquareSelected))
			{
				return this._axisLength * gizmoScale;
			}
			if (this._selectedAxis != GizmoAxis.None)
			{
				return this._axisLength * gizmoScale * this.GetAxisScaleFactorForAccumulatedDrag(this._selectedAxis);
			}
			if (this._selectedMultiAxisTriangle != MultiAxisTriangle.None)
			{
				return this._axisLength * gizmoScale * this.GetMultiAxisTriangleScaleFactorForAccumulatedDrag(this._selectedMultiAxisTriangle);
			}
			return this._axisLength * gizmoScale * this.GetAllAxesSquareScaleFactorForAccumulatedDrag();
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x0014500D File Offset: 0x0014320D
		private bool IsGizmoAxisSharedBySelectedMultiAxisTriangle(int axisIndex)
		{
			if (this._selectedMultiAxisTriangle == MultiAxisTriangle.None)
			{
				return false;
			}
			if (this._selectedMultiAxisTriangle == MultiAxisTriangle.XY)
			{
				return axisIndex == 0 || axisIndex == 1;
			}
			if (this._selectedMultiAxisTriangle == MultiAxisTriangle.XZ)
			{
				return axisIndex == 0 || axisIndex == 2;
			}
			return axisIndex == 1 || axisIndex == 2;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x0014504C File Offset: 0x0014324C
		private void DrawScaleBoxes(Matrix4x4[] worldTransforms)
		{
			Material gizmoSolidComponent = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
			gizmoSolidComponent.SetInt("_ZWrite", 1);
			gizmoSolidComponent.SetInt("_ZTest", 0);
			gizmoSolidComponent.SetInt("_IsLit", this._areScaleBoxesLit ? 1 : 0);
			gizmoSolidComponent.SetFloat("_LightIntensity", 1.5f);
			gizmoSolidComponent.SetVector("_LightDir", this._cameraTransform.forward);
			Mesh boxMesh = SingletonBase<MeshPool>.Instance.BoxMesh;
			for (int i = 0; i < 3; i++)
			{
				if (this._axesVisibilityMask[i])
				{
					Color value = (i == (int)this._selectedAxis) ? this._selectedAxisColor : this._axesColors[i];
					gizmoSolidComponent.SetColor("_Color", value);
					gizmoSolidComponent.SetInt("_StencilRefValue", this._axesStencilRefValues[i]);
					gizmoSolidComponent.SetPass(0);
					Graphics.DrawMeshNow(boxMesh, worldTransforms[i]);
				}
			}
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x00145134 File Offset: 0x00143334
		private Matrix4x4[] GetScaleBoxesWorldTransforms()
		{
			Matrix4x4[] array = new Matrix4x4[3];
			Vector3[] scaleBoxesGizmoLocalPositions = this.GetScaleBoxesGizmoLocalPositions(base.CalculateGizmoScale());
			Quaternion[] scaleBoxesGizmoLocalRotations = this.GetScaleBoxesGizmoLocalRotations();
			for (int i = 0; i < 3; i++)
			{
				Vector3 pos = this._gizmoTransform.position + this._gizmoTransform.rotation * scaleBoxesGizmoLocalPositions[i];
				Quaternion q = this._gizmoTransform.rotation * scaleBoxesGizmoLocalRotations[i];
				array[i] = default(Matrix4x4);
				array[i].SetTRS(pos, q, Vector3.Scale(this._gizmoTransform.lossyScale, new Vector3(this._scaleBoxWidth, this._scaleBoxHeight, this._scaleBoxDepth)));
			}
			return array;
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x001451F8 File Offset: 0x001433F8
		private Quaternion[] GetScaleBoxesGizmoLocalRotations()
		{
			return new Quaternion[]
			{
				Quaternion.identity,
				Quaternion.Euler(0f, 0f, 90f),
				Quaternion.Euler(0f, 90f, 0f)
			};
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x00145250 File Offset: 0x00143450
		private Vector3[] GetScaleBoxesGizmoLocalPositions(float gizmoScale)
		{
			float num = 0.5f * this._scaleBoxWidth * gizmoScale;
			return new Vector3[]
			{
				Vector3.right * (this.GetAxisLength(0, gizmoScale) + num),
				Vector3.up * (this.GetAxisLength(1, gizmoScale) + num),
				Vector3.forward * (this.GetAxisLength(2, gizmoScale) + num)
			};
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x001452C4 File Offset: 0x001434C4
		private void DrawMultiAxisTriangles(Matrix4x4[] worldTransforms)
		{
			Material gizmoSolidComponent = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
			gizmoSolidComponent.SetInt("_ZWrite", 1);
			gizmoSolidComponent.SetInt("_IsLit", 0);
			gizmoSolidComponent.SetInt("_ZTest", 0);
			gizmoSolidComponent.SetVector("_LightDir", this._cameraTransform.forward);
			int @int = gizmoSolidComponent.GetInt("_CullMode");
			gizmoSolidComponent.SetInt("_CullMode", 0);
			Mesh rightAngledTriangleMesh = SingletonBase<MeshPool>.Instance.RightAngledTriangleMesh;
			for (int i = 0; i < 3; i++)
			{
				if (this.IsMultiAxisTriangleVisible(i))
				{
					Color multiAxisTriangleColor = this.GetMultiAxisTriangleColor((MultiAxisTriangle)i, i == (int)this._selectedMultiAxisTriangle);
					gizmoSolidComponent.SetColor("_Color", multiAxisTriangleColor);
					gizmoSolidComponent.SetPass(0);
					Graphics.DrawMeshNow(rightAngledTriangleMesh, worldTransforms[i]);
				}
			}
			gizmoSolidComponent.SetInt("_CullMode", @int);
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x00145394 File Offset: 0x00143594
		private Matrix4x4[] GetMultiAxisTrianglesWorldTransforms()
		{
			Matrix4x4[] array = new Matrix4x4[3];
			Vector3[] multiAxisTrianglesGizmoLocalScales = this.GetMultiAxisTrianglesGizmoLocalScales();
			Quaternion[] multiAxisTrianglesGizmoLocalRotations = this.GetMultiAxisTrianglesGizmoLocalRotations();
			for (int i = 0; i < 3; i++)
			{
				float multiAxisTriangleScaleFactorDuringScaleOperation = this.GetMultiAxisTriangleScaleFactorDuringScaleOperation((MultiAxisTriangle)i);
				Vector3 s = Vector3.Scale(this._gizmoTransform.lossyScale, Vector3.Scale(multiAxisTrianglesGizmoLocalScales[i], new Vector3(this._multiAxisTriangleSideLength, this._multiAxisTriangleSideLength, 1f))) * multiAxisTriangleScaleFactorDuringScaleOperation;
				Quaternion q = this._gizmoTransform.rotation * multiAxisTrianglesGizmoLocalRotations[i];
				array[i] = default(Matrix4x4);
				array[i].SetTRS(this._gizmoTransform.position, q, s);
			}
			return array;
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x0014544E File Offset: 0x0014364E
		private float GetMultiAxisTriangleScaleFactorDuringScaleOperation(MultiAxisTriangle multiAxisTriangle)
		{
			if (!this._adjustMultiAxisTrianglesWhileScalingObjects || multiAxisTriangle != this._selectedMultiAxisTriangle)
			{
				return 1f;
			}
			return this.GetMultiAxisTriangleScaleFactorForAccumulatedDrag(multiAxisTriangle);
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x0014546E File Offset: 0x0014366E
		private float GetAllAxesScaleSquareScaleFactorDuringScaleOperation()
		{
			if (!this._adjustAllAxesScaleSquareWhileScalingObjects)
			{
				return 1f;
			}
			return this.GetAllAxesSquareScaleFactorForAccumulatedDrag();
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x00145484 File Offset: 0x00143684
		private Quaternion[] GetMultiAxisTrianglesGizmoLocalRotations()
		{
			return new Quaternion[]
			{
				Quaternion.identity,
				Quaternion.Euler(90f, 0f, 0f),
				Quaternion.Euler(0f, -90f, 0f)
			};
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x001454DC File Offset: 0x001436DC
		private Vector3[] GetMultiAxisTrianglesGizmoLocalScales()
		{
			float[] multiAxisExtensionSigns = base.GetMultiAxisExtensionSigns(this._adjustMultiAxisForBetterVisibility);
			return new Vector3[]
			{
				new Vector3(1f * multiAxisExtensionSigns[0], 1f * multiAxisExtensionSigns[1], 1f),
				new Vector3(1f * multiAxisExtensionSigns[0], 1f * multiAxisExtensionSigns[2], 1f),
				new Vector3(1f * multiAxisExtensionSigns[2], 1f * multiAxisExtensionSigns[1], 1f)
			};
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x00145568 File Offset: 0x00143768
		private void DrawMultiAxisTrianglesLines()
		{
			if (!this.IsScaleAlongAllAxesShActive)
			{
				Vector3[] linePoints;
				Color[] lineColors;
				this.GetMultiAxisTrianglesLinePointsAndColors(out linePoints, out lineColors);
				GLPrimitives.Draw3DLines(linePoints, lineColors, false, SingletonBase<MaterialPool>.Instance.GizmoLine, false, Color.black);
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x001455A0 File Offset: 0x001437A0
		private void GetMultiAxisTrianglesLinePointsAndColors(out Vector3[] triangleLinesPoints, out Color[] triangleLinesColors)
		{
			float num = base.CalculateGizmoScale();
			float d = (this._multiAxisTriangleSideLength + 0.001f) * num;
			triangleLinesPoints = new Vector3[18];
			triangleLinesColors = new Color[9];
			Vector3 position = this._gizmoTransform.position;
			Vector3[] worldAxesUsedToDrawMultiAxisTriangleLines = this.GetWorldAxesUsedToDrawMultiAxisTriangleLines();
			for (int i = 0; i < 3; i++)
			{
				Color multiAxisTriangleLineColor = this.GetMultiAxisTriangleLineColor((MultiAxisTriangle)i, i == (int)this._selectedMultiAxisTriangle);
				if (!this.IsMultiAxisTriangleVisible(i))
				{
					multiAxisTriangleLineColor.a = 0f;
				}
				int num2 = i * 3;
				triangleLinesColors[num2] = multiAxisTriangleLineColor;
				triangleLinesColors[num2 + 1] = multiAxisTriangleLineColor;
				triangleLinesColors[num2 + 2] = multiAxisTriangleLineColor;
				int num3 = i * 2;
				float multiAxisTriangleScaleFactorDuringScaleOperation = this.GetMultiAxisTriangleScaleFactorDuringScaleOperation((MultiAxisTriangle)i);
				Vector3 vector = this._gizmoTransform.position + worldAxesUsedToDrawMultiAxisTriangleLines[num3] * d * multiAxisTriangleScaleFactorDuringScaleOperation;
				Vector3 vector2 = this._gizmoTransform.position + worldAxesUsedToDrawMultiAxisTriangleLines[num3 + 1] * d * multiAxisTriangleScaleFactorDuringScaleOperation;
				int num4 = i * 6;
				triangleLinesPoints[num4] = position;
				triangleLinesPoints[num4 + 1] = vector;
				triangleLinesPoints[num4 + 2] = vector;
				triangleLinesPoints[num4 + 3] = vector2;
				triangleLinesPoints[num4 + 4] = vector2;
				triangleLinesPoints[num4 + 5] = position;
			}
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x00145704 File Offset: 0x00143904
		private Color GetMultiAxisTriangleColor(MultiAxisTriangle multiAxisTriangle, bool isSelected)
		{
			if (multiAxisTriangle == MultiAxisTriangle.XY)
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._multiAxisTriangleAlpha);
				}
				return new Color(this._axesColors[2].r, this._axesColors[2].g, this._axesColors[2].b, this._multiAxisTriangleAlpha);
			}
			else if (multiAxisTriangle == MultiAxisTriangle.XZ)
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._multiAxisTriangleAlpha);
				}
				return new Color(this._axesColors[1].r, this._axesColors[1].g, this._axesColors[1].b, this._multiAxisTriangleAlpha);
			}
			else
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._multiAxisTriangleAlpha);
				}
				return new Color(this._axesColors[0].r, this._axesColors[0].g, this._axesColors[0].b, this._multiAxisTriangleAlpha);
			}
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x00145864 File Offset: 0x00143A64
		private Color GetMultiAxisTriangleLineColor(MultiAxisTriangle multiAxisTriangle, bool isSelected)
		{
			if (multiAxisTriangle == MultiAxisTriangle.XY)
			{
				if (isSelected)
				{
					return new Color(this._selectedAxisColor.r, this._selectedAxisColor.g, this._selectedAxisColor.b, this._selectedAxisColor.a);
				}
				return new Color(this._axesColors[2].r, this._axesColors[2].g, this._axesColors[2].b, this._axesColors[2].a);
			}
			else if (multiAxisTriangle == MultiAxisTriangle.XZ)
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

		// Token: 0x06002FBE RID: 12222 RVA: 0x001459F4 File Offset: 0x00143BF4
		private void DrawAllAxesSquareLines()
		{
			if (this.IsScaleAlongAllAxesShActive)
			{
				Color borderLineColor = this._isAllAxesSquareSelected ? this._colorOfAllAxesSquareLinesWhenSelected : this._colorOfAllAxesSquareLines;
				GLPrimitives.Draw2DRectangleBorderLines(this.GetAllAxesScaleSquareScreenPoints(), borderLineColor, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
			}
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x00145A3C File Offset: 0x00143C3C
		private void DrawObjectsLocalAxesDuringScaleOperation()
		{
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0) && this._drawObjectsLocalAxesWhileScaling && this.IsReadyForObjectManipulation() && base.ControlledObjects != null)
			{
				List<GameObject> parentsFromControlledObjects = base.GetParentsFromControlledObjects(true);
				Vector3[] linePoints;
				Color[] lineColors;
				this.GetObjectLocalAxesLinePointsAndColors(parentsFromControlledObjects, out linePoints, out lineColors);
				GLPrimitives.Draw3DLines(linePoints, lineColors, false, SingletonBase<MaterialPool>.Instance.GizmoLine, false, Color.black);
			}
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x00145A9C File Offset: 0x00143C9C
		private void GetObjectLocalAxesLinePointsAndColors(List<GameObject> gameObjects, out Vector3[] axesLinesPoints, out Color[] axesLinesColors)
		{
			float num = base.CalculateGizmoScale();
			float num2 = this._preserveObjectLocalAxesScreenSize ? num : 1f;
			float d = this._objectsLocalAxesLength * num2;
			float d2 = 1f;
			float d3 = 1f;
			float d4 = 1f;
			if (this._adjustObjectLocalAxesWhileScalingObjects)
			{
				if (this._selectedAxis == GizmoAxis.X)
				{
					d2 = this.GetAxisScaleFactorForAccumulatedDrag(GizmoAxis.X);
				}
				else if (this._selectedMultiAxisTriangle == MultiAxisTriangle.XY || this._selectedMultiAxisTriangle == MultiAxisTriangle.XZ)
				{
					d2 = this.GetMultiAxisTriangleScaleFactorForAccumulatedDrag(this._selectedMultiAxisTriangle);
				}
				else if (this._isAllAxesSquareSelected)
				{
					d2 = this.GetAllAxesSquareScaleFactorForAccumulatedDrag();
				}
				if (this._selectedAxis == GizmoAxis.Y)
				{
					d3 = this.GetAxisScaleFactorForAccumulatedDrag(GizmoAxis.Y);
				}
				else if (this._selectedMultiAxisTriangle == MultiAxisTriangle.XY || this._selectedMultiAxisTriangle == MultiAxisTriangle.YZ)
				{
					d3 = this.GetMultiAxisTriangleScaleFactorForAccumulatedDrag(this._selectedMultiAxisTriangle);
				}
				else if (this._isAllAxesSquareSelected)
				{
					d3 = this.GetAllAxesSquareScaleFactorForAccumulatedDrag();
				}
				if (this._selectedAxis == GizmoAxis.Z)
				{
					d4 = this.GetAxisScaleFactorForAccumulatedDrag(GizmoAxis.Z);
				}
				else if (this._selectedMultiAxisTriangle == MultiAxisTriangle.XZ || this._selectedMultiAxisTriangle == MultiAxisTriangle.YZ)
				{
					d4 = this.GetMultiAxisTriangleScaleFactorForAccumulatedDrag(this._selectedMultiAxisTriangle);
				}
				else if (this._isAllAxesSquareSelected)
				{
					d4 = this.GetAllAxesSquareScaleFactorForAccumulatedDrag();
				}
			}
			int num3 = gameObjects.Count * 3;
			axesLinesPoints = new Vector3[num3 * 2];
			axesLinesColors = new Color[num3];
			for (int i = 0; i < gameObjects.Count; i++)
			{
				Transform transform = gameObjects[i].transform;
				Vector3 position = transform.position;
				Vector3 right = transform.right;
				Vector3 up = transform.up;
				Vector3 forward = transform.forward;
				int num4 = i * 3;
				axesLinesColors[num4] = this._axesColors[0];
				axesLinesColors[num4 + 1] = this._axesColors[1];
				axesLinesColors[num4 + 2] = this._axesColors[2];
				int num5 = i * 6;
				axesLinesPoints[num5] = position + right * d * d2;
				axesLinesPoints[num5 + 1] = position - right * d * d2;
				axesLinesPoints[num5 + 2] = position + up * d * d3;
				axesLinesPoints[num5 + 3] = position - up * d * d3;
				axesLinesPoints[num5 + 4] = position + forward * d * d4;
				axesLinesPoints[num5 + 5] = position - forward * d * d4;
			}
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x00145D2C File Offset: 0x00143F2C
		private Vector3[] GetWorldAxesUsedToDrawMultiAxisTriangleLines()
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

		// Token: 0x06002FC2 RID: 12226 RVA: 0x00145DE8 File Offset: 0x00143FE8
		private Vector3[] GetWorldAxesMultipliedByMultiAxisExtensionSigns()
		{
			float[] multiAxisExtensionSigns = base.GetMultiAxisExtensionSigns(this._adjustMultiAxisForBetterVisibility);
			return new Vector3[]
			{
				this._gizmoTransform.right * multiAxisExtensionSigns[0],
				this._gizmoTransform.up * multiAxisExtensionSigns[1],
				this._gizmoTransform.forward * multiAxisExtensionSigns[2]
			};
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x00145E58 File Offset: 0x00144058
		private void ScaleControlledObjects(bool[] axesFlags)
		{
			if (base.ControlledObjects != null)
			{
				float scaleFactorForGameObjectGlobalScaleSnapshot = this.GetScaleFactorForGameObjectGlobalScaleSnapshot();
				Vector3 one = Vector3.one;
				for (int i = 0; i < 3; i++)
				{
					if (axesFlags[i])
					{
						one[i] = scaleFactorForGameObjectGlobalScaleSnapshot;
					}
				}
				List<GameObject> parentsFromControlledObjects = base.GetParentsFromControlledObjects(true);
				if (parentsFromControlledObjects.Count != 0)
				{
					foreach (GameObject gameObject in parentsFromControlledObjects)
					{
						if (gameObject != null)
						{
							Transform transform = gameObject.transform;
							Vector3 vector = this._gameObjectLocalScaleSnapshot[gameObject];
							vector = Vector3.Scale(vector, one);
							if (vector.x == 0f)
							{
								vector.x = 1E-05f;
							}
							if (vector.y == 0f)
							{
								vector.y = 1E-05f;
							}
							if (vector.z == 0f)
							{
								vector.z = 1E-05f;
							}
							Vector3 localScale = transform.localScale;
							if (localScale.x == 0f)
							{
								localScale.x = 1E-05f;
							}
							if (localScale.y == 0f)
							{
								localScale.y = 1E-05f;
							}
							if (localScale.z == 0f)
							{
								localScale.z = 1E-05f;
							}
							Vector3 vector2 = Vector3.Scale(vector, new Vector3(1f / localScale.x, 1f / localScale.y, 1f / localScale.z));
							if (this._objAxisMask.ContainsKey(gameObject))
							{
								bool[] array = this._objAxisMask[gameObject];
								for (int j = 0; j < 3; j++)
								{
									if (!array[j])
									{
										vector[j] = this._gameObjectLocalScaleSnapshot[gameObject][j];
										vector2[j] = 1f;
									}
								}
							}
							if (this._transformPivotPoint == TransformPivotPoint.Center)
							{
								Vector3 rhs = transform.position - this._gizmoTransform.position;
								Vector3 right = transform.right;
								Vector3 up = transform.up;
								Vector3 forward = transform.forward;
								float num = Vector3.Dot(right, rhs);
								float num2 = Vector3.Dot(up, rhs);
								float num3 = Vector3.Dot(forward, rhs);
								if (axesFlags[0])
								{
									num *= vector2.x;
								}
								if (axesFlags[1])
								{
									num2 *= vector2.y;
								}
								if (axesFlags[2])
								{
									num3 *= vector2.z;
								}
								transform.localScale = vector;
								transform.position = this._gizmoTransform.position + right * num + up * num2 + forward * num3;
							}
							else
							{
								transform.localScale = vector;
							}
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

		// Token: 0x06002FC4 RID: 12228 RVA: 0x00146168 File Offset: 0x00144368
		private float GetScaleFactorForGameObjectGlobalScaleSnapshot()
		{
			if (this._selectedAxis != GizmoAxis.None)
			{
				return this.GetAxisScaleFactorForAccumulatedDrag(this._selectedAxis);
			}
			if (this._selectedMultiAxisTriangle != MultiAxisTriangle.None)
			{
				return this.GetMultiAxisTriangleScaleFactorForAccumulatedDrag(this._selectedMultiAxisTriangle);
			}
			return this.GetAllAxesSquareScaleFactorForAccumulatedDrag();
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x0014619C File Offset: 0x0014439C
		private float GetAxisScaleFactorForAccumulatedDrag(GizmoAxis gizmoAxis)
		{
			if (!this.IsStepSnappingShActive)
			{
				float num = this._axisLength * base.CalculateGizmoScale();
				return (num + this._accumulatedScaleAxisDrag) / num;
			}
			if (Mathf.Abs(this._accumulatedScaleAxisDrag) >= this._snapSettings.StepValueInWorldUnits)
			{
				float num2 = (float)((int)Mathf.Abs(this._accumulatedScaleAxisDrag / this._snapSettings.StepValueInWorldUnits));
				float num3 = this._snapSettings.StepValueInWorldUnits * num2 * Mathf.Sign(this._accumulatedScaleAxisDrag);
				return 1f + num3;
			}
			return 1f;
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x00146224 File Offset: 0x00144424
		private float GetMultiAxisTriangleScaleFactorForAccumulatedDrag(MultiAxisTriangle multiAxisTriangle)
		{
			if (!this.IsStepSnappingShActive)
			{
				float magnitude = this.GetMultiAxisTriangleMedianVector(this._selectedMultiAxisTriangle).magnitude;
				return (magnitude + this._accumulatedMultiAxisTriangleDrag) / magnitude;
			}
			if (Mathf.Abs(this._accumulatedMultiAxisTriangleDrag) >= this._snapSettings.StepValueInWorldUnits)
			{
				float num = (float)((int)Mathf.Abs(this._accumulatedMultiAxisTriangleDrag / this._snapSettings.StepValueInWorldUnits));
				float num2 = this._snapSettings.StepValueInWorldUnits * num * Mathf.Sign(this._accumulatedMultiAxisTriangleDrag);
				return 1f + num2;
			}
			return 1f;
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x001462B4 File Offset: 0x001444B4
		private float GetAllAxesSquareScaleFactorForAccumulatedDrag()
		{
			if (!this.IsStepSnappingShActive)
			{
				return (this._screenSizeOfAllAxesSquare + this._accumulatedAllAxesSquareDragInScreenUnits) / this._screenSizeOfAllAxesSquare;
			}
			if (Mathf.Abs(this._accumulatedAllAxesSquareDragInWorldUnits) >= this._snapSettings.StepValueInWorldUnits)
			{
				float num = (float)((int)Mathf.Abs(this._accumulatedAllAxesSquareDragInWorldUnits / this._snapSettings.StepValueInWorldUnits));
				float num2 = this._snapSettings.StepValueInWorldUnits * num * Mathf.Sign(this._accumulatedAllAxesSquareDragInWorldUnits);
				return 1f + num2;
			}
			return 1f;
		}

		// Token: 0x04001F33 RID: 7987
		[SerializeField]
		private ShortcutKeys _enableStepSnappingShortcut = new ShortcutKeys("Enable step snapping", 0)
		{
			LCtrl = true,
			UseMouseButtons = false
		};

		// Token: 0x04001F34 RID: 7988
		[SerializeField]
		private ShortcutKeys _enableScaleAlongAllAxesShortcut = new ShortcutKeys("Scale along all axes", 0)
		{
			LShift = true,
			UseMouseButtons = false
		};

		// Token: 0x04001F35 RID: 7989
		[SerializeField]
		private float _axisLength = 5f;

		// Token: 0x04001F36 RID: 7990
		[SerializeField]
		private float _scaleBoxWidth = 0.5f;

		// Token: 0x04001F37 RID: 7991
		[SerializeField]
		private float _scaleBoxHeight = 0.5f;

		// Token: 0x04001F38 RID: 7992
		[SerializeField]
		private float _scaleBoxDepth = 0.5f;

		// Token: 0x04001F39 RID: 7993
		[SerializeField]
		private float _screenSizeOfAllAxesSquare = 25f;

		// Token: 0x04001F3A RID: 7994
		[SerializeField]
		private Color _colorOfAllAxesSquareLines = Color.white;

		// Token: 0x04001F3B RID: 7995
		[SerializeField]
		private Color _colorOfAllAxesSquareLinesWhenSelected = Color.yellow;

		// Token: 0x04001F3C RID: 7996
		[SerializeField]
		private bool _adjustAllAxesScaleSquareWhileScalingObjects = true;

		// Token: 0x04001F3D RID: 7997
		private const float _allAxesSquareDragUnitsPerScreenUnit = 0.45f;

		// Token: 0x04001F3E RID: 7998
		[SerializeField]
		private bool _areScaleBoxesLit = true;

		// Token: 0x04001F3F RID: 7999
		[SerializeField]
		private bool _adjustAxisLengthWhileScalingObjects = true;

		// Token: 0x04001F40 RID: 8000
		[SerializeField]
		private float _multiAxisTriangleAlpha = 0.2f;

		// Token: 0x04001F41 RID: 8001
		[SerializeField]
		private float _multiAxisTriangleSideLength = 1.3f;

		// Token: 0x04001F42 RID: 8002
		[SerializeField]
		private bool _adjustMultiAxisForBetterVisibility = true;

		// Token: 0x04001F43 RID: 8003
		private float _accumulatedScaleAxisDrag;

		// Token: 0x04001F44 RID: 8004
		private float _accumulatedMultiAxisTriangleDrag;

		// Token: 0x04001F45 RID: 8005
		private float _accumulatedAllAxesSquareDragInScreenUnits;

		// Token: 0x04001F46 RID: 8006
		private float _accumulatedAllAxesSquareDragInWorldUnits;

		// Token: 0x04001F47 RID: 8007
		[SerializeField]
		private bool _adjustMultiAxisTrianglesWhileScalingObjects = true;

		// Token: 0x04001F48 RID: 8008
		[SerializeField]
		private bool _drawObjectsLocalAxesWhileScaling = true;

		// Token: 0x04001F49 RID: 8009
		[SerializeField]
		private float _objectsLocalAxesLength = 1f;

		// Token: 0x04001F4A RID: 8010
		[SerializeField]
		private bool _preserveObjectLocalAxesScreenSize = true;

		// Token: 0x04001F4B RID: 8011
		[SerializeField]
		private bool _adjustObjectLocalAxesWhileScalingObjects;

		// Token: 0x04001F4C RID: 8012
		[SerializeField]
		private ScaleGizmoSnapSettings _snapSettings = new ScaleGizmoSnapSettings();

		// Token: 0x04001F4D RID: 8013
		private Dictionary<GameObject, Vector3> _gameObjectLocalScaleSnapshot = new Dictionary<GameObject, Vector3>();

		// Token: 0x04001F4E RID: 8014
		private MultiAxisTriangle _selectedMultiAxisTriangle = MultiAxisTriangle.None;

		// Token: 0x04001F4F RID: 8015
		private bool _isAllAxesSquareSelected;
	}
}
