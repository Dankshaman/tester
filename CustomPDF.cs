using System;
using System.Collections.Generic;
using NewNet;
using Paroxe.PdfRenderer;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class CustomPDF : CustomObject
{
	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0004F23B File Offset: 0x0004D43B
	// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0004F243 File Offset: 0x0004D443
	public int PageCount { get; private set; }

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0004F24C File Offset: 0x0004D44C
	// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0004F254 File Offset: 0x0004D454
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public int CurrentPDFPage
	{
		get
		{
			return this._page;
		}
		set
		{
			if (value == this._page)
			{
				return;
			}
			this._page = value;
			this._highlightBox = Vector4.zero;
			if (this.customPDFContainer != null)
			{
				this.UpdatePDF();
			}
			base.DirtySync("CurrentPDFPage");
			if (Network.isServer && base.NPO)
			{
				EventManager.TriggerObjectPageChange(base.NPO);
			}
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0004F2B5 File Offset: 0x0004D4B5
	// (set) Token: 0x06000B6A RID: 2922 RVA: 0x0004F2C0 File Offset: 0x0004D4C0
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public int PageDisplayOffset
	{
		get
		{
			return this._pageDisplayOffset;
		}
		set
		{
			if (value == this._pageDisplayOffset)
			{
				return;
			}
			this._pageDisplayOffset = value;
			if (this.customPDFContainer != null)
			{
				this.UpdatePDF();
				if (Singleton<UIPDFPopout>.Instance.gameObject.activeInHierarchy)
				{
					Singleton<UIPDFPopout>.Instance.gameObject.SetActive(false);
					Wait.Frames(delegate
					{
						Singleton<UIPDFPopout>.Instance.Init(this);
					}, 1);
				}
			}
			base.DirtySync("PageDisplayOffset");
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0004F32B File Offset: 0x0004D52B
	// (set) Token: 0x06000B6C RID: 2924 RVA: 0x0004F333 File Offset: 0x0004D533
	[Sync(Permission.Client, null, SerializationMethod.Default, false)]
	public Vector4 HighlightBox
	{
		get
		{
			return this._highlightBox;
		}
		set
		{
			if (value == this._highlightBox)
			{
				return;
			}
			this._highlightBox = value;
			if (this.customPDFContainer != null)
			{
				this.UpdatePDF();
			}
			base.DirtySync("HighlightBox");
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0004F364 File Offset: 0x0004D564
	// (set) Token: 0x06000B6E RID: 2926 RVA: 0x0004F36C File Offset: 0x0004D56C
	public bool IsSearchable { get; private set; }

	// Token: 0x06000B6F RID: 2927 RVA: 0x0004F375 File Offset: 0x0004D575
	public bool HasPage(int page)
	{
		return page >= 0 && page <= this.PageCount - 1;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0004F38B File Offset: 0x0004D58B
	public bool HasPrevPage()
	{
		return this.CurrentPDFPage > 0;
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0004F398 File Offset: 0x0004D598
	public void PrevPage()
	{
		if (this.HasPrevPage() && this.lastPageTurn != Time.frameCount)
		{
			int currentPDFPage = this.CurrentPDFPage;
			this.CurrentPDFPage = currentPDFPage - 1;
			this.lastPageTurn = Time.frameCount;
		}
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x0004F3D5 File Offset: 0x0004D5D5
	public bool HasNextPage()
	{
		return this.CurrentPDFPage < this.PageCount - 1;
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0004F3E8 File Offset: 0x0004D5E8
	public void NextPage()
	{
		if (this.HasNextPage() && this.lastPageTurn != Time.frameCount)
		{
			int currentPDFPage = this.CurrentPDFPage;
			this.CurrentPDFPage = currentPDFPage + 1;
			this.lastPageTurn = Time.frameCount;
		}
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0004F425 File Offset: 0x0004D625
	public void ForwardPages()
	{
		if (this.bookmarkLevel == 0)
		{
			this.CurrentPDFPage = Mathf.Min(this.CurrentPDFPage + CustomPDF.BIG_PAGE_STEP, this.PageCount - 1);
			return;
		}
		this.CurrentPDFPage = this.NextBookmark(this.CurrentPDFPage);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0004F461 File Offset: 0x0004D661
	public void BackPages()
	{
		if (this.bookmarkLevel == 0)
		{
			this.CurrentPDFPage = Mathf.Max(this.CurrentPDFPage - CustomPDF.BIG_PAGE_STEP, 0);
			return;
		}
		this.CurrentPDFPage = this.PrevBookmark(this.CurrentPDFPage);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0004F496 File Offset: 0x0004D696
	public void ClampedSetCurrentPage(int page, bool usePageOffset = false)
	{
		this.CurrentPDFPage = Mathf.Clamp(usePageOffset ? (page - this.PageDisplayOffset - 1) : page, 0, this.PageCount - 1);
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0004F4BC File Offset: 0x0004D6BC
	public void PopoutToScreen()
	{
		if (Singleton<UIPDFPopout>.Instance.gameObject.activeInHierarchy)
		{
			return;
		}
		Singleton<UIPDFPopout>.Instance.Init(this);
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0004F4DB File Offset: 0x0004D6DB
	protected override void Awake()
	{
		base.Awake();
		base.NPO.SetTypedNumberHandlers(new NetworkPhysicsObject.MaxTypedNumberMethodDelegate(CustomPDF.MaxTypedNumber), new NetworkPhysicsObject.HandleTypedNumberMethodDelegate(CustomPDF.HandleTypedNumber), true);
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0004F508 File Offset: 0x0004D708
	private void Update()
	{
		if (this.facingUp != base.transform.up.y > 0f)
		{
			this.facingUp = (base.transform.up.y > 0f);
			if (this.facingUp)
			{
				this.Root.transform.localRotation = Quaternion.identity;
			}
			else
			{
				this.Root.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
			}
		}
		if (!zInput.GetButton("Grab", ControlType.All))
		{
			this.UpdateButtonVisibility();
		}
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0004F5A8 File Offset: 0x0004D7A8
	private void UpdateButtonVisibility()
	{
		bool flag = base.gameObject != NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentlyZoomedObject;
		if (CustomPDF.ONLY_SHOW_UI_WHEN_HOVERED && PlayerScript.PointerScript && PlayerScript.PointerScript.GetHoverLockID() != base.NPO.ID)
		{
			flag = false;
		}
		if (this.PopoutButton)
		{
			this.PopoutButton.gameObject.SetActive(flag);
		}
		if (this.NextButton)
		{
			this.NextButton.gameObject.SetActive(flag && this.HasNextPage());
		}
		if (this.PrevButton)
		{
			this.PrevButton.gameObject.SetActive(flag && this.HasPrevPage());
		}
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0004F668 File Offset: 0x0004D868
	private void UpdateButtons(float aspectRatioChange)
	{
		this.UpdateButtonVisibility();
		Vector3 localPosition = this.NextButton.transform.localPosition;
		localPosition.x *= aspectRatioChange;
		this.NextButton.transform.localPosition = localPosition;
		localPosition = this.PrevButton.transform.localPosition;
		localPosition.x *= aspectRatioChange;
		this.PrevButton.transform.localPosition = localPosition;
		localPosition = this.PopoutButton.transform.localPosition;
		localPosition.x *= aspectRatioChange;
		this.PopoutButton.transform.localPosition = localPosition;
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0004F708 File Offset: 0x0004D908
	protected override void Start()
	{
		base.Start();
		if (this.DummyObject)
		{
			this.StartPDFDownload();
			return;
		}
		Vector3 vector = new Vector3(this.scale, 1f, this.scale);
		Utilities.ScaleMesh(base.GetComponent<MeshFilter>().mesh, vector, true);
		EventDelegate.Add(this.NextButton.onClick, new EventDelegate.Callback(this.NextPage));
		EventDelegate.Add(this.PrevButton.onClick, new EventDelegate.Callback(this.PrevPage));
		EventDelegate.Add(this.PopoutButton.onClick, new EventDelegate.Callback(this.PopoutToScreen));
		EventManager.OnZoomObjectChange += this.OnZoomObjectChange;
		if (Network.isServer)
		{
			if (this.CustomPDFURL != "")
			{
				this.CallCustomRPC();
				return;
			}
			this.bCustomUI = true;
		}
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0004F7E8 File Offset: 0x0004D9E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Singleton<CustomLoadingManager>.Instance)
		{
			Singleton<CustomLoadingManager>.Instance.PDF.Cleanup(this.CustomPDFURL, new Action<CustomPDFContainer>(this.OnPDFFinish), true);
		}
		EventDelegate.Remove(this.NextButton.onClick, new EventDelegate.Callback(this.NextPage));
		EventDelegate.Remove(this.PrevButton.onClick, new EventDelegate.Callback(this.PrevPage));
		EventDelegate.Remove(this.PopoutButton.onClick, new EventDelegate.Callback(this.PopoutToScreen));
		EventManager.OnZoomObjectChange -= this.OnZoomObjectChange;
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0004F891 File Offset: 0x0004DA91
	private void OnZoomObjectChange(GameObject zoomObject)
	{
		this.UpdateButtonVisibility();
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0004642E File Offset: 0x0004462E
	// (set) Token: 0x06000B80 RID: 2944 RVA: 0x0004F899 File Offset: 0x0004DA99
	public override bool bCustomUI
	{
		get
		{
			return this.bcustomUI;
		}
		set
		{
			if (value == this.bcustomUI)
			{
				return;
			}
			this.bcustomUI = value;
			if (value && Network.isServer)
			{
				Singleton<UICustomPDF>.Instance.Queue(this);
			}
		}
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0004F8C1 File Offset: 0x0004DAC1
	public override void Cancel()
	{
		if (Network.isServer && !this.bSetupOnce)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(base.gameObject);
		}
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0004F8E4 File Offset: 0x0004DAE4
	public override void CallCustomRPC()
	{
		base.CallCustomRPC();
		base.NPO.IsLocked = true;
		if (this.CustomPDFURL == "")
		{
			return;
		}
		this.OnCallCustomRPC();
		base.networkView.RPC<string, string, int>(RPCTarget.All, new Action<string, string, int>(this.SetCustomURL), this.CustomPDFURL, this.CustomPDFPassword, this.CurrentPDFPage);
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0004F948 File Offset: 0x0004DB48
	public override void CallCustomRPC(NetworkPlayer networkPlayer)
	{
		base.CallCustomRPC();
		if (this.CustomPDFURL == "")
		{
			return;
		}
		this.OnCallCustomRPC(networkPlayer);
		base.networkView.RPC<string, string, int>(networkPlayer, new Action<string, string, int>(this.SetCustomURL), this.CustomPDFURL, this.CustomPDFPassword, this.CurrentPDFPage);
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0004F99F File Offset: 0x0004DB9F
	[Remote(Permission.Admin)]
	private void SetCustomURL(string URL, string password = "", int page = 0)
	{
		this.CustomPDFURL = URL;
		if (Network.isServer)
		{
			this.CurrentPDFPage = page;
		}
		this.CustomPDFPassword = password;
		this.StartPDFDownload();
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0004F9C4 File Offset: 0x0004DBC4
	private void StartPDFDownload()
	{
		if (this.CustomPDFURL != "")
		{
			base.AddLoading();
			Singleton<CustomLoadingManager>.Instance.PDF.Load(this.CustomPDFURL, new Action<CustomPDFContainer>(this.OnPDFFinish), this.CustomPDFPassword, CustomLoadingManager.LoadType.Auto, true);
		}
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0004FA14 File Offset: 0x0004DC14
	private async void UpdatePDF()
	{
		Texture2D texture2D = await this.customPDFContainer.GetPageTexture(this.CurrentPDFPage);
		if (texture2D == null)
		{
			Chat.LogError("Failed to load page texture for PDF: " + this.CustomPDFURL, true);
			base.RemoveLoading();
		}
		else
		{
			float num = (float)texture2D.width / (float)texture2D.height;
			float num2 = num / this.prevAspectRatio;
			if (num != this.prevAspectRatio)
			{
				Vector3 vector = new Vector3(1f * num2, 1f, 1f);
				Utilities.ScaleMesh(base.GetComponent<MeshFilter>().mesh, vector, true);
				this.prevAspectRatio = num;
			}
			base.GetComponent<Renderer>().sharedMaterials[1].mainTexture = texture2D;
			base.GetComponent<Renderer>().sharedMaterials[2].mainTexture = texture2D;
			UnityEngine.Object.Destroy(base.GetComponent<BoxCollider>());
			base.gameObject.AddComponent<BoxCollider>();
			base.ResetObject();
			this.UpdateButtons(num2);
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0004FA50 File Offset: 0x0004DC50
	public void Search()
	{
		string text = Language.Translate("Search");
		UIDialog.ShowInput(text, "OK", "Cancel", new Action<string>(this.Search), null, this.currentNeedle, text);
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x0004FA8C File Offset: 0x0004DC8C
	public void Search(string needle)
	{
		if (needle == "")
		{
			return;
		}
		if (needle != this.currentNeedle)
		{
			this.currentNeedle = needle;
			this.currentPageSearchResultIndex = -1;
		}
		PDFDocument pdfdocument = this.customPDFContainer.PDFDocument;
		for (int i = 0; i < this.PageCount; i++)
		{
			int num = (i + this.CurrentPDFPage) % this.PageCount;
			PDFTextPage textPage = pdfdocument.GetPage(num).GetTextPage();
			if (textPage != null)
			{
				this.searchResults = textPage.Search(needle, PDFSearchHandle.MatchOption.NONE, 0);
				if (i == 0)
				{
					if (this.currentPageSearchResultIndex < this.searchResults.Count - 1)
					{
						this.highlightSearchResult(textPage, num, this.currentPageSearchResultIndex + 1);
						return;
					}
				}
				else if (this.searchResults.Count > 0)
				{
					this.highlightSearchResult(textPage, num, 0);
					return;
				}
			}
		}
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0004FB50 File Offset: 0x0004DD50
	public void SearchNext()
	{
		this.Search(this.currentNeedle);
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0004FB60 File Offset: 0x0004DD60
	public void SearchPrev()
	{
		if (this.currentNeedle == "")
		{
			return;
		}
		PDFDocument pdfdocument = this.customPDFContainer.PDFDocument;
		for (int i = 0; i < this.PageCount; i++)
		{
			int num = (this.CurrentPDFPage - i) % this.PageCount;
			if (num < 0)
			{
				num += this.PageCount;
			}
			PDFTextPage textPage = pdfdocument.GetPage(num).GetTextPage();
			if (textPage != null)
			{
				this.searchResults = textPage.Search(this.currentNeedle, PDFSearchHandle.MatchOption.NONE, 0);
				if (i == 0)
				{
					if (this.currentPageSearchResultIndex > 0 && this.currentPageSearchResultIndex - 1 < this.searchResults.Count - 1)
					{
						this.highlightSearchResult(textPage, num, this.currentPageSearchResultIndex - 1);
						return;
					}
				}
				else if (this.searchResults.Count > 0)
				{
					this.highlightSearchResult(textPage, num, this.searchResults.Count - 1);
					return;
				}
			}
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0004FC3C File Offset: 0x0004DE3C
	private void highlightSearchResult(PDFTextPage textPage, int page, int resultIndex)
	{
		this.CurrentPDFPage = page;
		this.currentPageSearchResultIndex = resultIndex;
		PDFSearchResult pdfsearchResult = this.searchResults[resultIndex];
		Rect charBox = textPage.GetCharBox(pdfsearchResult.StartIndex);
		for (int i = 1; i < pdfsearchResult.Count; i++)
		{
			Rect charBox2 = textPage.GetCharBox(pdfsearchResult.StartIndex + i);
			if (charBox2.x > charBox.x)
			{
				charBox.width = charBox2.x + charBox2.width - charBox.x;
				charBox.height = Mathf.Max(charBox.height, charBox2.height);
			}
		}
		this.HighlightBox = this.HighlightBoxFromPDFRect(textPage.Page, charBox);
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x0004FCF0 File Offset: 0x0004DEF0
	private Vector4 HighlightBoxFromPDFRect(PDFPage page, Rect pdfRect)
	{
		Vector4 result = new Vector4(pdfRect.x, pdfRect.y - pdfRect.height, pdfRect.x + pdfRect.width, pdfRect.y);
		Vector2 pageSize = page.GetPageSize(1f);
		result.x /= pageSize.x;
		result.z /= pageSize.x;
		result.y /= pageSize.y;
		result.w /= pageSize.y;
		result.x -= 0.01f;
		result.z += 0.01f;
		result.y -= 0.01f;
		result.w += 0.01f;
		return result;
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnCallCustomRPC()
	{
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnCallCustomRPC(NetworkPlayer networkPlayer)
	{
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0004FDC4 File Offset: 0x0004DFC4
	private void OnPDFFinish(CustomPDFContainer _customPDFContainer)
	{
		if (_customPDFContainer.PDFDocument == null)
		{
			base.RemoveLoading();
			return;
		}
		this.customPDFContainer = _customPDFContainer;
		this.PageCount = _customPDFContainer.GetPageCount();
		this.IsSearchable = false;
		for (int i = 0; i < this.PageCount; i++)
		{
			PDFTextPage textPage = _customPDFContainer.PDFDocument.GetPage(i).GetTextPage();
			if (textPage != null && textPage.CountChars() > 0)
			{
				this.IsSearchable = true;
				break;
			}
		}
		base.GetComponent<Renderer>().sharedMaterials = Utilities.CloneMaterials(base.GetComponent<Renderer>().sharedMaterials);
		if (base.CurrentlyLoading())
		{
			base.RemoveLoading();
		}
		if (Network.isServer && base.NPO)
		{
			base.NPO.IsLocked = this.CacheFreeze;
		}
		this.bSetupOnce = true;
		this.UpdatePDF();
		if (Singleton<UIPDFPopout>.Instance.gameObject.activeInHierarchy)
		{
			Singleton<UIPDFPopout>.Instance.gameObject.SetActive(false);
			Wait.Frames(delegate
			{
				Singleton<UIPDFPopout>.Instance.Init(this);
			}, 1);
		}
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0004FEC4 File Offset: 0x0004E0C4
	public void CalculateBookmarkLevel()
	{
		this.bookmarkLevel = 0;
		this.highestLevel = 0;
		this.prevPage = 0;
		this.ExamineBookmarks(this.customPDFContainer.PDFDocument.GetRootBookmark(), 0);
		for (int i = 1; i <= this.highestLevel; i++)
		{
			int num;
			if (this.totalPageGapPerLevel.TryGetValue(i, out num) && (float)num / (float)this.countPerLevel[i] >= (float)CustomPDF.HEADER_GAP_THRESHOLD)
			{
				this.bookmarkLevel = i;
			}
		}
		this.totalPageGapPerLevel.Clear();
		this.countPerLevel.Clear();
		this.bookmarkLevel = Mathf.Min(this.bookmarkLevel, CustomPDF.BOOKMARK_LEVEL);
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x0004FF6C File Offset: 0x0004E16C
	private void ExamineBookmarks(PDFBookmark bookmark, int level = 0)
	{
		this.highestLevel = Mathf.Max(this.highestLevel, level);
		if (bookmark.GetDest() != null)
		{
			int pageIndex = bookmark.GetDest().PageIndex;
			if (pageIndex >= 0 && pageIndex < this.PageCount)
			{
				if (this.totalPageGapPerLevel.ContainsKey(level))
				{
					Dictionary<int, int> dictionary = this.totalPageGapPerLevel;
					dictionary[level] += pageIndex - this.prevPage;
					Dictionary<int, int> dictionary2 = this.countPerLevel;
					int num = dictionary2[level];
					dictionary2[level] = num + 1;
				}
				else
				{
					this.totalPageGapPerLevel[level] = pageIndex - this.prevPage;
					this.countPerLevel[level] = 1;
				}
				this.prevPage = pageIndex;
			}
		}
		foreach (PDFBookmark bookmark2 in bookmark.EnumerateChildrenBookmarks())
		{
			this.ExamineBookmarks(bookmark2, level + 1);
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0005006C File Offset: 0x0004E26C
	private int GetBookmark(PDFBookmark bookmark, int selectionPage, bool getPageAfter, int level = 0)
	{
		if (bookmark.GetDest() != null)
		{
			int pageIndex = bookmark.GetDest().PageIndex;
			if (pageIndex >= 0 && pageIndex < this.PageCount)
			{
				if (pageIndex > selectionPage)
				{
					if (!getPageAfter)
					{
						return this.prevPage;
					}
					return pageIndex;
				}
				else if (pageIndex < selectionPage)
				{
					this.prevPage = pageIndex;
				}
			}
		}
		if (level < this.bookmarkLevel)
		{
			foreach (PDFBookmark bookmark2 in bookmark.EnumerateChildrenBookmarks())
			{
				int bookmark3 = this.GetBookmark(bookmark2, selectionPage, getPageAfter, level + 1);
				if (bookmark3 >= 0)
				{
					return bookmark3;
				}
			}
			return -1;
		}
		return -1;
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00050114 File Offset: 0x0004E314
	private int NextBookmark(int targetPage)
	{
		this.prevPage = 0;
		int bookmark = this.GetBookmark(this.customPDFContainer.PDFDocument.GetRootBookmark(), targetPage, true, 0);
		if (bookmark >= 0)
		{
			return bookmark;
		}
		return this.PageCount - 1;
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00050150 File Offset: 0x0004E350
	private int PrevBookmark(int targetPage)
	{
		this.prevPage = 0;
		int bookmark = this.GetBookmark(this.customPDFContainer.PDFDocument.GetRootBookmark(), targetPage, false, 0);
		if (bookmark >= 0)
		{
			return bookmark;
		}
		return this.prevPage;
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x0005018C File Offset: 0x0004E38C
	private static int MaxTypedNumber(NetworkPhysicsObject npo)
	{
		CustomPDF component = npo.GetComponent<CustomPDF>();
		if (component)
		{
			return component.PageCount + component.PageDisplayOffset;
		}
		return 0;
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x000501B8 File Offset: 0x0004E3B8
	private static void HandleTypedNumber(NetworkPhysicsObject npo, int playerID, int number)
	{
		CustomPDF component = npo.GetComponent<CustomPDF>();
		if (component)
		{
			component.ClampedSetCurrentPage(number, true);
		}
	}

	// Token: 0x040007E9 RID: 2025
	public static int BIG_PAGE_STEP = 10;

	// Token: 0x040007EA RID: 2026
	public static int HEADER_GAP_THRESHOLD = 0;

	// Token: 0x040007EB RID: 2027
	public static int BOOKMARK_LEVEL = 1;

	// Token: 0x040007EC RID: 2028
	public static bool ONLY_SHOW_UI_WHEN_HOVERED = true;

	// Token: 0x040007ED RID: 2029
	public const float SEARCH_HIGHLIGHT_PADDING = 0.01f;

	// Token: 0x040007EE RID: 2030
	public GameObject Root;

	// Token: 0x040007EF RID: 2031
	public UIButton NextButton;

	// Token: 0x040007F0 RID: 2032
	public UIButton PrevButton;

	// Token: 0x040007F1 RID: 2033
	public UIButton BackButton;

	// Token: 0x040007F2 RID: 2034
	public UIButton ForwardButton;

	// Token: 0x040007F3 RID: 2035
	public UIButton PopoutButton;

	// Token: 0x040007F4 RID: 2036
	public string CustomPDFURL = "";

	// Token: 0x040007F5 RID: 2037
	public string CustomPDFPassword = "";

	// Token: 0x040007F7 RID: 2039
	public bool bDestroyOnCancel = true;

	// Token: 0x040007F8 RID: 2040
	private int lastPageTurn;

	// Token: 0x040007F9 RID: 2041
	private int _page;

	// Token: 0x040007FA RID: 2042
	private int _pageDisplayOffset;

	// Token: 0x040007FB RID: 2043
	private Vector4 _highlightBox = Vector4.zero;

	// Token: 0x040007FD RID: 2045
	private bool facingUp = true;

	// Token: 0x040007FE RID: 2046
	private float scale = 2f;

	// Token: 0x040007FF RID: 2047
	private CustomPDFContainer customPDFContainer;

	// Token: 0x04000800 RID: 2048
	private bool bSetupOnce;

	// Token: 0x04000801 RID: 2049
	private float prevAspectRatio = 1f;

	// Token: 0x04000802 RID: 2050
	private IList<PDFSearchResult> searchResults;

	// Token: 0x04000803 RID: 2051
	private int currentPageSearchResultIndex = -1;

	// Token: 0x04000804 RID: 2052
	private string currentNeedle = "";

	// Token: 0x04000805 RID: 2053
	private Dictionary<int, int> totalPageGapPerLevel = new Dictionary<int, int>();

	// Token: 0x04000806 RID: 2054
	private Dictionary<int, int> countPerLevel = new Dictionary<int, int>();

	// Token: 0x04000807 RID: 2055
	private int bookmarkLevel;

	// Token: 0x04000808 RID: 2056
	private int highestLevel;

	// Token: 0x04000809 RID: 2057
	private int prevPage;
}
