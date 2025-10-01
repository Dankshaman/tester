using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000389 RID: 905
[RequireComponent(typeof(MeshRenderer))]
public class ForceField : MonoBehaviour
{
	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06002A90 RID: 10896 RVA: 0x0012EDF9 File Offset: 0x0012CFF9
	public Vector3[] Times
	{
		get
		{
			return this.times;
		}
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x0012EE04 File Offset: 0x0012D004
	private void Start()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		List<Material> list = new List<Material>();
		list.AddRange(component.materials);
		list.Add(new Material(this.forceFieldShader));
		component.materials = list.ToArray();
		this.mat = component.materials[0];
		this.sound = base.GetComponent<AudioSource>();
		if (this.sound != null)
		{
			this.sound.clip = this.impactSound;
		}
		for (int i = 0; i < 10; i++)
		{
			this.times[i] = Vector3.zero;
			this.mat.SetVector("coords" + i.ToString(), Vector4.zero);
			this.mat.SetVector("times" + i.ToString(), new Vector4(0f, 0f, 0f, 0f));
		}
		this.SetShaderParameters();
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x0012EEFC File Offset: 0x0012D0FC
	private void Update()
	{
		for (int i = 0; i < 10; i++)
		{
			if (this.times[i].y > 0f)
			{
				if (this.times[i].x > 0f)
				{
					Vector3[] array = this.times;
					int num = i;
					array[num].x = array[num].x - Time.deltaTime / this._ImpactSplashLifetime;
				}
				if (this.times[i].x < 0f)
				{
					this.times[i].x = 0f;
				}
				if (this.times[i].y > 0f)
				{
					Vector3[] array2 = this.times;
					int num2 = i;
					array2[num2].y = array2[num2].y - Time.deltaTime / this._FieldSplashLifetime;
				}
				if (this.times[i].y < 0f)
				{
					this.times[i].y = 0f;
				}
				this.mat.SetVector("times" + i.ToString(), this.times[i]);
			}
		}
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x0012F032 File Offset: 0x0012D232
	private void OnCollisionEnter(Collision col)
	{
		if (this.ImpactOnCollision)
		{
			this.AddImpact(col.contacts[0].point, col.contacts[0].normal);
		}
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x0012F064 File Offset: 0x0012D264
	private int GetFirstFreeImpactPoint()
	{
		for (int i = 0; i < this.times.Length; i++)
		{
			if (this.times[i].y <= 0f)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x0012F0A0 File Offset: 0x0012D2A0
	public void AddImpact(Vector3 point, Vector3 normal)
	{
		int firstFreeImpactPoint = this.GetFirstFreeImpactPoint();
		if (firstFreeImpactPoint != -1)
		{
			this.times[firstFreeImpactPoint] = Vector3.one;
			this.mat.SetVector("coords" + firstFreeImpactPoint.ToString(), base.transform.InverseTransformPoint(point));
			this.mat.SetVector("times" + firstFreeImpactPoint.ToString(), this.times[firstFreeImpactPoint]);
			this.mat.SetVector("normals" + firstFreeImpactPoint.ToString(), base.transform.InverseTransformDirection(normal));
			if (this.sound != null)
			{
				this.sound.PlayOneShot(this.sound.clip);
			}
		}
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x0012F178 File Offset: 0x0012D378
	private void SetShaderParameters()
	{
		this.mat.SetColor("_FieldColor", this._FieldColor);
		this.mat.SetFloat("_FieldSplashRadius", this._FieldSplashRadius);
		this.mat.SetFloat("_FieldSplashGrowthMultiplier", this._FieldSplashGrowthMultiplier);
		this.mat.SetFloat("_FieldSplashPow", this._FieldSplashPow);
		this.mat.SetFloat("_FieldSplashDecayStartAfterLifeTimePercent", this._FieldSplashDecayStartAfterLifeTimePercent);
		this.mat.SetFloat("_ImpactSplashRadius", this._ImpactSplashRadius);
		this.mat.SetFloat("_ImpactSplashVisibility", this._ImpactSplashVisibility);
		this.mat.SetFloat("_ImpactSplashPow", this._ImpactSplashPow);
		this.mat.SetFloat("_ImpactSplashGrowthMultiplier", this._ImpactSplashGrowthMultiplier);
		this.mat.SetFloat("_PassiveFieldVisibility", this._PassiveFieldVisibility);
		this.mat.SetFloat("_FieldRimVisibility", this._FieldRimVisibility);
		this.mat.SetFloat("_FieldRimColorVisibility", this._FieldRimColorVisibility);
		this.mat.SetFloat("_FieldDischargeVisibility", this._FieldDischargeVisibility);
		this.mat.SetFloat("_FieldHighlightVisibility", this._FieldHighlightVisibility);
		this.mat.SetFloat("_FieldHighlightPow", this._FieldHighlightPow);
		this.mat.SetFloat("_FieldOutlineThickness", this._FieldOutlineThickness);
		this.mat.SetFloat("_Octaves", this._Octaves);
		this.mat.SetFloat("_Frequency", this._Frequency);
		this.mat.SetFloat("_Amplitude", this._Amplitude);
		this.mat.SetFloat("_Lacunarity", this._Lacunarity);
		this.mat.SetFloat("_Persistence", this._Persistence);
		this.mat.SetVector("_Offset", this._Offset);
		this.mat.SetFloat("_RidgeOffset", this._RidgeOffset);
		this.mat.SetFloat("_AnimSpeed", this._AnimSpeed);
		this.mat.SetFloat("_PowInner", this._PowInner);
		this.mat.SetFloat("_PowOuter", this._PowOuter);
		this.mat.SetFloat("_PowInner2", this._PowInner2);
		this.mat.SetFloat("_PowOuter2", this._PowOuter2);
		this.mat.SetFloat("_ScaleX", this._Scale.x);
		this.mat.SetFloat("_ScaleY", this._Scale.y);
		this.mat.SetFloat("_ScaleZ", this._Scale.z);
	}

	// Token: 0x04001CE7 RID: 7399
	public Shader forceFieldShader;

	// Token: 0x04001CE8 RID: 7400
	public AudioClip impactSound;

	// Token: 0x04001CE9 RID: 7401
	public bool ImpactOnCollision;

	// Token: 0x04001CEA RID: 7402
	public Vector3 _Scale = Vector3.zero;

	// Token: 0x04001CEB RID: 7403
	public Color _FieldColor = new Color(0f, 0.5f, 1f, 1f);

	// Token: 0x04001CEC RID: 7404
	public float _FieldSplashLifetime = 2f;

	// Token: 0x04001CED RID: 7405
	public float _FieldSplashRadius = 1f;

	// Token: 0x04001CEE RID: 7406
	public float _FieldSplashGrowthMultiplier = 4f;

	// Token: 0x04001CEF RID: 7407
	public float _FieldSplashPow = 3f;

	// Token: 0x04001CF0 RID: 7408
	public float _FieldSplashDecayStartAfterLifeTimePercent = 20f;

	// Token: 0x04001CF1 RID: 7409
	public float _ImpactSplashLifetime = 1.25f;

	// Token: 0x04001CF2 RID: 7410
	public float _ImpactSplashRadius = 0.5f;

	// Token: 0x04001CF3 RID: 7411
	[Range(0f, 1.5f)]
	public float _ImpactSplashVisibility = 1f;

	// Token: 0x04001CF4 RID: 7412
	public float _ImpactSplashPow = 2.6f;

	// Token: 0x04001CF5 RID: 7413
	public float _ImpactSplashGrowthMultiplier = 3f;

	// Token: 0x04001CF6 RID: 7414
	[Range(0f, 0.5f)]
	public float _PassiveFieldVisibility = 0.05f;

	// Token: 0x04001CF7 RID: 7415
	[Range(0f, 1f)]
	public float _FieldRimVisibility = 0.15f;

	// Token: 0x04001CF8 RID: 7416
	[Range(0f, 1f)]
	public float _FieldRimColorVisibility = 1f;

	// Token: 0x04001CF9 RID: 7417
	[Range(0f, 1f)]
	public float _FieldDischargeVisibility = 0.5f;

	// Token: 0x04001CFA RID: 7418
	[Range(0f, 1f)]
	public float _FieldHighlightVisibility = 0.6f;

	// Token: 0x04001CFB RID: 7419
	public float _FieldHighlightPow = 1.8f;

	// Token: 0x04001CFC RID: 7420
	public float _FieldOutlineThickness;

	// Token: 0x04001CFD RID: 7421
	public float _Octaves = 3f;

	// Token: 0x04001CFE RID: 7422
	public float _Frequency = 2f;

	// Token: 0x04001CFF RID: 7423
	public float _Amplitude = 0.9f;

	// Token: 0x04001D00 RID: 7424
	public float _Lacunarity = 2.5f;

	// Token: 0x04001D01 RID: 7425
	public float _Persistence = 0.5f;

	// Token: 0x04001D02 RID: 7426
	public Vector4 _Offset = new Vector4(0f, 0f, 0f, 0f);

	// Token: 0x04001D03 RID: 7427
	public float _RidgeOffset = 0.15f;

	// Token: 0x04001D04 RID: 7428
	public float _AnimSpeed = 10f;

	// Token: 0x04001D05 RID: 7429
	public float _PowInner = 1.5f;

	// Token: 0x04001D06 RID: 7430
	public float _PowOuter = 5f;

	// Token: 0x04001D07 RID: 7431
	public float _PowInner2 = 2f;

	// Token: 0x04001D08 RID: 7432
	public float _PowOuter2 = 5f;

	// Token: 0x04001D09 RID: 7433
	private AudioSource sound;

	// Token: 0x04001D0A RID: 7434
	private Material mat;

	// Token: 0x04001D0B RID: 7435
	public Vector3[] times = new Vector3[10];
}
