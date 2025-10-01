using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class MipmapBiasTextures : MonoBehaviour
{
	// Token: 0x060015FF RID: 5631 RVA: 0x00099324 File Offset: 0x00097524
	private void Start()
	{
		for (int i = 0; i < this.Textures.Length; i++)
		{
			this.Textures[i].mipMapBias = MipmapBiasTextures.CurrentMipMapBias;
		}
	}

	// Token: 0x04000C6B RID: 3179
	public Texture[] Textures;

	// Token: 0x04000C6C RID: 3180
	public static float CurrentMipMapBias = -0.5f;
}
