using System;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class UIPagesButton : MonoBehaviour
{
	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06002625 RID: 9765 RVA: 0x0010CF2A File Offset: 0x0010B12A
	// (set) Token: 0x06002626 RID: 9766 RVA: 0x0010CF32 File Offset: 0x0010B132
	public int Number { get; private set; }

	// Token: 0x06002627 RID: 9767 RVA: 0x0010CF3C File Offset: 0x0010B13C
	public void Set(int number, bool Highlight, int pageDisplayOffset = 0)
	{
		this.Label.text = (number + pageDisplayOffset).ToString();
		this.Number = number;
		this.Button.defaultColor = (Highlight ? Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonHighlightA] : Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal]);
	}

	// Token: 0x040018CE RID: 6350
	public UIButton Button;

	// Token: 0x040018CF RID: 6351
	public UILabel Label;
}
