using System;
using System.Collections;
using NewNet;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class CameraController : Singleton<CameraController>
{
	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000882 RID: 2178 RVA: 0x0003BC70 File Offset: 0x00039E70
	// (set) Token: 0x06000883 RID: 2179 RVA: 0x0003BC78 File Offset: 0x00039E78
	public float distance
	{
		get
		{
			return this._distance;
		}
		set
		{
			this._distance = Mathf.Clamp(value, this.distanceMin, this.distanceMax);
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x0003BC92 File Offset: 0x00039E92
	private float MovementMulti
	{
		get
		{
			return Mathf.Max(0.01f, ConfigGame.Settings.ConfigCamera.MovementSpeed * 2f);
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x0003BCB4 File Offset: 0x00039EB4
	public float xLookMulti
	{
		get
		{
			float num = Mathf.Max(0.01f, ConfigGame.Settings.ConfigCamera.LookSpeed * 2f);
			if (ConfigGame.Settings.ConfigCamera.InvertHorizontal)
			{
				num *= -1f;
			}
			return num;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000886 RID: 2182 RVA: 0x0003BCFC File Offset: 0x00039EFC
	public float yLookMulti
	{
		get
		{
			float num = Mathf.Max(0.01f, ConfigGame.Settings.ConfigCamera.LookSpeed * 2f);
			if (ConfigGame.Settings.ConfigCamera.InvertVertical)
			{
				num *= -1f;
			}
			return num;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000887 RID: 2183 RVA: 0x0003BD43 File Offset: 0x00039F43
	// (set) Token: 0x06000888 RID: 2184 RVA: 0x0003BD4B File Offset: 0x00039F4B
	private bool bZoomed
	{
		get
		{
			return this._bZoomed;
		}
		set
		{
			this._bZoomed = value;
			if (value)
			{
				this.distanceMin = this.refdistanceMin / 2f;
				return;
			}
			this.distanceMin = this.refdistanceMin;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000889 RID: 2185 RVA: 0x0003BD76 File Offset: 0x00039F76
	public bool bThirdPerson
	{
		get
		{
			return this.CurrentMode == CameraController.CameraMode.ThirdPerson;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x0600088A RID: 2186 RVA: 0x0003BD81 File Offset: 0x00039F81
	public bool bFirstPerson
	{
		get
		{
			return this.CurrentMode == CameraController.CameraMode.FirstPerson;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x0003BD8C File Offset: 0x00039F8C
	public bool bTopDown
	{
		get
		{
			return this.CurrentMode == CameraController.CameraMode.TopDown;
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0003BD97 File Offset: 0x00039F97
	protected override void Awake()
	{
		base.Awake();
		this.camera = base.GetComponent<Camera>();
		base.GetComponent<UICamera>().useController = false;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0003BDB8 File Offset: 0x00039FB8
	private void Start()
	{
		this.target.position = new Vector3(0f, -2.5f, 0f);
		this.actualdistance = this.distance;
		this.x = 0f;
		this.y = 65f;
		this.reftargetpos = this.target.position;
		this.refdistance = this.distance;
		this.refdistanceMin = this.distanceMin;
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0003BE2F File Offset: 0x0003A02F
	public void Shake()
	{
		this.shake_intensity = 0.1f;
		this.shake_decay = 0.42000002f;
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0003BE48 File Offset: 0x0003A048
	private void Update()
	{
		if (Network.peerType == NetworkPeerMode.Disconnected)
		{
			base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y + 3f * Time.deltaTime, base.transform.eulerAngles.z);
			return;
		}
		if (UICamera.SelectIsInput())
		{
			return;
		}
		if (this.bLuaAttached)
		{
			if (!this.LuaAttachment)
			{
				this.bLuaAttached = false;
				this.bLuaAttachmentRotate = false;
				return;
			}
			this.target.position = this.LuaAttachment.transform.localPosition + this.LuaAttachmentOffset;
			if (this.bLuaAttachmentRotate)
			{
				this.Angle = this.LuaAttachment.transform.localEulerAngles.y;
				this.AngleTwo = this.LuaAttachment.transform.localEulerAngles.x;
			}
		}
		bool flag = false;
		float num = this.LOAD_ROTATION_RATE * Time.deltaTime;
		if (this.bRotateHorizontal)
		{
			float num2 = 1f;
			float num3 = Mathf.Abs(this.Angle - base.transform.localEulerAngles.y);
			float num4 = Mathf.Abs(this.Angle + 360f - base.transform.localEulerAngles.y);
			if (num4 < num || num3 < num)
			{
				this.bRotateHorizontal = false;
				float num5 = this.Angle;
				int num6 = 0;
				bool flag2 = false;
				while (Mathf.Abs(num5 - this.x) >= 180f)
				{
					num6++;
					if (num6 > 1000)
					{
						Debug.LogError("Something broke in camera while loop");
						flag2 = true;
						break;
					}
					if (num5 < this.x)
					{
						num5 += 360f;
					}
					else
					{
						num5 -= 360f;
					}
				}
				if (!flag2)
				{
					this.x = num5;
				}
				else
				{
					this.x = (float)Math.Round((double)this.x, 1);
					float num7 = this.x;
					int num8 = 0;
					while (num7 >= 360f)
					{
						num8++;
						num7 -= 360f;
					}
					while (num7 <= -360f)
					{
						num8--;
						num7 += 360f;
					}
					if (this.x < 0f && this.Angle > 0.01f)
					{
						this.x = (float)(num8 * 360) + this.Angle - 360f;
					}
					else
					{
						this.x = (float)(num8 * 360) + this.Angle;
					}
				}
				if (!this.bRotateVertical && this.bCachedFirstPerson)
				{
					this.StartFirstPerson();
				}
			}
			else
			{
				if (num3 < 20f)
				{
					num2 = Mathf.Max(0.5f * (num3 / 10f), 0.05f);
				}
				if (num4 < 20f)
				{
					num2 = Mathf.Max(0.5f * (num4 / 10f), 0.05f);
				}
				if (this.Angle - base.transform.localEulerAngles.y > 180f || base.transform.localEulerAngles.y - this.Angle > 180f)
				{
					if (this.Angle > base.transform.localEulerAngles.y)
					{
						this.x -= this.LOAD_ROTATION_RATE * Time.deltaTime * num2;
					}
					else
					{
						this.x += this.LOAD_ROTATION_RATE * Time.deltaTime * num2;
					}
				}
				else if (this.Angle > base.transform.localEulerAngles.y)
				{
					this.x += this.LOAD_ROTATION_RATE * Time.deltaTime * num2;
				}
				else
				{
					this.x -= this.LOAD_ROTATION_RATE * Time.deltaTime * num2;
				}
			}
		}
		else
		{
			if (!this.bFirstPerson)
			{
				this.x += zInput.GetAxis("Camera Horizontal", ControlType.All, false) * 100f * -1f * Time.deltaTime * this.xLookMulti;
				this.target.rotation = base.transform.rotation;
				float num9 = zInput.GetAxis("Move Horizontal", ControlType.All, false) * 1f * Time.deltaTime * this.MovementMulti * this.distance;
				this.target.position = new Vector3(this.target.position.x + num9 * this.target.right.normalized.x, this.target.position.y, this.target.position.z + num9 * this.target.right.normalized.z);
			}
			else
			{
				this.x -= zInput.GetAxis("Camera Horizontal", ControlType.All, false) * 100f * -1f * Time.deltaTime * this.xLookMulti;
				this.target.rotation = base.transform.rotation;
				if (!this.bLuaAttached)
				{
					this.target.Translate(Vector3.right * zInput.GetAxis("Move Horizontal", ControlType.All, false) * 20f * Time.deltaTime * this.MovementMulti);
				}
			}
			if (zInput.GetButton("Camera Hold Rotate", ControlType.All) && (!zInput.GetButton("Grab", ControlType.All) || !zInput.GetButton("Tap", ControlType.All)))
			{
				flag = true;
				this.x += Input.GetAxisRaw("Mouse X") * 100f * 0.01666f * 2f * this.xLookMulti;
			}
		}
		if (this.bRotateVertical)
		{
			if (this.bTopDown)
			{
				this.bRotateVertical = false;
				this.y = this.AngleTwo;
			}
			float num10 = 1f;
			float num11 = Mathf.Abs(this.AngleTwo - base.transform.localEulerAngles.x);
			if (num11 < num)
			{
				this.bRotateVertical = false;
				this.y = this.AngleTwo;
				if (!this.bRotateHorizontal && this.bCachedFirstPerson)
				{
					this.StartFirstPerson();
				}
			}
			else
			{
				if (num11 < 20f)
				{
					num10 = Mathf.Max(0.5f * (num11 / 10f), 0.2f);
				}
				if (this.y < this.AngleTwo)
				{
					this.y += this.LOAD_ROTATION_RATE * Time.deltaTime * num10;
				}
				else
				{
					this.y -= this.LOAD_ROTATION_RATE * Time.deltaTime * num10;
				}
			}
		}
		else
		{
			if (!this.bFirstPerson)
			{
				if (!this.bTopDown)
				{
					this.y -= zInput.GetAxis("Camera Vertical", ControlType.All, false) * 100f * -1f * Time.deltaTime * this.yLookMulti;
				}
				this.target.rotation = base.transform.rotation;
				float num12 = zInput.GetAxis("Move Vertical", ControlType.All, false) * 1f * -1f * Time.deltaTime * this.MovementMulti * this.distance;
				this.target.position = new Vector3(this.target.position.x + num12 * this.target.right.normalized.z, this.target.position.y, this.target.position.z + -num12 * this.target.right.normalized.x);
			}
			else
			{
				this.y += zInput.GetAxis("Camera Vertical", ControlType.All, false) * 100f * -1f * Time.deltaTime * this.yLookMulti;
				this.target.rotation = base.transform.rotation;
				if (!this.bLuaAttached)
				{
					this.target.Translate(Vector3.forward * zInput.GetAxis("Move Vertical", ControlType.All, false) * 20f * Time.deltaTime * this.MovementMulti);
				}
			}
			if (zInput.GetButton("Camera Hold Rotate", ControlType.All) && !zInput.GetButton("Grab", ControlType.All))
			{
				flag = true;
				if (!this.bTopDown)
				{
					this.y += Input.GetAxisRaw("Mouse Y") * 100f * -1f * 0.01666f * 2f * this.yLookMulti;
				}
			}
		}
		float min = 10f;
		if (this.target.position.y < -2f)
		{
			min = 10f * Mathf.Max(30f / this.distance, 1f);
		}
		if (!this.bFirstPerson)
		{
			this.y = CameraController.ClampAngle(this.y, min, 90f);
		}
		else
		{
			this.y = CameraController.ClampAngle(this.y, -90f, 90f);
		}
		if (flag)
		{
			this.xSmooth = this.x;
			this.ySmooth = this.y;
		}
		else
		{
			this.xSmooth = Mathf.Lerp(this.xSmooth, this.x, 0.3f);
			this.ySmooth = Mathf.Lerp(this.ySmooth, this.y, 0.3f);
		}
		if (this.bTopDown)
		{
			this.ySmooth = 90f;
		}
		Quaternion quaternion = Quaternion.Euler(this.ySmooth, this.xSmooth, 0f);
		if (zInput.GetAxis("Zoom", ControlType.All, false) != 0f && !zInput.GetButton("Grab", ControlType.All) && GUIUtility.hotControl == 0)
		{
			float distance = this.distance;
			if (zInput.GetButton("Alt", ControlType.All))
			{
				if (NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject && NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject.GetComponent<CustomPDF>())
				{
					float axis = zInput.GetAxis("Zoom", ControlType.All, false);
					if (Mathf.Abs(axis) > 0.01f)
					{
						if (axis < 0f)
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject.GetComponent<NetworkPhysicsObject>().customPDF.NextPage();
						}
						else
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject.GetComponent<NetworkPhysicsObject>().customPDF.PrevPage();
						}
					}
				}
				else
				{
					this.AltZoom = Mathf.Clamp(this.AltZoom + zInput.GetAxis("Zoom", ControlType.All, false) * 3f * this.MovementMulti / 144f / Time.deltaTime, 0.5f, 2f);
				}
			}
			else if (zInput.GetButton("Magnify", ControlType.All))
			{
				this.MagnifyZoom = Mathf.Clamp(this.MagnifyZoom + zInput.GetAxis("Zoom", ControlType.All, false) * 3f * this.MovementMulti / 144f / Time.deltaTime, 0.5f, 2f);
			}
			else if (!UICamera.HoverIsScroll())
			{
				if (zInput.GetAxis("Zoom", ControlType.Controller, false) != 0f)
				{
					this.distance -= zInput.GetAxis("Zoom", ControlType.All, false) * this.MovementMulti * 1f * this.distance / 144f / Time.deltaTime;
				}
				else if (Input.GetAxis("Mouse Wheel") != 0f)
				{
					this.distance -= zInput.GetAxis("Zoom", ControlType.All, false) * this.MovementMulti * 3f * this.distance / 144f / Time.deltaTime;
					if (CameraController.WHEEL_CENTER_ON_ZOOM && zInput.GetAxis("Zoom", ControlType.All, false) > 0f && this.zeroToCursorPosition == null)
					{
						Vector3 pointerPosition = HoverScript.PointerPosition;
						if (pointerPosition != Vector3.zero)
						{
							this.zeroToCursorPosition = new Vector3?(pointerPosition);
							this.zeroToCursorEnd = Time.time + 0.1f;
						}
					}
				}
				else
				{
					this.distance -= zInput.GetAxis("Zoom", ControlType.All, false) * this.MovementMulti * 0.033f * this.distance * 144f * Time.deltaTime;
				}
			}
			float distance2 = this.distance;
		}
		if (zInput.GetButtonDown("Pan", ControlType.All))
		{
			this.PanHeldTime = 0f;
			this.PanStartVector = HoverScript.PointerPosition;
		}
		if (zInput.GetButton("Pan", ControlType.All))
		{
			this.PanHeldTime += Time.deltaTime;
		}
		if (zInput.GetButton("Pan", ControlType.All) && (!zInput.GetButton("Grab", ControlType.All) || !zInput.GetButton("Flip", ControlType.All)) && !this.bLuaAttached)
		{
			float num13 = Input.GetAxisRaw("Mouse X") * -1.25f * 0.01666f * this.distance;
			float num14 = Input.GetAxisRaw("Mouse Y") * -1.25f * 0.01666f * this.distance;
			this.target.position = new Vector3(this.target.position.x + num13 * this.target.right.normalized.x - num14 * this.target.right.normalized.z, this.target.position.y, this.target.position.z + num13 * this.target.right.normalized.z + num14 * this.target.right.normalized.x);
		}
		if (zInput.GetButtonUp("Zoom Toggle", ControlType.All) && !zInput.GetButton("Grab", ControlType.All) && !this.bFirstPerson && !zInput.GetButton("Alt", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All) && (this.PanHeldTime == 0f || (Vector3.Distance(this.PanStartVector, HoverScript.PointerPosition) < HoverScript.DISTANCE_CHECK && this.PanHeldTime < 0.3f)) && !UICamera.HoveredUIObject)
		{
			if (!this.bZoomed)
			{
				Vector3 pointerPosition2 = HoverScript.PointerPosition;
				if (pointerPosition2 != Vector3.zero)
				{
					this.bZoomed = true;
					this.ZoomedDistance = this.distance;
					this.ZoomedPosition = this.target.position;
					this.distance *= 0.3f;
					this.target.position = pointerPosition2;
				}
			}
			else
			{
				this.bZoomed = false;
				this.target.position = this.ZoomedPosition;
				if (this.ZoomedDistance != 0f)
				{
					this.distance = this.ZoomedDistance;
				}
				else
				{
					this.distance = this.refdistance;
				}
			}
		}
		if (zInput.GetButtonUp("Pan", ControlType.All))
		{
			this.PanHeldTime = 0f;
		}
		if (zInput.GetButtonDown("Reset View", ControlType.All) && (!this.bFirstPerson || !zInput.GetButton("Fly Up", ControlType.All)))
		{
			this.ResetCameraRotation(false);
		}
		if (zInput.GetButton("Fly Up", ControlType.All) && this.bFirstPerson)
		{
			this.target.rotation = base.transform.rotation;
			this.target.position = new Vector3(this.target.position.x, this.target.position.y + 15f * Time.deltaTime * this.MovementMulti, this.target.position.z);
		}
		if (zInput.GetButton("Fly Down", ControlType.All) && this.bFirstPerson)
		{
			this.target.rotation = base.transform.rotation;
			this.target.position = new Vector3(this.target.position.x, this.target.position.y - 15f * Time.deltaTime * this.MovementMulti, this.target.position.z);
		}
		int num15 = (int)this.distanceMax;
		this.target.position = new Vector3(Mathf.Clamp(this.target.position.x, (float)(-(float)num15), (float)num15), Mathf.Clamp(this.target.position.y, (float)(-(float)num15), (float)num15), Mathf.Clamp(this.target.position.z, (float)(-(float)num15), (float)num15));
		this.actualdistance = Mathf.Lerp(this.actualdistance, this.distance, 15f * Time.deltaTime);
		if (this.zeroToCursorPosition != null)
		{
			if (Time.time >= this.zeroToCursorEnd)
			{
				this.zeroToCursorPosition = null;
			}
			else
			{
				Vector3 pointerPosition3 = HoverScript.PointerPosition;
				this.target.position += this.zeroToCursorPosition.Value - pointerPosition3;
			}
		}
		this.actualtarget = Vector3.Lerp(this.actualtarget, this.target.position, 15f * Time.deltaTime);
		this.camera.orthographicSize = this.actualdistance * 0.5f;
		this.renderTopCamera.orthographicSize = this.camera.orthographicSize;
		if (zInput.GetButtonDown("Camera Mode", ControlType.All))
		{
			this.bLuaAttached = false;
			this.bLuaAttachmentRotate = false;
			switch (this.CurrentMode)
			{
			case CameraController.CameraMode.ThirdPerson:
				this.StartCameraMode(CameraController.CameraMode.FirstPerson);
				break;
			case CameraController.CameraMode.FirstPerson:
				this.StartCameraMode(CameraController.CameraMode.TopDown);
				break;
			case CameraController.CameraMode.TopDown:
				this.StartCameraMode(CameraController.CameraMode.ThirdPerson);
				break;
			}
		}
		Vector3 point = new Vector3(0f, 0f, -this.actualdistance);
		Vector3 vector = Vector3.zero;
		if (this.bFirstPerson)
		{
			vector = this.actualtarget;
		}
		else
		{
			vector = quaternion * point + this.actualtarget;
		}
		this.CheckCollisionPenetrate(ref vector, ref quaternion);
		if (this.shake_intensity > 0f)
		{
			base.transform.position = vector + UnityEngine.Random.insideUnitSphere * this.shake_intensity;
			base.transform.rotation = new Quaternion(quaternion.x + UnityEngine.Random.Range(-this.shake_intensity, this.shake_intensity) * 0.2f, quaternion.y + UnityEngine.Random.Range(-this.shake_intensity, this.shake_intensity) * 0.2f, quaternion.z + UnityEngine.Random.Range(-this.shake_intensity, this.shake_intensity) * 0.2f, quaternion.w + UnityEngine.Random.Range(-this.shake_intensity, this.shake_intensity) * 0.2f);
			this.shake_intensity -= this.shake_decay * Time.deltaTime;
		}
		else
		{
			base.transform.rotation = quaternion;
			base.transform.position = vector;
		}
		if (zInput.GetButton("Ctrl", ControlType.All))
		{
			for (int i = 0; i < 10; i++)
			{
				if (TTSInput.GetKeyUp(i.ToString()))
				{
					this.SaveCamera(i, true);
				}
			}
		}
		if (zInput.GetButton("Shift", ControlType.All))
		{
			for (int j = 0; j < 10; j++)
			{
				if (TTSInput.GetKeyUp(j.ToString()))
				{
					this.LoadCamera(j);
				}
			}
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0003D1A9 File Offset: 0x0003B3A9
	public bool CameraStateInUse(int Slot)
	{
		return this.CameraStates[Slot] != null;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0003D1BC File Offset: 0x0003B3BC
	public void SaveCamera(int Slot, bool removeWhenIdentical = true)
	{
		if (removeWhenIdentical && this.CameraStates[Slot] != null && this.CameraStates[Slot].Zoomed == this.bZoomed && this.CameraStates[Slot].Distance == this.distance && this.CameraStates[Slot].Position.ToVector() == this.target.position && this.CameraStates[Slot].Rotation.ToVector() == new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, 0f))
		{
			this.CameraStates[Slot] = null;
			UIBroadcast.Log("Removed Camera #" + Slot, Colour.Orange, 2f, 0f);
			return;
		}
		this.lastSavedCamera = Slot;
		this.CachedFirstPerson();
		this.CameraStates[Slot] = new CameraState();
		this.CameraStates[Slot].Zoomed = this.bZoomed;
		this.CameraStates[Slot].Distance = this.distance;
		this.CameraStates[Slot].Position = new VectorState(this.target.position);
		this.CameraStates[Slot].Rotation = new VectorState(base.transform.eulerAngles.x, base.transform.eulerAngles.y, 0f);
		this.CameraStates[Slot].AbsolutePosition = new VectorState?(new VectorState(base.transform.position));
		UIBroadcast.Log("Saved Camera #" + Slot, Colour.Green, 2f, 0f);
		if (this.bCachedFirstPerson)
		{
			this.StartThirdPerson();
		}
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0003D39C File Offset: 0x0003B59C
	public void LoadCamera(int Slot)
	{
		if (!this.CameraStateInUse(Slot))
		{
			UIBroadcast.Log("No Camera #" + Slot, Colour.Red, 2f, 0f);
			return;
		}
		this.lastLoadedCamera = Slot;
		this.CachedFirstPerson();
		this.bRotateHorizontal = true;
		this.bRotateVertical = true;
		this.bZoomed = this.CameraStates[Slot].Zoomed;
		this.distance = this.CameraStates[Slot].Distance;
		this.target.position = this.CameraStates[Slot].Position.ToVector();
		this.Angle = this.CameraStates[Slot].Rotation.y;
		this.AngleTwo = this.CameraStates[Slot].Rotation.x;
		this.AltZoom = 1f;
		this.MagnifyZoom = 1f;
		UIBroadcast.Log("Loaded Camera #" + Slot, Colour.Blue, 2f, 0f);
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0003D4A9 File Offset: 0x0003B6A9
	private void CachedFirstPerson()
	{
		this.bCachedFirstPerson = this.bFirstPerson;
		if (this.bFirstPerson)
		{
			this.StartThirdPerson();
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0003D4C5 File Offset: 0x0003B6C5
	public void StartCameraMode(CameraController.CameraMode mode)
	{
		switch (mode)
		{
		case CameraController.CameraMode.ThirdPerson:
			this.StartThirdPerson();
			return;
		case CameraController.CameraMode.FirstPerson:
			this.StartFirstPerson();
			return;
		case CameraController.CameraMode.TopDown:
			this.StartTopDown();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0003D4F0 File Offset: 0x0003B6F0
	public void StartThirdPerson()
	{
		if (!this.bThirdPerson)
		{
			UIBroadcast.Log("3rd person camera activated.", Colour.Blue, 2f, 0f);
			Debug.Log("3rd person camera activated.");
			if (this.CurrentMode == CameraController.CameraMode.FirstPerson)
			{
				this.target.position = base.transform.rotation * new Vector3(0f, 0f, this.distance) + base.transform.position;
				this.actualtarget = base.transform.rotation * new Vector3(0f, 0f, this.distance) + base.transform.position;
			}
			this.CurrentMode = CameraController.CameraMode.ThirdPerson;
			this.camera.nearClipPlane = 0.3f;
			this.camera.orthographic = false;
			this.renderTopCamera.orthographic = false;
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0003D5E4 File Offset: 0x0003B7E4
	public void StartFirstPerson()
	{
		if (!this.bFirstPerson)
		{
			UIBroadcast.Log("1st person camera activated.", Colour.Green, 2f, 0f);
			Debug.Log("1st person camera activated.");
			this.CurrentMode = CameraController.CameraMode.FirstPerson;
			this.target.position = base.transform.position;
			this.actualtarget = base.transform.position;
			this.camera.nearClipPlane = 0.3f;
			this.camera.orthographic = false;
			this.renderTopCamera.orthographic = false;
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003D678 File Offset: 0x0003B878
	public void StartTopDown()
	{
		if (!this.bTopDown)
		{
			UIBroadcast.Log("Top down camera activated.", Colour.Purple, 2f, 0f);
			Debug.Log("Top down camera activated.");
			if (this.CurrentMode == CameraController.CameraMode.FirstPerson)
			{
				this.target.position = base.transform.rotation * new Vector3(0f, 0f, this.distance) + base.transform.position;
				this.actualtarget = base.transform.rotation * new Vector3(0f, 0f, this.distance) + base.transform.position;
			}
			this.CurrentMode = CameraController.CameraMode.TopDown;
			this.camera.nearClipPlane = 1E-06f;
			this.camera.orthographic = true;
			this.renderTopCamera.orthographic = true;
		}
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0003D76C File Offset: 0x0003B96C
	private void CheckCollisionPenetrate(ref Vector3 position, ref Quaternion rotation)
	{
		if (this.bTopDown)
		{
			return;
		}
		for (int i = 0; i < 1; i++)
		{
			int num = Physics.OverlapSphereNonAlloc(position, this.cameraCollider.radius, this.colliders);
			Vector3 a = Vector3.zero;
			float num2 = 0f;
			for (int j = 0; j < num; j++)
			{
				Collider collider = this.colliders[j];
				if (!(collider == this.cameraCollider) && !collider.isTrigger)
				{
					NetworkPhysicsObject component = collider.transform.root.GetComponent<NetworkPhysicsObject>();
					if (component && component.IsLocked)
					{
						Collider colliderA = this.cameraCollider;
						Collider collider2 = collider;
						Vector3 vector;
						float num3;
						Physics.ComputePenetration(colliderA, position, rotation, collider2, collider2.transform.position, collider2.transform.rotation, out vector, out num3);
						if (num3 > 0.0001f && !float.IsNaN(num3) && num3 > num2)
						{
							a = vector;
							num2 = num3;
						}
					}
				}
			}
			if (num2 == 0f)
			{
				break;
			}
			Vector3 b = a * num2;
			position += b;
			if (this.bFirstPerson)
			{
				this.target.position = position;
			}
		}
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0003D8E0 File Offset: 0x0003BAE0
	public void ResetCameraRotation()
	{
		base.StartCoroutine(this.DelayResetCameraRotation(true, ""));
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0003D8F5 File Offset: 0x0003BAF5
	public void ResetCameraRotation(bool delayOneFrame)
	{
		base.StartCoroutine(this.DelayResetCameraRotation(delayOneFrame, ""));
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0003D90A File Offset: 0x0003BB0A
	public void ResetCameraRotation(string colourLabel)
	{
		base.StartCoroutine(this.DelayResetCameraRotation(true, colourLabel));
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0003D91B File Offset: 0x0003BB1B
	public void ResetCameraRotation(bool delayOneFrame, string colourLabel)
	{
		base.StartCoroutine(this.DelayResetCameraRotation(delayOneFrame, colourLabel));
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0003D92C File Offset: 0x0003BB2C
	private IEnumerator DelayResetCameraRotation(bool delayOneFrame, string colourLabel = "")
	{
		if (delayOneFrame)
		{
			yield return null;
		}
		this.bLuaAttached = false;
		this.bLuaAttachmentRotate = false;
		if (this.CameraStateInUse(0) && !VRHMD.isVR)
		{
			this.LoadCamera(0);
			yield break;
		}
		Vector3 vector = Vector3.zero;
		if (PlayerScript.Pointer && colourLabel == "")
		{
			colourLabel = PlayerScript.PointerScript.PointerColorLabel;
		}
		GameObject gameObject = HandZone.GetHand(colourLabel, 0);
		if (!gameObject)
		{
			gameObject = HandZone.GetStartHand();
		}
		if (gameObject)
		{
			vector = gameObject.transform.position;
			if (VRHMD.isVR)
			{
				Singleton<VRHMD>.Instance.StoreAfterFade = 2;
				Singleton<VRHMD>.Instance.Spin = gameObject.transform.eulerAngles.y;
				Singleton<VRHMD>.Instance.Teleport(gameObject.transform.position);
			}
			this.refdistance = Mathf.Abs(vector.x * gameObject.transform.forward.normalized.x) + Mathf.Abs(vector.z * gameObject.transform.forward.normalized.z);
			this.refdistance *= 2.25f;
			this.Angle = gameObject.transform.eulerAngles.y;
			this.playerAngle = this.Angle;
			if (this.bOblong)
			{
				this.target.position = new Vector3(Mathf.Clamp(vector.x * 0.8f, -20f, 20f), this.target.position.y, 0f);
				this.refdistance = Vector3.Distance(this.target.position, vector) * 3.25f;
				this.refdistance = Mathf.Clamp(this.refdistance * 0.6f, 30f, 50f);
				this.AngleTwo = 45f;
			}
			else
			{
				this.AngleTwo = 65f;
			}
			this.playerAngleTwo = this.AngleTwo;
			this.CachedFirstPerson();
			this.bRotateHorizontal = true;
			this.bRotateVertical = true;
			this.distance = this.refdistance;
			if (!this.bOblong)
			{
				this.target.position = this.reftargetpos;
			}
			this.Angle = this.playerAngle;
			this.AngleTwo = this.playerAngleTwo;
			this.AltZoom = 1f;
			this.MagnifyZoom = 1f;
			HoverScript.AltZoomRotate = 180f;
			this.bZoomed = false;
			yield break;
		}
		yield break;
	}

	// Token: 0x040005F7 RID: 1527
	public static bool WHEEL_CENTER_ON_ZOOM;

	// Token: 0x040005F8 RID: 1528
	public int lastLoadedCamera;

	// Token: 0x040005F9 RID: 1529
	public int lastSavedCamera;

	// Token: 0x040005FA RID: 1530
	public Camera renderTopCamera;

	// Token: 0x040005FB RID: 1531
	public Transform target;

	// Token: 0x040005FC RID: 1532
	public SphereCollider cameraCollider;

	// Token: 0x040005FD RID: 1533
	private Vector3 actualtarget;

	// Token: 0x040005FE RID: 1534
	private Vector3? zeroToCursorPosition;

	// Token: 0x040005FF RID: 1535
	private float zeroToCursorEnd;

	// Token: 0x04000600 RID: 1536
	private const float ZERO_DURATION = 0.1f;

	// Token: 0x04000601 RID: 1537
	private Vector3 reftargetpos;

	// Token: 0x04000602 RID: 1538
	private float refdistance;

	// Token: 0x04000603 RID: 1539
	private float refdistanceMin;

	// Token: 0x04000604 RID: 1540
	private float _distance = 30f;

	// Token: 0x04000605 RID: 1541
	private float actualdistance;

	// Token: 0x04000606 RID: 1542
	private const float xSpeed = 100f;

	// Token: 0x04000607 RID: 1543
	private const float ySpeed = 100f;

	// Token: 0x04000608 RID: 1544
	public float LOAD_ROTATION_RATE = 100f;

	// Token: 0x04000609 RID: 1545
	public const float yMinLimit = 10f;

	// Token: 0x0400060A RID: 1546
	public const float yMaxLimit = 90f;

	// Token: 0x0400060B RID: 1547
	public const float DISTANCE_MAX = 140f;

	// Token: 0x0400060C RID: 1548
	public float distanceMax = 140f;

	// Token: 0x0400060D RID: 1549
	public float distanceMin = 10f;

	// Token: 0x0400060E RID: 1550
	public float x;

	// Token: 0x0400060F RID: 1551
	public float y;

	// Token: 0x04000610 RID: 1552
	private float xSmooth;

	// Token: 0x04000611 RID: 1553
	private float ySmooth;

	// Token: 0x04000612 RID: 1554
	public float Angle;

	// Token: 0x04000613 RID: 1555
	private float playerAngle;

	// Token: 0x04000614 RID: 1556
	public float AngleTwo;

	// Token: 0x04000615 RID: 1557
	private float playerAngleTwo;

	// Token: 0x04000616 RID: 1558
	public bool bRotateHorizontal;

	// Token: 0x04000617 RID: 1559
	public bool bRotateVertical;

	// Token: 0x04000618 RID: 1560
	public bool bOblong;

	// Token: 0x04000619 RID: 1561
	private bool _bZoomed;

	// Token: 0x0400061A RID: 1562
	public float ZoomedDistance;

	// Token: 0x0400061B RID: 1563
	public Vector3 ZoomedPosition;

	// Token: 0x0400061C RID: 1564
	public CameraController.CameraMode CurrentMode;

	// Token: 0x0400061D RID: 1565
	private bool bCachedFirstPerson;

	// Token: 0x0400061E RID: 1566
	public float AltZoom = 1f;

	// Token: 0x0400061F RID: 1567
	public float MagnifyZoom = 1f;

	// Token: 0x04000620 RID: 1568
	public CameraState[] CameraStates = new CameraState[10];

	// Token: 0x04000621 RID: 1569
	public bool bLuaAttached;

	// Token: 0x04000622 RID: 1570
	public GameObject LuaAttachment;

	// Token: 0x04000623 RID: 1571
	public Vector3 LuaAttachmentOffset = Vector3.zero;

	// Token: 0x04000624 RID: 1572
	public bool bLuaAttachmentRotate;

	// Token: 0x04000625 RID: 1573
	private Camera camera;

	// Token: 0x04000626 RID: 1574
	private float shake_decay;

	// Token: 0x04000627 RID: 1575
	private float shake_intensity;

	// Token: 0x04000628 RID: 1576
	private Vector3 PanStartVector;

	// Token: 0x04000629 RID: 1577
	private float PanHeldTime;

	// Token: 0x0400062A RID: 1578
	private const int DEPENETRATE_ITERATIONS = 1;

	// Token: 0x0400062B RID: 1579
	private Collider[] colliders = new Collider[20];

	// Token: 0x02000580 RID: 1408
	public enum CameraMode
	{
		// Token: 0x0400250C RID: 9484
		ThirdPerson,
		// Token: 0x0400250D RID: 9485
		FirstPerson,
		// Token: 0x0400250E RID: 9486
		TopDown
	}
}
