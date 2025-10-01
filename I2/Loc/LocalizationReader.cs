using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000483 RID: 1155
	public class LocalizationReader
	{
		// Token: 0x060033F6 RID: 13302 RVA: 0x0015E178 File Offset: 0x0015C378
		public static Dictionary<string, string> ReadTextAsset(TextAsset asset)
		{
			StringReader stringReader = new StringReader(Encoding.UTF8.GetString(asset.bytes, 0, asset.bytes.Length).Replace("\r\n", "\n").Replace("\r", "\n"));
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
			string line;
			while ((line = stringReader.ReadLine()) != null)
			{
				string text;
				string value;
				string text2;
				string text3;
				string text4;
				if (LocalizationReader.TextAsset_ReadLine(line, out text, out value, out text2, out text3, out text4) && !string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(value))
				{
					dictionary[text] = value;
				}
			}
			return dictionary;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x0015E208 File Offset: 0x0015C408
		public static bool TextAsset_ReadLine(string line, out string key, out string value, out string category, out string comment, out string termType)
		{
			key = string.Empty;
			category = string.Empty;
			comment = string.Empty;
			termType = string.Empty;
			value = string.Empty;
			int num = line.LastIndexOf("//");
			if (num >= 0)
			{
				comment = line.Substring(num + 2).Trim();
				comment = LocalizationReader.DecodeString(comment);
				line = line.Substring(0, num);
			}
			int num2 = line.IndexOf("=");
			if (num2 < 0)
			{
				return false;
			}
			key = line.Substring(0, num2).Trim();
			value = line.Substring(num2 + 1).Trim();
			value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
			value = LocalizationReader.DecodeString(value);
			if (key.Length > 2 && key[0] == '[')
			{
				int num3 = key.IndexOf(']');
				if (num3 >= 0)
				{
					termType = key.Substring(1, num3 - 1);
					key = key.Substring(num3 + 1);
				}
			}
			LocalizationReader.ValidateFullTerm(ref key);
			return true;
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x0015E314 File Offset: 0x0015C514
		public static string ReadCSVfile(string Path, Encoding encoding)
		{
			string text = string.Empty;
			using (StreamReader streamReader = new StreamReader(Path, encoding))
			{
				text = streamReader.ReadToEnd();
			}
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\r", "\n");
			return text;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0015E378 File Offset: 0x0015C578
		public static List<string[]> ReadCSV(string Text, char Separator = ',')
		{
			int i = 0;
			List<string[]> list = new List<string[]>();
			while (i < Text.Length)
			{
				string[] array = LocalizationReader.ParseCSVline(Text, ref i, Separator);
				if (array == null)
				{
					break;
				}
				list.Add(array);
			}
			return list;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x0015E3B0 File Offset: 0x0015C5B0
		private static string[] ParseCSVline(string Line, ref int iStart, char Separator)
		{
			List<string> list = new List<string>();
			int length = Line.Length;
			int num = iStart;
			bool flag = false;
			while (iStart < length)
			{
				char c = Line[iStart];
				if (flag)
				{
					if (c == '"')
					{
						if (iStart + 1 >= length || Line[iStart + 1] != '"')
						{
							flag = false;
						}
						else if (iStart + 2 < length && Line[iStart + 2] == '"')
						{
							flag = false;
							iStart += 2;
						}
						else
						{
							iStart++;
						}
					}
				}
				else if (c == '\n' || c == Separator)
				{
					LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
					if (c == '\n')
					{
						iStart++;
						break;
					}
				}
				else if (c == '"')
				{
					flag = true;
				}
				iStart++;
			}
			if (iStart > num)
			{
				LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
			}
			return list.ToArray();
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0015E47C File Offset: 0x0015C67C
		private static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
		{
			string text = Line.Substring(iWordStart, iEnd - iWordStart);
			iWordStart = iEnd + 1;
			text = text.Replace("\"\"", "\"");
			if (text.Length > 1 && text[0] == '"' && text[text.Length - 1] == '"')
			{
				text = text.Substring(1, text.Length - 2);
			}
			list.Add(text);
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x0015E4EC File Offset: 0x0015C6EC
		public static List<string[]> ReadI2CSV(string Text)
		{
			string[] separator = new string[]
			{
				"[*]"
			};
			string[] separator2 = new string[]
			{
				"[ln]"
			};
			List<string[]> list = new List<string[]>();
			foreach (string text in Text.Split(separator2, StringSplitOptions.None))
			{
				list.Add(text.Split(separator, StringSplitOptions.None));
			}
			return list;
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x0015E550 File Offset: 0x0015C750
		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			int num = Term.IndexOf('/');
			if (num < 0)
			{
				return;
			}
			int startIndex;
			while ((startIndex = Term.LastIndexOf('/')) != num)
			{
				Term = Term.Remove(startIndex, 1);
			}
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x0015E592 File Offset: 0x0015C792
		public static string EncodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("\r\n", "<\\n>").Replace("\r", "<\\n>").Replace("\n", "<\\n>");
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x0015E5D0 File Offset: 0x0015C7D0
		public static string DecodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("<\\n>", "\r\n");
		}
	}
}
