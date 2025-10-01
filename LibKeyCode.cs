using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public static class LibKeyCode
{
	// Token: 0x060010C2 RID: 4290 RVA: 0x000741BE File Offset: 0x000723BE
	public static KeyCode KeyCodeFromString(string s)
	{
		return (KeyCode)Enum.Parse(typeof(KeyCode), LibString.CamelCaseFromUnderscore(s, true, false));
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x000741DC File Offset: 0x000723DC
	public static ModifiedKeyCode ModifiedKeyCodeFromString(string s)
	{
		ModifiedKeyCode result = new ModifiedKeyCode(KeyCode.None, false, false, false);
		bool flag = false;
		string text = s.ToLower();
		while (!flag)
		{
			if (text.StartsWith("ctrl+"))
			{
				result.ctrl = true;
				text = text.Substring(5);
			}
			else if (text.StartsWith("control+"))
			{
				result.ctrl = true;
				text = text.Substring(8);
			}
			else if (text.StartsWith("shift+"))
			{
				result.shift = true;
				text = text.Substring(6);
			}
			else if (text.StartsWith("alt+"))
			{
				result.alt = true;
				text = text.Substring(4);
			}
			else
			{
				flag = true;
			}
		}
		result.keyCode = LibKeyCode.KeyCodeFromString(text);
		return result;
	}
}
