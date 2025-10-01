using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class UINotebook : UINotepad, INotifySceneAwake
{
	// Token: 0x06002570 RID: 9584 RVA: 0x00107940 File Offset: 0x00105B40
	protected override void Awake()
	{
		base.Awake();
		this.tabTitle.GetComponent<UILabel>().supportEncoding = true;
		this.tabBody.GetComponent<UILabel>().supportEncoding = true;
		EventManager.OnChangePlayerColor += this.OnPlayerColorChanged;
		EventManager.OnLanguageChange += this.OnLanguageChange;
		UIEventListener uieventListener = UIEventListener.Get(this.visibleButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnVisibleButtonClick));
		this.HideWheel(0f);
		base.GetComponent<UISprite>().enabled = false;
		base.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("NotebookX", 0f), PlayerPrefs.GetFloat("NotebookY", 0f), base.transform.localPosition.z);
		base.GetComponent<UIWidget>().width = PlayerPrefs.GetInt("NotebookWidth", 630);
		base.GetComponent<UIWidget>().height = PlayerPrefs.GetInt("NotebookHeight", 602);
		base.GetComponent<UISprite>().enabled = true;
		UIEventListener uieventListener2 = UIEventListener.Get(this.resize.gameObject);
		uieventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener2.onDrag, new UIEventListener.VectorDelegate(this.NotebookResize));
		base.OnResize();
	}

	// Token: 0x06002571 RID: 9585 RVA: 0x00107A90 File Offset: 0x00105C90
	private void HideWheel(float delay)
	{
		Collider2D[] array = this.colorWhellColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		TweenScale.Begin(this.colorWheel, delay, Vector3.zero);
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x00107AD0 File Offset: 0x00105CD0
	public override void Init()
	{
		base.NoTabSelected();
		UITab uitab = new UITab();
		uitab.title = "Rules";
		uitab.VisibleColor = Colour.Grey;
		this.AddTab(uitab, 0);
		uitab.title = "White";
		uitab.VisibleColor = Colour.White;
		this.AddTab(uitab, 1);
		uitab.title = "Brown";
		uitab.VisibleColor = Colour.Brown;
		this.AddTab(uitab, 2);
		uitab.title = "Red";
		uitab.VisibleColor = Colour.Red;
		this.AddTab(uitab, 3);
		uitab.title = "Orange";
		uitab.VisibleColor = Colour.Orange;
		this.AddTab(uitab, 4);
		uitab.title = "Yellow";
		uitab.VisibleColor = Colour.Yellow;
		this.AddTab(uitab, 5);
		uitab.title = "Green";
		uitab.VisibleColor = Colour.Green;
		this.AddTab(uitab, 6);
		uitab.title = "Blue";
		uitab.VisibleColor = Colour.Blue;
		this.AddTab(uitab, 7);
		uitab.title = "Teal";
		uitab.VisibleColor = Colour.Teal;
		this.AddTab(uitab, 8);
		uitab.title = "Purple";
		uitab.VisibleColor = Colour.Purple;
		this.AddTab(uitab, 9);
		uitab.title = "Pink";
		uitab.VisibleColor = Colour.Pink;
		this.AddTab(uitab, 10);
		uitab.title = "Black";
		uitab.VisibleColor = Colour.Black;
		this.AddTab(uitab, 11);
		base.ResetTabs();
		if (this.tabObjectList.Count > 0)
		{
			this.OnTabClicked(this.tabObjectList.First<UITab>().gameObject);
		}
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x00107CC8 File Offset: 0x00105EC8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventManager.OnChangePlayerColor -= this.OnPlayerColorChanged;
		EventManager.OnLanguageChange -= this.OnLanguageChange;
		if (this.visibleButton != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.visibleButton);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnVisibleButtonClick));
		}
		if (this.resize != null)
		{
			UIEventListener uieventListener2 = UIEventListener.Get(this.resize.gameObject);
			uieventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener2.onDrag, new UIEventListener.VectorDelegate(this.NotebookResize));
		}
	}

	// Token: 0x06002574 RID: 9588 RVA: 0x00107D78 File Offset: 0x00105F78
	private void OnDrag(Vector2 delta)
	{
		PlayerPrefs.SetInt("NotebookWidth", base.GetComponent<UIWidget>().width);
		PlayerPrefs.SetInt("NotebookHeight", base.GetComponent<UIWidget>().height);
		PlayerPrefs.SetFloat("NotebookX", base.transform.localPosition.x);
		PlayerPrefs.SetFloat("NotebookY", base.transform.localPosition.y);
	}

	// Token: 0x06002575 RID: 9589 RVA: 0x00107DE4 File Offset: 0x00105FE4
	private void NotebookResize(GameObject go, Vector2 vec2)
	{
		PlayerPrefs.SetInt("NotebookWidth", base.GetComponent<UIWidget>().width);
		PlayerPrefs.SetInt("NotebookHeight", base.GetComponent<UIWidget>().height);
		PlayerPrefs.SetFloat("NotebookX", base.transform.localPosition.x);
		PlayerPrefs.SetFloat("NotebookY", base.transform.localPosition.y);
		base.OnResize();
	}

	// Token: 0x06002576 RID: 9590 RVA: 0x00107E55 File Offset: 0x00106055
	public override void AddNewTabServer(UITab to)
	{
		if (!this.CheckIfInteractionAllowed(false))
		{
			return;
		}
		base.AddNewTabServer(to);
		this.newTabObjectLabel.supportEncoding = true;
	}

	// Token: 0x06002577 RID: 9591 RVA: 0x00107E74 File Offset: 0x00106074
	protected override void OnTabTitleSubmit()
	{
		base.OnTabTitleSubmit();
		UITab uitab = this.tabObjects[this.currentIndex];
		if (!string.IsNullOrEmpty(this.tabTitle.value) || !this.tabTitle.value.Equals(uitab.title))
		{
			this.tabTitle.GetComponent<UILabel>().supportEncoding = true;
		}
	}

	// Token: 0x06002578 RID: 9592 RVA: 0x00107ED4 File Offset: 0x001060D4
	protected override void OnTabTitleSelect(GameObject go, bool select)
	{
		if (select && !this.CheckIfInteractionAllowed(false))
		{
			return;
		}
		if (!select && !this.CheckIfInteractionAllowed(true))
		{
			return;
		}
		base.OnTabTitleSelect(go, select);
		if (select)
		{
			this.tabTitle.GetComponent<UILabel>().supportEncoding = false;
			return;
		}
		if (this.currentIndex >= 0)
		{
			UITab uitab = this.tabObjects[this.currentIndex];
			if (!string.IsNullOrEmpty(this.tabTitle.value) || !this.tabTitle.value.Equals(uitab.title))
			{
				this.tabTitle.GetComponent<UILabel>().supportEncoding = true;
			}
		}
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x00107F6E File Offset: 0x0010616E
	protected override void OnNewTabSelect(GameObject go, bool select)
	{
		base.OnNewTabSelect(go, select);
		if (select)
		{
			this.ChangeFilterColor(Colour.Grey);
			this.newTabObjectLabel.supportEncoding = false;
		}
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x00107F97 File Offset: 0x00106197
	private void OnLanguageChange(string oldCode, string newCode)
	{
		this.visibleText.text = Language.Translate(Colour.LabelFromColour(this.currentVisibleColor));
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x00107FBC File Offset: 0x001061BC
	private void ChangeFilterColor(Color color)
	{
		this.currentVisibleColor = color;
		this.visibleText.text = Language.Translate(Colour.LabelFromColour(this.currentVisibleColor));
		this.visibleButton.GetComponent<UISprite>().color = this.currentVisibleColor;
		this.colorIndicatorStrip.GetComponent<UISprite>().color = this.currentVisibleColor;
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x0010801C File Offset: 0x0010621C
	protected override void OnTabClicked(GameObject go)
	{
		this.HideWheel(0.15f);
		UITab component = go.GetComponent<UITab>();
		if (component.lockSprite.activeSelf)
		{
			return;
		}
		if (this.currentIndex >= 0)
		{
			this.tabObjects[this.currentIndex].Selected(false);
		}
		this.tabTitle.GetComponent<Collider2D>().enabled = true;
		this.tabBody.GetComponent<Collider2D>().enabled = true;
		component = go.GetComponent<UITab>();
		this.currentIndex = component.id;
		component.Selected(true);
		this.tabTitle.value = component.title;
		this.tabTitle.label.text = component.title;
		this.tabBody.value = component.body;
		this.tabBody.label.text = component.body;
		base.UpdateTextBodyCollider();
		this.bodyTextPanel.GetComponent<UIScrollView>().ResetPosition();
		this.ChangeFilterColor(component.VisibleColor);
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x00108118 File Offset: 0x00106318
	public void OnColorFilterSelected(Color color)
	{
		this.currentVisibleColor = color;
		if (this.currentIndex >= 0)
		{
			this.tabObjects[this.currentIndex].VisibleColor = color;
			base.SendNetworkTabUpdate();
		}
		this.visibleText.text = Language.Translate(Colour.LabelFromColour(color));
		this.visibleButton.GetComponent<UISprite>().color = color;
		this.colorIndicatorStrip.GetComponent<UISprite>().color = color;
		this.HideWheel(0.15f);
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x0010819C File Offset: 0x0010639C
	private void OnColorWheelTweenFinish()
	{
		Collider2D[] array = this.colorWhellColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x001081C8 File Offset: 0x001063C8
	public void OnVisibleButtonClick(GameObject go)
	{
		if (!this.CheckIfInteractionAllowed(false))
		{
			return;
		}
		Collider2D[] array = this.colorWhellColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		EventDelegate.Add(TweenScale.Begin(this.colorWheel, 0.15f, Vector3.one).onFinished, new EventDelegate.Callback(this.OnColorWheelTweenFinish), true);
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x0010822A File Offset: 0x0010642A
	private void OnPlayerChangedTeam(bool join, int id)
	{
		this.OnPlayerColorChanged(this.tabObjects[this.currentIndex].VisibleColor, id);
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x0010824C File Offset: 0x0010644C
	protected override void OnTabBodySelect(GameObject go, bool select)
	{
		if (select && !this.CheckIfInteractionAllowed(false))
		{
			return;
		}
		if (!select && !this.CheckIfInteractionAllowed(true))
		{
			return;
		}
		base.OnTabBodySelect(go, select);
		if (select)
		{
			this.tabBody.GetComponent<UILabel>().supportEncoding = false;
			return;
		}
		if (this.currentIndex >= 0)
		{
			this.tabBody.GetComponent<UILabel>().supportEncoding = true;
			UITab uitab = this.tabObjects[this.currentIndex];
			if (!string.IsNullOrEmpty(this.tabBody.value) || !this.tabBody.value.Equals(uitab.body))
			{
				base.SendNetworkTabUpdate();
			}
		}
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x001082EC File Offset: 0x001064EC
	protected override void DeleteTab()
	{
		if (!this.CheckIfInteractionAllowed(false))
		{
			return;
		}
		if (this.tabObjects.ContainsKey(this.tabToDelete.id))
		{
			base.NotepadTabDelete(new TabState(this.tabToDelete));
		}
		base.DeleteTab();
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x00108328 File Offset: 0x00106528
	private void OnPlayerColorChanged(Color newColor, int id)
	{
		if (this.tabObjects.ContainsKey(this.currentIndex) && (new Colour(this.tabObjects[this.currentIndex].VisibleColor) == Colour.Grey || newColor == this.tabObjects[this.currentIndex].VisibleColor || NetworkSingleton<PlayerManager>.Instance.SameTeam(this.tabObjects[this.currentIndex].VisibleColor)))
		{
			return;
		}
		int num = -1;
		foreach (KeyValuePair<int, UITab> keyValuePair in this.tabObjects)
		{
			if (keyValuePair.Value.VisibleColor == Colour.Grey || keyValuePair.Value.VisibleColor == newColor || NetworkSingleton<PlayerManager>.Instance.SameTeam(keyValuePair.Value.VisibleColor))
			{
				num = keyValuePair.Key;
				break;
			}
		}
		if (num < 0)
		{
			base.NoTabSelected();
			return;
		}
		this.OnTabClicked(this.tabObjects[num].gameObject);
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x00108464 File Offset: 0x00106664
	private bool CheckIfInteractionAllowed(bool silent = false)
	{
		if (!PlayerScript.Pointer)
		{
			if (!silent)
			{
				Chat.LogError("Grey (Spectator) cannot interact. Click your name in the top right -> Change Color, then click a colored circle.", true);
			}
			return false;
		}
		if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.NotebookEdit, -1))
		{
			if (!silent)
			{
				PermissionsOptions.BroadcastPermissionWarning("edit the Notebook");
			}
			return false;
		}
		return true;
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x001084A4 File Offset: 0x001066A4
	public void SceneAwake()
	{
		Wait.Frames(new Action(this.Init), 1);
	}

	// Token: 0x04001846 RID: 6214
	public GameObject visibleButton;

	// Token: 0x04001847 RID: 6215
	public GameObject colorWheel;

	// Token: 0x04001848 RID: 6216
	public Collider2D[] colorWhellColliders;

	// Token: 0x04001849 RID: 6217
	public UILabel visibleText;

	// Token: 0x0400184A RID: 6218
	public UISprite colorIndicatorStrip;

	// Token: 0x0400184B RID: 6219
	private string legacyRules;
}
