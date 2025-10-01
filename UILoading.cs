using System;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class UILoading : Singleton<UILoading>
{
	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x0600250D RID: 9485 RVA: 0x00105925 File Offset: 0x00103B25
	// (set) Token: 0x0600250E RID: 9486 RVA: 0x0010592D File Offset: 0x00103B2D
	public int NumLoading
	{
		get
		{
			return this._numLoading;
		}
		private set
		{
			if (this._numLoading != value)
			{
				this._numLoading = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x00105948 File Offset: 0x00103B48
	public void AddLoading()
	{
		int numLoading = this.NumLoading;
		this.NumLoading = numLoading + 1;
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06002510 RID: 9488 RVA: 0x00105965 File Offset: 0x00103B65
	// (set) Token: 0x06002511 RID: 9489 RVA: 0x0010596D File Offset: 0x00103B6D
	public int NumComplete
	{
		get
		{
			return this._numComplete;
		}
		private set
		{
			if (value != this._numComplete)
			{
				this._numComplete = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x00105988 File Offset: 0x00103B88
	public void RemoveLoading()
	{
		int numComplete = this.NumComplete;
		this.NumComplete = numComplete + 1;
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06002513 RID: 9491 RVA: 0x001059A5 File Offset: 0x00103BA5
	// (set) Token: 0x06002514 RID: 9492 RVA: 0x001059AD File Offset: 0x00103BAD
	public string ProgressName { get; private set; } = "";

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06002515 RID: 9493 RVA: 0x001059B6 File Offset: 0x00103BB6
	// (set) Token: 0x06002516 RID: 9494 RVA: 0x001059BE File Offset: 0x00103BBE
	public int Progress
	{
		get
		{
			return this._progress;
		}
		private set
		{
			if (value != this._progress)
			{
				this._progress = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x06002517 RID: 9495 RVA: 0x001059D6 File Offset: 0x00103BD6
	public void AddProgress(string name, float progress)
	{
		this.ProgressName = name;
		this.Progress = (int)Mathf.Round(progress * 100f);
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x001059F4 File Offset: 0x00103BF4
	private void UpdateLabel()
	{
		if (this.NumLoading <= 0 || this.NumComplete >= this.NumLoading)
		{
			this.Reset();
			EventManager.TriggerLoadingChange(this.NumLoading, this.NumComplete);
			return;
		}
		EventManager.TriggerLoadingChange(this.NumLoading, this.NumComplete);
		UILoading.IsLoading = true;
		if (!this.startedShow)
		{
			this.startedShow = true;
			this.show = Wait.Time(new Action(this.Show), (float)(ConfigGame.Settings.ConfigMods.Threading ? 1 : 0), 1);
		}
		float num = Mathf.Round((float)this.NumComplete / (float)this.NumLoading * 100f);
		this.MyUILabel.text = string.Format("({0}/{1}) {2}%", this.NumComplete, this.NumLoading, num);
		if (this.Progress > 0 && this.Progress < 100)
		{
			UILabel myUILabel = this.MyUILabel;
			myUILabel.text += string.Format("\n{0} {1}%", this.ProgressName, this.Progress);
		}
		if (string.IsNullOrEmpty(this.ProgressName))
		{
			if (CustomCache.GetCacheMode() == CacheMode.None)
			{
				UILabel myUILabel2 = this.MyUILabel;
				myUILabel2.text += "\nMod Caching: Off";
			}
			if (CustomCache.GetCacheMode() == CacheMode.NoRawCache)
			{
				UILabel myUILabel3 = this.MyUILabel;
				myUILabel3.text += "\nMod Caching: No Raw Cache";
			}
			if (!ConfigGame.Settings.ConfigMods.Threading)
			{
				UILabel myUILabel4 = this.MyUILabel;
				myUILabel4.text += "\nMod Threading: Off";
			}
		}
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x00105B92 File Offset: 0x00103D92
	private void Show()
	{
		if (UILoading.IsLoading)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x00105BA8 File Offset: 0x00103DA8
	public void Reset()
	{
		this._numLoading = 0;
		this._numComplete = 0;
		this._progress = 0;
		this.ProgressName = "";
		UILoading.IsLoading = false;
		base.gameObject.SetActive(false);
		Wait.Stop(this.show);
		this.startedShow = false;
	}

	// Token: 0x04001816 RID: 6166
	public static bool IsLoading;

	// Token: 0x04001817 RID: 6167
	private int _numLoading;

	// Token: 0x04001818 RID: 6168
	private int _numComplete;

	// Token: 0x0400181A RID: 6170
	private int _progress;

	// Token: 0x0400181B RID: 6171
	public UILabel MyUILabel;

	// Token: 0x0400181C RID: 6172
	private bool startedShow;

	// Token: 0x0400181D RID: 6173
	private Wait.Identifier show;
}
