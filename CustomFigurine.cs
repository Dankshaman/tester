using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class CustomFigurine : MonoBehaviour
{
	// Token: 0x06000A5F RID: 2655 RVA: 0x000493B8 File Offset: 0x000475B8
	private void Update()
	{
		if (this.enhancedPrecisionBase != SystemConsole.EnhancedFigurinePrecision)
		{
			this.enhancedPrecisionBase = SystemConsole.EnhancedFigurinePrecision;
			GameObject gameObject = base.transform.Find("Cube").gameObject;
			if (gameObject == null)
			{
				return;
			}
			MeshCollider meshCollider = base.GetComponent<MeshCollider>();
			if (this.enhancedPrecisionBase)
			{
				if (meshCollider == null)
				{
					meshCollider = base.gameObject.AddComponent<MeshCollider>();
					meshCollider.convex = true;
					meshCollider.sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
				}
				meshCollider.enabled = true;
				gameObject.SetActive(false);
				return;
			}
			if (meshCollider)
			{
				meshCollider.enabled = false;
			}
			gameObject.SetActive(true);
		}
	}

	// Token: 0x04000759 RID: 1881
	private bool enhancedPrecisionBase;
}
