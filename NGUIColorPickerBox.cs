using System;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class NGUIColorPickerBox : MonoBehaviour
{
	// Token: 0x1400005B RID: 91
	// (add) Token: 0x060020BC RID: 8380 RVA: 0x000ECF2C File Offset: 0x000EB12C
	// (remove) Token: 0x060020BD RID: 8381 RVA: 0x000ECF64 File Offset: 0x000EB164
	public event Action thumbChanged;

	// Token: 0x060020BE RID: 8382 RVA: 0x000ECF99 File Offset: 0x000EB199
	private void Start()
	{
		this.RegenerateSVTexture();
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x000025B8 File Offset: 0x000007B8
	private void SliderChanged(float saturation, float value)
	{
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x000025B8 File Offset: 0x000007B8
	private void HSVChanged(float h, float s, float v)
	{
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x000ECFA4 File Offset: 0x000EB1A4
	private void RegenerateSVTexture()
	{
		Texture2D texture2D = new Texture2D(100, 100);
		for (int i = 0; i < 100; i++)
		{
			Color32[] array = new Color32[100];
			for (int j = 0; j < 100; j++)
			{
				array[j] = NGUIColorPicker.ConvertHsvToRgba(this.picker.hue, (float)i / 100f, (float)j / 100f, 1f);
			}
			texture2D.SetPixels32(i, 0, 1, 100, array);
		}
		texture2D.Apply();
		this.image.material = new Material(this.image.shader);
		this.image.material.mainTexture = texture2D;
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x000ED04C File Offset: 0x000EB24C
	private void OnDrag(Vector2 delta)
	{
		this.UpdateThumbPosition();
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x000ED04C File Offset: 0x000EB24C
	private void OnPress(bool pressed)
	{
		this.UpdateThumbPosition();
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000ED054 File Offset: 0x000EB254
	public void OnPickerChange()
	{
		this.RegenerateSVTexture();
		this.SetThumbPosOffValue();
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x000ED064 File Offset: 0x000EB264
	private void SetThumbPosOffValue()
	{
		this.thumb.transform.localPosition = new Vector2(Mathf.Lerp(-this.image.localSize.x * 0.5f + this.thumb.localSize.x * 0.25f, this.image.localSize.x * 0.5f - this.thumb.localSize.x * 0.25f, Mathf.InverseLerp(0f, 100f, this.picker.saturation)), Mathf.Lerp(-this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f, Mathf.InverseLerp(0f, 100f, this.picker.brightness)));
		float x = Mathf.InverseLerp(-this.image.localSize.x * 0.5f + this.thumb.localSize.x * 0.25f, this.image.localSize.x * 0.5f - this.thumb.localSize.x * 0.25f, this.thumb.transform.localPosition.x);
		float y = Mathf.InverseLerp(-this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f, this.thumb.transform.localPosition.y);
		this.thumbPosNormalized = new Vector2(x, y);
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x000ED278 File Offset: 0x000EB478
	private void UpdateThumbPosition()
	{
		Vector2 lastEventPosition = UICamera.lastEventPosition;
		lastEventPosition.x = Mathf.Clamp01(lastEventPosition.x / (float)Screen.width);
		lastEventPosition.y = Mathf.Clamp01(lastEventPosition.y / (float)Screen.height);
		this.thumb.transform.position = UICamera.currentCamera.ViewportToWorldPoint(lastEventPosition);
		this.thumb.transform.localPosition = new Vector2(Mathf.Clamp(this.thumb.transform.localPosition.x, -this.image.localSize.x * 0.5f + this.thumb.localSize.x * 0.25f, this.image.localSize.x * 0.5f - this.thumb.localSize.x * 0.25f), Mathf.Clamp(this.thumb.transform.localPosition.y, -this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f));
		float x = Mathf.InverseLerp(-this.image.localSize.x * 0.5f + this.thumb.localSize.x * 0.25f, this.image.localSize.x * 0.5f - this.thumb.localSize.x * 0.25f, this.thumb.transform.localPosition.x);
		float y = Mathf.InverseLerp(-this.image.localSize.y * 0.5f + this.thumb.localSize.y * 0.25f, this.image.localSize.y * 0.5f - this.thumb.localSize.y * 0.25f, this.thumb.transform.localPosition.y);
		this.thumbPosNormalized = new Vector2(x, y);
		if (this.thumbChanged != null)
		{
			this.thumbChanged();
		}
	}

	// Token: 0x0400143E RID: 5182
	public NGUIColorPicker picker;

	// Token: 0x0400143F RID: 5183
	public UISprite thumb;

	// Token: 0x04001440 RID: 5184
	public UITexture image;

	// Token: 0x04001441 RID: 5185
	public Texture2D fake;

	// Token: 0x04001442 RID: 5186
	private Texture2D tex;

	// Token: 0x04001443 RID: 5187
	public Vector2 thumbPosNormalized;
}
