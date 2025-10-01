using System;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class VRTransform : MonoBehaviour
{
	// Token: 0x060029DC RID: 10716 RVA: 0x0012A7F2 File Offset: 0x001289F2
	private void Start()
	{
		this.renderer = base.GetComponent<Renderer>();
		this.mat = this.renderer.material;
		this.renderer.enabled = false;
		this.startScale = base.transform.localScale;
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x0012A830 File Offset: 0x00128A30
	private void Update()
	{
		if (!this.Right.gameObject.activeSelf || !this.Left.gameObject.activeSelf)
		{
			return;
		}
		bool flag = false;
		bool flag2;
		bool flag3;
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.Old)
		{
			flag2 = this.Left.controller.GetPressGrip();
			flag3 = this.Right.controller.GetPressGrip();
			flag = (this.Right.controller.GetPressTrigger() || this.Left.controller.GetPressTrigger());
		}
		else
		{
			flag2 = this.Left.controller.IsMoveEnabled();
			flag3 = this.Right.controller.IsMoveEnabled();
		}
		bool flag4 = flag2 && flag3;
		bool flag5 = this.Right.currentMode == TrackedControllerMode.Default && this.Left.currentMode == TrackedControllerMode.Default;
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.Old)
		{
			if (!flag4 || flag || !flag5)
			{
				if (!flag)
				{
					switch (this.currentMode)
					{
					case VRTransformMode.Spin:
						Singleton<VRHMD>.Instance.Spin += base.transform.eulerAngles.y - this.InitialRotation.y;
						break;
					case VRTransformMode.Floor:
						Singleton<VRHMD>.Instance.Floor += (base.transform.position.y - this.InitialMidPoint.y) * 2f;
						break;
					case VRTransformMode.Scale:
						Singleton<VRHMD>.Instance.Scale *= this.renderer.transform.localScale.x / this.InitialScale.x;
						break;
					}
				}
				this.renderer.enabled = false;
				VRTransform.isTransforming = false;
				this.currentMode = VRTransformMode.None;
				this.DummyObject.SetActive(false);
				return;
			}
			VRTransform.isTransforming = true;
			this.distance = Vector3.Distance(this.Right.transform.position, this.Left.transform.position);
			this.midPoint = new Vector3((this.Right.transform.position.x + this.Left.transform.position.x) * 0.5f, (this.Right.transform.position.y + this.Left.transform.position.y) * 0.5f, (this.Right.transform.position.z + this.Left.transform.position.z) * 0.5f);
			this.line = new Vector3(this.Right.transform.position.x - this.Left.transform.position.x, this.Right.transform.position.y - this.Left.transform.position.y, this.Right.transform.position.z - this.Left.transform.position.z);
			Vector3 vector = new Vector3(this.line.x, this.InitialLine.y, this.line.z);
			float num = Vector3.Angle(this.InitialLine, vector);
			num *= Mathf.Sign(Vector3.Cross(this.InitialLine, vector).y);
			if (!this.renderer.enabled)
			{
				this.SetInitial(Singleton<VRHMD>.Instance.Head.transform.eulerAngles, this.startScale);
				return;
			}
			if (this.currentMode == VRTransformMode.None || this.currentMode == VRTransformMode.Spin)
			{
				base.transform.eulerAngles = new Vector3(0f, this.InitialRotation.y + num, 0f);
				if (Mathf.Abs(num) > 5f)
				{
					this.TurnOnDummy();
					this.currentMode = VRTransformMode.Spin;
					this.mat.color = Colour.Purple;
				}
			}
			if (this.currentMode == VRTransformMode.None || this.currentMode == VRTransformMode.Floor)
			{
				if (Mathf.Abs(this.midPoint.y - this.InitialMidPoint.y) > 1f * Singleton<VRHMD>.Instance.Scale || this.currentMode == VRTransformMode.Floor)
				{
					this.TurnOnDummy();
					this.currentMode = VRTransformMode.Floor;
					this.mat.color = Colour.Green;
					base.transform.position = new Vector3(base.transform.position.x, this.midPoint.y, base.transform.position.z);
				}
				else
				{
					base.transform.position = this.midPoint;
				}
			}
			if (this.currentMode == VRTransformMode.None || this.currentMode == VRTransformMode.Scale)
			{
				this.renderer.transform.localScale = this.InitialScale * (this.distance / this.InitialDistance);
				if (Mathf.Abs(this.distance - this.InitialDistance) > 1f * Singleton<VRHMD>.Instance.Scale)
				{
					this.TurnOnDummy();
					this.currentMode = VRTransformMode.Scale;
					this.mat.color = Colour.Blue;
					return;
				}
			}
		}
		else
		{
			if (flag4)
			{
				this.midPoint = new Vector3((this.Right.transform.position.x + this.Left.transform.position.x) * 0.5f, (this.Right.transform.position.y + this.Left.transform.position.y) * 0.5f, (this.Right.transform.position.z + this.Left.transform.position.z) * 0.5f);
				this.distance = Vector3.Distance(this.Right.transform.position, this.Left.transform.position);
				this.line = new Vector3(this.Right.transform.position.x - this.Left.transform.position.x, this.Right.transform.position.y - this.Left.transform.position.y, this.Right.transform.position.z - this.Left.transform.position.z);
				if (!VRTransform.isTransforming)
				{
					VRTransform.isTransforming = true;
					this.InitialVRHMDScale = 1f / Singleton<VRHMD>.Instance.VRCameraRig.localScale.x;
					this.InitialDistance = this.distance * this.InitialVRHMDScale;
					this.InitialPosition = this.midPoint * this.InitialVRHMDScale;
					this.InitialCameraRigPosition = Singleton<VRHMD>.Instance.VRCameraRig.position;
					this.InitialRotation = Singleton<VRHMD>.Instance.VRCameraRig.eulerAngles;
					this.InitialLine = this.line;
				}
				else
				{
					this.latestScaleRotate = Time.time;
					float num2 = 1f / Singleton<VRHMD>.Instance.VRCameraRig.localScale.x;
					num2 = Mathf.Clamp(this.InitialVRHMDScale * num2 * this.distance / this.InitialDistance, 0.01f, 1000f);
					float num3 = 1f / num2;
					this.DesiredScale = new Vector3(num3, num3, num3);
					this.DesiredPosition = this.InitialCameraRigPosition;
					if (this.distance * num2 > 0.2f)
					{
						Vector3 vector2 = new Vector3(this.line.x, this.InitialLine.y, this.line.z);
						float num4 = Vector3.Angle(this.InitialLine, vector2);
						num4 *= Mathf.Sign(Vector3.Cross(this.InitialLine, vector2).y);
						this.DesiredRotation = Singleton<VRHMD>.Instance.VRCameraRig.eulerAngles;
						this.DesiredRotation.y = this.DesiredRotation.y - num4;
					}
				}
			}
			else if (VRTransform.isTransforming)
			{
				VRTransform.isTransforming = false;
			}
			if (this.latestScaleRotate > 0f)
			{
				if (Time.time >= this.latestScaleRotate + 0.2f)
				{
					this.latestScaleRotate = 0f;
					return;
				}
				Singleton<VRHMD>.Instance.VRCameraRig.localScale = Vector3.Lerp(Singleton<VRHMD>.Instance.VRCameraRig.localScale, this.DesiredScale, VRTransform.ScaleLerp * Time.deltaTime);
				Singleton<VRHMD>.Instance.VRCameraRig.position = Vector3.Lerp(Singleton<VRHMD>.Instance.VRCameraRig.position, this.DesiredPosition, VRTransform.TranslateLerp * Time.deltaTime);
				Vector3 eulerAngles = Vector3.Lerp(Singleton<VRHMD>.Instance.VRCameraRig.eulerAngles, this.DesiredRotation, VRTransform.RotateLerp * Time.deltaTime);
				eulerAngles.x = Singleton<VRHMD>.Instance.VRCameraRig.eulerAngles.x;
				eulerAngles.z = Singleton<VRHMD>.Instance.VRCameraRig.eulerAngles.z;
				Singleton<VRHMD>.Instance.VRCameraRig.eulerAngles = eulerAngles;
			}
		}
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x0012B188 File Offset: 0x00129388
	private void SetInitial(Vector3 Rotation, Vector3 Scale)
	{
		this.mat.color = Colour.White;
		base.transform.position = this.midPoint;
		base.transform.localScale = Scale;
		base.transform.eulerAngles = new Vector3(0f, Rotation.y, 0f);
		this.renderer.enabled = true;
		this.InitialDistance = this.distance;
		this.InitialMidPoint = this.midPoint;
		this.InitialLine = this.line;
		this.InitialRotation = base.transform.eulerAngles;
		this.InitialScale = base.transform.localScale;
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x0012B23C File Offset: 0x0012943C
	private void TurnOnDummy()
	{
		if (!this.DummyObject.activeSelf)
		{
			this.SetInitial(base.transform.eulerAngles, base.transform.localScale);
			this.DummyObject.transform.position = base.transform.position;
			this.DummyObject.transform.rotation = base.transform.rotation;
			this.DummyObject.transform.localScale = base.transform.localScale;
			this.DummyObject.SetActive(true);
		}
	}

	// Token: 0x04001C80 RID: 7296
	public VRTrackedController Right;

	// Token: 0x04001C81 RID: 7297
	public VRTrackedController Left;

	// Token: 0x04001C82 RID: 7298
	public GameObject DummyObject;

	// Token: 0x04001C83 RID: 7299
	private VRTransformMode currentMode = VRTransformMode.None;

	// Token: 0x04001C84 RID: 7300
	private Renderer renderer;

	// Token: 0x04001C85 RID: 7301
	private Vector3 startScale;

	// Token: 0x04001C86 RID: 7302
	private Material mat;

	// Token: 0x04001C87 RID: 7303
	private float InitialDistance;

	// Token: 0x04001C88 RID: 7304
	private Vector3 InitialLine;

	// Token: 0x04001C89 RID: 7305
	private Vector3 InitialMidPoint;

	// Token: 0x04001C8A RID: 7306
	private Vector3 InitialRotation;

	// Token: 0x04001C8B RID: 7307
	private Vector3 InitialScale;

	// Token: 0x04001C8C RID: 7308
	private Vector3 InitialPosition;

	// Token: 0x04001C8D RID: 7309
	private Vector3 InitialCameraRigPosition;

	// Token: 0x04001C8E RID: 7310
	private float InitialVRHMDScale;

	// Token: 0x04001C8F RID: 7311
	private Vector3 midPoint;

	// Token: 0x04001C90 RID: 7312
	private float distance;

	// Token: 0x04001C91 RID: 7313
	private Vector3 line;

	// Token: 0x04001C92 RID: 7314
	private Vector3 DesiredPosition;

	// Token: 0x04001C93 RID: 7315
	private Vector3 DesiredRotation;

	// Token: 0x04001C94 RID: 7316
	private Vector3 DesiredScale;

	// Token: 0x04001C95 RID: 7317
	public static float TranslateLerp = 5f;

	// Token: 0x04001C96 RID: 7318
	public static float RotateLerp = 5f;

	// Token: 0x04001C97 RID: 7319
	public static float ScaleLerp = 5f;

	// Token: 0x04001C98 RID: 7320
	private const float runOn = 0.2f;

	// Token: 0x04001C99 RID: 7321
	private float latestScaleRotate;

	// Token: 0x04001C9A RID: 7322
	public static bool isTransforming = false;
}
