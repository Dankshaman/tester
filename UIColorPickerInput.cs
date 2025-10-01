using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200028D RID: 653
public class UIColorPickerInput : MonoBehaviour
{
	// Token: 0x0600217E RID: 8574 RVA: 0x000F15B8 File Offset: 0x000EF7B8
	private void OnEnable()
	{
		if (!this.ColorSprite)
		{
			this.ColorSprite = base.GetComponent<UISprite>();
		}
		if (!this.ClickButton)
		{
			this.ClickButton = base.GetComponent<UIButton>();
		}
		ColorPickerType colorPickerType = this.type;
		if (colorPickerType != ColorPickerType.ColorTint)
		{
			if (colorPickerType != ColorPickerType.TextTool)
			{
				return;
			}
			this.textTool = base.transform.parent.parent.parent.parent.GetComponent<TextTool>();
			this.SetColor(this.textTool.input.label.color);
		}
		else if (PlayerScript.Pointer && PlayerScript.PointerScript.InfoObject)
		{
			this.SetColor(PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().DiffuseColor);
			return;
		}
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x000F167E File Offset: 0x000EF87E
	private void Start()
	{
		EventDelegate.Add(this.ClickButton.onClick, new EventDelegate.Callback(this.onButtonClick));
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x000F169D File Offset: 0x000EF89D
	private void OnDestroy()
	{
		EventDelegate.Remove(this.ClickButton.onClick, new EventDelegate.Callback(this.onButtonClick));
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x000F16BC File Offset: 0x000EF8BC
	private void onButtonClick()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIColorPickerScript.Show(this.GetColor(), new Action<Color>(this.callback));
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x000F16E0 File Offset: 0x000EF8E0
	private void callback(Color value)
	{
		this.SetColor(value);
		switch (this.type)
		{
		case ColorPickerType.ColorTint:
			NetworkSingleton<NetworkUI>.Instance.GUIColorContextual(value);
			return;
		case ColorPickerType.TextTool:
			this.textTool.SetColorFromColorPicker(value);
			return;
		case ColorPickerType.None:
			break;
		case ColorPickerType.Theme:
			Singleton<UIThemeEditor>.Instance.ApplyThemeChange(this.Theme, value);
			Singleton<UIThemeEditor>.Instance.UpdateEditorSprites();
			return;
		case ColorPickerType.UserCallback:
			this.UserCallback(value);
			break;
		default:
			return;
		}
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x000F175C File Offset: 0x000EF95C
	public Color GetColor()
	{
		return this.ColorSprite.color;
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x000F1769 File Offset: 0x000EF969
	public void SetColor(Color color)
	{
		if (this.ColorSprite.color != color)
		{
			this.ColorSprite.color = color;
			this.TriggerColorChange(color);
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06002185 RID: 8581 RVA: 0x000F17A9 File Offset: 0x000EF9A9
	// (set) Token: 0x06002186 RID: 8582 RVA: 0x000F17B1 File Offset: 0x000EF9B1
	public Color value
	{
		get
		{
			return this.GetColor();
		}
		set
		{
			this.SetColor(value);
		}
	}

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06002187 RID: 8583 RVA: 0x000F17BC File Offset: 0x000EF9BC
	// (remove) Token: 0x06002188 RID: 8584 RVA: 0x000F17F4 File Offset: 0x000EF9F4
	public event UIColorPickerInput.OnColorChange ColorChanged;

	// Token: 0x06002189 RID: 8585 RVA: 0x000F1829 File Offset: 0x000EFA29
	public void TriggerColorChange(Color color)
	{
		if (this.ColorChanged != null)
		{
			this.ColorChanged(color);
		}
	}

	// Token: 0x040014CB RID: 5323
	public ColorPickerType type;

	// Token: 0x040014CC RID: 5324
	public UISprite ColorSprite;

	// Token: 0x040014CD RID: 5325
	public UIButton ClickButton;

	// Token: 0x040014CE RID: 5326
	public UIColorPickerInput.ColorCallback UserCallback;

	// Token: 0x040014CF RID: 5327
	private TextTool textTool;

	// Token: 0x040014D0 RID: 5328
	[NonSerialized]
	public UIPalette.UI Theme;

	// Token: 0x040014D2 RID: 5330
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x0200070D RID: 1805
	// (Invoke) Token: 0x06003D87 RID: 15751
	public delegate void ColorCallback(Color color);

	// Token: 0x0200070E RID: 1806
	// (Invoke) Token: 0x06003D8B RID: 15755
	public delegate void OnColorChange(Color color);
}
