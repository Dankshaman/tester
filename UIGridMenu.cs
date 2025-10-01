using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using I2.Loc;
using NewNet;
using UnityEngine;

// Token: 0x020002D4 RID: 724
public class UIGridMenu : MonoBehaviour
{
	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06002343 RID: 9027 RVA: 0x000FA4DA File Offset: 0x000F86DA
	// (set) Token: 0x06002344 RID: 9028 RVA: 0x000FA4E2 File Offset: 0x000F86E2
	[HideInInspector]
	public int currentPage { get; private set; }

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06002345 RID: 9029 RVA: 0x000FA4EB File Offset: 0x000F86EB
	// (set) Token: 0x06002346 RID: 9030 RVA: 0x000FA4F3 File Offset: 0x000F86F3
	public bool currentApplyDefaultTheme { get; private set; }

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06002347 RID: 9031 RVA: 0x000FA4FC File Offset: 0x000F86FC
	// (set) Token: 0x06002348 RID: 9032 RVA: 0x000FA504 File Offset: 0x000F8704
	[HideInInspector]
	public string RootPath { get; protected set; }

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06002349 RID: 9033 RVA: 0x000FA50D File Offset: 0x000F870D
	// (set) Token: 0x0600234A RID: 9034 RVA: 0x000FA515 File Offset: 0x000F8715
	public string CurrentPath { get; protected set; }

