using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class UICustomMesh : UICustomObject<UICustomMesh>
{
	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06002269 RID: 8809 RVA: 0x000F6AC0 File Offset: 0x000F4CC0
	// (set) Token: 0x0600226A RID: 8810 RVA: 0x000F6ACD File Offset: 0x000F4CCD
	private string MeshURL
	{
		get
		{
			return this.MeshInput.value;
		}
		set
		{
			this.MeshInput.value = value;
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x0600226B RID: 8811 RVA: 0x000F6ADB File Offset: 0x000F4CDB
	// (set) Token: 0x0600226C RID: 8812 RVA: 0x000F6AE8 File Offset: 0x000F4CE8
	private string DiffuseURL
	{
		get
		{
			return this.ImageInput.value;
		}
		set
		{
			this.ImageInput.value = value;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x0600226D RID: 8813 RVA: 0x000F6AF6 File Offset: 0x000F4CF6
	// (set) Token: 0x0600226E RID: 8814 RVA: 0x000F6B03 File Offset: 0x000F4D03
	private string NormalURL
	{
		get
		{
			return this.NormalInput.value;
		}
		set
		{
			this.NormalInput.value = value;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x0600226F RID: 8815 RVA: 0x000F6B11 File Offset: 0x000F4D11
	// (set) Token: 0x06002270 RID: 8816 RVA: 0x000F6B1E File Offset: 0x000F4D1E
	private string ColliderURL
	{
		get
		{
			return this.ColliderInput.value;
		}
		set
		{
			this.ColliderInput.value = value;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06002271 RID: 8817 RVA: 0x000F6B2C File Offset: 0x000F4D2C
	// (set) Token: 0x06002272 RID: 8818 RVA: 0x000F6B39 File Offset: 0x000F4D39
	private bool NonConvex
	{
		get
		{
			return this.NonConvexToggle.value;
		}
		set
		{
			this.NonConvexToggle.value = value;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06002273 RID: 8819 RVA: 0x000F6B48 File Offset: 0x000F4D48
	// (set) Token: 0x06002274 RID: 8820 RVA: 0x000F6B7C File Offset: 0x000F4D7C
	private int TypeIndex
	{
		get
		{
			for (int i = 0; i < this.TypeToggles.Length; i++)
			{
				if (this.TypeToggles[i].value)
				{
					return i;
				}
			}
			return 0;
		}
		set
		{
			int group = this.TypeToggles[0].group;
			for (int i = 0; i < this.TypeToggles.Length; i++)
			{
				this.TypeToggles[i].group = 0;
				this.TypeToggles[i].value = (i == value);
				this.TypeToggles[i].group = group;
			}
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06002275 RID: 8821 RVA: 0x000F6BD7 File Offset: 0x000F4DD7
	// (set) Token: 0x06002276 RID: 8822 RVA: 0x000F6BF4 File Offset: 0x000F4DF4
	private int MaterialIndex
	{
		get
		{
			return this.MaterialPopupList.items.IndexOf(this.MaterialPopupList.value);
		}
		set
		{
			this.MaterialPopupList.value = this.MaterialPopupList.items[value];
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06002277 RID: 8823 RVA: 0x000F6C12 File Offset: 0x000F4E12
	// (set) Token: 0x06002278 RID: 8824 RVA: 0x000F6C1F File Offset: 0x000F4E1F
	private bool CastShadows
	{
		get
		{
			return this.CastShadowsToggle.value;
		}
		set
		{
			this.CastShadowsToggle.value = value;
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06002279 RID: 8825 RVA: 0x000F6C30 File Offset: 0x000F4E30
	// (set) Token: 0x0600227A RID: 8826 RVA: 0x000F6CA8 File Offset: 0x000F4EA8
	public CustomShaderState CustomShader
	{
		get
		{
			CustomShaderState customShaderState = new CustomShaderState();
			float specularIntensity;
			float.TryParse(this.SpecularIntensityInput.value, out specularIntensity);
			customShaderState.SpecularIntensity = specularIntensity;
			customShaderState.SpecularColor = new ColourState(this.ColorInput.GetColor());
			customShaderState.SpecularSharpness = 6f * this.SpecularSharpnessSlider.value + 2f;
			customShaderState.FresnelStrength = this.FresnalStrengthSlider.value;
			return customShaderState;
		}
		set
		{
			this.SpecularIntensityInput.value = value.SpecularIntensity.ToString();
			this.ColorInput.SetColor(value.SpecularColor.ToColour());
			this.SpecularSharpnessSlider.value = (value.SpecularSharpness - 2f) / 6f;
			this.FresnalStrengthSlider.value = value.FresnelStrength;
		}
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x000F6D18 File Offset: 0x000F4F18
	private void Start()
	{
		this.MeshInput.SelectAllTextOnClick = true;
		this.ImageInput.SelectAllTextOnClick = true;
		this.NormalInput.SelectAllTextOnClick = true;
		this.ColliderInput.SelectAllTextOnClick = true;
		EventDelegate.Add(this.MaterialPopupList.onChange, new EventDelegate.Callback(this.UpdateShaderGUI));
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x000F6D72 File Offset: 0x000F4F72
	private void OnDestroy()
	{
		EventDelegate.Remove(this.MaterialPopupList.onChange, new EventDelegate.Callback(this.UpdateShaderGUI));
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x000F6D94 File Offset: 0x000F4F94
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomMesh = this.TargetCustomObject.GetComponent<CustomMesh>();
		if (!this.TargetCustomMesh)
		{
			return;
		}
		this.MaterialPopupList.items = new List<string>(CustomMesh.MaterialList);
		this.SetupCustomShaders();
		CustomMeshState customMeshState = this.TargetCustomMesh.customMeshState;
		this.MeshURL = customMeshState.MeshURL;
		this.DiffuseURL = customMeshState.DiffuseURL;
		this.NormalURL = customMeshState.NormalURL;
		this.ColliderURL = customMeshState.ColliderURL;
		this.MaterialIndex = customMeshState.MaterialIndex;
		this.TypeIndex = customMeshState.TypeIndex;
		this.NonConvex = !customMeshState.Convex;
		this.CastShadows = customMeshState.CastShadows;
		if (customMeshState.CustomShader == null)
		{
			this.UpdateShaderGUI();
			return;
		}
		this.CustomShader = customMeshState.CustomShader;
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x000F6E74 File Offset: 0x000F5074
	private void SetupCustomShaders()
	{
		this.CustomShaderDict.Clear();
		for (int i = 0; i < CustomMesh.MaterialList.Length; i++)
		{
			this.CustomShaderDict.Add(i, this.TargetCustomMesh.GetCustomShaderState(i));
		}
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x000F6EB8 File Offset: 0x000F50B8
	public override void Import()
	{
		this.MeshURL = this.MeshURL.Trim();
		this.DiffuseURL = this.DiffuseURL.Trim();
		this.NormalURL = this.NormalURL.Trim();
		this.ColliderURL = this.ColliderURL.Trim();
		if (string.IsNullOrEmpty(this.MeshURL))
		{
			Chat.LogError("You must supply a model URL to create a custom model.", true);
			return;
		}
		base.CheckUpdateMatchingCustomObjects();
		this.TargetCustomMesh.customMeshState = new CustomMeshState();
		CustomMeshState customMeshState = this.TargetCustomMesh.customMeshState;
		customMeshState.MeshURL = this.MeshURL;
		customMeshState.DiffuseURL = this.DiffuseURL;
		customMeshState.NormalURL = this.NormalURL;
		customMeshState.ColliderURL = this.ColliderURL;
		customMeshState.MaterialIndex = this.MaterialIndex;
		customMeshState.TypeIndex = this.TypeIndex;
		customMeshState.Convex = !this.NonConvex;
		customMeshState.CastShadows = this.CastShadows;
		CustomShaderState left = this.CustomShaderDict[this.MaterialIndex];
		CustomShaderState customShader = this.CustomShader;
		if (left != customShader)
		{
			Debug.Log("Override shader!");
			customMeshState.CustomShader = customShader;
		}
		else
		{
			customMeshState.CustomShader = null;
		}
		if (Network.isServer)
		{
			this.TargetCustomMesh.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000F6FF9 File Offset: 0x000F51F9
	public void UpdateShaderGUI()
	{
		this.CustomShader = this.CustomShaderDict[this.MaterialIndex];
	}

	// Token: 0x040015BC RID: 5564
	public UIInput MeshInput;

	// Token: 0x040015BD RID: 5565
	public UIInput ImageInput;

	// Token: 0x040015BE RID: 5566
	public UIInput NormalInput;

	// Token: 0x040015BF RID: 5567
	public UIInput ColliderInput;

	// Token: 0x040015C0 RID: 5568
	public UIToggle NonConvexToggle;

	// Token: 0x040015C1 RID: 5569
	public UIToggle[] TypeToggles;

	// Token: 0x040015C2 RID: 5570
	public UIPopupList MaterialPopupList;

	// Token: 0x040015C3 RID: 5571
	public UIInput SpecularIntensityInput;

	// Token: 0x040015C4 RID: 5572
	public UIInput[] SpecularColorInputs;

	// Token: 0x040015C5 RID: 5573
	public UIColorPickerInput ColorInput;

	// Token: 0x040015C6 RID: 5574
	public UISlider SpecularSharpnessSlider;

	// Token: 0x040015C7 RID: 5575
	public UISlider FresnalStrengthSlider;

	// Token: 0x040015C8 RID: 5576
	public UIToggle CastShadowsToggle;

	// Token: 0x040015C9 RID: 5577
	private CustomShaderState CustomShaderPlastic;

	// Token: 0x040015CA RID: 5578
	private CustomShaderState CustomShaderWood;

	// Token: 0x040015CB RID: 5579
	private CustomShaderState CustomShaderMetal;

	// Token: 0x040015CC RID: 5580
	private CustomShaderState CustomShaderCardboard;

	// Token: 0x040015CD RID: 5581
	private CustomShaderState CustomShaderGlass;

	// Token: 0x040015CE RID: 5582
	private CustomMesh TargetCustomMesh;

	// Token: 0x040015CF RID: 5583
	private Dictionary<int, CustomShaderState> CustomShaderDict = new Dictionary<int, CustomShaderState>();
}
