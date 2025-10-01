using System;
using NewNet;

// Token: 0x020002BA RID: 698
public class UICustomPDF : UICustomObject<UICustomPDF>
{
	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x060022A9 RID: 8873 RVA: 0x000F7D9E File Offset: 0x000F5F9E
	// (set) Token: 0x060022AA RID: 8874 RVA: 0x000F7DAB File Offset: 0x000F5FAB
	private string PDFURL
	{
		get
		{
			return this.URLInput.value;
		}
		set
		{
			this.URLInput.value = value;
		}
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000F7DBC File Offset: 0x000F5FBC
	protected override void OnEnable()
	{
		base.OnEnable();
		this._targetObject = this.TargetCustomObject.GetComponent<CustomPDF>();
		if (!this._targetObject)
		{
			return;
		}
		this.URLInput.SelectAllTextOnClick = true;
		this.PageOffsetInput.SelectAllTextOnClick = true;
		this.PDFURL = this._targetObject.CustomPDFURL;
		this.PageOffsetInput.value = this._targetObject.PageDisplayOffset.ToString();
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x000F7E38 File Offset: 0x000F6038
	public override void Import()
	{
		this.PDFURL = this.PDFURL.Trim();
		if (string.IsNullOrEmpty(this.PDFURL))
		{
			Chat.LogError("You must supply a custom PDF URL.", true);
			return;
		}
		if (Network.isServer)
		{
			base.CheckUpdateMatchingCustomObjects();
			this._targetObject.CustomPDFURL = this.PDFURL;
			this._targetObject.CurrentPDFPage = 0;
			int pageDisplayOffset = 0;
			int.TryParse(this.PageOffsetInput.value, out pageDisplayOffset);
			this._targetObject.PageDisplayOffset = pageDisplayOffset;
			this._targetObject.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x040015E9 RID: 5609
	public UIInput URLInput;

	// Token: 0x040015EA RID: 5610
	public UIInput PageOffsetInput;

	// Token: 0x040015EB RID: 5611
	private CustomPDF _targetObject;
}
