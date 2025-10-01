using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class HandCamera : ViewCamera
{
	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06000F26 RID: 3878 RVA: 0x000677BF File Offset: 0x000659BF
	// (set) Token: 0x06000F27 RID: 3879 RVA: 0x000677C6 File Offset: 0x000659C6
	public static HandCamera Instance { get; private set; }

	// Token: 0x06000F28 RID: 3880 RVA: 0x000677CE File Offset: 0x000659CE
	protected override void Awake()
	{
		base.Awake();
		HandCamera.Instance = this;
		this.handStretch = this.HandSprite.GetComponent<UIStretch>();
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x000677F0 File Offset: 0x000659F0
	private void Update()
	{
		if (zInput.GetButtonDown("Hide Hand", ControlType.All) && !UICamera.SelectIsInput())
		{
			this.Enabled = !this.Enabled;
		}
		HandZone hand = this.GetHand();
		bool flag2;
		bool flag = this.HoldingHandObjects(out flag2);
		this.handInInteraction = this.GetPixelInteractionRect().Contains(HoverScript.InputScreenPosition(0));
		bool flag3 = this.Enabled && hand && (hand.HasHandObjects || (flag && this.handInInteraction)) && !VRHMD.isVR;
		if (flag3 && flag2 && !flag && this.handInInteraction)
		{
			flag3 = false;
		}
		if (this.handInInteraction && flag3)
		{
			this.showVelocity = 0f;
			this.slideAmount = Mathf.SmoothDamp(this.slideAmount, 0f, ref this.hideVelocity, 0.04f);
		}
		else
		{
			this.hideVelocity = 0f;
			this.slideAmount = Mathf.SmoothDamp(this.slideAmount, (1f - HandCamera.MINIMIZED_SIZE) / 2f, ref this.showVelocity, 0.04f);
		}
		this.camera.gameObject.SetActive(flag3);
		this.HandSprite.gameObject.SetActive(HoverScript.HoverCamera == this.camera && flag && !PlayerScript.PointerScript.ClickingWhileInHandSelectMode);
		Colour colour = PlayerScript.Pointer ? PlayerScript.PointerScript.PointerColour : Colour.White;
		colour.a = 0.75f;
		this.HandSprite.color = colour;
		this.handStretch.relativeSize.x = this.screenWidth;
		if (flag3)
		{
			base.transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y + 25f, hand.transform.position.z);
			base.transform.eulerAngles = new Vector3(90f, hand.transform.eulerAngles.y, 0f);
			Bounds handBounds = hand.HandBounds;
			Vector2 vector = new Vector2((hand.HandObjectsTotalXSize < handBounds.size.x) ? hand.HandObjectsTotalXSize : handBounds.size.x, hand.TallestYSize);
			if (!hand.HasHandObjects)
			{
				vector = new Vector2(handBounds.size.x, handBounds.size.z);
			}
			if (this.BoundsY == 0f)
			{
				this.BoundsY = vector.y;
			}
			this.BoundsY = Mathf.Lerp(this.BoundsY, vector.y, 3f * Time.deltaTime);
			float num = 1f;
			float num2 = this.screenWidth * this.maxBoundsWidth * (float)Screen.width / (float)Screen.height * this.BoundsY;
			if (vector.x > num2)
			{
				num *= vector.x / num2;
			}
			float num3 = this.sizeMulti * this.BoundsY;
			this.camera.orthographicSize = num3 / this.screenHeight * num;
			this.camera.rect = new Rect(Vector2.zero, Vector2.one);
			float num4 = this.camera.orthographicSize - num3;
			num4 *= 1f + this.slideAmount;
			base.transform.position = new Vector3(base.transform.position.x + num4 * hand.transform.forward.x, base.transform.position.y, base.transform.position.z + num4 * hand.transform.forward.z);
		}
		else
		{
			this.BoundsY = 0f;
		}
		if (PlayerScript.Pointer && PlayerScript.PointerScript.ReferenceFollower.layer != 29)
		{
			PlayerScript.PointerScript.ReferenceFollower.layer = ((HoverScript.HoverCamera == this.camera) ? 19 : 8);
		}
		if (flag3 && flag && HoverScript.PrevHoverCamera == this.camera && HoverScript.HoverCamera != this.camera)
		{
			int num5 = (NetworkSingleton<ManagerPhysicsObject>.Instance.SpinRotationIndexFromGrabbable(PlayerScript.PointerScript.gameObject, (float)PlayerScript.PointerScript.RotationSnap) - NetworkSingleton<ManagerPhysicsObject>.Instance.SpinRotationIndexFromGrabbable(hand.gameObject, (float)PlayerScript.PointerScript.RotationSnap) - 12) % 24;
			if (num5 != 0)
			{
				PlayerScript.PointerScript.ChangeHeldSpinRotationIndex(num5, -1);
			}
		}
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x00067C88 File Offset: 0x00065E88
	public HandZone GetHand()
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
		HandZone handZone = null;
		if (playerState != null)
		{
			handZone = HandZone.GetHandZone(playerState.stringColor, 0, true);
			if (handZone && handZone.bDisabled)
			{
				handZone = null;
			}
		}
		return handZone;
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x00067CC8 File Offset: 0x00065EC8
	public Rect SizeToRect(float ySize, float boundsAspectRatio)
	{
		float num = (float)Screen.width / (float)Screen.height;
		float x = ySize / num * boundsAspectRatio;
		return this.SizeToRect(new Vector2(x, ySize));
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x00067CF6 File Offset: 0x00065EF6
	public Rect SizeToRect(Vector2 size)
	{
		return new Rect(new Vector2(0.5f - size.x / 2f, 0f), new Vector2(size.x, size.y));
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x00067D2C File Offset: 0x00065F2C
	public override bool Contains(Vector2 screenPos)
	{
		if (this.camera.gameObject.activeSelf && this.GetPixelInteractionRect().Contains(screenPos))
		{
			bool flag;
			bool result = this.HoldingHandObjects(out flag);
			if (flag)
			{
				return result;
			}
			HandZone hand = this.GetHand();
			if (hand)
			{
				List<NetworkPhysicsObject> handObjects = hand.GetHandObjects(false);
				int num;
				RaycastHit[] array = HoverScript.RaySphereCast(this.camera.ScreenPointToRay(screenPos), out num, 0.1f, 50f, HoverScript.GrabbableLayerMask);
				for (int i = 0; i < num; i++)
				{
					RaycastHit raycastHit = array[i];
					NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(raycastHit.collider);
					if (networkPhysicsObject && handObjects.Contains(networkPhysicsObject))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x00067DF8 File Offset: 0x00065FF8
	public Rect GetPixelInteractionRect()
	{
		HandZone hand = this.GetHand();
		bool flag = hand && hand.HasHandObjects;
		float num = 0.25f;
		if (flag)
		{
			num = Mathf.Clamp(HandCamera.MINIMIZED_SIZE + 0.01f, 0.25f, 1f);
		}
		float y = this.handInInteraction ? this.screenHeight : (this.screenHeight * num);
		Rect rect = this.SizeToRect(new Vector2(this.screenWidth, y));
		return new Rect(rect.position.x * (float)Screen.width, rect.position.y * (float)Screen.height, rect.size.x * (float)Screen.width, rect.size.y * (float)Screen.height);
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00067EC0 File Offset: 0x000660C0
	private bool HoldingHandObjects()
	{
		bool flag;
		return this.HoldingHandObjects(out flag);
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00067ED8 File Offset: 0x000660D8
	private bool HoldingHandObjects(out bool boolHoldingAnything)
	{
		boolHoldingAnything = false;
		if (PlayerScript.Pointer)
		{
			List<GameObject> grabbedObjects = PlayerScript.PointerScript.GrabbedObjects;
			for (int i = 0; i < grabbedObjects.Count; i++)
			{
				if (grabbedObjects[i])
				{
					NetworkPhysicsObject component = grabbedObjects[i].GetComponent<NetworkPhysicsObject>();
					if (component)
					{
						boolHoldingAnything = true;
						if (component.CanBeHeldInHand)
						{
							return true;
						}
					}
				}
			}
			if (zInput.GetButton("Grab", ControlType.All))
			{
				List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
				for (int j = 0; j < grabbableNPOs.Count; j++)
				{
					NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[j];
					if (networkPhysicsObject && networkPhysicsObject.HeldByPlayerID == NetworkID.ID)
					{
						boolHoldingAnything = true;
						if (networkPhysicsObject.CanBeHeldInHand)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04000967 RID: 2407
	public UISprite HandSprite;

	// Token: 0x04000968 RID: 2408
	public bool Enabled = true;

	// Token: 0x04000969 RID: 2409
	private float maxBoundsWidth = 5.05f;

	// Token: 0x0400096A RID: 2410
	private float sizeMulti = 0.505f;

	// Token: 0x0400096B RID: 2411
	private float screenHeight = 0.2f;

	// Token: 0x0400096C RID: 2412
	private float screenWidth = 0.6f;

	// Token: 0x0400096D RID: 2413
	private float BoundsY;

	// Token: 0x0400096F RID: 2415
	public bool handInInteraction;

	// Token: 0x04000970 RID: 2416
	private UIStretch handStretch;

	// Token: 0x04000971 RID: 2417
	private float slideAmount;

	// Token: 0x04000972 RID: 2418
	public static float MINIMIZED_SIZE = 0.26f;

	// Token: 0x04000973 RID: 2419
	private float hideVelocity;

	// Token: 0x04000974 RID: 2420
	private float showVelocity;
}
