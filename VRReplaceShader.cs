using System;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class VRReplaceShader : MonoBehaviour
{
	// Token: 0x06002959 RID: 10585 RVA: 0x0012298C File Offset: 0x00120B8C
	private void Update()
	{
		this.child = base.gameObject.transform.Find("body");
		if (this.child != null && this.child.GetComponent<MeshRenderer>())
		{
			Material sharedMaterial = this.child.GetComponent<MeshRenderer>().sharedMaterial;
			sharedMaterial.SetInt("_SrcBlend", 1);
			sharedMaterial.SetInt("_DstBlend", 0);
			sharedMaterial.SetInt("_ZWrite", 1);
			sharedMaterial.EnableKeyword("_ALPHATEST_ON");
			sharedMaterial.DisableKeyword("_ALPHABLEND_ON");
			sharedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			sharedMaterial.renderQueue = 2450;
			base.enabled = false;
		}
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x00122A3C File Offset: 0x00120C3C
	public void SetAlpha(float a)
	{
		Renderer[] componentsInChildren = base.transform.parent.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material sharedMaterial = componentsInChildren[i].sharedMaterial;
			Color color = sharedMaterial.color;
			color.a = a;
			sharedMaterial.SetColor("_Color", color);
			sharedMaterial.SetFloat("_Mode", 2f);
			sharedMaterial.SetInt("_SrcBlend", 5);
			sharedMaterial.SetInt("_DstBlend", 10);
			sharedMaterial.SetInt("_ZWrite", 0);
			sharedMaterial.DisableKeyword("_ALPHATEST_ON");
			sharedMaterial.EnableKeyword("_ALPHABLEND_ON");
			sharedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			sharedMaterial.renderQueue = 3000;
		}
	}

	// Token: 0x04001B6B RID: 7019
	private Transform child;
}
