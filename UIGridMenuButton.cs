using System;
using System.IO;
using UnityEngine;

// Token: 0x020002D5 RID: 725
public class UIGridMenuButton : MonoBehaviour
{
	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06002368 RID: 9064 RVA: 0x000FAE54 File Offset: 0x000F9054
	// (set) Token: 0x06002369 RID: 9065 RVA: 0x000FAE5C File Offset: 0x000F905C
	public GameObject SpawnedGameObject { get; private set; }

	// Token: 0x0600236A RID: 9066 RVA: 0x000FAE68 File Offset: 0x000F9068
	private void Awake()
	{
		EventManager.OnDummyObjectFinish += this.FitToButton;
		UIEventListener uieventListener = UIEventListener.Get(this.PopupList.gameObject);
		uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Combine(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScrollPopupList));
		this.ThumbnailTextureWidth = this.ThumbnailTexture.width;
		this.ThumbnailTextureHeight = this.ThumbnailTexture.height;
		this.NameLabel.DoNotInitTheme();
		this.NameLabel.ThemeAs = UIPalette.UI.DoNotTheme;
		this.NameLabel.ThemeAsSetting = UIPalette.UI.DoNotTheme;
		this.ParentSpawnGameObject = new GameObject();
		this.ParentSpawnGameObject.transform.parent = base.transform;
		this.ParentSpawnGameObject.layer = base.gameObject.layer;
		this.ParentSpawnGameObject.transform.Reset();
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000FAF44 File Offset: 0x000F9144
	private void OnDisable()
	{
		bool flag = this.isThumbnailLoading;
		this.CleanupThumbnail();
		this.isThumbnailLoading = flag;
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000FAF68 File Offset: 0x000F9168
	private void OnDestroy()
	{
		this.CleanupThumbnail();
		EventManager.OnDummyObjectFinish -= this.FitToButton;
		UIEventListener uieventListener = UIEventListener.Get(this.PopupList.gameObject);
		uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Remove(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScrollPopupList));
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000FAFC0 File Offset: 0x000F91C0
	public void Cleanup()
	{
		this.onClick = null;
		this.onOptionsPopup = null;
		this.onDragStart = null;
		this.onDrag = null;
		this.onDragEnd = null;
		this.onScroll = null;
		this.TopLeftLabel.text = "";
		this.DelayThumbnailCleanup = true;
		this.ThumbnailSprite.enabled = false;
		this.QuestionMarkSprite.enabled = true;
		this.HoverEnable.enabled = false;
		this.BackgroundSprite.enabled = true;
		this.BackgroundSprite.gameObject.SetActive(true);
		this.LockSprite.gameObject.SetActive(false);
		this.NewSprite.gameObject.SetActive(false);
		this.DiscountPercentLabel.gameObject.SetActive(false);
		this.OptionsButton.gameObject.SetActive(false);
		this.ParentSpawnGameObject.transform.Reset();
		this.ThumbnailTexture.width = this.ThumbnailTextureWidth;
		this.ThumbnailTexture.height = this.ThumbnailTextureHeight;
		this.CleanupThumbnail();
		this.CleanupGameObject();
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000FB0D1 File Offset: 0x000F92D1
	public void CleanupGameObject()
	{
		UnityEngine.Object.Destroy(this.SpawnedGameObject);
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000FB0E0 File Offset: 0x000F92E0
	public void CleanupThumbnail()
	{
		if (Singleton<CustomLoadingManager>.Instance)
		{
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this.CurrentThumbnailPath, new Action<CustomTextureContainer>(this.SetupThumbnail), this.DelayThumbnailCleanup);
		}
		this.isThumbnailLoading = false;
		this.CurrentThumbnailPath = null;
		this.NextThumbnailPath = null;
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000FB138 File Offset: 0x000F9338
	public void StartLoadThumbnail(string URL)
	{
		if (string.IsNullOrEmpty(URL))
		{
			return;
		}
		if (DataRequest.IsLocalFile(URL) && !File.Exists(URL.Replace("file:///", "")))
		{
			return;
		}
		if (this.isThumbnailLoading)
		{
			this.NextThumbnailPath = URL;
			return;
		}
		this.CleanupThumbnail();
		this.isThumbnailLoading = true;
		this.CurrentThumbnailPath = URL;
		Singleton<CustomLoadingManager>.Instance.Texture.Load(URL, new Action<CustomTextureContainer>(this.SetupThumbnail), true, false, false, true, false, false, 8192, CustomLoadingManager.LoadType.Auto);
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000FB1BC File Offset: 0x000F93BC
	public void SetupThumbnail(CustomTextureContainer textureContainer)
	{
		if (textureContainer.nonCodeStrippedURL == this.CurrentThumbnailPath)
		{
			this.LoadedThumbnailTexture = textureContainer.texture;
			this.ThumbnailTexture.mainTexture = this.LoadedThumbnailTexture;
			this.QuestionMarkSprite.enabled = false;
			if (textureContainer.aspectRatio < 1f)
			{
				this.ThumbnailTexture.width = (int)((float)this.ThumbnailTextureWidth * textureContainer.aspectRatio);
			}
			else if (textureContainer.aspectRatio > 1f)
			{
				this.ThumbnailTexture.height = (int)((float)this.ThumbnailTextureHeight / textureContainer.aspectRatio);
			}
		}
		else
		{
			Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(textureContainer.nonCodeStrippedURL, new Action<CustomTextureContainer>(this.SetupThumbnail), this.DelayThumbnailCleanup);
		}
		this.isThumbnailLoading = false;
		if (!string.IsNullOrEmpty(this.NextThumbnailPath))
		{
			this.StartLoadThumbnail(this.NextThumbnailPath);
		}
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000FB29E File Offset: 0x000F949E
	private void OnClick()
	{
		if (UICamera.currentKey == KeyCode.Mouse1)
		{
			if (this.onRightClick != null)
			{
				this.onRightClick();
				return;
			}
		}
		else if (this.onClick != null)
		{
			this.onClick();
		}
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x000FB2D3 File Offset: 0x000F94D3
	public void OnOptionsPopup(string selection)
	{
		if (this.onOptionsPopup != null)
		{
			this.onOptionsPopup(selection);
		}
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x000FB2EC File Offset: 0x000F94EC
	public void SetSpawnedObject(GameObject GO, float Size = 95f, Vector3? RotationTarget = null)
	{
		if (this.SpawnedGameObject)
		{
			this.CleanupGameObject();
		}
		this.SpawnedGameObject = GO;
		this.initalScaleSpawned = this.SpawnedGameObject.transform.localScale;
		this.SpawnObjectSize = Size;
		this.SpawnObjectRotationTarget = RotationTarget;
		this.FitToButton(this.SpawnedGameObject);
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x000FB344 File Offset: 0x000F9544
	private void FitToButton(GameObject GO)
	{
		if (GO != this.SpawnedGameObject)
		{
			return;
		}
		this.SpawnedGameObject.transform.localScale = this.initalScaleSpawned;
		NGUIHelper.FitGameObjectToUI(GO, this.ParentSpawnGameObject.transform, this.BackgroundSprite.transform.localPosition, this.initalScaleSpawned, this.SpawnObjectSize, this.SpawnObjectRotationTarget, null);
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x000FB3B2 File Offset: 0x000F95B2
	private void OnDragStart()
	{
		if (this.onDragStart != null)
		{
			this.onDragStart();
		}
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x000FB3C7 File Offset: 0x000F95C7
	private void OnDrag()
	{
		if (this.onDrag != null)
		{
			this.onDrag();
		}
	}

	// Token: 0x06002378 RID: 9080 RVA: 0x000FB3DC File Offset: 0x000F95DC
	private void OnDragEnd()
	{
		if (this.onDragEnd != null)
		{
			this.onDragEnd();
		}
	}

	// Token: 0x06002379 RID: 9081 RVA: 0x000FB3F1 File Offset: 0x000F95F1
	private void OnScroll(float delta)
	{
		if (this.onScroll != null)
		{
			this.onScroll(delta);
		}
	}

	// Token: 0x0600237A RID: 9082 RVA: 0x000FB407 File Offset: 0x000F9607
	private void OnScrollPopupList(GameObject go, float delta)
	{
		if (go == this.PopupList.gameObject)
		{
			this.OnScroll(delta);
		}
	}

	// Token: 0x04001675 RID: 5749
	public UIButton MainButton;

	// Token: 0x04001676 RID: 5750
	public UILabel NameLabel;

	// Token: 0x04001677 RID: 5751
	public UILabel TopLeftLabel;

	// Token: 0x04001678 RID: 5752
	public UITexture ThumbnailTexture;

	// Token: 0x04001679 RID: 5753
	public UIButton OptionsButton;

	// Token: 0x0400167A RID: 5754
	public UISprite SpriteButton;

	// Token: 0x0400167B RID: 5755
	public UISprite BackgroundSprite;

	// Token: 0x0400167C RID: 5756
	public UISprite QuestionMarkSprite;

	// Token: 0x0400167D RID: 5757
	public UISprite LockSprite;

	// Token: 0x0400167E RID: 5758
	public UISprite NewSprite;

	// Token: 0x0400167F RID: 5759
	public UISprite ThumbnailSprite;

	// Token: 0x04001680 RID: 5760
	public UILabel DiscountPercentLabel;

	// Token: 0x04001681 RID: 5761
	public UIPopupList PopupList;

	// Token: 0x04001682 RID: 5762
	public UIHoverEnableObjects HoverEnable;

	// Token: 0x04001683 RID: 5763
	[NonSerialized]
	public Action onClick;

	// Token: 0x04001684 RID: 5764
	[NonSerialized]
	public Action onRightClick;

	// Token: 0x04001685 RID: 5765
	[NonSerialized]
	public Action<string> onOptionsPopup;

	// Token: 0x04001686 RID: 5766
	[NonSerialized]
	public Action onDragStart;

	// Token: 0x04001687 RID: 5767
	[NonSerialized]
	public Action onDrag;

	// Token: 0x04001688 RID: 5768
	[NonSerialized]
	public Action onDragEnd;

	// Token: 0x04001689 RID: 5769
	[NonSerialized]
	public Action<float> onScroll;

	// Token: 0x0400168B RID: 5771
	private Vector3 initalScaleSpawned;

	// Token: 0x0400168C RID: 5772
	private Texture LoadedThumbnailTexture;

	// Token: 0x0400168D RID: 5773
	private bool isThumbnailLoading;

	// Token: 0x0400168E RID: 5774
	private string CurrentThumbnailPath;

	// Token: 0x0400168F RID: 5775
	private string NextThumbnailPath;

	// Token: 0x04001690 RID: 5776
	public bool DelayThumbnailCleanup = true;

	// Token: 0x04001691 RID: 5777
	private int ThumbnailTextureWidth;

	// Token: 0x04001692 RID: 5778
	private int ThumbnailTextureHeight;

	// Token: 0x04001693 RID: 5779
	private GameObject ParentSpawnGameObject;

	// Token: 0x04001694 RID: 5780
	private float SpawnObjectSize = 95f;

	// Token: 0x04001695 RID: 5781
	private Vector3? SpawnObjectRotationTarget;
}
