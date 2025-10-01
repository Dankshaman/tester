using System;
using MoonSharp.Interpreter;

// Token: 0x0200016C RID: 364
public class LuaBrowser : LuaComponent
{
	// Token: 0x0600117F RID: 4479 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaBrowser(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001180 RID: 4480 RVA: 0x00078D94 File Offset: 0x00076F94
	// (set) Token: 0x06001181 RID: 4481 RVA: 0x00078DF4 File Offset: 0x00076FF4
	public string url
	{
		get
		{
			if (this.LGOS.NPO.tabletScript && this.LGOS.NPO.tabletScript.browser)
			{
				return this.LGOS.NPO.tabletScript.browser.Url;
			}
			return "";
		}
		set
		{
			if (this.LGOS.NPO.tabletScript && this.LGOS.NPO.tabletScript.browser)
			{
				this.LGOS.NPO.tabletScript.browser.LoadURL(value, true);
			}
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001182 RID: 4482 RVA: 0x00078E50 File Offset: 0x00077050
	// (set) Token: 0x06001183 RID: 4483 RVA: 0x00078EB4 File Offset: 0x000770B4
	public int pixel_width
	{
		get
		{
			if (this.LGOS.NPO.tabletScript && this.LGOS.NPO.tabletScript.browser)
			{
				return (int)this.LGOS.NPO.tabletScript.browser.Size.x;
			}
			return 0;
		}
		set
		{
			int num = (int)((float)value * 0.6503906f);
			if (value > 0 && num > 0 && this.LGOS.NPO.tabletScript && this.LGOS.NPO.tabletScript.browser)
			{
				this.LGOS.NPO.tabletScript.browser.Resize(value, num);
			}
		}
	}
}
