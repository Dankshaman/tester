using System;
using UnityEngine;

// Token: 0x0200029C RID: 668
public class UIConfigHotkeysButton : MonoBehaviour
{
	// Token: 0x060021E5 RID: 8677 RVA: 0x000F458C File Offset: 0x000F278C
	private void OnClick()
	{
		if (!VRHMD.isVR)
		{
			base.GetComponentInChildren<UILabel>().text = "<Press Key>";
			cInput.ChangeKey(base.transform.parent.GetComponent<UILabel>().name, this.Primary ? 1 : 2);
			return;
		}
		string text = base.GetComponentInChildren<UILabel>().text;
		cInput.ChangeKey(base.transform.parent.GetComponent<UILabel>().text, this.Primary ? 1 : 2);
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

	// Token: 0x04001550 RID: 5456
	public bool Primary = true;
}
