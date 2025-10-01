using System;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class UITab : MonoBehaviour
{
	// Token: 0x060027AB RID: 10155 RVA: 0x00119795 File Offset: 0x00117995
	public UITab()
	{
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x001197D4 File Offset: 0x001179D4
	public UITab(TabState ts)
	{
		this.title = ts.title;
		this.body = ts.body;
		if (string.IsNullOrEmpty(ts.color))
		{
			this.VisibleColor = ts.visibleColor.ToColour();
		}
		else
		{
			this.VisibleColor = Colour.ColourFromLabel(ts.color);
		}
		this.id = ts.id;
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x0011987C File Offset: 0x00117A7C
	private void Awake()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.deleteSprite);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnDeleteClick));
		EventManager.OnChangePlayerColor += this.OnPlayerChangedColor;
		EventManager.OnChangePlayerTeam += this.OnPlayerChangedTeam;
		this.Label = base.GetComponentInChildren<UILabel>();
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x001198E3 File Offset: 0x00117AE3
	private void Update()
	{
		if (this.Label.text != this.PrevLabel)
		{
			this.PrevLabel = this.Label.text;
			NGUIHelper.ClampAndAddDots(this.Label, base.gameObject, true);
		}
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x00119924 File Offset: 0x00117B24
	private void OnDestroy()
	{
		if (this.deleteSprite != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.deleteSprite);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnDeleteClick));
		}
		EventManager.OnChangePlayerColor -= this.OnPlayerChangedColor;
		EventManager.OnChangePlayerTeam -= this.OnPlayerChangedTeam;
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x060027B0 RID: 10160 RVA: 0x0011998D File Offset: 0x00117B8D
	// (set) Token: 0x060027B1 RID: 10161 RVA: 0x00119998 File Offset: 0x00117B98
	public Color VisibleColor
	{
		get
		{
			return this.visibleColor;
		}
		set
		{
			this.visibleColor = value;
			if (this.background != null)
			{
				if (Colour.IsPlayerColour(this.visibleColor))
				{
					this.background.ThemeAs = Colour.UIFromColour(this.visibleColor);
					this.background.ThemeAsSetting = this.background.ThemeAs;
					this.background.color = Singleton<UIPalette>.Instance.CurrentThemeColours[this.background.ThemeAs];
				}
				else
				{
					this.background.ThemeAs = UIPalette.UI.DoNotTheme;
					this.background.color = this.visibleColor;
				}
				if (PlayerScript.Pointer != null)
				{
					this.OnPlayerChangedColor(Colour.ColourFromLabel(PlayerScript.PointerScript.PointerColorLabel), NetworkID.ID);
				}
			}
		}
	}

	// Token: 0x060027B2 RID: 10162 RVA: 0x00119A75 File Offset: 0x00117C75
	private void LockTab(bool locked)
	{
		if (!this.allowLocking)
		{
			this.lockSprite.SetActive(false);
			return;
		}
		this.lockSprite.SetActive(locked);
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x00119A98 File Offset: 0x00117C98
	public void Selected(bool selected)
	{
		this.selectionRoot.SetActive(selected);
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x00119AA8 File Offset: 0x00117CA8
	public void DeepCopy(UITab uiTab)
	{
		this.title = uiTab.title;
		this.body = uiTab.body;
		this.allowLocking = uiTab.allowLocking;
		this.allowDelete = uiTab.allowDelete;
		this.VisibleColor = uiTab.visibleColor;
		this.id = uiTab.id;
		UITooltipScript component = base.GetComponent<UITooltipScript>();
		if (component != null)
		{
			component.Tooltip = this.title;
		}
		if (!this.allowDelete)
		{
			this.deleteSprite.GetComponent<Collider2D>().enabled = false;
			this.deleteSprite.GetComponent<UISprite>().enabled = false;
			return;
		}
		this.deleteSprite.GetComponent<Collider2D>().enabled = true;
		this.deleteSprite.GetComponent<UISprite>().enabled = true;
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x00119B66 File Offset: 0x00117D66
	public void OnDeleteClick(GameObject go)
	{
		if (this.OnDeleteClickCallback != null && this.allowDelete)
		{
			this.OnDeleteClickCallback(this);
		}
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x00119B84 File Offset: 0x00117D84
	private void OnPlayerChangedTeam(bool join, int id)
	{
		this.OnPlayerChangedColor(this.visibleColor, id);
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x00119B94 File Offset: 0x00117D94
	public void OnPlayerChangedColor(Color newColor, int id)
	{
		if (!(this.visibleColor != Color.grey) || !NetworkSingleton<PlayerManager>.Instance || NetworkSingleton<PlayerManager>.Instance.SameTeam(this.visibleColor))
		{
			this.LockTab(false);
			return;
		}
		if (PlayerScript.Pointer && PlayerScript.PointerScript.PointerColorLabel == "Black")
		{
			this.LockTab(false);
			return;
		}
		if (id == NetworkID.ID && newColor != this.visibleColor)
		{
			this.LockTab(true);
			return;
		}
		this.LockTab(true);
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x00119C28 File Offset: 0x00117E28
	public override bool Equals(object o)
	{
		if (o == null)
		{
			return false;
		}
		UITab uitab = o as UITab;
		return uitab != null && this.title.Equals(uitab.title) && this.body.Equals(uitab.body) && this.visibleColor.Equals(uitab.visibleColor) && this.id.Equals(uitab.id);
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x00119C9B File Offset: 0x00117E9B
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x04001A0A RID: 6666
	public string title = string.Empty;

	// Token: 0x04001A0B RID: 6667
	public string body = string.Empty;

	// Token: 0x04001A0C RID: 6668
	private Color visibleColor;

	// Token: 0x04001A0D RID: 6669
	public int id = -1;

	// Token: 0x04001A0E RID: 6670
	public bool allowLocking = true;

	// Token: 0x04001A0F RID: 6671
	public bool allowDelete = true;

	// Token: 0x04001A10 RID: 6672
	public UISprite background;

	// Token: 0x04001A11 RID: 6673
	public GameObject selectionRoot;

	// Token: 0x04001A12 RID: 6674
	public GameObject deleteSprite;

	// Token: 0x04001A13 RID: 6675
	public GameObject lockSprite;

	// Token: 0x04001A14 RID: 6676
	public UITab.DeleteClickHandler OnDeleteClickCallback;

	// Token: 0x04001A15 RID: 6677
	private UILabel Label;

	// Token: 0x04001A16 RID: 6678
	private string PrevLabel = "";

	// Token: 0x02000792 RID: 1938
	// (Invoke) Token: 0x06003F43 RID: 16195
	public delegate void DeleteClickHandler(UITab to);
}
