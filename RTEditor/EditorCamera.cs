using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003B7 RID: 951
	public class EditorCamera : MonoSingletonBase<EditorCamera>
	{
		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinRotationSpeedInDegrees
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002CEA RID: 11498 RVA: 0x0013A337 File Offset: 0x00138537
		public EditorCameraZoomSettings ZoomSettings
		{
			get
			{
				return this._zoomSettings;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002CEB RID: 11499 RVA: 0x0013A33F File Offset: 0x0013853F
		public EditorCameraPanSettings PanSettings
		{
			get
			{
				return this._panSettings;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06002CEC RID: 11500 RVA: 0x0013A347 File Offset: 0x00138547
		public EditorCameraFocusSettings FocusSettings
		{
			get
			{
				return this._focusSettings;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06002CED RID: 11501 RVA: 0x0013A34F File Offset: 0x0013854F
		public EditorCameraMoveSettings MoveSettings
		{
			get
			{
				return this._moveSettings;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06002CEE RID: 11502 RVA: 0x0013A357 File Offset: 0x00138557
		// (set) Token: 0x06002CEF RID: 11503 RVA: 0x0013A35F File Offset: 0x0013855F
		public float RotationSpeedInDegrees
		{
			get
			{
				return this._rotationSpeedInDegrees;
			}
			set
			{
				this._rotationSpeedInDegrees = Mathf.Max(value, EditorCamera.MinRotationSpeedInDegrees);
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x0013A372 File Offset: 0x00138572
		public EditorCameraBk Background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06002CF1 RID: 11505 RVA: 0x0013A37A File Offset: 0x0013857A
		public Vector3 LastFocusPoint
		{
			get
			{
				return this._lastFocusPoint;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x0013A382 File Offset: 0x00138582
		public Camera Camera
		{
			get
			{
				if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseCustomCamera)
				{
					return MonoSingletonBase<RuntimeEditorApplication>.Instance.CustomCamera;
				}
				return this._camera;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x0013A3A1 File Offset: 0x001385A1
		public ShortcutKeys MoveForwardShortcut
		{
			get
			{
				return this._moveForwardShortcut;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x0013A3A9 File Offset: 0x001385A9
		public ShortcutKeys MoveBackShortcut
		{
			get
			{
				return this._moveBackShortcut;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x0013A3B1 File Offset: 0x001385B1
		public ShortcutKeys StrafeLeftShortcut
		{
			get
			{
				return this._strafeLeftShortcut;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x0013A3B9 File Offset: 0x001385B9
		public ShortcutKeys StrafeRightShortcut
		{
			get
			{
				return this._strafeRightShortcut;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x0013A3C1 File Offset: 0x001385C1
		public ShortcutKeys MoveUpShortcut
		{
			get
			{
				return this._moveUpShortcut;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x0013A3C9 File Offset: 0x001385C9
		public ShortcutKeys MoveDownShortcut
		{
			get
			{
				return this._moveDownShortcut;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x0013A3D1 File Offset: 0x001385D1
		public ShortcutKeys FocusOnSelectionShortcut
		{
			get
			{
				return this._focusOnSelectionShortcut;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06002CFA RID: 11514 RVA: 0x0013A3D9 File Offset: 0x001385D9
		public ShortcutKeys CameraLookAroundShortcut
		{
			get
			{
				return this._cameraLookAroundShortcut;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06002CFB RID: 11515 RVA: 0x0013A3E1 File Offset: 0x001385E1
		public ShortcutKeys CameraOrbitShortcut
		{
			get
			{
				return this._cameraOrbitShortcut;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06002CFC RID: 11516 RVA: 0x0013A3E9 File Offset: 0x001385E9
		public ShortcutKeys CameraPanShortcut
		{
			get
			{
				return this._cameraPanShortcut;
			}
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x0013A3F1 File Offset: 0x001385F1
		public void SetOrtho(bool isOrtho)
		{
			this.Camera.orthographic = isOrtho;
			this.SetObjectVisibilityDirty();
			this._background.OnCameraUpdate(this.Camera, this._isDoingPerspectiveSwitch);
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x0013A41C File Offset: 0x0013861C
		public void SetOrthoSize(float size)
		{
			this.Camera.orthographicSize = size;
			this.SetObjectVisibilityDirty();
			this._background.OnCameraUpdate(this.Camera, this._isDoingPerspectiveSwitch);
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x0013A447 File Offset: 0x00138647
		public void SetObjectVisibilityDirty()
		{
			this._isObjectVisibilityDirty = true;
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x0013A450 File Offset: 0x00138650
		public void AdjustObjectVisibility(GameObject gameObject)
		{
			if (this.Camera.IsGameObjectVisible(gameObject))
			{
				this._visibleGameObjects.Add(gameObject);
				return;
			}
			this._visibleGameObjects.Remove(gameObject);
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x0013A47C File Offset: 0x0013867C
		public List<GameObject> GetVisibleGameObjects()
		{
			if (this._isObjectVisibilityDirty)
			{
				this._visibleGameObjects = new HashSet<GameObject>(this.Camera.GetVisibleGameObjects());
				this._isObjectVisibilityDirty = false;
			}
			this._visibleGameObjects.RemoveWhere((GameObject item) => item == null);
			return new List<GameObject>(this._visibleGameObjects);
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x0013A4E4 File Offset: 0x001386E4
		public void AlignLookWithWorldAxis(Axis worldAxis, bool negativeAxis, float duration)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.StartAlignLookWithWorldAxis(worldAxis, negativeAxis, duration));
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x0013A4FC File Offset: 0x001386FC
		public void ChangePerspective(float duration)
		{
			if (this._isDoingPerspectiveSwitch)
			{
				return;
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.StartPerspectiveChange(duration));
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x0013A51C File Offset: 0x0013871C
		public void FocusOnSelection()
		{
			if (MonoSingletonBase<EditorObjectSelection>.Instance.NumberOfSelectedObjects == 0)
			{
				return;
			}
			if (this._focusSettings.FocusMode == EditorCameraFocusMode.Instant)
			{
				EditorCameraFocusOperationInfo focusOperationInfo = EditorCameraFocus.GetFocusOperationInfo(this.Camera, this._focusSettings);
				this.Camera.orthographicSize = focusOperationInfo.OrthoCameraHalfVerticalSize;
				this.Camera.transform.position = focusOperationInfo.CameraDestinationPosition;
				this._wasFocused = true;
				this._lastFocusPoint = focusOperationInfo.FocusPoint;
				this.CalculateOrbitOffsetAlongLook(focusOperationInfo);
				return;
			}
			if (this._focusSettings.FocusMode == EditorCameraFocusMode.ConstantSpeed)
			{
				base.StopCoroutine("StartConstantFocusOnSelection");
				base.StopCoroutine("StartSmoothZoom");
				base.StopCoroutine("StartSmoothPan");
				base.StartCoroutine("StartConstantFocusOnSelection");
				return;
			}
			if (this._focusSettings.FocusMode == EditorCameraFocusMode.Smooth)
			{
				base.StopCoroutine("StartSmoothFocusOnSelection");
				base.StopCoroutine("StartSmoothZoom");
				base.StopCoroutine("StartSmoothPan");
				base.StartCoroutine("StartSmoothFocusOnSelection");
			}
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x0013A60E File Offset: 0x0013880E
		private void SetProjectionMatrix(Matrix4x4 projectionMatrix)
		{
			this.Camera.projectionMatrix = projectionMatrix;
			this.SetObjectVisibilityDirty();
			this._background.OnCameraUpdate(this.Camera, this._isDoingPerspectiveSwitch);
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x0013A63C File Offset: 0x0013883C
		private void Awake()
		{
			this._camera = base.gameObject.GetComponent<Camera>();
			if (this._camera == null)
			{
				this._camera = base.gameObject.AddComponent<Camera>();
			}
			this._transform = base.transform;
			this._initialNearClipPlane = this.Camera.nearClipPlane;
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x0013A698 File Offset: 0x00138898
		private void Update()
		{
			if (MonoSingletonBase<RuntimeEditorApplication>.Instance.UseCustomCamera)
			{
				this._camera.enabled = false;
			}
			if (!PlayerScript.Pointer || !Pointer.IsGizmoTool(PlayerScript.PointerScript.CurrentPointerMode))
			{
				return;
			}
			if (this._applicationJustGainedFocus)
			{
				this._applicationJustGainedFocus = false;
				this._mouse.ResetCursorPositionInPreviousFrame();
			}
			this._mouse.UpdateInfoForCurrentFrame();
			if (!MonoSingletonBase<RuntimeEditorApplication>.Instance.UseCustomCamera)
			{
				if (this._focusOnSelectionShortcut.IsActiveInCurrentFrame())
				{
					this.FocusOnSelection();
				}
				if (this._zoomSettings.IsZoomEnabled)
				{
					this.ApplyCameraZoomBasedOnUserInput();
				}
				this.PanCameraBasedOnUserInput();
				this.RotateCameraBasedOnUserInput();
				this.MoveCameraBasedOnUserInput();
			}
			if (this.Camera.orthographic)
			{
				this.Camera.nearClipPlane = 1E-06f;
				CameraViewVolume viewVolume = this.Camera.GetViewVolume();
				List<Vector3> list = new List<Vector3>();
				list.Add(viewVolume.TopLeftPointOnNearPlane);
				list.Add(viewVolume.TopRightPointOnNearPlane);
				list.Add(viewVolume.BottomLeftPointOnNearPlane);
				list.Add(viewVolume.BottomRightPointOnNearPlane);
				Plane plane = MonoSingletonBase<RuntimeEditorApplication>.Instance.XZGrid.Plane;
				if (!plane.AreAllPointsInFrontOrBehindPlane(list))
				{
					Vector3 zero = Vector3.zero;
					float num = Vector3.Dot(plane.normal, this.Camera.transform.forward);
					if (num > 0f)
					{
						plane.GetFurthestPointInFront(list, out zero);
					}
					else if (num < 0f)
					{
						plane.GetFurthestPointBehind(list, out zero);
					}
					if (num != 0f)
					{
						Ray ray = new Ray(zero, -this.Camera.transform.forward);
						float distance;
						if (plane.Raycast(ray, out distance))
						{
							float magnitude = (ray.GetPoint(distance) - zero).magnitude;
							this.Camera.nearClipPlane -= magnitude;
						}
					}
				}
			}
			else
			{
				this.Camera.nearClipPlane = this._initialNearClipPlane;
			}
			this._background.OnCameraUpdate(this.Camera, this._isDoingPerspectiveSwitch);
			if (this._transform.hasChanged)
			{
				this.SetObjectVisibilityDirty();
				this._transform.hasChanged = false;
			}
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x0013A8B8 File Offset: 0x00138AB8
		private void ApplyCameraZoomBasedOnUserInput()
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis != 0f)
			{
				base.StopAllCoroutines();
				if (this._zoomSettings.ZoomMode == EditorCameraZoomMode.Standard)
				{
					float num = this.Camera.orthographic ? this._zoomSettings.OrthographicStandardZoomSpeed : (this._zoomSettings.PerspectiveStandardZoomSpeed * Time.deltaTime);
					num *= this.CalculateZoomFactor();
					EditorCameraZoom.ZoomCamera(this.Camera, axis * num);
					this.SetOrthoSize(this.Camera.orthographicSize);
					return;
				}
				base.StopAllCoroutines();
				base.StartCoroutine("StartSmoothZoom");
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x0013A954 File Offset: 0x00138B54
		private void PanCameraBasedOnUserInput()
		{
			if (this._mouse.WasMouseMovedSinceLastFrame && this._cameraPanShortcut.IsActive())
			{
				base.StopAllCoroutines();
				if (this._panSettings.PanMode == EditorCameraPanMode.Standard)
				{
					float num = Time.deltaTime * this._panSettings.StandardPanSpeed;
					EditorCameraPan.PanCamera(this.Camera, -this._mouse.CursorOffsetSinceLastFrame.x * num * (this._panSettings.InvertXAxis ? -1f : 1f), -this._mouse.CursorOffsetSinceLastFrame.y * num * (this._panSettings.InvertYAxis ? -1f : 1f));
					return;
				}
				base.StartCoroutine(this.StartSmoothPan());
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x0013AA1C File Offset: 0x00138C1C
		private void RotateCameraBasedOnUserInput()
		{
			if (!this._mouse.WasMouseMovedSinceLastFrame)
			{
				return;
			}
			bool flag = this._cameraOrbitShortcut.IsActive();
			bool flag2 = !flag && this._cameraLookAroundShortcut.IsActive();
			if (flag || flag2)
			{
				base.StopAllCoroutines();
				float num = this._rotationSpeedInDegrees * Time.deltaTime;
				if (flag2 || !this._wasFocused)
				{
					EditorCameraRotation.RotateCamera(this.Camera, -this._mouse.CursorOffsetSinceLastFrame.y * num, this._mouse.CursorOffsetSinceLastFrame.x * num);
					return;
				}
				if (this._wasFocused && flag)
				{
					Transform transform = this.Camera.transform;
					Vector3 orbitPoint = transform.position + transform.forward * this._orbitOffsetAlongLook;
					EditorCameraOrbit.OrbitCamera(this.Camera, -this._mouse.CursorOffsetSinceLastFrame.y * num, this._mouse.CursorOffsetSinceLastFrame.x * num, orbitPoint);
				}
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x0013AB14 File Offset: 0x00138D14
		private void MoveCameraBasedOnUserInput()
		{
			float num = this._moveSettings.MoveSpeed * Time.deltaTime;
			num *= this.CalculateZoomFactor();
			Transform transform = this.Camera.transform;
			if (this._moveForwardShortcut.IsActive())
			{
				if (this.Camera.orthographic)
				{
					EditorCameraZoom.ZoomCamera(this.Camera, num);
				}
				else
				{
					transform.position += transform.forward * num;
				}
			}
			else if (this._moveBackShortcut.IsActive())
			{
				if (this.Camera.orthographic)
				{
					EditorCameraZoom.ZoomCamera(this.Camera, -num);
				}
				else
				{
					transform.position -= transform.forward * num;
				}
			}
			if (this._strafeLeftShortcut.IsActive())
			{
				transform.position -= transform.right * num;
			}
			else if (this._strafeRightShortcut.IsActive())
			{
				transform.position += transform.right * num;
			}
			if (this._moveDownShortcut.IsActive())
			{
				transform.position -= transform.up * num;
			}
			else if (this._moveUpShortcut.IsActive())
			{
				transform.position += transform.up * num;
			}
			this.SetOrthoSize(this.Camera.orthographicSize);
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x0013AC94 File Offset: 0x00138E94
		private float CalculateZoomFactor()
		{
			float distanceToPoint = MonoSingletonBase<RuntimeEditorApplication>.Instance.XZGrid.Plane.GetDistanceToPoint(base.transform.position);
			return Mathf.Max(1f, distanceToPoint / 10f);
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x0013ACD5 File Offset: 0x00138ED5
		private void OnApplicationFocus(bool focusStatus)
		{
			if (focusStatus)
			{
				this._applicationJustGainedFocus = true;
			}
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x0013ACE4 File Offset: 0x00138EE4
		private void CalculateOrbitOffsetAlongLook(EditorCameraFocusOperationInfo focusOpInfo)
		{
			this._orbitOffsetAlongLook = (this.Camera.transform.position - focusOpInfo.FocusPoint).magnitude;
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x0013AD1A File Offset: 0x00138F1A
		private IEnumerator StartSmoothZoom()
		{
			float currentSpeed = (this.Camera.orthographic ? this._zoomSettings.OrthographicSmoothZoomSpeed : this._zoomSettings.PerspectiveSmoothZoomSpeed) * Input.GetAxis("Mouse ScrollWheel");
			float smoothValue = this.Camera.orthographic ? this._zoomSettings.OrthographicSmoothValue : this._zoomSettings.PerspectiveSmoothValue;
			currentSpeed *= this.CalculateZoomFactor();
			for (;;)
			{
				EditorCameraZoom.ZoomCamera(this.Camera, currentSpeed * Time.deltaTime);
				this.SetOrthoSize(this.Camera.orthographicSize);
				currentSpeed = Mathf.Lerp(currentSpeed, 0f, smoothValue);
				if (Mathf.Abs(currentSpeed) < 1E-05f)
				{
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x0013AD29 File Offset: 0x00138F29
		private IEnumerator StartSmoothPan()
		{
			float panSpeedRightAxis = -this._mouse.CursorOffsetSinceLastFrame.x * this._panSettings.SmoothPanSpeed * (this._panSettings.InvertXAxis ? -1f : 1f);
			float panSpeedUpAxis = -this._mouse.CursorOffsetSinceLastFrame.y * this._panSettings.SmoothPanSpeed * (this._panSettings.InvertYAxis ? -1f : 1f);
			float smoothValue = this._panSettings.SmoothValue;
			for (;;)
			{
				EditorCameraPan.PanCamera(this.Camera, panSpeedRightAxis * Time.deltaTime, panSpeedUpAxis * Time.deltaTime);
				panSpeedRightAxis = Mathf.Lerp(panSpeedRightAxis, 0f, smoothValue);
				panSpeedUpAxis = Mathf.Lerp(panSpeedUpAxis, 0f, smoothValue);
				if (Mathf.Abs(panSpeedRightAxis) < 1E-05f && Mathf.Abs(panSpeedUpAxis) < 1E-05f)
				{
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x0013AD38 File Offset: 0x00138F38
		private IEnumerator StartConstantFocusOnSelection()
		{
			EditorCameraFocusOperationInfo focusOpInfo = EditorCameraFocus.GetFocusOperationInfo(this.Camera, this._focusSettings);
			this._lastFocusPoint = focusOpInfo.FocusPoint;
			Vector3 cameraDestinationPoint = focusOpInfo.CameraDestinationPosition;
			float cameraSpeed = this._focusSettings.ConstantFocusSpeed;
			Transform cameraTransform = this.Camera.transform;
			Vector3 fromCamPosToDestination = cameraDestinationPoint - cameraTransform.position;
			float distanceToTravel = fromCamPosToDestination.magnitude;
			if (distanceToTravel < 0.0001f)
			{
				yield break;
			}
			fromCamPosToDestination.Normalize();
			Vector3 initialCameraPosition = cameraTransform.position;
			float initialCameraOrthoSize = this.Camera.orthographicSize;
			this._wasFocused = true;
			for (;;)
			{
				cameraTransform.position += fromCamPosToDestination * cameraSpeed * Time.deltaTime;
				float magnitude = (cameraTransform.position - initialCameraPosition).magnitude;
				this.SetOrthoSize(Mathf.Lerp(initialCameraOrthoSize, focusOpInfo.OrthoCameraHalfVerticalSize, magnitude / distanceToTravel));
				this.CalculateOrbitOffsetAlongLook(focusOpInfo);
				if (Vector3.Dot(fromCamPosToDestination, cameraDestinationPoint - cameraTransform.position) <= 0f && Mathf.Abs(this.Camera.orthographicSize - focusOpInfo.OrthoCameraHalfVerticalSize) < 0.001f)
				{
					break;
				}
				yield return null;
			}
			cameraTransform.position = cameraDestinationPoint;
			yield break;
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x0013AD47 File Offset: 0x00138F47
		private IEnumerator StartSmoothFocusOnSelection()
		{
			EditorCameraFocusOperationInfo focusOpInfo = EditorCameraFocus.GetFocusOperationInfo(this.Camera, this._focusSettings);
			this._lastFocusPoint = focusOpInfo.FocusPoint;
			Vector3 cameraDestinationPoint = focusOpInfo.CameraDestinationPosition;
			Transform cameraTransform = this.Camera.transform;
			if ((cameraDestinationPoint - cameraTransform.position).magnitude < 0.0001f)
			{
				yield break;
			}
			Vector3 velocity = Vector3.zero;
			float orthoSizeVelocity = 0f;
			this._wasFocused = true;
			for (;;)
			{
				cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, cameraDestinationPoint, ref velocity, this._focusSettings.SmoothFocusTime);
				this.SetOrthoSize(Mathf.SmoothDamp(this.Camera.orthographicSize, focusOpInfo.OrthoCameraHalfVerticalSize, ref orthoSizeVelocity, this._focusSettings.SmoothFocusTime));
				this.CalculateOrbitOffsetAlongLook(focusOpInfo);
				if ((cameraTransform.position - cameraDestinationPoint).magnitude < 0.001f && Mathf.Abs(this.Camera.orthographicSize - focusOpInfo.OrthoCameraHalfVerticalSize) < 0.001f)
				{
					break;
				}
				yield return null;
			}
			cameraTransform.position = cameraDestinationPoint;
			this.Camera.orthographicSize = focusOpInfo.OrthoCameraHalfVerticalSize;
			yield break;
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x0013AD56 File Offset: 0x00138F56
		private IEnumerator StartAlignLookWithWorldAxis(Axis worldAxis, bool negativeAxis, float duration)
		{
			Vector3 vector = Vector3.right;
			if (worldAxis == Axis.Y)
			{
				vector = Vector3.up;
			}
			else if (worldAxis == Axis.Z)
			{
				vector = Vector3.forward;
			}
			if (negativeAxis)
			{
				vector *= -1f;
			}
			float elapsedTime = 0f;
			Quaternion desiredRotation = Quaternion.LookRotation(vector);
			Quaternion initialRotation = this.Camera.transform.rotation;
			while (elapsedTime <= duration)
			{
				this.Camera.transform.rotation = Quaternion.Slerp(initialRotation, desiredRotation, elapsedTime / duration);
				elapsedTime += Time.deltaTime;
				this._background.OnCameraUpdate(this.Camera, this._isDoingPerspectiveSwitch);
				yield return null;
			}
			this.Camera.transform.rotation = desiredRotation;
			yield break;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x0013AD7A File Offset: 0x00138F7A
		private IEnumerator StartPerspectiveChange(float duration)
		{
			this.Camera.ResetProjectionMatrix();
			Matrix4x4 initialProjectionMatrix = this.Camera.projectionMatrix;
			bool willBeOrtho = !this.Camera.orthographic;
			Matrix4x4 desiredProjectionMatrix;
			if (willBeOrtho)
			{
				float orthographicSize = this.Camera.orthographicSize;
				float num = orthographicSize * this.Camera.aspect;
				desiredProjectionMatrix = Matrix4x4.Ortho(-num, num, -orthographicSize, orthographicSize, this.Camera.nearClipPlane, this.Camera.farClipPlane);
			}
			else
			{
				desiredProjectionMatrix = Matrix4x4.Perspective(this.Camera.fieldOfView, this.Camera.aspect, this.Camera.nearClipPlane, this.Camera.farClipPlane);
			}
			float elapsedTime = 0f;
			this._isDoingPerspectiveSwitch = true;
			while (elapsedTime <= duration)
			{
				float t = elapsedTime / duration;
				this.SetProjectionMatrix(Matrix4x4Extensions.Lerp(initialProjectionMatrix, desiredProjectionMatrix, t));
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			this.SetOrtho(willBeOrtho);
			this.Camera.ResetProjectionMatrix();
			this._isDoingPerspectiveSwitch = false;
			this._background.OnCameraUpdate(this.Camera, this._isDoingPerspectiveSwitch);
			yield break;
		}

		// Token: 0x04001E1F RID: 7711
		[SerializeField]
		private ShortcutKeys _moveForwardShortcut = new ShortcutKeys("Move forward", 1)
		{
			Key0 = KeyCode.W,
			UseModifiers = false,
			RMouseButton = true
		};

		// Token: 0x04001E20 RID: 7712
		[SerializeField]
		private ShortcutKeys _moveBackShortcut = new ShortcutKeys("Move back", 1)
		{
			Key0 = KeyCode.S,
			UseModifiers = false,
			RMouseButton = true
		};

		// Token: 0x04001E21 RID: 7713
		[SerializeField]
		private ShortcutKeys _strafeLeftShortcut = new ShortcutKeys("Strafe left", 1)
		{
			Key0 = KeyCode.A,
			UseModifiers = false,
			RMouseButton = true
		};

		// Token: 0x04001E22 RID: 7714
		[SerializeField]
		private ShortcutKeys _strafeRightShortcut = new ShortcutKeys("Strafe right", 1)
		{
			Key0 = KeyCode.D,
			UseModifiers = false,
			RMouseButton = true
		};

		// Token: 0x04001E23 RID: 7715
		[SerializeField]
		private ShortcutKeys _moveUpShortcut = new ShortcutKeys("Move up", 1)
		{
			Key0 = KeyCode.E,
			UseModifiers = false,
			RMouseButton = true
		};

		// Token: 0x04001E24 RID: 7716
		[SerializeField]
		private ShortcutKeys _moveDownShortcut = new ShortcutKeys("Move down", 1)
		{
			Key0 = KeyCode.Q,
			UseModifiers = false,
			RMouseButton = true
		};

		// Token: 0x04001E25 RID: 7717
		[SerializeField]
		private ShortcutKeys _focusOnSelectionShortcut = new ShortcutKeys("Focus on selection", 1)
		{
			Key0 = KeyCode.F,
			UseModifiers = false,
			UseMouseButtons = false
		};

		// Token: 0x04001E26 RID: 7718
		[SerializeField]
		private ShortcutKeys _cameraLookAroundShortcut = new ShortcutKeys("Camera look around", 0)
		{
			UseModifiers = false,
			RMouseButton = true
		};

		// Token: 0x04001E27 RID: 7719
		[SerializeField]
		private ShortcutKeys _cameraOrbitShortcut = new ShortcutKeys("Camera orbit", 0)
		{
			LAlt = true,
			RMouseButton = true
		};

		// Token: 0x04001E28 RID: 7720
		[SerializeField]
		private ShortcutKeys _cameraPanShortcut = new ShortcutKeys("Camera pan", 0)
		{
			UseModifiers = false,
			MMouseButton = true
		};

		// Token: 0x04001E29 RID: 7721
		[SerializeField]
		private EditorCameraZoomSettings _zoomSettings = new EditorCameraZoomSettings();

		// Token: 0x04001E2A RID: 7722
		[SerializeField]
		private EditorCameraPanSettings _panSettings = new EditorCameraPanSettings();

		// Token: 0x04001E2B RID: 7723
		[SerializeField]
		private EditorCameraFocusSettings _focusSettings = new EditorCameraFocusSettings();

		// Token: 0x04001E2C RID: 7724
		[SerializeField]
		private EditorCameraMoveSettings _moveSettings = new EditorCameraMoveSettings();

		// Token: 0x04001E2D RID: 7725
		[SerializeField]
		private float _rotationSpeedInDegrees = 8.8f;

		// Token: 0x04001E2E RID: 7726
		private Camera _camera;

		// Token: 0x04001E2F RID: 7727
		private Transform _transform;

		// Token: 0x04001E30 RID: 7728
		private Mouse _mouse = new Mouse();

		// Token: 0x04001E31 RID: 7729
		private bool _wasFocused;

		// Token: 0x04001E32 RID: 7730
		private Vector3 _lastFocusPoint = Vector3.zero;

		// Token: 0x04001E33 RID: 7731
		private float _orbitOffsetAlongLook;

		// Token: 0x04001E34 RID: 7732
		private bool _applicationJustGainedFocus;

		// Token: 0x04001E35 RID: 7733
		private float _initialNearClipPlane;

		// Token: 0x04001E36 RID: 7734
		private bool _isDoingPerspectiveSwitch;

		// Token: 0x04001E37 RID: 7735
		private bool _isObjectVisibilityDirty = true;

		// Token: 0x04001E38 RID: 7736
		private HashSet<GameObject> _visibleGameObjects = new HashSet<GameObject>();

		// Token: 0x04001E39 RID: 7737
		[SerializeField]
		private EditorCameraBk _background = new EditorCameraBk();
	}
}
