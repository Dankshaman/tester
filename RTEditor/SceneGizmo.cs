using System;
using System.Collections;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000408 RID: 1032
	public class SceneGizmo : MonoSingletonBase<SceneGizmo>
	{
		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinScreenSize
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002FCE RID: 12238 RVA: 0x00146473 File Offset: 0x00144673
		public static float MaxScreenSize
		{
			get
			{
				return 200f;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x0013DECE File Offset: 0x0013C0CE
		public static float MinDuration
		{
			get
			{
				return 0.001f;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002FD0 RID: 12240 RVA: 0x00141A9D File Offset: 0x0013FC9D
		public static Color DefaultXAxisColor
		{
			get
			{
				return new Color(0.85882354f, 0.24313726f, 0.11372549f, 1f);
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x00141AB8 File Offset: 0x0013FCB8
		public static Color DefaultYAxisColor
		{
			get
			{
				return new Color(0.6039216f, 0.9529412f, 0.28235295f, 1f);
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x00141AD3 File Offset: 0x0013FCD3
		public static Color DefaultZAxisColor
		{
			get
			{
				return new Color(0.22745098f, 0.47843137f, 0.972549f, 1f);
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x00141AEE File Offset: 0x0013FCEE
		public static Color DefaultHoveredComponentColor
		{
			get
			{
				return new Color(0.9647059f, 0.9490196f, 0.19607843f, 1f);
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x0014647A File Offset: 0x0014467A
		public static Color DefaultCubeColor
		{
			get
			{
				return new Color(0.8f, 0.8f, 0.8f, 0.92941177f);
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x00146495 File Offset: 0x00144695
		public static Color DefaultNegativeAxisColor
		{
			get
			{
				return SceneGizmo.DefaultCubeColor;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x0014649C File Offset: 0x0014469C
		public static float DefaultScreenSize
		{
			get
			{
				return 100f;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x00024D16 File Offset: 0x00022F16
		public static int DefaultAxisLabelCharSize
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x001464A3 File Offset: 0x001446A3
		// (set) Token: 0x06002FD9 RID: 12249 RVA: 0x001464AB File Offset: 0x001446AB
		public SceneGizmoCorner Corner
		{
			get
			{
				return this._corner;
			}
			set
			{
				this._corner = value;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002FDA RID: 12250 RVA: 0x001464B4 File Offset: 0x001446B4
		// (set) Token: 0x06002FDB RID: 12251 RVA: 0x001464BC File Offset: 0x001446BC
		public float CameraAlignDuration
		{
			get
			{
				return this._cameraAlignDuration;
			}
			set
			{
				this._cameraAlignDuration = Mathf.Max(value, SceneGizmo.MinDuration);
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002FDC RID: 12252 RVA: 0x001464CF File Offset: 0x001446CF
		// (set) Token: 0x06002FDD RID: 12253 RVA: 0x001464D7 File Offset: 0x001446D7
		public Color NegativeAxisColor
		{
			get
			{
				return this._negativeAxisColor;
			}
			set
			{
				this._negativeAxisColor = value;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x001464E0 File Offset: 0x001446E0
		// (set) Token: 0x06002FDF RID: 12255 RVA: 0x001464E8 File Offset: 0x001446E8
		public Color XAxisColor
		{
			get
			{
				return this._xAxisColor;
			}
			set
			{
				this._xAxisColor = value;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x001464F1 File Offset: 0x001446F1
		// (set) Token: 0x06002FE1 RID: 12257 RVA: 0x001464F9 File Offset: 0x001446F9
		public Color YAxisColor
		{
			get
			{
				return this._yAxisColor;
			}
			set
			{
				this._yAxisColor = value;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x00146502 File Offset: 0x00144702
		// (set) Token: 0x06002FE3 RID: 12259 RVA: 0x0014650A File Offset: 0x0014470A
		public Color ZAxisColor
		{
			get
			{
				return this._zAxisColor;
			}
			set
			{
				this._zAxisColor = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x00146513 File Offset: 0x00144713
		// (set) Token: 0x06002FE5 RID: 12261 RVA: 0x0014651B File Offset: 0x0014471B
		public Color HoveredComponentColor
		{
			get
			{
				return this._hoveredComponentColor;
			}
			set
			{
				this._hoveredComponentColor = value;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x00146524 File Offset: 0x00144724
		// (set) Token: 0x06002FE7 RID: 12263 RVA: 0x0014652C File Offset: 0x0014472C
		public Color CubeColor
		{
			get
			{
				return this._cubeColor;
			}
			set
			{
				this._cubeColor = value;
			}
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x00146535 File Offset: 0x00144735
		public bool IsHovered()
		{
			return this.DetectHoveredComponent() != -1;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x00146543 File Offset: 0x00144743
		private void Awake()
		{
			this._gizmoTransform = base.transform;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x00146554 File Offset: 0x00144754
		private void Start()
		{
			this.CreateSceneGizmoCamera();
			this._axisLabelTextures[0] = (Resources.Load("Textures/XAxisLabel") as Texture2D);
			this._axisLabelTextures[1] = (Resources.Load("Textures/YAxisLabel") as Texture2D);
			this._axisLabelTextures[2] = (Resources.Load("Textures/ZAxisLabel") as Texture2D);
			int length = Enum.GetValues(typeof(SceneGizmoComponent)).Length;
			for (int i = 0; i < length; i++)
			{
				this._componentAlphaFadeInfo[i] = new SceneGizmo.GizmoCompAlphaFadeInfo();
				this._componentAlphas[i] = 1f;
			}
			this.UpdateGizmoCamera();
			base.StartCoroutine(this.DoComponentAlphaTransitions());
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x001465FC File Offset: 0x001447FC
		private void Update()
		{
			this.UpdateGizmoCamera();
			this._gizmoTransform.rotation = this.CalculateGizmoRotation();
			this._gizmoTransform.gameObject.SetAbsoluteScale(Vector3.one);
			this._gizmoTransform.position = this.CalculateCubePosition();
			this._hoveredComponent = this.DetectHoveredComponent();
			this.EstablishComponentColors();
			if (MonoSingletonBase<InputDevice>.Instance.WasPressedInCurrentFrame(0))
			{
				this.OnFirstInputDeviceBtnDown();
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x0014666C File Offset: 0x0014486C
		private void OnGUI()
		{
			Rect pixelRect = this._gizmoCamera.pixelRect;
			GUI.Label(new Rect(pixelRect.xMin + 35f, (float)Screen.height - pixelRect.yMin - 12f, pixelRect.width, pixelRect.height), this._gizmoCamera.orthographic ? "Ortho" : "Persp");
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x001466D8 File Offset: 0x001448D8
		private void OnFirstInputDeviceBtnDown()
		{
			if (this._hoveredComponent == 0)
			{
				Camera camera = MonoSingletonBase<EditorCamera>.Instance.Camera;
				MonoSingletonBase<EditorCamera>.Instance.SetOrtho(!camera.orthographic);
				this._gizmoCamera.orthographic = camera.orthographic;
			}
			if (this._hoveredComponent == 1)
			{
				MonoSingletonBase<EditorCamera>.Instance.AlignLookWithWorldAxis(Axis.X, false, this._cameraAlignDuration);
				return;
			}
			if (this._hoveredComponent == 2)
			{
				MonoSingletonBase<EditorCamera>.Instance.AlignLookWithWorldAxis(Axis.X, true, this._cameraAlignDuration);
				return;
			}
			if (this._hoveredComponent == 3)
			{
				MonoSingletonBase<EditorCamera>.Instance.AlignLookWithWorldAxis(Axis.Y, false, this._cameraAlignDuration);
				return;
			}
			if (this._hoveredComponent == 4)
			{
				MonoSingletonBase<EditorCamera>.Instance.AlignLookWithWorldAxis(Axis.Y, true, this._cameraAlignDuration);
				return;
			}
			if (this._hoveredComponent == 5)
			{
				MonoSingletonBase<EditorCamera>.Instance.AlignLookWithWorldAxis(Axis.Z, false, this._cameraAlignDuration);
				return;
			}
			if (this._hoveredComponent == 6)
			{
				MonoSingletonBase<EditorCamera>.Instance.AlignLookWithWorldAxis(Axis.Z, true, this._cameraAlignDuration);
			}
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x001467C4 File Offset: 0x001449C4
		private int DetectHoveredComponent()
		{
			int result = -1;
			if (this._gizmoCamera == null)
			{
				return this._hoveredComponent;
			}
			Ray ray;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPickRay(this._gizmoCamera, out ray))
			{
				return -1;
			}
			Matrix4x4[] componentTransforms = this.GetComponentTransforms();
			float num = float.MaxValue;
			float num2;
			if (ray.IntersectsBox(1f, 1f, 1f, componentTransforms[0], out num2))
			{
				num = num2;
				result = 0;
			}
			int length = Enum.GetValues(typeof(SceneGizmoComponent)).Length;
			for (int i = 1; i < length; i++)
			{
				if (this._componentAlphas[i] == 1f && ray.IntersectsCone(1f, 1f, componentTransforms[i], out num2) && num2 < num)
				{
					num = num2;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x00146890 File Offset: 0x00144A90
		private void EstablishComponentColors()
		{
			this._componentColors[0] = this._cubeColor;
			this._componentColors[0].a = this._componentAlphas[0];
			this._componentColors[1] = this._xAxisColor;
			this._componentColors[1].a = this._componentAlphas[1];
			this._componentColors[3] = this._yAxisColor;
			this._componentColors[3].a = this._componentAlphas[3];
			this._componentColors[5] = this._zAxisColor;
			this._componentColors[5].a = this._componentAlphas[5];
			this._componentColors[2] = this._negativeAxisColor;
			this._componentColors[2].a = this._componentAlphas[2];
			this._componentColors[4] = this._negativeAxisColor;
			this._componentColors[4].a = this._componentAlphas[4];
			this._componentColors[6] = this._negativeAxisColor;
			this._componentColors[6].a = this._componentAlphas[6];
			if (this._hoveredComponent >= 0)
			{
				this._componentColors[this._hoveredComponent] = this._hoveredComponentColor;
			}
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x001469EC File Offset: 0x00144BEC
		private void OnRenderObject()
		{
			if (Camera.current != this._gizmoCamera)
			{
				return;
			}
			Material material = SingletonBase<MaterialPool>.Instance.GizmoSolidComponent;
			material.SetVector("_LightDir", this._gizmoCameraTransform.forward);
			material.SetInt("_IsLit", 1);
			material.SetInt("_ZTest", 2);
			material.SetFloat("_LightIntensity", 1.23f);
			Mesh coneMesh = SingletonBase<MeshPool>.Instance.ConeMesh;
			Matrix4x4[] componentTransforms = this.GetComponentTransforms();
			material.SetColor("_Color", this._componentColors[0]);
			material.SetInt("_ZWrite", 1);
			material.SetPass(0);
			Graphics.DrawMeshNow(SingletonBase<MeshPool>.Instance.BoxMesh, componentTransforms[0]);
			int length = Enum.GetValues(typeof(SceneGizmoComponent)).Length;
			int num = -1;
			int num2 = -1;
			for (int i = 1; i < length; i++)
			{
				if (this._componentAlphaFadeInfo[i].IsActive)
				{
					if (num == -1)
					{
						num = i;
					}
					if (num != -1)
					{
						num2 = i;
					}
					if (num != -1 && num2 != -1)
					{
						break;
					}
				}
			}
			for (int j = 1; j < length; j++)
			{
				if (num != j && num2 != j)
				{
					material.SetInt("_ZWrite", this.GetZWriteForComponent((SceneGizmoComponent)j));
					material.SetColor("_Color", this._componentColors[j]);
					material.SetPass(0);
					Graphics.DrawMeshNow(coneMesh, componentTransforms[j]);
				}
			}
			if (num != -1)
			{
				material.SetInt("_ZWrite", this.GetZWriteForComponent((SceneGizmoComponent)num));
				material.SetColor("_Color", this._componentColors[num]);
				material.SetPass(0);
				Graphics.DrawMeshNow(coneMesh, componentTransforms[num]);
			}
			if (num2 != -1)
			{
				material.SetInt("_ZWrite", this.GetZWriteForComponent((SceneGizmoComponent)num2));
				material.SetColor("_Color", this._componentColors[num2]);
				material.SetPass(0);
				Graphics.DrawMeshNow(coneMesh, componentTransforms[num2]);
			}
			material = SingletonBase<MaterialPool>.Instance.TintedDiffuse;
			material.SetInt("_ZWrite", 0);
			Mesh xysquareMesh = SingletonBase<MeshPool>.Instance.XYSquareMesh;
			float num3 = this.CalculateAxisLabelSquareSize();
			Vector3 s = new Vector3(num3, num3, num3);
			Quaternion rotation = this._gizmoCameraTransform.rotation;
			Vector3 a = this.CalculateCubePosition();
			float num4 = this.CalculateGizmoExtent();
			float num5 = this.CalculateAxisConeRadius();
			Vector3[] array = new Vector3[]
			{
				this._gizmoTransform.right,
				this._gizmoTransform.up,
				this._gizmoTransform.forward
			};
			for (int k = 0; k < 3; k++)
			{
				Color white = Color.white;
				white.a = this._componentAlphas[k * 2 + 1];
				material.SetColor("_Color", white);
				material.SetTexture("_MainTex", this._axisLabelTextures[k]);
				material.SetPass(0);
				float num6 = Vector3.Dot(array[k], this._gizmoCameraTransform.forward);
				float num7 = num4 - num3 * 0.5f;
				float num8 = Mathf.Lerp(0f, num3 * 0.5f + num5 * 0.5f, Mathf.Abs(num6));
				float d = 0f;
				if (num6 > 0f)
				{
					d = Mathf.Lerp(0f, num3 * 0.5f, num6);
				}
				Vector3 pos = a + array[k] * (num7 + num8) + this._gizmoTransform.up * d;
				Graphics.DrawMeshNow(xysquareMesh, Matrix4x4.TRS(pos, rotation, s));
			}
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x00146D99 File Offset: 0x00144F99
		private int GetZWriteForComponent(SceneGizmoComponent comp)
		{
			if (comp == SceneGizmoComponent.Cube)
			{
				return 1;
			}
			if (this._componentAlphas[(int)comp] == 1f)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x00146DB4 File Offset: 0x00144FB4
		private Matrix4x4[] GetComponentTransforms()
		{
			Vector3 vector = this.CalculateCubePosition();
			float num = this.CalculateAxisConeLength();
			float num2 = this.CalculateAxisConeRadius();
			float num3 = this.CalculateCubeSideLength();
			Vector3 s = Vector3.Scale(Vector3.one, new Vector3(num2, num, num2));
			Matrix4x4[] array = new Matrix4x4[Enum.GetValues(typeof(SceneGizmoComponent)).Length];
			array[0] = Matrix4x4.TRS(vector, this._gizmoTransform.rotation, Vector3.one * num3);
			array[1] = Matrix4x4.TRS(vector + this._gizmoTransform.right * (num3 * 0.5f + num), this._gizmoTransform.rotation * Quaternion.Euler(0f, 0f, 90f), s);
			array[3] = Matrix4x4.TRS(vector + this._gizmoTransform.up * (num3 * 0.5f + num), this._gizmoTransform.rotation * Quaternion.Euler(0f, 0f, 180f), s);
			array[5] = Matrix4x4.TRS(vector + this._gizmoTransform.forward * (num3 * 0.5f + num), this._gizmoTransform.rotation * Quaternion.Euler(-90f, 0f, 0f), s);
			array[2] = Matrix4x4.TRS(vector - this._gizmoTransform.right * (num3 * 0.5f + num), this._gizmoTransform.rotation * Quaternion.Euler(0f, 0f, -90f), s);
			array[4] = Matrix4x4.TRS(vector - this._gizmoTransform.up * (num3 * 0.5f + num), this._gizmoTransform.rotation * Quaternion.identity, s);
			array[6] = Matrix4x4.TRS(vector - this._gizmoTransform.forward * (num3 * 0.5f + num), this._gizmoTransform.rotation * Quaternion.Euler(90f, 0f, 0f), s);
			return array;
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x00147004 File Offset: 0x00145204
		private void CreateSceneGizmoCamera()
		{
			GameObject gameObject = new GameObject("Scene Gizmo Camera");
			this._gizmoCamera = gameObject.AddComponent<Camera>();
			this._gizmoCameraTransform = this._gizmoCamera.transform;
			this._gizmoCamera.cullingMask = 0;
			this._gizmoCamera.clearFlags = CameraClearFlags.Depth;
			this._gizmoCamera.depth = MonoSingletonBase<EditorCamera>.Instance.Camera.depth + 1f;
			this._gizmoCamera.renderingPath = RenderingPath.Forward;
			this._gizmoCameraTransform.parent = this._gizmoTransform.parent;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x00147094 File Offset: 0x00145294
		private void UpdateGizmoCamera()
		{
			Transform transform = MonoSingletonBase<EditorCamera>.Instance.transform;
			this._gizmoCameraTransform.position = transform.position;
			this._gizmoCameraTransform.rotation = transform.rotation;
			this.UpdateGizmoCameraViewport();
			this.UpdateGizmoCameraViewVolume();
			if (this._gizmoCamera.orthographic != MonoSingletonBase<EditorCamera>.Instance.Camera.orthographic)
			{
				this._gizmoCamera.orthographic = MonoSingletonBase<EditorCamera>.Instance.Camera.orthographic;
			}
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x00147110 File Offset: 0x00145310
		private void UpdateGizmoCameraViewport()
		{
			Rect pixelRect = MonoSingletonBase<EditorCamera>.Instance.Camera.pixelRect;
			if (this._corner == SceneGizmoCorner.TopRight)
			{
				this._gizmoCamera.pixelRect = new Rect(pixelRect.xMax - this._screenSize, pixelRect.yMax - this._screenSize, this._screenSize, this._screenSize);
				return;
			}
			if (this._corner == SceneGizmoCorner.TopLeft)
			{
				this._gizmoCamera.pixelRect = new Rect(pixelRect.xMin, pixelRect.yMax - this._screenSize, this._screenSize, this._screenSize);
				return;
			}
			if (this._corner == SceneGizmoCorner.BottomRight)
			{
				this._gizmoCamera.pixelRect = new Rect(pixelRect.xMax - this._screenSize, 10f + pixelRect.yMin, this._screenSize, this._screenSize);
				return;
			}
			this._gizmoCamera.pixelRect = new Rect(pixelRect.xMin, 10f + pixelRect.yMin, this._screenSize, this._screenSize);
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x0014721C File Offset: 0x0014541C
		private void UpdateGizmoCameraViewVolume()
		{
			if (this._gizmoCamera.orthographic)
			{
				this._gizmoCamera.orthographicSize = this.CalculateGizmoExtent() + 0.005f;
				return;
			}
			this._gizmoCamera.fieldOfView = 1.1f + 114.59156f * Mathf.Atan2(this.CalculateGizmoExtent(), this.CalculateGizmoOffsetFromCamera());
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x00147276 File Offset: 0x00145476
		private float CalculateGizmoOffsetFromCamera()
		{
			return this._gizmoCamera.nearClipPlane + this.CalculateGizmoExtent() + 0.01f;
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x00147290 File Offset: 0x00145490
		private float CalculateGizmoExtent()
		{
			return this.CalculateCubeSideLength() * 0.5f + this.CalculateAxisConeLength() + this.CalculateAxisLabelSquareSize() + this.CalculateAxisLabelOffsetFromCone();
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x001472B3 File Offset: 0x001454B3
		private Vector3 CalculateCubePosition()
		{
			return this._gizmoCameraTransform.position + this._gizmoCameraTransform.forward * this.CalculateGizmoOffsetFromCamera();
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x001472DB File Offset: 0x001454DB
		private Quaternion CalculateGizmoRotation()
		{
			return Quaternion.identity;
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x001472E2 File Offset: 0x001454E2
		private float CalculateCubeSideLength()
		{
			return 0.0075f;
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x001472E9 File Offset: 0x001454E9
		private float CalculateAxisConeLength()
		{
			return 1.45f * this.CalculateCubeSideLength();
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x001472F7 File Offset: 0x001454F7
		private float CalculateAxisConeRadius()
		{
			return this.CalculateCubeSideLength() * 0.5f;
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x00147305 File Offset: 0x00145505
		private float CalculateAxisLabelSquareSize()
		{
			return this.CalculateAxisConeRadius() * 1.5f;
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x00147313 File Offset: 0x00145513
		private float CalculateAxisLabelOffsetFromCone()
		{
			return 0.003f;
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x0014731A File Offset: 0x0014551A
		private IEnumerator DoComponentAlphaTransitions()
		{
			int numberOfAxes = Enum.GetValues(typeof(SceneGizmoComponent)).Length;
			Vector3[] gizmoAxes = new Vector3[]
			{
				this._gizmoTransform.right,
				-this._gizmoTransform.right,
				this._gizmoTransform.up,
				-this._gizmoTransform.up,
				this._gizmoTransform.forward,
				-this._gizmoTransform.forward
			};
			for (;;)
			{
				Vector3 forward = this._gizmoCameraTransform.forward;
				for (int i = 0; i < numberOfAxes; i++)
				{
					if (i == 0)
					{
						this._componentAlphas[i] = 1f;
					}
					else
					{
						SceneGizmo.GizmoCompAlphaFadeInfo gizmoCompAlphaFadeInfo = this._componentAlphaFadeInfo[i];
						float num = Mathf.Abs(Vector3.Dot(forward, gizmoAxes[i - 1]));
						if (!gizmoCompAlphaFadeInfo.IsActive)
						{
							if (num >= 0.89f)
							{
								gizmoCompAlphaFadeInfo.ChangeTransition(SceneGizmo.GizmoCompTransition.FadeOut, this._componentAlphas[i]);
							}
							else
							{
								gizmoCompAlphaFadeInfo.ChangeTransition(SceneGizmo.GizmoCompTransition.FadeIn, this._componentAlphas[i]);
							}
						}
						if (gizmoCompAlphaFadeInfo.IsActive)
						{
							this._componentAlphas[i] = Mathf.Lerp(gizmoCompAlphaFadeInfo.SrcAlpha, gizmoCompAlphaFadeInfo.DestAlpha, gizmoCompAlphaFadeInfo.ElapsedTime / 0.189f);
							gizmoCompAlphaFadeInfo.ElapsedTime += Time.deltaTime;
							if (gizmoCompAlphaFadeInfo.ElapsedTime >= 0.189f)
							{
								this._componentAlphas[i] = gizmoCompAlphaFadeInfo.DestAlpha;
								gizmoCompAlphaFadeInfo.Stop();
							}
						}
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04001F51 RID: 8017
		private Camera _gizmoCamera;

		// Token: 0x04001F52 RID: 8018
		private Transform _gizmoCameraTransform;

		// Token: 0x04001F53 RID: 8019
		private Transform _gizmoTransform;

		// Token: 0x04001F54 RID: 8020
		[SerializeField]
		private SceneGizmoCorner _corner = SceneGizmoCorner.TopRight;

		// Token: 0x04001F55 RID: 8021
		[SerializeField]
		private Color _negativeAxisColor = SceneGizmo.DefaultNegativeAxisColor;

		// Token: 0x04001F56 RID: 8022
		[SerializeField]
		private Color _xAxisColor = SceneGizmo.DefaultXAxisColor;

		// Token: 0x04001F57 RID: 8023
		[SerializeField]
		private Color _yAxisColor = SceneGizmo.DefaultYAxisColor;

		// Token: 0x04001F58 RID: 8024
		[SerializeField]
		private Color _zAxisColor = SceneGizmo.DefaultZAxisColor;

		// Token: 0x04001F59 RID: 8025
		[SerializeField]
		private Color _cubeColor = SceneGizmo.DefaultCubeColor;

		// Token: 0x04001F5A RID: 8026
		[SerializeField]
		private Color _hoveredComponentColor = SceneGizmo.DefaultHoveredComponentColor;

		// Token: 0x04001F5B RID: 8027
		[SerializeField]
		private float _cameraAlignDuration = 0.3f;

		// Token: 0x04001F5C RID: 8028
		private Texture2D[] _axisLabelTextures = new Texture2D[3];

		// Token: 0x04001F5D RID: 8029
		private float _screenSize = SceneGizmo.DefaultScreenSize;

		// Token: 0x04001F5E RID: 8030
		private int _hoveredComponent = -1;

		// Token: 0x04001F5F RID: 8031
		private Color[] _componentColors = new Color[Enum.GetValues(typeof(SceneGizmoComponent)).Length];

		// Token: 0x04001F60 RID: 8032
		private float[] _componentAlphas = new float[Enum.GetValues(typeof(SceneGizmoComponent)).Length];

		// Token: 0x04001F61 RID: 8033
		private SceneGizmo.GizmoCompAlphaFadeInfo[] _componentAlphaFadeInfo = new SceneGizmo.GizmoCompAlphaFadeInfo[Enum.GetValues(typeof(SceneGizmoComponent)).Length];

		// Token: 0x020007FC RID: 2044
		private enum GizmoCompTransition
		{
			// Token: 0x04002DE9 RID: 11753
			FadeIn,
			// Token: 0x04002DEA RID: 11754
			FadeOut
		}

		// Token: 0x020007FD RID: 2045
		private class GizmoCompAlphaFadeInfo
		{
			// Token: 0x17000860 RID: 2144
			// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0018381B File Offset: 0x00181A1B
			public bool IsActive
			{
				get
				{
					return this._isActive;
				}
			}

			// Token: 0x17000861 RID: 2145
			// (get) Token: 0x060040A6 RID: 16550 RVA: 0x00183823 File Offset: 0x00181A23
			// (set) Token: 0x060040A7 RID: 16551 RVA: 0x0018382B File Offset: 0x00181A2B
			public float ElapsedTime
			{
				get
				{
					return this._elapsedTime;
				}
				set
				{
					this._elapsedTime = value;
				}
			}

			// Token: 0x17000862 RID: 2146
			// (get) Token: 0x060040A8 RID: 16552 RVA: 0x00183834 File Offset: 0x00181A34
			public float SrcAlpha
			{
				get
				{
					return this._srcAlpha;
				}
			}

			// Token: 0x17000863 RID: 2147
			// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0018383C File Offset: 0x00181A3C
			public float DestAlpha
			{
				get
				{
					return this._destAlpha;
				}
			}

			// Token: 0x17000864 RID: 2148
			// (get) Token: 0x060040AA RID: 16554 RVA: 0x00183844 File Offset: 0x00181A44
			public SceneGizmo.GizmoCompTransition Transition
			{
				get
				{
					return this._transition;
				}
			}

			// Token: 0x060040AB RID: 16555 RVA: 0x0018384C File Offset: 0x00181A4C
			public void ChangeTransition(SceneGizmo.GizmoCompTransition newTransition, float srcAlpha)
			{
				if (this._transition != newTransition)
				{
					this._transition = newTransition;
					this._srcAlpha = srcAlpha;
					this._destAlpha = ((newTransition == SceneGizmo.GizmoCompTransition.FadeIn) ? 1f : 0f);
					this._elapsedTime = 0f;
					this._isActive = true;
				}
			}

			// Token: 0x060040AC RID: 16556 RVA: 0x0018388C File Offset: 0x00181A8C
			public void Stop()
			{
				this._isActive = false;
			}

			// Token: 0x04002DEB RID: 11755
			private SceneGizmo.GizmoCompTransition _transition;

			// Token: 0x04002DEC RID: 11756
			private float _elapsedTime;

			// Token: 0x04002DED RID: 11757
			private bool _isActive;

			// Token: 0x04002DEE RID: 11758
			private float _srcAlpha;

			// Token: 0x04002DEF RID: 11759
			private float _destAlpha;
		}
	}
}
