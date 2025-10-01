using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class UIFixedStepDualSlider : UIDualSlider
{
	// Token: 0x06000182 RID: 386 RVA: 0x00009F8C File Offset: 0x0000818C
	public new void Awake()
	{
		if (this._awake)
		{
			return;
		}
		this._awake = true;
		this.MinValue = 0;
		this.MaxValue = this.FixedSteps.Length - 1;
		this.MinLabel.text = this.FixedSteps.First<int>().ToString();
		this.MaxLabel.text = this.FixedSteps.First<int>().ToString();
		base.Awake();
		this.sliderMin.numberOfSteps = this.FixedSteps.Length;
		this.sliderMax.numberOfSteps = this.FixedSteps.Length;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000A028 File Offset: 0x00008228
	protected override void UpdateRange()
	{
		int num = this.FixedSteps[Mathf.RoundToInt(this.sliderMin.value * (float)(this.FixedSteps.Length - 1))];
		int num2 = this.FixedSteps[Mathf.RoundToInt(this.sliderMax.value * (float)(this.FixedSteps.Length - 1))];
		this.MinLabel.text = num.ToString();
		this.MaxLabel.text = num2.ToString();
		if (base.Range != null && num == base.Range.Item1 && num2 == base.Range.Item2)
		{
			return;
		}
		base.Range = new Tuple<int, int>(num, num2);
		this.OnPropertyChanged("Range");
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000A0E0 File Offset: 0x000082E0
	public override void SetRange(int low, int high)
	{
		if (!this._awake)
		{
			this.Awake();
		}
		this.MinLabel.text = low.ToString();
		this.MaxLabel.text = high.ToString();
		this.sliderMin.value = (float)(Array.IndexOf<int>(this.FixedSteps, low) - this.MinValue) / (float)(this.MaxValue - this.MinValue);
		this.sliderMax.value = (float)(Array.IndexOf<int>(this.FixedSteps, high) - this.MinValue) / (float)(this.MaxValue - this.MinValue);
		base.Range = new Tuple<int, int>(low, high);
		base.AdjustBar();
	}

	// Token: 0x04000154 RID: 340
	public UILabel MinLabel;

	// Token: 0x04000155 RID: 341
	public UILabel MaxLabel;

	// Token: 0x04000156 RID: 342
	public int[] FixedSteps;

	// Token: 0x04000157 RID: 343
	private bool _awake;
}
