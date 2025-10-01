using System;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class LuaLighting
{
	// Token: 0x17000344 RID: 836
	// (get) Token: 0x0600138A RID: 5002 RVA: 0x00082B5C File Offset: 0x00080D5C
	// (set) Token: 0x0600138B RID: 5003 RVA: 0x00082B68 File Offset: 0x00080D68
	[MoonSharpHidden]
	private LightingState lightingState
	{
		get
		{
			return NetworkSingleton<LightingScript>.Instance.lightingState;
		}
		set
		{
			NetworkSingleton<LightingScript>.Instance.lightingState = value;
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x0600138C RID: 5004 RVA: 0x00082B75 File Offset: 0x00080D75
	// (set) Token: 0x0600138D RID: 5005 RVA: 0x00082B84 File Offset: 0x00080D84
	public float light_intensity
	{
		get
		{
			return this.lightingState.LightIntensity;
		}
		set
		{
			this.lightingState = new LightingState(this.lightingState, new float?(value), null, null, null, null, null, null, null, null, null, null);
		}
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x00082BFA File Offset: 0x00080DFA
	public Color GetLightColor()
	{
		return this.lightingState.LightColor.ToColour();
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00082C14 File Offset: 0x00080E14
	public bool SetLightColor(Color color)
	{
		this.lightingState = new LightingState(this.lightingState, null, new ColourState?(new ColourState(color)), null, null, null, null, null, null, null, null, null);
		return true;
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06001390 RID: 5008 RVA: 0x00082C95 File Offset: 0x00080E95
	// (set) Token: 0x06001391 RID: 5009 RVA: 0x00082CA4 File Offset: 0x00080EA4
	public float ambient_intensity
	{
		get
		{
			return this.lightingState.AmbientIntensity;
		}
		set
		{
			this.lightingState = new LightingState(this.lightingState, null, null, new float?(value), null, null, null, null, null, null, null, null);
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06001392 RID: 5010 RVA: 0x00082D1A File Offset: 0x00080F1A
	// (set) Token: 0x06001393 RID: 5011 RVA: 0x00082D2C File Offset: 0x00080F2C
	public int ambient_type
	{
		get
		{
			return (int)(this.lightingState.AmbientType + 1);
		}
		set
		{
			this.lightingState = new LightingState(this.lightingState, null, null, null, new AmbientType?((AmbientType)(value - 1)), null, null, null, null, null, null, null);
		}
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x00082DA4 File Offset: 0x00080FA4
	public Color GetAmbientSkyColor()
	{
		return this.lightingState.AmbientSkyColor.ToColour();
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x00082DBC File Offset: 0x00080FBC
	public bool SetAmbientSkyColor(Color color)
	{
		this.lightingState = new LightingState(this.lightingState, null, null, null, null, new ColourState?(new ColourState(color)), null, null, null, null, null, null);
		return true;
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x00082E3D File Offset: 0x0008103D
	public Color GetAmbientEquatorColor()
	{
		return this.lightingState.AmbientEquatorColor.ToColour();
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x00082E54 File Offset: 0x00081054
	public bool SetAmbientEquatorColor(Color color)
	{
		this.lightingState = new LightingState(this.lightingState, null, null, null, null, null, new ColourState?(new ColourState(color)), null, null, null, null, null);
		return true;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x00082ED5 File Offset: 0x000810D5
	public Color GetAmbientGroundColor()
	{
		return this.lightingState.AmbientGroundColor.ToColour();
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x00082EEC File Offset: 0x000810EC
	public bool SetAmbientGroundColor(Color color)
	{
		this.lightingState = new LightingState(this.lightingState, null, null, null, null, null, null, new ColourState?(new ColourState(color)), null, null, null, null);
		return true;
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x0600139A RID: 5018 RVA: 0x00082F6D File Offset: 0x0008116D
	// (set) Token: 0x0600139B RID: 5019 RVA: 0x00082F7C File Offset: 0x0008117C
	public float reflection_intensity
	{
		get
		{
			return this.lightingState.ReflectionIntensity;
		}
		set
		{
			this.lightingState = new LightingState(this.lightingState, null, null, null, null, null, null, null, new float?(value), null, null, null);
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x0600139C RID: 5020 RVA: 0x00082FF2 File Offset: 0x000811F2
	// (set) Token: 0x0600139D RID: 5021 RVA: 0x00083000 File Offset: 0x00081200
	public int lut_index
	{
		get
		{
			return this.lightingState.LutIndex;
		}
		set
		{
			this.lightingState = new LightingState(this.lightingState, null, null, null, null, null, null, null, null, new int?(value), null, null);
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x0600139E RID: 5022 RVA: 0x00083076 File Offset: 0x00081276
	// (set) Token: 0x0600139F RID: 5023 RVA: 0x00083084 File Offset: 0x00081284
	public float lut_contribution
	{
		get
		{
			return this.lightingState.LutContribution;
		}
		set
		{
			this.lightingState = new LightingState(this.lightingState, null, null, null, null, null, null, null, null, null, new float?(value), null);
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x060013A0 RID: 5024 RVA: 0x000830FA File Offset: 0x000812FA
	// (set) Token: 0x060013A1 RID: 5025 RVA: 0x00083108 File Offset: 0x00081308
	public string lut_url
	{
		get
		{
			return this.lightingState.LutURL;
		}
		set
		{
			this.lightingState = new LightingState(this.lightingState, null, null, null, null, null, null, null, null, null, null, value);
		}
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x00014D66 File Offset: 0x00012F66
	public bool Apply()
	{
		return true;
	}
}
