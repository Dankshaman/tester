using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class ScrollingUVs01 : MonoBehaviour
{
	// Token: 0x0600003B RID: 59 RVA: 0x00003114 File Offset: 0x00001314
	private void LateUpdate()
	{
		this.uvOffset += this.uvAnimationRate * Time.deltaTime;
		if (base.GetComponent<Renderer>().enabled)
		{
			base.GetComponent<Renderer>().materials[this.materialIndex].SetTextureOffset(this.textureName0, this.uvOffset);
			base.GetComponent<Renderer>().materials[this.materialIndex].SetTextureOffset(this.textureName1, this.uvOffset);
		}
	}

	// Token: 0x04000011 RID: 17
	public int materialIndex;

	// Token: 0x04000012 RID: 18
	public Vector2 uvAnimationRate = new Vector2(1f, 0f);

	// Token: 0x04000013 RID: 19
	public string textureName0 = "_MainTex";

	// Token: 0x04000014 RID: 20
	public string textureName1 = "_BumpMap";

	// Token: 0x04000015 RID: 21
	private Vector2 uvOffset = Vector2.zero;
}
