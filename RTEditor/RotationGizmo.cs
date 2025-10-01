using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000404 RID: 1028
	public class RotationGizmo : Gizmo
	{
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06002F29 RID: 12073 RVA: 0x0014263D File Offset: 0x0014083D
		private bool IsStepSnappingShActive
		{
			get
			{
				return this._enableStepSnappingShortcut.IsActive();
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002F2A RID: 12074 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinCameraLookRotationCircleRadiusScale
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinRotationSphereRadius
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06002F2C RID: 12076 RVA: 0x00142651 File Offset: 0x00140851
		public ShortcutKeys EnableStepSnappingShortcut
		{
			get
			{
				return this._enableStepSnappingShortcut;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06002F2D RID: 12077 RVA: 0x00142659 File Offset: 0x00140859
		// (set) Token: 0x06002F2E RID: 12078 RVA: 0x00142661 File Offset: 0x00140861
		public float RotationSphereRadius
		{
			get
			{
				return this._rotationSphereRadius;
			}
			set
			{
				this._rotationSphereRadius = Mathf.Max(RotationGizmo.MinRotationSphereRadius, value);
				if (Application.isPlaying)
				{
					this.CalculateRotationCirclePointsInGizmoLocalSpace();
				}
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002F2F RID: 12079 RVA: 0x00142681 File Offset: 0x00140881
		// (set) Token: 0x06002F30 RID: 12080 RVA: 0x00142689 File Offset: 0x00140889
		public Color RotationSphereColor
		{
			get
			{
				return this._rotationSphereColor;
			}
			set
			{
				this._rotationSphereColor = value;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06002F31 RID: 12081 RVA: 0x00142692 File Offset: 0x00140892
		// (set) Token: 0x06002F32 RID: 12082 RVA: 0x0014269A File Offset: 0x0014089A
		public bool IsRotationSphereLit
		{
			get
			{
				return this._isRotationSphereLit;
			}
			set
			{
				this._isRotationSphereLit = value;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002F33 RID: 12083 RVA: 0x001426A3 File Offset: 0x001408A3
		// (set) Token: 0x06002F34 RID: 12084 RVA: 0x001426AB File Offset: 0x001408AB
		public bool ShowRotationGuide
		{
			get
			{
				return this._showRotationGuide;
			}
			set
			{
				this._showRotationGuide = value;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002F35 RID: 12085 RVA: 0x001426B4 File Offset: 0x001408B4
		// (set) Token: 0x06002F36 RID: 12086 RVA: 0x001426BC File Offset: 0x001408BC
		public Color RotationGuieLineColor
		{
			get
			{
				return this._rotationGuieLineColor;
			}
			set
			{
				this._rotationGuieLineColor = value;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002F37 RID: 12087 RVA: 0x001426C5 File Offset: 0x001408C5
		// (set) Token: 0x06002F38 RID: 12088 RVA: 0x001426CD File Offset: 0x001408CD
		public Color RotationGuideDiscColor
		{
			get
			{
				return this._rotationGuideDiscColor;
			}
			set
			{
				this._rotationGuideDiscColor = value;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002F39 RID: 12089 RVA: 0x001426D6 File Offset: 0x001408D6
		// (set) Token: 0x06002F3A RID: 12090 RVA: 0x001426DE File Offset: 0x001408DE
		public bool ShowRotationSphere
		{
			get
			{
				return this._showRotationSphere;
			}
			set
			{
				this._showRotationSphere = value;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002F3B RID: 12091 RVA: 0x001426E7 File Offset: 0x001408E7
		// (set) Token: 0x06002F3C RID: 12092 RVA: 0x001426EF File Offset: 0x001408EF
		public bool ShowSphereBoundary
		{
			get
			{
				return this._showRotationSphereBoundary;
			}
			set
			{
				this._showRotationSphereBoundary = value;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002F3D RID: 12093 RVA: 0x001426F8 File Offset: 0x001408F8
		// (set) Token: 0x06002F3E RID: 12094 RVA: 0x00142700 File Offset: 0x00140900
		public Color SphereBoundaryLineColor
		{
			get
			{
				return this._rotationSphereBoundaryLineColor;
			}
			set
			{
				this._rotationSphereBoundaryLineColor = value;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002F3F RID: 12095 RVA: 0x00142709 File Offset: 0x00140909
		// (set) Token: 0x06002F40 RID: 12096 RVA: 0x00142711 File Offset: 0x00140911
		public bool ShowCameraLookRotationCircle
		{
			get
			{
				return this._showCameraLookRotationCircle;
			}
			set
			{
				this._showCameraLookRotationCircle = value;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002F41 RID: 12097 RVA: 0x0014271A File Offset: 0x0014091A
		// (set) Token: 0x06002F42 RID: 12098 RVA: 0x00142722 File Offset: 0x00140922
		public float CameraLookRotationCircleRadiusScale
		{
			get
			{
				return this._cameraLookRotationCircleRadiusScale;
			}
			set
			{
				this._cameraLookRotationCircleRadiusScale = Mathf.Max(RotationGizmo.MinCameraLookRotationCircleRadiusScale, value);
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002F43 RID: 12099 RVA: 0x00142735 File Offset: 0x00140935
		// (set) Token: 0x06002F44 RID: 12100 RVA: 0x0014273D File Offset: 0x0014093D
		public Color CameraLookRotationCircleLineColor
		{
			get
			{
				return this._cameraLookRotationCircleLineColor;
			}
			set
			{
				this._cameraLookRotationCircleLineColor = value;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002F45 RID: 12101 RVA: 0x00142746 File Offset: 0x00140946
		// (set) Token: 0x06002F46 RID: 12102 RVA: 0x0014274E File Offset: 0x0014094E
		public Color CameraLookRotationCircleColorWhenSelected
		{
			get
			{
				return this._cameraLookRotationCircleColorWhenSelected;
			}
			set
			{
				this._cameraLookRotationCircleColorWhenSelected = value;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002F47 RID: 12103 RVA: 0x00142757 File Offset: 0x00140957
		public RotationGizmoSnapSettings SnapSettings
		{
			get
			{
				return this._snapSettings;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002F48 RID: 12104 RVA: 0x0014275F File Offset: 0x0014095F
		// (set) Token: 0x06002F49 RID: 12105 RVA: 0x00142767 File Offset: 0x00140967
		public bool ShowFullRotationCircleX
		{
			get
			{
				return this._showFullRotationCircleX;
			}
			set
			{
				this._showFullRotationCircleX = value;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002F4A RID: 12106 RVA: 0x00142770 File Offset: 0x00140970
		// (set) Token: 0x06002F4B RID: 12107 RVA: 0x00142778 File Offset: 0x00140978
		public bool ShowFullRotationCircleY
		{
			get
			{
				return this._showFullRotationCircleY;
			}
			set
			{
				this._showFullRotationCircleY = value;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002F4C RID: 12108 RVA: 0x00142781 File Offset: 0x00140981
		// (set) Token: 0x06002F4D RID: 12109 RVA: 0x00142789 File Offset: 0x00140989
		public bool ShowFullRotationCircleZ
		{
			get
			{
				return this._showFullRotationCircleZ;
			}
			set
			{
				this._showFullRotationCircleZ = value;
			}
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x00142792 File Offset: 0x00140992
		public override bool IsReadyForObjectManipulation()
		{
			return this._selectedAxis != GizmoAxis.None || this._isRotationSphereSelected || this._isCameraLookRotationCircleSelected || this.DetectHoveredComponents(false);
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x00014D66 File Offset: 0x00012F66
		public override GizmoType GetGizmoType()
		{
			return GizmoType.Rotation;
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x001427B6 File Offset: 0x001409B6
		protected override void Start()
		{
			base.Start();
			this.CalculateRotationCirclePointsInGizmoLocalSpace();
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x001427C4 File Offset: 0x001409C4
		protected override void Update()
		{
			base.Update();
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0))
			{
				return;
			}
			this.DetectHoveredComponents(true);
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x001427E4 File Offset: 0x001409E4
		protected override void OnInputDeviceFirstButtonDown()
		{
			base.OnInputDeviceFirstButtonDown();
			if (MonoSingletonBase<InputDevice>.Instance.UsingMobile)
			{
				this.DetectHoveredComponents(true);
			}
			if (this._selectedAxis != GizmoAxis.None)
			{
				Ray ray;
				if (!MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray))
				{
					return;
				}
				Vector3 vector;
				if (this._selectedAxis == GizmoAxis.X)
				{
					vector = this._gizmoTransform.right;
				}
				else if (this._selectedAxis == GizmoAxis.Y)
				{
					vector = this._gizmoTransform.up;
				}
				else
				{
					vector = this._gizmoTransform.forward;
				}
				Vector3 vector2;
				if (this.RayIntersectsRotationCircle(ray, vector, this.GetWorldSpaceRotationCircleRadius(), out vector2))
				{
					Plane plane = new Plane(vector, this._gizmoTransform.position);
					float distanceToPoint = plane.GetDistanceToPoint(vector2);
					vector2 -= vector * distanceToPoint;
					Vector3 a = vector2 - this._gizmoTransform.position;
					a.Normalize();
					this._rotationGuideLinePoints[0] = this._gizmoTransform.position + a * this.GetWorldSpaceRotationCircleRadius();
				}
			}
			else if (this._isCameraLookRotationCircleSelected)
			{
				this._rotationGuideLinePoints[0] = this._cameraLookRotationCirclePickPoint;
			}
			this._rotationGuideLinePoints[1] = this._rotationGuideLinePoints[0];
			this._accumulatedSnapRotation = 0f;
			this._totalAccumulatedRotation = 0f;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x00142938 File Offset: 0x00140B38
		protected override void OnInputDeviceMoved()
		{
			base.OnInputDeviceMoved();
			if (!base.CanAnyControlledObjectBeManipulated())
			{
				return;
			}
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0))
			{
				float num = 0.45f;
				if (this._selectedAxis != GizmoAxis.None)
				{
					Vector3 vector;
					if (this._selectedAxis == GizmoAxis.X)
					{
						vector = this._gizmoTransform.right;
					}
					else if (this._selectedAxis == GizmoAxis.Y)
					{
						vector = this._gizmoTransform.up;
					}
					else
					{
						vector = this._gizmoTransform.forward;
					}
					Vector3 rhs = this._rotationGuideLinePoints[0] - this._gizmoTransform.position;
					Vector3 b = Vector3.Cross(vector, rhs);
					b.Normalize();
					Vector3 vector2 = this._rotationGuideLinePoints[0];
					Vector3 vector3 = vector2 + b;
					vector2 = this._camera.WorldToScreenPoint(vector2);
					vector3 = this._camera.WorldToScreenPoint(vector3);
					Vector2 lhs = vector3 - vector2;
					lhs.Normalize();
					float num2 = Vector2.Dot(lhs, MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0));
					float num3 = num * num2;
					if (!this.IsStepSnappingShActive)
					{
						this._totalAccumulatedRotation += num3;
						Vector3 vector4 = this._rotationGuideLinePoints[1] - this._gizmoTransform.position;
						vector4 = Quaternion.AngleAxis(num3, vector) * vector4;
						vector4.Normalize();
						this._rotationGuideLinePoints[1] = this._gizmoTransform.position + vector4 * this.GetWorldSpaceRotationCircleRadius();
						this._gizmoTransform.Rotate(vector, num3, Space.World);
						this.RotateControlledObjects(vector, num3, (int)this._selectedAxis);
						return;
					}
					this._accumulatedSnapRotation += num3;
					if (Mathf.Abs(this._accumulatedSnapRotation) >= this._snapSettings.StepValueInDegrees)
					{
						float num4 = (float)((int)Mathf.Abs(this._accumulatedSnapRotation / this._snapSettings.StepValueInDegrees));
						float num5 = this._snapSettings.StepValueInDegrees * num4 * Mathf.Sign(num3);
						this._totalAccumulatedRotation += num5;
						float totalAccumulatedRotation = this._totalAccumulatedRotation;
						float num6 = (float)((int)Mathf.Abs(this._totalAccumulatedRotation / this._snapSettings.StepValueInDegrees));
						this._totalAccumulatedRotation = num6 * this._snapSettings.StepValueInDegrees * Mathf.Sign(this._totalAccumulatedRotation);
						num5 -= totalAccumulatedRotation - this._totalAccumulatedRotation;
						Vector3 vector5 = this._rotationGuideLinePoints[1] - this._gizmoTransform.position;
						vector5 = Quaternion.AngleAxis(num5, vector) * vector5;
						vector5.Normalize();
						this._rotationGuideLinePoints[1] = this._gizmoTransform.position + vector5 * this.GetWorldSpaceRotationCircleRadius();
						this._gizmoTransform.Rotate(vector, num5, Space.World);
						this.RotateControlledObjects(vector, num5, (int)this._selectedAxis);
						if (this._accumulatedSnapRotation > 0f)
						{
							this._accumulatedSnapRotation -= this._snapSettings.StepValueInDegrees * num4;
							return;
						}
						if (this._accumulatedSnapRotation < 0f)
						{
							this._accumulatedSnapRotation += this._snapSettings.StepValueInDegrees * num4;
							return;
						}
					}
				}
				else
				{
					if (this._isCameraLookRotationCircleSelected)
					{
						Vector2 rotationSphereScreenSpaceCenter = this.GetRotationSphereScreenSpaceCenter();
						Vector2 vector6 = this._cameraLookRotationCirclePickPoint - rotationSphereScreenSpaceCenter;
						Vector2 lhs2 = new Vector2(-vector6.y, vector6.x);
						lhs2.Normalize();
						Vector2 deltaSincePressed = MonoSingletonBase<InputDevice>.Instance.GetDeltaSincePressed(0);
						deltaSincePressed.Normalize();
						float num7 = Vector2.Dot(lhs2, deltaSincePressed);
						Quaternion rotation = Quaternion.AngleAxis(MonoSingletonBase<InputDevice>.Instance.GetDeltaSincePressed(0).magnitude * num * num7, Vector3.forward);
						Vector2 vector7 = vector6;
						vector7 = rotation * vector7;
						vector7.Normalize();
						Vector2 deltaSinceLastFrame = MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0);
						deltaSinceLastFrame.Normalize();
						float num8 = Vector2.Dot(lhs2, deltaSinceLastFrame);
						float num9 = MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0).magnitude * num8 * num;
						this._gizmoTransform.Rotate(this._cameraTransform.forward, num9, Space.World);
						this._rotationGuideLinePoints[1] = rotationSphereScreenSpaceCenter + vector7 * this.EstimateRotationSphereScreenSpaceBoundaryCircleRadius(rotationSphereScreenSpaceCenter) * this._cameraLookRotationCircleRadiusScale;
						this.RotateControlledObjects(this._cameraTransform.forward, num9, -1);
						return;
					}
					if (this._isRotationSphereSelected)
					{
						Vector2 lhs3 = new Vector2(1f, 0f);
						Vector2 lhs4 = new Vector2(0f, 1f);
						Vector2 deltaSinceLastFrame2 = MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0);
						deltaSinceLastFrame2.Normalize();
						float num10 = -Vector2.Dot(lhs3, deltaSinceLastFrame2);
						float num11 = Vector2.Dot(lhs4, deltaSinceLastFrame2);
						float num12 = MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0).magnitude * num11 * num;
						float num13 = MonoSingletonBase<InputDevice>.Instance.GetDeltaSinceLastFrame(0).magnitude * num10 * num;
						this._gizmoTransform.Rotate(this._cameraTransform.right, num12, Space.World);
						this._gizmoTransform.Rotate(this._cameraTransform.up, num13, Space.World);
						this.RotateControlledObjects(this._cameraTransform.right, num12, -1);
						this.RotateControlledObjects(this._cameraTransform.up, num13, -1);
					}
				}
			}
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x00142E8C File Offset: 0x0014108C
		protected override void OnRenderObject()
		{
			if (Camera.current != MonoSingletonBase<EditorCamera>.Instance.Camera)
			{
				return;
			}
			base.OnRenderObject();
			if (this._showRotationSphere)
			{
				this.DrawRotationSphere(this.GetRotationSphereWorldTransform());
			}
			if (MonoSingletonBase<InputDevice>.Instance.IsPressed(0))
			{
				if (this._selectedAxis != GizmoAxis.None && this._showRotationGuide)
				{
					GLPrimitives.Draw3DLine(this._gizmoTransform.position, this._rotationGuideLinePoints[0], this._rotationGuieLineColor, SingletonBase<MaterialPool>.Instance.GizmoLine);
					GLPrimitives.Draw3DLine(this._gizmoTransform.position, this._rotationGuideLinePoints[1], this._rotationGuieLineColor, SingletonBase<MaterialPool>.Instance.GizmoLine);
					Vector3 discPlaneNormal = this._gizmoTransform.right;
					if (this._selectedAxis == GizmoAxis.Y)
					{
						discPlaneNormal = this._gizmoTransform.up;
					}
					else if (this._selectedAxis == GizmoAxis.Z)
					{
						discPlaneNormal = this._gizmoTransform.forward;
					}
					Material gizmoSolidComponent = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
					gizmoSolidComponent.SetInt("_ZTest", 0);
					gizmoSolidComponent.SetInt("_ZWrite", 1);
					gizmoSolidComponent.SetInt("_IsLit", 0);
					int @int = gizmoSolidComponent.GetInt("_CullMode");
					gizmoSolidComponent.SetInt("_CullMode", 0);
					GLPrimitives.Draw3DFilledDisc(this._gizmoTransform.position, this._rotationGuideLinePoints[0], this._rotationGuideLinePoints[1], discPlaneNormal, this._rotationGuideDiscColor, gizmoSolidComponent);
					gizmoSolidComponent.SetInt("_CullMode", @int);
				}
				else if (this._isCameraLookRotationCircleSelected && this._showRotationGuide)
				{
					Vector2 rotationSphereScreenSpaceCenter = this.GetRotationSphereScreenSpaceCenter();
					GLPrimitives.Draw2DLine(rotationSphereScreenSpaceCenter, this._rotationGuideLinePoints[0], this._rotationGuieLineColor, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
					GLPrimitives.Draw2DLine(rotationSphereScreenSpaceCenter, this._rotationGuideLinePoints[1], this._rotationGuieLineColor, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
					Material gizmoSolidComponent2 = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
					gizmoSolidComponent2.SetInt("_IsLit", 0);
					gizmoSolidComponent2.SetInt("_ZWrite", 1);
					gizmoSolidComponent2.SetInt("_ZTest", 0);
					int int2 = gizmoSolidComponent2.GetInt("_CullMode");
					gizmoSolidComponent2.SetInt("_CullMode", 0);
					GLPrimitives.Draw2DFilledDisc(rotationSphereScreenSpaceCenter, this._rotationGuideLinePoints[0], this._rotationGuideLinePoints[1], this._rotationGuideDiscColor, gizmoSolidComponent2, this._camera);
					gizmoSolidComponent2.SetInt("_CullMode", int2);
				}
			}
			if (this._axesVisibilityMask[0])
			{
				this.DrawRotationCircle(GizmoAxis.X);
			}
			if (this._axesVisibilityMask[1])
			{
				this.DrawRotationCircle(GizmoAxis.Y);
			}
			if (this._axesVisibilityMask[2])
			{
				this.DrawRotationCircle(GizmoAxis.Z);
			}
			if ((this._showRotationSphereBoundary || this._showCameraLookRotationCircle) && this.IsGizmoVisible())
			{
				Vector3 circleCenter = this.GetRotationSphereScreenSpaceCenter();
				Vector3[] rotationSphereScreenSpaceBoundaryPoints = this.GetRotationSphereScreenSpaceBoundaryPoints();
				if (this._showRotationSphereBoundary)
				{
					GLPrimitives.Draw2DCircleBorderLines(rotationSphereScreenSpaceBoundaryPoints, circleCenter, this._rotationSphereBoundaryLineColor, 1f, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
				}
				if (this._showCameraLookRotationCircle)
				{
					Color borderLineColor = this._isCameraLookRotationCircleSelected ? this._cameraLookRotationCircleColorWhenSelected : this._cameraLookRotationCircleLineColor;
					GLPrimitives.Draw2DCircleBorderLines(rotationSphereScreenSpaceBoundaryPoints, circleCenter, borderLineColor, this._cameraLookRotationCircleRadiusScale, SingletonBase<MaterialPool>.Instance.GizmoLine, this._camera);
				}
			}
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x001431DC File Offset: 0x001413DC
		protected override bool DetectHoveredComponents(bool updateCompStates)
		{
			if (updateCompStates)
			{
				this._selectedAxis = GizmoAxis.None;
				this._isCameraLookRotationCircleSelected = false;
				this._isRotationSphereSelected = false;
				if (this._camera == null)
				{
					return false;
				}
				Ray ray = new Ray(Vector3.zero, Vector3.zero);
				bool pickRay = MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray);
				if (pickRay)
				{
					float num = float.MaxValue;
					float distanceFromCameraPositionToRotationSphereCenter = this.GetDistanceFromCameraPositionToRotationSphereCenter();
					float worldSpaceRotationCircleRadius = this.GetWorldSpaceRotationCircleRadius();
					Vector3[] gizmoLocalAxes = base.GetGizmoLocalAxes();
					for (int i = 0; i < 3; i++)
					{
						if (this._axesVisibilityMask[i])
						{
							bool flag = (i == 0 && this.ShowFullRotationCircleX) || (i == 1 && this.ShowFullRotationCircleY) || (i == 2 && this.ShowFullRotationCircleZ);
							Vector3 vector;
							if (this.RayIntersectsRotationCircle(ray, gizmoLocalAxes[i], worldSpaceRotationCircleRadius, out vector))
							{
								float magnitude = (ray.origin - vector).magnitude;
								if (magnitude < num && (flag || this.IsPointOnRotationCircleVisible(vector, distanceFromCameraPositionToRotationSphereCenter)))
								{
									num = magnitude;
									this._selectedAxis = (GizmoAxis)i;
								}
							}
						}
					}
				}
				if (this._showCameraLookRotationCircle)
				{
					Vector2 rotationSphereScreenSpaceCenter = this.GetRotationSphereScreenSpaceCenter();
					float num2 = this.EstimateRotationSphereScreenSpaceBoundaryCircleRadius(rotationSphereScreenSpaceCenter) * this._cameraLookRotationCircleRadiusScale;
					Vector2 a;
					if (MonoSingletonBase<InputDevice>.Instance.GetPosition(out a) && Mathf.Abs((a - rotationSphereScreenSpaceCenter).magnitude - num2) <= 5f)
					{
						this._selectedAxis = GizmoAxis.None;
						this._isCameraLookRotationCircleSelected = true;
						Vector2 a2 = a - rotationSphereScreenSpaceCenter;
						a2.Normalize();
						this._cameraLookRotationCirclePickPoint = rotationSphereScreenSpaceCenter + a2 * this.EstimateRotationSphereScreenSpaceBoundaryCircleRadius(rotationSphereScreenSpaceCenter) * this._cameraLookRotationCircleRadiusScale;
					}
				}
				float num3;
				if (pickRay && this._selectedAxis == GizmoAxis.None && !this._isCameraLookRotationCircleSelected && this._showRotationSphere && ray.IntersectsSphere(this._gizmoTransform.position, this.GetWorldSpaceRotationSphereRadius(), out num3))
				{
					this._isRotationSphereSelected = true;
				}
				return this._selectedAxis != GizmoAxis.None || this._isCameraLookRotationCircleSelected || this._isRotationSphereSelected;
			}
			else
			{
				if (this._camera == null)
				{
					return false;
				}
				Ray ray2;
				bool pickRay2 = MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._camera, out ray2);
				if (pickRay2)
				{
					float distanceFromCameraPositionToRotationSphereCenter2 = this.GetDistanceFromCameraPositionToRotationSphereCenter();
					float worldSpaceRotationCircleRadius2 = this.GetWorldSpaceRotationCircleRadius();
					Vector3[] gizmoLocalAxes2 = base.GetGizmoLocalAxes();
					for (int j = 0; j < 3; j++)
					{
						Vector3 point;
						if (this._axesVisibilityMask[j] && this.RayIntersectsRotationCircle(ray2, gizmoLocalAxes2[j], worldSpaceRotationCircleRadius2, out point) && this.IsPointOnRotationCircleVisible(point, distanceFromCameraPositionToRotationSphereCenter2))
						{
							return true;
						}
					}
				}
				if (this._showCameraLookRotationCircle)
				{
					Vector2 rotationSphereScreenSpaceCenter2 = this.GetRotationSphereScreenSpaceCenter();
					float num4 = this.EstimateRotationSphereScreenSpaceBoundaryCircleRadius(rotationSphereScreenSpaceCenter2) * this._cameraLookRotationCircleRadiusScale;
					Vector2 a3;
					if (MonoSingletonBase<InputDevice>.Instance.GetPosition(out a3) && Mathf.Abs((a3 - rotationSphereScreenSpaceCenter2).magnitude - num4) <= 5f)
					{
						return true;
					}
				}
				float num5;
				return pickRay2 && this._selectedAxis == GizmoAxis.None && !this._isCameraLookRotationCircleSelected && this._showRotationSphere && ray2.IntersectsSphere(this._gizmoTransform.position, this.GetWorldSpaceRotationSphereRadius(), out num5);
			}
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x00143508 File Offset: 0x00141708
		private bool IsGizmoVisible()
		{
			return !this.IsGizmoInFrontOfCameraNearClipPlane() && !this.IsGizmoInFrontOfCameraFarClipPlane();
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x00143520 File Offset: 0x00141720
		private bool IsGizmoInFrontOfCameraNearClipPlane()
		{
			Vector3 point = this._gizmoTransform.position + this._cameraTransform.forward * this.GetWorldSpaceRotationSphereRadius();
			Plane plane = new Plane(-this._cameraTransform.forward, this._cameraTransform.position + this._cameraTransform.forward * this._camera.nearClipPlane);
			return plane.GetDistanceToPoint(point) > 0f;
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x001435A8 File Offset: 0x001417A8
		private bool IsGizmoInFrontOfCameraFarClipPlane()
		{
			Vector3 point = this._gizmoTransform.position - this._cameraTransform.forward * this.GetWorldSpaceRotationSphereRadius();
			Plane plane = new Plane(this._cameraTransform.forward, this._cameraTransform.position + this._cameraTransform.forward * this._camera.farClipPlane);
			return plane.GetDistanceToPoint(point) > 0f;
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x00143628 File Offset: 0x00141828
		private Vector3[] GetRotationCirclePointInWorldSpace(GizmoAxis gizmoAxis)
		{
			Vector3[] array;
			if (gizmoAxis == GizmoAxis.X)
			{
				array = this._rotationCirclePointsForXAxisInLocalSpace;
			}
			else if (gizmoAxis == GizmoAxis.Y)
			{
				array = this._rotationCirclePointsForYAxisInLocalSpace;
			}
			else
			{
				array = this._rotationCirclePointsForZAxisInLocalSpace;
			}
			Vector3[] array2 = new Vector3[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = this._gizmoTransform.TransformPoint(array[i]);
			}
			return array2;
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x00143688 File Offset: 0x00141888
		private void DrawRotationCircle(GizmoAxis gizmoAxis)
		{
			float distanceFromCameraPositionToRotationSphereCenter = this.GetDistanceFromCameraPositionToRotationSphereCenter();
			Vector3[] rotationCirclePointInWorldSpace = this.GetRotationCirclePointInWorldSpace(gizmoAxis);
			Color[] array = new Color[rotationCirclePointInWorldSpace.Length];
			Color color = (this._selectedAxis == gizmoAxis) ? this._selectedAxisColor : this._axesColors[(int)gizmoAxis];
			bool flag = false;
			if (gizmoAxis == GizmoAxis.X && this.ShowFullRotationCircleX)
			{
				flag = true;
			}
			if (gizmoAxis == GizmoAxis.Y && this.ShowFullRotationCircleY)
			{
				flag = true;
			}
			if (gizmoAxis == GizmoAxis.Z && this.ShowFullRotationCircleZ)
			{
				flag = true;
			}
			for (int i = 0; i < rotationCirclePointInWorldSpace.Length; i++)
			{
				Vector3 point = rotationCirclePointInWorldSpace[i];
				bool flag2 = flag || this.IsPointOnRotationCircleVisible(point, distanceFromCameraPositionToRotationSphereCenter);
				array[i] = (flag2 ? color : new Color(color.r, color.g, color.b, 0f));
			}
			SingletonBase<MaterialPool>.Instance.GizmoLine.SetInt("_StencilRefValue", this._doNotUseStencil);
			GLPrimitives.Draw3DLines(rotationCirclePointInWorldSpace, array, true, SingletonBase<MaterialPool>.Instance.GizmoLine, true, array[0]);
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x00143788 File Offset: 0x00141988
		private Vector3[] GetRotationSphereScreenSpaceBoundaryPoints()
		{
			Vector3 vector = this.GetRotationSphereScreenSpaceCenter();
			float num = this.EstimateRotationSphereScreenSpaceBoundaryCircleRadius(vector);
			Vector3[] array = new Vector3[100];
			float num2 = 3.6363637f;
			for (int i = 0; i < 100; i++)
			{
				float f = num2 * (float)i * 0.017453292f;
				Vector3 vector2 = new Vector3(Mathf.Sin(f) * num, Mathf.Cos(f) * num, 0f);
				vector2 += vector;
				array[i] = vector2;
			}
			return array;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x00143808 File Offset: 0x00141A08
		private Vector2 GetRotationSphereScreenSpaceCenter()
		{
			return this._camera.WorldToScreenPoint(this._gizmoTransform.position);
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x00143828 File Offset: 0x00141A28
		private float EstimateRotationSphereScreenSpaceBoundaryCircleRadius(Vector3 screenSpaceBoundaryCircleCenter)
		{
			Vector3 vector = this._gizmoTransform.position + this._cameraTransform.up * this.GetWorldSpaceRotationSphereRadius();
			vector = this._camera.WorldToScreenPoint(vector);
			vector.z = 0f;
			return (vector - screenSpaceBoundaryCircleCenter).magnitude;
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x00143884 File Offset: 0x00141A84
		private float GetDistanceFromCameraPositionToRotationSphereCenter()
		{
			if (this._camera.orthographic)
			{
				Plane plane = new Plane(this._cameraTransform.forward, this._cameraTransform.position + this._cameraTransform.forward * this._camera.nearClipPlane);
				return Mathf.Abs(plane.GetDistanceToPoint(this._gizmoTransform.position));
			}
			return (this._cameraTransform.position - this._gizmoTransform.position).magnitude;
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x00143918 File Offset: 0x00141B18
		private bool IsPointOnRotationCircleVisible(Vector3 point, float distanceFromCameraPositionToSphereCenter)
		{
			if (this._camera.orthographic)
			{
				Plane plane = new Plane(this._cameraTransform.forward, this._cameraTransform.position + this._cameraTransform.forward * this._camera.nearClipPlane);
				return Mathf.Abs(plane.GetDistanceToPoint(point)) <= distanceFromCameraPositionToSphereCenter;
			}
			return (point - this._cameraTransform.position).magnitude <= distanceFromCameraPositionToSphereCenter;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x001439A2 File Offset: 0x00141BA2
		private void CalculateRotationCirclePointsInGizmoLocalSpace()
		{
			this._rotationCirclePointsForXAxisInLocalSpace = this.CalculateRotationCirclePointsInInGizmoLocalSpace(GizmoAxis.X);
			this._rotationCirclePointsForYAxisInLocalSpace = this.CalculateRotationCirclePointsInInGizmoLocalSpace(GizmoAxis.Y);
			this._rotationCirclePointsForZAxisInLocalSpace = this.CalculateRotationCirclePointsInInGizmoLocalSpace(GizmoAxis.Z);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x001439CC File Offset: 0x00141BCC
		private Vector3[] CalculateRotationCirclePointsInInGizmoLocalSpace(GizmoAxis gizmoAxis)
		{
			float num = this._rotationSphereRadius * 1f;
			Matrix4x4 matrix4x = default(Matrix4x4);
			if (gizmoAxis == GizmoAxis.X)
			{
				matrix4x.SetTRS(Vector3.zero, Quaternion.Euler(0f, 0f, 90f), Vector3.one);
			}
			else if (gizmoAxis == GizmoAxis.Y)
			{
				matrix4x = Matrix4x4.identity;
			}
			else
			{
				matrix4x.SetTRS(Vector3.zero, Quaternion.Euler(90f, 0f, 0f), Vector3.one);
			}
			Vector3[] array = new Vector3[100];
			float num2 = 3.6363637f;
			for (int i = 0; i < 100; i++)
			{
				Vector3 vector = new Vector3(Mathf.Cos(0.017453292f * num2 * (float)i) * num, 0f, Mathf.Sin(0.017453292f * num2 * (float)i) * num);
				vector = matrix4x.MultiplyPoint(vector);
				array[i] = vector;
			}
			return array;
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x00143AAE File Offset: 0x00141CAE
		private float GetWorldSpaceRotationSphereRadius()
		{
			return this._rotationSphereRadius * base.CalculateGizmoScale();
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x00143ABD File Offset: 0x00141CBD
		private float GetWorldSpaceRotationCircleRadius()
		{
			return this.GetWorldSpaceRotationSphereRadius() * 1f;
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x00143ACB File Offset: 0x00141CCB
		private float GetRotationCircleCylinderAxisLength()
		{
			if (this._camera.orthographic)
			{
				return 0.4f;
			}
			return 0.2f * base.CalculateGizmoScale();
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x00143AEC File Offset: 0x00141CEC
		private float GetRotationCircleIntersectionEpsilon()
		{
			if (this._camera.orthographic)
			{
				return 0.35f;
			}
			return 0.2f * base.CalculateGizmoScale();
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x00143B10 File Offset: 0x00141D10
		private bool RayIntersectsRotationCircle(Ray ray, Vector3 circlePlaneNormal, float circleRadius, out Vector3 intersectionPoint)
		{
			intersectionPoint = Vector3.zero;
			float d;
			if (ray.Intersects3DCircle(this._gizmoTransform.position, circleRadius, circlePlaneNormal, true, this.GetRotationCircleIntersectionEpsilon(), out d))
			{
				intersectionPoint = ray.origin + ray.direction * d;
				return true;
			}
			float rotationCircleCylinderAxisLength = this.GetRotationCircleCylinderAxisLength();
			Vector3 cylinderAxisFirstPoint = this._gizmoTransform.position - circlePlaneNormal * rotationCircleCylinderAxisLength * 0.5f;
			Vector3 cylinderAxisSecondPoint = this._gizmoTransform.position + circlePlaneNormal * rotationCircleCylinderAxisLength * 0.5f;
			if (ray.IntersectsCylinder(cylinderAxisFirstPoint, cylinderAxisSecondPoint, circleRadius, out d))
			{
				intersectionPoint = ray.origin + ray.direction * d;
				return true;
			}
			return false;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x00143BE4 File Offset: 0x00141DE4
		private void RotateControlledObjects(Vector3 rotationAxis, float angleInDegrees, int rotationAxisIndex = -1)
		{
			if (base.ControlledObjects != null)
			{
				rotationAxis.Normalize();
				List<GameObject> parentsFromControlledObjects = base.GetParentsFromControlledObjects(true);
				bool flag = !this._isCameraLookRotationCircleSelected && !this._isRotationSphereSelected && rotationAxisIndex >= 0 && rotationAxisIndex < 3;
				if (parentsFromControlledObjects.Count != 0)
				{
					if (this._transformPivotPoint == TransformPivotPoint.Center)
					{
						using (List<GameObject>.Enumerator enumerator = parentsFromControlledObjects.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GameObject gameObject = enumerator.Current;
								if (gameObject != null)
								{
									float angleInDegrees2 = angleInDegrees;
									if (flag && this._objAxisMask.ContainsKey(gameObject) && !this._objAxisMask[gameObject][rotationAxisIndex])
									{
										angleInDegrees2 = 0f;
									}
									gameObject.Rotate(rotationAxis, angleInDegrees2, this._gizmoTransform.position);
									IRTEditorEventListener component = gameObject.GetComponent<IRTEditorEventListener>();
									if (component != null)
									{
										component.OnAlteredByTransformGizmo(this);
									}
									this._objectsWereTransformedSinceLeftMouseButtonWasPressed = true;
								}
							}
							return;
						}
					}
					foreach (GameObject gameObject2 in parentsFromControlledObjects)
					{
						if (gameObject2 != null)
						{
							float angle = angleInDegrees;
							if (flag && this._objAxisMask.ContainsKey(gameObject2) && !this._objAxisMask[gameObject2][rotationAxisIndex])
							{
								angle = 0f;
							}
							gameObject2.transform.Rotate(rotationAxis, angle, Space.World);
							IRTEditorEventListener component2 = gameObject2.GetComponent<IRTEditorEventListener>();
							if (component2 != null)
							{
								component2.OnAlteredByTransformGizmo(this);
							}
							this._objectsWereTransformedSinceLeftMouseButtonWasPressed = true;
						}
					}
				}
			}
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x00143D78 File Offset: 0x00141F78
		private void DrawRotationSphere(Matrix4x4 worldTransform)
		{
			Material gizmoSolidComponent = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
			gizmoSolidComponent.SetInt("_ZWrite", 1);
			gizmoSolidComponent.SetVector("_LightDir", this._cameraTransform.forward);
			gizmoSolidComponent.SetInt("_IsLit", this._isRotationSphereLit ? 1 : 0);
			gizmoSolidComponent.SetFloat("_LightIntensity", 1.5f);
			gizmoSolidComponent.SetColor("_Color", this._rotationSphereColor);
			gizmoSolidComponent.SetInt("_ZTest", 0);
			gizmoSolidComponent.SetPass(0);
			Graphics.DrawMeshNow(SingletonBase<MeshPool>.Instance.SphereMesh, worldTransform);
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x00143E11 File Offset: 0x00142011
		private Matrix4x4 GetRotationSphereWorldTransform()
		{
			return Matrix4x4.TRS(this._gizmoTransform.position, this._gizmoTransform.rotation, this._gizmoTransform.lossyScale * this._rotationSphereRadius);
		}

		// Token: 0x04001F16 RID: 7958
		[SerializeField]
		private ShortcutKeys _enableStepSnappingShortcut = new ShortcutKeys("Enable step snapping", 0)
		{
			LCtrl = true,
			UseMouseButtons = false
		};

		// Token: 0x04001F17 RID: 7959
		[SerializeField]
		private float _rotationSphereRadius = 3f;

		// Token: 0x04001F18 RID: 7960
		[SerializeField]
		private Color _rotationSphereColor = new Color(0.3f, 0.3f, 0.3f, 0.12f);

		// Token: 0x04001F19 RID: 7961
		[SerializeField]
		private bool _isRotationSphereLit = true;

		// Token: 0x04001F1A RID: 7962
		[SerializeField]
		private bool _showRotationGuide = true;

		// Token: 0x04001F1B RID: 7963
		[SerializeField]
		private Color _rotationGuieLineColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);

		// Token: 0x04001F1C RID: 7964
		[SerializeField]
		private Color _rotationGuideDiscColor = new Color(0.5f, 0.5f, 0.5f, 0.1f);

		// Token: 0x04001F1D RID: 7965
		[SerializeField]
		private bool _showRotationSphere = true;

		// Token: 0x04001F1E RID: 7966
		[SerializeField]
		private bool _showFullRotationCircleX;

		// Token: 0x04001F1F RID: 7967
		[SerializeField]
		private bool _showFullRotationCircleY;

		// Token: 0x04001F20 RID: 7968
		[SerializeField]
		private bool _showFullRotationCircleZ;

		// Token: 0x04001F21 RID: 7969
		[SerializeField]
		private bool _showRotationSphereBoundary = true;

		// Token: 0x04001F22 RID: 7970
		[SerializeField]
		private Color _rotationSphereBoundaryLineColor = Color.white;

		// Token: 0x04001F23 RID: 7971
		[SerializeField]
		private bool _showCameraLookRotationCircle = true;

		// Token: 0x04001F24 RID: 7972
		[SerializeField]
		private float _cameraLookRotationCircleRadiusScale = 1.11f;

		// Token: 0x04001F25 RID: 7973
		[SerializeField]
		private Color _cameraLookRotationCircleLineColor = Color.white;

		// Token: 0x04001F26 RID: 7974
		[SerializeField]
		private Color _cameraLookRotationCircleColorWhenSelected = Color.yellow;

		// Token: 0x04001F27 RID: 7975
		[SerializeField]
		private RotationGizmoSnapSettings _snapSettings = new RotationGizmoSnapSettings();

		// Token: 0x04001F28 RID: 7976
		private float _accumulatedSnapRotation;

		// Token: 0x04001F29 RID: 7977
		private float _totalAccumulatedRotation;

		// Token: 0x04001F2A RID: 7978
		private const float _rotationCircleRadiusScale = 1f;

		// Token: 0x04001F2B RID: 7979
		private Vector3[] _rotationCirclePointsForXAxisInLocalSpace;

		// Token: 0x04001F2C RID: 7980
		private Vector3[] _rotationCirclePointsForYAxisInLocalSpace;

		// Token: 0x04001F2D RID: 7981
		private Vector3[] _rotationCirclePointsForZAxisInLocalSpace;

		// Token: 0x04001F2E RID: 7982
		private Vector3[] _rotationGuideLinePoints = new Vector3[2];

		// Token: 0x04001F2F RID: 7983
		private bool _isCameraLookRotationCircleSelected;

		// Token: 0x04001F30 RID: 7984
		private Vector2 _cameraLookRotationCirclePickPoint;

		// Token: 0x04001F31 RID: 7985
		private bool _isRotationSphereSelected;
	}
}
