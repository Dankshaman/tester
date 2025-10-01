using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class UIPDFPopout : Singleton<UIPDFPopout>
{
	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x060025F2 RID: 9714 RVA: 0x0010B993 File Offset: 0x00109B93
	// (set) Token: 0x060025F3 RID: 9715 RVA: 0x0010B99A File Offset: 0x00109B9A
	public static Colour HIGHLIGHT_COLOUR
	{
		get
		{
			return UIPDFPopout._highlightColour;
		}
		set
		{
			UIPDFPopout._highlightColour = value;
			Singleton<UIPDFPopout>.Instance.highlighter.color = value;
		}
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x0010B9B8 File Offset: 0x00109BB8
	protected override void Awake()
	{
		base.Awake();
		this.OnThemeChange();
		this.Pages.PageRight += this.PageRight;
		this.Pages.PageLeft += this.PageLeft;
		this.Pages.PageBigRight += this.PageForward;
		this.Pages.PageBigLeft += this.PageBack;
		this.Pages.SetPage += this.SetPage;
		EventManager.OnUIThemeChange += this.OnThemeChange;
		UIEventListener uieventListener = UIEventListener.Get(this.PDFTexture.gameObject);
		uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Combine(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScrollCollider));
		UIEventListener uieventListener2 = UIEventListener.Get(this.PDFTexture.gameObject);
		uieventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener2.onDrag, new UIEventListener.VectorDelegate(this.OnDrag));
		UIEventListener uieventListener3 = UIEventListener.Get(this.PDFTexture.gameObject);
		uieventListener3.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener3.onDragEnd, new UIEventListener.VoidDelegate(this.OnDragEnd));
		this.highlighter.color = UIPDFPopout.HIGHLIGHT_COLOUR;
		this.panelObjects.Add(base.gameObject);
		foreach (Transform transform in base.transform.GetComponentsInChildren<Transform>())
		{
			this.panelObjects.Add(transform.gameObject);
		}
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x0010BB40 File Offset: 0x00109D40
	private void OnDestroy()
	{
		this.Pages.PageRight -= this.PageRight;
		this.Pages.PageLeft -= this.PageLeft;
		this.Pages.PageBigRight -= this.PageForward;
		this.Pages.PageBigLeft -= this.PageBack;
		this.Pages.SetPage -= this.SetPage;
		EventManager.OnUIThemeChange -= this.OnThemeChange;
		UIEventListener uieventListener = UIEventListener.Get(this.PDFTexture.gameObject);
		uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Remove(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScrollCollider));
		UIEventListener uieventListener2 = UIEventListener.Get(this.PDFTexture.gameObject);
		uieventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener2.onDrag, new UIEventListener.VectorDelegate(this.OnDrag));
		UIEventListener uieventListener3 = UIEventListener.Get(this.PDFTexture.gameObject);
		uieventListener3.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener3.onDragEnd, new UIEventListener.VoidDelegate(this.OnDragEnd));
	}

	// Token: 0x060025F6 RID: 9718 RVA: 0x0010BC64 File Offset: 0x00109E64
	private void OnThemeChange()
	{
		float a = this.panelOutColour.a;
		this.panelInColour = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.WindowBackground];
		this.panelOutColour = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.WindowBackground];
		this.panelOutColour.a = a;
	}

	// Token: 0x060025F7 RID: 9719 RVA: 0x0010BCB7 File Offset: 0x00109EB7
	private void PageRight()
	{
		this.targetPDFObject.NextPage();
	}

	// Token: 0x060025F8 RID: 9720 RVA: 0x0010BCC4 File Offset: 0x00109EC4
	private void PageLeft()
	{
		this.targetPDFObject.PrevPage();
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x0010BCD1 File Offset: 0x00109ED1
	private void PageForward()
	{
		this.targetPDFObject.ForwardPages();
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x0010BCDE File Offset: 0x00109EDE
	private void PageBack()
	{
		this.targetPDFObject.BackPages();
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x0010BCEB File Offset: 0x00109EEB
	private void SetPage(int page)
	{
		this.targetPDFObject.CurrentPDFPage = page - 1;
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x0010BCFB File Offset: 0x00109EFB
	public void OnScrollCollider(GameObject go, float delta)
	{
		this.OnScroll(delta);
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x0010BD04 File Offset: 0x00109F04
	public void OnScroll(float delta)
	{
		if (Time.time - this.lastScrollTime < 0.05f)
		{
			return;
		}
		this.lastScrollTime = Time.time;
		if (delta > 0f)
		{
			this.PageLeft();
			return;
		}
		this.PageRight();
	}

	// Token: 0x060025FE RID: 9726 RVA: 0x0010BD3C File Offset: 0x00109F3C
	public void Init(CustomPDF customPDFObject)
	{
		this.targetPDFObject = customPDFObject;
		this.targetMaterial = customPDFObject.GetComponent<Renderer>().sharedMaterials[1];
		this.backgroundPanel.color = Colour.Clear;
		this.pdfTexture.color = Colour.Clear;
		this.fadeState = UIPDFPopout.FadeState.Out;
		Wait.Frames(new Action(this.MakeVisible), 1);
		this.SetPanelOutAlpha(UIPDFPopout.PANEL_ALPHA);
		this.SetPDFOutAlpha(UIPDFPopout.PDF_ALPHA);
		for (int i = 0; i < this.searchControls.Length; i++)
		{
			this.searchControls[i].SetActive(customPDFObject.IsSearchable);
		}
		base.gameObject.SetActive(true);
		this.UpdatePage();
	}

	// Token: 0x060025FF RID: 9727 RVA: 0x0010BDF8 File Offset: 0x00109FF8
	public void MakeVisible()
	{
		this.backgroundPanel.color = Colour.White;
		this.pdfTexture.color = Colour.White;
		this.fadeState = UIPDFPopout.FadeState.FadingIn;
		this.progress = 0f;
		this.hoverPDFUntil = Time.time + 1f;
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x0010BE54 File Offset: 0x0010A054
	private void Update()
	{
		if (this.targetPDFObject == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		bool hovering = this.panelObjects.Contains(UICamera.HoveredUIObject);
		this.CheckFade(hovering);
		this.CheckKeys(hovering);
		this.PDFTexture.mainTexture = this.targetMaterial.mainTexture;
		this.PDFTexture.height = (int)((float)Screen.height * NetworkSingleton<ManagerPhysicsObject>.Instance.UIResolutionScale());
		int a = (int)((float)this.PDFTexture.height * (float)this.PDFTexture.mainTexture.width / (float)this.PDFTexture.mainTexture.height);
		this.PDFTexture.width = Mathf.Min(a, Screen.width - 100);
		if (this.targetPDFObject.HighlightBox != Vector4.zero)
		{
			if (this.targetPDFObject.HighlightBox != this.lastHighlightBox)
			{
				this.lastHighlightBox = this.targetPDFObject.HighlightBox;
				if (!this.highlighting)
				{
					this.highlighting = true;
					this.highlighter.gameObject.SetActive(true);
				}
				this.highlighter.leftAnchor.relative = this.lastHighlightBox.x;
				this.highlighter.bottomAnchor.relative = this.lastHighlightBox.y;
				this.highlighter.rightAnchor.relative = this.lastHighlightBox.z;
				this.highlighter.topAnchor.relative = this.lastHighlightBox.w;
				this.highlighter.UpdateAnchors();
			}
		}
		else if (this.highlighting)
		{
			this.lastHighlightBox = Vector4.zero;
			this.highlighter.gameObject.SetActive(false);
			this.highlighting = false;
		}
		if (this.prevPage != this.targetPDFObject.CurrentPDFPage)
		{
			this.UpdatePage();
		}
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x0010C03C File Offset: 0x0010A23C
	private void UpdatePage()
	{
		this.prevPage = this.targetPDFObject.CurrentPDFPage;
		this.Pages.PageDisplayOffset = this.targetPDFObject.PageDisplayOffset;
		this.Pages.Set(this.targetPDFObject.CurrentPDFPage + 1, this.targetPDFObject.PageCount);
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x0010C093 File Offset: 0x0010A293
	public void SetPDFOutAlpha(float alpha)
	{
		this.pdfOutColour.a = alpha;
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x0010C0A1 File Offset: 0x0010A2A1
	public void SetPanelOutAlpha(float alpha)
	{
		this.panelOutColour.a = alpha;
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x0010C0B0 File Offset: 0x0010A2B0
	private void CheckFade(bool hovering)
	{
		if (this.hoverPDFUntil > Time.time)
		{
			hovering = true;
		}
		switch (this.fadeState)
		{
		case UIPDFPopout.FadeState.Out:
			if (hovering)
			{
				this.fadeState = UIPDFPopout.FadeState.FadingIn;
				return;
			}
			break;
		case UIPDFPopout.FadeState.FadingIn:
			if (!hovering)
			{
				this.fadeState = UIPDFPopout.FadeState.FadingOut;
				return;
			}
			this.progress += Time.deltaTime / 0.2f;
			if (this.progress >= 1f)
			{
				this.progress = 1f;
				this.fadeState = UIPDFPopout.FadeState.In;
			}
			this.UpdateOpacity(this.progress);
			return;
		case UIPDFPopout.FadeState.FadingOut:
			if (hovering)
			{
				this.fadeState = UIPDFPopout.FadeState.FadingIn;
				return;
			}
			this.progress -= Time.deltaTime / 0.2f;
			if (this.progress <= 0f)
			{
				this.progress = 0f;
				this.fadeState = UIPDFPopout.FadeState.Out;
			}
			this.UpdateOpacity(this.progress);
			break;
		case UIPDFPopout.FadeState.In:
			if (!hovering)
			{
				this.fadeState = UIPDFPopout.FadeState.FadingOut;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x0010C1A4 File Offset: 0x0010A3A4
	private void CheckKeys(bool hovering)
	{
		if (hovering && !UICamera.SelectIsInput() && (this.holderNPO == null || PlayerScript.PointerScript.GetHoverLockID() != this.holderNPO.ID))
		{
			if (!zInput.GetButton("Shift", ControlType.All) && !zInput.GetButton("Ctrl", ControlType.All))
			{
				if (zInput.GetButtonDown("Next State", ControlType.All))
				{
					this.PageRight();
				}
				if (zInput.GetButtonDown("Previous State", ControlType.All))
				{
					this.PageLeft();
				}
				for (int i = 0; i < 10; i++)
				{
					if (TTSInput.GetKeyDown(i.ToString()))
					{
						if (this.holderint == -1)
						{
							this.holderint = i;
							this.holderNPO = this.targetPDFObject.GetComponent<NetworkPhysicsObject>();
							this.holdermaxdigits = 1 + (int)Mathf.Log10((float)this.holderNPO.MaxTypedNumber());
						}
						else
						{
							this.holderint = this.holderint * 10 + i;
						}
						this.holderstring += i.ToString();
						if (this.holderstring.Length >= this.holdermaxdigits)
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.HandleTypedNumberWrapper(this.holderNPO, NetworkID.ID, this.holderint);
							this.holderint = -1;
							this.holderstring = "";
							this.holderNPO = null;
						}
						else
						{
							this.holderinttime = Time.time;
						}
					}
				}
			}
			else
			{
				if (zInput.GetButtonDown("Next State", ControlType.All))
				{
					this.PageForward();
				}
				if (zInput.GetButtonDown("Previous State", ControlType.All))
				{
					this.PageBack();
				}
			}
		}
		if (this.holderint != -1 && Time.time > this.holderinttime + 1f)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.HandleTypedNumberWrapper(this.holderNPO, NetworkID.ID, this.holderint);
			this.holderint = -1;
			this.holderstring = "";
			this.holderNPO = null;
		}
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x0010C388 File Offset: 0x0010A588
	private void UpdateOpacity(float opacity)
	{
		this.backgroundPanel.color = Colour.Lerp(this.panelOutColour, this.panelInColour, opacity * opacity);
		this.pdfTexture.color = Colour.Lerp(this.pdfOutColour, this.pdfInColour, opacity);
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x0010C3DC File Offset: 0x0010A5DC
	private void OnDrag(GameObject go, Vector2 delta)
	{
		if (!this.dragging)
		{
			this.dragging = true;
			this.PointOnPDFTexture(UICamera.currentTouch.pos, out this.dragStart);
		}
		Vector2 vector;
		if (this.PointOnPDFTexture(UICamera.currentTouch.pos, out vector))
		{
			this.dragEnd = vector;
		}
		else
		{
			this.dragEnd = Vector2.negativeInfinity;
		}
		this.targetPDFObject.HighlightBox = LibVector.Vector4FromPairs(this.dragStart.x, this.dragStart.y, this.dragEnd.x, this.dragEnd.y);
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x0010C474 File Offset: 0x0010A674
	private void OnDragEnd(GameObject go)
	{
		this.dragging = false;
		if (this.dragEnd.x >= 0f && this.dragEnd.y >= 0f)
		{
			Vector4 vector = LibVector.Vector4FromPairs(this.dragStart.x, this.dragStart.y, this.dragEnd.x, this.dragEnd.y);
			if (vector.z - vector.x > 0.03f && vector.w - vector.y > 0.03f)
			{
				this.targetPDFObject.HighlightBox = vector;
				return;
			}
		}
		this.targetPDFObject.HighlightBox = Vector4.zero;
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x0010C524 File Offset: 0x0010A724
	private bool PointOnPDFTexture(Vector2 point, out Vector2 position)
	{
		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.pdfTexture.transform);
		Vector3 vector = UICamera.mainCamera.WorldToScreenPoint(bounds.min);
		Vector3 vector2 = UICamera.mainCamera.WorldToScreenPoint(bounds.max);
		int num = (int)(vector2.x - vector.x);
		int num2 = (int)(vector2.y - vector.y);
		Rect rect = new Rect(vector.x, vector.y, (float)num, (float)num2);
		if (rect.Contains(point))
		{
			float x = (point.x - vector.x) / (float)num;
			float y = (point.y - vector.y) / (float)num2;
			position = new Vector2(x, y);
			return true;
		}
		position = Vector2.zero;
		return false;
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x0010C5E4 File Offset: 0x0010A7E4
	public void PrevSearch()
	{
		this.targetPDFObject.SearchPrev();
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x0010C5F1 File Offset: 0x0010A7F1
	public void NextSearch()
	{
		this.targetPDFObject.SearchNext();
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x0010C5FE File Offset: 0x0010A7FE
	public void Search()
	{
		this.targetPDFObject.Search();
	}

	// Token: 0x0400189C RID: 6300
	public static float PANEL_ALPHA = 0f;

	// Token: 0x0400189D RID: 6301
	public static float PDF_ALPHA = 0.9f;

	// Token: 0x0400189E RID: 6302
	private static Colour _highlightColour = new Colour(byte.MaxValue, 192, 0, 168);

	// Token: 0x0400189F RID: 6303
	private const float FADE_DURATION = 0.2f;

	// Token: 0x040018A0 RID: 6304
	[SerializeField]
	private UIPages Pages;

	// Token: 0x040018A1 RID: 6305
	[SerializeField]
	private UITexture PDFTexture;

	// Token: 0x040018A2 RID: 6306
	public UISprite backgroundPanel;

	// Token: 0x040018A3 RID: 6307
	public UITexture pdfTexture;

	// Token: 0x040018A4 RID: 6308
	public UISprite highlighter;

	// Token: 0x040018A5 RID: 6309
	public GameObject[] searchControls;

	// Token: 0x040018A6 RID: 6310
	public UIPDFPopout.FadeState fadeState;

	// Token: 0x040018A7 RID: 6311
	private float progress;

	// Token: 0x040018A8 RID: 6312
	private float hoverPDFUntil;

	// Token: 0x040018A9 RID: 6313
	private Colour panelInColour = Colour.White;

	// Token: 0x040018AA RID: 6314
	private Colour panelOutColour = Colour.White;

	// Token: 0x040018AB RID: 6315
	private Colour pdfInColour = Colour.White;

	// Token: 0x040018AC RID: 6316
	private Colour pdfOutColour = Colour.White;

	// Token: 0x040018AD RID: 6317
	private CustomPDF targetPDFObject;

	// Token: 0x040018AE RID: 6318
	private Material targetMaterial;

	// Token: 0x040018AF RID: 6319
	private List<GameObject> panelObjects = new List<GameObject>();

	// Token: 0x040018B0 RID: 6320
	private bool dragging;

	// Token: 0x040018B1 RID: 6321
	private bool highlighting;

	// Token: 0x040018B2 RID: 6322
	private Vector4 lastHighlightBox = Vector4.zero;

	// Token: 0x040018B3 RID: 6323
	private Vector2 dragStart;

	// Token: 0x040018B4 RID: 6324
	private Vector2 dragEnd;

	// Token: 0x040018B5 RID: 6325
	private float lastScrollTime;

	// Token: 0x040018B6 RID: 6326
	private int prevPage = -1;

	// Token: 0x040018B7 RID: 6327
	private int holderint = -1;

	// Token: 0x040018B8 RID: 6328
	private float holderinttime;

	// Token: 0x040018B9 RID: 6329
	private int holdermaxdigits;

	// Token: 0x040018BA RID: 6330
	private string holderstring = "";

	// Token: 0x040018BB RID: 6331
	private NetworkPhysicsObject holderNPO;

	// Token: 0x02000773 RID: 1907
	public enum FadeState
	{
		// Token: 0x04002BF9 RID: 11257
		Out,
		// Token: 0x04002BFA RID: 11258
		FadingIn,
		// Token: 0x04002BFB RID: 11259
		FadingOut,
		// Token: 0x04002BFC RID: 11260
		In
	}
}
