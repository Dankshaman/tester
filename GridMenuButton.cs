using System;
using UnityEngine;

// Token: 0x020002D9 RID: 729
[Serializable]
public class GridMenuButton
{
	// Token: 0x040016AB RID: 5803
	public string Name;

	// Token: 0x040016AC RID: 5804
	public UIButton OpenButton;

	// Token: 0x040016AD RID: 5805
	public Color LightColor;

	// Token: 0x040016AE RID: 5806
	public Color DarkColor;

	// Token: 0x040016AF RID: 5807
	public UIButton AdditionalButton;

	// Token: 0x040016B0 RID: 5808
	public UIGridMenu GridMenu;

	// Token: 0x040016B1 RID: 5809
	public EventDelegate OnClick;

	// Token: 0x040016B2 RID: 5810
	public GameObject PrefabButton;

	// Token: 0x040016B3 RID: 5811
	[NonSerialized]
	public string CurrentSort;
}
