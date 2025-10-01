using System;
using System.Collections.Generic;
using System.Text;
using I2.Loc;

// Token: 0x02000255 RID: 597
public static class TextCode
{
	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06001F76 RID: 8054 RVA: 0x000E0A99 File Offset: 0x000DEC99
	public static List<string> LocalizationCodes
	{
		get
		{
			List<string> result;
			if ((result = TextCode._LocalizationCodes) == null)
			{
				result = (TextCode._LocalizationCodes = LocalizationManager.GetAllLanguagesCode(true, true));
			}
			return result;
		}
	}

	// Token: 0x06001F77 RID: 8055 RVA: 0x000E0AB1 File Offset: 0x000DECB1
	public static void CheckURLCodes(string url, out string cleanurl, out bool isVerifyCache, out bool isLocalization)
	{
		TextCode.CheckVerifyCacheCode(url, out cleanurl, out isVerifyCache);
		TextCode.CheckLocalizationCode(cleanurl, out cleanurl, out isLocalization, TextCode.HandleWhiteSpace.Trim);
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x000E0AC5 File Offset: 0x000DECC5
	public static void CheckVerifyCacheCode(string url, out string cleanurl, out bool isVerifyCache)
	{
		cleanurl = url;
		isVerifyCache = false;
		if (url.StartsWith("{verifycache}", StringComparison.OrdinalIgnoreCase))
		{
			cleanurl = url.Substring("{verifycache}".Length);
			isVerifyCache = true;
		}
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x000E0AF0 File Offset: 0x000DECF0
	private static string GetPartialLocalizationCode(string code)
	{
		string result;
		if (code.Contains("-"))
		{
			result = code.Split(new char[]
			{
				'-'
			})[0];
		}
		else
		{
			result = code.Replace("}", "");
		}
		return result;
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x000E0B34 File Offset: 0x000DED34
	public static void CheckLocalizationCode(string text, out string textLocalized, out bool isLocalization, TextCode.HandleWhiteSpace handleWhiteSpace = TextCode.HandleWhiteSpace.Trim)
	{
		textLocalized = text;
		isLocalization = false;
		if (text.Contains("{"))
		{
			int indexOfLocalizationCode = TextCode.GetIndexOfLocalizationCode(LocalizationManager.CurrentLanguageCode, text);
			if (indexOfLocalizationCode < 0)
			{
				foreach (string code in TextCode.LocalizationCodes)
				{
					indexOfLocalizationCode = TextCode.GetIndexOfLocalizationCode(code, text);
					if (indexOfLocalizationCode >= 0)
					{
						break;
					}
				}
			}
			if (indexOfLocalizationCode >= 0)
			{
				isLocalization = TextCode.GetLocalization(text, indexOfLocalizationCode, out textLocalized, handleWhiteSpace);
			}
		}
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000E0BC0 File Offset: 0x000DEDC0
	public static bool HasLocalizationCodes(string text)
	{
		if (text.Contains("{"))
		{
			using (List<string>.Enumerator enumerator = TextCode.LocalizationCodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TextCode.GetIndexOfLocalizationCode(enumerator.Current, text) >= 0)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000E0C28 File Offset: 0x000DEE28
	public static List<string> GetLocalizations(string text, TextCode.HandleWhiteSpace handleWhiteSpace = TextCode.HandleWhiteSpace.Trim)
	{
		List<string> list = new List<string>();
		foreach (string code in TextCode.LocalizationCodes)
		{
			int indexOfLocalizationCode = TextCode.GetIndexOfLocalizationCode(code, text);
			string item;
			if (indexOfLocalizationCode >= 0 && TextCode.GetLocalization(text, indexOfLocalizationCode, out item, handleWhiteSpace))
			{
				list.TryAddUnique(item);
			}
		}
		return list;
	}

	// Token: 0x06001F7D RID: 8061 RVA: 0x000E0C98 File Offset: 0x000DEE98
	private static bool GetLocalization(string text, int index, out string textLocalized, TextCode.HandleWhiteSpace handleWhiteSpace)
	{
		if (text[0] == '[')
		{
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				char c = text[i];
				if (c == '{')
				{
					break;
				}
				if (c == ']')
				{
					num = i;
				}
			}
			if (num > 0)
			{
				for (int j = 0; j < num + 1; j++)
				{
					char value = text[j];
					TextCode.stringBuilder.Append(value);
				}
			}
		}
		textLocalized = text;
		for (int k = index; k < text.Length; k++)
		{
			char c2 = text[k];
			if (c2 == '{')
			{
				break;
			}
			TextCode.stringBuilder.Append(c2);
		}
		if (TextCode.stringBuilder.Length > 0)
		{
			switch (handleWhiteSpace)
			{
			case TextCode.HandleWhiteSpace.None:
				break;
			case TextCode.HandleWhiteSpace.Trim:
				for (int l = 0; l < TextCode.stringBuilder.Length; l++)
				{
					char c3 = TextCode.stringBuilder[l];
					if (c3 != ' ' && c3 != '\n' && c3 != '\r')
					{
						break;
					}
					TextCode.stringBuilder.Remove(l, 1);
				}
				for (int m = TextCode.stringBuilder.Length - 1; m >= 0; m--)
				{
					char c4 = TextCode.stringBuilder[m];
					if (c4 != ' ' && c4 != '\n' && c4 != '\r')
					{
						break;
					}
					TextCode.stringBuilder.Remove(m, 1);
				}
				break;
			case TextCode.HandleWhiteSpace.TrimTrailingLineBreak:
			{
				char c5 = TextCode.stringBuilder[TextCode.stringBuilder.Length - 1];
				if (c5 == '\n' || c5 == '\r')
				{
					TextCode.stringBuilder.Remove(TextCode.stringBuilder.Length - 1, 1);
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("handleWhiteSpace", handleWhiteSpace, null);
			}
			textLocalized = TextCode.stringBuilder.ToString();
			TextCode.stringBuilder.Clear();
			return true;
		}
		return false;
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x000E0E54 File Offset: 0x000DF054
	private static int GetIndexOfLocalizationCode(string code, string text)
	{
		if (code.Length < 2)
		{
			return -1;
		}
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == '{')
			{
				int num = i + code.Length + 2;
				if (num < text.Length)
				{
					bool flag = true;
					for (int j = 0; j < code.Length; j++)
					{
						if (char.ToLower(text[i + 1 + j]) != char.ToLower(code[j]))
						{
							flag = false;
							break;
						}
					}
					if (flag && text[num - 1] == '}')
					{
						return num;
					}
				}
			}
		}
		int num2 = 2;
		for (int k = 0; k < text.Length; k++)
		{
			if (text[k] == '{')
			{
				int num3 = k + num2 + 2;
				if (num3 < text.Length)
				{
					bool flag2 = true;
					for (int l = 0; l < num2; l++)
					{
						if (char.ToLower(text[k + 1 + l]) != char.ToLower(code[l]))
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						if (text[num3 - 1] == '}')
						{
							return num3;
						}
						if (num3 + 3 < text.Length && text[num3 - 1] == '-')
						{
							if (text[num3 + 2] == '}')
							{
								return num3 + 3;
							}
							if (text[num3 + 3] == '}' && num3 + 4 < text.Length)
							{
								return num3 + 4;
							}
						}
					}
				}
			}
		}
		return -1;
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000E0FC8 File Offset: 0x000DF1C8
	public static bool LocalizeUIText(ref string text)
	{
		if (TextCode.HasLocalizationCodes(text))
		{
			bool flag;
			TextCode.CheckLocalizationCode(text, out text, out flag, TextCode.HandleWhiteSpace.TrimTrailingLineBreak);
			return true;
		}
		return false;
	}

	// Token: 0x04001350 RID: 4944
	private static List<string> _LocalizationCodes;

	// Token: 0x04001351 RID: 4945
	private const string VerifyCacheCode = "{verifycache}";

	// Token: 0x04001352 RID: 4946
	private static readonly StringBuilder stringBuilder = new StringBuilder();

	// Token: 0x020006F9 RID: 1785
	public enum HandleWhiteSpace
	{
		// Token: 0x04002A39 RID: 10809
		None,
		// Token: 0x04002A3A RID: 10810
		Trim,
		// Token: 0x04002A3B RID: 10811
		TrimTrailingLineBreak
	}
}
