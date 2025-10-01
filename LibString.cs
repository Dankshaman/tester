using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000148 RID: 328
public static class LibString
{
	// Token: 0x060010C9 RID: 4297 RVA: 0x000743A8 File Offset: 0x000725A8
	public static string SpacedCamelCase(string s)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < s.Length; i++)
		{
			if (i > 0 && char.IsUpper(s[i]))
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(s[i]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x00074400 File Offset: 0x00072600
	public static string UnderscoreFromCamelCase(string s)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < s.Length; i++)
		{
			if (char.IsUpper(s[i]))
			{
				if (i > 0)
				{
					stringBuilder.Append("_");
				}
				stringBuilder.Append(s.Substring(i, 1).ToLower());
			}
			else
			{
				stringBuilder.Append(s[i]);
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x0007446C File Offset: 0x0007266C
	public static string CamelCaseFromUnderscore(string s, bool capitalize = false, bool insertSpaces = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = capitalize;
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] == '_')
			{
				flag = true;
			}
			else if (flag)
			{
				if (insertSpaces && i > 0)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(s.Substring(i, 1).ToUpper());
				flag = false;
			}
			else
			{
				stringBuilder.Append(s[i]);
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x000744E4 File Offset: 0x000726E4
	public static bool StringIsTrue(string value)
	{
		value = value.ToLower();
		return value == "+" || value == "true" || value == "yes" || value == "y" || value == "t" || value == "on" || value == "1";
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x00074554 File Offset: 0x00072754
	public static bool StringIsFalse(string value)
	{
		value = value.ToLower();
		return value == "-" || value == "false" || value == "no" || value == "n" || value == "f" || value == "off" || value == "0";
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x000745C4 File Offset: 0x000727C4
	public static bool StringIsToggle(string value)
	{
		value = value.ToLower();
		return value == "!" || value == "toggle" || value == "not" || value == "-1";
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x00074604 File Offset: 0x00072804
	public static string StringFromFloat(float f)
	{
		string text = f.ToString("F39").TrimEnd(new char[]
		{
			'0'
		});
		if (text[text.Length - 1] == '.')
		{
			text += "0";
		}
		return text;
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x00074650 File Offset: 0x00072850
	public static string StringFromVector3(Vector3 v3)
	{
		return string.Concat(new string[]
		{
			v3.x.ToString("F39").TrimEnd(new char[]
			{
				'0'
			}),
			" ",
			v3.y.ToString("F39").TrimEnd(new char[]
			{
				'0'
			}),
			" ",
			v3.z.ToString("F39").TrimEnd(new char[]
			{
				'0'
			}),
			" "
		}).Replace(". ", " ").TrimEnd(new char[]
		{
			' '
		});
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x0007470C File Offset: 0x0007290C
	public static int IntOrDefault(string s, int defaultValue)
	{
		int result;
		if (int.TryParse(s, out result))
		{
			return result;
		}
		return defaultValue;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x00074728 File Offset: 0x00072928
	public static float FloatOrDefault(string s, float defaultValue)
	{
		float result;
		if (float.TryParse(s, out result))
		{
			return result;
		}
		return defaultValue;
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x00074744 File Offset: 0x00072944
	public static bool stripLead(ref string s, char toStrip = ' ')
	{
		int num = 0;
		while (num < s.Length && s[num] == toStrip)
		{
			num++;
		}
		if (num > 0)
		{
			s = s.Substring(num);
			return true;
		}
		return false;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x00074780 File Offset: 0x00072980
	public static int paramCount(string s, int max = 0)
	{
		int num = 0;
		LibString.bite(ref s, false, ' ', false, false, '\0');
		while (s != "" && (max == 0 || num < max))
		{
			num++;
			LibString.bite(ref s, false, ' ', false, false, '\0');
		}
		return num;
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x000747C8 File Offset: 0x000729C8
	public static string bite(ref string remainder, bool convertToLowerCase = false, char separator = ' ', bool stripTrailingWhitespace = false, bool stripLeadingWhitespace = false, char unquotableSeperator = '\0')
	{
		int num = 0;
		while (num < remainder.Length && (remainder[num] == separator || remainder[num] == unquotableSeperator))
		{
			num++;
		}
		if (num > 0)
		{
			remainder = remainder.Substring(num);
		}
		if (stripLeadingWhitespace)
		{
			num = 0;
			while (num < remainder.Length && remainder[num] == ' ')
			{
				num++;
			}
			if (num > 0)
			{
				remainder = remainder.Substring(num);
			}
		}
		if (remainder == "")
		{
			return null;
		}
		num = 0;
		char c = separator;
		int num2 = 0;
		bool flag = false;
		if (remainder[0] == '"' || remainder[0] == '\'')
		{
			c = remainder[0];
			remainder = remainder.Substring(1);
			num2 = 1;
			flag = true;
		}
		bool flag2 = false;
		char c2 = '\0';
		while (num < remainder.Length && remainder[num] != unquotableSeperator)
		{
			if (!flag2)
			{
				if (remainder[num] == c)
				{
					break;
				}
				if (remainder[num] == '"' || remainder[num] == '\'')
				{
					c2 = remainder[num];
					flag2 = true;
				}
			}
			else if (remainder[num] == c2)
			{
				flag2 = false;
			}
			num++;
		}
		if (num == remainder.Length && c != separator && c != unquotableSeperator)
		{
			num2 = 0;
		}
		string text = remainder.Substring(0, num);
		remainder = remainder.Substring(num + num2);
		num = 0;
		while (num < remainder.Length && (remainder[num] == separator || remainder[num] == unquotableSeperator))
		{
			num++;
		}
		if (num > 0)
		{
			remainder = remainder.Substring(num);
		}
		if (stripTrailingWhitespace)
		{
			num = text.Length;
			while (num > 0 && text[num - 1] == ' ')
			{
				num--;
			}
			if (num < text.Length)
			{
				text = text.Substring(0, num);
			}
		}
		if (text == "" && !flag)
		{
			text = null;
		}
		else if (convertToLowerCase)
		{
			text = text.ToLower();
		}
		return text;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x000749A1 File Offset: 0x00072BA1
	public static string bite(ref string remainder, char separator)
	{
		return LibString.bite(ref remainder, false, separator, false, false, '\0');
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x000749B0 File Offset: 0x00072BB0
	public static string lookAhead(string remainder, bool convertToLowerCase = false, char separator = ' ', int count = 0, bool stripTrailingWhitespace = false, bool stripLeadingWhitespace = false)
	{
		string result = LibString.bite(ref remainder, convertToLowerCase, separator, stripTrailingWhitespace, stripLeadingWhitespace, '\0');
		while (count > 0)
		{
			result = LibString.bite(ref remainder, convertToLowerCase, separator, stripTrailingWhitespace, stripLeadingWhitespace, '\0');
			count--;
		}
		return result;
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x000749E7 File Offset: 0x00072BE7
	public static string lookAhead(string remainder, char seperator)
	{
		return LibString.lookAhead(remainder, false, seperator, 0, false, false);
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x000749F4 File Offset: 0x00072BF4
	public static string StringFromFile(string path, int startLine, int endLine)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StreamReader streamReader = File.OpenText(path);
		int num = 0;
		while (num <= endLine && !streamReader.EndOfStream)
		{
			string value = streamReader.ReadLine();
			if (num >= startLine)
			{
				stringBuilder.Append(value);
			}
			num++;
		}
		streamReader.Close();
		return stringBuilder.ToString();
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00074A44 File Offset: 0x00072C44
	public static List<string> LinesFromFile(string path, int startLine, int endLine)
	{
		List<string> list = new List<string>();
		StreamReader streamReader = File.OpenText(path);
		int num = 0;
		while (num <= endLine && !streamReader.EndOfStream)
		{
			string item = streamReader.ReadLine();
			if (num >= startLine)
			{
				list.Add(item);
			}
			num++;
		}
		streamReader.Close();
		return list;
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x00074A8C File Offset: 0x00072C8C
	public static bool TryGetStartEndLineNumbers(string path, string tag, out int startLine, out int endLine)
	{
		startLine = (endLine = -1);
		StreamReader streamReader = File.OpenText(path);
		int num = 0;
		while (!streamReader.EndOfStream)
		{
			if (streamReader.ReadLine().Contains(tag))
			{
				if (startLine != -1)
				{
					endLine = num;
					break;
				}
				startLine = num;
			}
			num++;
		}
		streamReader.Close();
		return startLine >= 0 && endLine >= 0;
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x00074AEC File Offset: 0x00072CEC
	public static byte[] CompressBytes(byte[] buffer)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gzipStream.Write(buffer, 0, buffer.Length);
			}
			memoryStream.Position = 0L;
			byte[] array = new byte[memoryStream.Length];
			memoryStream.Read(array, 0, array.Length);
			byte[] array2 = new byte[array.Length + 4];
			Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, array2, 0, 4);
			Buffer.BlockCopy(array, 0, array2, 4, array.Length);
			result = array2;
		}
		return result;
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x00074B98 File Offset: 0x00072D98
	public static byte[] DecompressBytes(byte[] gZipBuffer)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			int num = BitConverter.ToInt32(gZipBuffer, 0);
			memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
			byte[] array = new byte[num];
			memoryStream.Position = 0L;
			using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
			{
				gzipStream.Read(array, 0, array.Length);
			}
			result = array;
		}
		return result;
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00074C18 File Offset: 0x00072E18
	public static string CompressString(string text)
	{
		return Convert.ToBase64String(LibString.CompressBytes(Encoding.UTF8.GetBytes(text)));
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x00074C30 File Offset: 0x00072E30
	public static string DecompressString(string compressedText)
	{
		byte[] gZipBuffer = Convert.FromBase64String(compressedText);
		return Encoding.UTF8.GetString(LibString.DecompressBytes(gZipBuffer));
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x00074C54 File Offset: 0x00072E54
	public static void CreateForeignLookup()
	{
		LibString.asciiFromForeign = new Dictionary<char, string>();
		foreach (KeyValuePair<string, string> keyValuePair in LibString.foreignCharacterLookup)
		{
			for (int i = 0; i < keyValuePair.Key.Length; i++)
			{
				LibString.asciiFromForeign[keyValuePair.Key[i]] = keyValuePair.Value;
			}
		}
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x00074CE0 File Offset: 0x00072EE0
	public static string NormalizedString(string text, bool ignoreCase = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < text.Length; i++)
		{
			string value;
			if (LibString.asciiFromForeign.TryGetValue(text[i], out value))
			{
				stringBuilder.Append(value);
			}
			else
			{
				stringBuilder.Append(text[i]);
			}
		}
		if (ignoreCase)
		{
			return stringBuilder.ToString().ToLower();
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x00074D46 File Offset: 0x00072F46
	public static string NormalizedTag(string text)
	{
		return text.ToLower().Replace(" ", "_");
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x00074D60 File Offset: 0x00072F60
	public static string StripBBCode(string text)
	{
		if (LibString.stripBB == null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\\[-\\]");
			stringBuilder.Append("|\\[/?[bius]\\]");
			stringBuilder.Append("|\\[/?su[bp]\\]");
			stringBuilder.Append("|\\[[0-9a-f]{6}\\]");
			LibString.stripBB = new Regex(stringBuilder.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled, new TimeSpan(0, 0, 1));
		}
		return LibString.stripBB.Replace(text, "");
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x00074DD2 File Offset: 0x00072FD2
	public static Result OK()
	{
		return new Result("");
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x00074DDE File Offset: 0x00072FDE
	public static Result Error(string message)
	{
		return new Result(message);
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00074DE8 File Offset: 0x00072FE8
	private static bool leadingInt(string s, out int i, out int remainderStart)
	{
		i = 0;
		remainderStart = 0;
		if (s == "")
		{
			return false;
		}
		int num = (s[0] == '-') ? 1 : 0;
		remainderStart = num;
		while (remainderStart < s.Length && s[remainderStart] >= '0' && s[remainderStart] <= '9')
		{
			remainderStart++;
		}
		if (remainderStart - num == 0)
		{
			return false;
		}
		i = int.Parse(s.Substring(0, remainderStart));
		return true;
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x00074E60 File Offset: 0x00073060
	public static int FuzzyCompare(string a, string b)
	{
		int num;
		int startIndex;
		int value;
		int startIndex2;
		if (!LibString.leadingInt(a, out num, out startIndex) || !LibString.leadingInt(b, out value, out startIndex2))
		{
			return a.CompareTo(b);
		}
		int num2 = num.CompareTo(value);
		if (num2 == 0)
		{
			return a.Substring(startIndex).CompareTo(b.Substring(startIndex2));
		}
		return num2;
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x00074EB4 File Offset: 0x000730B4
	public static string GetRandomGUID()
	{
		return Guid.NewGuid().ToString().Substring(0, 6);
	}

	// Token: 0x04000AC0 RID: 2752
	private static Dictionary<string, string> foreignCharacterLookup = new Dictionary<string, string>
	{
		{
			"äæǽ",
			"ae"
		},
		{
			"öœ",
			"oe"
		},
		{
			"ü",
			"ue"
		},
		{
			"Ä",
			"Ae"
		},
		{
			"Ü",
			"Ue"
		},
		{
			"Ö",
			"Oe"
		},
		{
			"ÀÁÂÃÄÅǺĀĂĄǍΑΆẢẠẦẪẨẬẰẮẴẲẶА",
			"A"
		},
		{
			"àáâãåǻāăąǎªαάảạầấẫẩậằắẵẳặа",
			"a"
		},
		{
			"Б",
			"B"
		},
		{
			"б",
			"b"
		},
		{
			"ÇĆĈĊČ",
			"C"
		},
		{
			"çćĉċč",
			"c"
		},
		{
			"Д",
			"D"
		},
		{
			"д",
			"d"
		},
		{
			"ÐĎĐΔ",
			"Dj"
		},
		{
			"ðďđδ",
			"dj"
		},
		{
			"ÈÉÊËĒĔĖĘĚΕΈẼẺẸỀẾỄỂỆЕЭ",
			"E"
		},
		{
			"èéêëēĕėęěέεẽẻẹềếễểệеэ",
			"e"
		},
		{
			"Ф",
			"F"
		},
		{
			"ф",
			"f"
		},
		{
			"ĜĞĠĢΓГҐ",
			"G"
		},
		{
			"ĝğġģγгґ",
			"g"
		},
		{
			"ĤĦ",
			"H"
		},
		{
			"ĥħ",
			"h"
		},
		{
			"ÌÍÎÏĨĪĬǏĮİΗΉΊΙΪỈỊИЫ",
			"I"
		},
		{
			"ìíîïĩīĭǐįıηήίιϊỉịиыї",
			"i"
		},
		{
			"Ĵ",
			"J"
		},
		{
			"ĵ",
			"j"
		},
		{
			"ĶΚК",
			"K"
		},
		{
			"ķκк",
			"k"
		},
		{
			"ĹĻĽĿŁΛЛ",
			"L"
		},
		{
			"ĺļľŀłλл",
			"l"
		},
		{
			"М",
			"M"
		},
		{
			"м",
			"m"
		},
		{
			"ÑŃŅŇΝН",
			"N"
		},
		{
			"ñńņňŉνн",
			"n"
		},
		{
			"ÒÓÔÕŌŎǑŐƠØǾΟΌΩΏỎỌỒỐỖỔỘỜỚỠỞỢО",
			"O"
		},
		{
			"òóôõōŏǒőơøǿºοόωώỏọồốỗổộờớỡởợо",
			"o"
		},
		{
			"П",
			"P"
		},
		{
			"п",
			"p"
		},
		{
			"ŔŖŘΡР",
			"R"
		},
		{
			"ŕŗřρр",
			"r"
		},
		{
			"ŚŜŞȘŠΣС",
			"S"
		},
		{
			"śŝşșšſσςс",
			"s"
		},
		{
			"ȚŢŤŦτТ",
			"T"
		},
		{
			"țţťŧт",
			"t"
		},
		{
			"ÙÚÛŨŪŬŮŰŲƯǓǕǗǙǛŨỦỤỪỨỮỬỰУ",
			"U"
		},
		{
			"ùúûũūŭůűųưǔǖǘǚǜυύϋủụừứữửựу",
			"u"
		},
		{
			"ÝŸŶΥΎΫỲỸỶỴЙ",
			"Y"
		},
		{
			"ýÿŷỳỹỷỵй",
			"y"
		},
		{
			"В",
			"V"
		},
		{
			"в",
			"v"
		},
		{
			"Ŵ",
			"W"
		},
		{
			"ŵ",
			"w"
		},
		{
			"ŹŻŽΖЗ",
			"Z"
		},
		{
			"źżžζз",
			"z"
		},
		{
			"ÆǼ",
			"AE"
		},
		{
			"ß",
			"ss"
		},
		{
			"Ĳ",
			"IJ"
		},
		{
			"ĳ",
			"ij"
		},
		{
			"Œ",
			"OE"
		},
		{
			"ƒ",
			"f"
		},
		{
			"ξ",
			"ks"
		},
		{
			"π",
			"p"
		},
		{
			"β",
			"v"
		},
		{
			"μ",
			"m"
		},
		{
			"ψ",
			"ps"
		},
		{
			"Ё",
			"Yo"
		},
		{
			"ё",
			"yo"
		},
		{
			"Є",
			"Ye"
		},
		{
			"є",
			"ye"
		},
		{
			"Ї",
			"Yi"
		},
		{
			"Ж",
			"Zh"
		},
		{
			"ж",
			"zh"
		},
		{
			"Х",
			"Kh"
		},
		{
			"х",
			"kh"
		},
		{
			"Ц",
			"Ts"
		},
		{
			"ц",
			"ts"
		},
		{
			"Ч",
			"Ch"
		},
		{
			"ч",
			"ch"
		},
		{
			"Ш",
			"Sh"
		},
		{
			"ш",
			"sh"
		},
		{
			"Щ",
			"Shch"
		},
		{
			"щ",
			"shch"
		},
		{
			"ЪъЬь",
			""
		},
		{
			"Ю",
			"Yu"
		},
		{
			"ю",
			"yu"
		},
		{
			"Я",
			"Ya"
		},
		{
			"я",
			"ya"
		}
	};

	// Token: 0x04000AC1 RID: 2753
	private static Dictionary<char, string> asciiFromForeign;

	// Token: 0x04000AC2 RID: 2754
	private static Regex stripBB;
}
