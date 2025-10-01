using System;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class LuaTextTool : LuaComponent
{
	// Token: 0x06001190 RID: 4496 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaTextTool(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x00078CE8 File Offset: 0x00076EE8
	public DynValue getValue()
	{
		return this.LGOS.GetValue();
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x00078CF5 File Offset: 0x00076EF5
	public bool setValue(DynValue Text)
	{
		return this.LGOS.SetValue(Text);
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x0007909E File Offset: 0x0007729E
	public int getFontSize()
	{
		return this.LGOS.NPO.textTool.input.label.fontSize;
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x000790BF File Offset: 0x000772BF
	public bool setFontSize(int FontSize)
	{
		this.LGOS.NPO.textTool.SetFontSize(FontSize);
		return true;
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x000790D8 File Offset: 0x000772D8
	public Color getFontColor()
	{
		return this.LGOS.NPO.textTool.input.label.color;
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x000790F9 File Offset: 0x000772F9
	public bool setFontColor(Color FontColor)
	{
		this.LGOS.NPO.textTool.SetColorFromColorPicker(FontColor);
		return true;
	}
}
