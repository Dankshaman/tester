using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[RequireComponent(typeof(UIWidget))]
public class SetColorPickerColor : MonoBehaviour
{
	// Token: 0x060000A2 RID: 162 RVA: 0x00004EA7 File Offset: 0x000030A7
	public void SetToCurrent()
	{
		if (this.mWidget == null)
		{
			this.mWidget = base.GetComponent<UIWidget>();
		}
		if (UIColorPicker.current != null)
		{
			this.mWidget.color = UIColorPicker.current.value;
		}
	}

	// Token: 0x04000071 RID: 113
	[NonSerialized]
	private UIWidget mWidget;
}
