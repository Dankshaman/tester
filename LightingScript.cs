using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000150 RID: 336
public class LightingScript : NetworkSingleton<LightingScript>
{
	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001106 RID: 4358 RVA: 0x0007578F File Offset: 0x0007398F
	// (set) Token: 0x06001107 RID: 4359 RVA: 0x00075798 File Offset: 0x00073998
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public LightingState lightingState
	{
		get
		{
			return this._lightingState;
		}
		set
		{
			bool flag = value.LutURL != this._lightingState.LutURL;
			if (!string.IsNullOrEmpty(this._lightingState.LutURL) && flag)
			{
				Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(this._lightingState.LutURL, new Action<CustomTextureContainer>(this.LutCallback), true);
			}
			this._lightingState = value;
			if (this._lightingState.LutIndex < 0 || this._lightingState.LutIndex >= this.LUTs.Count)
			{
				this._lightingState.LutIndex = 0;
			}
			if (string.IsNullOrEmpty(this._lightingState.LutURL))
			{
				this.SetLutTexture(this.LUTs[this._lightingState.LutIndex]);
			}
			else if (flag)
			{
				this.SetLutTexture(this.LUTs[this._lightingState.LutIndex]);
				Singleton<CustomLoadingManager>.Instance.Texture.Load(this._lightingState.LutURL, new Action<CustomTextureContainer>(this.LutCallback), false, false, true, false, true, true, 8192, CustomLoadingManager.LoadType.Auto);
			}
			else
			{
				this.PostProcessingProfile.GetSetting<ColorGrading>().ldrLutContribution.value = this._lightingState.LutContribution;
			}
			this.DirectionalLight.intensity = this._lightingState.LightIntensity;
			this.DirectionalLight.color = this._lightingState.LightColor.ToColour();
			this.DirectionalLight.gameObject.SetActive(this._lightingState.LightIntensity != 0f);
			RenderSettings.ambientIntensity = this._lightingState.AmbientIntensity;
			RenderSettings.ambientMode = ((this._lightingState.AmbientType == AmbientType.Background) ? AmbientMode.Skybox : AmbientMode.Trilight);
			float b = 1f;
			if (this._lightingState.AmbientType == AmbientType.Gradient)
			{
				b = this._lightingState.AmbientIntensity;
				RenderSettings.ambientIntensity = 1.3f;
			}
			RenderSettings.ambientSkyColor = this._lightingState.AmbientSkyColor.ToColour() * b;
			RenderSettings.ambientEquatorColor = this._lightingState.AmbientEquatorColor.ToColour() * b;
			RenderSettings.ambientGroundColor = this._lightingState.AmbientGroundColor.ToColour() * b;
			RenderSettings.reflectionIntensity = this._lightingState.ReflectionIntensity;
			SkyScript.UpdateSpecIntensity();
			DynamicGI.UpdateEnvironment();
			base.DirtySync("lightingState");
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001108 RID: 4360 RVA: 0x00075A06 File Offset: 0x00073C06
	public PostProcessProfile PostProcessingProfile
	{
		get
		{
			if (!this._PostProcessingProfile)
			{
				this._PostProcessingProfile = UnityEngine.Object.FindObjectOfType<PostProcessVolume>().profile;
			}
			return this._PostProcessingProfile;
		}
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x00075A2B File Offset: 0x00073C2B
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x00075A33 File Offset: 0x00073C33
	private IEnumerator Start()
	{
		yield return null;
		this.lightingState = new LightingState();
		yield break;
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x00075A44 File Offset: 0x00073C44
	public void Reset()
	{
		LightingState lightingState = new LightingState();
		if (lightingState != this.lightingState)
		{
			this.lightingState = lightingState;
		}
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x00075A6C File Offset: 0x00073C6C
	public void LutCallback(CustomTextureContainer customTextureContainer)
	{
		if (customTextureContainer.texture == null)
		{
			return;
		}
		if (customTextureContainer.texture.width != 256 || customTextureContainer.texture.height != 16)
		{
			Chat.LogError("Error loading custom lut images dimensions are incorrect, should be 256x16.", true);
			return;
		}
		customTextureContainer.texture.filterMode = FilterMode.Bilinear;
		customTextureContainer.texture.anisoLevel = 0;
		this.SetLutTexture(customTextureContainer.texture);
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x00075AD9 File Offset: 0x00073CD9
	private void SetLutTexture(Texture texture)
	{
		ColorGrading setting = this.PostProcessingProfile.GetSetting<ColorGrading>();
		setting.ldrLut.value = texture;
		setting.ldrLutContribution.value = this.lightingState.LutContribution;
	}

	// Token: 0x04000ACB RID: 2763
	public List<Texture2D> LUTs;

	// Token: 0x04000ACC RID: 2764
	private LightingState _lightingState = new LightingState();

	// Token: 0x04000ACD RID: 2765
	public Light DirectionalLight;

	// Token: 0x04000ACE RID: 2766
	private PostProcessProfile _PostProcessingProfile;
}
