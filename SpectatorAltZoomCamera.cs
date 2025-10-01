using System;
using NewNet;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class SpectatorAltZoomCamera : Singleton<SpectatorAltZoomCamera>
{
	// Token: 0x06001C11 RID: 7185 RVA: 0x000C0814 File Offset: 0x000BEA14
	protected override void Awake()
	{
		base.Awake();
		this.camera = base.GetComponent<Camera>();
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x000C0828 File Offset: 0x000BEA28
	private void OnPreCull()
	{
		if (Camera.current != this.camera || !SpectatorAltZoomCamera.SpectatorAltZoomActive || !Singleton<SpectatorCamera>.Instance.DisplayingFullscreen)
		{
			return;
		}
		Singleton<AltZoomCamera>.Instance.GetZoomObjects(out this.ZoomObject, out this.ZoomObjectNPO);
		if (this.ZoomObject)
		{
			if (SpectatorAltZoomCamera.SpectatorAltZoomRestricted && Singleton<SpectatorCamera>.Instance.RestrictView && Singleton<SpectatorCamera>.Instance.ZoomObjectHidden)
			{
				this.ZoomObject = null;
				return;
			}
			this.Rotation = this.ZoomObject.transform.rotation;
			this.Position = this.ZoomObject.transform.position;
			this.UIItem = (this.ZoomObject.layer == 5);
			if (this.UIItem)
			{
				this.ZoomObject.transform.rotation = Quaternion.identity;
				this.Scale = this.ZoomObject.transform.localScale;
				this.ZoomObject.transform.localScale *= 15f;
				Transform[] componentsInChildren = this.ZoomObject.GetComponentsInChildren<Transform>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.layer = 10;
				}
			}
			if (this.isThumbnail)
			{
				this.Scale = this.ZoomObject.transform.localScale;
				this.ZoomObject.transform.localScale *= 10f;
			}
			if (Network.isServer)
			{
				this.isSleeping = (this.ZoomObjectNPO && !this.ZoomObjectNPO.IsMoving && this.ZoomObjectNPO.IsHeldByNobody);
			}
			bool flag = this.ZoomObject.transform.up.normalized.y <= 0f;
			bool flag2 = this.ZoomObjectNPO && this.ZoomObjectNPO.HasRotationsValues();
			bool flag3 = this.ZoomObject.CompareTag("Dice") && !this.UIItem;
			bool flag4 = flag2 || flag3;
			bool flag5 = this.ZoomObjectNPO && this.ZoomObjectNPO.customImage && this.ZoomObjectNPO.customImage.ObjectName == "Figurine";
			if (!this.ZoomObjectNPO)
			{
				flag5 = (this.ZoomObject.GetComponent<CustomImage>() && this.ZoomObject.GetComponent<CustomImage>().ObjectName == "Figurine");
			}
			bool flag6 = this.ZoomObjectNPO && ((this.ZoomObjectNPO.deckScript && this.ZoomObjectNPO.deckScript.bSideways) || (this.ZoomObjectNPO.cardScript && this.ZoomObjectNPO.cardScript.bSideways));
			if (!this.ZoomObjectNPO)
			{
				flag6 = ((this.ZoomObject.GetComponent<DeckScript>() && this.ZoomObject.GetComponent<DeckScript>().bSideways) || (this.ZoomObject.GetComponent<CardScript>() && this.ZoomObject.GetComponent<CardScript>().bSideways));
			}
			bool flag7 = this.ZoomObject.CompareTag("Notecard") || this.ZoomObject.CompareTag("Counter");
			bool flag8 = flag5 || this.ZoomObject.CompareTag("Clock");
			bool flag9 = this.ZoomObjectNPO && (this.ZoomObject.GetComponentInChildren<Animation>() || this.ZoomObject.GetComponentInChildren<ParticleSystem>());
			Vector3 rotation = new Vector3(this.ZoomObject.transform.eulerAngles.x, 180f, this.ZoomObject.transform.eulerAngles.z);
			if (!flag4)
			{
				rotation = new Vector3(0f, 0f, (float)((this.ZoomObject.transform.up.normalized.y >= 0f) ? 0 : -180));
				if (flag7)
				{
					rotation.y -= 180f;
				}
				if (flag8)
				{
					rotation.z -= 180f;
				}
			}
			else if (flag2)
			{
				rotation = this.ZoomObjectNPO.GetRotationValue(false).rotation;
			}
			else if (flag3)
			{
				rotation = new Vector3(this.ZoomObject.transform.eulerAngles.x, 0f, this.ZoomObject.transform.eulerAngles.z);
			}
			this.ZoomObject.transform.eulerAngles = Vector3.zero;
			Bounds bounds = default(Bounds);
			Renderer[] componentsInChildren2 = this.ZoomObject.GetComponentsInChildren<Renderer>(true);
			if (flag9 && this.ZoomObjectNPO)
			{
				bounds = this.ZoomObjectNPO.GetBoundsNotNormalized();
			}
			else
			{
				foreach (Renderer renderer in componentsInChildren2)
				{
					if (bounds == default(Bounds))
					{
						bounds = renderer.bounds;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			this.ZoomObject.transform.eulerAngles = rotation;
			if (!flag4)
			{
				this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(HoverScript.AltZoomRotate, Vector3.up);
			}
			if (flag6)
			{
				this.ZoomObject.transform.rotation *= Quaternion.AngleAxis((float)((this.ZoomObject.transform.up.normalized.y >= 0f) ? -90 : 90), Vector3.up);
			}
			bounds.size = new Vector3((float)Math.Round((double)bounds.size.x, 4), (float)Math.Round((double)bounds.size.y, 4), (float)Math.Round((double)bounds.size.z, 4));
			Utilities.Face face = Utilities.GetLargestFace(bounds);
			if (this.ZoomObject.GetComponent<StackObject>() && (!this.ZoomObject.GetComponent<StackObject>().bBag & !this.ZoomObject.GetComponent<StackObject>().IsInfiniteStack))
			{
				face = Utilities.Face.Top;
			}
			if (flag4)
			{
				face = Utilities.Face.Top;
			}
			switch (face)
			{
			case Utilities.Face.Top:
				if (zInput.GetButton("Shift", ControlType.All) && PlayerScript.Pointer && PermissionsOptions.CheckAllow(PermissionsOptions.options.Peeking, -1))
				{
					this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(-180f, Vector3.forward);
				}
				break;
			case Utilities.Face.Front:
				this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(-90f, Vector3.right);
				if (flag)
				{
					this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(-180f, Vector3.right);
				}
				if (zInput.GetButton("Shift", ControlType.All))
				{
					this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(-180f, Vector3.up);
				}
				break;
			case Utilities.Face.Side:
				this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(90f, Vector3.forward);
				this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(-90f, Vector3.right);
				if (flag)
				{
					this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(-180f, Vector3.forward);
				}
				if (zInput.GetButton("Shift", ControlType.All))
				{
					this.ZoomObject.transform.rotation *= Quaternion.AngleAxis(-180f, Vector3.up);
				}
				break;
			}
			base.transform.position = new Vector3(1000f, 100f, 1000f);
			base.transform.eulerAngles = new Vector3(90f, 0f, 0f);
			float num = 3.5f / Singleton<CameraController>.Instance.AltZoom;
			this.camera.orthographicSize = num;
			Vector3 position;
			if (this.isThumbnail)
			{
				position = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), HoverScript.PointerPixel.z);
			}
			else if (SpectatorAltZoomCamera.SpectatorAltZoomFollowsPointer)
			{
				position = Singleton<SpectatorCamera>.Instance.GetComponent<Camera>().WorldToScreenPoint(PlayerScript.PointerScript.ReferenceFollower.transform.position);
			}
			else
			{
				position = new Vector3((float)Screen.width * SpectatorAltZoomCamera.SpectatorAltZoomLocation.x, (float)Screen.height * SpectatorAltZoomCamera.SpectatorAltZoomLocation.y, HoverScript.PointerPixel.z);
			}
			Vector3 position2 = this.camera.ScreenToWorldPoint(position);
			position2.y -= 100f;
			this.ZoomObject.transform.position = position2;
			bounds = default(Bounds);
			if (flag9 && this.ZoomObjectNPO)
			{
				bounds = this.ZoomObjectNPO.GetBoundsNotNormalized();
			}
			else
			{
				foreach (Renderer renderer2 in componentsInChildren2)
				{
					if (bounds == default(Bounds))
					{
						bounds = renderer2.bounds;
					}
					else
					{
						bounds.Encapsulate(renderer2.bounds);
					}
				}
			}
			Vector3 vector = new Vector3(this.ZoomObject.transform.position.x - bounds.center.x, this.ZoomObject.transform.position.y - bounds.center.y, this.ZoomObject.transform.position.z - bounds.center.z);
			Vector3 vector2 = this.camera.WorldToViewportPoint(this.ZoomObject.transform.position);
			float num2 = bounds.size.x * 0.25f * (float)Screen.height / (float)Screen.width / num;
			float num3 = bounds.size.z * 0.25f / num;
			Vector3 vector3 = vector2;
			vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector2, num2, num3, SpriteAlignment.Center, false);
			if (vector2.y == 0.5f && vector2.x == 0.5f && vector3 != vector2)
			{
				if (num3 > num2)
				{
					this.camera.orthographicSize *= num3 * 2f;
					vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num2 / (num3 * 2f), num3, SpriteAlignment.Center, false);
				}
				else
				{
					this.camera.orthographicSize *= num2 * 2f;
					vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num2, num3 / (num2 * 2f), SpriteAlignment.Center, false);
				}
			}
			else if (vector2.y == 0.5f && vector3.y != vector2.y)
			{
				this.camera.orthographicSize *= num3 * 2f;
				vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num2 / (num3 * 2f), num3, SpriteAlignment.Center, false);
			}
			else if (vector2.x == 0.5f && vector3.x != vector2.x)
			{
				this.camera.orthographicSize *= num2 * 2f;
				vector2 = NetworkSingleton<ManagerPhysicsObject>.Instance.ClampToScreen(vector3, num2, num3 / (num2 * 2f), SpriteAlignment.Center, false);
			}
			if (this.isThumbnail)
			{
				vector2.x = 0.5f;
				vector2.y = 0.5f;
				this.camera.orthographicSize *= 1.1f;
				if (bounds.size.x > bounds.size.z)
				{
					this.camera.orthographicSize *= bounds.size.x / bounds.size.z;
				}
			}
			Vector3 vector4 = this.camera.ViewportToWorldPoint(vector2);
			vector4 = new Vector3(vector4.x + vector.x, vector4.y + vector.y, vector4.z + vector.z);
			this.ZoomObject.transform.position = vector4;
			UIPanel componentInChildren = this.ZoomObject.GetComponentInChildren<UIPanel>();
			if (componentInChildren)
			{
				componentInChildren.ForceUpdate(3000);
			}
		}
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x000C1514 File Offset: 0x000BF714
	private void OnPostRender()
	{
		if (Camera.current != this.camera || !SpectatorAltZoomCamera.SpectatorAltZoomActive || !Singleton<SpectatorCamera>.Instance.DisplayingFullscreen)
		{
			return;
		}
		if (this.ZoomObject)
		{
			this.ZoomObject.transform.rotation = this.Rotation;
			this.ZoomObject.transform.position = this.Position;
			if (this.isSleeping)
			{
				this.ZoomObjectNPO.rigidbody.Sleep();
			}
			if (this.UIItem)
			{
				this.ZoomObject.transform.localScale = this.Scale;
				Transform[] componentsInChildren = this.ZoomObject.GetComponentsInChildren<Transform>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.layer = 5;
				}
			}
			if (this.isThumbnail)
			{
				this.ZoomObject.transform.localScale = this.Scale;
			}
		}
	}

	// Token: 0x040011AE RID: 4526
	public static bool SpectatorAltZoomActive = true;

	// Token: 0x040011AF RID: 4527
	public static bool SpectatorAltZoomRestricted = true;

	// Token: 0x040011B0 RID: 4528
	public static bool SpectatorAltZoomFollowsPointer = true;

	// Token: 0x040011B1 RID: 4529
	public static Vector2 SpectatorAltZoomLocation = new Vector2(0f, 0f);

	// Token: 0x040011B2 RID: 4530
	private GameObject ZoomObject;

	// Token: 0x040011B3 RID: 4531
	private NetworkPhysicsObject ZoomObjectNPO;

	// Token: 0x040011B4 RID: 4532
	private Vector3 Position;

	// Token: 0x040011B5 RID: 4533
	private Quaternion Rotation;

	// Token: 0x040011B6 RID: 4534
	private bool UIItem;

	// Token: 0x040011B7 RID: 4535
	private Vector3 Scale;

	// Token: 0x040011B8 RID: 4536
	private bool isSleeping;

	// Token: 0x040011B9 RID: 4537
	private Camera camera;

	// Token: 0x040011BA RID: 4538
	private bool isThumbnail;
}
