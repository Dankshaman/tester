using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200029B RID: 667
public class UIConfigGraphics : UIReactiveMenu, INotifySceneAwake
{
	// Token: 0x060021D9 RID: 8665 RVA: 0x000F3F48 File Offset: 0x000F2148
	protected override void Awake()
	{
		base.Awake();
		this.ReactiveElements.Add(this.Preset.onChange);
		this.ReactiveElements.Add(this.Res.onChange);
		this.ReactiveElements.Add(this.Fullscreen.onChange);
		this.ReactiveElements.Add(this.VSync.onChange);
		this.ReactiveElements.Add(this.FullTextures.onChange);
		this.ReactiveElements.Add(this.PostAA.onChange);
		this.ReactiveElements.Add(this.MSAA.onChange);
		this.ReactiveElements.Add(this.SSAO.onChange);
		this.ReactiveElements.Add(this.Shadows.onChange);
		this.ReactiveElements.Add(this.Anisotropic.onChange);
		this.ReactiveElements.Add(this.CapFPS.onChange);
		this.ReactiveElements.Add(this.Sharpen.onChange);
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000F4064 File Offset: 0x000F2264
	protected override void OnEnable()
	{
		base.OnEnable();
		if (UIConfigGraphics.CurrentGraphics == null)
		{
			UIConfigGraphics.Load(false);
		}
		Resolution[] resolutions = Screen.resolutions;
		List<string> items = this.Res.items;
		items.Clear();
		for (int i = 0; i < resolutions.Length; i++)
		{
			string item = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
			if (!items.Contains(item))
			{
				items.Add(item);
			}
		}
		if (items.Count < 2)
		{
			items.Add("1920x1080");
			items.Add(Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString());
		}
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000F4141 File Offset: 0x000F2341
	public void Reset()
	{
		Chat.Log("Graphics have been reset to default.", ChatMessageType.Game);
		UIConfigGraphics.CurrentGraphics.SetQuality(QualityState.Medium);
		base.TriggerReloadUI();
		UIConfigGraphics.CurrentGraphics.Apply();
		UIConfigGraphics.Save();
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000F416E File Offset: 0x000F236E
	public static string QualityStateToString(QualityState QS)
	{
		if (QS == QualityState.VeryLow)
		{
			return "Very Low";
		}
		if (QS != QualityState.VeryHigh)
		{
			return QS.ToString();
		}
		return "Very High";
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000F4192 File Offset: 0x000F2392
	public static QualityState StringToQualityState(string EnumString)
	{
		if (EnumString == "Very Low")
		{
			return QualityState.VeryLow;
		}
		if (!(EnumString == "Very High"))
		{
			return (QualityState)Enum.Parse(typeof(QualityState), EnumString, true);
		}
		return QualityState.VeryHigh;
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000F41CA File Offset: 0x000F23CA
	public static void Save()
	{
		SerializationScript.Save(UIConfigGraphics.CurrentGraphics);
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000F41D8 File Offset: 0x000F23D8
	public static void Load(bool bApply = true)
	{
		UIConfigGraphics.CurrentGraphics = SerializationScript.LoadGraphics();
		if (UIConfigGraphics.CurrentGraphics == null)
		{
			UIConfigGraphics.CurrentGraphics = new GraphicsState();
			UIConfigGraphics.CurrentGraphics.ResX = Screen.currentResolution.width;
			UIConfigGraphics.CurrentGraphics.ResY = Screen.currentResolution.height;
			UIConfigGraphics.CurrentGraphics.SetQuality(QualityState.Medium);
			UIConfigGraphics.Save();
		}
		if (bApply)
		{
			UIConfigGraphics.CurrentGraphics.Apply();
		}
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000F424C File Offset: 0x000F244C
	protected override void ReloadUI()
	{
		this.Preset.value = UIConfigGraphics.QualityStateToString(UIConfigGraphics.CurrentGraphics.Presets);
		this.Res.value = UIConfigGraphics.CurrentGraphics.ResX.ToString() + "x" + UIConfigGraphics.CurrentGraphics.ResY.ToString();
		this.Fullscreen.value = UIConfigGraphics.CurrentGraphics.Fullscreen;
		this.VSync.value = UIConfigGraphics.CurrentGraphics.VSync;
		this.FullTextures.value = UIConfigGraphics.CurrentGraphics.FullTextures;
		if (this.PostAA)
		{
			this.PostAA.value = UIConfigGraphics.CurrentGraphics.PostAA;
		}
		this.SSAO.value = UIConfigGraphics.CurrentGraphics.SSAO;
		if (UIConfigGraphics.CurrentGraphics.MSAA == 0)
		{
			this.MSAA.value = "Disabled";
		}
		else
		{
			this.MSAA.value = UIConfigGraphics.CurrentGraphics.MSAA.ToString() + "x";
		}
		this.Shadows.value = UIConfigGraphics.QualityStateToString(UIConfigGraphics.CurrentGraphics.Shadows);
		this.Anisotropic.value = UIConfigGraphics.CurrentGraphics.Anisotropic;
		this.CapFPS.value = UIConfigGraphics.CurrentGraphics.CapFPS;
		this.Sharpen.value = UIConfigGraphics.CurrentGraphics.Sharpen;
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000F43B8 File Offset: 0x000F25B8
	protected override void UpdateSource()
	{
		QualityState qualityState = UIConfigGraphics.StringToQualityState(this.Preset.value);
		if (qualityState != UIConfigGraphics.CurrentGraphics.Presets)
		{
			UIConfigGraphics.CurrentGraphics.SetQuality(qualityState);
			base.TriggerReloadUI();
		}
		else
		{
			UIConfigGraphics.CurrentGraphics.Presets = QualityState.Custom;
			this.Preset.value = UIConfigGraphics.QualityStateToString(UIConfigGraphics.CurrentGraphics.Presets);
			string[] array = this.Res.value.Split(new char[]
			{
				'x'
			}, StringSplitOptions.RemoveEmptyEntries);
			UIConfigGraphics.CurrentGraphics.ResX = int.Parse(array[0]);
			UIConfigGraphics.CurrentGraphics.ResY = int.Parse(array[1]);
			UIConfigGraphics.CurrentGraphics.Fullscreen = this.Fullscreen.value;
			UIConfigGraphics.CurrentGraphics.VSync = this.VSync.value;
			UIConfigGraphics.CurrentGraphics.FullTextures = this.FullTextures.value;
			if (this.PostAA)
			{
				UIConfigGraphics.CurrentGraphics.PostAA = this.PostAA.value;
			}
			UIConfigGraphics.CurrentGraphics.SSAO = this.SSAO.value;
			if (this.MSAA.value == "Disabled")
			{
				UIConfigGraphics.CurrentGraphics.MSAA = 0;
			}
			else
			{
				UIConfigGraphics.CurrentGraphics.MSAA = int.Parse(this.MSAA.value.Substring(0, 1));
			}
			UIConfigGraphics.CurrentGraphics.Shadows = UIConfigGraphics.StringToQualityState(this.Shadows.value);
			UIConfigGraphics.CurrentGraphics.Anisotropic = this.Anisotropic.value;
			UIConfigGraphics.CurrentGraphics.CapFPS = this.CapFPS.value;
			UIConfigGraphics.CurrentGraphics.Sharpen = this.Sharpen.value;
		}
		UIConfigGraphics.CurrentGraphics.Apply();
		UIConfigGraphics.Save();
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x000025B8 File Offset: 0x000007B8
	protected override void NetworkSync()
	{
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000F4582 File Offset: 0x000F2782
	public void SceneAwake()
	{
		UIConfigGraphics.Load(true);
	}

	// Token: 0x04001542 RID: 5442
	public UIPopupList Preset;

	// Token: 0x04001543 RID: 5443
	public UIPopupList Res;

	// Token: 0x04001544 RID: 5444
	public UIToggle Fullscreen;

	// Token: 0x04001545 RID: 5445
	public UIToggle VSync;

	// Token: 0x04001546 RID: 5446
	public UIToggle FullTextures;

	// Token: 0x04001547 RID: 5447
	public UIToggle PostAA;

	// Token: 0x04001548 RID: 5448
	public UIPopupList MSAA;

	// Token: 0x04001549 RID: 5449
	public UIToggle SSAO;

	// Token: 0x0400154A RID: 5450
	public UIPopupList Shadows;

	// Token: 0x0400154B RID: 5451
	public UIToggle Anisotropic;

	// Token: 0x0400154C RID: 5452
	public UIToggle CapFPS;

	// Token: 0x0400154D RID: 5453
	public UIToggle Sharpen;

	// Token: 0x0400154E RID: 5454
	public static GraphicsState CurrentGraphics;

	// Token: 0x0400154F RID: 5455
	private const string DISABLED_STRING = "Disabled";
}
