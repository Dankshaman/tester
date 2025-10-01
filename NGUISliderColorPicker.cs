using System;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class NGUISliderColorPicker : MonoBehaviour
{
	// Token: 0x1400005C RID: 92
	// (add) Token: 0x060020E1 RID: 8417 RVA: 0x000EE2AC File Offset: 0x000EC4AC
	// (remove) Token: 0x060020E2 RID: 8418 RVA: 0x000EE2E4 File Offset: 0x000EC4E4
	public event Action thumbChanged;

	// Token: 0x060020E3 RID: 8419 RVA: 0x000EE319 File Offset: 0x000EC519
	private void Awake()
	{
		this.RegenerateTexture();
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000025B8 File Offset: 0x000007B8
	private void Start()
	{
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000EE324 File Offset: 0x000EC524
	private void RegenerateTexture()
	{
		int num = (this.type == ColorValues.Alpha) ? 255 : 360;
		Texture2D texture2D = new Texture2D(1, num);
		Color32[] array = new Color32[num];
		for (int i = 0; i < num; i++)
		{
			if (this.type == ColorValues.Alpha)
			{
				array[i] = new Color32((byte)i, (byte)i, (byte)i, byte.MaxValue);
			}
			else
			{
				array[i] = NGUIColorPicker.ConvertHsvToRgba((float)i, 1f, 1f, 1f);
			}
		}
		texture2D.SetPixels32(array);
		texture2D.Apply();
		this.image.material = new Material(this.image.shader);
		this.image.material.mainTexture = texture2D;
		this.image.uvRect = new Rect(0f, 0f, 2f, 1f);
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000EE403 File Offset: 0x000EC603
	private void OnDrag(Vector2 delta)
	{
		this.UpdateThumbPosition();
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000EE403 File Offset: 0x000EC603
	private void OnPress(bool pressed)
	{
		this.UpdateThumbPosition();
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000EE40B File Offset: 0x000EC60B
	public void OnPickerChange()
	{
		this.RegenerateTexture();
		this.SetThumbPosOffValue();
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000EE41C File Offset: 0x000EC61C
	private void SetThumbPosOffValue()
	{
		if (this.type == ColorValues.Alpha)
		{
			this.thumb.transform.localPosition = new Vector2(0f, Mathf.Lerp(-this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f, Mathf.InverseLerp(0f, 255f, this.picker.alpha)));
		}
		else
		{
			this.thumb.transform.localPosition = new Vector2(0f, Mathf.Lerp(-this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f, Mathf.InverseLerp(0f, 360f, this.picker.hue)));
		}
		this.normY = Mathf.InverseLerp(-this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f, this.thumb.transform.localPosition.y);
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x000EE5E8 File Offset: 0x000EC7E8
	private void UpdateThumbPosition()
	{
		Vector2 lastEventPosition = UICamera.lastEventPosition;
		lastEventPosition.x = 0f;
		lastEventPosition.y = Mathf.Clamp01(lastEventPosition.y / (float)Screen.height);
		this.thumb.transform.position = UICamera.currentCamera.ViewportToWorldPoint(lastEventPosition);
		this.thumb.transform.localPosition = new Vector2(0f, Mathf.Clamp(this.thumb.transform.localPosition.y, -this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f));
		this.normY = Mathf.InverseLerp(-this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f, this.thumb.transform.localPosition.y);
		if (this.thumbChanged != null)
		{
			this.thumbChanged();
		}
	}

	// Token: 0x04001446 RID: 5190
	public ColorValues type;

	// Token: 0x04001447 RID: 5191
	public Texture2D fake;

	// Token: 0x04001448 RID: 5192
	public NGUIColorPicker picker;

	// Token: 0x04001449 RID: 5193
	public UITexture image;

	// Token: 0x0400144A RID: 5194
	private Texture2D tex;

	// Token: 0x0400144B RID: 5195
	public UISprite thumb;

	// Token: 0x0400144C RID: 5196
	public bool moveX;

	// Token: 0x0400144E RID: 5198
	public float normY;
}
