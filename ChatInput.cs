using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class ChatInput : MonoBehaviour
{
	// Token: 0x0600007F RID: 127 RVA: 0x00004614 File Offset: 0x00002814
	private void Start()
	{
		this.mInput = base.GetComponent<UIInput>();
		this.mInput.label.maxLineCount = 1;
		if (this.fillWithDummyData && this.textList != null)
		{
			for (int i = 0; i < 30; i++)
			{
				this.textList.Add(string.Concat(new object[]
				{
					(i % 2 == 0) ? "[FFFFFF]" : "[AAAAAA]",
					"This is an example paragraph for the text list, testing line ",
					i,
					"[-]"
				}));
			}
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000046A4 File Offset: 0x000028A4
	public void OnSubmit()
	{
		if (this.textList != null)
		{
			string text = NGUIText.StripSymbols(this.mInput.value);
			if (!string.IsNullOrEmpty(text))
			{
				this.textList.Add(text);
				this.mInput.value = "";
				this.mInput.isSelected = false;
			}
		}
	}

	// Token: 0x0400004F RID: 79
	public UITextList textList;

	// Token: 0x04000050 RID: 80
	public bool fillWithDummyData;

	// Token: 0x04000051 RID: 81
	private UIInput mInput;
}
