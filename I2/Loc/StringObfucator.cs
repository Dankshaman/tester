using System;
using System.Text;

namespace I2.Loc
{
	// Token: 0x020004B4 RID: 1204
	public class StringObfucator
	{
		// Token: 0x0600356D RID: 13677 RVA: 0x00164890 File Offset: 0x00162A90
		public static string Encode(string NormalString)
		{
			string result;
			try
			{
				result = StringObfucator.ToBase64(StringObfucator.XoREncode(NormalString));
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x001648C4 File Offset: 0x00162AC4
		public static string Decode(string ObfucatedString)
		{
			string result;
			try
			{
				result = StringObfucator.XoREncode(StringObfucator.FromBase64(ObfucatedString));
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x001648F8 File Offset: 0x00162AF8
		private static string ToBase64(string regularString)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(regularString));
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x0016490C File Offset: 0x00162B0C
		private static string FromBase64(string base64string)
		{
			byte[] array = Convert.FromBase64String(base64string);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x00164930 File Offset: 0x00162B30
		private static string XoREncode(string NormalString)
		{
			string result;
			try
			{
				char[] stringObfuscatorPassword = StringObfucator.StringObfuscatorPassword;
				char[] array = NormalString.ToCharArray();
				int num = stringObfuscatorPassword.Length;
				int i = 0;
				int num2 = array.Length;
				while (i < num2)
				{
					array[i] = (array[i] ^ stringObfuscatorPassword[i % num] ^ (char)((byte)((i % 2 == 0) ? (i * 23) : (-i * 51))));
					i++;
				}
				result = new string(array);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04002204 RID: 8708
		public static char[] StringObfuscatorPassword = "ÝúbUu¸CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd1<QÛÝúbUu¸CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd".ToCharArray();
	}
}
