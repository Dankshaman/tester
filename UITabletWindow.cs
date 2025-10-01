using System;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class UITabletWindow : MonoBehaviour
{
	// Token: 0x060027C4 RID: 10180 RVA: 0x00119F1A File Offset: 0x0011811A
	private void Start()
	{
		this.ThisUIInput.SelectAllTextOnClick = true;
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x00119F28 File Offset: 0x00118128
	private void OnEnable()
	{
		if (this.UITextureObject.GetComponent<UISprite>())
		{
			UnityEngine.Object.Destroy(this.UITextureObject.GetComponent<UISprite>());
		}
		this.ThisUITextureView = this.UITextureObject.GetComponent<UITexture>();
		if (!this.ThisUITextureView)
		{
			this.ThisUITextureView = this.UITextureObject.AddComponent<UITexture>();
			this.ThisUITextureView.autoResizeBoxCollider = true;
			this.ThisUITextureView.depth = 5;
			this.ThisUITextureView.pivot = UIWidget.Pivot.TopLeft;
			this.UITextureObject.transform.localPosition = new Vector3(-515f, 312f, 0f);
			this.ThisUITextureView.SetDimensions(1024, 666);
		}
		this.ThisUITextureView.mainTexture = this.CurrentTablet.browser.Texture;
		this.ThisUIInput.value = this.CurrentTablet.CurrentURL;
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x0011A014 File Offset: 0x00118214
	public void OnURLChange(string URL)
	{
		if (string.IsNullOrEmpty(URL))
		{
			return;
		}
		this.ThisUIInput.value = URL;
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x0011A02B File Offset: 0x0011822B
	private void Disable()
	{
		this.ThisUITextureView.mainTexture = null;
		this.CurrentTablet = null;
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x0011A040 File Offset: 0x00118240
	public void GUIBack()
	{
		this.CurrentTablet.GUIBack();
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x0011A04D File Offset: 0x0011824D
	public void GUIForward()
	{
		this.CurrentTablet.GUIForward();
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x0011A05A File Offset: 0x0011825A
	public void GUIHome()
	{
		this.CurrentTablet.GUIHome();
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x0011A067 File Offset: 0x00118267
	public void LoadSearchURL(string url)
	{
		this.CurrentTablet.LoadSearchURL(url);
	}

	// Token: 0x04001A18 RID: 6680
	public TabletScript CurrentTablet;

	// Token: 0x04001A19 RID: 6681
	public GameObject UITextureObject;

	// Token: 0x04001A1A RID: 6682
	public UITexture ThisUITextureView;

	// Token: 0x04001A1B RID: 6683
	public UIInput ThisUIInput;
}
