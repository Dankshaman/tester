using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class Mipmapbias : MonoBehaviour
{
	// Token: 0x06001602 RID: 5634 RVA: 0x00099362 File Offset: 0x00097562
	private void Start()
	{
		base.GetComponent<Renderer>().material.mainTexture.mipMapBias = MipmapBiasTextures.CurrentMipMapBias;
	}
}
