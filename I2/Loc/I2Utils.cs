using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x020004A4 RID: 1188
	public static class I2Utils
	{
		// Token: 0x0600352C RID: 13612 RVA: 0x001627C4 File Offset: 0x001609C4
		public static string ReverseText(string source)
		{
			int length = source.Length;
			char[] array = new char[length];
			for (int i = 0; i < length; i++)
			{
				array[length - 1 - i] = source[i];
			}
			return new string(array);
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x00162800 File Offset: 0x00160A00
		public static string RemoveNonASCII(string text, bool allowCategory = false)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			int num = 0;
			char[] array = new char[text.Length];
			bool flag = false;
			foreach (char c in text.Trim().ToCharArray())
			{
				char c2 = ' ';
				if ((allowCategory && (c == '\\' || c == '"' || c == '/')) || char.IsLetterOrDigit(c) || ".-_$#@*()[]{}+:?!&',^=<>~`".IndexOf(c) >= 0)
				{
					c2 = c;
				}
				if (char.IsWhiteSpace(c2))
				{
					if (!flag)
					{
						if (num > 0)
						{
							array[num++] = ' ';
						}
						flag = true;
					}
				}
				else
				{
					flag = false;
					array[num++] = c2;
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x001628B0 File Offset: 0x00160AB0
		public static string GetValidTermName(string text, bool allowCategory = false)
		{
			if (text == null)
			{
				return null;
			}
			text = I2Utils.RemoveTags(text);
			return I2Utils.RemoveNonASCII(text, allowCategory);
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x001628C8 File Offset: 0x00160AC8
		public static string SplitLine(string line, int maxCharacters)
		{
			if (maxCharacters <= 0 || line.Length < maxCharacters)
			{
				return line;
			}
			char[] array = line.ToCharArray();
			bool flag = true;
			bool flag2 = false;
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				if (flag)
				{
					num++;
					if (array[i] == '\n')
					{
						num = 0;
					}
					if (num >= maxCharacters && char.IsWhiteSpace(array[i]))
					{
						array[i] = '\n';
						flag = false;
						flag2 = false;
					}
				}
				else if (!char.IsWhiteSpace(array[i]))
				{
					flag = true;
					num = 0;
				}
				else if (array[i] != '\n')
				{
					array[i] = '\0';
				}
				else
				{
					if (!flag2)
					{
						array[i] = '\0';
					}
					flag2 = true;
				}
				i++;
			}
			return new string((from c in array
			where c > '\0'
			select c).ToArray<char>());
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x00162984 File Offset: 0x00160B84
		public static bool FindNextTag(string line, int iStart, out int tagStart, out int tagEnd)
		{
			tagStart = -1;
			tagEnd = -1;
			int length = line.Length;
			tagStart = iStart;
			while (tagStart < length && line[tagStart] != '[' && line[tagStart] != '(' && line[tagStart] != '{' && line[tagStart] != '<')
			{
				tagStart++;
			}
			if (tagStart == length)
			{
				return false;
			}
			bool flag = false;
			for (tagEnd = tagStart + 1; tagEnd < length; tagEnd++)
			{
				char c = line[tagEnd];
				if (c == ']' || c == ')' || c == '}' || c == '>')
				{
					return !flag || I2Utils.FindNextTag(line, tagEnd + 1, out tagStart, out tagEnd);
				}
				if (c > 'ÿ')
				{
					flag = true;
				}
			}
			return false;
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x00162A34 File Offset: 0x00160C34
		public static string RemoveTags(string text)
		{
			return Regex.Replace(text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>", "");
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x00162A48 File Offset: 0x00160C48
		public static bool RemoveResourcesPath(ref string sPath)
		{
			int num = sPath.IndexOf("\\Resources\\");
			int num2 = sPath.IndexOf("\\Resources/");
			int num3 = sPath.IndexOf("/Resources\\");
			int num4 = sPath.IndexOf("/Resources/");
			int num5 = Mathf.Max(new int[]
			{
				num,
				num2,
				num3,
				num4
			});
			bool result = false;
			if (num5 >= 0)
			{
				sPath = sPath.Substring(num5 + 11);
				result = true;
			}
			else
			{
				num5 = sPath.LastIndexOfAny(LanguageSourceData.CategorySeparators);
				if (num5 > 0)
				{
					sPath = sPath.Substring(num5 + 1);
				}
			}
			string extension = Path.GetExtension(sPath);
			if (!string.IsNullOrEmpty(extension))
			{
				sPath = sPath.Substring(0, sPath.Length - extension.Length);
			}
			return result;
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x00162B0E File Offset: 0x00160D0E
		public static bool IsPlaying()
		{
			return Application.isPlaying;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x00162B1C File Offset: 0x00160D1C
		public static string GetPath(this Transform tr)
		{
			Transform parent = tr.parent;
			if (tr == null)
			{
				return tr.name;
			}
			return parent.GetPath() + "/" + tr.name;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x00162B56 File Offset: 0x00160D56
		public static Transform FindObject(string objectPath)
		{
			return I2Utils.FindObject(SceneManager.GetActiveScene(), objectPath);
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x00162B64 File Offset: 0x00160D64
		public static Transform FindObject(Scene scene, string objectPath)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				Transform transform = rootGameObjects[i].transform;
				if (transform.name == objectPath)
				{
					return transform;
				}
				if (objectPath.StartsWith(transform.name + "/"))
				{
					return I2Utils.FindObject(transform, objectPath.Substring(transform.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00162BD4 File Offset: 0x00160DD4
		public static Transform FindObject(Transform root, string objectPath)
		{
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == objectPath)
				{
					return child;
				}
				if (objectPath.StartsWith(child.name + "/"))
				{
					return I2Utils.FindObject(child, objectPath.Substring(child.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x00162C40 File Offset: 0x00160E40
		public static H FindInParents<H>(Transform tr) where H : Component
		{
			if (!tr)
			{
				return default(!!0);
			}
			H component = tr.GetComponent<H>();
			while (!component && tr)
			{
				component = tr.GetComponent<H>();
				tr = tr.parent;
			}
			return component;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x00162C90 File Offset: 0x00160E90
		public static string GetCaptureMatch(Match match)
		{
			for (int i = match.Groups.Count - 1; i >= 0; i--)
			{
				if (match.Groups[i].Success)
				{
					return match.Groups[i].ToString();
				}
			}
			return match.ToString();
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x00162CE0 File Offset: 0x00160EE0
		public static void SendWebRequest(UnityWebRequest www)
		{
			www.SendWebRequest();
		}

		// Token: 0x04002199 RID: 8601
		public const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

		// Token: 0x0400219A RID: 8602
		public const string NumberChars = "0123456789";

		// Token: 0x0400219B RID: 8603
		public const string ValidNameSymbols = ".-_$#@*()[]{}+:?!&',^=<>~`";
	}
}
