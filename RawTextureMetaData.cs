using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class RawTextureMetaData
{
	// Token: 0x06000A2E RID: 2606 RVA: 0x0004813E File Offset: 0x0004633E
	public RawTextureMetaData()
	{
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00048158 File Offset: 0x00046358
	public RawTextureMetaData(TextureFormat format, int width_original, int height_original, int width_new, int height_new, bool mip_maps, bool normal_map, int max_size)
	{
		this.format = format;
		this.width_original = width_original;
		this.height_original = height_original;
		this.width_new = width_new;
		this.height_new = height_new;
		this.mip_maps = mip_maps;
		this.normal_map = normal_map;
		this.max_size = max_size;
	}

	// Token: 0x04000736 RID: 1846
	public TextureFormat format;

	// Token: 0x04000737 RID: 1847
	public int width_original;

	// Token: 0x04000738 RID: 1848
	public int height_original;

	// Token: 0x04000739 RID: 1849
	public int width_new;

	// Token: 0x0400073A RID: 1850
	public int height_new;

	// Token: 0x0400073B RID: 1851
	public bool mip_maps = true;

	// Token: 0x0400073C RID: 1852
	public bool normal_map;

	// Token: 0x0400073D RID: 1853
	public int max_size = 8192;
}
