using System;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class UIPointerLiftHeight : MonoBehaviour
{
	// Token: 0x0600267D RID: 9853 RVA: 0x0011278C File Offset: 0x0011098C
	private void Awake()
	{
		this.ThisUIButton = base.GetComponent<UIButton>();
		this.SliderSlider = base.GetComponentInChildren<UISlider>();
		this.Slider = this.SliderSlider.gameObject;
		this.Slider.SetActive(false);
		EventManager.OnChangePlayerColor += this.OnPlayerChangedColor;
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x001127DF File Offset: 0x001109DF
	private void OnDestroy()
	{
		EventManager.OnChangePlayerColor -= this.OnPlayerChangedColor;
	}

	// Token: 0x0600267F RID: 9855 RVA: 0x001127F4 File Offset: 0x001109F4
	private void OnPlayerChangedColor(Color newColor, int id)
	{
		if (id != NetworkID.ID)
		{
			return;
		}
		NGUIHelper.ButtonDisable(this.ThisUIButton, new Colour(newColor) == Colour.Grey, null, null);
	}

	// Token: 0x06002680 RID: 9856 RVA: 0x00112837 File Offset: 0x00110A37
	private void OnClick()
	{
		if (PlayerScript.Pointer)
		{
			if (!this.Slider.activeSelf)
			{
				this.ShowSlider();
				return;
			}
			this.DisableSlider();
		}
	}

	// Token: 0x06002681 RID: 9857 RVA: 0x00112860 File Offset: 0x00110A60
	public void ShowSlider()
	{
		base.CancelInvoke("DisableSlider");
		if (!this.Slider.activeSelf)
		{
			this.SliderSlider.value = (PlayerScript.Pointer ? PlayerScript.PointerScript.RaiseHeight : 0.5f);
		}
		this.Slider.SetActive(true);
		base.Invoke("DisableSlider", 4f);
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x001128C9 File Offset: 0x00110AC9
	private void DisableSlider()
	{
		this.Slider.SetActive(false);
	}

	// Token: 0x04001918 RID: 6424
	private UIButton ThisUIButton;

	// Token: 0x04001919 RID: 6425
	private GameObject Slider;

	// Token: 0x0400191A RID: 6426
	private UISlider SliderSlider;
}
