using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class ScrollingUVs : MonoBehaviour
{
	// Token: 0x06000039 RID: 57 RVA: 0x00003054 File Offset: 0x00001254
	private void LateUpdate()
	{
		this.uvOffset += this.uvAnimationRate * Time.deltaTime;
		if (base.GetComponent<Renderer>().enabled)
		{
			base.GetComponent<Renderer>().materials[this.materialIndex].SetTextureOffset(this.textureName0, this.uvOffset);
			base.GetComponent<Renderer>().materials[this.materialIndex].SetTextureOffset(this.textureName1, this.uvOffset);
		}
	}

	// Token: 0x0400000C RID: 12
	public int materialIndex;

	// Token: 0x0400000D RID: 13
	public Vector2 uvAnimationRate = new Vector2(1f, 0f);

	// Token: 0x0400000E RID: 14
	public string textureName0 = "_MainTex";

	// Token: 0x0400000F RID: 15
	public string textureName1 = "_BumpMap";

	// Token: 0x04000010 RID: 16
	private Vector2 uvOffset = Vector2.zero;
}