	// Token: 0x0600234B RID: 9035 RVA: 0x000FA520 File Offset: 0x000F8720
	public virtual string GetCurrentFolder()
	{
		if (string.IsNullOrEmpty(this.RootPath) || string.IsNullOrEmpty(this.CurrentPath) || this.RootPath == this.CurrentPath)
		{
			return "<Root Folder>";
		}
		return this.CurrentPath.Replace(this.RootPath, "");
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x0600234C RID: 9036 RVA: 0x000FA576 File Offset: 0x000F8776
	// (set) Token: 0x0600234D RID: 9037 RVA: 0x000FA57E File Offset: 0x000F877E
	[HideInInspector]
	public List<string> Folders { get; protected set; } = new List<string>();

	// Token: 0x0600234E RID: 9038 RVA: 0x000FA588 File Offset: 0x000F8788
	protected virtual void Awake()
	{
		if (this.SearchInput)
		{
			EventDelegate.Add(this.SearchInput.onChange, new EventDelegate.Callback(this.SearchChange));
		}
		if (this.ScrollCollider)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.ScrollCollider);
			uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Combine(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScrollCollider));
		}
		if (this.BackButton)
		{
			EventDelegate.Add(this.BackButton.onClick, new EventDelegate.Callback(this.OnClickBack));
		}
		if (this.Pages)
		{
			this.Pages.PageLeft += this.PageLeft;
			this.Pages.PageRight += this.PageRight;
			this.Pages.SetPage += this.SetPage;
		}
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnEnable()
	{
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x000FA678 File Offset: 0x000F8878
	protected virtual void OnDisable()
	{
		for (int i = 0; i < this.objectGridButtons.Count; i++)
		{
			this.objectGridButtons[i].Cleanup();
		}
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x000FA6AC File Offset: 0x000F88AC
	protected virtual void OnDestroy()
	{
		if (this.SearchInput)
		{
			EventDelegate.Remove(this.SearchInput.onChange, new EventDelegate.Callback(this.SearchChange));
		}
		if (this.ScrollCollider)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.ScrollCollider);
			uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Remove(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScrollCollider));
		}
		if (this.BackButton)
		{
			EventDelegate.Remove(this.BackButton.onClick, new EventDelegate.Callback(this.OnClickBack));
		}
		if (this.Pages)
		{
			this.Pages.PageLeft -= this.PageLeft;
			this.Pages.PageRight -= this.PageRight;
			this.Pages.SetPage -= this.SetPage;
		}
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x000FA79C File Offset: 0x000F899C
	private void Init()
	{
		if (this.initialized)
		{
			return;
		}
		List<Transform> childList = this.Grid.GetChildList();
		for (int i = 0; i < childList.Count; i++)
		{
			Transform transform = childList[i];
			transform.parent = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		this.objectGridButtons.Clear();
		this.currentPage = 1;
		for (int j = 0; j < this.NumberButtons; j++)
		{
			GameObject gameObject = this.Grid.gameObject.AddChild(this.PrefabButton);
			this.objectGridButtons.Add(gameObject.GetComponent<UIGridMenuButton>());
		}
		this.Grid.Reposition();
		this.initialized = true;
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000FA844 File Offset: 0x000F8A44
	public void Load<T>(List<T> buttons, int page = 1, string title = "", bool addBack = true, bool applyDefaultTheme = true) where T : UIGridMenu.GridButton
	{
		this.Init();
		if (addBack && this.BackButton)
		{
			this.BackStates.Add(new UIGridMenu.GridMenuState(this.MenuTitle ? this.MenuTitle.GetComponent<Localize>().mTerm : "", this.currentButtons, this.currentPage, this.currentSearch, this.currentApplyDefaultTheme));
		}
		if (this.MenuTitle)
		{
			this.MenuTitle.GetComponent<Localize>().SetTerm(title);
			this.MenuTitle.text = Language.Translate(title);
		}
		if (this.BackButton)
		{
			this.BackButton.gameObject.SetActive(this.BackStates.Count > 0);
		}
		if (this.MenuSprite)
		{
			this.MenuSprite.gameObject.SetActive(this.BackStates.Count <= 0);
		}
		this.currentButtons = buttons.Cast<UIGridMenu.GridButton>().ToList<UIGridMenu.GridButton>();
		string[] searches = this.currentSearch.ToLower().Split(new char[]
		{
			' '
		});
		this.currentApplyDefaultTheme = applyDefaultTheme;
		for (int i = 0; i < this.currentButtons.Count; i++)
		{
			UIGridMenu.GridButton gridButton = this.currentButtons[i];
			gridButton.Enable = gridButton.IsSearched(searches);
			if (applyDefaultTheme)
			{
				gridButton.applyDefaultTheme = true;
			}
		}
		this.SetPage(page);
		this.OnLoad();
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void OnLoad()
	{
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000FA9BC File Offset: 0x000F8BBC
	public void UpdateButtons()
	{
		int num = (this.currentPage - 1) * this.objectGridButtons.Count;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.currentButtons.Count; i++)
		{
			UIGridMenu.GridButton gridButton = this.currentButtons[i];
			if (gridButton.Enable)
			{
				if (num3 >= num && num2 < this.objectGridButtons.Count)
				{
					UIGridMenuButton objectButton = this.objectGridButtons[num2];
					gridButton.UpdateButton(objectButton);
					num2++;
				}
				num3++;
			}
		}
		if (this.Pages)
		{
			this.Pages.Set(this.currentPage, this.numberPages);
		}
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x000FAA64 File Offset: 0x000F8C64
	protected void SetPage(int page)
	{
		this.currentPage = page;
		int num = (this.currentPage - 1) * this.objectGridButtons.Count;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.objectGridButtons.Count; i++)
		{
			this.objectGridButtons[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this.currentButtons.Count; j++)
		{
			UIGridMenu.GridButton gridButton = this.currentButtons[j];
			if (gridButton.Enable)
			{
				if (num2 >= num && num3 < this.objectGridButtons.Count)
				{
					UIGridMenuButton uigridMenuButton = this.objectGridButtons[num3];
					uigridMenuButton.gameObject.SetActive(true);
					gridButton.GridMenu = this;
					gridButton.UpdateButton(uigridMenuButton);
					num3++;
				}
				num2++;
			}
		}
		this.numberPages = (num2 - 1) / Mathf.Max(this.objectGridButtons.Count, 1) + 1;
		if (this.Pages)
		{
			this.Pages.Set(this.currentPage, this.numberPages);
		}
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000FAB77 File Offset: 0x000F8D77
	public void SafeSetPage(int page)
	{
		if (page < 1 || page > this.numberPages)
		{
			return;
		}
		this.SetPage(page);
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x000FAB8E File Offset: 0x000F8D8E
	private void SearchChange()
	{
		if (!this.blockOnChangeSearch)
		{
			this.Search(this.SearchInput.value);
		}
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000FABAC File Offset: 0x000F8DAC
	public virtual void Search(string search)
	{
		string[] searches = search.ToLower().Split(new char[]
		{
			' '
		});
		for (int i = 0; i < this.currentButtons.Count; i++)
		{
			UIGridMenu.GridButton gridButton = this.currentButtons[i];
			gridButton.Enable = gridButton.IsSearched(searches);
		}
		this.currentSearch = search;
		this.SetPage(1);
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000FAC0C File Offset: 0x000F8E0C
	public virtual void ResetSearch(bool blockOnChange = true)
	{
		this.currentSearch = "";
		this.blockOnChangeSearch = blockOnChange;
		this.SearchInput.value = "";
		this.blockOnChangeSearch = false;
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000FAC37 File Offset: 0x000F8E37
	public void PageRight()
	{
		if (this.currentPage == this.numberPages)
		{
			return;
		}
		this.SetPage(this.currentPage + 1);
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000FAC56 File Offset: 0x000F8E56
	public void PageLeft()
	{
		if (this.currentPage == 1)
		{
			return;
		}
		this.SetPage(this.currentPage - 1);
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000FAC70 File Offset: 0x000F8E70
	public void Reload(bool DelayThumbnailCleanup = true)
	{
		this.Reload(this.currentPage, DelayThumbnailCleanup);
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000FAC80 File Offset: 0x000F8E80
	public virtual void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		for (int i = 0; i < this.objectGridButtons.Count; i++)
		{
			this.objectGridButtons[i].DelayThumbnailCleanup = DelayThumbnailCleanup;
		}
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000FACB5 File Offset: 0x000F8EB5
	public void ChangeButtonPrefab(GameObject prefab)
	{
		if (prefab != this.PrefabButton)
		{
			this.PrefabButton = prefab;
			this.initialized = false;
		}
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x000FACD3 File Offset: 0x000F8ED3
	public void OnScrollCollider(GameObject go, float delta)
	{
		this.OnScroll(delta);
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000FACDC File Offset: 0x000F8EDC
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

	// Token: 0x06002362 RID: 9058 RVA: 0x000FAD14 File Offset: 0x000F8F14
	public virtual void OnClickBack()
	{
		if (this.BackStates.Count > 0)
		{
			UIGridMenu.GridMenuState gridMenuState = this.BackStates[this.BackStates.Count - 1];
			this.BackStates.Remove(gridMenuState);
			this.Load<UIGridMenu.GridButton>(gridMenuState.buttons, gridMenuState.page, gridMenuState.title, false, gridMenuState.applyDefaultTheme);
		}
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000FAD74 File Offset: 0x000F8F74
	public void ClearBackStates()
	{
		this.BackStates.Clear();
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000FAD81 File Offset: 0x000F8F81
	public void RemoveLastBackState()
	{
		if (this.BackStates.Count > 0)
		{
			this.BackStates.RemoveAt(this.BackStates.Count - 1);
		}
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000FADA9 File Offset: 0x000F8FA9
	public void SetPathFolders(string path, string excludePath = "")
	{
		if (string.IsNullOrEmpty(path))
		{
			this.RootPath = "";
			this.Folders.Clear();
			return;
		}
		this.Folders = SerializationScript.GetLocalFolders(path, true, excludePath);
		this.RootPath = path;
	}

	// Token: 0x0400165E RID: 5726
	private static readonly Vector3 offscreenSpawn = new Vector3(10000f, 10000f, 10000f);

	// Token: 0x0400165F RID: 5727
	public UIInput SearchInput;

	// Token: 0x04001660 RID: 5728
	public UIButton BackButton;

	// Token: 0x04001661 RID: 5729
	public UISprite MenuSprite;

	// Token: 0x04001662 RID: 5730
	public UILabel MenuTitle;

	// Token: 0x04001663 RID: 5731
	public GameObject PrefabButton;

	// Token: 0x04001664 RID: 5732
	public int NumberButtons = 28;

	// Token: 0x04001665 RID: 5733
	public UIGrid Grid;

	// Token: 0x04001666 RID: 5734
	public UIPages Pages;

	// Token: 0x04001667 RID: 5735
	public GameObject ScrollCollider;

	// Token: 0x04001668 RID: 5736
	private readonly List<UIGridMenuButton> objectGridButtons = new List<UIGridMenuButton>();

	// Token: 0x04001669 RID: 5737
	protected List<UIGridMenu.GridButton> currentButtons = new List<UIGridMenu.GridButton>();

	// Token: 0x0400166A RID: 5738
	private bool initialized;

	// Token: 0x04001670 RID: 5744
	protected List<UIGridMenu.GridMenuState> BackStates = new List<UIGridMenu.GridMenuState>();

	// Token: 0x04001671 RID: 5745
	protected int numberPages = 1;

	// Token: 0x04001672 RID: 5746
	private string currentSearch = "";

	// Token: 0x04001673 RID: 5747
	private bool blockOnChangeSearch;

	// Token: 0x04001674 RID: 5748
	private float lastScrollTime;

	// Token: 0x02000725 RID: 1829
	[Serializable]
	public abstract class GridButton
	{
		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x0017C56E File Offset: 0x0017A76E
		// (set) Token: 0x06003DBB RID: 15803 RVA: 0x0017C578 File Offset: 0x0017A778
		public bool applyDefaultTheme
		{
			get
			{
				return this._applyDefaultTheme;
			}
			set
			{
				this._applyDefaultTheme = value;
				if (this._applyDefaultTheme)
				{
					this.ThemeNormalAs = UIPalette.UI.ButtonNormal;
					this.ThemeHoverAs = UIPalette.UI.ButtonHover;
					this.ThemePressedAs = UIPalette.UI.ButtonPressed;
					this.ThemeDisabledAs = UIPalette.UI.ButtonDisabled;
					this.ThemeLabelAs = UIPalette.UI.Label;
					this.ThemeSpriteAs = UIPalette.UI.Low;
					return;
				}
				this.ThemeNormalAs = UIPalette.UI.DoNotTheme;
				this.ThemeHoverAs = UIPalette.UI.DoNotTheme;
				this.ThemePressedAs = UIPalette.UI.DoNotTheme;
				this.ThemeDisabledAs = UIPalette.UI.DoNotTheme;
				this.ThemeLabelAs = UIPalette.UI.DoNotTheme;
				this.ThemeSpriteAs = UIPalette.UI.DoNotTheme;
			}
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x0017C5EC File Offset: 0x0017A7EC
		public GridButton()
		{
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x0017C672 File Offset: 0x0017A872
		public virtual void OnClick()
		{
			if (this.CloseMenu)
			{
				this.CloseMenu.SetActive(false);
			}
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x0017C68D File Offset: 0x0017A88D
		public virtual void OnRightClick()
		{
			this.OnClick();
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0017C695 File Offset: 0x0017A895
		public void OnOptionsPopup(string selection)
		{
			this.OptionsPopupActions[selection]();
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x0017C6A8 File Offset: 0x0017A8A8
		public virtual void UpdateButton(UIGridMenuButton objectButton)
		{
			this.GridMenuButton = objectButton;
			objectButton.Cleanup();
			objectButton.onClick = new Action(this.OnClick);
			objectButton.onRightClick = new Action(this.OnRightClick);
			objectButton.onScroll = new Action<float>(this.OnScroll);
			if (this.ThemeLabelAs != UIPalette.UI.DoNotTheme)
			{
				objectButton.NameLabel.color = Singleton<UIPalette>.Instance.CurrentThemeColours[this.ThemeLabelAs];
				objectButton.NameLabel.ThemeAs = this.ThemeLabelAs;
				objectButton.NameLabel.DoNotInitTheme();
			}
			objectButton.NameLabel.text = (this.Autotranslate ? Language.Translate(this.Name) : this.Name);
			NGUIHelper.ClampAndAddDots(objectButton.NameLabel, objectButton.gameObject, false);
			if (objectButton.GetComponent<UITooltipScript>())
			{
				objectButton.GetComponent<UITooltipScript>().DelayTooltip = "";
			}
			objectButton.ThumbnailTexture.mainTexture = this.Thumbnail;
			if (!string.IsNullOrEmpty(this.ThumbnailSpriteName))
			{
				this.SpriteName = this.ThumbnailSpriteName;
			}
			if (!string.IsNullOrEmpty(this.SpriteName))
			{
				objectButton.ThumbnailSprite.enabled = true;
				objectButton.ThumbnailSprite.spriteName = this.SpriteName;
				if (this.ThemeSpriteAs != UIPalette.UI.DoNotTheme)
				{
					objectButton.ThumbnailSprite.ThemeAs = this.ThemeSpriteAs;
					objectButton.ThumbnailSprite.color = Singleton<UIPalette>.Instance.CurrentThemeColours[this.ThemeSpriteAs];
				}
				else
				{
					objectButton.ThumbnailSprite.color = this.SpriteColor;
				}
			}
			if (this.Thumbnail || !string.IsNullOrEmpty(this.SpriteName))
			{
				objectButton.QuestionMarkSprite.enabled = false;
			}
			if (this.ThemeNormalAs == UIPalette.UI.DoNotTheme)
			{
				objectButton.MainButton.defaultColor = this.ButtonColor;
				objectButton.SpriteButton.color = this.ButtonColor;
				objectButton.BackgroundSprite.color = this.BackgroundColor;
			}
			else
			{
				objectButton.MainButton.ThemeNormalAs = this.ThemeNormalAs;
				objectButton.MainButton.ThemeNormalAsSetting = this.ThemeNormalAs;
				Colour colour = Singleton<UIPalette>.Instance.CurrentThemeColours[this.ThemeNormalAs];
				objectButton.MainButton.defaultColor = colour;
				objectButton.SpriteButton.color = colour;
				objectButton.BackgroundSprite.color = colour;
			}
			if (this.ThemeHoverAs == UIPalette.UI.DoNotTheme)
			{
				objectButton.MainButton.hover = this.ButtonHoverColor;
			}
			else
			{
				objectButton.MainButton.hover = Singleton<UIPalette>.Instance.CurrentThemeColours[this.ThemeHoverAs];
				objectButton.MainButton.ThemeHoverAs = this.ThemeHoverAs;
				objectButton.MainButton.ThemeHoverAsSetting = this.ThemeHoverAs;
			}
			if (this.ThemePressedAs != UIPalette.UI.DoNotTheme)
			{
				objectButton.MainButton.ThemePressedAs = this.ThemePressedAs;
				objectButton.MainButton.ThemePressedAsSetting = this.ThemePressedAs;
			}
			if (this.ThemeDisabledAs != UIPalette.UI.DoNotTheme)
			{
				objectButton.MainButton.ThemeDisabledAs = this.ThemeDisabledAs;
				objectButton.MainButton.ThemeDisabledAsSetting = this.ThemeDisabledAs;
			}
			objectButton.TopLeftLabel.text = this.TopLeftText;
			if (this.OptionsPopupActions.Count > 0)
			{
				objectButton.HoverEnable.enabled = true;
				objectButton.PopupList.items = this.OptionsPopupActions.Keys.Reverse<string>().ToList<string>();
				objectButton.onOptionsPopup = new Action<string>(this.OnOptionsPopup);
			}
			if (!string.IsNullOrEmpty(this.DelayTooltip))
			{
				objectButton.gameObject.AddMissingComponent<UITooltipScript>().DelayTooltip = this.DelayTooltip;
			}
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual void OnFavorite()
		{
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x0017CA49 File Offset: 0x0017AC49
		public virtual void OnScroll(float delta)
		{
			if (this.GridMenu && !zInput.GetButton("Alt", ControlType.All))
			{
				this.GridMenu.OnScroll(delta);
			}
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x0017CA74 File Offset: 0x0017AC74
		public bool IsSearched(string[] searches)
		{
			if (this.cachedSearch == null)
			{
				this.cachedSearch = new List<string>(this.Tags.Count + 1);
				this.cachedSearch.Add(this.Name.ToLower());
				this.cachedSearch.AddRange(this.Tags);
			}
			foreach (string value in searches)
			{
				bool flag = false;
				for (int j = 0; j < this.cachedSearch.Count; j++)
				{
					if (this.cachedSearch[j].Contains(value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002ACC RID: 10956
		public string Name;

		// Token: 0x04002ACD RID: 10957
		[NonSerialized]
		public bool Autotranslate;

		// Token: 0x04002ACE RID: 10958
		public Texture Thumbnail;

		// Token: 0x04002ACF RID: 10959
		public string ThumbnailSpriteName;

		// Token: 0x04002AD0 RID: 10960
		public List<string> Tags = new List<string>();

		// Token: 0x04002AD1 RID: 10961
		[HideInInspector]
		[NonSerialized]
		public Color ButtonColor = Color.white;

		// Token: 0x04002AD2 RID: 10962
		[NonSerialized]
		public Color ButtonHoverColor = Color.white;

		// Token: 0x04002AD3 RID: 10963
		[HideInInspector]
		[NonSerialized]
		public Color BackgroundColor = Color.white;

		// Token: 0x04002AD4 RID: 10964
		[HideInInspector]
		[NonSerialized]
		public bool Enable = true;

		// Token: 0x04002AD5 RID: 10965
		[HideInInspector]
		[NonSerialized]
		public GameObject CloseMenu;

		// Token: 0x04002AD6 RID: 10966
		[HideInInspector]
		[NonSerialized]
		public UIGridMenu GridMenu;

		// Token: 0x04002AD7 RID: 10967
		[HideInInspector]
		[NonSerialized]
		public Dictionary<string, Action> OptionsPopupActions = new Dictionary<string, Action>();

		// Token: 0x04002AD8 RID: 10968
		[HideInInspector]
		[NonSerialized]
		public UIGridMenuButton GridMenuButton;

		// Token: 0x04002AD9 RID: 10969
		[HideInInspector]
		[NonSerialized]
		public string TopLeftText;

		// Token: 0x04002ADA RID: 10970
		[HideInInspector]
		[NonSerialized]
		public string SpriteName;

		// Token: 0x04002ADB RID: 10971
		[HideInInspector]
		[NonSerialized]
		public Color SpriteColor = Color.white;

		// Token: 0x04002ADC RID: 10972
		[HideInInspector]
		[NonSerialized]
		public string DelayTooltip;

		// Token: 0x04002ADD RID: 10973
		[HideInInspector]
		[NonSerialized]
		public UIPalette.UI ThemeNormalAs = UIPalette.UI.DoNotTheme;

		// Token: 0x04002ADE RID: 10974
		[HideInInspector]
		[NonSerialized]
		public UIPalette.UI ThemeHoverAs = UIPalette.UI.DoNotTheme;

		// Token: 0x04002ADF RID: 10975
		[NonSerialized]
		public UIPalette.UI ThemePressedAs = UIPalette.UI.DoNotTheme;

		// Token: 0x04002AE0 RID: 10976
		[NonSerialized]
		public UIPalette.UI ThemeDisabledAs = UIPalette.UI.DoNotTheme;

		// Token: 0x04002AE1 RID: 10977
		[HideInInspector]
		[NonSerialized]
		public UIPalette.UI ThemeLabelAs = UIPalette.UI.DoNotTheme;

		// Token: 0x04002AE2 RID: 10978
		[HideInInspector]
		[NonSerialized]
		public UIPalette.UI ThemeSpriteAs = UIPalette.UI.DoNotTheme;

		// Token: 0x04002AE3 RID: 10979
		private bool _applyDefaultTheme;

		// Token: 0x04002AE4 RID: 10980
		private List<string> cachedSearch;
	}

	// Token: 0x02000726 RID: 1830
	[Serializable]
	public class GridButtonFolder : UIGridMenu.GridButton
	{
		// Token: 0x06003DC4 RID: 15812 RVA: 0x0017CB10 File Offset: 0x0017AD10
		public GridButtonFolder()
		{
			this.ButtonHoverColor = Colour.UIBlue;
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x0017CB40 File Offset: 0x0017AD40
		public override void OnClick()
		{
			base.OnClick();
			if (this.FolderButtons.Count > 0)
			{
				this.GridMenu.Load<UIGridMenu.GridButtonFolder>(this.FolderButtons, 1, this.Name, true, base.applyDefaultTheme);
				return;
			}
			this.GridMenu.Load<UIGridMenu.GridButtonComponent>(this.ComponentButtons, 1, this.Name, true, base.applyDefaultTheme);
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x0017CBA0 File Offset: 0x0017ADA0
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			if (this.Thumbnail)
			{
				objectButton.BackgroundSprite.enabled = false;
			}
		}

		// Token: 0x04002AE5 RID: 10981
		public List<UIGridMenu.GridButtonFolder> FolderButtons = new List<UIGridMenu.GridButtonFolder>();

		// Token: 0x04002AE6 RID: 10982
		public List<UIGridMenu.GridButtonComponent> ComponentButtons = new List<UIGridMenu.GridButtonComponent>();
	}

	// Token: 0x02000727 RID: 1831
	public abstract class GridButtonDrag : UIGridMenu.GridButton
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x0017CBC2 File Offset: 0x0017ADC2
		// (set) Token: 0x06003DC8 RID: 15816 RVA: 0x0017CBCA File Offset: 0x0017ADCA
		protected GameObject dragTarget
		{
			get
			{
				return this._dragTarget;
			}
			set
			{
				this._dragTarget = value;
				this.DraggingUI = this._dragTarget.GetComponentInChildren<UIWidget>();
			}
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x0017CBEC File Offset: 0x0017ADEC
		public virtual void OnDragStart()
		{
			this.localDragStartPos = this.dragTarget.transform.localPosition;
			this.dragStartZ = this.dragTarget.transform.position.z;
			if (this.DraggingUI)
			{
				if (this.GridMenuButton.GetComponent<BoxCollider2D>())
				{
					this.GridMenuButton.GetComponent<BoxCollider2D>().enabled = false;
				}
				this.prevParent = this.GridMenuButton.transform.parent;
				this.GridMenuButton.transform.parent = UIDragDropRoot.root;
				NGUITools.MarkParentAsChanged(this.GridMenuButton.gameObject);
			}
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0017CC90 File Offset: 0x0017AE90
		public virtual void OnDrag()
		{
			Vector3 vector = UICamera.currentCamera.ScreenToWorldPoint(UICamera.currentTouch.pos);
			vector = new Vector3(vector.x + this.dragOffset.x, vector.y + this.dragOffset.y, this.dragStartZ + this.dragOffset.z);
			this.dragTarget.transform.position = vector;
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0017CD08 File Offset: 0x0017AF08
		public virtual void OnDragEnd()
		{
			if (this.DraggingUI)
			{
				if (this.GridMenuButton.GetComponent<BoxCollider2D>())
				{
					this.GridMenuButton.GetComponent<BoxCollider2D>().enabled = true;
				}
				this.GridMenuButton.transform.parent = this.prevParent;
				NGUITools.MarkParentAsChanged(this.GridMenuButton.gameObject);
			}
			this.dragTarget.transform.localPosition = this.localDragStartPos;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0017CD7C File Offset: 0x0017AF7C
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			if (this.Draggable)
			{
				objectButton.onDragStart = new Action(this.OnDragStart);
				objectButton.onDrag = new Action(this.OnDrag);
				objectButton.onDragEnd = new Action(this.OnDragEnd);
			}
		}

		// Token: 0x04002AE7 RID: 10983
		public bool Draggable = true;

		// Token: 0x04002AE8 RID: 10984
		private bool DraggingUI;

		// Token: 0x04002AE9 RID: 10985
		protected GameObject _dragTarget;

		// Token: 0x04002AEA RID: 10986
		protected Vector3 dragOffset = Vector3.zero;

		// Token: 0x04002AEB RID: 10987
		private Vector3 localDragStartPos = Vector3.zero;

		// Token: 0x04002AEC RID: 10988
		private float dragStartZ;

		// Token: 0x04002AED RID: 10989
		private Transform prevParent;
	}

	// Token: 0x02000728 RID: 1832
	public abstract class GridButtonSpawn : UIGridMenu.GridButtonDrag
	{
		// Token: 0x06003DCE RID: 15822 RVA: 0x0017CDF8 File Offset: 0x0017AFF8
		public override void OnClick()
		{
			base.OnClick();
			ObjectVisualizer objectVisualizer = this.CreateVisualRepresentation();
			if (objectVisualizer != null)
			{
				if (PlayerScript.PointerScript)
				{
					PlayerScript.PointerScript.InteractiveSpawn(new SpawnDelegate(this.InteractiveSpawn), objectVisualizer);
					return;
				}
				this.Spawn(NetworkSingleton<NetworkUI>.Instance.SpawnPos);
			}
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x0017CE4C File Offset: 0x0017B04C
		public override void OnDragEnd()
		{
			Vector3 position = base.dragTarget.transform.position;
			base.OnDragEnd();
			if (UICamera.HoveredUIObject)
			{
				return;
			}
			Vector3 pos = UICamera.mainCamera.WorldToScreenPoint(position);
			RaycastHit[] array = Physics.RaycastAll(Camera.main.ScreenPointToRay(pos), 1000f);
			Array.Sort<RaycastHit>(array, new RaycastHitComparator());
			Vector3 spawnPos = Vector3.zero;
			foreach (RaycastHit raycastHit in array)
			{
				if (!raycastHit.collider.gameObject.CompareTag("Pointer"))
				{
					spawnPos = raycastHit.point;
					break;
				}
			}
			this.Spawn(spawnPos);
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual void Spawn(Vector3 spawnPos)
		{
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual void InteractiveSpawn(Vector3 spawnPos)
		{
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x00079594 File Offset: 0x00077794
		public virtual ObjectVisualizer CreateVisualRepresentation()
		{
			return null;
		}
	}

	// Token: 0x02000729 RID: 1833
	[Serializable]
	public class GridButtonComponent : UIGridMenu.GridButtonSpawn
	{
		// Token: 0x06003DD4 RID: 15828 RVA: 0x0017CEFD File Offset: 0x0017B0FD
		public GridButtonComponent()
		{
			this.ButtonHoverColor = Colour.UIBlue;
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x0017CF15 File Offset: 0x0017B115
		public override void InteractiveSpawn(Vector3 spawnPos)
		{
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<GameMode>.Instance.SpawnNameCoroutineFromUI(this.SpawnName, spawnPos, false, true, true);
				return;
			}
			Chat.NotPromotedErrorMessage("load");
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0017CF44 File Offset: 0x0017B144
		public override void Spawn(Vector3 spawnPos)
		{
			if (PlayerScript.Pointer)
			{
				spawnPos = PlayerScript.PointerScript.GetSpawnPosition(spawnPos, false);
			}
			else
			{
				spawnPos.y += 1f;
			}
			spawnPos.y += this.SpawnYOffset;
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<GameMode>.Instance.SpawnNameCoroutineFromUI(this.SpawnName, spawnPos, false, true, true);
				return;
			}
			Chat.NotPromotedErrorMessage("load");
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0017CFBB File Offset: 0x0017B1BB
		public override ObjectVisualizer CreateVisualRepresentation()
		{
			GameObject gameObject = this.CreatVisualObject();
			return new ObjectVisualizer(gameObject, gameObject.CompareTag("Board"));
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0017CFD4 File Offset: 0x0017B1D4
		private GameObject CreatVisualObject()
		{
			GameObject gameObject = NetworkSingleton<GameMode>.Instance.SpawnNameCoroutine(this.SpawnName, new Vector3(0f, 200f, 0f), true, true, true, false);
			if (gameObject == null)
			{
				Debug.LogError("Component does not exist: " + this.SpawnName);
				return null;
			}
			Vector3 vector;
			Bounds boundsNotNormalized = gameObject.GetComponent<NetworkPhysicsObject>().GetBoundsNotNormalized(out vector);
			this.SpawnYOffset = vector.y + boundsNotNormalized.extents.y;
			ManagerPhysicsObject.CleanupDummyObject(gameObject);
			UnityEngine.Object.Destroy(gameObject.GetComponent<CustomObject>());
			return gameObject;
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0017D064 File Offset: 0x0017B264
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			GameObject gameObject = this.CreatVisualObject();
			if (!gameObject)
			{
				return;
			}
			objectButton.BackgroundSprite.gameObject.SetActive(false);
			objectButton.SetSpawnedObject(gameObject, 95f, null);
			base.dragTarget = gameObject;
			this.dragOffset = new Vector3(0f, 0f, -1f);
		}

		// Token: 0x04002AEE RID: 10990
		public string SpawnName;

		// Token: 0x04002AEF RID: 10991
		[HideInInspector]
		private float SpawnYOffset;
	}

	// Token: 0x0200072A RID: 1834
	[Serializable]
	public class GridButtonObjectState : UIGridMenu.GridButtonSpawn
	{
		// Token: 0x06003DDA RID: 15834 RVA: 0x0017D0CF File Offset: 0x0017B2CF
		public GridButtonObjectState()
		{
			this.ButtonHoverColor = Colour.UIBlue;
			this.OptionsPopupActions.Add("Spawn", new Action(this.SpawnInPlace));
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0017D103 File Offset: 0x0017B303
		public override void OnClick()
		{
			if (PlayerScript.PointerScript)
			{
				PlayerScript.PointerScript.InteractiveSpawn(new SpawnDelegate(this.InteractiveSpawn), this.CreateVisualRepresentation());
				return;
			}
			this.Spawn(NetworkSingleton<NetworkUI>.Instance.SpawnPos);
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x0017D13F File Offset: 0x0017B33F
		public void SpawnInPlace()
		{
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObject(this.objectState);
				return;
			}
			Chat.NotPromotedErrorMessage("load");
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x000025B8 File Offset: 0x000007B8
		public void SaveObject()
		{
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0017D169 File Offset: 0x0017B369
		public override void InteractiveSpawn(Vector3 spawnPos)
		{
			this.SpawnOffset(spawnPos, false);
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0017D173 File Offset: 0x0017B373
		public override void Spawn(Vector3 spawnPos)
		{
			if (PlayerScript.Pointer)
			{
				spawnPos = PlayerScript.PointerScript.GetSpawnPosition(spawnPos, true);
			}
			else
			{
				spawnPos.y += 1f;
			}
			this.SpawnOffset(spawnPos, true);
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0017D1A9 File Offset: 0x0017B3A9
		private void SpawnOffset(Vector3 spawnPos, bool bOffsetY)
		{
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectOffset(this.objectState, spawnPos, bOffsetY);
				return;
			}
			Chat.NotPromotedErrorMessage("load");
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x0017D1D5 File Offset: 0x0017B3D5
		public override ObjectVisualizer CreateVisualRepresentation()
		{
			return new ObjectVisualizer(NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(this.objectState, true, false), this.objectState.Locked);
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x0017D1FC File Offset: 0x0017B3FC
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(this.objectState, true, false);
			if (gameObject == null)
			{
				return;
			}
			objectButton.BackgroundSprite.gameObject.SetActive(false);
			objectButton.SetSpawnedObject(gameObject, 95f, null);
			base.dragTarget = gameObject;
			this.dragOffset = new Vector3(0f, 0f, -1f);
		}

		// Token: 0x04002AF0 RID: 10992
		public ObjectState objectState;
	}

	// Token: 0x0200072B RID: 1835
	public class GridButtonCloudBase : UIGridMenu.GridButton
	{
		// Token: 0x06003DE3 RID: 15843 RVA: 0x0017D274 File Offset: 0x0017B474
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			if (this.GridMenu.Folders.Count <= 1 && this.OptionsPopupActions.ContainsKey("Move"))
			{
				this.OptionsPopupActions.Remove("Move");
			}
			base.UpdateButton(objectButton);
			objectButton.BackgroundSprite.enabled = false;
		}

		// Token: 0x04002AF1 RID: 10993
		public int Size;
	}

	// Token: 0x0200072C RID: 1836
	public class GridButtonCloudFolder : UIGridMenu.GridButtonCloudBase
	{
		// Token: 0x06003DE5 RID: 15845 RVA: 0x0017D2D4 File Offset: 0x0017B4D4
		public GridButtonCloudFolder()
		{
			this.SpriteName = "Icon-Folder2";
			this.SpriteColor = Color.black;
			this.ButtonHoverColor = Colour.UIBlue;
			this.OptionsPopupActions.Add("Delete", new Action(this.OnDelete));
			this.OptionsPopupActions.Add("Move", new Action(this.OnMove));
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0017D345 File Offset: 0x0017B545
		public override void OnClick()
		{
			base.OnClick();
			this.GridMenu.Load<UIGridMenu.GridButtonCloudBase>(this.GridMenu.GetComponent<UIGridMenuCloud>().GetCloudButtons(this.folder), 1, this.Name, true, true);
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x0017D377 File Offset: 0x0017B577
		public void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete {0}? All files contained will stop working in any mods.", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x0017D3A5 File Offset: 0x0017B5A5
		public void Delete()
		{
			Singleton<SteamManager>.Instance.RemoveCloudFolder(this.folder);
			this.GridMenu.Reload(true);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0017D3C3 File Offset: 0x0017B5C3
		public void OnMove()
		{
			UIDialog.ShowDropDown(Language.Translate("Move {0}?", this.Name), "Move", "Cancel", this.GridMenu.Folders, new Action<string>(this.Move), null, "");
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0017D404 File Offset: 0x0017B604
		private void Move(string LocalFolder)
		{
			if (LocalFolder == "<Root Folder>")
			{
				LocalFolder = "";
			}
			Singleton<SteamManager>.Instance.MoveCloudFolder(this.folder, Path.Combine(LocalFolder, Path.GetFileName(this.folder)));
			this.GridMenu.Reload(true);
		}

		// Token: 0x04002AF2 RID: 10994
		public string folder;
	}

	// Token: 0x0200072D RID: 1837
	[Serializable]
	public class GridButtonCloud : UIGridMenu.GridButtonCloudBase
	{
		// Token: 0x06003DEB RID: 15851 RVA: 0x0017D454 File Offset: 0x0017B654
		public GridButtonCloud()
		{
			this.ButtonHoverColor = Colour.UIBlue;
			this.OptionsPopupActions.Add("Delete", new Action(this.OnDelete));
			this.OptionsPopupActions.Add("Move", new Action(this.OnMove));
			this.OptionsPopupActions.Add("Preview", new Action(this.OnPreview));
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x0017D4CB File Offset: 0x0017B6CB
		public override void OnClick()
		{
			base.OnClick();
			TTSUtilities.CopyToClipboard(this.URL);
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x0017D4DE File Offset: 0x0017B6DE
		public void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete {0}? This file will stop working in any mods.", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x0017D50C File Offset: 0x0017B70C
		public void Delete()
		{
			Singleton<SteamManager>.Instance.DeleteFromCloud(this.CloudName);
			this.GridMenu.Reload(true);
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x0017D52A File Offset: 0x0017B72A
		public void OnMove()
		{
			UIDialog.ShowDropDown(Language.Translate("Move {0}?", this.Name), "Move", "Cancel", this.GridMenu.Folders, new Action<string>(this.Move), null, "");
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x0017D568 File Offset: 0x0017B768
		private void Move(string LocalFolder)
		{
			if (LocalFolder == "<Root Folder>")
			{
				LocalFolder = "";
			}
			Singleton<SteamManager>.Instance.MoveCloudInfo(this.CloudName, LocalFolder);
			this.GridMenu.Reload(true);
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x0017D59B File Offset: 0x0017B79B
		public void OnPreview()
		{
			if (string.IsNullOrEmpty(this.ThumbnailPath))
			{
				this.LoadObjectState();
				return;
			}
			TTSUtilities.OpenURL(this.URL);
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x0017D5BC File Offset: 0x0017B7BC
		private void LoadObjectState()
		{
			if (this.objectState == null)
			{
				return;
			}
			if (this.spawnedObject)
			{
				UnityEngine.Object.Destroy(this.spawnedObject);
				return;
			}
			this.spawnedObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(this.objectState, true, false);
			if (this.spawnedObject == null)
			{
				return;
			}
			this.GridMenuButton.BackgroundSprite.gameObject.SetActive(false);
			this.GridMenuButton.SetSpawnedObject(this.spawnedObject, 95f, null);
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0017D64D File Offset: 0x0017B84D
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			if (string.IsNullOrEmpty(this.ThumbnailPath))
			{
				this.LoadObjectState();
				return;
			}
			objectButton.StartLoadThumbnail(this.ThumbnailPath);
		}

		// Token: 0x04002AF3 RID: 10995
		public string URL;

		// Token: 0x04002AF4 RID: 10996
		public string ThumbnailPath;

		// Token: 0x04002AF5 RID: 10997
		public string CloudName;

		// Token: 0x04002AF6 RID: 10998
		public ObjectState objectState;

		// Token: 0x04002AF7 RID: 10999
		private GameObject spawnedObject;
	}

	// Token: 0x0200072E RID: 1838
	[Serializable]
	public class GridButtonBackground : UIGridMenu.GridButton
	{
		// Token: 0x06003DF4 RID: 15860 RVA: 0x0017D676 File Offset: 0x0017B876
		public GridButtonBackground()
		{
			this.SpriteName = "Icon-Backgrounds";
			this.SpriteColor = Color.black;
			this.ButtonHoverColor = Colour.UIBlue;
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x0017D6A4 File Offset: 0x0017B8A4
		public override void OnClick()
		{
			base.OnClick();
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<NetworkUI>.Instance.GUIChangeBackground(this.Background);
				if (!string.IsNullOrEmpty(this.BackgroundURL))
				{
					NetworkSingleton<NetworkUI>.Instance.GUIChangeBackground("Custom");
					CustomSky.ActiveCustomSky.CustomSkyURL = this.BackgroundURL;
					if (Network.isClient)
					{
						CustomSky.ActiveCustomSky.CallCustomRPC();
						return;
					}
				}
			}
			else
			{
				Chat.NotPromotedErrorMessage("load");
			}
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x0017D71C File Offset: 0x0017B91C
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.BackgroundSprite.enabled = false;
			string a = Utilities.RemoveCloneFromName(NetworkSingleton<ManagerPhysicsObject>.Instance.Sky.name);
			string b = "Sky_" + this.Background;
			if (a == b || (this.Background == "Custom" && CustomSky.ActiveCustomSky != null))
			{
				objectButton.MainButton.defaultColor = Colour.BlueDark;
				objectButton.SpriteButton.color = Colour.BlueDark;
			}
		}

		// Token: 0x04002AF8 RID: 11000
		public string Background;

		// Token: 0x04002AF9 RID: 11001
		public string BackgroundURL;
	}

	// Token: 0x0200072F RID: 1839
	[Serializable]
	public class GridButtonTable : UIGridMenu.GridButton
	{
		// Token: 0x06003DF7 RID: 15863 RVA: 0x0017D7B2 File Offset: 0x0017B9B2
		public GridButtonTable()
		{
			this.SpriteName = "Icon-Tables";
			this.SpriteColor = Color.black;
			this.ButtonHoverColor = Colour.UIBlue;
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x0017D7E0 File Offset: 0x0017B9E0
		public override void OnClick()
		{
			base.OnClick();
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<NetworkUI>.Instance.GUIChangeTable(this.Table);
				if (!string.IsNullOrEmpty(this.TableURL) && NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>() && NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>().CustomImageURL != this.TableURL)
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>().CustomImageURL = this.TableURL;
					if (Network.isClient)
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>().CallCustomRPC();
						return;
					}
				}
			}
			else
			{
				Chat.NotPromotedErrorMessage("load");
			}
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x0017D898 File Offset: 0x0017BA98
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.BackgroundSprite.enabled = false;
			string a = Utilities.RemoveCloneFromName(NetworkSingleton<ManagerPhysicsObject>.Instance.TableScript.name);
			string tablePrefabName = TableScript.GetTablePrefabName(this.Table);
			if (a == tablePrefabName)
			{
				objectButton.MainButton.defaultColor = Colour.GreenDark;
				objectButton.SpriteButton.color = Colour.GreenDark;
			}
		}

		// Token: 0x04002AFA RID: 11002
		public string Table;

		// Token: 0x04002AFB RID: 11003
		public string TableURL;
	}

	// Token: 0x02000730 RID: 1840
	[Serializable]
	public class GridButtonLighting : UIGridMenu.GridButton
	{
		// Token: 0x06003DFA RID: 15866 RVA: 0x0017D90A File Offset: 0x0017BB0A
		public GridButtonLighting()
		{
			this.SpriteName = "Icon-Lighting";
			this.SpriteColor = Color.black;
			this.ButtonHoverColor = Colour.UIBlue;
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x0017D938 File Offset: 0x0017BB38
		public override void OnClick()
		{
			base.OnClick();
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<LightingScript>.Instance.lightingState = this.lightingState;
				return;
			}
			Chat.NotPromotedErrorMessage("load");
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x0017D968 File Offset: 0x0017BB68
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.BackgroundSprite.enabled = false;
		}

		// Token: 0x04002AFC RID: 11004
		public LightingState lightingState;
	}

	// Token: 0x02000731 RID: 1841
	[Serializable]
	public class GridButtonMusic : UIGridMenu.GridButton
	{
		// Token: 0x06003DFD RID: 15869 RVA: 0x0017D97D File Offset: 0x0017BB7D
		public GridButtonMusic()
		{
			this.SpriteName = "VoiceSpeaker32";
			this.SpriteColor = Color.black;
			this.ButtonHoverColor = Colour.UIBlue;
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x0017D9AB File Offset: 0x0017BBAB
		public override void OnClick()
		{
			base.OnClick();
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<CustomMusicPlayer>.Instance.InitFromState(this.musicPlayerState);
				return;
			}
			Chat.NotPromotedErrorMessage("load");
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0017D968 File Offset: 0x0017BB68
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.BackgroundSprite.enabled = false;
		}

		// Token: 0x04002AFD RID: 11005
		public MusicPlayerState musicPlayerState;
	}

	// Token: 0x02000732 RID: 1842
	[Serializable]
	public abstract class GridButtonPath : UIGridMenu.GridButtonSpawn
	{
		// Token: 0x06003E00 RID: 15872 RVA: 0x0017D9DC File Offset: 0x0017BBDC
		public GridButtonPath()
		{
			this.OptionsPopupActions.Add("Delete", new Action(this.OnDelete));
			this.OptionsPopupActions.Add("Move", new Action(this.OnMove));
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0017DA34 File Offset: 0x0017BC34
		public virtual void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete {0}?", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0017DA63 File Offset: 0x0017BC63
		public virtual void Delete()
		{
			SerializationScript.Delete(this.Path);
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x0017DA71 File Offset: 0x0017BC71
		public virtual void OnMove()
		{
			UIDialog.ShowDropDown(Language.Translate("Move {0}?", this.Name), "Move", "Cancel", this.GridMenu.Folders, new Action<string>(this.Move), null, "");
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x0017DAB0 File Offset: 0x0017BCB0
		public virtual void Move(string localFolder)
		{
			this.movePath = SerializationScript.CombineLocalFolder(this.GridMenu.RootPath, localFolder, System.IO.Path.GetFileName(this.Path));
			if (SerializationScript.GetCleanPath(this.movePath) == SerializationScript.GetCleanPath(this.Path))
			{
				Chat.LogError(string.Format("Cannot move {0} to the same location.", this.Name), true);
				return;
			}
			if (File.Exists(this.movePath))
			{
				UIDialog.Show(Language.Translate("{0} already exists at this location. Move and overwrite?", this.Name), "Overwrite", "Cancel", new Action(this.DeleteMove), null);
				return;
			}
			SerializationScript.Move(this.Path, this.movePath);
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0017DB5F File Offset: 0x0017BD5F
		public virtual void DeleteMove()
		{
			SerializationScript.Delete(this.movePath);
			SerializationScript.Move(this.Path, this.movePath);
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0017DB80 File Offset: 0x0017BD80
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			if ((string.IsNullOrEmpty(this.GridMenu.RootPath) || this.GridMenu.Folders.Count <= 1) && this.OptionsPopupActions.ContainsKey("Move"))
			{
				this.OptionsPopupActions.Remove("Move");
			}
			base.UpdateButton(objectButton);
			objectButton.StartLoadThumbnail("file:///" + this.ThumbnailPath);
			base.dragTarget = objectButton.gameObject;
			if (this.UpdateTime != 0U)
			{
				objectButton.gameObject.AddMissingComponent<UITooltipScript>().DelayTooltip = SerializationScript.GetDateTime(this.UpdateTime).ToString();
			}
		}

		// Token: 0x04002AFE RID: 11006
		public string Path;

		// Token: 0x04002AFF RID: 11007
		public string ThumbnailPath;

		// Token: 0x04002B00 RID: 11008
		public uint UpdateTime;

		// Token: 0x04002B01 RID: 11009
		public uint LoadTime;

		// Token: 0x04002B02 RID: 11010
		public uint DownloadTime;

		// Token: 0x04002B03 RID: 11011
		[HideInInspector]
		private string movePath = "";
	}

	// Token: 0x02000733 RID: 1843
	[Serializable]
	public abstract class GridButtonPathFolder : UIGridMenu.GridButtonPath
	{
		// Token: 0x06003E07 RID: 15879 RVA: 0x0017DC29 File Offset: 0x0017BE29
		public GridButtonPathFolder()
		{
			this.Draggable = false;
			this.SpriteName = "Icon-Folder2";
			this.SpriteColor = Color.black;
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0017DC4E File Offset: 0x0017BE4E
		public override void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete {0}? (This will delete anything contained within this folder)", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}
	}

	// Token: 0x02000734 RID: 1844
	[Serializable]
	public class GridButtonSavedObject : UIGridMenu.GridButtonPath
	{
		// Token: 0x06003E09 RID: 15881 RVA: 0x0017DC7D File Offset: 0x0017BE7D
		public GridButtonSavedObject()
		{
			this.ButtonHoverColor = Colour.UIBlue;
			this.OptionsPopupActions.Add("Spawn", new Action(this.SpawnInPlace));
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0017DCB4 File Offset: 0x0017BEB4
		public void SpawnInPlace()
		{
			List<ObjectState> objectStates;
			if (!SerializationScript.FileIsJson(this.Path))
			{
				PhysicsState ps = SerializationScript.LoadCJC(this.Path, false)[0];
				ObjectState item = NetworkSingleton<ManagerPhysicsObject>.Instance.ObjectStateFromPhysicsState(ps);
				objectStates = new List<ObjectState>
				{
					item
				};
			}
			else
			{
				SaveState saveState = SerializationScript.LoadJson(this.Path, false);
				objectStates = saveState.ObjectStates;
				Vector3 vector = saveState.ObjectStates[0].Transform.Position();
				for (int i = 1; i < saveState.ObjectStates.Count; i++)
				{
					ObjectState objectState = saveState.ObjectStates[i];
					objectState.Transform.posX += vector.x;
					objectState.Transform.posY += vector.y;
					objectState.Transform.posZ += vector.z;
				}
			}
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjects(objectStates);
				return;
			}
			Chat.NotPromotedErrorMessage("load");
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0017D103 File Offset: 0x0017B303
		public override void OnClick()
		{
			if (PlayerScript.PointerScript)
			{
				PlayerScript.PointerScript.InteractiveSpawn(new SpawnDelegate(this.InteractiveSpawn), this.CreateVisualRepresentation());
				return;
			}
			this.Spawn(NetworkSingleton<NetworkUI>.Instance.SpawnPos);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0017DDC1 File Offset: 0x0017BFC1
		public override void Spawn(Vector3 SpawnPos)
		{
			if (PlayerScript.Pointer)
			{
				SpawnPos = PlayerScript.PointerScript.GetSpawnPosition(SpawnPos, true);
			}
			else
			{
				SpawnPos.y += 1f;
			}
			this.SpawnOffset(SpawnPos, true);
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0017DDF7 File Offset: 0x0017BFF7
		public override void InteractiveSpawn(Vector3 SpawnPos)
		{
			this.SpawnOffset(SpawnPos, false);
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0017DE04 File Offset: 0x0017C004
		private void SpawnOffset(Vector3 SpawnPos, bool bOffsetY)
		{
			if (!SerializationScript.FileIsJson(this.Path))
			{
				PhysicsState physicsState = SerializationScript.LoadCJC(this.Path, false)[0];
				physicsState.posX = SpawnPos.x;
				physicsState.posY = SpawnPos.y;
				physicsState.posZ = SpawnPos.z;
				if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnCJCObjectOffset(physicsState, bOffsetY);
				}
				else
				{
					Chat.NotPromotedErrorMessage("load");
				}
			}
			else
			{
				SaveState saveState = SerializationScript.LoadJson(this.Path, false);
				if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectsOffset(saveState.ObjectStates, SpawnPos, bOffsetY);
				}
				else
				{
					Chat.NotPromotedErrorMessage("load");
				}
			}
			SerializationScript.UpdateAccessTime(this.Path);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0017DEC0 File Offset: 0x0017C0C0
		public override ObjectVisualizer CreateVisualRepresentation()
		{
			if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				return null;
			}
			bool locked;
			List<GameObject> list;
			if (!SerializationScript.FileIsJson(this.Path))
			{
				PhysicsState physicsState = SerializationScript.LoadCJC(this.Path, false)[0];
				physicsState.posX = UIGridMenu.offscreenSpawn.x;
				physicsState.posY = UIGridMenu.offscreenSpawn.y;
				physicsState.posZ = UIGridMenu.offscreenSpawn.z;
				ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.ObjectStateFromPhysicsState(physicsState);
				locked = objectState.Locked;
				List<ObjectState> objectStates = new List<ObjectState>
				{
					objectState
				};
				list = NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectStatesOffset(objectStates, UIGridMenu.offscreenSpawn, false, true, false, true);
			}
			else
			{
				SaveState saveState = SerializationScript.LoadJson(this.Path, false);
				locked = saveState.ObjectStates[0].Locked;
				list = NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectStatesOffset(saveState.ObjectStates, UIGridMenu.offscreenSpawn, false, true, false, true);
			}
			Transform transform = new GameObject().transform;
			transform.position = UIGridMenu.offscreenSpawn;
			foreach (GameObject gameObject in list)
			{
				gameObject.transform.parent = transform;
				CustomObject component = gameObject.GetComponent<CustomObject>();
				if (component)
				{
					component.bCustomUI = false;
				}
			}
			return new ObjectVisualizer(transform.gameObject, locked);
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0017E024 File Offset: 0x0017C224
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.BackgroundSprite.enabled = false;
		}
	}

	// Token: 0x02000735 RID: 1845
	[Serializable]
	public class GridButtonFolderSavedObject : UIGridMenu.GridButtonPathFolder
	{
		// Token: 0x06003E11 RID: 15889 RVA: 0x0017E039 File Offset: 0x0017C239
		public GridButtonFolderSavedObject()
		{
			this.ButtonHoverColor = Colour.UIBlue;
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0017E051 File Offset: 0x0017C251
		public override void OnClick()
		{
			base.OnClick();
			this.GridMenu.GetComponent<UIGridMenuObjects>().LoadSavedObjects(this.Name, true, this.Path);
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0017E024 File Offset: 0x0017C224
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.BackgroundSprite.enabled = false;
		}
	}

	// Token: 0x02000736 RID: 1846
	[Serializable]
	public class GridButtonFolderWorkshop : UIGridMenu.GridButtonPathFolder
	{
		// Token: 0x06003E14 RID: 15892 RVA: 0x0017E076 File Offset: 0x0017C276
		public override void OnClick()
		{
			base.OnClick();
			NetworkSingleton<NetworkUI>.Instance.GUIGames.GetComponent<UIGridMenuGames>().OpenWorkshop(1, this.Path);
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0017E099 File Offset: 0x0017C299
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
		}
	}

	// Token: 0x02000737 RID: 1847
	[Serializable]
	public class GridButtonFolderSave : UIGridMenu.GridButtonPathFolder
	{
		// Token: 0x06003E17 RID: 15895 RVA: 0x0017E0AA File Offset: 0x0017C2AA
		public override void OnClick()
		{
			base.OnClick();
			NetworkSingleton<NetworkUI>.Instance.GUIGames.GetComponent<UIGridMenuGames>().OpenSaves(1, this.Path);
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x0017E099 File Offset: 0x0017C299
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
		}
	}

	// Token: 0x02000738 RID: 1848
	[Serializable]
	public abstract class GridButtonConfirmLoad : UIGridMenu.GridButtonPath
	{
		// Token: 0x06003E1A RID: 15898 RVA: 0x0017E0CD File Offset: 0x0017C2CD
		public GridButtonConfirmLoad()
		{
			this.Draggable = false;
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x0017E0E4 File Offset: 0x0017C2E4
		public override void OnClick()
		{
			if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				Chat.NotPromotedErrorMessage("load");
				return;
			}
			if (this.ConfirmLoad && !Singleton<SystemConsole>.Instance.confirmOveride)
			{
				UIDialog.Show(Language.Translate("Are you sure you want to load {0}?", this.Name), "Load", "Cancel", new Action(this.Click), null);
				return;
			}
			Singleton<SystemConsole>.Instance.confirmOveride = false;
			this.Click();
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x0017C672 File Offset: 0x0017A872
		public virtual void Click()
		{
			if (this.CloseMenu)
			{
				this.CloseMenu.SetActive(false);
			}
		}

		// Token: 0x04002B04 RID: 11012
		public bool ConfirmLoad = true;
	}

	// Token: 0x02000739 RID: 1849
	[Serializable]
	public abstract class GridButtonGamePath : UIGridMenu.GridButtonConfirmLoad
	{
		// Token: 0x06003E1D RID: 15901 RVA: 0x0017E15C File Offset: 0x0017C35C
		public GridButtonGamePath()
		{
			this.OptionsPopupActions.Add("Additive Load", new Action(this.OnAdditiveLoad));
			this.OptionsPopupActions.Add("Search", new Action(this.OnSearch));
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x0017E1A8 File Offset: 0x0017C3A8
		public override void Click()
		{
			base.Click();
			if (Network.isServer)
			{
				if (!SerializationScript.FileIsJson(this.Path))
				{
					SerializationScript.LoadCJC(this.Path, true);
				}
				else
				{
					SerializationScript.LoadJson(this.Path, true);
				}
			}
			else if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				if (!SerializationScript.FileIsJson(this.Path))
				{
					List<PhysicsState> listPS = SerializationScript.LoadCJC(this.Path, false);
					NetworkSingleton<ManagerPhysicsObject>.Instance.LoadPromotedPhysicsState(SerializationScript.GetBytes(listPS));
				}
				else
				{
					SaveState saveState = SerializationScript.LoadJson(this.Path, false);
					NetworkSingleton<ManagerPhysicsObject>.Instance.LoadPromotedSaveState(saveState);
				}
			}
			else
			{
				Chat.NotPromotedErrorMessage("load");
			}
			SerializationScript.UpdateAccessTime(this.Path);
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x0017E255 File Offset: 0x0017C455
		private void OnAdditiveLoad()
		{
			UIDialog.Show(Language.Translate("Are you sure you want to additively load {0}?", this.Name), "Load", "Cancel", new Action(this.AdditiveLoad), null);
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x0017E284 File Offset: 0x0017C484
		private void AdditiveLoad()
		{
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				List<ObjectState> list2;
				if (!SerializationScript.FileIsJson(this.Path))
				{
					List<PhysicsState> list = SerializationScript.LoadCJC(this.Path, false);
					list2 = new List<ObjectState>();
					for (int i = 1; i < list.Count; i++)
					{
						list2.Add(NetworkSingleton<ManagerPhysicsObject>.Instance.ObjectStateFromPhysicsState(list[i]));
					}
				}
				else
				{
					list2 = SerializationScript.LoadJson(this.Path, false).ObjectStates;
				}
				if (list2.Count > 0)
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjects(list2);
				}
			}
			else
			{
				Chat.NotPromotedErrorMessage("load");
			}
			SerializationScript.UpdateAccessTime(this.Path);
			if (this.CloseMenu)
			{
				this.CloseMenu.SetActive(false);
			}
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x0017E340 File Offset: 0x0017C540
		public virtual void OnSearch()
		{
			List<ObjectState> list = new List<ObjectState>();
			List<UIGridMenu.GridButton> list2 = new List<UIGridMenu.GridButton>();
			if (!SerializationScript.FileIsJson(this.Path))
			{
				List<PhysicsState> list3 = SerializationScript.LoadCJC(this.Path, false);
				List<ObjectState> list4 = new List<ObjectState>();
				for (int i = 1; i < list3.Count; i++)
				{
					ObjectState objectState = NetworkSingleton<ManagerPhysicsObject>.Instance.ObjectStateFromPhysicsState(list3[i]);
					if (!(objectState == null))
					{
						list4.Add(objectState);
					}
				}
				this.Cleanup(list4);
				list = ObjectState.RemoveDuplicates(list4);
				if (list3[0].StringList != null && list3[0].StringList.Count > 1 && !string.IsNullOrEmpty(list3[0].StringList[1]))
				{
					list2.Add(new UIGridMenu.GridButtonTable
					{
						Name = Utilities.CleanName(list3[0].StringList[0]),
						Table = list3[0].StringList[0],
						TableURL = list3[0].StringList[1]
					});
				}
			}
			else
			{
				SaveState saveState = SerializationScript.LoadJson(this.Path, false);
				List<ObjectState> objectStates = saveState.ObjectStates;
				this.Cleanup(objectStates);
				list = ObjectState.RemoveDuplicates(objectStates);
				if (!string.IsNullOrEmpty(saveState.SkyURL))
				{
					UIGridMenu.GridButtonBackground item = new UIGridMenu.GridButtonBackground
					{
						Name = "Custom Background",
						Background = saveState.Sky.Substring(4, saveState.Sky.Length - 4),
						BackgroundURL = saveState.SkyURL
					};
					list2.Add(item);
				}
				if (!string.IsNullOrEmpty(saveState.TableURL))
				{
					UIGridMenu.GridButtonTable item2 = new UIGridMenu.GridButtonTable
					{
						Name = Utilities.CleanName(saveState.Table),
						Table = saveState.Table,
						TableURL = saveState.TableURL
					};
					list2.Add(item2);
				}
				if (saveState.Lighting != null && saveState.Lighting != new LightingState())
				{
					UIGridMenu.GridButtonLighting item3 = new UIGridMenu.GridButtonLighting
					{
						Name = "Custom Lighting",
						lightingState = saveState.Lighting
					};
					list2.Add(item3);
				}
				if (saveState.MusicPlayer != null)
				{
					UIGridMenu.GridButtonMusic item4 = new UIGridMenu.GridButtonMusic
					{
						Name = "Music - " + saveState.MusicPlayer.CurrentAudioTitle,
						musicPlayerState = saveState.MusicPlayer
					};
					list2.Add(item4);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				ObjectState objectState2 = list[j];
				list2.Add(new UIGridMenu.GridButtonObjectState
				{
					Name = TTSUtilities.CleanName(objectState2),
					objectState = objectState2
				});
			}
			UIGridMenuGames component = NetworkSingleton<NetworkUI>.Instance.GUIGames.GetComponent<UIGridMenuGames>();
			component.ResetSearch(true);
			component.Open<UIGridMenu.GridButton>(this.Name, NetworkSingleton<NetworkUI>.Instance.GUIGames.GetComponent<UIGridMenuGames>().PrefabButtonLessText, delegate(int x, string y)
			{
			}, "", list2, new List<string>(), 1);
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x0017E668 File Offset: 0x0017C868
		private void Cleanup(List<ObjectState> objectStates)
		{
			for (int i = 0; i < objectStates.Count; i++)
			{
				ObjectState objectState = objectStates[i];
				if (objectState.Name == "3DText" || objectState.Name.Contains("Trigger"))
				{
					objectStates.Remove(objectState);
					i--;
				}
			}
		}
	}

	// Token: 0x0200073A RID: 1850
	[Serializable]
	public class GridButtonSave : UIGridMenu.GridButtonGamePath
	{
		// Token: 0x06003E23 RID: 15907 RVA: 0x0017E6BE File Offset: 0x0017C8BE
		public GridButtonSave()
		{
			this.OptionsPopupActions.Add("Overwrite", new Action(this.OnOverwrite));
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x0017E6E2 File Offset: 0x0017C8E2
		public override void Click()
		{
			base.Click();
			EventManager.TriggerUnityAnalytic("Save_Game_OnClick", null, 0);
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x0017E099 File Offset: 0x0017C299
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x0017E6F8 File Offset: 0x0017C8F8
		private void OnOverwrite()
		{
			if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				Chat.NotPromotedErrorMessage("overwrite");
				return;
			}
			UIDialog.ShowInput(Language.Translate("Overwrite {0}?", this.Name), "Overwrite", "Cancel", new Action<string>(this.Overwrite), null, this.SaveName, "Save Name");
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x0017E754 File Offset: 0x0017C954
		private void Overwrite(string name)
		{
			NetworkSingleton<NetworkUI>.Instance.GUISaveSlot(this.Path, name);
		}

		// Token: 0x04002B05 RID: 11013
		public string SaveName;

		// Token: 0x04002B06 RID: 11014
		public uint Slot;
	}

	// Token: 0x0200073B RID: 1851
	[Serializable]
	public class GridButtonWorkshop : UIGridMenu.GridButtonGamePath
	{
		// Token: 0x06003E28 RID: 15912 RVA: 0x0017E767 File Offset: 0x0017C967
		public GridButtonWorkshop()
		{
			this.OptionsPopupActions.Add("Info", new Action(this.OnInfo));
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x0017E78B File Offset: 0x0017C98B
		public override void Click()
		{
			base.Click();
			EventManager.TriggerUnityAnalytic("Workshop_Game_OnClick", null, 0);
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x0017E79F File Offset: 0x0017C99F
		public override void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete and Unsubscribe from {0}?", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x0017E7CE File Offset: 0x0017C9CE
		public override void Delete()
		{
			base.Delete();
			Singleton<SteamManager>.Instance.UnsubscribeFromId(this.WorkshopId);
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x0017E099 File Offset: 0x0017C299
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x0017E7E6 File Offset: 0x0017C9E6
		private void OnInfo()
		{
			TTSUtilities.OpenURL("http://steamcommunity.com/sharedfiles/filedetails/?id=" + this.WorkshopId.ToString());
		}

		// Token: 0x04002B07 RID: 11015
		public ulong WorkshopId;
	}

	// Token: 0x0200073C RID: 1852
	[Serializable]
	public class GridButtonDLC : UIGridMenu.GridButton
	{
		// Token: 0x06003E2E RID: 15918 RVA: 0x0017E802 File Offset: 0x0017CA02
		public GridButtonDLC()
		{
			this.OptionsPopupActions.Add("Store", new Action(this.OnStore));
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x0017E828 File Offset: 0x0017CA28
		public override void OnClick()
		{
			base.OnClick();
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				Singleton<DLCManager>.Instance.Load(this.LoadName);
			}
			else
			{
				Chat.NotPromotedErrorMessage("load");
			}
			EventManager.TriggerUnityAnalytic("DLC_Game_OnClick", "Game", this.Name, 0);
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x0017E87C File Offset: 0x0017CA7C
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.NewSprite.gameObject.SetActive(this.New);
			objectButton.LockSprite.gameObject.SetActive(this.Lock);
			objectButton.DiscountPercentLabel.gameObject.SetActive(this.DiscountPercent != 0);
			objectButton.DiscountPercentLabel.text = "-" + this.DiscountPercent.ToString() + "%";
			objectButton.StartLoadThumbnail(this.ThumbnailURL);
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x0017E906 File Offset: 0x0017CB06
		private void OnStore()
		{
			TTSUtilities.OpenURL("http://store.steampowered.com/app/" + this.AppId.ToString());
		}

		// Token: 0x04002B08 RID: 11016
		public string LoadName;

		// Token: 0x04002B09 RID: 11017
		public int AppId;

		// Token: 0x04002B0A RID: 11018
		public int DiscountPercent;

		// Token: 0x04002B0B RID: 11019
		public bool New;

		// Token: 0x04002B0C RID: 11020
		public bool Lock;

		// Token: 0x04002B0D RID: 11021
		public uint Purchased;

		// Token: 0x04002B0E RID: 11022
		public string ThumbnailURL;
	}

	// Token: 0x0200073D RID: 1853
	[Serializable]
	public class GridButtonClassic : UIGridMenu.GridButton
	{
		// Token: 0x06003E33 RID: 15923 RVA: 0x0017E924 File Offset: 0x0017CB24
		public override void OnClick()
		{
			base.OnClick();
			if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<NetworkUI>.Instance.GUIChangeGame(this.Name);
				PlayerPrefs.SetInt("LastPlayed" + this.Name, (int)SerializationScript.GetTimeFromEpoch(DateTime.UtcNow));
			}
			else
			{
				Chat.NotPromotedErrorMessage("load");
			}
			EventManager.TriggerUnityAnalytic("Classic_Game_OnClick", "Game", this.Name, 0);
		}

		// Token: 0x04002B0F RID: 11023
		[HideInInspector]
		[NonSerialized]
		public uint LastPlayed;
	}

	// Token: 0x0200073E RID: 1854
	[Serializable]
	public class GridButtonSpotlight : UIGridMenu.GridButton
	{
		// Token: 0x06003E34 RID: 15924 RVA: 0x0017E995 File Offset: 0x0017CB95
		public override void OnClick()
		{
			EventManager.TriggerUnityAnalytic("Spotlight_OnClick", "Name", this.Name, 0);
			TTSUtilities.OpenURL(this.ClickURL);
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x0017E9B8 File Offset: 0x0017CBB8
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
		}

		// Token: 0x04002B10 RID: 11024
		public string ClickURL;
	}

	// Token: 0x0200073F RID: 1855
	public class GridButtonDecal : UIGridMenu.GridButton
	{
		// Token: 0x06003E37 RID: 15927 RVA: 0x0017E9C4 File Offset: 0x0017CBC4
		public GridButtonDecal()
		{
			if (Network.isServer)
			{
				this.OptionsPopupActions.Add("Delete", new Action(this.OnDelete));
				this.OptionsPopupActions.Add("Edit", new Action(this.OnEdit));
			}
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x0017EA17 File Offset: 0x0017CC17
		public virtual void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete {0}?", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x0017EA45 File Offset: 0x0017CC45
		private void Delete()
		{
			NetworkSingleton<DecalManager>.Instance.RemoveDecalPallet(this.customDecal);
			this.GridMenu.Reload(true);
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x0017EA63 File Offset: 0x0017CC63
		private void OnEdit()
		{
			this.GridMenu.GetComponent<UIGridMenuDecals>().AddCustomDecalMenu.Override(this.customDecal);
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x0017EA80 File Offset: 0x0017CC80
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			objectButton.StartLoadThumbnail(this.customDecal.ImageURL);
			objectButton.BackgroundSprite.enabled = false;
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x0017EAA6 File Offset: 0x0017CCA6
		public override void OnClick()
		{
			base.OnClick();
			NetworkSingleton<DecalManager>.Instance.SelectedDecal = this.customDecal;
			this.GridMenu.Reload(true);
		}

		// Token: 0x04002B11 RID: 11025
		public CustomDecalState customDecal;
	}

	// Token: 0x02000740 RID: 1856
	public class GridButtonAsset : UIGridMenu.GridButton
	{
		// Token: 0x06003E3D RID: 15933 RVA: 0x0017EACC File Offset: 0x0017CCCC
		public GridButtonAsset()
		{
			this.OptionsPopupActions.Add("Delete", new Action(this.OnDelete));
			this.OptionsPopupActions.Add("Edit", new Action(this.OnEdit));
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x0017EB18 File Offset: 0x0017CD18
		public virtual void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete {0}?", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x0017EB46 File Offset: 0x0017CD46
		private void Delete()
		{
			this.GridMenu.GetComponent<UIGridMenuUIAssets>().targetXmlUI.RemoveCustomAsset(this.customAsset);
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x0017EB63 File Offset: 0x0017CD63
		private void OnEdit()
		{
			this.GridMenu.GetComponent<UIGridMenuUIAssets>().customUIAsset.Override(this.customAsset);
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x0017EB80 File Offset: 0x0017CD80
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			if (this.customAsset.Type == CustomAssetType.Image)
			{
				objectButton.StartLoadThumbnail(this.customAsset.URL);
			}
			objectButton.BackgroundSprite.enabled = false;
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x0017EBB3 File Offset: 0x0017CDB3
		public override void OnClick()
		{
			TTSUtilities.CopyToClipboard(this.Name);
			base.OnClick();
		}

		// Token: 0x04002B12 RID: 11026
		public CustomAssetState customAsset;
	}

	// Token: 0x02000741 RID: 1857
	public class GridButtonRotationValue : UIGridMenu.GridButton
	{
		// Token: 0x06003E43 RID: 15939 RVA: 0x0017EBC8 File Offset: 0x0017CDC8
		public GridButtonRotationValue()
		{
			this.ButtonHoverColor = Colour.UIBlue;
			this.OptionsPopupActions.Add("->", new Action(this.OnRight));
			this.OptionsPopupActions.Add("<-", new Action(this.OnLeft));
			this.OptionsPopupActions.Add("Delete", new Action(this.OnDelete));
			this.OptionsPopupActions.Add("Edit", new Action(this.OnEdit));
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x0017EC63 File Offset: 0x0017CE63
		public virtual void OnDelete()
		{
			UIDialog.Show(Language.Translate("Delete {0}?", this.Name), "Delete", "Cancel", new Action(this.Delete), null);
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x0017EC91 File Offset: 0x0017CE91
		private void Delete()
		{
			this.NPO.RotationValues.RemoveAt(this.index);
			this.NPO.SetRotationValues(this.NPO.RotationValues);
			this.GridMenu.Reload(true);
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x0017ECCB File Offset: 0x0017CECB
		private void OnEdit()
		{
			this.GridMenu.GetComponent<UIGridMenuRotationValue>().RotationValueAddMenu.Begin(this.NPO, this.index);
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x0017ECF0 File Offset: 0x0017CEF0
		private void OnLeft()
		{
			int num = this.index - 1;
			if (num == -1)
			{
				num = this.GridMenu.currentButtons.Count - 1;
			}
			this.SwapWith((UIGridMenu.GridButtonRotationValue)this.GridMenu.currentButtons[num]);
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x0017ED3C File Offset: 0x0017CF3C
		private void OnRight()
		{
			int num = (this.index + 1) % this.GridMenu.currentButtons.Count;
			this.SwapWith((UIGridMenu.GridButtonRotationValue)this.GridMenu.currentButtons[num]);
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x0017ED80 File Offset: 0x0017CF80
		private void SwapWith(UIGridMenu.GridButtonRotationValue otherButton)
		{
			RotationValue value = this.NPO.RotationValues[this.index];
			this.NPO.RotationValues[this.index] = this.NPO.RotationValues[otherButton.index];
			this.NPO.RotationValues[otherButton.index] = value;
			this.NPO.SetRotationValues(this.NPO.RotationValues);
			this.GridMenu.Reload(true);
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x0017EE09 File Offset: 0x0017D009
		public override void OnClick()
		{
			this.NPO.SetRotation(this.NPO.RotationValues[this.index].rotation);
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x0017EE34 File Offset: 0x0017D034
		public override void UpdateButton(UIGridMenuButton objectButton)
		{
			base.UpdateButton(objectButton);
			ObjectState os = NetworkSingleton<ManagerPhysicsObject>.Instance.SaveObjectState(this.NPO);
			RotationValue rotationValue = this.NPO.RotationValues[this.index];
			GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.LoadObjectState(os, true, false);
			if (gameObject == null)
			{
				return;
			}
			objectButton.BackgroundSprite.gameObject.SetActive(false);
			objectButton.SetSpawnedObject(gameObject, 95f, new Vector3?(rotationValue.rotation));
		}

		// Token: 0x04002B13 RID: 11027
		public int index = -1;

		// Token: 0x04002B14 RID: 11028
		public NetworkPhysicsObject NPO;
	}

	// Token: 0x02000742 RID: 1858
	public class GridMenuState
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x0017EEB0 File Offset: 0x0017D0B0
		public GridMenuState(string title, List<UIGridMenu.GridButton> buttons, int page, string search, bool applyDefaultTheme)
		{
			this.title = title;
			this.buttons = buttons;
			this.page = page;
			this.search = search;
			this.applyDefaultTheme = applyDefaultTheme;
		}

		// Token: 0x04002B15 RID: 11029
		public string title;

		// Token: 0x04002B16 RID: 11030
		public List<UIGridMenu.GridButton> buttons;

		// Token: 0x04002B17 RID: 11031
		public int page;

		// Token: 0x04002B18 RID: 11032
		public string search;

		// Token: 0x04002B19 RID: 11033
		public bool applyDefaultTheme;
	}
}
