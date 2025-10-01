using System;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010D RID: 269
public class FileBrowserScript : Singleton<FileBrowserScript>
{
	// Token: 0x06000DE3 RID: 3555 RVA: 0x000597C8 File Offset: 0x000579C8
	protected override void Awake()
	{
		base.Awake();
		GameObject gameObject = FileBrowser.Instance.gameObject;
		gameObject.GetComponent<Canvas>().GetCopyOf(this.uGUICanvas.GetComponent<Canvas>());
		gameObject.GetComponent<CanvasScaler>().GetCopyOf(this.uGUICanvas.GetComponent<CanvasScaler>());
		gameObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920f, 900f);
		gameObject.GetComponent<GraphicRaycaster>().GetCopyOf(this.uGUICanvas.GetComponent<GraphicRaycaster>());
	}

	// Token: 0x04000905 RID: 2309
	public GameObject uGUICanvas;
}
