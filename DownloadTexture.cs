using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000017 RID: 23
[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
	// Token: 0x06000082 RID: 130 RVA: 0x00004700 File Offset: 0x00002900
	private IEnumerator Start()
	{
		WWW www = new WWW(this.url);
		yield return www;
		this.mTex = www.texture;
		if (this.mTex != null)
		{
			UITexture component = base.GetComponent<UITexture>();
			component.mainTexture = this.mTex;
			if (this.pixelPerfect)
			{
				component.MakePixelPerfect();
			}
		}
		www.Dispose();
		yield break;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x0000470F File Offset: 0x0000290F
	private void OnDestroy()
	{
		if (this.mTex != null)
		{
			UnityEngine.Object.Destroy(this.mTex);
		}
	}

	// Token: 0x04000052 RID: 82
	public string url = "http://www.yourwebsite.com/logo.png";

	// Token: 0x04000053 RID: 83
	public bool pixelPerfect = true;

	// Token: 0x04000054 RID: 84
	private Texture2D mTex;
}
