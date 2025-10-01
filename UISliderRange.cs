using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200033C RID: 828
public class UISliderRange : MonoBehaviour, INotifyPropertyChanged
{
	// Token: 0x06002760 RID: 10080 RVA: 0x0011840B File Offset: 0x0011660B
	private void Start()
	{
		if (this.TargetScrollBar)
		{
			this.TargetScrollBar.attachedSliderRange = this;
			return;
		}
		if (this.TargetSlider)
		{
			this.TargetSlider.attachedSliderRange = this;
		}
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x00118440 File Offset: 0x00116640
	public void SetWithDialog()
	{
		UIDialog.ShowInput(string.Format("{0} - {1}", this.Min, this.Max), "OK", "Cancel", delegate(string s)
		{
			float floatValue;
			if (float.TryParse(s, out floatValue))
			{
				this.floatValue = floatValue;
			}
		}, null, this.stringValue, "");
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06002762 RID: 10082 RVA: 0x00118494 File Offset: 0x00116694
	// (set) Token: 0x06002763 RID: 10083 RVA: 0x001184D4 File Offset: 0x001166D4
	public string stringValue
	{
		get
		{
			this.ThisUILabel = base.GetComponent<UILabel>();
			this.ThisUIInput = base.GetComponent<UIInput>();
			if (this.ThisUILabel)
			{
				return this.ThisUILabel.text;
			}
			return this.ThisUIInput.value;
		}
		set
		{
			this.ThisUILabel = base.GetComponent<UILabel>();
			this.ThisUIInput = base.GetComponent<UIInput>();
			if (this.ThisUILabel)
			{
				this.ThisUILabel.text = value;
			}
			else
			{
				this.ThisUIInput.value = value;
			}
			this.PrevValue = this.GetConvertedRange();
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06002764 RID: 10084 RVA: 0x0011852C File Offset: 0x0011672C
	// (set) Token: 0x06002765 RID: 10085 RVA: 0x00118584 File Offset: 0x00116784
	public float slideValue
	{
		get
		{
			float num = 0f;
			if (this.TargetScrollBar)
			{
				num = this.TargetScrollBar.value;
			}
			else if (this.TargetSlider)
			{
				num = this.TargetSlider.value;
			}
			if (this.Invert)
			{
				num = 1f - num;
			}
			return num;
		}
		set
		{
			if (this.TargetScrollBar)
			{
				this.TargetScrollBar.value = value;
				return;
			}
			if (this.TargetSlider)
			{
				this.TargetSlider.value = value;
			}
		}
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06002766 RID: 10086 RVA: 0x001185B9 File Offset: 0x001167B9
	// (set) Token: 0x06002767 RID: 10087 RVA: 0x001185EC File Offset: 0x001167EC
	public float floatValue
	{
		get
		{
			if (this.ValueFromSliderConverter != null)
			{
				return this.ValueFromSliderConverter(this.slideValue);
			}
			return Mathf.Lerp(this.Min, this.Max, this.slideValue);
		}
		set
		{
			if (this.SliderFromValueConverter != null)
			{
				this.slideValue = this.SliderFromValueConverter(value);
				return;
			}
			this.slideValue = Mathf.InverseLerp(this.Min, this.Max, value);
		}
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06002768 RID: 10088 RVA: 0x00118621 File Offset: 0x00116821
	// (set) Token: 0x06002769 RID: 10089 RVA: 0x00118656 File Offset: 0x00116856
	public int intValue
	{
		get
		{
			if (this.rounding != UISliderRange.Rounding.ObeyRountIntUp)
			{
				return Mathf.RoundToInt(this.floatValue);
			}
			if (this.RoundIntUp)
			{
				return Mathf.CeilToInt(this.floatValue);
			}
			return Mathf.FloorToInt(this.floatValue);
		}
		set
		{
			this.floatValue = (float)value;
		}
	}

	// Token: 0x0600276A RID: 10090 RVA: 0x00118660 File Offset: 0x00116860
	public void Update()
	{
		string convertedRange = this.GetConvertedRange();
		if (convertedRange != "" && convertedRange != this.PrevValue)
		{
			this.stringValue = convertedRange;
			this.OnPropertyChanged("stringValue");
		}
	}

	// Token: 0x0600276B RID: 10091 RVA: 0x001186A4 File Offset: 0x001168A4
	private string GetConvertedRange()
	{
		if (this.NumDecimals != 0)
		{
			return this.floatValue.ToString("f" + this.NumDecimals.ToString());
		}
		return this.intValue.ToString();
	}

	// Token: 0x0600276C RID: 10092 RVA: 0x001186EB File Offset: 0x001168EB
	public UIProgressBar GetProgressBar()
	{
		if (this.TargetSlider)
		{
			return this.TargetSlider;
		}
		if (this.TargetScrollBar)
		{
			return this.TargetScrollBar;
		}
		return null;
	}

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x0600276D RID: 10093 RVA: 0x00118718 File Offset: 0x00116918
	// (remove) Token: 0x0600276E RID: 10094 RVA: 0x00118750 File Offset: 0x00116950
	public event PropertyChangedEventHandler PropertyChanged;

	// Token: 0x0600276F RID: 10095 RVA: 0x00118785 File Offset: 0x00116985
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
		if (propertyChanged == null)
		{
			return;
		}
		propertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

	// Token: 0x040019D9 RID: 6617
	public UISliderRange.ValueConverterDelegate SliderFromValueConverter;

	// Token: 0x040019DA RID: 6618
	public UISliderRange.ValueConverterDelegate ValueFromSliderConverter;

	// Token: 0x040019DB RID: 6619
	public UIScrollBar TargetScrollBar;

	// Token: 0x040019DC RID: 6620
	public UISlider TargetSlider;

	// Token: 0x040019DD RID: 6621
	public bool Invert;

	// Token: 0x040019DE RID: 6622
	public bool RoundIntUp = true;

	// Token: 0x040019DF RID: 6623
	public UISliderRange.Rounding rounding;

	// Token: 0x040019E0 RID: 6624
	public float Min;

	// Token: 0x040019E1 RID: 6625
	public float Max = 10f;

	// Token: 0x040019E2 RID: 6626
	private UILabel ThisUILabel;

	// Token: 0x040019E3 RID: 6627
	private UIInput ThisUIInput;

	// Token: 0x040019E4 RID: 6628
	private string PrevValue = "";

	// Token: 0x040019E5 RID: 6629
	public int NumDecimals;

	// Token: 0x0200078B RID: 1931
	// (Invoke) Token: 0x06003F37 RID: 16183
	public delegate float ValueConverterDelegate(float fromValue);

	// Token: 0x0200078C RID: 1932
	public enum Rounding
	{
		// Token: 0x04002C99 RID: 11417
		ObeyRountIntUp,
		// Token: 0x04002C9A RID: 11418
		RoundToNearestInt
	}
}
