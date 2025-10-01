using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class UINotes : MonoBehaviour
{
	// Token: 0x060025BA RID: 9658 RVA: 0x00109AD9 File Offset: 0x00107CD9
	private IEnumerator Start()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.textInput.gameObject);
		uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnInputSelect));
		UIEventListener uieventListener2 = UIEventListener.Get(this.textInput.gameObject);
		uieventListener2.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onHover, new UIEventListener.BoolDelegate(this.OnInputHover));
		UIEventListener uieventListener3 = UIEventListener.Get(this.editButton);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnEditButtonClick));
		UIEventListener uieventListener4 = UIEventListener.Get(this.visibleButton);
		uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.OnVisibleButtonClick));
		yield return null;
		this.textInput.label.supportEncoding = true;
		yield break;
	}

	// Token: 0x060025BB RID: 9659 RVA: 0x00109AE8 File Offset: 0x00107CE8
	private void OnDestroy()
	{
		if (this.textInput != null)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.textInput.gameObject);
			uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnInputSelect));
			UIEventListener uieventListener2 = UIEventListener.Get(this.textInput.gameObject);
			uieventListener2.onHover = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener2.onHover, new UIEventListener.BoolDelegate(this.OnInputHover));
		}
		UIEventListener uieventListener3 = UIEventListener.Get(this.editButton);
		uieventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener3.onClick, new UIEventListener.VoidDelegate(this.OnEditButtonClick));
		UIEventListener uieventListener4 = UIEventListener.Get(this.visibleButton);
		uieventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener4.onClick, new UIEventListener.VoidDelegate(this.OnVisibleButtonClick));
	}

	// Token: 0x060025BC RID: 9660 RVA: 0x00109BBD File Offset: 0x00107DBD
	private void OnInputHover(GameObject go, bool isOver)
	{
		if (!isOver)
		{
			this.background.SetActive(false);
			return;
		}
		this.background.SetActive(true);
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x00109BDB File Offset: 0x00107DDB
	private void OnInputSelect(GameObject go, bool select)
	{
		if (!select)
		{
			this.background.SetActive(false);
			this.textInput.label.supportEncoding = true;
			return;
		}
		this.background.SetActive(true);
		this.textInput.label.supportEncoding = false;
	}

	// Token: 0x060025BE RID: 9662 RVA: 0x00109C1C File Offset: 0x00107E1C
	private void OnEditButtonClick(GameObject go)
	{
		if (base.gameObject.GetComponent<Collider2D>().enabled)
		{
			base.gameObject.GetComponent<Collider2D>().enabled = false;
			this.background.SetActive(false);
			return;
		}
		base.gameObject.GetComponent<Collider2D>().enabled = true;
		this.background.SetActive(true);
	}

	// Token: 0x060025BF RID: 9663 RVA: 0x00109C76 File Offset: 0x00107E76
	private void OnVisibleButtonClick(GameObject go)
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
			this.editButton.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		this.editButton.SetActive(true);
	}

	// Token: 0x0400186A RID: 6250
	public UIInput textInput;

	// Token: 0x0400186B RID: 6251
	public GameObject visibleButton;

	// Token: 0x0400186C RID: 6252
	public GameObject editButton;

	// Token: 0x0400186D RID: 6253
	public GameObject background;
}
