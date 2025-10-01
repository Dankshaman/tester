using System;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class UISetPageNumber : MonoBehaviour
{
	// Token: 0x06002733 RID: 10035 RVA: 0x00116CCC File Offset: 0x00114ECC
	private void OnEnable()
	{
		CustomPDF component = PlayerScript.PointerScript.InfoObject.GetComponent<CustomPDF>();
		this.UpdateSelection(component.CurrentPDFPage + component.PageDisplayOffset);
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x00116CFC File Offset: 0x00114EFC
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject && ((gameObject != null) ? gameObject.GetComponent<CustomPDF>() : null))
			{
				int pageDisplayOffset = gameObject.GetComponent<CustomPDF>().PageDisplayOffset;
				gameObject.GetComponent<CustomPDF>().CurrentPDFPage = this._page - 1 - pageDisplayOffset;
			}
		}
		Transform parent = base.transform.parent;
		for (int i = 0; i < parent.childCount; i++)
		{
			parent.GetChild(i).GetComponent<UISetPageNumber>().UpdateSelection(this._page - 1);
		}
	}

	// Token: 0x06002735 RID: 10037 RVA: 0x00116DE8 File Offset: 0x00114FE8
	private void UpdateSelection(int number)
	{
		this._label = base.GetComponentsInChildren<UILabel>(true)[0];
		this._page = int.Parse(base.gameObject.name);
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		if (number + 1 == this._page)
		{
			this._label.ThemeAs = UIPalette.UI.ContextMenuHighlight;
			this._label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuHighlight];
			return;
		}
		this._label.ThemeAs = UIPalette.UI.ContextMenuText;
		this._label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuText];
	}

	// Token: 0x040019AB RID: 6571
	private UILabel _label;

	// Token: 0x040019AC RID: 6572
	private int _page;
}
