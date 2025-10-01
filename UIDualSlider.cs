using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200003D RID: 61
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Dual Slider")]
public class UIDualSlider : MonoBehaviour, INotifyPropertyChanged
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x0600016A RID: 362 RVA: 0x00009912 File Offset: 0x00007B12
	// (set) Token: 0x0600016B RID: 363 RVA: 0x0000991A File Offset: 0x00007B1A
	public Tuple<int, int> Range { get; protected set; }

	// Token: 0x0600016C RID: 364 RVA: 0x00009924 File Offset: 0x00007B24
	public void Awake()
	{
		this._uiSliderMinRange = this.sliderMin.GetComponentInChildren<UISliderRange>();
		this._uiSliderMinRange.Min = (float)this.MinValue;
		this._uiSliderMinRange.Max = (float)this.MaxValue;
		EventDelegate.Add(this.sliderMin.onChange, new EventDelegate.Callback(this.OnChangeMin));
		this._uiSliderMaxRange = this.sliderMax.GetComponentInChildren<UISliderRange>();
		this._uiSliderMaxRange.Min = (float)this.MinValue;
		this._uiSliderMaxRange.Max = (float)this.MaxValue;
		EventDelegate.Add(this.sliderMax.onChange, new EventDelegate.Callback(this.OnChangeMax));
		this._uiSliderMid = this.sliderMid.GetComponent<UISprite>();
		if (this.Range != null)
		{
			return;
		}
		this.sliderMin.value = 0f;
		this.sliderMax.value = 1f;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00009A10 File Offset: 0x00007C10
	protected virtual void UpdateRange()
	{
		int num = (int)(this.sliderMin.value * (float)(this.MaxValue - this.MinValue) + (float)this.MinValue);
		int num2 = (int)(this.sliderMax.value * (float)(this.MaxValue - this.MinValue) + (float)this.MinValue);
		if (this.Range != null && num == this.Range.Item1 && num2 == this.Range.Item2)
		{
			return;
		}
		this.Range = new Tuple<int, int>(num, num2);
		this.OnPropertyChanged("Range");
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00009AA4 File Offset: 0x00007CA4
	public virtual void SetRange(int low, int high)
	{
		this.sliderMin.value = (float)(low - this.MinValue) / (float)(this.MaxValue - this.MinValue);
		this.sliderMax.value = (float)(high - this.MinValue) / (float)(this.MaxValue - this.MinValue);
		this.Range = new Tuple<int, int>(low, high);
		this.AdjustBar();
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00009B0A File Offset: 0x00007D0A
	private void OnChangeMin()
	{
		if (this.sliderMin.value > this.sliderMax.value)
		{
			this.sliderMax.value = this.sliderMin.value;
		}
		this.AdjustBar();
		this.UpdateRange();
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00009B46 File Offset: 0x00007D46
	private void OnChangeMax()
	{
		if (this.sliderMin.value > this.sliderMax.value)
		{
			this.sliderMin.value = this.sliderMax.value;
		}
		this.AdjustBar();
		this.UpdateRange();
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00009B84 File Offset: 0x00007D84
	protected void AdjustBar()
	{
		float num = (this.sliderMax.value - this.sliderMin.value) * 180f;
		if (this._uiSliderMid == null)
		{
			this._uiSliderMid = this.sliderMid.GetComponent<UISprite>();
		}
		this._uiSliderMid.width = (int)num;
		Vector3 zero = Vector3.zero;
		zero.x = (float)((int)((this.sliderMax.value + this.sliderMin.value) / 2f * 180f - 90f));
		this.sliderMid.transform.localPosition = zero;
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000172 RID: 370 RVA: 0x00009C24 File Offset: 0x00007E24
	// (remove) Token: 0x06000173 RID: 371 RVA: 0x00009C5C File Offset: 0x00007E5C
	public event PropertyChangedEventHandler PropertyChanged;

	// Token: 0x06000174 RID: 372 RVA: 0x00009C91 File Offset: 0x00007E91
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
		if (propertyChanged == null)
		{
			return;
		}
		propertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

	// Token: 0x0400013C RID: 316
	public UISlider sliderMin;

	// Token: 0x0400013D RID: 317
	public UISlider sliderMax;

	// Token: 0x0400013E RID: 318
	public GameObject sliderMid;

	// Token: 0x0400013F RID: 319
	public int MinValue;

	// Token: 0x04000140 RID: 320
	public int MaxValue = 100;

	// Token: 0x04000141 RID: 321
	private UISliderRange _uiSliderMinRange;

	// Token: 0x04000142 RID: 322
	private UISliderRange _uiSliderMaxRange;

	// Token: 0x04000143 RID: 323
	private UISprite _uiSliderMid;
}
