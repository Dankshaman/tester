using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000048 RID: 72
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : UIWidgetContainer
{
	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060001FD RID: 509 RVA: 0x0000C4BE File Offset: 0x0000A6BE
	// (set) Token: 0x060001FE RID: 510 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = (value as Font);
				this.bitmapFont = null;
				this.font = null;
				return;
			}
			if (value is UIFont)
			{
				this.bitmapFont = (value as UIFont);
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060001FF RID: 511 RVA: 0x0000C542 File Offset: 0x0000A742
	// (set) Token: 0x06000200 RID: 512 RVA: 0x0000C54A File Offset: 0x0000A74A
	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000201 RID: 513 RVA: 0x0000C553 File Offset: 0x0000A753
	public static bool isOpen
	{
		get
		{
			return UIPopupList.current != null && (UIPopupList.mChild != null || UIPopupList.mFadeOutComplete > Time.unscaledTime);
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000202 RID: 514 RVA: 0x0000C57F File Offset: 0x0000A77F
	// (set) Token: 0x06000203 RID: 515 RVA: 0x0000C587 File Offset: 0x0000A787
	public virtual string value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.Set(value, true);
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000204 RID: 516 RVA: 0x0000C594 File Offset: 0x0000A794
	public virtual object data
	{
		get
		{
			int num = this.items.IndexOf(this.mSelectedItem);
			if (num <= -1 || num >= this.itemData.Count)
			{
				return null;
			}
			return this.itemData[num];
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000205 RID: 517 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000206 RID: 518 RVA: 0x0000C610 File Offset: 0x0000A810
	// (set) Token: 0x06000207 RID: 519 RVA: 0x0000C618 File Offset: 0x0000A818
	[Obsolete("Use 'value' instead")]
	public string selection
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000208 RID: 520 RVA: 0x0000C621 File Offset: 0x0000A821
	private bool isValid
	{
		get
		{
			return this.bitmapFont != null || this.trueTypeFont != null;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000209 RID: 521 RVA: 0x0000C63F File Offset: 0x0000A83F
	private int activeFontSize
	{
		get
		{
			if (!(this.trueTypeFont != null) && !(this.bitmapFont == null))
			{
				return this.bitmapFont.defaultSize;
			}
			return this.fontSize;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600020A RID: 522 RVA: 0x0000C66F File Offset: 0x0000A86F
	private float activeFontScale
	{
		get
		{
			if (!(this.trueTypeFont != null) && !(this.bitmapFont == null))
			{
				return (float)this.fontSize / (float)this.bitmapFont.defaultSize;
			}
			return 1f;
		}
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000C6A7 File Offset: 0x0000A8A7
	public void Set(string value, bool notify = true)
	{
		this.mSelectedItem = value;
		if (this.mSelectedItem == null)
		{
			return;
		}
		if (notify && this.mSelectedItem != null)
		{
			this.TriggerCallbacks();
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000C6CA File Offset: 0x0000A8CA
	public virtual void Clear()
	{
		this.items.Clear();
		this.itemData.Clear();
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000C6E2 File Offset: 0x0000A8E2
	public virtual void AddItem(string text)
	{
		this.items.Add(text);
		this.itemData.Add(null);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000C6FC File Offset: 0x0000A8FC
	public virtual void AddItem(string text, object data)
	{
		this.items.Add(text);
		this.itemData.Add(data);
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000C718 File Offset: 0x0000A918
	public virtual void RemoveItem(string text)
	{
		int num = this.items.IndexOf(text);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000C750 File Offset: 0x0000A950
	public virtual void RemoveItemByData(object data)
	{
		int num = this.itemData.IndexOf(data);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000C788 File Offset: 0x0000A988
	protected void TriggerCallbacks()
	{
		if (!this.mExecuting)
		{
			this.mExecuting = true;
			UIPopupList uipopupList = UIPopupList.current;
			UIPopupList.current = this;
			if (this.mLegacyEvent != null)
			{
				this.mLegacyEvent(this.mSelectedItem);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			UIPopupList.current = uipopupList;
			this.mExecuting = false;
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000C824 File Offset: 0x0000AA24
	protected virtual void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = ((this.bitmapFont != null) ? Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale) : 16);
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000C944 File Offset: 0x0000AB44
	protected virtual void OnValidate()
	{
		Font x = this.trueTypeFont;
		UIFont uifont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (x != null && (uifont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
			return;
		}
		if (!(uifont != null))
		{
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
			return;
		}
		if (uifont.isDynamic)
		{
			this.trueTypeFont = uifont.dynamicFont;
			this.fontStyle = uifont.dynamicFontStyle;
			this.fontSize = uifont.defaultSize;
			this.mUseDynamicFont = true;
			return;
		}
		this.bitmapFont = uifont;
		this.mUseDynamicFont = false;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
	public virtual void Start()
	{
		if (this.mStarted)
		{
			return;
		}
		this.mStarted = true;
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000CA47 File Offset: 0x0000AC47
	protected virtual void OnLocalize()
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000CA58 File Offset: 0x0000AC58
	protected virtual void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			this.mHighlightedLabel = lbl;
			Vector3 highlightPosition = this.GetHighlightPosition();
			if (!instant && this.isAnimated)
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
				if (!this.mTweening)
				{
					this.mTweening = true;
					base.StartCoroutine("UpdateTweenPosition");
					return;
				}
			}
			else
			{
				this.mHighlight.cachedTransform.localPosition = highlightPosition;
			}
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000CAD8 File Offset: 0x0000ACD8
	protected virtual Vector3 GetHighlightPosition()
	{
		if (this.mHighlightedLabel == null || this.mHighlight == null)
		{
			return Vector3.zero;
		}
		Vector4 border = this.mHighlight.border;
		float num = (this.atlas != null) ? this.atlas.pixelSize : 1f;
		float num2 = border.x * num;
		float y = border.w * num;
		return this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-num2, y, 1f);
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000CB66 File Offset: 0x0000AD66
	protected virtual IEnumerator UpdateTweenPosition()
	{
		if (this.mHighlight != null && this.mHighlightedLabel != null)
		{
			TweenPosition tp = this.mHighlight.GetComponent<TweenPosition>();
			while (tp != null && tp.enabled)
			{
				tp.to = this.GetHighlightPosition();
				yield return null;
			}
			tp = null;
		}
		this.mTweening = false;
		yield break;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000CB78 File Offset: 0x0000AD78
	protected virtual void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			this.Highlight(component, false);
		}
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000CB98 File Offset: 0x0000AD98
	protected virtual void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
			UIEventListener component = go.GetComponent<UIEventListener>();
			this.value = (component.parameter as string);
			UIPlaySound[] components = base.GetComponents<UIPlaySound>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				UIPlaySound uiplaySound = components[i];
				if (uiplaySound.trigger == UIPlaySound.Trigger.OnClick)
				{
					NGUITools.PlaySound(uiplaySound.audioClip, uiplaySound.volume, 1f);
				}
				i++;
			}
			this.CloseSelf();
			NetworkSingleton<NetworkUI>.Instance.GetComponent<SoundScript>().PlayGUISound(NetworkSingleton<NetworkUI>.Instance.ButtonSound, 0.3f, 1f);
		}
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000CC37 File Offset: 0x0000AE37
	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000CC44 File Offset: 0x0000AE44
	protected virtual void OnNavigate(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (num == -1)
			{
				num = 0;
			}
			if (key == KeyCode.UpArrow)
			{
				if (num > 0)
				{
					this.Select(this.mLabelList[num - 1], false);
					return;
				}
			}
			else if (key == KeyCode.DownArrow && num + 1 < this.mLabelList.Count)
			{
				this.Select(this.mLabelList[num + 1], false);
			}
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000CCD1 File Offset: 0x0000AED1
	protected virtual void OnKey(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this && (key == UICamera.current.cancelKey0 || key == UICamera.current.cancelKey1))
		{
			this.OnSelect(false);
		}
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000CD09 File Offset: 0x0000AF09
	protected virtual void OnDisable()
	{
		this.CloseSelf();
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000CD11 File Offset: 0x0000AF11
	protected virtual void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.CloseSelf();
		}
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000CD1C File Offset: 0x0000AF1C
	public static void Close()
	{
		if (UIPopupList.current != null)
		{
			UIPopupList.current.CloseSelf();
			UIPopupList.current = null;
		}
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000CD3C File Offset: 0x0000AF3C
	public virtual void CloseSelf()
	{
		if (UIPopupList.mChild != null && UIPopupList.current == this)
		{
			base.StopCoroutine("CloseIfUnselected");
			this.mSelection = null;
			this.mLabelList.Clear();
			if (this.isAnimated)
			{
				UIWidget[] componentsInChildren = UIPopupList.mChild.GetComponentsInChildren<UIWidget>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					UIWidget uiwidget = componentsInChildren[i];
					Color color = uiwidget.color;
					color.a = 0f;
					TweenColor.Begin(uiwidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					i++;
				}
				Collider[] componentsInChildren2 = UIPopupList.mChild.GetComponentsInChildren<Collider>();
				int j = 0;
				int num2 = componentsInChildren2.Length;
				while (j < num2)
				{
					componentsInChildren2[j].enabled = false;
					j++;
				}
				UnityEngine.Object.Destroy(UIPopupList.mChild, 0.15f);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + Mathf.Max(0.1f, 0.15f);
			}
			else
			{
				UnityEngine.Object.Destroy(UIPopupList.mChild);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + 0.1f;
			}
			this.mBackground = null;
			this.mHighlight = null;
			UIPopupList.mChild = null;
			UIPopupList.current = null;
		}
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000CE64 File Offset: 0x0000B064
	protected virtual void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000CEB4 File Offset: 0x0000B0B4
	protected virtual void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = placeAbove ? new Vector3(localPosition.x, bottom, localPosition.z) : new Vector3(localPosition.x, 0f, localPosition.z);
		widget.cachedTransform.localPosition = localPosition2;
		TweenPosition.Begin(widget.gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000CF20 File Offset: 0x0000B120
	protected virtual void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000CFCE File Offset: 0x0000B1CE
	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
	protected virtual void OnClick()
	{
		if (this.mOpenFrame == Time.frameCount)
		{
			return;
		}
		if (UIPopupList.mChild == null)
		{
			if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual)
			{
				return;
			}
			if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
			{
				return;
			}
			this.Show();
			return;
		}
		else
		{
			if (this.autoCloseOnClick)
			{
				this.CloseSelf();
				return;
			}
			if (this.mHighlightedLabel != null)
			{
				this.OnItemPress(this.mHighlightedLabel.gameObject, true);
			}
			return;
		}
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000D064 File Offset: 0x0000B264
	protected virtual void OnDoubleClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick)
		{
			this.Show();
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000D075 File Offset: 0x0000B275
	private IEnumerator CloseIfUnselected()
	{
		do
		{
			yield return null;
		}
		while (!(UICamera.selectedObject != this.mSelection));
		this.CloseSelf();
		yield break;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000D084 File Offset: 0x0000B284
	public virtual void Show()
	{
		if (!this.BeenInit)
		{
			Singleton<UIPalette>.Instance.InitTheme(this, null);
			Wait.Frames(new Action(this.Show), 2);
			return;
		}
		Singleton<UIPalette>.Instance.SetColours(this, Singleton<UIPalette>.Instance.CurrentThemeColours, true);
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !(UIPopupList.mChild == null) || !this.isValid || this.items.Count <= 0)
		{
			this.OnSelect(false);
			return;
		}
		this.mLabelList.Clear();
		base.StopCoroutine("CloseIfUnselected");
		UICamera.selectedObject = (UICamera.hoveredObject ?? base.gameObject);
		this.mSelection = UICamera.selectedObject;
		this.source = UICamera.selectedObject;
		if (this.source == null)
		{
			Debug.LogError("Popup list needs a source object...");
			return;
		}
		this.mOpenFrame = Time.frameCount;
		if (this.mPanel == null)
		{
			this.mPanel = UIPanel.Find(base.transform);
			if (this.mPanel == null)
			{
				return;
			}
		}
		UIPopupList.mChild = new GameObject("Drop-down List");
		UIPopupList.mChild.layer = base.gameObject.layer;
		if (this.separatePanel)
		{
			if (base.GetComponent<Collider>() != null)
			{
				UIPopupList.mChild.AddComponent<Rigidbody>().isKinematic = true;
			}
			else if (base.GetComponent<Collider2D>() != null)
			{
				UIPopupList.mChild.AddComponent<Rigidbody2D>().isKinematic = true;
			}
			NGUIHelper.BringToFront(UIPopupList.mChild.AddComponent<UIPanel>());
		}
		UIPopupList.current = this;
		Transform transform = UIPopupList.mChild.transform;
		transform.parent = this.mPanel.cachedTransform;
		Vector3 vector;
		Vector3 vector2;
		if (this.openOn == UIPopupList.OpenOn.Manual && this.mSelection != base.gameObject)
		{
			this.startingPosition = UICamera.lastEventPosition;
			vector = this.mPanel.cachedTransform.InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(this.startingPosition));
			vector2 = vector;
			transform.localPosition = vector;
			this.startingPosition = transform.position;
		}
		else
		{
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, base.transform, false, false);
			vector = bounds.min;
			vector2 = bounds.max;
			transform.localPosition = vector;
			this.startingPosition = transform.position;
		}
		base.StartCoroutine("CloseIfUnselected");
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		int num = this.separatePanel ? 0 : NGUITools.CalculateNextDepth(this.mPanel.gameObject);
		if (this.background2DSprite != null)
		{
			UI2DSprite ui2DSprite = UIPopupList.mChild.AddWidget(num);
			ui2DSprite.sprite2D = this.background2DSprite;
			this.mBackground = ui2DSprite;
		}
		else
		{
			if (!(this.atlas != null))
			{
				return;
			}
			this.mBackground = UIPopupList.mChild.AddSprite(this.atlas, this.backgroundSprite, num);
		}
		this.mBackground.pivot = UIWidget.Pivot.TopLeft;
		this.mBackground.color = this.backgroundColor;
		Vector4 border = this.mBackground.border;
		this.mBgBorder = border.y;
		this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
		if (this.highlight2DSprite != null)
		{
			UI2DSprite ui2DSprite2 = UIPopupList.mChild.AddWidget(num + 1);
			ui2DSprite2.sprite2D = this.highlight2DSprite;
			this.mHighlight = ui2DSprite2;
		}
		else
		{
			if (!(this.atlas != null))
			{
				return;
			}
			this.mHighlight = UIPopupList.mChild.AddSprite(this.atlas, this.highlightSprite, num + 1);
		}
		float num2 = 0f;
		float num3 = 0f;
		if (this.mHighlight.hasBorder)
		{
			num2 = this.mHighlight.border.w;
			num3 = this.mHighlight.border.x;
		}
		this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
		this.mHighlight.color = this.highlightColor;
		float num4 = (float)this.activeFontSize;
		float activeFontScale = this.activeFontScale;
		float num5 = num4 * activeFontScale;
		float num6 = 0f;
		float num7 = -this.padding.y;
		List<UILabel> list = new List<UILabel>();
		if (!this.items.Contains(this.mSelectedItem))
		{
			this.mSelectedItem = null;
		}
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			string text = this.items[i];
			UILabel uilabel = UIPopupList.mChild.AddWidget(this.mBackground.depth + 2);
			uilabel.name = i.ToString();
			uilabel.pivot = UIWidget.Pivot.TopLeft;
			uilabel.bitmapFont = this.bitmapFont;
			uilabel.trueTypeFont = this.trueTypeFont;
			uilabel.fontSize = this.fontSize;
			uilabel.fontStyle = this.fontStyle;
			uilabel.text = Language.Translate(text);
			uilabel.color = this.textColor;
			uilabel.cachedTransform.localPosition = new Vector3(border.x + this.padding.x - uilabel.pivotOffset.x, num7, -1f);
			uilabel.overflowMethod = UILabel.Overflow.ResizeFreely;
			uilabel.alignment = this.alignment;
			list.Add(uilabel);
			num7 -= num5;
			num7 -= this.padding.y;
			num6 = Mathf.Max(num6, uilabel.printedSize.x);
			UIEventListener uieventListener = UIEventListener.Get(uilabel.gameObject);
			uieventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
			uieventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
			uieventListener.parameter = text;
			if (this.mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(this.mSelectedItem)))
			{
				this.Highlight(uilabel, true);
			}
			this.mLabelList.Add(uilabel);
			i++;
		}
		num6 = Mathf.Max(num6, vector2.x - vector.x - (border.x + this.padding.x) * 2f);
		float num8 = num6;
		Vector3 vector3 = new Vector3(num8 * 0.5f, -num5 * 0.5f, 0f);
		Vector3 vector4 = new Vector3(num8, num5 + this.padding.y, 1f);
		int j = 0;
		int count2 = list.Count;
		while (j < count2)
		{
			UILabel uilabel2 = list[j];
			NGUITools.AddWidgetCollider(uilabel2.gameObject);
			uilabel2.autoResizeBoxCollider = false;
			BoxCollider component = uilabel2.GetComponent<BoxCollider>();
			if (component != null)
			{
				vector3.z = component.center.z;
				component.center = vector3;
				component.size = vector4;
			}
			else
			{
				BoxCollider2D component2 = uilabel2.GetComponent<BoxCollider2D>();
				component2.offset = vector3;
				component2.size = vector4;
			}
			j++;
		}
		int width = Mathf.RoundToInt(num6);
		num6 += (border.x + this.padding.x) * 2f;
		num7 -= border.y;
		this.mBackground.width = Mathf.RoundToInt(num6);
		this.mBackground.height = Mathf.RoundToInt(-num7 + border.y);
		int k = 0;
		int count3 = list.Count;
		while (k < count3)
		{
			UILabel uilabel3 = list[k];
			uilabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
			uilabel3.width = width;
			k++;
		}
		float num9 = (this.atlas != null) ? (2f * this.atlas.pixelSize) : 2f;
		float f = num6 - (border.x + this.padding.x) * 2f + num3 * num9;
		float f2 = num5 + num2 * num9;
		this.mHighlight.width = Mathf.RoundToInt(f);
		this.mHighlight.height = Mathf.RoundToInt(f2);
		bool flag = this.position == UIPopupList.Position.Above;
		if (this.position == UIPopupList.Position.Auto)
		{
			UICamera uicamera = UICamera.FindCameraForLayer(this.mSelection.layer);
			if (uicamera != null)
			{
				flag = (uicamera.cachedCamera.WorldToViewportPoint(this.startingPosition).y < 0.5f);
			}
		}
		if (this.isAnimated)
		{
			this.AnimateColor(this.mBackground);
			if (Time.timeScale == 0f || Time.timeScale >= 0.1f)
			{
				float bottom = num7 + num5;
				this.Animate(this.mHighlight, flag, bottom);
				int l = 0;
				int count4 = list.Count;
				while (l < count4)
				{
					this.Animate(list[l], flag, bottom);
					l++;
				}
				this.AnimateScale(this.mBackground, flag, bottom);
			}
		}
		if (flag)
		{
			vector.y = vector2.y - border.y;
			vector2.y = vector.y + (float)this.mBackground.height;
			vector2.x = vector.x + (float)this.mBackground.width;
			transform.localPosition = new Vector3(vector.x, vector2.y - border.y, vector.z);
		}
		else
		{
			vector2.y = vector.y + border.y;
			vector.y = vector2.y - (float)this.mBackground.height;
			vector2.x = vector.x + (float)this.mBackground.width;
		}
		Transform parent = this.mPanel.cachedTransform.parent;
		if (parent != null)
		{
			vector = this.mPanel.cachedTransform.TransformPoint(vector);
			vector2 = this.mPanel.cachedTransform.TransformPoint(vector2);
			vector = parent.InverseTransformPoint(vector);
			vector2 = parent.InverseTransformPoint(vector2);
		}
		Vector3 b = Vector3.zero;
		if (this.applyPositionOffset && !this.mPanel.hasClipping)
		{
			b = this.mPanel.CalculateConstrainOffset(vector, vector2);
		}
		Vector3 vector5 = transform.localPosition + b;
		vector5.x = Mathf.Round(vector5.x);
		vector5.y = Mathf.Round(vector5.y);
		transform.localPosition = vector5;
	}

	// Token: 0x040001B0 RID: 432
	public static UIPopupList current;

	// Token: 0x040001B1 RID: 433
	private static GameObject mChild;

	// Token: 0x040001B2 RID: 434
	private static float mFadeOutComplete;

	// Token: 0x040001B3 RID: 435
	public UIPalette.UI ThemeBackgroundAsSetting;

	// Token: 0x040001B4 RID: 436
	public UIPalette.UI ThemeBackgroundAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040001B5 RID: 437
	public UIPalette.UI ThemeHighlightAsSetting;

	// Token: 0x040001B6 RID: 438
	public UIPalette.UI ThemeHighlightAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040001B7 RID: 439
	public UIPalette.UI ThemeLabelAsSetting;

	// Token: 0x040001B8 RID: 440
	public UIPalette.UI ThemeLabelAs = UIPalette.UI.DoNotTheme;

	// Token: 0x040001B9 RID: 441
	[NonSerialized]
	public bool BeenInit;

	// Token: 0x040001BA RID: 442
	private const float animSpeed = 0.15f;

	// Token: 0x040001BB RID: 443
	public UIAtlas atlas;

	// Token: 0x040001BC RID: 444
	public UIFont bitmapFont;

	// Token: 0x040001BD RID: 445
	public Font trueTypeFont;

	// Token: 0x040001BE RID: 446
	public int fontSize = 16;

	// Token: 0x040001BF RID: 447
	public FontStyle fontStyle;

	// Token: 0x040001C0 RID: 448
	public string backgroundSprite;

	// Token: 0x040001C1 RID: 449
	public string highlightSprite;

	// Token: 0x040001C2 RID: 450
	public Sprite background2DSprite;

	// Token: 0x040001C3 RID: 451
	public Sprite highlight2DSprite;

	// Token: 0x040001C4 RID: 452
	public UIPopupList.Position position;

	// Token: 0x040001C5 RID: 453
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x040001C6 RID: 454
	public List<string> items = new List<string>();

	// Token: 0x040001C7 RID: 455
	public List<object> itemData = new List<object>();

	// Token: 0x040001C8 RID: 456
	public Vector2 padding = new Vector3(4f, 4f);

	// Token: 0x040001C9 RID: 457
	public Color textColor = Color.white;

	// Token: 0x040001CA RID: 458
	public Color backgroundColor = Color.white;

	// Token: 0x040001CB RID: 459
	public Color highlightColor = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x040001CC RID: 460
	public bool isAnimated = true;

	// Token: 0x040001CD RID: 461
	public bool isLocalized;

	// Token: 0x040001CE RID: 462
	public bool separatePanel = true;

	// Token: 0x040001CF RID: 463
	public bool autoCloseOnClick = true;

	// Token: 0x040001D0 RID: 464
	public bool applyPositionOffset;

	// Token: 0x040001D1 RID: 465
	public UIPopupList.OpenOn openOn;

	// Token: 0x040001D2 RID: 466
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x040001D3 RID: 467
	[HideInInspector]
	[SerializeField]
	protected string mSelectedItem;

	// Token: 0x040001D4 RID: 468
	[HideInInspector]
	[SerializeField]
	protected UIPanel mPanel;

	// Token: 0x040001D5 RID: 469
	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite mBackground;

	// Token: 0x040001D6 RID: 470
	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite mHighlight;

	// Token: 0x040001D7 RID: 471
	[HideInInspector]
	[SerializeField]
	protected UILabel mHighlightedLabel;

	// Token: 0x040001D8 RID: 472
	[HideInInspector]
	[SerializeField]
	protected List<UILabel> mLabelList = new List<UILabel>();

	// Token: 0x040001D9 RID: 473
	[HideInInspector]
	[SerializeField]
	protected float mBgBorder;

	// Token: 0x040001DA RID: 474
	[NonSerialized]
	protected GameObject mSelection;

	// Token: 0x040001DB RID: 475
	[NonSerialized]
	protected int mOpenFrame;

	// Token: 0x040001DC RID: 476
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x040001DD RID: 477
	[HideInInspector]
	[SerializeField]
	private string functionName = "OnSelectionChange";

	// Token: 0x040001DE RID: 478
	[HideInInspector]
	[SerializeField]
	private float textScale;

	// Token: 0x040001DF RID: 479
	[HideInInspector]
	[SerializeField]
	private UIFont font;

	// Token: 0x040001E0 RID: 480
	[HideInInspector]
	[SerializeField]
	private UILabel textLabel;

	// Token: 0x040001E1 RID: 481
	[NonSerialized]
	public Vector3 startingPosition;

	// Token: 0x040001E2 RID: 482
	private UIPopupList.LegacyEvent mLegacyEvent;

	// Token: 0x040001E3 RID: 483
	[NonSerialized]
	protected bool mExecuting;

	// Token: 0x040001E4 RID: 484
	protected bool mUseDynamicFont;

	// Token: 0x040001E5 RID: 485
	[NonSerialized]
	protected bool mStarted;

	// Token: 0x040001E6 RID: 486
	protected bool mTweening;

	// Token: 0x040001E7 RID: 487
	public GameObject source;

	// Token: 0x02000510 RID: 1296
	public enum Position
	{
		// Token: 0x040023CB RID: 9163
		Auto,
		// Token: 0x040023CC RID: 9164
		Above,
		// Token: 0x040023CD RID: 9165
		Below
	}

	// Token: 0x02000511 RID: 1297
	public enum OpenOn
	{
		// Token: 0x040023CF RID: 9167
		ClickOrTap,
		// Token: 0x040023D0 RID: 9168
		RightClick,
		// Token: 0x040023D1 RID: 9169
		DoubleClick,
		// Token: 0x040023D2 RID: 9170
		Manual
	}

	// Token: 0x02000512 RID: 1298
	// (Invoke) Token: 0x0600374F RID: 14159
	public delegate void LegacyEvent(string val);
}
