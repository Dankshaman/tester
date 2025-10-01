using System;
using System.Text;

namespace StringLeakTest
{
	// Token: 0x0200039A RID: 922
	public static class StringBuilderExtensions
	{
		// Token: 0x06002B3E RID: 11070 RVA: 0x0013218C File Offset: 0x0013038C
		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount, char pad_char, uint base_val)
		{
			uint num = 0U;
			uint num2 = uint_val;
			do
			{
				num2 /= base_val;
				num += 1U;
			}
			while (num2 > 0U);
			string_builder.Append(pad_char, (int)Math.Max(pad_amount, num));
			int num3 = string_builder.Length;
			while (num > 0U)
			{
				num3--;
				string_builder[num3] = StringBuilderExtensions.ms_digits[(int)(uint_val % base_val)];
				uint_val /= base_val;
				num -= 1U;
			}
			return string_builder;
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x001321E6 File Offset: 0x001303E6
		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val)
		{
			string_builder.Concat(uint_val, 0U, StringBuilderExtensions.ms_default_pad_char, 10U);
			return string_builder;
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x001321F9 File Offset: 0x001303F9
		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount)
		{
			string_builder.Concat(uint_val, pad_amount, StringBuilderExtensions.ms_default_pad_char, 10U);
			return string_builder;
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x0013220C File Offset: 0x0013040C
		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount, char pad_char)
		{
			string_builder.Concat(uint_val, pad_amount, pad_char, 10U);
			return string_builder;
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x0013221C File Offset: 0x0013041C
		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount, char pad_char, uint base_val)
		{
			if (int_val < 0)
			{
				string_builder.Append('-');
				uint uint_val = (uint)(-1 - int_val + 1);
				string_builder.Concat(uint_val, pad_amount, pad_char, base_val);
			}
			else
			{
				string_builder.Concat((uint)int_val, pad_amount, pad_char, base_val);
			}
			return string_builder;
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x00132257 File Offset: 0x00130457
		public static StringBuilder Concat(this StringBuilder string_builder, int int_val)
		{
			string_builder.Concat(int_val, 0U, StringBuilderExtensions.ms_default_pad_char, 10U);
			return string_builder;
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x0013226A File Offset: 0x0013046A
		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount)
		{
			string_builder.Concat(int_val, pad_amount, StringBuilderExtensions.ms_default_pad_char, 10U);
			return string_builder;
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x0013227D File Offset: 0x0013047D
		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount, char pad_char)
		{
			string_builder.Concat(int_val, pad_amount, pad_char, 10U);
			return string_builder;
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x0013228C File Offset: 0x0013048C
		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places, uint pad_amount, char pad_char)
		{
			if (decimal_places == 0U)
			{
				int int_val;
				if (float_val >= 0f)
				{
					int_val = (int)(float_val + 0.5f);
				}
				else
				{
					int_val = (int)(float_val - 0.5f);
				}
				string_builder.Concat(int_val, pad_amount, pad_char, 10U);
			}
			else
			{
				int num = (int)float_val;
				string_builder.Concat(num, pad_amount, pad_char, 10U);
				string_builder.Append('.');
				float num2 = Math.Abs(float_val - (float)num);
				do
				{
					num2 *= 10f;
					decimal_places -= 1U;
				}
				while (decimal_places > 0U);
				num2 += 0.5f;
				string_builder.Concat((uint)num2, 0U, '0', 10U);
			}
			return string_builder;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x00132312 File Offset: 0x00130512
		public static StringBuilder Concat(this StringBuilder string_builder, float float_val)
		{
			string_builder.Concat(float_val, StringBuilderExtensions.ms_default_decimal_places, 0U, StringBuilderExtensions.ms_default_pad_char);
			return string_builder;
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x00132328 File Offset: 0x00130528
		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places)
		{
			string_builder.Concat(float_val, decimal_places, 0U, StringBuilderExtensions.ms_default_pad_char);
			return string_builder;
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x0013233A File Offset: 0x0013053A
		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places, uint pad_amount)
		{
			string_builder.Concat(float_val, decimal_places, pad_amount, StringBuilderExtensions.ms_default_pad_char);
			return string_builder;
		}

		// Token: 0x04001D5E RID: 7518
		private static readonly char[] ms_digits = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		// Token: 0x04001D5F RID: 7519
		private static readonly uint ms_default_decimal_places = 5U;

		// Token: 0x04001D60 RID: 7520
		private static readonly char ms_default_pad_char = '0';
	}
}
