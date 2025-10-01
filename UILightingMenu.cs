using System;

// Token: 0x020002FF RID: 767
public class UILightingMenu : UIReactiveMenu
{
	// Token: 0x060024FF RID: 9471 RVA: 0x001053CC File Offset: 0x001035CC
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveElements.Add(this.LightIntensitySlider.onChange);
		this.ReactiveElements.Add(this.LightColorPicker.onChange);
		this.ReactiveElements.Add(this.AmbientIntensitySlider.onChange);
		this.ReactiveElements.Add(this.AmbientBackgroundToggle.onChange);
		this.ReactiveElements.Add(this.AmbientGradientToggle.onChange);
		this.ReactiveElements.Add(this.AmbientSkyColorPicker.onChange);
		this.ReactiveElements.Add(this.AmbientEquatorColorPicker.onChange);
		this.ReactiveElements.Add(this.AmbientGroundColorPicker.onChange);
		this.ReactiveElements.Add(this.ReflectionIntensitySlider.onChange);
		this.ReactiveElements.Add(this.LutInput.onChange);
		this.ReactiveElements.Add(this.LutContributionSlider.onChange);
		this.ReactiveInputs.Add(this.CustomLutInput.gameObject);
		EventDelegate.Add(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
		EventDelegate.Add(this.LeftLutButton.onClick, new EventDelegate.Callback(this.LeftLutOnClick));
		EventDelegate.Add(this.RightLutButton.onClick, new EventDelegate.Callback(this.RightLutOnClick));
		EventManager.OnLoadingComplete += this.EventManager_OnLoadingComplete;
	}

