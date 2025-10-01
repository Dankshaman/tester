using System;
using System.Globalization;
using System.Text;

namespace LuaFramework
{
	// Token: 0x020003B4 RID: 948
	public static class LuaWriterExtensionMethods
	{
		// Token: 0x06002CDD RID: 11485 RVA: 0x00139FC0 File Offset: 0x001381C0
		public static string ToLiteral(this string input)
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length + 2);
			stringBuilder.Append("\"");
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				if (c <= '"')
				{
					switch (c)
					{
					case '\0':
						stringBuilder.Append("\\0");
						break;
					case '\u0001':
					case '\u0002':
					case '\u0003':
					case '\u0004':
					case '\u0005':
					case '\u0006':
						goto IL_12D;
					case '\a':
						stringBuilder.Append("\\a");
						break;
					case '\b':
						stringBuilder.Append("\\b");
						break;
					case '\t':
						stringBuilder.Append("\\t");
						break;
					case '\n':
						stringBuilder.Append("\\n");
						break;
					case '\v':
						stringBuilder.Append("\\v");
						break;
					case '\f':
						stringBuilder.Append("\\f");
						break;
					case '\r':
						stringBuilder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							goto IL_12D;
						}
						stringBuilder.Append("\\\"");
						break;
					}
				}
				else if (c != '\'')
				{
					if (c != '\\')
					{
						goto IL_12D;
					}
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append("\\'");
				}
				IL_157:
				i++;
				continue;
				IL_12D:
				if (char.GetUnicodeCategory(c) != UnicodeCategory.Control)
				{
					stringBuilder.Append(c);
					goto IL_157;
				}
				StringBuilder stringBuilder2 = stringBuilder;
				ushort num = (ushort)c;
				stringBuilder2.Append(num.ToString("x4"));
				goto IL_157;
			}
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}
	}
}
