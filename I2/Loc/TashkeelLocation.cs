using System;

namespace I2.Loc
{
	// Token: 0x020004AD RID: 1197
	internal class TashkeelLocation
	{
		// Token: 0x0600354F RID: 13647 RVA: 0x00163530 File Offset: 0x00161730
		public TashkeelLocation(char tashkeel, int position)
		{
			this.tashkeel = tashkeel;
			this.position = position;
		}

		// Token: 0x040021FB RID: 8699
		public char tashkeel;

		// Token: 0x040021FC RID: 8700
		public int position;
	}
}
