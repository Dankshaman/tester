using System;
using System.Collections.Generic;
using SCPE;
using UnityEngine;

// Token: 0x02000299 RID: 665
public class GraphicsState
{
	// Token: 0x060021D5 RID: 8661 RVA: 0x000F3B2C File Offset: 0x000F1D2C
	public void SetQuality(QualityState QS)
	{
		this.Presets = QS;
		switch (QS)
		{
		case QualityState.VeryLow:
			this.VSync = false;
			this.PostAA = false;
			this.MSAA = 0;
			this.SSAO = false;
			this.FullTextures = false;
			this.Shadows = QualityState.VeryLow;
			this.Anisotropic = false;
			this.CapFPS = true;
			this.Sharpen = true;
			return;
		case QualityState.Low:
			this.VSync = false;
			this.PostAA = false;
			this.MSAA = 0;
			this.SSAO = false;
			this.FullTextures = true;
			this.Shadows = QualityState.Low;
			this.Anisotropic = false;
			this.CapFPS = true;
			this.Sharpen = true;
			return;
		case QualityState.Medium:
			this.VSync = false;
			this.PostAA = false;
			this.MSAA = 0;
			this.SSAO = false;
			this.FullTextures = true;
			this.Shadows = QualityState.Medium;
			this.Anisotropic = true;
			this.CapFPS = true;
			this.Sharpen = true;
			return;
		case QualityState.High:
			this.VSync = true;
			this.PostAA = false;
			this.MSAA = 2;
			this.SSAO = false;
			this.FullTextures = true;
			this.Shadows = QualityState.High;
			this.Anisotropic = true;
			this.CapFPS = true;
			this.Sharpen = true;
			return;
		case QualityState.VeryHigh:
			this.VSync = true;
			this.PostAA = false;
			this.MSAA = 2;
			this.SSAO = true;
			this.FullTextures = true;
			this.Shadows = QualityState.VeryHigh;
			this.Anisotropic = true;
			this.CapFPS = true;
			this.Sharpen = true;
			return;
		case QualityState.Ultra:
			this.VSync = true;
			this.PostAA = false;
			this.MSAA = 4;
			this.SSAO = true;
			this.FullTextures = true;
			this.Shadows = QualityState.Ultra;
			this.Anisotropic = true;
			this.CapFPS = true;
			this.Sharpen = true;
			return;
		default:
			return;
		}
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000F3CE0 File Offset: 0x000F1EE0
	public void Apply()
	{
		QualitySettings.SetQualityLevel((int)this.Shadows);
		if (Screen.currentResolution.width != this.ResX || Screen.currentResolution.height != this.ResY || Screen.fullScreen != this.Fullscreen)
		{
			Screen.SetResolution(this.ResX, this.ResY, this.Fullscreen);
		}
		SSAOPro[] array = Utilities.FindObjectsOfType<SSAOPro>(true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = this.SSAO;
		}
		Sharpen setting = NetworkSingleton<LightingScript>.Instance.PostProcessingProfile.GetSetting<Sharpen>();
		if (setting)
		{
			setting.enabled.value = this.Sharpen;
		}
		QualitySettings.antiAliasing = this.MSAA;
		QualitySettings.vSyncCount = (this.VSync ? 1 : 0);
		QualitySettings.masterTextureLimit = (this.FullTextures ? 0 : 1);
		MipmapBiasTextures.CurrentMipMapBias = (this.FullTextures ? -0.5f : 0f);
		QualitySettings.anisotropicFiltering = (this.Anisotropic ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable);
		this.ApplyFramerateCap();
		EventManager.TriggerUnityAnalytic("Graphics", new Dictionary<string, object>
		{
			{
				"Preset",
				UIConfigGraphics.QualityStateToString(this.Presets)
			},
			{
				"Resolution",
				this.ResX + "x" + this.ResY
			},
			{
				"Fullscreen",
				this.Fullscreen
			},
			{
				"VSync",
				this.VSync
			},
			{
				"Anisotropic",
				this.Anisotropic
			},
			{
				"MSAA",
				this.MSAA
			},
			{
				"FullTextures",
				this.FullTextures
			},
			{
				"SSAO",
				this.SSAO
			},
			{
				"Shadows",
				UIConfigGraphics.QualityStateToString(this.Shadows)
			},
			{
				"Sharpen",
				this.Sharpen
			}
		}, 1);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000F3EF0 File Offset: 0x000F20F0
	public void ApplyFramerateCap()
	{
		Application.targetFrameRate = (this.CapFPS ? ((this.CustomCapFPSRate > 0) ? this.CustomCapFPSRate : Screen.currentResolution.refreshRate) : -1);
	}

	// Token: 0x0400152C RID: 5420
	public QualityState Presets;

	// Token: 0x0400152D RID: 5421
	public int ResX;

	// Token: 0x0400152E RID: 5422
	public int ResY;

	// Token: 0x0400152F RID: 5423
	public bool Fullscreen = true;

	// Token: 0x04001530 RID: 5424
	public bool VSync;

	// Token: 0x04001531 RID: 5425
	public bool FullTextures = true;

	// Token: 0x04001532 RID: 5426
	public bool PostAA;

	// Token: 0x04001533 RID: 5427
	public int MSAA;

	// Token: 0x04001534 RID: 5428
	public bool SSAO;

	// Token: 0x04001535 RID: 5429
	public QualityState Shadows;

	// Token: 0x04001536 RID: 5430
	public bool Anisotropic;

	// Token: 0x04001537 RID: 5431
	public bool CapFPS;

	// Token: 0x04001538 RID: 5432
	public int CustomCapFPSRate;

	// Token: 0x04001539 RID: 5433
	public bool Sharpen = true;
}
