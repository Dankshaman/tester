using System;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class VRLaserPointer : MonoBehaviour
{
	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x0600293E RID: 10558 RVA: 0x00121E95 File Offset: 0x00120095
	// (set) Token: 0x0600293F RID: 10559 RVA: 0x00121EA1 File Offset: 0x001200A1
	public static float LASER_DOT_SIZE
	{
		get
		{
			return VRLaserPointer.LaserDotSize.x;
		}
		set
		{
			if (VRLaserPointer.LaserDotSize.x != value)
			{
				VRLaserPointer.LaserDotSize = new Vector3(value, value, value);
			}
		}
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x00121EC0 File Offset: 0x001200C0
	private void Start()
	{
		this.VRcontroller = base.GetComponent<VRTrackedController>();
		this.holder = new GameObject();
		this.holder.layer = this.LaserTransform.gameObject.layer;
		this.holder.transform.parent = this.LaserTransform;
		this.holder.transform.localPosition = Vector3.zero;
		this.holder.transform.localRotation = Quaternion.identity;
		this.pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.pointer.layer = this.holder.layer;
		this.pointer.transform.parent = this.holder.transform;
		this.pointer.transform.localScale = new Vector3(this.thickness, this.thickness, 100f);
		this.pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
		this.pointer.transform.localRotation = Quaternion.identity;
		BoxCollider component = this.pointer.GetComponent<BoxCollider>();
		if (this.addRigidBody)
		{
			if (component)
			{
				component.isTrigger = true;
			}
			this.pointer.AddComponent<Rigidbody>().isKinematic = true;
		}
		else if (component)
		{
			UnityEngine.Object.Destroy(component);
		}
		this.mat = new Material(this.laserBeamMaterial);
		this.pointer.GetComponent<MeshRenderer>().material = this.mat;
		VRLaserPointer.raycastHitComparator = new RaycastHitComparator();
		this.sphereHitPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		this.sphereHitPoint.layer = this.holder.layer;
		this.sphereHitPoint.transform.parent = this.holder.transform;
		if (VRLaserPointer.LASER_DOT_SIZE == 0f)
		{
			VRLaserPointer.LASER_DOT_SIZE = 0.3f;
		}
		this.sphereHitPoint.transform.localScale = VRLaserPointer.LaserDotSize;
		this.sphereHitPoint.GetComponent<MeshRenderer>().material = this.mat;
		UnityEngine.Object.Destroy(this.sphereHitPoint.GetComponent<Collider>());
		this.SetColor(this.color);
		this.startingLossyScale = this.holder.transform.lossyScale;
		this.pointer.SetActive(!this.HideLaser);
		this.SetCap(this.turnedOn);
		this.UpdateLaserVisiblity();
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x00122128 File Offset: 0x00120328
	private void LateUpdate()
	{
		if (Singleton<VRHMD>.Instance.VRUIScript && Singleton<VRHMD>.Instance.VRUIScript.Active)
		{
			Singleton<VRHMD>.Instance.VRUIScript.GetComponent<Collider>().enabled = true;
		}
		if (this.turnedOn)
		{
			if (!this.LaserTransform.gameObject.activeInHierarchy)
			{
				this.LaserTransform.gameObject.SetActive(true);
				this.SetCap(true);
			}
		}
		else if (this.LaserTransform.gameObject.activeInHierarchy)
		{
			this.LaserTransform.gameObject.SetActive(false);
			this.SetCap(false);
		}
		Transform transform = this.pointer.transform;
		Transform transform2 = this.sphereHitPoint.transform;
		this.HitPoint = Vector3.zero;
		this.UIHitPoint = Vector2.zero;
		this.UIHitDistance = 9999f;
		this.UIHitFirst = false;
		Vector3 vector = Vector3.zero;
		this.HitLockNetwork = null;
		this.HitNetwork = null;
		this.HitObject = null;
		this.UI3DHitObject = null;
		this.HitTrigger = null;
		float num = -1f;
		int num2 = Physics.RaycastNonAlloc(new Ray(this.LaserTransform.position, this.LaserTransform.forward), VRLaserPointer.raycast_hits, 1000f, HoverScript.GrabbableLayerMask, QueryTriggerInteraction.Collide);
		Array.Sort<RaycastHit>(VRLaserPointer.raycast_hits, 0, num2, VRLaserPointer.raycastHitComparator);
		float num3 = 99999f;
		for (int i = 0; i < num2; i++)
		{
			RaycastHit raycastHit = VRLaserPointer.raycast_hits[i];
			if (raycastHit.distance < num3)
			{
				num3 = raycastHit.distance;
				if (raycastHit.collider.gameObject.CompareTag("VR UI"))
				{
					this.UIHitFirst = true;
					this.UIHitDistance = num3;
				}
				else
				{
					this.UIHitFirst = false;
				}
			}
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(raycastHit.collider);
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject);
			if (networkPhysicsObject && networkPhysicsObject.zone && this.HitTrigger == null)
			{
				this.HitTrigger = gameObject;
			}
			if (gameObject.CompareTag(Pointer.PointerModeToTag(PointerMode.Hands)) && this.HitTrigger == null)
			{
				this.HitTrigger = gameObject;
			}
			if (networkPhysicsObject && networkPhysicsObject.IsGrabbable && !networkPhysicsObject.IsLocked && this.HitNetwork == null)
			{
				this.HitNetwork = networkPhysicsObject;
				if (num == -1f && networkPhysicsObject.IsHeldByNobody)
				{
					num = raycastHit.distance;
					if (vector == Vector3.zero)
					{
						vector = raycastHit.point;
					}
				}
				this.HitLockNetwork = networkPhysicsObject;
			}
			if (this.HitObject == null && !raycastHit.collider.gameObject.name.StartsWith("Bounds"))
			{
				this.HitObject = raycastHit.collider.transform;
			}
			if (Layers.IsUI3D(raycastHit.collider.gameObject))
			{
				this.UI3DHitObject = raycastHit.collider.gameObject;
			}
			if (raycastHit.collider.gameObject.CompareTag("VR UI"))
			{
				int num4 = (int)(raycastHit.textureCoord.x * (float)Screen.width);
				int num5 = (int)(raycastHit.textureCoord.y * (float)Screen.height);
				this.UIHitPoint = new Vector2((float)num4, (float)num5);
			}
			if ((!this.VRcontroller || !(raycastHit.collider.gameObject.name == "Bounds Mouse Floor 3.16") || this.VRcontroller.controller.GetPressTriggerDown() || (this.VRcontroller.currentMode != TrackedControllerMode.Default && this.VRcontroller.currentMode != TrackedControllerMode.UI)) && this.HitPoint == Vector3.zero)
			{
				bool flag = networkPhysicsObject && networkPhysicsObject.IsLocked;
				if (raycastHit.collider.gameObject.CompareTag("Surface") || raycastHit.collider.gameObject.CompareTag("VR UI") || flag)
				{
					if (num == -1f)
					{
						num = raycastHit.distance;
					}
					this.HitPoint = raycastHit.point;
					if (vector == Vector3.zero)
					{
						vector = raycastHit.point;
					}
				}
				if (flag && networkPhysicsObject.IsGrabbable && this.HitLockNetwork == null)
				{
					this.HitLockNetwork = networkPhysicsObject;
				}
			}
		}
		if (num == -1f)
		{
			num = 100f;
		}
		transform2.position = vector;
		if (this.UIHitFirst && this.UIHitDistance < 10f)
		{
			transform2.transform.localScale = VRLaserPointer.LaserDotSize / 2f;
		}
		else
		{
			this.sphereHitPoint.transform.localScale = VRLaserPointer.LaserDotSize;
		}
		num *= this.startingLossyScale.x / transform.transform.parent.lossyScale.x;
		transform.localScale = new Vector3(VRLaserPointer.LASER_BEAM_THICKNESS, VRLaserPointer.LASER_BEAM_THICKNESS, num);
		transform.localPosition = new Vector3(0f, 0f, num / 2f);
		if (Singleton<VRHMD>.Instance.VRUIScript && Singleton<VRHMD>.Instance.VRUIScript.VRUIAttached != VRControllerAttachment.Detached)
		{
			Singleton<VRHMD>.Instance.VRUIScript.GetComponent<Collider>().enabled = false;
		}
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x0012267C File Offset: 0x0012087C
	public void SetColor(Color color)
	{
		color.a = VRTrackedController.LASER_BEAM_ALPHA;
		if (this.mat.color != color)
		{
			this.mat.color = color;
			if (this.cap)
			{
				this.cap.GetComponent<Renderer>().material.color = color;
				this.cap.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
			}
		}
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x001226F2 File Offset: 0x001208F2
	public void SetCap(bool active)
	{
		if (this.cap)
		{
			this.cap.SetActive(this.turnedOn);
		}
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x00122712 File Offset: 0x00120912
	public void UpdateLaserVisiblity()
	{
		if (this.pointer)
		{
			this.pointer.GetComponent<Renderer>().enabled = VRTrackedController.LASER_BEAM_VISIBLE;
		}
	}

	// Token: 0x04001B3C RID: 6972
	private const float LASER_DOT_DEACTIVATION_THRESHOLD = 10f;

	// Token: 0x04001B3D RID: 6973
	public static float LASER_BEAM_THICKNESS = 0.05f;

	// Token: 0x04001B3E RID: 6974
	private static Vector3 LaserDotSize = Vector3.zero;

	// Token: 0x04001B3F RID: 6975
	public bool active = true;

	// Token: 0x04001B40 RID: 6976
	public Material laserBeamMaterial;

	// Token: 0x04001B41 RID: 6977
	public Color color;

	// Token: 0x04001B42 RID: 6978
	public float thickness = 0.002f;

	// Token: 0x04001B43 RID: 6979
	public GameObject holder;

	// Token: 0x04001B44 RID: 6980
	public GameObject pointer;

	// Token: 0x04001B45 RID: 6981
	public GameObject cap;

	// Token: 0x04001B46 RID: 6982
	public bool turnedOn;

	// Token: 0x04001B47 RID: 6983
	public bool addRigidBody;

	// Token: 0x04001B48 RID: 6984
	public Transform reference;

	// Token: 0x04001B49 RID: 6985
	public Transform HitObject;

	// Token: 0x04001B4A RID: 6986
	public NetworkPhysicsObject HitNetwork;

	// Token: 0x04001B4B RID: 6987
	public NetworkPhysicsObject HitLockNetwork;

	// Token: 0x04001B4C RID: 6988
	public Vector3 HitPoint;

	// Token: 0x04001B4D RID: 6989
	public Vector2 UIHitPoint;

	// Token: 0x04001B4E RID: 6990
	public float UIHitDistance;

	// Token: 0x04001B4F RID: 6991
	public bool UIHitFirst;

	// Token: 0x04001B50 RID: 6992
	public GameObject UI3DHitObject;

	// Token: 0x04001B51 RID: 6993
	public GameObject HitTrigger;

	// Token: 0x04001B52 RID: 6994
	private VRTrackedController VRcontroller;

	// Token: 0x04001B53 RID: 6995
	public Transform LaserTransform;

	// Token: 0x04001B54 RID: 6996
	private Material mat;

	// Token: 0x04001B55 RID: 6997
	private static RaycastHitComparator raycastHitComparator;

	// Token: 0x04001B56 RID: 6998
	private GameObject sphereHitPoint;

	// Token: 0x04001B57 RID: 6999
	private Vector3 startingLossyScale;

	// Token: 0x04001B58 RID: 7000
	public bool HideLaser;

	// Token: 0x04001B59 RID: 7001
	private static RaycastHit[] raycast_hits = new RaycastHit[20];
}
