using System;
using System.Collections.Generic;
using System.Linq;
using NewNet;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class UINotepad : NetworkBehavior
{
	// Token: 0x06002587 RID: 9607 RVA: 0x001084C4 File Offset: 0x001066C4
	protected virtual void Awake()
	{
		EventDelegate.Add(this.tabTitle.onSubmit, new EventDelegate.Callback(this.OnTabTitleSubmit));
		if (this.newTabObject)
		{
			EventDelegate.Add(this.newTabObject.onSubmit, new EventDelegate.Callback(this.OnNewTabSubmit));
			UIEventListener uieventListener = UIEventListener.Get(this.newTabObject.gameObject);
			uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnNewTabSelect));
		}
		if (this.bodyScrollView && this.bodyScrollView.verticalScrollBar)
		{
			EventDelegate.Add(this.bodyScrollView.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollbarChanged));
		}
		if (this.bodyScrollView && this.bodyScrollView.horizontalScrollBar)
		{
			EventDelegate.Add(this.bodyScrollView.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollbarChanged));
		}
		EventDelegate.Add(this.tabBody.onChange, new EventDelegate.Callback(this.OnBodyChanged));
		UIEventListener uieventListener2 = UIEventListener.Get(this.tabBody.gameObject);
		uieventListener2.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onSelect, new UIEventListener.BoolDelegate(this.OnTabBodySelect));
		UIEventListener uieventListener3 = UIEventListener.Get(this.tabTitle.gameObject);
		uieventListener3.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener3.onSelect, new UIEventListener.BoolDelegate(this.OnTabTitleSelect));
		this.tabBodyCollider = this.tabBody.GetComponent<Collider2D>();
		this.tabTitleCollider = this.tabTitle.GetComponent<Collider2D>();
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x00108674 File Offset: 0x00106874
	protected void OnResize()
	{
		this.OnBodyChanged();
		this.ResetTabs();
		this.initialOffsetX = this.bodyTextPanel.clipOffset.x;
		this.initialOffsetY = this.bodyTextPanel.clipOffset.y;
		base.GetComponent<UIWidget>().SetDirty();
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x001086C4 File Offset: 0x001068C4
	private void CalcCaretLimit(UITexture caret)
	{
		this.MaxCaretX = float.MinValue;
		this.MinCaretX = float.MaxValue;
		this.MaxCaretY = float.MaxValue;
		this.MinCaretY = float.MinValue;
		foreach (Vector3 vector in caret.geometry.verts)
		{
			if (vector.x > this.MaxCaretX)
			{
				this.MaxCaretX = vector.x;
			}
			if (vector.x < this.MinCaretX)
			{
				this.MinCaretX = vector.x;
			}
			if (vector.y > this.MinCaretY)
			{
				this.MinCaretY = vector.y;
			}
			if (vector.y < this.MaxCaretY)
			{
				this.MaxCaretY = vector.y;
			}
		}
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x001087AC File Offset: 0x001069AC
	private void CalcScrollViewLimit()
	{
		this.minclipX = this.bodyTextPanel.clipOffset.x - this.initialOffsetX;
		this.maxClipX = this.minclipX + this.bodyTextPanel.finalClipRegion.z;
		this.minclipY = this.bodyTextPanel.clipOffset.y - this.initialOffsetY;
		this.maxClipY = this.minclipY + this.bodyTextPanel.finalClipRegion.w * -1f;
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x00108834 File Offset: 0x00106A34
	private void UpdateScroll()
	{
		if (this.tabBody.caret == null)
		{
			return;
		}
		UIScrollView component = this.bodyTextPanel.GetComponent<UIScrollView>();
		this.CalcCaretLimit(this.tabBody.caret);
		if (this.MaxCaretX != -3.4028235E+38f)
		{
			this.CalcScrollViewLimit();
			if ((this.MaxCaretX <= this.minclipX || this.MaxCaretX >= this.maxClipX) && component.horizontalScrollBar)
			{
				if (this.MaxCaretX > this.maxClipX)
				{
					component.MoveRelative(new Vector3(this.maxClipX - this.MaxCaretX, 0f, 0f));
				}
				else if (this.MinCaretX < this.minclipX)
				{
					component.MoveRelative(new Vector3(this.minclipX - this.MinCaretX, 0f, 0f));
				}
			}
			if (this.MaxCaretY <= this.minclipY || this.MaxCaretY >= this.maxClipY)
			{
				if (this.MaxCaretY < this.maxClipY)
				{
					component.MoveRelative(new Vector3(0f, this.maxClipY - this.MaxCaretY, 0f));
				}
				else if (this.MinCaretY > this.minclipY)
				{
					component.MoveRelative(new Vector3(0f, this.minclipY - this.MinCaretY, 0f));
				}
			}
		}
		component.UpdateScrollbars(true);
		this.UpdateTextBodyCollider();
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x001089A0 File Offset: 0x00106BA0
	private void Update()
	{
		if (this.tabBody.isSelected && this.tabBody.caret && (TTSInput.GetKey(KeyCode.Return) || TTSInput.GetKey(KeyCode.KeypadEnter) || TTSInput.GetKey(KeyCode.LeftArrow) || TTSInput.GetKey(KeyCode.RightArrow) || TTSInput.GetKey(KeyCode.UpArrow) || TTSInput.GetKey(KeyCode.DownArrow)))
		{
			this.OnBodyChanged();
		}
		this.tabBodyCollider.enabled = PermissionsOptions.CheckAllow(PermissionsOptions.options.Notes, -1);
		this.tabTitleCollider.enabled = PermissionsOptions.CheckAllow(PermissionsOptions.options.Notes, -1);
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x00108A4D File Offset: 0x00106C4D
	private void OnBodyChanged()
	{
		this.UpdateTextBodyCollider();
		this.UpdateScroll();
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x00108A5B File Offset: 0x00106C5B
	public virtual void Init()
	{
		this.NoTabSelected();
		this.ResetTabs();
		if (this.tabObjectList.Count > 0)
		{
			this.OnTabClicked(this.tabObjectList.First<UITab>().gameObject);
		}
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x00108A8D File Offset: 0x00106C8D
	protected void ResetTabs()
	{
		this.tabNameGrid.repositionNow = true;
		this.DelayResetTabs();
		base.Invoke("DelayResetTabs", 0.2f);
		base.Invoke("DelayResetTabs", 0.5f);
	}

	// Token: 0x06002590 RID: 9616 RVA: 0x00108AC1 File Offset: 0x00106CC1
	private void DelayResetTabs()
	{
		this.tabNameGrid.Reposition();
		this.tabScrollView.ResetPosition();
		this.tabScrollView.UpdateScrollbars();
	}

	// Token: 0x06002591 RID: 9617 RVA: 0x00108AE4 File Offset: 0x00106CE4
	protected virtual void OnDestroy()
	{
		if (this.bodyScrollView && this.bodyScrollView.verticalScrollBar)
		{
			EventDelegate.Remove(this.bodyScrollView.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollbarChanged));
		}
		if (this.bodyScrollView && this.bodyScrollView.horizontalScrollBar)
		{
			EventDelegate.Remove(this.bodyScrollView.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollbarChanged));
		}
		if (this.tabBody != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.tabBody.gameObject);
			uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnTabBodySelect));
			EventDelegate.Remove(this.tabBody.onChange, new EventDelegate.Callback(this.OnBodyChanged));
		}
		if (this.tabTitle != null)
		{
			UIEventListener uieventListener2 = UIEventListener.Get(this.tabTitle.gameObject);
			uieventListener2.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener2.onSelect, new UIEventListener.BoolDelegate(this.OnTabTitleSelect));
			EventDelegate.Remove(this.tabTitle.onSubmit, new EventDelegate.Callback(this.OnTabTitleSubmit));
		}
		if (this.newTabObject != null)
		{
			UIEventListener uieventListener3 = UIEventListener.Get(this.newTabObject.gameObject);
			uieventListener3.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener3.onSelect, new UIEventListener.BoolDelegate(this.OnNewTabSelect));
			EventDelegate.Remove(this.newTabObject.onSubmit, new EventDelegate.Callback(this.OnNewTabSubmit));
		}
		foreach (UITab uitab in this.tabObjectList)
		{
			if (uitab != null)
			{
				UIEventListener uieventListener4 = UIEventListener.Get(uitab.gameObject);
				uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
			}
		}
	}

	// Token: 0x06002592 RID: 9618 RVA: 0x00108D04 File Offset: 0x00106F04
	public void ClearTabs()
	{
		foreach (UITab uitab in this.tabObjectList)
		{
			if (uitab != null)
			{
				UIEventListener uieventListener = UIEventListener.Get(uitab.gameObject);
				uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
				UnityEngine.Object.Destroy(uitab.gameObject);
			}
		}
		this.tabObjectList.Clear();
		this.tabObjects.Clear();
	}

	// Token: 0x06002593 RID: 9619 RVA: 0x00108DA8 File Offset: 0x00106FA8
	public void LoadNotepad(Dictionary<int, TabState> loadTextObjects)
	{
		this.ClearTabs();
		this.currentIndex = -1;
		if (loadTextObjects.Count <= 0)
		{
			this.Init();
			return;
		}
		foreach (KeyValuePair<int, TabState> keyValuePair in loadTextObjects)
		{
			UITab uiTab = new UITab(keyValuePair.Value);
			this.AddTab(uiTab, keyValuePair.Key);
		}
		this.ResetTabs();
		if (this.tabObjectList.Count > 0)
		{
			this.OnTabClicked(this.tabObjectList.First<UITab>().gameObject);
		}
	}

	// Token: 0x06002594 RID: 9620 RVA: 0x00108E54 File Offset: 0x00107054
	protected virtual UITab AddTab(UITab uiTab, int id)
	{
		UITab component = this.tabNameGrid.gameObject.AddChild(this.tabPrefab).GetComponent<UITab>();
		component.OnDeleteClickCallback = new UITab.DeleteClickHandler(this.OnDeleteTab);
		component.DeepCopy(uiTab);
		component.id = id;
		component.GetComponent<UILabel>().text = component.title;
		UIEventListener uieventListener = UIEventListener.Get(component.gameObject);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
		this.tabObjectList.Add(component);
		this.tabObjects[component.id] = component;
		return component;
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x00108EFB File Offset: 0x001070FB
	public virtual void OnDeleteTab(UITab to)
	{
		this.tabToDelete = to;
		UIDialog.Show(Language.Translate("Are you sure you want to delete {0}?", to.title), "Yes", "No", new Action(this.DeleteTab), null);
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x00108F34 File Offset: 0x00107134
	public void DoDeleteTab(UITab toDelete)
	{
		if (this.tabObjects.ContainsKey(toDelete.id))
		{
			bool flag = true;
			if (this.currentIndex >= 0 && !this.tabObjects[this.currentIndex].Equals(toDelete))
			{
				flag = false;
			}
			UITab uitab = this.tabObjects[toDelete.id];
			this.tabObjectList.Remove(uitab);
			UIEventListener uieventListener = UIEventListener.Get(uitab.gameObject);
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
			UnityEngine.Object.Destroy(uitab.gameObject);
			this.tabObjects.Remove(toDelete.id);
			if (flag)
			{
				this.NoTabSelected();
			}
			this.ResetTabs();
		}
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x00108FF5 File Offset: 0x001071F5
	protected virtual void DeleteTab()
	{
		this.DoDeleteTab(this.tabToDelete);
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x00109004 File Offset: 0x00107204
	protected virtual void OnNewTabSelect(GameObject go, bool select)
	{
		if (select)
		{
			if (this.currentIndex >= 0)
			{
				this.tabObjects[this.currentIndex].Selected(false);
			}
			this.NoTabSelected();
			return;
		}
		if (!string.IsNullOrEmpty(this.newTabObject.value))
		{
			this.OnNewTabSubmit();
		}
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x00109054 File Offset: 0x00107254
	protected virtual void OnNewTabSubmit()
	{
		UITab uitab = new UITab();
		uitab.title = this.newTabObject.value;
		uitab.VisibleColor = this.currentVisibleColor;
		this.newTabObject.value = string.Empty;
		this.newTabObject.label.text = "Add New";
		if (Network.isServer)
		{
			this.AddNewTabServer(uitab);
			return;
		}
		this.NotepadAddTabServer(new TabState(uitab));
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x001090C4 File Offset: 0x001072C4
	public virtual void AddNewTabServer(UITab to)
	{
		to = this.AddTab(to, this.GetNextId());
		this.tabNameGrid.repositionNow = true;
		this.ResetTabs();
		this.OnTabClicked(to.gameObject);
		this.NotepadAddTabOthers(new TabState(to));
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x001090FF File Offset: 0x001072FF
	public void AddNewTabOthers(UITab to)
	{
		this.AddTab(to, to.id);
		this.tabNameGrid.repositionNow = true;
		this.ResetTabs();
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x00109124 File Offset: 0x00107324
	protected virtual void OnTabTitleSubmit()
	{
		if (this.currentIndex < 0)
		{
			return;
		}
		UITab uitab = this.tabObjects[this.currentIndex];
		if (!string.IsNullOrEmpty(this.tabTitle.value) || !this.tabTitle.value.Equals(uitab.title))
		{
			uitab.GetComponent<UILabel>().text = this.tabTitle.value;
			uitab.title = this.tabTitle.value;
			this.SendNetworkTabUpdate();
		}
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x001091A4 File Offset: 0x001073A4
	protected virtual void OnTabTitleSelect(GameObject go, bool select)
	{
		if (!select && this.currentIndex >= 0)
		{
			UITab uitab = this.tabObjects[this.currentIndex];
			if (!string.IsNullOrEmpty(this.tabTitle.value) || !this.tabTitle.value.Equals(uitab.title))
			{
				uitab.GetComponent<UILabel>().text = this.tabTitle.value;
				uitab.title = this.tabTitle.value;
				this.SendNetworkTabUpdate();
			}
		}
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x00109228 File Offset: 0x00107428
	protected virtual void OnTabBodySelect(GameObject go, bool select)
	{
		if (!select && this.currentIndex >= 0)
		{
			UITab uitab = this.tabObjects[this.currentIndex];
			if (!string.IsNullOrEmpty(this.tabBody.value) || !this.tabBody.value.Equals(uitab.body))
			{
				uitab.body = this.tabBody.value;
				this.UpdateTextBodyCollider();
			}
			if (PlayerScript.Pointer && uitab.VisibleColor == PlayerScript.PointerScript.PointerColour)
			{
				Achievements.Set("ACH_USE_PRIVATE_NOTEPAD");
			}
		}
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x001092C7 File Offset: 0x001074C7
	protected void OnScrollbarChanged()
	{
		if (this.tabBody.label.text.Length > 10000)
		{
			this.tabBody.label.ProcessText(false, true);
		}
		this.UpdateTextBodyCollider();
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x00109300 File Offset: 0x00107500
	protected void UpdateTextBodyCollider()
	{
		Vector2 size = this.tabBody.GetComponent<BoxCollider2D>().size;
		size.x = Mathf.Max(100000f, 4f * Mathf.Abs(this.tabBody.label.drawingDimensions.x));
		size.y = Mathf.Max(100000f, 4f * Mathf.Abs(this.tabBody.label.drawingDimensions.y));
		this.tabBody.GetComponent<BoxCollider2D>().size = size;
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x00109394 File Offset: 0x00107594
	protected virtual void OnTabClicked(GameObject go)
	{
		UITab component = go.GetComponent<UITab>();
		if (component.lockSprite.activeSelf)
		{
			return;
		}
		if (this.currentIndex >= 0)
		{
			this.tabObjects[this.currentIndex].Selected(false);
		}
		if (this.tabTitle.GetComponent<Collider2D>())
		{
			this.tabTitle.GetComponent<Collider2D>().enabled = true;
		}
		this.tabBody.GetComponent<Collider2D>().enabled = true;
		component = go.GetComponent<UITab>();
		this.currentIndex = component.id;
		component.Selected(true);
		this.tabTitle.value = component.title;
		this.tabTitle.label.text = component.title;
		this.tabBody.value = component.body;
		this.tabBody.label.text = component.body;
		this.UpdateTextBodyCollider();
		this.bodyTextPanel.GetComponent<UIScrollView>().ResetPosition();
		this.initialOffsetX = this.bodyTextPanel.clipOffset.x;
		this.initialOffsetY = this.bodyTextPanel.clipOffset.y;
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x001094B4 File Offset: 0x001076B4
	protected void SendNetworkTabUpdate()
	{
		this.NotepadTabChange(new Dictionary<int, TabState>
		{
			{
				this.currentIndex,
				new TabState(this.tabObjects[this.currentIndex])
			}
		});
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x001094F0 File Offset: 0x001076F0
	public Dictionary<int, TabState> GetSaveState()
	{
		Dictionary<int, TabState> dictionary = new Dictionary<int, TabState>();
		foreach (KeyValuePair<int, UITab> keyValuePair in this.tabObjects)
		{
			dictionary.Add(keyValuePair.Key, new TabState(keyValuePair.Value));
		}
		return dictionary;
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x0010955C File Offset: 0x0010775C
	public void UpdateTab(Dictionary<int, TabState> textToUpdate)
	{
		if (textToUpdate == null || textToUpdate.Count <= 0)
		{
			return;
		}
		KeyValuePair<int, TabState> keyValuePair = textToUpdate.FirstOrDefault<KeyValuePair<int, TabState>>();
		if (this.tabObjects.ContainsKey(keyValuePair.Key))
		{
			UITab uitab = new UITab(keyValuePair.Value);
			if (this.tabObjects[keyValuePair.Key].Equals(uitab))
			{
				return;
			}
			this.tabObjects[keyValuePair.Key].DeepCopy(uitab);
			this.tabObjects[keyValuePair.Key].GetComponent<UILabel>().text = uitab.title;
			if (keyValuePair.Key == this.currentIndex && !this.tabObjects[keyValuePair.Key].lockSprite.activeSelf)
			{
				this.OnTabClicked(this.tabObjects[keyValuePair.Key].gameObject);
			}
			else
			{
				this.NoTabSelected();
			}
			this.ResetTabs();
		}
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x00109650 File Offset: 0x00107850
	public void SetTabFromRulesLegacy(string rules)
	{
		if (this.tabObjects.Count <= 0)
		{
			return;
		}
		UITab uitab = null;
		foreach (KeyValuePair<int, UITab> keyValuePair in this.tabObjects)
		{
			if (keyValuePair.Value.title.Equals("rules", StringComparison.CurrentCultureIgnoreCase))
			{
				uitab = keyValuePair.Value;
				break;
			}
		}
		if (uitab == null)
		{
			if (this.tabObjectList.Count > 0)
			{
				this.OnTabClicked(this.tabObjectList.First<UITab>().gameObject);
				return;
			}
			uitab = new UITab();
			uitab.title = "Rules";
			uitab.VisibleColor = Colour.Grey;
			uitab.body = rules;
			if (Network.isServer)
			{
				this.AddNewTabServer(uitab);
			}
			else
			{
				this.NotepadAddTabServer(new TabState(uitab));
			}
		}
		else
		{
			uitab.body = rules;
			this.OnTabClicked(uitab.gameObject);
		}
		this.ResetTabs();
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x00109760 File Offset: 0x00107960
	protected void NoTabSelected()
	{
		this.currentIndex = -1;
		this.tabTitle.GetComponent<UILabel>().text = string.Empty;
		this.tabBody.GetComponent<UILabel>().text = string.Empty;
		if (this.tabTitle.GetComponent<Collider2D>())
		{
			this.tabTitle.GetComponent<Collider2D>().enabled = false;
		}
		this.tabBody.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x001097D2 File Offset: 0x001079D2
	private void OnPlayerChangedTeam(bool join, int id)
	{
		this.OnPlayerColorChanged(this.tabObjects[this.currentIndex].VisibleColor, id);
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x001097F4 File Offset: 0x001079F4
	private void OnPlayerColorChanged(Color newColor, int id)
	{
		if (this.tabObjects.ContainsKey(this.currentIndex) && (new Colour(this.tabObjects[this.currentIndex].VisibleColor) == Colour.Grey || newColor == this.tabObjects[this.currentIndex].VisibleColor || NetworkSingleton<PlayerManager>.Instance.SameTeam(this.tabObjects[this.currentIndex].VisibleColor)))
		{
			return;
		}
		int num = -1;
		foreach (KeyValuePair<int, UITab> keyValuePair in this.tabObjects)
		{
			if (new Colour(keyValuePair.Value.VisibleColor) == Colour.Grey || keyValuePair.Value.VisibleColor == newColor || NetworkSingleton<PlayerManager>.Instance.SameTeam(keyValuePair.Value.VisibleColor))
			{
				num = keyValuePair.Key;
				break;
			}
		}
		if (num < 0)
		{
			this.NoTabSelected();
			return;
		}
		this.OnTabClicked(this.tabObjects[num].gameObject);
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x00109934 File Offset: 0x00107B34
	public List<UITab> GetAllTabs()
	{
		return this.tabObjectList;
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x0010993C File Offset: 0x00107B3C
	public UITab GetTab(int IndexOfTab)
	{
		return this.tabObjectList[IndexOfTab];
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x0010994A File Offset: 0x00107B4A
	public int GetNumTabs()
	{
		return this.tabObjectList.Count;
	}

	// Token: 0x060025AC RID: 9644 RVA: 0x00109958 File Offset: 0x00107B58
	public int GetNextId()
	{
		int num = 0;
		foreach (int num2 in this.tabObjects.Keys)
		{
			if (num2 >= num)
			{
				num = num2 + 1;
			}
		}
		return num;
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x001099B4 File Offset: 0x00107BB4
	public void DeleteTabOthers(UITab tab)
	{
		this.tabToDelete = tab;
		this.DeleteTab();
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x001099C3 File Offset: 0x00107BC3
	public void UpdateNotepadForPlayer(NetworkPlayer player)
	{
		base.networkView.RPC<Dictionary<int, TabState>>(player, new Action<Dictionary<int, TabState>>(this.UpdateNotepadRPC), this.GetSaveState());
	}

	// Token: 0x060025AF RID: 9647 RVA: 0x001099E3 File Offset: 0x00107BE3
	public void NotepadChange()
	{
		base.networkView.RPC<Dictionary<int, TabState>>(RPCTarget.Others, new Action<Dictionary<int, TabState>>(this.UpdateNotepadRPC), this.GetSaveState());
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x00109A03 File Offset: 0x00107C03
	public void NotepadTabChange(Dictionary<int, TabState> singleChange)
	{
		base.networkView.RPC<Dictionary<int, TabState>>(RPCTarget.All, new Action<Dictionary<int, TabState>>(this.UpdateTabRPC), singleChange);
	}

	// Token: 0x060025B1 RID: 9649 RVA: 0x00109A1E File Offset: 0x00107C1E
	public void NotepadTabDelete(TabState singleDelete)
	{
		base.networkView.RPC<TabState>(RPCTarget.Others, new Action<TabState>(this.DeleteTabRPC), singleDelete);
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x00109A39 File Offset: 0x00107C39
	public void NotepadAddTabOthers(TabState addedTab)
	{
		base.networkView.RPC<TabState>(RPCTarget.Others, new Action<TabState>(this.NotepadAddTabOthersRPC), addedTab);
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x00109A54 File Offset: 0x00107C54
	public void NotepadAddTabServer(TabState addedTab)
	{
		base.networkView.RPC<TabState>(RPCTarget.Server, new Action<TabState>(this.NotepadNewTabAddServerRPC), addedTab);
	}

	// Token: 0x060025B4 RID: 9652 RVA: 0x00109A6F File Offset: 0x00107C6F
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void NotepadNewTabAddServerRPC(TabState tab)
	{
		this.AddNewTabServer(new UITab(tab));
	}

	// Token: 0x060025B5 RID: 9653 RVA: 0x00109A7D File Offset: 0x00107C7D
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void NotepadAddTabOthersRPC(TabState tab)
	{
		this.AddNewTabOthers(new UITab(tab));
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x00109A8B File Offset: 0x00107C8B
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void DeleteTabRPC(TabState tab)
	{
		this.DeleteTabOthers(new UITab(tab));
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x00109A99 File Offset: 0x00107C99
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void UpdateTabRPC(Dictionary<int, TabState> singleChange)
	{
		this.UpdateTab(singleChange);
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x00109AA2 File Offset: 0x00107CA2
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void UpdateNotepadRPC(Dictionary<int, TabState> tabs)
	{
		this.LoadNotepad(tabs);
	}

	// Token: 0x0400184C RID: 6220
	public GameObject tabPrefab;

	// Token: 0x0400184D RID: 6221
	public UIInput tabTitle;

	// Token: 0x0400184E RID: 6222
	public UIInput tabBody;

	// Token: 0x0400184F RID: 6223
	public UIInput newTabObject;

	// Token: 0x04001850 RID: 6224
	public UILabel newTabObjectLabel;

	// Token: 0x04001851 RID: 6225
	public UIGrid tabNameGrid;

	// Token: 0x04001852 RID: 6226
	public UIPanel bodyTextPanel;

	// Token: 0x04001853 RID: 6227
	public UIScrollView tabScrollView;

	// Token: 0x04001854 RID: 6228
	protected int currentIndex;

	// Token: 0x04001855 RID: 6229
	public Dictionary<int, UITab> tabObjects = new Dictionary<int, UITab>();

	// Token: 0x04001856 RID: 6230
	protected List<UITab> tabObjectList = new List<UITab>();

	// Token: 0x04001857 RID: 6231
	public UIDialog dialog;

	// Token: 0x04001858 RID: 6232
	protected UITab tabToDelete;

	// Token: 0x04001859 RID: 6233
	public UISprite titleBackground;

	// Token: 0x0400185A RID: 6234
	public UISprite bodyBackground;

	// Token: 0x0400185B RID: 6235
	protected Color currentVisibleColor = Colour.Grey;

	// Token: 0x0400185C RID: 6236
	public UIScrollView bodyScrollView;

	// Token: 0x0400185D RID: 6237
	private float MaxCaretX;

	// Token: 0x0400185E RID: 6238
	private float MinCaretX;

	// Token: 0x0400185F RID: 6239
	private float MaxCaretY;

	// Token: 0x04001860 RID: 6240
	private float MinCaretY;

	// Token: 0x04001861 RID: 6241
	private float minclipX;

	// Token: 0x04001862 RID: 6242
	private float maxClipX;

	// Token: 0x04001863 RID: 6243
	private float minclipY;

	// Token: 0x04001864 RID: 6244
	private float maxClipY;

	// Token: 0x04001865 RID: 6245
	private float initialOffsetX;

	// Token: 0x04001866 RID: 6246
	private float initialOffsetY;

	// Token: 0x04001867 RID: 6247
	public UIDragResize resize;

	// Token: 0x04001868 RID: 6248
	private Collider2D tabBodyCollider;

	// Token: 0x04001869 RID: 6249
	private Collider2D tabTitleCollider;
}
