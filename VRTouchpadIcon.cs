using System;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class VRTouchpadIcon : MonoBehaviour
{
	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06002988 RID: 10632 RVA: 0x001234B8 File Offset: 0x001216B8
	// (set) Token: 0x06002989 RID: 10633 RVA: 0x001234C0 File Offset: 0x001216C0
	public PointerMode AttachedTool
	{
		get
		{
			return this._AttachedTool;
		}
		set
		{
			if (this._AttachedTool != value)
			{
				this._AttachedTool = value;
				this.flareState = VRTouchpadIcon.FlareState.Unflaring;
			}
		}
	}

	// Token: 0x0600298A RID: 10634 RVA: 0x001234DC File Offset: 0x001216DC
	private void Awake()
	{
		this.InitialPosition = base.transform.localPosition;
		this.InitialScale = base.transform.localScale;
		this.InitialColor = Colour.UnityBlack;
		this.InitialEmission = Colour.UnityBlack;
		this.FlareDuration = 0.125f;
		this.UnflareDuration = 0.075f;
		this.FlaredPosition = this.InitialPosition;
		this.FlaredPosition.z = this.FlaredPosition.z - 0.005f;
		this.FlaredScale = new Vector3(0.02f * Mathf.Sign(this.InitialScale.x), 0.02f * Mathf.Sign(this.InitialScale.y), 0.02f * Mathf.Sign(this.InitialScale.z));
		this.flareState = VRTouchpadIcon.FlareState.Unflaring;
		this.iconMaterial = base.GetComponent<Renderer>().material;
	}

	// Token: 0x0600298B RID: 10635 RVA: 0x001235C4 File Offset: 0x001217C4
	private void Update()
	{
		if (VRTrackedController.ControllerStyle == TrackedControllerStyle.Old)
		{
			this.Touching = false;
		}
		else if (this.touchUntil != 0f)
		{
			if (this.touchUntil > Time.time)
			{
				this.Touching = true;
			}
			else
			{
				this.touchUntil = 0f;
				this.Touching = false;
			}
		}
		switch (this.flareState)
		{
		case VRTouchpadIcon.FlareState.Dormant:
			if (this.Touching)
			{
				this.flareState = VRTouchpadIcon.FlareState.Flaring;
				return;
			}
			break;
		case VRTouchpadIcon.FlareState.Flaring:
			if (!this.Touching)
			{
				this.flareState = VRTouchpadIcon.FlareState.Unflaring;
				return;
			}
			this.progress += Time.deltaTime / this.FlareDuration;
			if (this.progress >= 1f)
			{
				this.progress = 1f;
				this.flareState = VRTouchpadIcon.FlareState.Flared;
			}
			this.UpdateIcon();
			return;
		case VRTouchpadIcon.FlareState.Unflaring:
			if (this.Touching)
			{
				this.flareState = VRTouchpadIcon.FlareState.Flaring;
				return;
			}
			this.progress -= Time.deltaTime / this.UnflareDuration;
			if (this.progress <= 0f)
			{
				this.progress = 0f;
				this.flareState = VRTouchpadIcon.FlareState.Dormant;
			}
			this.UpdateIcon();
			break;
		case VRTouchpadIcon.FlareState.Flared:
			if (!this.Touching)
			{
				this.flareState = VRTouchpadIcon.FlareState.Unflaring;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600298C RID: 10636 RVA: 0x001236F8 File Offset: 0x001218F8
	private void UpdateIcon()
	{
		base.transform.localPosition = Vector3.Lerp(this.InitialPosition, this.FlaredPosition, this.progress);
		base.transform.localScale = Vector3.Lerp(this.InitialScale, this.FlaredScale, this.progress);
		this.iconMaterial.color = Colour.Lerp(this.InitialColor.WithAlpha(VRTrackedController.IconAlpha), this.FlaredColor, this.progress);
		this.iconMaterial.SetColor("_EmissionColor", Colour.Lerp(this.InitialEmission, this.FlaredEmission, this.progress));
	}

	// Token: 0x0600298D RID: 10637 RVA: 0x001237A6 File Offset: 0x001219A6
	public void HideIcon()
	{
		base.GetComponent<Renderer>().enabled = false;
	}

	// Token: 0x0600298E RID: 10638 RVA: 0x001237B4 File Offset: 0x001219B4
	public void SetIcon(VRControlIcon controlIcon)
	{
		this.SetIcon(Singleton<VRIconRepo>.Instance.GetControlMaterial(controlIcon), false);
	}

	// Token: 0x0600298F RID: 10639 RVA: 0x001237C8 File Offset: 0x001219C8
	public void SetIcon(PointerMode pointerMode)
	{
		this.SetIcon(Singleton<VRIconRepo>.Instance.GetToolMaterial(pointerMode), true);
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x001237DC File Offset: 0x001219DC
	private void SetIcon(Material material, bool isTool)
	{
		if (material)
		{
			base.GetComponent<Renderer>().enabled = true;
			base.GetComponent<Renderer>().material = material;
			this.iconMaterial = material;
			if (isTool)
			{
				this.FlaredColor = Colour.White;
				this.FlaredEmission = Colour.UnityBlack;
			}
			else
			{
				this.FlaredColor = new Colour(206, 248, byte.MaxValue);
				this.FlaredEmission = new Colour(67, 130, 193);
			}
			this.flareState = VRTouchpadIcon.FlareState.Unflaring;
		}
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x00123863 File Offset: 0x00121A63
	public void FakeTouch(float duration)
	{
		this.touchUntil = Time.time + duration;
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x00123872 File Offset: 0x00121A72
	public void SetFlarePosition(Vector3 offset)
	{
		this.FlaredPosition = this.InitialPosition + offset;
		this.flareState = VRTouchpadIcon.FlareState.Flaring;
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x0012388D File Offset: 0x00121A8D
	public void SetInitialColour(Colour colour, Colour? emission = null)
	{
		this.InitialColor = colour;
		if (emission != null)
		{
			this.InitialEmission = emission.Value;
		}
		this.flareState = VRTouchpadIcon.FlareState.Flaring;
	}

	// Token: 0x04001B92 RID: 7058
	public const float FLARE_DURATION = 0.125f;

	// Token: 0x04001B93 RID: 7059
	public const float UNFLARE_DURATION = 0.075f;

	// Token: 0x04001B94 RID: 7060
	private const float FLARE_HEIGHT = 0.005f;

	// Token: 0x04001B95 RID: 7061
	private const float FLARE_SCALE = 0.02f;

	// Token: 0x04001B96 RID: 7062
	private Vector3 InitialPosition;

	// Token: 0x04001B97 RID: 7063
	private Vector3 InitialScale;

	// Token: 0x04001B98 RID: 7064
	private Colour InitialColor;

	// Token: 0x04001B99 RID: 7065
	private Colour InitialEmission;

	// Token: 0x04001B9A RID: 7066
	private Vector3 FlaredPosition;

	// Token: 0x04001B9B RID: 7067
	private Vector3 FlaredScale;

	// Token: 0x04001B9C RID: 7068
	private Colour FlaredColor;

	// Token: 0x04001B9D RID: 7069
	private Colour FlaredEmission;

	// Token: 0x04001B9E RID: 7070
	public float FlareDuration;

	// Token: 0x04001B9F RID: 7071
	public float UnflareDuration;

	// Token: 0x04001BA0 RID: 7072
	private VRTouchpadIcon.FlareState flareState;

	// Token: 0x04001BA1 RID: 7073
	private float progress;

	// Token: 0x04001BA2 RID: 7074
	public bool Touching;

	// Token: 0x04001BA3 RID: 7075
	private float touchUntil;

	// Token: 0x04001BA4 RID: 7076
	private PointerMode _AttachedTool = PointerMode.None;

	// Token: 0x04001BA5 RID: 7077
	private Material iconMaterial;

	// Token: 0x020007AE RID: 1966
	public enum FlareState
	{
		// Token: 0x04002D05 RID: 11525
		Dormant,
		// Token: 0x04002D06 RID: 11526
		Flaring,
		// Token: 0x04002D07 RID: 11527
		Unflaring,
		// Token: 0x04002D08 RID: 11528
		Flared
	}
}
