using System;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class LowerPolyScript : MonoBehaviour
{
	// Token: 0x0600112B RID: 4395 RVA: 0x00076395 File Offset: 0x00074595
	private void Start()
	{
		if (!UIConfigGraphics.CurrentGraphics.FullTextures)
		{
			base.GetComponent<Renderer>().GetComponent<MeshFilter>().mesh = this.LowPolyMesh;
		}
	}

	// Token: 0x04000B06 RID: 2822
	public Mesh LowPolyMesh;
}