	// Token: 0x06002500 RID: 9472 RVA: 0x00105550 File Offset: 0x00103750
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ResetButton.onClick, new EventDelegate.Callback(this.ResetOnClick));
		EventDelegate.Remove(this.LeftLutButton.onClick, new EventDelegate.Callback(this.LeftLutOnClick));
		EventDelegate.Remove(this.RightLutButton.onClick, new EventDelegate.Callback(this.RightLutOnClick));
		EventManager.OnLoadingComplete -= this.EventManager_OnLoadingComplete;
	}

	// Token: 0x06002501 RID: 9473 RVA: 0x000FFF61 File Offset: 0x000FE161
	private void EventManager_OnLoadingComplete()
	{
		if (base.gameObject.activeSelf)
		{
			base.TriggerReloadUI();
		}
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x001055CB File Offset: 0x001037CB
	private void ResetOnClick()
	{
		NetworkSingleton<LightingScript>.Instance.Reset();
		base.TriggerReloadUI();
	}

	// Token: 0x06002503 RID: 9475 RVA: 0x001055E0 File Offset: 0x001037E0
	private void LeftLutOnClick()
	{
		int num = this.GetLutIndex() - 1;
		if (num < 0)
		{
			num = NetworkSingleton<LightingScript>.Instance.LUTs.Count - 1;
		}
		this.SetLutIndex(num);
	}

	// Token: 0x06002504 RID: 9476 RVA: 0x00105614 File Offset: 0x00103814
	private void RightLutOnClick()
	{
		int num = this.GetLutIndex() + 1;
		if (num >= NetworkSingleton<LightingScript>.Instance.LUTs.Count)
		{
			num = 0;
		}
		this.SetLutIndex(num);
	}

	// Token: 0x06002505 RID: 9477 RVA: 0x00105648 File Offset: 0x00103848
	protected override void ReloadUI()
	{
		LightingState lightingState = NetworkSingleton<LightingScript>.Instance.lightingState;
		this.LightIntensitySlider.value = lightingState.LightIntensity / 4f;
		this.LightColorPicker.value = lightingState.LightColor.ToColour();
		this.AmbientIntensitySlider.value = lightingState.AmbientIntensity / 4f;
		this.AmbientBackgroundToggle.value = (lightingState.AmbientType == AmbientType.Background);
		this.AmbientGradientToggle.value = (lightingState.AmbientType == AmbientType.Gradient);
		this.AmbientSkyColorPicker.value = lightingState.AmbientSkyColor.ToColour();
		this.AmbientEquatorColorPicker.value = lightingState.AmbientEquatorColor.ToColour();
		this.AmbientGroundColorPicker.value = lightingState.AmbientGroundColor.ToColour();
		this.ReflectionIntensitySlider.value = lightingState.ReflectionIntensity;
		this.SetLutIndex(lightingState.LutIndex);
		this.LutContributionSlider.value = lightingState.LutContribution;
		this.CustomLutInput.value = lightingState.LutURL;
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x00105761 File Offset: 0x00103961
	protected override void UpdateSource()
	{
		NetworkSingleton<LightingScript>.Instance.lightingState = this.GetLightingState();
		this.UpdateLUTName(NetworkSingleton<LightingScript>.Instance.lightingState.LutIndex);
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x00105788 File Offset: 0x00103988
	private LightingState GetLightingState()
	{
		return new LightingState
		{
			LightIntensity = this.LightIntensitySlider.value * 4f,
			LightColor = new ColourState(this.LightColorPicker.value),
			AmbientIntensity = this.AmbientIntensitySlider.value * 4f,
			AmbientType = (this.AmbientBackgroundToggle.value ? AmbientType.Background : AmbientType.Gradient),
			AmbientSkyColor = new ColourState(this.AmbientSkyColorPicker.value),
			AmbientEquatorColor = new ColourState(this.AmbientEquatorColorPicker.value),
			AmbientGroundColor = new ColourState(this.AmbientGroundColorPicker.value),
			ReflectionIntensity = this.ReflectionIntensitySlider.value,
			LutIndex = this.GetLutIndex(),
			LutContribution = this.LutContributionSlider.value,
			LutURL = this.CustomLutInput.value
		};
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x0010588C File Offset: 0x00103A8C
	private void SetLutIndex(int index)
	{
		this.LutInput.value = (index + 1).ToString();
		this.UpdateLUTName(index);
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x001058B8 File Offset: 0x00103AB8
	private int GetLutIndex()
	{
		int num = 0;
		if (int.TryParse(this.LutInput.value, out num))
		{
			num--;
		}
		return num;
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x001058E0 File Offset: 0x00103AE0
	private void UpdateLUTName(int index)
	{
		string[] array = NetworkSingleton<LightingScript>.Instance.LUTs[index].name.Split(new char[]
		{
			'_'
		});
		this.LutLabel.text = array[array.Length - 1];
	}

	// Token: 0x04001805 RID: 6149
	public UISlider LightIntensitySlider;

	// Token: 0x04001806 RID: 6150
	public UIColorPickerInput LightColorPicker;

	// Token: 0x04001807 RID: 6151
	public UISlider AmbientIntensitySlider;

	// Token: 0x04001808 RID: 6152
	public UIToggle AmbientBackgroundToggle;

	// Token: 0x04001809 RID: 6153
	public UIToggle AmbientGradientToggle;

	// Token: 0x0400180A RID: 6154
	public UIColorPickerInput AmbientSkyColorPicker;

	// Token: 0x0400180B RID: 6155
	public UIColorPickerInput AmbientEquatorColorPicker;

	// Token: 0x0400180C RID: 6156
	public UIColorPickerInput AmbientGroundColorPicker;

	// Token: 0x0400180D RID: 6157
	public UISlider ReflectionIntensitySlider;

	// Token: 0x0400180E RID: 6158
	public UIButton LeftLutButton;

	// Token: 0x0400180F RID: 6159
	public UIButton RightLutButton;

	// Token: 0x04001810 RID: 6160
	public UIInput LutInput;

	// Token: 0x04001811 RID: 6161
	public UILabel LutLabel;

	// Token: 0x04001812 RID: 6162
	public UISlider LutContributionSlider;

	// Token: 0x04001813 RID: 6163
	public UIInput CustomLutInput;

	// Token: 0x04001814 RID: 6164
	public UIButton ResetButton;

	// Token: 0x04001815 RID: 6165
	private const float IntensityMulti = 4f;
}
