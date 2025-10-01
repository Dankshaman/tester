using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class UISpectatorView : Singleton<UISpectatorView>
{
	// Token: 0x0600277D RID: 10109 RVA: 0x001189EC File Offset: 0x00116BEC
	protected override void Awake()
	{
		base.Awake();
		this.OnThemeChange();
		EventManager.OnUIThemeChange += this.OnThemeChange;
		EventManager.OnBlindfold += this.OnBlindfold;
		this.panelObjects.Add(base.gameObject);
		foreach (Transform transform in base.transform.GetComponentsInChildren<Transform>())
		{
			this.panelObjects.Add(transform.gameObject);
		}
		for (int j = 0; j < this.ResizePanels.Length; j++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.ResizePanels[j].gameObject);
			uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener.onDrag, new UIEventListener.VectorDelegate(this.OnResize));
		}
		UIEventListener uieventListener2 = UIEventListener.Get(this.SpectatorTexture.gameObject);
		uieventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener2.onDrag, new UIEventListener.VectorDelegate(this.OnMove));
		for (int k = 0; k < this.MoveHandles.Length; k++)
		{
			this.MoveHandles[k].DoTweenIn = false;
		}
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x00118B04 File Offset: 0x00116D04
	protected void OnDestroy()
	{
		EventManager.OnUIThemeChange -= this.OnThemeChange;
		EventManager.OnBlindfold -= this.OnBlindfold;
		for (int i = 0; i < this.ResizePanels.Length; i++)
		{
			UIEventListener uieventListener = UIEventListener.Get(this.ResizePanels[i].gameObject);
			uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener.onDrag, new UIEventListener.VectorDelegate(this.OnResize));
		}
		UIEventListener uieventListener2 = UIEventListener.Get(this.SpectatorTexture.gameObject);
		uieventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener2.onDrag, new UIEventListener.VectorDelegate(this.OnMove));
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x00118BAC File Offset: 0x00116DAC
	private void OnEnable()
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(NetworkID.ID);
		this.SpectatorTexture.gameObject.SetActive(!playerState.blind);
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x00118BE4 File Offset: 0x00116DE4
	private void OnThemeChange()
	{
		float a = this.panelOutColour.a;
		this.panelInColour = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.WindowBackground];
		this.panelOutColour = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.WindowBackground];
		this.panelOutColour.a = a;
		if (this.fadeState == UISpectatorView.FadeState.In || this.fadeState == UISpectatorView.FadeState.Out)
		{
			this.UpdateOpacity(this.progress);
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06002781 RID: 10113 RVA: 0x00118C54 File Offset: 0x00116E54
	private bool blindfolded
	{
		get
		{
			return !this.SpectatorTexture.gameObject.activeSelf;
		}
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x00118C69 File Offset: 0x00116E69
	private void OnBlindfold(bool bBlind, int id)
	{
		if (id == NetworkID.ID)
		{
			this.SpectatorTexture.gameObject.SetActive(!bBlind);
		}
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x00118C88 File Offset: 0x00116E88
	private void UpdatePanelColour(Colour colour)
	{
		for (int i = 0; i < this.ToFade.Length; i++)
		{
			this.ToFade[i].color = colour;
		}
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x00118CBC File Offset: 0x00116EBC
	public RenderTexture Init(int x, int y, int width, int height, bool overlayButtons, bool sizingCorners, bool movable, bool hideOnHover)
	{
		this.SpectatorRenderTexture = new RenderTexture(width, height, 24);
		this.SpectatorTexture.mainTexture = this.SpectatorRenderTexture;
		this.SpectatorTexture.width = this.SpectatorRenderTexture.width;
		this.SpectatorTexture.height = this.SpectatorRenderTexture.height;
		this.UpdatePanelColour(Colour.Clear);
		this.SpectatorTexture.color = Colour.Clear;
		this.fadeState = UISpectatorView.FadeState.Out;
		Wait.Frames(new Action(this.MakeVisible), 1);
		this.SetPanelOutAlpha(UISpectatorView.PANEL_ALPHA);
		this.SetTextureOutAlpha(UISpectatorView.TEXTURE_ALPHA);
		this.OverlayButtonsActive = overlayButtons;
		this.SizingCornersActive = sizingCorners;
		this.Movable = movable;
		this.HideOnHover = hideOnHover;
		base.gameObject.SetActive(true);
		this.desiredPosition = new Vector3?(new Vector3((float)x, (float)y, 0f));
		base.transform.localPosition = this.desiredPosition.Value;
		this.UpdateIcons(true);
		this.needToStorePrefs = true;
		return this.SpectatorRenderTexture;
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x00118DD8 File Offset: 0x00116FD8
	public void Init(bool overlayButtons, bool sizingCorners, bool movable, bool hideOnHover)
	{
		int @int = PlayerPrefs.GetInt("SpectatorPanelX", 0);
		int int2 = PlayerPrefs.GetInt("SpectatorPanelY", 0);
		int int3 = PlayerPrefs.GetInt("SpectatorPanelW", 400);
		int int4 = PlayerPrefs.GetInt("SpectatorPanelH", 400);
		this.Init(@int, int2, int3, int4, overlayButtons, sizingCorners, movable, hideOnHover);
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x00118E30 File Offset: 0x00117030
	public int[] GetPanelPrefs()
	{
		return new int[]
		{
			PlayerPrefs.GetInt("SpectatorPanelX", 0),
			PlayerPrefs.GetInt("SpectatorPanelY", 0),
			PlayerPrefs.GetInt("SpectatorPanelW", 400),
			PlayerPrefs.GetInt("SpectatorPanelH", 400)
		};
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06002787 RID: 10119 RVA: 0x00118E83 File Offset: 0x00117083
	// (set) Token: 0x06002788 RID: 10120 RVA: 0x00118E97 File Offset: 0x00117097
	public bool OverlayButtonsActive
	{
		get
		{
			return this.ToFade[4].gameObject.activeSelf;
		}
		set
		{
			if (this.OverlayButtonsActive == value)
			{
				return;
			}
			this.ToFade[4].gameObject.SetActive(value);
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06002789 RID: 10121 RVA: 0x00118EB6 File Offset: 0x001170B6
	// (set) Token: 0x0600278A RID: 10122 RVA: 0x00118ECC File Offset: 0x001170CC
	public bool SizingCornersActive
	{
		get
		{
			return this.ResizePanels[0].gameObject.activeSelf;
		}
		set
		{
			if (this.SizingCornersActive == value)
			{
				return;
			}
			for (int i = 0; i < this.ResizePanels.Length; i++)
			{
				this.ResizePanels[i].gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x0600278B RID: 10123 RVA: 0x00118F09 File Offset: 0x00117109
	// (set) Token: 0x0600278C RID: 10124 RVA: 0x00118F20 File Offset: 0x00117120
	public bool Movable
	{
		get
		{
			return this.DisableForLocked[0].gameObject.activeSelf;
		}
		set
		{
			for (int i = 0; i < this.DisableForLocked.Length; i++)
			{
				this.DisableForLocked[i].gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x00118F54 File Offset: 0x00117154
	private void SetPrefs()
	{
		PlayerPrefs.SetInt("SpectatorPanelX", (int)base.transform.localPosition.x);
		PlayerPrefs.SetInt("SpectatorPanelY", (int)base.transform.localPosition.y);
		PlayerPrefs.SetInt("SpectatorPanelW", this.SpectatorTexture.width);
		PlayerPrefs.SetInt("SpectatorPanelH", this.SpectatorTexture.height);
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x00118FC1 File Offset: 0x001171C1
	private void OnMove(GameObject go, Vector2 vec2)
	{
		this.needToStorePrefs = true;
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x00118FCC File Offset: 0x001171CC
	private void OnResize(GameObject go, Vector2 vec2)
	{
		this.hoverUntil = Time.time + 0.5f;
		this.SpectatorTexture.width += (int)vec2.x;
		this.SpectatorTexture.height += (int)vec2.y;
		this.SpectatorRenderTexture.Release();
		this.SpectatorRenderTexture = new RenderTexture(this.SpectatorTexture.width, this.SpectatorTexture.height, 24);
		this.SpectatorTexture.mainTexture = this.SpectatorRenderTexture;
		Singleton<SpectatorCamera>.Instance.ReassignRenderTexture(this.SpectatorRenderTexture);
		this.needToStorePrefs = true;
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x00119072 File Offset: 0x00117272
	public void OnClose()
	{
		this.SetPrefs();
		Singleton<SpectatorCamera>.Instance.TurnOff();
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x00119084 File Offset: 0x00117284
	public void OnCopyPlayerView()
	{
		Singleton<SpectatorCamera>.Instance.CopyPlayerCamera();
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x00119090 File Offset: 0x00117290
	public void OnLockToPlayerView()
	{
		Singleton<SpectatorCamera>.Instance.lockedToPlayer = !Singleton<SpectatorCamera>.Instance.lockedToPlayer;
		this.UpdateIcons(false);
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x001190B0 File Offset: 0x001172B0
	public void OnRestrictViewToggle()
	{
		Singleton<SpectatorCamera>.Instance.RestrictView = !Singleton<SpectatorCamera>.Instance.RestrictView;
		this.UpdateIcons(false);
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x001190D0 File Offset: 0x001172D0
	public void UpdateIcons(bool force = false)
	{
		if (Singleton<SpectatorCamera>.Instance.lockedToPlayer != this.isLockedToPlayer || force)
		{
			this.isLockedToPlayer = Singleton<SpectatorCamera>.Instance.lockedToPlayer;
			if (Singleton<SpectatorCamera>.Instance.lockedToPlayer)
			{
				this.LockButtonSprite.spriteName = "Icon-Lock-Closed";
				UITooltipScript tooltip = this.LockButtonSprite.transform.parent.GetComponent<UITooltipScript>();
				tooltip.enabled = false;
				tooltip.Tooltip = "Locked to player view";
				Wait.Frames(delegate
				{
					tooltip.enabled = true;
				}, 3);
			}
			else
			{
				this.LockButtonSprite.spriteName = "Icon-Lock-Open-Outline";
				UITooltipScript tooltip = this.LockButtonSprite.transform.parent.GetComponent<UITooltipScript>();
				tooltip.enabled = false;
				tooltip.Tooltip = "Unlocked from player view";
				Wait.Frames(delegate
				{
					tooltip.enabled = true;
				}, 3);
			}
		}
		if (Singleton<SpectatorCamera>.Instance.RestrictView != this.isViewRestricted || force)
		{
			this.isViewRestricted = Singleton<SpectatorCamera>.Instance.RestrictView;
			UITooltipScript tooltip;
			if (Singleton<SpectatorCamera>.Instance.RestrictView)
			{
				this.RestrictButtonSprite.spriteName = "Icon-Blindfold";
				UITooltipScript tooltip = this.RestrictButtonSprite.transform.parent.GetComponent<UITooltipScript>();
				tooltip.enabled = false;
				tooltip.Tooltip = "View restricted to what spectators see.";
				Wait.Frames(delegate
				{
					tooltip.enabled = true;
				}, 3);
				return;
			}
			this.RestrictButtonSprite.spriteName = "Icon-EyeBall";
			tooltip = this.RestrictButtonSprite.transform.parent.GetComponent<UITooltipScript>();
			tooltip.enabled = false;
			tooltip.Tooltip = "Unrestricted view.";
			Wait.Frames(delegate
			{
				tooltip.enabled = true;
			}, 3);
		}
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x001192C8 File Offset: 0x001174C8
	public void MakeVisible()
	{
		this.fadeState = UISpectatorView.FadeState.Out;
		this.progress = 0f;
		this.hoverUntil = 0f;
		this.UpdateOpacity(0f);
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x001192F4 File Offset: 0x001174F4
	private void Update()
	{
		this.SpectatorTexture.transform.localScale = Vector3.one * NetworkSingleton<ManagerPhysicsObject>.Instance.UIResolutionScale();
		this.UpdateIcons(false);
		if (Time.time >= this.nextCheckStorePrefs)
		{
			if (this.needToStorePrefs)
			{
				this.SetPrefs();
				this.needToStorePrefs = false;
			}
			this.nextCheckStorePrefs = Time.time + 5f;
		}
		if (this.desiredPosition != null)
		{
			base.transform.localPosition = this.desiredPosition.Value;
			this.desiredPosition = null;
		}
		bool flag = this.panelObjects.Contains(UICamera.HoveredUIObject) || HoverScript.HoverCamera == Singleton<CameraManager>.Instance.SpectatorCamera;
		if (!this.HideOnHover)
		{
			if (flag)
			{
				this.hoverUntil = Time.time + 0.5f;
			}
			this.CheckFade(flag);
		}
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x001193DB File Offset: 0x001175DB
	public void SetTextureOutAlpha(float alpha)
	{
		this.textureOutColour.a = alpha;
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x001193E9 File Offset: 0x001175E9
	public void SetPanelOutAlpha(float alpha)
	{
		this.panelOutColour.a = alpha;
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x001193F8 File Offset: 0x001175F8
	private void CheckFade(bool hovering)
	{
		if (this.hoverUntil > Time.time)
		{
			hovering = true;
		}
		if (this.blindfolded)
		{
			hovering = false;
		}
		switch (this.fadeState)
		{
		case UISpectatorView.FadeState.Out:
			if (hovering)
			{
				this.fadeState = UISpectatorView.FadeState.FadingIn;
				return;
			}
			break;
		case UISpectatorView.FadeState.FadingIn:
			if (!hovering)
			{
				this.fadeState = UISpectatorView.FadeState.FadingOut;
				return;
			}
			this.progress += Time.deltaTime / 0.2f;
			if (this.progress >= 1f)
			{
				this.progress = 1f;
				this.fadeState = UISpectatorView.FadeState.In;
			}
			this.UpdateOpacity(this.progress);
			return;
		case UISpectatorView.FadeState.FadingOut:
			if (hovering)
			{
				this.fadeState = UISpectatorView.FadeState.FadingIn;
				return;
			}
			this.progress -= Time.deltaTime / 0.2f;
			if (this.progress <= 0f)
			{
				this.progress = 0f;
				this.fadeState = UISpectatorView.FadeState.Out;
			}
			this.UpdateOpacity(this.progress);
			break;
		case UISpectatorView.FadeState.In:
			if (!hovering)
			{
				this.fadeState = UISpectatorView.FadeState.FadingOut;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600279A RID: 10138 RVA: 0x001194F7 File Offset: 0x001176F7
	private void UpdateOpacity(float opacity)
	{
		this.UpdatePanelColour(Colour.Lerp(this.panelOutColour, this.panelInColour, opacity * opacity));
		this.SpectatorTexture.color = Colour.Lerp(this.textureOutColour, this.textureInColour, opacity);
	}

	// Token: 0x0600279B RID: 10139 RVA: 0x00119538 File Offset: 0x00117738
	private bool PointOnTexture(Vector2 point, out Vector2 position)
	{
		Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.SpectatorTexture.transform);
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

	// Token: 0x040019EB RID: 6635
	public static float PANEL_ALPHA = 0f;

	// Token: 0x040019EC RID: 6636
	public static float TEXTURE_ALPHA = 1f;

	// Token: 0x040019ED RID: 6637
	private const float FADE_DURATION = 0.2f;

	// Token: 0x040019EE RID: 6638
	private const float HOVER_DELAY = 0.5f;

	// Token: 0x040019EF RID: 6639
	private const float CHECK_STORE_PREFS_THROTTLE = 5f;

	// Token: 0x040019F0 RID: 6640
	public UITexture SpectatorTexture;

	// Token: 0x040019F1 RID: 6641
	public UISprite[] ResizePanels;

	// Token: 0x040019F2 RID: 6642
	public UISprite[] ToFade;

	// Token: 0x040019F3 RID: 6643
	public RenderTexture SpectatorRenderTexture;

	// Token: 0x040019F4 RID: 6644
	public UISprite LockButtonSprite;

	// Token: 0x040019F5 RID: 6645
	public UISprite RestrictButtonSprite;

	// Token: 0x040019F6 RID: 6646
	public bool HideOnHover;

	// Token: 0x040019F7 RID: 6647
	public UIDragObject[] MoveHandles;

	// Token: 0x040019F8 RID: 6648
	public GameObject[] DisableForLocked;

	// Token: 0x040019F9 RID: 6649
	public UISpectatorView.FadeState fadeState;

	// Token: 0x040019FA RID: 6650
	private float progress;

	// Token: 0x040019FB RID: 6651
	private float hoverUntil;

	// Token: 0x040019FC RID: 6652
	private Colour panelInColour = Colour.White;

	// Token: 0x040019FD RID: 6653
	private Colour panelOutColour = Colour.White;

	// Token: 0x040019FE RID: 6654
	private Colour textureInColour = Colour.White;

	// Token: 0x040019FF RID: 6655
	private Colour textureOutColour = Colour.White;

	// Token: 0x04001A00 RID: 6656
	private bool needToStorePrefs;

	// Token: 0x04001A01 RID: 6657
	private float nextCheckStorePrefs;

	// Token: 0x04001A02 RID: 6658
	private Vector3? desiredPosition;

	// Token: 0x04001A03 RID: 6659
	private List<GameObject> panelObjects = new List<GameObject>();

	// Token: 0x04001A04 RID: 6660
	private bool isLockedToPlayer;

	// Token: 0x04001A05 RID: 6661
	private bool isViewRestricted;

	// Token: 0x0200078D RID: 1933
	public enum FadeState
	{
		// Token: 0x04002C9C RID: 11420
		Out,
		// Token: 0x04002C9D RID: 11421
		FadingIn,
		// Token: 0x04002C9E RID: 11422
		FadingOut,
		// Token: 0x04002C9F RID: 11423
		In
	}
}
