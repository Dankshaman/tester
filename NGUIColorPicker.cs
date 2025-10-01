using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class NGUIColorPicker : MonoBehaviour
{
	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06002098 RID: 8344 RVA: 0x000EB406 File Offset: 0x000E9606
	// (set) Token: 0x06002099 RID: 8345 RVA: 0x000EB410 File Offset: 0x000E9610
	public Color CurrentColor
	{
		get
		{
			return this.currentColor;
		}
		set
		{
			this.red = Mathf.Ceil(value.r * 255f);
			this.green = Mathf.Ceil(value.g * 255f);
			this.blue = Mathf.Ceil(value.b * 255f);
			this.alpha = Mathf.Ceil(value.a * 255f);
			this.currentColor = value;
			this.currentColorSprite.color = value;
			this.currentColorOpaqueSprite.color = new Color(value.r, value.g, value.b);
		}
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x000EB4B0 File Offset: 0x000E96B0
	private void Awake()
	{
		this.colorBox.thumbChanged += this.OnColorBoxChanged;
		this.hueSlider.thumbChanged += this.OnHueChanged;
		this.alphaSlider.thumbChanged += this.OnAlphaChanged;
		EventDelegate.Add(this.OkButton.onClick, new EventDelegate.Callback(this.OkClick));
		EventDelegate.Add(this.CancelButton.onClick, new EventDelegate.Callback(this.CancelClick));
		UIEventListener uieventListener = UIEventListener.Get(this.hueInput.gameObject);
		uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.HSBTextSelect));
		UIEventListener uieventListener2 = UIEventListener.Get(this.saturationInput.gameObject);
		uieventListener2.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onSelect, new UIEventListener.BoolDelegate(this.HSBTextSelect));
		UIEventListener uieventListener3 = UIEventListener.Get(this.brightnessInput.gameObject);
		uieventListener3.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener3.onSelect, new UIEventListener.BoolDelegate(this.HSBTextSelect));
		EventDelegate.Add(this.hueInput.onSubmit, new EventDelegate.Callback(this.HSBTextSubmit));
		EventDelegate.Add(this.saturationInput.onSubmit, new EventDelegate.Callback(this.HSBTextSubmit));
		EventDelegate.Add(this.brightnessInput.onSubmit, new EventDelegate.Callback(this.HSBTextSubmit));
		UIEventListener uieventListener4 = UIEventListener.Get(this.redInput.gameObject);
		uieventListener4.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener4.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		UIEventListener uieventListener5 = UIEventListener.Get(this.greenInput.gameObject);
		uieventListener5.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		UIEventListener uieventListener6 = UIEventListener.Get(this.blueInput.gameObject);
		uieventListener6.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener6.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		UIEventListener uieventListener7 = UIEventListener.Get(this.alphaInput.gameObject);
		uieventListener7.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener7.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		EventDelegate.Add(this.redInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		EventDelegate.Add(this.greenInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		EventDelegate.Add(this.blueInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		EventDelegate.Add(this.alphaInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		UIEventListener uieventListener8 = UIEventListener.Get(this.hexInput.gameObject);
		uieventListener8.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener8.onSelect, new UIEventListener.BoolDelegate(this.HexTextSelect));
		EventDelegate.Add(this.hexInput.onSubmit, new EventDelegate.Callback(this.HexTextSubmit));
		this.whitePalette.color = Colour.White;
		UIEventListener uieventListener9 = UIEventListener.Get(this.whitePalette.gameObject);
		uieventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener9.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.brownPalette.color = Colour.Brown;
		UIEventListener uieventListener10 = UIEventListener.Get(this.brownPalette.gameObject);
		uieventListener10.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener10.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.redPalette.color = Colour.Red;
		UIEventListener uieventListener11 = UIEventListener.Get(this.redPalette.gameObject);
		uieventListener11.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener11.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.orangePalette.color = Colour.Orange;
		UIEventListener uieventListener12 = UIEventListener.Get(this.orangePalette.gameObject);
		uieventListener12.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener12.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.yellowPalette.color = Colour.Yellow;
		UIEventListener uieventListener13 = UIEventListener.Get(this.yellowPalette.gameObject);
		uieventListener13.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener13.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.greenPalette.color = Colour.Green;
		UIEventListener uieventListener14 = UIEventListener.Get(this.greenPalette.gameObject);
		uieventListener14.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener14.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.tealPalette.color = Colour.Teal;
		UIEventListener uieventListener15 = UIEventListener.Get(this.tealPalette.gameObject);
		uieventListener15.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener15.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.bluePalette.color = Colour.Blue;
		UIEventListener uieventListener16 = UIEventListener.Get(this.bluePalette.gameObject);
		uieventListener16.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener16.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.purplePalette.color = Colour.Purple;
		UIEventListener uieventListener17 = UIEventListener.Get(this.purplePalette.gameObject);
		uieventListener17.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener17.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.pinkPalette.color = Colour.Pink;
		UIEventListener uieventListener18 = UIEventListener.Get(this.pinkPalette.gameObject);
		uieventListener18.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener18.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.greyPalette.color = Colour.Grey;
		UIEventListener uieventListener19 = UIEventListener.Get(this.greyPalette.gameObject);
		uieventListener19.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener19.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.blackPalette.color = Colour.Black;
		UIEventListener uieventListener20 = UIEventListener.Get(this.blackPalette.gameObject);
		uieventListener20.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener20.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		for (int i = 0; i < this.customPalette.Length; i++)
		{
			UIEventListener uieventListener21 = UIEventListener.Get(this.customPalette[i].gameObject);
			uieventListener21.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener21.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
			string @string = PlayerPrefs.GetString(this.customPalette[i].gameObject.name, "");
			int num = this.customPaletteNextIndex % this.customPalette.Length;
			if (@string != "")
			{
				this.setCustomPaletteColor(i, Colour.ColourFromRGBHex(@string));
			}
			else
			{
				this.setCustomPaletteColor(i, Colour.White);
			}
		}
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x000EBB95 File Offset: 0x000E9D95
	private void setCustomPaletteColor(int color_index, Colour colour)
	{
		this.customPalette[color_index].color = colour;
		colour.a = 1f;
		this.customPaletteBackgrounds[color_index].color = colour;
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x000EBBCC File Offset: 0x000E9DCC
	private void StoreNewRecentColor(Color color)
	{
		for (int i = 0; i < this.customPalette.Length; i++)
		{
			if (this.customPalette[i].color == color)
			{
				return;
			}
		}
		for (int j = this.customPalette.Length - 1; j > 0; j--)
		{
			this.setCustomPaletteColor(j, this.customPalette[j - 1].color);
			PlayerPrefs.SetString(this.customPalette[j].gameObject.name, new Colour(this.customPalette[j].color).RGBHex);
		}
		this.setCustomPaletteColor(0, color);
		PlayerPrefs.SetString(this.customPalette[0].gameObject.name, new Colour(this.customPalette[0].color).RGBHex);
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x000EBCA4 File Offset: 0x000E9EA4
	private void OnDestroy()
	{
		this.colorBox.thumbChanged -= this.OnColorBoxChanged;
		this.hueSlider.thumbChanged -= this.OnHueChanged;
		this.alphaSlider.thumbChanged -= this.OnAlphaChanged;
		EventDelegate.Remove(this.OkButton.onClick, new EventDelegate.Callback(this.OkClick));
		EventDelegate.Remove(this.CancelButton.onClick, new EventDelegate.Callback(this.CancelClick));
		UIEventListener uieventListener = UIEventListener.Get(this.hueInput.gameObject);
		uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.HSBTextSelect));
		UIEventListener uieventListener2 = UIEventListener.Get(this.saturationInput.gameObject);
		uieventListener2.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener2.onSelect, new UIEventListener.BoolDelegate(this.HSBTextSelect));
		UIEventListener uieventListener3 = UIEventListener.Get(this.brightnessInput.gameObject);
		uieventListener3.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener3.onSelect, new UIEventListener.BoolDelegate(this.HSBTextSelect));
		EventDelegate.Remove(this.hueInput.onSubmit, new EventDelegate.Callback(this.HSBTextSubmit));
		EventDelegate.Remove(this.saturationInput.onSubmit, new EventDelegate.Callback(this.HSBTextSubmit));
		EventDelegate.Remove(this.brightnessInput.onSubmit, new EventDelegate.Callback(this.HSBTextSubmit));
		UIEventListener uieventListener4 = UIEventListener.Get(this.redInput.gameObject);
		uieventListener4.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener4.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		UIEventListener uieventListener5 = UIEventListener.Get(this.greenInput.gameObject);
		uieventListener5.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener5.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		UIEventListener uieventListener6 = UIEventListener.Get(this.blueInput.gameObject);
		uieventListener6.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener6.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		UIEventListener uieventListener7 = UIEventListener.Get(this.alphaInput.gameObject);
		uieventListener7.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener7.onSelect, new UIEventListener.BoolDelegate(this.RGBTextSelect));
		EventDelegate.Remove(this.redInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		EventDelegate.Remove(this.greenInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		EventDelegate.Remove(this.blueInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		EventDelegate.Remove(this.alphaInput.onSubmit, new EventDelegate.Callback(this.RGBTextSubmit));
		UIEventListener uieventListener8 = UIEventListener.Get(this.hexInput.gameObject);
		uieventListener8.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener8.onSelect, new UIEventListener.BoolDelegate(this.HexTextSelect));
		EventDelegate.Remove(this.hexInput.onSubmit, new EventDelegate.Callback(this.HexTextSubmit));
		this.whitePalette.color = Colour.White;
		UIEventListener uieventListener9 = UIEventListener.Get(this.whitePalette.gameObject);
		uieventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener9.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.brownPalette.color = Colour.Brown;
		UIEventListener uieventListener10 = UIEventListener.Get(this.brownPalette.gameObject);
		uieventListener10.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener10.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.redPalette.color = Colour.Red;
		UIEventListener uieventListener11 = UIEventListener.Get(this.redPalette.gameObject);
		uieventListener11.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener11.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.orangePalette.color = Colour.Orange;
		UIEventListener uieventListener12 = UIEventListener.Get(this.orangePalette.gameObject);
		uieventListener12.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener12.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.yellowPalette.color = Colour.Yellow;
		UIEventListener uieventListener13 = UIEventListener.Get(this.yellowPalette.gameObject);
		uieventListener13.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener13.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.greenPalette.color = Colour.Green;
		UIEventListener uieventListener14 = UIEventListener.Get(this.greenPalette.gameObject);
		uieventListener14.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener14.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.tealPalette.color = Colour.Teal;
		UIEventListener uieventListener15 = UIEventListener.Get(this.tealPalette.gameObject);
		uieventListener15.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener15.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.bluePalette.color = Colour.Blue;
		UIEventListener uieventListener16 = UIEventListener.Get(this.bluePalette.gameObject);
		uieventListener16.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener16.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.purplePalette.color = Colour.Purple;
		UIEventListener uieventListener17 = UIEventListener.Get(this.purplePalette.gameObject);
		uieventListener17.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener17.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.pinkPalette.color = Colour.Pink;
		UIEventListener uieventListener18 = UIEventListener.Get(this.pinkPalette.gameObject);
		uieventListener18.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener18.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.greyPalette.color = Colour.Grey;
		UIEventListener uieventListener19 = UIEventListener.Get(this.greyPalette.gameObject);
		uieventListener19.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener19.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		this.blackPalette.color = Colour.Black;
		UIEventListener uieventListener20 = UIEventListener.Get(this.blackPalette.gameObject);
		uieventListener20.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener20.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		for (int i = 0; i < this.customPalette.Length; i++)
		{
			UIEventListener uieventListener21 = UIEventListener.Get(this.customPalette[i].gameObject);
			uieventListener21.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener21.onClick, new UIEventListener.VoidDelegate(this.OnPaletteClicked));
		}
	}

	// Token: 0x0600209E RID: 8350 RVA: 0x000EC32E File Offset: 0x000EA52E
	public void Show(Color startColor, Action<Color> callback)
	{
		this.callback = callback;
		this.SetToColor(startColor);
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x000EC34A File Offset: 0x000EA54A
	private void OkClick()
	{
		this.StoreNewRecentColor(this.CurrentColor);
		if (this.callback != null)
		{
			this.callback(this.CurrentColor);
		}
		this.callback = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x000EC384 File Offset: 0x000EA584
	private void CancelClick()
	{
		this.callback = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x000EC39C File Offset: 0x000EA59C
	private void OnHueChanged()
	{
		this.hue = Mathf.Lerp(0f, 360f, this.hueSlider.normY);
		this.colorBox.OnPickerChange();
		this.CurrentColor = NGUIColorPicker.ConvertHsvToRgba(this.hue, this.colorBox.thumbPosNormalized.x, this.colorBox.thumbPosNormalized.y, this.alpha / 255f);
		this.SetInputValues();
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x000EC418 File Offset: 0x000EA618
	private void OnAlphaChanged()
	{
		this.alpha = Mathf.Lerp(0f, 255f, this.alphaSlider.normY);
		this.colorBox.OnPickerChange();
		this.CurrentColor = NGUIColorPicker.ConvertHsvToRgba(this.hue, this.colorBox.thumbPosNormalized.x, this.colorBox.thumbPosNormalized.y, this.alpha / 255f);
		this.SetInputValues();
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x000EC494 File Offset: 0x000EA694
	private void OnColorBoxChanged()
	{
		this.saturation = Mathf.Lerp(0f, 100f, this.colorBox.thumbPosNormalized.x);
		this.brightness = Mathf.Lerp(0f, 100f, this.colorBox.thumbPosNormalized.y);
		this.hueSlider.OnPickerChange();
		this.alphaSlider.OnPickerChange();
		this.CurrentColor = NGUIColorPicker.ConvertHsvToRgba(this.hue, this.colorBox.thumbPosNormalized.x, this.colorBox.thumbPosNormalized.y, this.alpha / 255f);
		this.SetInputValues();
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x000EC544 File Offset: 0x000EA744
	private void RGBTextSelect(GameObject go, bool selected)
	{
		if (!selected)
		{
			this.RGBTextSubmit();
		}
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x000EC550 File Offset: 0x000EA750
	private void SetInputValues()
	{
		this.redInput.value = this.red.ToString("0");
		this.greenInput.value = this.green.ToString("0");
		this.blueInput.value = this.blue.ToString("0");
		this.alphaInput.value = this.alpha.ToString("0");
		this.hueInput.value = this.hue.ToString("0");
		this.saturationInput.value = this.saturation.ToString("0");
		this.brightnessInput.value = this.brightness.ToString("0");
		this.hexInput.value = this.ColorToHex(this.CurrentColor);
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x000EC636 File Offset: 0x000EA836
	private void UpdateSliders()
	{
		this.colorBox.OnPickerChange();
		this.hueSlider.OnPickerChange();
		this.alphaSlider.OnPickerChange();
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x000EC65C File Offset: 0x000EA85C
	private void RGBTextSubmit()
	{
		int num;
		int num2;
		int num3;
		int num4;
		if (int.TryParse(this.redInput.value, out num) && int.TryParse(this.greenInput.value, out num2) && int.TryParse(this.blueInput.value, out num3) && int.TryParse(this.alphaInput.value, out num4))
		{
			bool flag = false;
			if ((float)num != this.red)
			{
				this.red = (float)Mathf.Clamp(num, 0, 255);
				flag = true;
			}
			else if ((float)num2 != this.green)
			{
				this.green = (float)Mathf.Clamp(num2, 0, 255);
				flag = true;
			}
			else if ((float)num3 != this.blue)
			{
				this.blue = (float)Mathf.Clamp(num3, 0, 255);
				flag = true;
			}
			else if ((float)num4 != this.alpha)
			{
				this.alpha = (float)Mathf.Clamp(num4, 0, 255);
				flag = true;
			}
			if (flag)
			{
				this.SetToColor(new Color(this.red / 255f, this.green / 255f, this.blue / 255f, this.alpha / 255f));
			}
		}
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x000EC790 File Offset: 0x000EA990
	public void SetToColor(Color color)
	{
		this.CurrentColor = color;
		HsvColor hsvColor = NGUIColorPicker.ConvertRgbToHsv(this.CurrentColor);
		this.hue = hsvColor.H;
		this.saturation = hsvColor.S;
		this.brightness = hsvColor.V;
		this.UpdateSliders();
		this.SetInputValues();
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x000EC7E0 File Offset: 0x000EA9E0
	private void HSBTextSelect(GameObject go, bool selected)
	{
		if (!selected)
		{
			this.HSBTextSubmit();
		}
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000EC7EC File Offset: 0x000EA9EC
	private void HSBTextSubmit()
	{
		float num;
		float num2;
		float num3;
		if (float.TryParse(this.hueInput.value, out num) && float.TryParse(this.saturationInput.value, out num2) && float.TryParse(this.brightnessInput.value, out num3))
		{
			bool flag = false;
			if (num != this.hue)
			{
				this.hue = Mathf.Clamp(num, 0f, 360f);
				flag = true;
			}
			else if (num2 != this.saturation)
			{
				this.saturation = Mathf.Clamp(num2, 0f, 100f);
				flag = true;
			}
			else if (num3 != this.brightness)
			{
				this.brightness = Mathf.Clamp(num3, 0f, 100f);
				flag = true;
			}
			if (flag)
			{
				this.UpdateSliders();
				this.CurrentColor = NGUIColorPicker.ConvertHsvToRgba(this.hue, this.colorBox.thumbPosNormalized.x, this.colorBox.thumbPosNormalized.y, this.alpha / 255f);
				this.SetInputValues();
			}
		}
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x000EC8F3 File Offset: 0x000EAAF3
	private void HexTextSelect(GameObject go, bool selected)
	{
		if (!selected)
		{
			this.HexTextSubmit();
		}
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x000EC900 File Offset: 0x000EAB00
	private void HexTextSubmit()
	{
		Color toColor;
		if (NGUIColorPicker.HexToColor(this.hexInput.value, out toColor))
		{
			this.SetToColor(toColor);
		}
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x000EC928 File Offset: 0x000EAB28
	public static HsvColor ConvertRgbToHsv(Color color)
	{
		return NGUIColorPicker.ConvertRgbaToHsv((float)((int)(color.r * 255f)), (float)((int)(color.g * 255f)), (float)((int)(color.b * 255f)), color.a);
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x000EC960 File Offset: 0x000EAB60
	public static HsvColor ConvertRgbaToHsv(float r, float b, float g, float a)
	{
		float num = 0f;
		float num2 = Mathf.Min(Mathf.Min(r, g), b);
		float num3 = Mathf.Max(Mathf.Max(r, g), b);
		float num4 = num3 - num2;
		float num5;
		if ((double)num3 == 0.0)
		{
			num5 = 0f;
		}
		else
		{
			num5 = num4 / num3;
		}
		if (num5 == 0f)
		{
			num = 360f;
		}
		else
		{
			if (r == num3)
			{
				num = (g - b) / num4;
			}
			else if (g == num3)
			{
				num = 2f + (b - r) / num4;
			}
			else if (b == num3)
			{
				num = 4f + (r - g) / num4;
			}
			num *= 60f;
			if ((double)num <= 0.0)
			{
				num += 360f;
			}
		}
		HsvColor hsvColor = default(HsvColor);
		hsvColor.H = 360f - num;
		hsvColor.S = Mathf.Lerp(0f, 100f, num5);
		hsvColor.V = num3 / 255f;
		hsvColor.V = Mathf.Lerp(0f, 100f, hsvColor.V);
		hsvColor.A = a;
		return hsvColor;
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x000ECA74 File Offset: 0x000EAC74
	public static Color ConvertHsvToRgba(float h, float s, float v, float a)
	{
		double num;
		double num2;
		double num3;
		if (s == 0f)
		{
			num = (double)v;
			num2 = (double)v;
			num3 = (double)v;
		}
		else
		{
			if (h == 360f)
			{
				h = 0f;
			}
			else
			{
				h /= 60f;
			}
			int num4 = (int)h;
			double num5 = (double)(h - (float)num4);
			double num6 = (double)(v * (1f - s));
			double num7 = (double)v * (1.0 - (double)s * num5);
			double num8 = (double)v * (1.0 - (double)s * (1.0 - num5));
			switch (num4)
			{
			case 0:
				num = (double)v;
				num2 = num8;
				num3 = num6;
				break;
			case 1:
				num = num7;
				num2 = (double)v;
				num3 = num6;
				break;
			case 2:
				num = num6;
				num2 = (double)v;
				num3 = num8;
				break;
			case 3:
				num = num6;
				num2 = num7;
				num3 = (double)v;
				break;
			case 4:
				num = num8;
				num2 = num6;
				num3 = (double)v;
				break;
			default:
				num = (double)v;
				num2 = num6;
				num3 = num7;
				break;
			}
		}
		return new Color((float)num, (float)num2, (float)num3, a);
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x000ECB7F File Offset: 0x000EAD7F
	private string ColorToHex(Color32 color)
	{
		return string.Format("{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x000ECBAC File Offset: 0x000EADAC
	public static bool HexToColor(string hex, out Color color)
	{
		if (Regex.IsMatch(hex, "^#?(?:[0-9a-fA-F]{3,4}){1,2}$"))
		{
			int num = hex.StartsWith("#") ? 1 : 0;
			if (hex.Length == num + 8)
			{
				color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 6, 2), NumberStyles.AllowHexSpecifier));
			}
			else if (hex.Length == num + 6)
			{
				color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.MaxValue);
			}
			else if (hex.Length == num + 4)
			{
				color = new Color32(byte.Parse(hex[num].ToString() + hex[num].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 1].ToString() + hex[num + 1].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 2].ToString() + hex[num + 2].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 3].ToString() + hex[num + 3].ToString(), NumberStyles.AllowHexSpecifier));
			}
			else
			{
				color = new Color32(byte.Parse(hex[num].ToString() + hex[num].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 1].ToString() + hex[num + 1].ToString(), NumberStyles.AllowHexSpecifier), byte.Parse(hex[num + 2].ToString() + hex[num + 2].ToString(), NumberStyles.AllowHexSpecifier), byte.MaxValue);
			}
			return true;
		}
		color = default(Color32);
		return false;
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x000ECE46 File Offset: 0x000EB046
	private void OnPaletteClicked(GameObject go)
	{
		this.SetToColor(go.GetComponent<UISprite>().color);
	}

	// Token: 0x04001412 RID: 5138
	private const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";

	// Token: 0x04001413 RID: 5139
	public float hue;

	// Token: 0x04001414 RID: 5140
	public float saturation;

	// Token: 0x04001415 RID: 5141
	public float brightness;

	// Token: 0x04001416 RID: 5142
	public float red;

	// Token: 0x04001417 RID: 5143
	public float green;

	// Token: 0x04001418 RID: 5144
	public float blue;

	// Token: 0x04001419 RID: 5145
	public float alpha;

	// Token: 0x0400141A RID: 5146
	private Color currentColor;

	// Token: 0x0400141B RID: 5147
	public UIButton OkButton;

	// Token: 0x0400141C RID: 5148
	public UIButton CancelButton;

	// Token: 0x0400141D RID: 5149
	public UISprite currentColorSprite;

	// Token: 0x0400141E RID: 5150
	public UISprite currentColorOpaqueSprite;

	// Token: 0x0400141F RID: 5151
	public NGUISliderColorPicker hueSlider;

	// Token: 0x04001420 RID: 5152
	public NGUISliderColorPicker alphaSlider;

	// Token: 0x04001421 RID: 5153
	public NGUIColorPickerBox colorBox;

	// Token: 0x04001422 RID: 5154
	public UIInput hexInput;

	// Token: 0x04001423 RID: 5155
	public UIInput hueInput;

	// Token: 0x04001424 RID: 5156
	public UIInput saturationInput;

	// Token: 0x04001425 RID: 5157
	public UIInput brightnessInput;

	// Token: 0x04001426 RID: 5158
	public UIInput redInput;

	// Token: 0x04001427 RID: 5159
	public UIInput greenInput;

	// Token: 0x04001428 RID: 5160
	public UIInput blueInput;

	// Token: 0x04001429 RID: 5161
	public UIInput alphaInput;

	// Token: 0x0400142A RID: 5162
	public UISprite whitePalette;

	// Token: 0x0400142B RID: 5163
	public UISprite brownPalette;

	// Token: 0x0400142C RID: 5164
	public UISprite redPalette;

	// Token: 0x0400142D RID: 5165
	public UISprite orangePalette;

	// Token: 0x0400142E RID: 5166
	public UISprite yellowPalette;

	// Token: 0x0400142F RID: 5167
	public UISprite greenPalette;

	// Token: 0x04001430 RID: 5168
	public UISprite tealPalette;

	// Token: 0x04001431 RID: 5169
	public UISprite bluePalette;

	// Token: 0x04001432 RID: 5170
	public UISprite purplePalette;

	// Token: 0x04001433 RID: 5171
	public UISprite pinkPalette;

	// Token: 0x04001434 RID: 5172
	public UISprite greyPalette;

	// Token: 0x04001435 RID: 5173
	public UISprite blackPalette;

	// Token: 0x04001436 RID: 5174
	public UISprite[] customPalette;

	// Token: 0x04001437 RID: 5175
	public UISprite[] customPaletteBackgrounds;

	// Token: 0x04001438 RID: 5176
	private int customPaletteNextIndex;

	// Token: 0x04001439 RID: 5177
	private Action<Color> callback;
}
