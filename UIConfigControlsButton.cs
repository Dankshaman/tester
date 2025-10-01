using System;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class UIConfigControlsButton : MonoBehaviour
{
	// Token: 0x060021CC RID: 8652 RVA: 0x000F3724 File Offset: 0x000F1924
	private void OnClick()
	{
		if (!VRHMD.isVR)
		{
			base.GetComponentInChildren<UILabel>().text = "<Press Key>";
			cInput.ChangeKey(base.transform.parent.name, this.Primary ? 1 : 2);
			return;
		}
		string text = base.GetComponentInChildren<UILabel>().text;
		cInput.ChangeKey(base.transform.parent.name, this.Primary ? 1 : 2);
		if (text == "None")
		{
			base.GetComponentInChildren<UILabel>().text = "<Press Key>";
			return;
		}
		Wait.Frames(delegate
		{
			TTSInput.Override(KeyCode.Escape);
		}, 1);
	}

	// Token: 0x04001520 RID: 5408
	public bool Primary = true;
}
