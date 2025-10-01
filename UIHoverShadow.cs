using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class UIHoverShadow : MonoBehaviour
{
	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x0600245D RID: 9309 RVA: 0x00100A00 File Offset: 0x000FEC00
	public UISprite DropShadow
	{
		get
		{
			if (this.backgroundSprite == null)
			{
				this.backgroundSprite = base.GetComponent<UISprite>();
			}
			if (this._dropShadowSprite == null)
			{
				this._dropShadowSprite = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.AddChild<UISprite>();
				this._dropShadowSprite.type = UIBasicSprite.Type.Sliced;
				this._dropShadowSprite.name = "DropShadow";
				this._dropShadowSprite.spriteName = "DropShadow";
				this._dropShadowSprite.color = Color.black;
				this._dropShadowSprite.transform.parent = base.transform;
				this._dropShadowSprite.transform.Reset();
				this._dropShadowSprite.transform.localPosition = new Vector3(0f, -4f, 0f);
				this._dropShadowSprite.ParentHasChanged();
				this._dropShadowSprite.atlas = this.backgroundSprite.atlas;
				this._dropShadowSprite.alpha = 0f;
			}
			this._dropShadowSprite.depth = this.backgroundSprite.depth - 1;
			this._dropShadowSprite.width = this.backgroundSprite.width + 42 + this.WidthPadding;
			this._dropShadowSprite.height = this.backgroundSprite.height + 42 + this.HeightPadding;
			return this._dropShadowSprite;
		}
	}

	// Token: 0x0600245E RID: 9310 RVA: 0x00100B64 File Offset: 0x000FED64
	private void OnEnable()
	{
		if (this.backgroundSprite == null)
		{
			this.backgroundSprite = base.GetComponent<UISprite>();
		}
		if (this.backgroundSprite == null)
		{
			Debug.LogError("No sprite found for hover Shadow");
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.DropShadow.alpha = 0f;
		this.OnHover(UICamera.HoveredUIObject == base.gameObject);
	}

	// Token: 0x0600245F RID: 9311 RVA: 0x00100BD0 File Offset: 0x000FEDD0
	private void OnDisable()
	{
		if (this.backgroundSprite == null)
		{
			return;
		}
		this.DropShadow.alpha = 0f;
	}

	// Token: 0x06002460 RID: 9312 RVA: 0x00100BF1 File Offset: 0x000FEDF1
	private void OnHover(bool hover)
	{
		if (this.dropShadowCoroutine != null)
		{
			base.StopCoroutine(this.dropShadowCoroutine);
		}
		this.dropShadowCoroutine = base.StartCoroutine(this.SmoothDropShadow(hover));
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x00100C1A File Offset: 0x000FEE1A
	private IEnumerator SmoothDropShadow(bool on)
	{
		if (on)
		{
			while (this.DropShadow.alpha < 1f)
			{
				this.DropShadow.alpha = Mathf.Lerp(this.DropShadow.alpha, 1f, 10f * Time.deltaTime);
				yield return null;
			}
		}
		else
		{
			while (this.DropShadow.alpha > 0f)
			{
				this.DropShadow.alpha = Mathf.Lerp(this.DropShadow.alpha, 0f, 3f * Time.deltaTime);
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04001765 RID: 5989
	public int WidthPadding;

	// Token: 0x04001766 RID: 5990
	public int HeightPadding;

	// Token: 0x04001767 RID: 5991
	public UISprite backgroundSprite;

	// Token: 0x04001768 RID: 5992
	private UISprite _dropShadowSprite;

	// Token: 0x04001769 RID: 5993
	private Coroutine dropShadowCoroutine;
}
