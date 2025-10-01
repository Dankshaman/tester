using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003FF RID: 1023
	public abstract class Gizmo : MonoBehaviour
	{
		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06002EEB RID: 12011 RVA: 0x00141950 File Offset: 0x0013FB50
		// (remove) Token: 0x06002EEC RID: 12012 RVA: 0x00141988 File Offset: 0x0013FB88
		public event Gizmo.GizmoDragStartHandler GizmoDragStart;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06002EED RID: 12013 RVA: 0x001419C0 File Offset: 0x0013FBC0
		// (remove) Token: 0x06002EEE RID: 12014 RVA: 0x001419F8 File Offset: 0x0013FBF8
		public event Gizmo.GizmoDragUpdateHandler GizmoDragUpdate;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06002EEF RID: 12015 RVA: 0x00141A30 File Offset: 0x0013FC30
		// (remove) Token: 0x06002EF0 RID: 12016 RVA: 0x00141A68 File Offset: 0x0013FC68
		public event Gizmo.GizmoDragEndHandler GizmoDragEnd;

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinGizmoBaseScale
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x00141A9D File Offset: 0x0013FC9D
		public static Color DefaultXAxisColor
		{
			get
			{
				return new Color(0.85882354f, 0.24313726f, 0.11372549f, 1f);
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002EF3 RID: 12019 RVA: 0x00141AB8 File Offset: 0x0013FCB8
		public static Color DefaultYAxisColor
		{
			get
			{
				return new Color(0.6039216f, 0.9529412f, 0.28235295f, 1f);
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x00141AD3 File Offset: 0x0013FCD3
		public static Color DefaultZAxisColor
		{
			get
			{
				return new Color(0.22745098f, 0.47843137f, 0.972549f, 1f);
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06002EF5 RID: 12021 RVA: 0x00141AEE File Offset: 0x0013FCEE
		public static Color DefaultSelectedAxisColor
		{
			get
			{
				return new Color(0.9647059f, 0.9490196f, 0.19607843f, 1f);
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002EF6 RID: 12022 RVA: 0x00141B09 File Offset: 0x0013FD09
		// (set) Token: 0x06002EF7 RID: 12023 RVA: 0x00141B11 File Offset: 0x0013FD11
		public Color SelectedAxisColor
		{
			get
			{
				return this._selectedAxisColor;
			}
			set
			{
				this._selectedAxisColor = value;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x00141B1A File Offset: 0x0013FD1A
		// (set) Token: 0x06002EF9 RID: 12025 RVA: 0x00141B22 File Offset: 0x0013FD22
		public float GizmoBaseScale
		{
			get
			{
				return this._gizmoBaseScale;
			}
			set
			{
				this._gizmoBaseScale = Mathf.Max(Gizmo.MinGizmoBaseScale, value);
				this.AdjustGizmoScale();
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x00141B3B File Offset: 0x0013FD3B
		// (set) Token: 0x06002EFB RID: 12027 RVA: 0x00141B43 File Offset: 0x0013FD43
		public bool PreserveGizmoScreenSize
		{
			get
			{
				return this._preserveGizmoScreenSize;
			}
			set
			{
				this._preserveGizmoScreenSize = value;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x00141B4C File Offset: 0x0013FD4C
		// (set) Token: 0x06002EFD RID: 12029 RVA: 0x00141B54 File Offset: 0x0013FD54
		public TransformPivotPoint TransformPivotPoint
		{
			get
			{
				return this._transformPivotPoint;
			}
			set
			{
				this._transformPivotPoint = value;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002EFE RID: 12030 RVA: 0x00141B5D File Offset: 0x0013FD5D
		// (set) Token: 0x06002EFF RID: 12031 RVA: 0x00141B65 File Offset: 0x0013FD65
		public IEnumerable<GameObject> ControlledObjects { get; set; }

		// Token: 0x06002F00 RID: 12032 RVA: 0x00141B6E File Offset: 0x0013FD6E
		public void SetAxisVisibility(bool isVisible, int axisIndex)
		{
			this._axesVisibilityMask[axisIndex] = isVisible;
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x00141B79 File Offset: 0x0013FD79
		public bool GetAxisVisibility(int axisIndex)
		{
			return this._axesVisibilityMask[axisIndex];
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x00141B83 File Offset: 0x0013FD83
		public bool IsTransformingObjects()
		{
			return this.IsReadyForObjectManipulation() && this._isDragging;
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x00141B95 File Offset: 0x0013FD95
		public Color GetAxisColor(GizmoAxis axis)
		{
			if (axis == GizmoAxis.None)
			{
				return Color.black;
			}
			return this._axesColors[(int)axis];
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x00141BAD File Offset: 0x0013FDAD
		public void SetAxisColor(GizmoAxis axis, Color color)
		{
			if (axis == GizmoAxis.None)
			{
				return;
			}
			this._axesColors[(int)axis] = color;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x00141BC1 File Offset: 0x0013FDC1
		public void MaskObject(GameObject gameObject)
		{
			this._maskedObjects.Add(gameObject);
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x00141BD0 File Offset: 0x0013FDD0
		public void UnmaskObject(GameObject gameObject)
		{
			this._maskedObjects.Remove(gameObject);
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x00141BE0 File Offset: 0x0013FDE0
		public void MaskObjectCollection(IEnumerable<GameObject> objectCollection)
		{
			foreach (GameObject gameObject in objectCollection)
			{
				this.MaskObject(gameObject);
			}
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x00141C28 File Offset: 0x0013FE28
		public void UnmaskObjectCollection(IEnumerable<GameObject> objectCollection)
		{
			foreach (GameObject gameObject in objectCollection)
			{
				this.UnmaskObject(gameObject);
			}
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x00141C70 File Offset: 0x0013FE70
		public bool IsGameObjectMasked(GameObject gameObject)
		{
			return this._maskedObjects.Contains(gameObject);
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x00141C7E File Offset: 0x0013FE7E
		public void MaskObjectLayer(int objectLayer)
		{
			this._maskedObjectLayers = LayerHelper.SetLayerBit(this._maskedObjectLayers, objectLayer);
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x00141C92 File Offset: 0x0013FE92
		public void UnmaskObjectkLayer(int objectLayer)
		{
			this._maskedObjectLayers = LayerHelper.ClearLayerBit(this._maskedObjectLayers, objectLayer);
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x00141CA6 File Offset: 0x0013FEA6
		public bool IsObjectLayerMasked(int objectLayer)
		{
			return LayerHelper.IsLayerBitSet(this._maskedObjectLayers, objectLayer);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x00141CB4 File Offset: 0x0013FEB4
		public bool CanObjectBeManipulated(GameObject gameObject)
		{
			return !this.IsGameObjectMasked(gameObject) && !this.IsObjectLayerMasked(gameObject.layer);
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x00141CD0 File Offset: 0x0013FED0
		public bool CanAllControlledObjectsBeManipulated()
		{
			if (this.ControlledObjects != null)
			{
				foreach (GameObject gameObject in this.ControlledObjects)
				{
					if (!this.CanObjectBeManipulated(gameObject))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x00141D30 File Offset: 0x0013FF30
		public bool CanAnyControlledObjectBeManipulated()
		{
			if (this.ControlledObjects != null)
			{
				foreach (GameObject gameObject in this.ControlledObjects)
				{
					if (this.CanObjectBeManipulated(gameObject))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x00141D90 File Offset: 0x0013FF90
		public List<GameObject> GetControlledObjectsWhichCanBeManipulated()
		{
			if (this.ControlledObjects == null)
			{
				return new List<GameObject>();
			}
			List<GameObject> list = new List<GameObject>(this.ControlledObjects);
			list.RemoveAll((GameObject item) => !this.CanObjectBeManipulated(item));
			return list;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x00141DC0 File Offset: 0x0013FFC0
		public void SetObjectAxisMask(GameObject gameObj, bool[] axisMask)
		{
			if (axisMask == null || axisMask.Length < 3)
			{
				return;
			}
			bool[] value = axisMask.Clone() as bool[];
			if (this._objAxisMask.ContainsKey(gameObj))
			{
				this._objAxisMask[gameObj] = value;
				return;
			}
			this._objAxisMask.Add(gameObj, value);
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x00141E0C File Offset: 0x0014000C
		public void SetObjectAxisMask(GameObject gameObj, int axisIndex, bool isMasked)
		{
			if (axisIndex < 0 || axisIndex >= 3)
			{
				return;
			}
			if (this._objAxisMask.ContainsKey(gameObj))
			{
				this._objAxisMask[gameObj][axisIndex] = isMasked;
				return;
			}
			bool[] array = new bool[]
			{
				true,
				true,
				true
			};
			if (isMasked)
			{
				array[axisIndex] = false;
			}
			this._objAxisMask.Add(gameObj, array);
		}

		// Token: 0x06002F13 RID: 12051
		public abstract bool IsReadyForObjectManipulation();

		// Token: 0x06002F14 RID: 12052
		public abstract GizmoType GetGizmoType();

		// Token: 0x06002F15 RID: 12053 RVA: 0x00141E68 File Offset: 0x00140068
		protected virtual void Start()
		{
			this._camera = MonoSingletonBase<EditorCamera>.Instance.Camera;
			this._cameraTransform = this._camera.transform;
			this._gizmoTransform = base.transform;
			SingletonBase<MaterialPool>.Instance.GizmoLine.SetInt("_StencilRefValue", this._doNotUseStencil);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x00141EBC File Offset: 0x001400BC
		protected virtual void Update()
		{
			if (MonoSingletonBase<InputDevice>.Instance.WasPressedInCurrentFrame(0))
			{
				this.OnInputDeviceFirstButtonDown();
			}
			if (MonoSingletonBase<InputDevice>.Instance.WasReleasedInCurrentFrame(0))
			{
				this.OnInputDeviceFirstButtonUp();
			}
			if (MonoSingletonBase<InputDevice>.Instance.WasMoved())
			{
				this.OnInputDeviceMoved();
			}
			this.AdjustGizmoScale();
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x00141EFC File Offset: 0x001400FC
		protected void AdjustGizmoScale()
		{
			if (this._camera != null && this._cameraTransform != null)
			{
				float num = this.CalculateGizmoScale();
				base.transform.localScale = new Vector3(num, num, num);
			}
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x00141F40 File Offset: 0x00140140
		protected float CalculateGizmoScale()
		{
			if (!this._preserveGizmoScreenSize)
			{
				return this._gizmoBaseScale;
			}
			if (this._camera.orthographic)
			{
				float num = 0.02f;
				return this._gizmoBaseScale * this._camera.orthographicSize / (this._camera.pixelRect.height * num);
			}
			Vector3 vector = base.transform.position - this._cameraTransform.position;
			return this._gizmoBaseScale * vector.magnitude / (this._camera.pixelRect.height * 0.045f);
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x00141FDC File Offset: 0x001401DC
		protected int[] GetSortedGizmoAxesIndices()
		{
			Vector3[] gizmoLocalAxes = this.GetGizmoLocalAxes();
			int num = 0;
			float num2 = Vector3.Dot(gizmoLocalAxes[0], this._cameraTransform.forward);
			int[] array = new int[]
			{
				0,
				1,
				2
			};
			for (int i = 1; i < 3; i++)
			{
				float num3 = Vector3.Dot(gizmoLocalAxes[i], this._cameraTransform.forward);
				if (num3 >= 0f && num3 > num2)
				{
					num2 = num3;
					num = i;
					if (Mathf.Abs(1f - num2) < 0.0001f)
					{
						break;
					}
				}
			}
			array[0] = num;
			array[num] = 0;
			return array;
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x00142074 File Offset: 0x00140274
		protected void UpdateShaderStencilRefValuesForGizmoAxisLineDraw(int axisIndex, Vector3 startPoint, Vector3 endPoint, float gizmoScale)
		{
			Vector3 vector = endPoint - startPoint;
			float magnitude = vector.magnitude;
			vector.Normalize();
			Vector3 vector2 = startPoint + vector * 0.97f * magnitude;
			Vector3 direction = this._camera.orthographic ? (-this._cameraTransform.forward) : (this._cameraTransform.position - vector2);
			Ray ray = new Ray(vector2, direction);
			Plane plane = new Plane(vector, endPoint);
			float num;
			if (plane.Raycast(ray, out num))
			{
				SingletonBase<MaterialPool>.Instance.GizmoLine.SetInt("_StencilRefValue", this._axesStencilRefValues[axisIndex]);
				return;
			}
			SingletonBase<MaterialPool>.Instance.GizmoLine.SetInt("_StencilRefValue", this._doNotUseStencil);
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x00142137 File Offset: 0x00140337
		protected Vector3[] GetGizmoLocalAxes()
		{
			return new Vector3[]
			{
				this._gizmoTransform.right,
				this._gizmoTransform.up,
				this._gizmoTransform.forward
			};
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x00142178 File Offset: 0x00140378
		protected Plane GetCoordinateSystemPlaneFromSelectedAxis()
		{
			switch (this._selectedAxis)
			{
			case GizmoAxis.X:
				return new Plane(this.GetAxisPlaneNormalMostAlignedWithCameraLook(GizmoAxis.X), this._gizmoTransform.position);
			case GizmoAxis.Y:
				return new Plane(this.GetAxisPlaneNormalMostAlignedWithCameraLook(GizmoAxis.Y), this._gizmoTransform.position);
			case GizmoAxis.Z:
				return new Plane(this.GetAxisPlaneNormalMostAlignedWithCameraLook(GizmoAxis.Z), this._gizmoTransform.position);
			default:
				return default(Plane);
			}
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x001421F4 File Offset: 0x001403F4
		protected Vector3 GetAxisPlaneNormalMostAlignedWithCameraLook(GizmoAxis gizmoAxis)
		{
			float num = 0f;
			Vector3 result = this._gizmoTransform.forward;
			Vector3[] array;
			if (gizmoAxis == GizmoAxis.X)
			{
				array = new Vector3[]
				{
					this._gizmoTransform.forward,
					this._gizmoTransform.up
				};
			}
			else if (gizmoAxis == GizmoAxis.Y)
			{
				array = new Vector3[]
				{
					this._gizmoTransform.forward,
					this._gizmoTransform.right
				};
			}
			else
			{
				array = new Vector3[]
				{
					this._gizmoTransform.right,
					this._gizmoTransform.up
				};
			}
			for (int i = 0; i < 2; i++)
			{
				float num2 = Mathf.Abs(Vector3.Dot(this._cameraTransform.forward, array[i]));
				if (num2 > num)
				{
					num = num2;
					result = array[i];
				}
			}
			return result;
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x001422DC File Offset: 0x001404DC
		protected float[] GetMultiAxisExtensionSigns(bool adjustForBetterVisibility)
		{
			float num = Mathf.Sign(this.CalculateGizmoScale());
			if (adjustForBetterVisibility)
			{
				Vector3 forward = this._cameraTransform.forward;
				float num2 = Vector3.Dot(forward, this._gizmoTransform.right);
				float num3 = Vector3.Dot(forward, this._gizmoTransform.up);
				float num4 = Vector3.Dot(forward, this._gizmoTransform.forward);
				return new float[]
				{
					(num2 > 0f) ? (-1f * num) : (1f * num),
					(num3 > 0f) ? (-1f * num) : (1f * num),
					(num4 > 0f) ? (-1f * num) : (1f * num)
				};
			}
			return new float[]
			{
				1f * num,
				1f * num,
				1f * num
			};
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x001423B7 File Offset: 0x001405B7
		protected void DestroyGizmoMesh(Mesh gizmoMesh)
		{
			if (gizmoMesh == null)
			{
				return;
			}
			if (Application.isEditor && Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(gizmoMesh);
				return;
			}
			if (!Application.isEditor && Application.isPlaying)
			{
				UnityEngine.Object.Destroy(gizmoMesh);
			}
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x001423EC File Offset: 0x001405EC
		protected List<GameObject> GetParentsFromControlledObjects(bool filterOnlyCanBeManipulated)
		{
			if (this.ControlledObjects == null)
			{
				return new List<GameObject>();
			}
			if (!filterOnlyCanBeManipulated)
			{
				return GameObjectExtensions.GetParentsFromObjectCollection(this.ControlledObjects);
			}
			return GameObjectExtensions.GetParentsFromObjectCollection(this.GetControlledObjectsWhichCanBeManipulated());
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x00142416 File Offset: 0x00140616
		protected virtual void OnInputDeviceFirstButtonDown()
		{
			this.TakeObjectTransformSnapshots(out this._preTransformObjectSnapshots);
			if (this.IsReadyForObjectManipulation())
			{
				this._isDragging = true;
				if (this.GizmoDragStart != null)
				{
					this.GizmoDragStart(this);
				}
			}
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x00142448 File Offset: 0x00140648
		protected virtual void OnInputDeviceFirstButtonUp()
		{
			if (this._objectsWereTransformedSinceLeftMouseButtonWasPressed)
			{
				this.TakeObjectTransformSnapshots(out this._postTransformObjectSnapshots);
				new PostGizmoTransformedObjectsAction(this._preTransformObjectSnapshots, this._postTransformObjectSnapshots, this).Execute();
				this._objectsWereTransformedSinceLeftMouseButtonWasPressed = false;
			}
			if (this._isDragging)
			{
				this._isDragging = false;
				if (this.GizmoDragEnd != null)
				{
					this.GizmoDragEnd(this);
				}
			}
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x001424AA File Offset: 0x001406AA
		protected virtual void OnInputDeviceMoved()
		{
			if (this.GizmoDragUpdate != null && MonoSingletonBase<InputDevice>.Instance.IsPressed(0) && this.IsReadyForObjectManipulation())
			{
				this.GizmoDragUpdate(this);
			}
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x001424D5 File Offset: 0x001406D5
		protected virtual void OnRenderObject()
		{
			if (Camera.current != MonoSingletonBase<EditorCamera>.Instance.Camera)
			{
				return;
			}
			this.AdjustGizmoScale();
		}

		// Token: 0x06002F25 RID: 12069
		protected abstract bool DetectHoveredComponents(bool updateCompStates);

		// Token: 0x06002F26 RID: 12070 RVA: 0x001424F4 File Offset: 0x001406F4
		private void TakeObjectTransformSnapshots(out List<ObjectTransformSnapshot> objectTransformSnapshots)
		{
			objectTransformSnapshots = null;
			List<GameObject> parentsFromControlledObjects = this.GetParentsFromControlledObjects(true);
			if (parentsFromControlledObjects.Count == 0)
			{
				return;
			}
			objectTransformSnapshots = new List<ObjectTransformSnapshot>(parentsFromControlledObjects.Count);
			foreach (GameObject gameObject in parentsFromControlledObjects)
			{
				ObjectTransformSnapshot objectTransformSnapshot = new ObjectTransformSnapshot();
				objectTransformSnapshot.TakeSnapshot(gameObject);
				objectTransformSnapshots.Add(objectTransformSnapshot);
			}
		}

		// Token: 0x04001EED RID: 7917
		protected Dictionary<GameObject, bool[]> _objAxisMask = new Dictionary<GameObject, bool[]>();

		// Token: 0x04001EEE RID: 7918
		protected HashSet<GameObject> _maskedObjects = new HashSet<GameObject>();

		// Token: 0x04001EEF RID: 7919
		protected int _maskedObjectLayers;

		// Token: 0x04001EF0 RID: 7920
		protected Camera _camera;

		// Token: 0x04001EF1 RID: 7921
		[SerializeField]
		protected Color[] _axesColors = new Color[]
		{
			Gizmo.DefaultXAxisColor,
			Gizmo.DefaultYAxisColor,
			Gizmo.DefaultZAxisColor
		};

		// Token: 0x04001EF2 RID: 7922
		[SerializeField]
		protected Color _selectedAxisColor = Gizmo.DefaultSelectedAxisColor;

		// Token: 0x04001EF3 RID: 7923
		[SerializeField]
		protected float _gizmoBaseScale = 0.77f;

		// Token: 0x04001EF4 RID: 7924
		[SerializeField]
		protected bool _preserveGizmoScreenSize = true;

		// Token: 0x04001EF5 RID: 7925
		protected Transform _gizmoTransform;

		// Token: 0x04001EF6 RID: 7926
		protected Transform _cameraTransform;

		// Token: 0x04001EF7 RID: 7927
		protected GizmoAxis _selectedAxis = GizmoAxis.None;

		// Token: 0x04001EF8 RID: 7928
		protected Vector3 _lastGizmoPickPoint;

		// Token: 0x04001EF9 RID: 7929
		protected int[] _axesStencilRefValues = new int[]
		{
			252,
			253,
			254
		};

		// Token: 0x04001EFA RID: 7930
		protected int _doNotUseStencil = 255;

		// Token: 0x04001EFB RID: 7931
		protected TransformPivotPoint _transformPivotPoint = TransformPivotPoint.Center;

		// Token: 0x04001EFC RID: 7932
		protected List<ObjectTransformSnapshot> _preTransformObjectSnapshots;

		// Token: 0x04001EFD RID: 7933
		protected List<ObjectTransformSnapshot> _postTransformObjectSnapshots;

		// Token: 0x04001EFE RID: 7934
		protected bool _objectsWereTransformedSinceLeftMouseButtonWasPressed;

		// Token: 0x04001EFF RID: 7935
		protected bool _isDragging;

		// Token: 0x04001F00 RID: 7936
		[SerializeField]
		protected bool[] _axesVisibilityMask = new bool[]
		{
			true,
			true,
			true
		};

		// Token: 0x020007F9 RID: 2041
		// (Invoke) Token: 0x0600409A RID: 16538
		public delegate void GizmoDragStartHandler(Gizmo gizmo);

		// Token: 0x020007FA RID: 2042
		// (Invoke) Token: 0x0600409E RID: 16542
		public delegate void GizmoDragUpdateHandler(Gizmo gizmo);

		// Token: 0x020007FB RID: 2043
		// (Invoke) Token: 0x060040A2 RID: 16546
		public delegate void GizmoDragEndHandler(Gizmo gizmo);
	}
}
