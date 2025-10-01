using System;

namespace I2.Loc
{
	// Token: 0x0200047D RID: 1149
	[Serializable]
	public class LanguageData
	{
		// Token: 0x06003391 RID: 13201 RVA: 0x0015B5A0 File Offset: 0x001597A0
		public bool IsEnabled()
		{
			return (this.Flags & 1) == 0;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0015B5AD File Offset: 0x001597AD
		public void SetEnabled(bool bEnabled)
		{
			if (bEnabled)
			{
				this.Flags = (byte)((int)this.Flags & -2);
				return;
			}
			this.Flags |= 1;
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x0015B5D2 File Offset: 0x001597D2
		public bool IsLoaded()
		{
			return (this.Flags & 4) == 0;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x0015B5DF File Offset: 0x001597DF
		public bool CanBeUnloaded()
		{
			return (this.Flags & 2) == 0;
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x0015B5EC File Offset: 0x001597EC
		public void SetLoaded(bool loaded)
		{
			if (loaded)
			{
				this.Flags = (byte)((int)this.Flags & -5);
				return;
			}
			this.Flags |= 4;
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x0015B611 File Offset: 0x00159811
		public void SetCanBeUnLoaded(bool allowUnloading)
		{
			if (allowUnloading)
			{
				this.Flags = (byte)((int)this.Flags & -3);
				return;
			}
			this.Flags |= 2;
		}

		// Token: 0x040020FE RID: 8446
		public string Name;

		// Token: 0x040020FF RID: 8447
		public string Code;

		// Token: 0x04002100 RID: 8448
		public byte Flags;

		// Token: 0x04002101 RID: 8449
		[NonSerialized]
		public bool Compressed;
	}
}
