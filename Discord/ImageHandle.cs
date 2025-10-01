using System;

namespace Discord
{
	// Token: 0x020004D2 RID: 1234
	public struct ImageHandle
	{
		// Token: 0x06003602 RID: 13826 RVA: 0x00166355 File Offset: 0x00164555
		public static ImageHandle User(long id)
		{
			return ImageHandle.User(id, 128U);
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x00166364 File Offset: 0x00164564
		public static ImageHandle User(long id, uint size)
		{
			return new ImageHandle
			{
				Type = ImageType.User,
				Id = id,
				Size = size
			};
		}

		// Token: 0x0400229C RID: 8860
		public ImageType Type;

		// Token: 0x0400229D RID: 8861
		public long Id;

		// Token: 0x0400229E RID: 8862
		public uint Size;
	}
}
