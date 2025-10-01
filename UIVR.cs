using System;
using UnityEngine;

// Token: 0x02000359 RID: 857
public class UIVR : MonoBehaviour
{
	// Token: 0x060028AA RID: 10410 RVA: 0x0011F248 File Offset: 0x0011D448
	private void Start()
	{
		if (!VRHMD.isVR)
		{
			base.gameObject.SetActive(false);
			return;
		}
		EventDelegate.Add(this.ScaleUpButton.onClick, new EventDelegate.Callback(this.ScaleUp));
		EventDelegate.Add(this.ScaleDownButton.onClick, new EventDelegate.Callback(this.ScaleDown));
		EventDelegate.Add(this.FloorUpButton.onClick, new EventDelegate.Callback(this.FloorUp));
		EventDelegate.Add(this.FloorDownButton.onClick, new EventDelegate.Callback(this.FloorDown));
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x0011F2E0 File Offset: 0x0011D4E0
	private void OnDestroy()
	{
		EventDelegate.Remove(this.ScaleUpButton.onClick, new EventDelegate.Callback(this.ScaleUp));
		EventDelegate.Remove(this.ScaleDownButton.onClick, new EventDelegate.Callback(this.ScaleDown));
		EventDelegate.Remove(this.FloorUpButton.onClick, new EventDelegate.Callback(this.FloorUp));
		EventDelegate.Remove(this.FloorDownButton.onClick, new EventDelegate.Callback(this.FloorDown));
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x0011F361 File Offset: 0x0011D561
	private void ScaleUp()
	{
		Singleton<VRHMD>.Instance.Scale *= 1.1f;
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x0011F379 File Offset: 0x0011D579
	private void ScaleDown()
	{
		Singleton<VRHMD>.Instance.Scale /= 1.1f;
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x0011F391 File Offset: 0x0011D591
	private void FloorUp()
	{
		Singleton<VRHMD>.Instance.Floor += 2f;
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x0011F3A9 File Offset: 0x0011D5A9
	private void FloorDown()
	{
		Singleton<VRHMD>.Instance.Floor -= 2f;
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x0011F3C1 File Offset: 0x0011D5C1
	public void ToggleKeyboard()
	{
		UIOnScreenKeyboard.ON_SCREEN_KEYBOARD = !UIOnScreenKeyboard.ON_SCREEN_KEYBOARD;
	}

	// Token: 0x060028B1 RID: 10417 RVA: 0x000A4771 File Offset: 0x000A2971
	public void ToggleBlindfold()
	{
		NetworkSingleton<PlayerManager>.Instance.ToggleBlindfold();
	}

	// Token: 0x04001AC6 RID: 6854
	public UIButton ScaleUpButton;

	// Token: 0x04001AC7 RID: 6855
	public UIButton ScaleDownButton;

	// Token: 0x04001AC8 RID: 6856
	public UIButton FloorUpButton;

	// Token: 0x04001AC9 RID: 6857
	public UIButton FloorDownButton;
}
