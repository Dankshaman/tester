using System;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class UIThemeButton : MonoBehaviour
{
	// Token: 0x060027FC RID: 10236 RVA: 0x0011A9EA File Offset: 0x00118BEA
	public void LoadThisTheme()
	{
		Singleton<UIThemeEditor>.Instance.LoadTheme(this.ID, this.Name);
	}

	// Token: 0x060027FD RID: 10237 RVA: 0x0011AA02 File Offset: 0x00118C02
	public void DeleteThisTheme()
	{
		Singleton<UIThemeEditor>.Instance.AskDeleteTheme(this.ID);
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x0011AA14 File Offset: 0x00118C14
	public void RenameThisTheme()
	{
		Singleton<UIThemeEditor>.Instance.AskRenameTheme(this.ID);
	}

	// Token: 0x04001A3F RID: 6719
	public string Name;

	// Token: 0x04001A40 RID: 6720
	public int ID;

	// Token: 0x04001A41 RID: 6721
	public GameObject CurrentIndicator;

	// Token: 0x04001A42 RID: 6722
	public GameObject DeleteButton;

	// Token: 0x04001A43 RID: 6723
	public GameObject RenameButton;
}
