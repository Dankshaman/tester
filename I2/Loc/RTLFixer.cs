using System;

namespace I2.Loc
{
	// Token: 0x020004A8 RID: 1192
	public class RTLFixer
	{
		// Token: 0x06003547 RID: 13639 RVA: 0x00162F05 File Offset: 0x00161105
		public static string Fix(string str)
		{
			return RTLFixer.Fix(str, false, true);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x00162F10 File Offset: 0x00161110
		public static string Fix(string str, bool rtl)
		{
			if (rtl)
			{
				return RTLFixer.Fix(str);
			}
			string[] array = str.Split(new char[]
			{
				' '
			});
			string text = "";
			string text2 = "";
			foreach (string text3 in array)
			{
				if (char.IsLower(text3.ToLower()[text3.Length / 2]))
				{
					text = text + RTLFixer.Fix(text2) + text3 + " ";
					text2 = "";
				}
				else
				{
					text2 = text2 + text3 + " ";
				}
			}
			if (text2 != "")
			{
				text += RTLFixer.Fix(text2);
			}
			return text;
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x00162FBC File Offset: 0x001611BC
		public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
		{
			string text = HindiFixer.Fix(str);
			if (text != str)
			{
				return text;
			}
			RTLFixerTool.showTashkeel = showTashkeel;
			RTLFixerTool.useHinduNumbers = useHinduNumbers;
			if (str.Contains("\n"))
			{
				str = str.Replace("\n", Environment.NewLine);
			}
			if (!str.Contains(Environment.NewLine))
			{
				return RTLFixerTool.FixLine(str);
			}
			string[] separator = new string[]
			{
				Environment.NewLine
			};
			string[] array = str.Split(separator, StringSplitOptions.None);
			if (array.Length == 0)
			{
				return RTLFixerTool.FixLine(str);
			}
			if (array.Length == 1)
			{
				return RTLFixerTool.FixLine(str);
			}
			string text2 = RTLFixerTool.FixLine(array[0]);
			int i = 1;
			if (array.Length > 1)
			{
				while (i < array.Length)
				{
					text2 = text2 + Environment.NewLine + RTLFixerTool.FixLine(array[i]);
					i++;
				}
			}
			return text2;
		}
	}
}
