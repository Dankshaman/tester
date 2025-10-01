using System;
using System.Collections.Generic;
using mset;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class AltZoomCamera : Singleton<AltZoomCamera>
{
	// Token: 0x1700019B RID: 411
	// (set) Token: 0x06000824 RID: 2084 RVA: 0x00038AD4 File Offset: 0x00036CD4
	public GameObject ZoomObject
	{
		set
		{
			if (this.zoomObject)
			{
				this.zoomObject.GetComponentsInChildren<Transform>(true, this.transforms);
				foreach (Transform transform in this.transforms)
				{
					if (transform.gameObject.layer == 22)
					{
						transform.gameObject.layer = 17;
					}
				}
				this.transforms.Clear();
			}
			this.zoomObject = value;
			base.gameObject.SetActive(this.zoomObject != null);
			Singleton<SpectatorAltZoomCamera>.Instance.gameObject.SetActive(this.zoomObject != null);
			if (this.zoomObject)
			{
				this.zoomObject.GetComponentsInChildren<Transform>(true, this.transforms);
				foreach (Transform transform2 in this.transforms)
				{
					if (transform2.gameObject.layer == 17)
					{
						transform2.gameObject.layer = 22;
					}
				}
				this.transforms.Clear();
				this.zoomObjectNPO = this.zoomObject.GetComponent<NetworkPhysicsObject>();
			}
			else
			{
				this.zoomObjectNPO = null;
			}
			NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject = this.zoomObject;
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00038C4C File Offset: 0x00036E4C
	protected override void Awake()
	{
		base.Awake();
		this.camera = base.GetComponent<Camera>();
		this.cullingMask = this.camera.cullingMask;
		this.camera.cullingMask = 4194304;
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00038C81 File Offset: 0x00036E81
	public void GetZoomObjects(out GameObject zoomObject, out NetworkPhysicsObject zoomNPO)
	{
		zoomObject = this.zoomObject;
		zoomNPO = this.zoomObjectNPO;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00038C94 File Offset: 0x00036E94
	public void SaveThumbnail(string thumbnailPath, GameObject target)
	{
		if (target == null)
		{
			Debug.LogError("SaveThumbnail target is null");
			return;
		}
		this.isThumbnail = true;
		Rigidbody component = target.GetComponent<Rigidbody>();
		bool isKinematic = false;
		if (component)
		{
			isKinematic = component.isKinematic;
			component.isKinematic = true;
		}
		this.ZoomObject = target;
		ScreenshotScript.SaveTexture(ScreenshotScript.TakeScreenshot(256, 256, this.camera, true), thumbnailPath, false, true);
		this.ZoomObject = null;
		this.isThumbnail = false;
		if (component)
		{
			component.isKinematic = isKinematic;
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00038D20 File Offset: 0x00036F20
	private void UpdateLighting(bool altZoomEnabled)
	{
		Sky globalSky = SkyManager.Get().GlobalSky;
		Transform transform = globalSky.transform;
		Transform transform2 = NetworkSingleton<LightingScript>.Instance.DirectionalLight.transform;
		if (altZoomEnabled)
		{
			this.sceneSkyRotation = transform.rotation;
			this.sceneDirectionalRotation = transform2.rotation;
			Quaternion rotation = this.camera.transform.rotation;
			Quaternion rhs = this.sceneSkyRotation * AltZoomCamera.UP_ROTATION;
			transform.rotation = rotation * rhs;
			Quaternion rhs2 = this.sceneDirectionalRotation * AltZoomCamera.UP_ROTATION;
			transform2.rotation = rotation * rhs2;
		}
		else
		{
			transform.rotation = this.sceneSkyRotation;
			transform2.rotation = this.sceneDirectionalRotation;
		}
		globalSky.UpdateSkyTransform();
		transform.hasChanged = false;
		if (this.zoomObjectNPO)
		{
			using (List<Renderer>.Enumerator enumerator = this.zoomObjectNPO.Renderers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Renderer target = enumerator.Current;
					globalSky.Apply(target);
				}
				return;
			}
		}
		this.zoomObject.GetComponents<Renderer>(this.renderers);
		foreach (Renderer target2 in this.renderers)
		{
			globalSky.Apply(target2);
		}
		this.renderers.Clear();
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x00038EA0 File Offset: 0x000370A0
	public void OnPreCull()
	{
		if (!this.zoomObject)
		{
			return;
		}
		this.zoomObject.GetComponentsInChildren<Transform>(true, this.transforms);
		if (this.transformLayers.Capacity < this.transforms.Capacity)
		{
			this.transformLayers.Capacity = this.transforms.Capacity;
		}
		bool flag = this.zoomObject.layer == 5;
		if (flag)
		{
			this.uiObjectInitialRotation = this.zoomObject.transform.rotation;
			this.zoomObject.transform.rotation = Quaternion.identity;
		}
		for (int i = 0; i < this.transforms.Count; i++)
		{
			Transform transform = this.transforms[i];
			this.transformLayers.Add(transform.gameObject.layer);
			if ((this.cullingMask & 1 << transform.gameObject.layer) != 0 || flag)
			{
				transform.gameObject.layer = 22;
			}
		}
		this.zoomObject.GetComponentsInChildren<UIPanel>(this.panels);
		if (this.panels.Count > 0)
		{
			for (int j = 0; j < this.panels.Count; j++)
			{
				this.panels[j].UpdateLayers();
			}
			this.panels.Clear();
		}
		CardScript cardScript = this.zoomObjectNPO ? this.zoomObjectNPO.cardScript : this.zoomObject.GetComponent<CardScript>();
		DeckScript deckScript = this.zoomObjectNPO ? this.zoomObjectNPO.deckScript : this.zoomObject.GetComponent<DeckScript>();
		StackObject stackObject = this.zoomObjectNPO ? this.zoomObjectNPO.stackObject : this.zoomObject.GetComponent<StackObject>();
		CustomImage customImage = this.zoomObjectNPO ? this.zoomObjectNPO.customImage : this.zoomObject.GetComponent<CustomImage>();
		bool flag2 = stackObject && (!stackObject.bBag & !stackObject.IsInfiniteStack);
		bool flag3 = customImage && customImage.ObjectName == "Figurine";
		bool flag4 = this.zoomObjectNPO && this.zoomObjectNPO.HasRotationsValues();
		bool flag5 = cardScript ? cardScript.bSideways : (deckScript && deckScript.bSideways);
		bool flag6 = flag4 || this.zoomObject.transform.up.normalized.y <= 0f;
		bool flag7 = !flag4 && !flag2;
		Utilities.Face face = Utilities.Face.Top;
		bool flag8 = this.zoomObject.GetComponentInChildren<Animation>() || this.zoomObject.GetComponentInChildren<ParticleSystem>();
		Bounds bounds;
		if (this.zoomObjectNPO)
		{
			if (flag8)
			{
				bounds = this.zoomObjectNPO.GetBounds();
			}
			else
			{
				bounds = this.zoomObjectNPO.GetRendererBounds();
			}
		}
		else
		{
			this.zoomObject.GetComponentsInChildren<Renderer>(true, this.renderers);
			bounds = ((this.renderers.Count > 0) ? this.renderers[0].bounds : default(Bounds));
			for (int k = 1; k < this.renderers.Count; k++)
			{
				bounds.Encapsulate(this.renderers[k].bounds);
			}
			this.renderers.Clear();
		}
		bounds.size = new Vector3((float)Math.Round((double)bounds.size.x, 4), (float)Math.Round((double)bounds.size.y, 4), (float)Math.Round((double)bounds.size.z, 4));
		if (flag7)
		{
			face = Utilities.GetLargestFace(bounds);
		}
		Transform transform2 = this.zoomObject.transform;
		Vector3 position = transform2.position;
		Quaternion quaternion = transform2.rotation;
		if (flag4)
		{
			quaternion *= Quaternion.Inverse(Quaternion.Euler(this.zoomObjectNPO.GetRotationValue(false).rotation));
		}
		switch (face)
		{
		case Utilities.Face.Top:
			quaternion *= Quaternion.AngleAxis((float)(flag6 ? 90 : -90), Vector3.right);
			if (zInput.GetButton("Shift", ControlType.All))
			{
				if (PlayerScript.Pointer)
				{
					if (PermissionsOptions.CheckAllow(PermissionsOptions.options.Peeking, -1))
					{
						quaternion *= Quaternion.AngleAxis(180f, Vector3.up);
						if (this.zoomObjectNPO && this.zoomObjectNPO.networkView)
						{
							this.zoomObjectNPO.AddPeekIndicator();
						}
					}
					else
					{
						PermissionsOptions.BroadcastPermissionWarning("Peek");
					}
				}
				else
				{
					quaternion *= Quaternion.AngleAxis(180f, Vector3.right);
				}
			}
			break;
		case Utilities.Face.Front:
			quaternion *= Quaternion.AngleAxis((float)(zInput.GetButton("Shift", ControlType.All) ? 180 : 0), Vector3.up);
			break;
		case Utilities.Face.Side:
			quaternion *= Quaternion.AngleAxis((float)(zInput.GetButton("Shift", ControlType.All) ? -90 : 90), Vector3.up);
			break;
		}
		if (this.zoomObjectNPO && this.zoomObjectNPO.AltLookAngle != Vector3.zero)
		{
			quaternion *= Quaternion.Euler(this.zoomObjectNPO.AltLookAngle);
		}
		else if (flag7)
		{
			bool flag9 = flag3 || this.zoomObject.CompareTag("Clock");
			bool flag10 = this.zoomObject.CompareTag("Notecard") || this.zoomObject.CompareTag("Counter");
			if (!flag6)
			{
				flag10 = !flag10;
			}
			quaternion *= Quaternion.Euler(0f, (float)(flag9 ? 0 : 180), (float)(flag10 ? 180 : 0));
		}
		float num = HoverScript.AltZoomRotate;
		if (flag5)
		{
			num += -90f;
		}
		else if (flag4)
		{
			num += 180f;
		}
		quaternion *= Quaternion.AngleAxis(num, Vector3.forward);
		Transform transform3 = this.camera.transform;
		transform3.rotation = quaternion;
		transform3.position = position - 1000f * transform3.forward;
		this.UpdateLighting(true);
		float num2 = (flag ? 0.233f : 3.5f) / Singleton<CameraController>.Instance.AltZoom;
		if (this.isThumbnail)
		{
			num2 = 0.1f;
		}
		this.camera.orthographicSize = num2;
		Vector3 pointerPixel;
		if (this.isThumbnail)
		{
			pointerPixel = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), HoverScript.PointerPixel.z);
		}
		else if (AltZoomCamera.AltZoomFollowsPointer)
		{
			pointerPixel = HoverScript.PointerPixel;
		}
		else
		{
			pointerPixel = new Vector3((float)Screen.width * AltZoomCamera.AltZoomLocation.x, (float)Screen.height * AltZoomCamera.AltZoomLocation.y, HoverScript.PointerPixel.z);
		}
		Vector3 b = position;
		Vector3 b2 = Vector3.zero;
		Bounds bounds3;
		if (this.zoomObjectNPO)
		{
			Quaternion quaternion2 = quaternion * Quaternion.AngleAxis(-90f, Vector3.right);
			quaternion2 *= Quaternion.AngleAxis(180f, Vector3.up);
			Quaternion qangles = Quaternion.Inverse(this.zoomObject.transform.rotation) * quaternion2;
			Vector3 zero = Vector3.zero;
			Vector3 vector = bounds.size * 0.5f;
			Vector3[] array = new Vector3[]
			{
				zero + new Vector3(vector.x, vector.y, vector.z),
				zero + new Vector3(vector.x, vector.y, -vector.z),
				zero + new Vector3(vector.x, -vector.y, vector.z),
				zero + new Vector3(vector.x, -vector.y, -vector.z),
				zero + new Vector3(-vector.x, vector.y, vector.z),
				zero + new Vector3(-vector.x, vector.y, -vector.z),
				zero + new Vector3(-vector.x, -vector.y, vector.z),
				zero + new Vector3(-vector.x, -vector.y, -vector.z)
			};
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = Utilities.RotatePointAroundPivot(array[l], zero, qangles);
			}
			Bounds bounds2 = default(Bounds);
			for (int m = 0; m < array.Length; m++)
			{
				bounds2.Encapsulate(array[m]);
			}
			bounds3 = bounds2;
			b2 = Utilities.RotatePointAroundPivot(flag8 ? this.zoomObjectNPO.GetBoundsCenterOffset() : this.zoomObjectNPO.GetRendererBoundsCenterOffset(), zero, this.zoomObject.transform.rotation);
		}
		else
		{
			bounds3 = bounds;
			b2 = position - bounds3.center;
		}
		Vector3 vector2 = new Vector3(pointerPixel.x / (float)Screen.width, pointerPixel.y / (float)Screen.height, bounds3.size.y * 2f + 10f);
		float num3 = this.isThumbnail ? 1f : ((float)Screen.height / (float)Screen.width);
		float num4 = bounds3.size.x * 0.25f * num3 / num2;
		float num5 = bounds3.size.z * 0.25f / num2;
		Vector3 vector3 = vector2;
		vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector2, num4, num5, SpriteAlignment.Center, false);
		if (vector2.y == 0.5f && vector2.x == 0.5f && (vector3 != vector2 || this.isThumbnail))
		{
			if (num5 > num4)
			{
				this.camera.orthographicSize *= num5 * 2f;
				vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num4 / (num5 * 2f), num5, SpriteAlignment.Center, false);
			}
			else
			{
				this.camera.orthographicSize *= num4 * 2f;
				vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num4, num5 / (num4 * 2f), SpriteAlignment.Center, false);
			}
		}
		else if (vector2.y == 0.5f && (vector3.y != vector2.y || this.isThumbnail))
		{
			this.camera.orthographicSize *= num5 * 2f;
			vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num4 / (num5 * 2f), num5, SpriteAlignment.Center, false);
		}
		else if (vector2.x == 0.5f && (vector3.x != vector2.x || this.isThumbnail))
		{
			this.camera.orthographicSize *= num4 * 2f;
			vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num4, num5 / (num4 * 2f), SpriteAlignment.Center, false);
		}
		if (this.isThumbnail)
		{
			vector2.x = 0.5f;
			vector2.y = 0.5f;
			this.camera.orthographicSize *= 1.1f;
		}
		Vector3 a = this.camera.ViewportToWorldPoint(vector2) + b2;
		transform3.position -= a - b;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00039AC0 File Offset: 0x00037CC0
	private void OnPostRender()
	{
		if (!this.zoomObject)
		{
			return;
		}
		for (int i = 0; i < this.transforms.Count; i++)
		{
			this.transforms[i].gameObject.layer = this.transformLayers[i];
		}
		this.transforms.Clear();
		this.transformLayers.Clear();
		if (this.zoomObject.layer == 5)
		{
			this.zoomObject.transform.rotation = this.uiObjectInitialRotation;
		}
		this.UpdateLighting(false);
	}

	// Token: 0x0400059E RID: 1438
	private static readonly Quaternion UP_ROTATION = Quaternion.AngleAxis(-90f, Vector3.right);

	// Token: 0x0400059F RID: 1439
	public static bool AltZoomFollowsPointer = true;

	// Token: 0x040005A0 RID: 1440
	public static bool AltZoomAlwaysOn = false;

	// Token: 0x040005A1 RID: 1441
	public static Vector2 AltZoomLocation = new Vector2(1f, 0f);

	// Token: 0x040005A2 RID: 1442
	private GameObject zoomObject;

	// Token: 0x040005A3 RID: 1443
	private NetworkPhysicsObject zoomObjectNPO;

	// Token: 0x040005A4 RID: 1444
	private Quaternion uiObjectInitialRotation;

	// Token: 0x040005A5 RID: 1445
	private Camera camera;

	// Token: 0x040005A6 RID: 1446
	private int cullingMask;

	// Token: 0x040005A7 RID: 1447
	private bool isThumbnail;

	// Token: 0x040005A8 RID: 1448
	private Quaternion sceneSkyRotation;

	// Token: 0x040005A9 RID: 1449
	private Quaternion sceneDirectionalRotation;

	// Token: 0x040005AA RID: 1450
	private readonly List<int> transformLayers = new List<int>(128);

	// Token: 0x040005AB RID: 1451
	private readonly List<Transform> transforms = new List<Transform>(128);

	// Token: 0x040005AC RID: 1452
	private readonly List<UIPanel> panels = new List<UIPanel>();

	// Token: 0x040005AD RID: 1453
	private readonly List<Renderer> renderers = new List<Renderer>();
}
