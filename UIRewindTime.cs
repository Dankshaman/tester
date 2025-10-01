using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class UIRewindTime : MonoBehaviour
{
	// Token: 0x060026B8 RID: 9912 RVA: 0x001136F4 File Offset: 0x001118F4
	private void Start()
	{
		this.HoverObjects = new List<GameObject>
		{
			this.BackwardButton.gameObject,
			this.ForwardButton.gameObject,
			this.TimeSlider.gameObject,
			this.TimeSlider.thumb.gameObject
		};
		this.saveManager = NetworkSingleton<SaveManager>.Instance;
		this.backwardButtonLabel = this.BackwardButton.GetComponentInChildren<UILabel>();
		this.forwardButtonLabel = this.ForwardButton.GetComponentInChildren<UILabel>();
		this.sliderRange = this.TimeSlider.thumb.GetComponentInChildren<UISliderRange>();
		this.SetActive(false);
		EventDelegate.Add(this.BackwardButton.onClick, new EventDelegate.Callback(this.OnClickBackward));
		EventDelegate.Add(this.ForwardButton.onClick, new EventDelegate.Callback(this.OnClickForward));
		EventDelegate.Add(this.TimeSlider.onChange, new EventDelegate.Callback(this.OnDragChange));
		UISlider timeSlider = this.TimeSlider;
		timeSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(timeSlider.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
		EventManager.OnPlayerPromoted += this.OnPlayerPromoted;
		EventManager.OnUIThemeChange += this.UpdateVisibility;
		this.saveManager.OnRewindSaveChange += this.UpdateVisibility;
		NetworkEvents.OnConnectedToServer += this.UpdateVisibility;
		this.UpdateVisibility();
	}

	// Token: 0x060026B9 RID: 9913 RVA: 0x00113870 File Offset: 0x00111A70
	private void OnDestroy()
	{
		EventDelegate.Remove(this.BackwardButton.onClick, new EventDelegate.Callback(this.OnClickBackward));
		EventDelegate.Remove(this.ForwardButton.onClick, new EventDelegate.Callback(this.OnClickForward));
		EventDelegate.Remove(this.TimeSlider.onChange, new EventDelegate.Callback(this.OnDragChange));
		UISlider timeSlider = this.TimeSlider;
		timeSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(timeSlider.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
		EventManager.OnPlayerPromoted -= this.OnPlayerPromoted;
		EventManager.OnUIThemeChange -= this.UpdateVisibility;
		this.saveManager.OnRewindSaveChange -= this.UpdateVisibility;
		NetworkEvents.OnConnectedToServer -= this.UpdateVisibility;
	}

	// Token: 0x060026BA RID: 9914 RVA: 0x00113948 File Offset: 0x00111B48
	private void Update()
	{
		if (Network.peerType != NetworkPeerMode.Disconnected && zInput.GetButton("Ctrl", ControlType.All) && TTSInput.GetKeyDown(KeyCode.Z) && !UICamera.SelectIsInput())
		{
			if (!TTSInput.GetKey(KeyCode.LeftShift))
			{
				this.GUIRewind();
			}
			else
			{
				this.GUIRewindForward();
			}
		}
		this.SetActiveButtonLabels(this.HoverObjects.Contains(UICamera.HoveredUIObject) || this.SelectedObjectIsSlider());
	}

	// Token: 0x060026BB RID: 9915 RVA: 0x001139B4 File Offset: 0x00111BB4
	private void OnClickBackward()
	{
		this.GUIRewind();
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x001139BC File Offset: 0x00111BBC
	private void OnClickForward()
	{
		this.GUIRewindForward();
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x001139C4 File Offset: 0x00111BC4
	private void OnDragChange()
	{
		int sliderRewindIndex = this.GetSliderRewindIndex();
		this.SliderTimeLabel.gameObject.SetActive(this.rewindReverseIndex != sliderRewindIndex);
		if (this.SliderTimeLabel.gameObject.activeSelf)
		{
			uint rewindTimeFromReverseIndex = this.saveManager.GetRewindTimeFromReverseIndex(sliderRewindIndex);
			uint rewindTimeFromReverseIndex2 = this.saveManager.GetRewindTimeFromReverseIndex(this.rewindReverseIndex);
			object obj = (sliderRewindIndex > this.rewindReverseIndex) ? (rewindTimeFromReverseIndex2 - rewindTimeFromReverseIndex) : (rewindTimeFromReverseIndex - rewindTimeFromReverseIndex2);
			string arg = (sliderRewindIndex > this.rewindReverseIndex) ? "-" : "+";
			object obj2 = obj;
			uint num = obj2 % 60;
			uint num2 = obj2 / 60;
			this.SliderTimeLabel.text = string.Format("{0}{1}:{2}", arg, num2, num.ToString("D2"));
		}
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x00113A80 File Offset: 0x00111C80
	private void OnDragFinished()
	{
		this.SliderTimeLabel.gameObject.SetActive(false);
		int sliderRewindIndex = this.GetSliderRewindIndex();
		if (this.rewindReverseIndex != sliderRewindIndex)
		{
			uint time = this.saveManager.GetRewindTimeFromReverseIndex(sliderRewindIndex);
			uint rewindTimeFromReverseIndex = this.saveManager.GetRewindTimeFromReverseIndex(this.rewindReverseIndex);
			uint num = (sliderRewindIndex > this.rewindReverseIndex) ? (rewindTimeFromReverseIndex - time) : (time - rewindTimeFromReverseIndex);
			UIDialog.Show(string.Format((sliderRewindIndex > this.rewindReverseIndex) ? "Rewind time {0} seconds?" : "Fast-forward time {0} seconds?", num), "Yes", "No", delegate()
			{
				this.saveManager.LoadRewindState(time);
			}, null);
		}
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x00113B3A File Offset: 0x00111D3A
	private int GetSliderRewindIndex()
	{
		return Mathf.RoundToInt(Mathf.Lerp(0f, (float)this.rewindCount - 1f, this.TimeSlider.value));
	}

	// Token: 0x060026C0 RID: 9920 RVA: 0x00113B64 File Offset: 0x00111D64
	private void OnPlayerPromoted(bool ispromoted, int id)
	{
		if (id != (int)Network.player.id)
		{
			return;
		}
		this.UpdateVisibility();
	}

	// Token: 0x060026C1 RID: 9921 RVA: 0x00113B88 File Offset: 0x00111D88
	private string GetStringNumber(int number)
	{
		string result;
		if (this.numberToString.TryGetValue(number, out result))
		{
			return result;
		}
		if (number == 0)
		{
			this.numberToString[number] = "";
			return this.numberToString[number];
		}
		this.numberToString[number] = number.ToString();
		return this.numberToString[number];
	}

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x060026C2 RID: 9922 RVA: 0x00113BE7 File Offset: 0x00111DE7
	private bool rewindSaveEnable
	{
		get
		{
			return this.saveManager.RewindSaveEnable;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x060026C3 RID: 9923 RVA: 0x00113BF4 File Offset: 0x00111DF4
	private int rewindCount
	{
		get
		{
			return this.saveManager.GetRewindSaveCount();
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x060026C4 RID: 9924 RVA: 0x00113C01 File Offset: 0x00111E01
	private int rewindReverseIndex
	{
		get
		{
			return this.saveManager.RewindReverseIndex;
		}
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x00113C0E File Offset: 0x00111E0E
	private bool ShowBackArrow()
	{
		return this.rewindCount > 0 && this.rewindCount - 1 > this.rewindReverseIndex;
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x00113C2B File Offset: 0x00111E2B
	private bool ShowForwardArrow()
	{
		return this.rewindReverseIndex > 0 && this.rewindCount > 1;
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x00113C44 File Offset: 0x00111E44
	private void SetActiveButtonLabels(bool active)
	{
		if (this.active == active)
		{
			return;
		}
		this.active = active;
		Wait.Stop(this.id);
		if (!active)
		{
			this.id = Wait.Time(delegate
			{
				this.SetActive(false);
			}, 2f, 1);
			return;
		}
		this.SetActive(true);
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x00113C96 File Offset: 0x00111E96
	private void SetActive(bool active)
	{
		this.backwardButtonLabel.gameObject.SetActive(active);
		this.forwardButtonLabel.gameObject.SetActive(active);
		this.TimeSlider.gameObject.SetActive(active);
		this.UpdateButtonLabels();
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x00113CD4 File Offset: 0x00111ED4
	private void UpdateButtonLabels()
	{
		if (!this.backwardButtonLabel.gameObject.activeSelf)
		{
			return;
		}
		this.backwardButtonLabel.text = this.GetStringNumber(Mathf.Max(this.rewindCount - this.rewindReverseIndex - 1, 0));
		this.forwardButtonLabel.text = this.GetStringNumber(this.rewindReverseIndex);
		this.TimeSlider.numberOfSteps = this.rewindCount;
		this.sliderRange.Max = (float)(this.rewindCount - 1);
		if (!this.SelectedObjectIsSlider())
		{
			this.TimeSlider.value = (float)this.rewindReverseIndex / ((float)this.rewindCount - 1f);
			return;
		}
		this.TimeSlider.value = this.TimeSlider.value;
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x00113D98 File Offset: 0x00111F98
	private bool SelectedObjectIsSlider()
	{
		return (UICamera.selectedObject == this.TimeSlider.thumb.gameObject || UICamera.selectedObject == this.TimeSlider.gameObject) && Input.GetKey(KeyCode.Mouse0);
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x00113DE4 File Offset: 0x00111FE4
	private void UpdateVisibility()
	{
		base.gameObject.SetActive(Network.player.isAdmin && this.rewindSaveEnable);
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		this.UpdateButtonLabels();
		NGUIHelper.ButtonDisable(this.BackwardButton, !this.ShowBackArrow(), null, null);
		NGUIHelper.ButtonDisable(this.ForwardButton, !this.ShowForwardArrow(), null, null);
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x00113E74 File Offset: 0x00112074
	private void GUIRewind()
	{
		if (!this.ShowBackArrow())
		{
			return;
		}
		if (Time.time < this.LastRewind + 0.5f)
		{
			return;
		}
		this.LastRewind = Time.time;
		this.saveManager.LoadRewindState(this.rewindReverseIndex + 1);
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x00113EB1 File Offset: 0x001120B1
	private void GUIRewindForward()
	{
		if (!this.ShowForwardArrow())
		{
			return;
		}
		if (Time.time < this.LastRewind + 0.5f)
		{
			return;
		}
		this.LastRewind = Time.time;
		this.saveManager.LoadRewindState(this.rewindReverseIndex - 1);
	}

	// Token: 0x0400193E RID: 6462
	public UIButton BackwardButton;

	// Token: 0x0400193F RID: 6463
	public UIButton ForwardButton;

	// Token: 0x04001940 RID: 6464
	public UISlider TimeSlider;

	// Token: 0x04001941 RID: 6465
	public UILabel SliderTimeLabel;

	// Token: 0x04001942 RID: 6466
	private UILabel backwardButtonLabel;

	// Token: 0x04001943 RID: 6467
	private UILabel forwardButtonLabel;

	// Token: 0x04001944 RID: 6468
	private UISliderRange sliderRange;

	// Token: 0x04001945 RID: 6469
	private SaveManager saveManager;

	// Token: 0x04001946 RID: 6470
	private List<GameObject> HoverObjects;

	// Token: 0x04001947 RID: 6471
	private readonly Dictionary<int, string> numberToString = new Dictionary<int, string>();

	// Token: 0x04001948 RID: 6472
	private Wait.Identifier id;

	// Token: 0x04001949 RID: 6473
	private bool active;

	// Token: 0x0400194A RID: 6474
	private float LastRewind;
}
