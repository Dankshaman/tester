using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Token: 0x02000068 RID: 104
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Draw Call")]
public class UIDrawCall : MonoBehaviour
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000448 RID: 1096 RVA: 0x00020725 File Offset: 0x0001E925
	[Obsolete("Use UIDrawCall.activeList")]
	public static BetterList<UIDrawCall> list
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000449 RID: 1097 RVA: 0x00020725 File Offset: 0x0001E925
	public static BetterList<UIDrawCall> activeList
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600044A RID: 1098 RVA: 0x0002072C File Offset: 0x0001E92C
	public static BetterList<UIDrawCall> inactiveList
	{
		get
		{
			return UIDrawCall.mInactiveList;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600044B RID: 1099 RVA: 0x00020733 File Offset: 0x0001E933
	// (set) Token: 0x0600044C RID: 1100 RVA: 0x0002073B File Offset: 0x0001E93B
	public int renderQueue
	{
		get
		{
			return this.mRenderQueue;
		}
		set
		{
			if (this.mRenderQueue != value)
			{
				this.mRenderQueue = value;
				if (this.mDynamicMat != null)
				{
					this.mDynamicMat.renderQueue = value;
				}
			}
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600044D RID: 1101 RVA: 0x00020767 File Offset: 0x0001E967
	// (set) Token: 0x0600044E RID: 1102 RVA: 0x0002076F File Offset: 0x0001E96F
	public int sortingOrder
	{
		get
		{
			return this.mSortingOrder;
		}
		set
		{
			if (this.mSortingOrder != value)
			{
				this.mSortingOrder = value;
				if (this.mRenderer != null)
				{
					this.mRenderer.sortingOrder = value;
				}
			}
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600044F RID: 1103 RVA: 0x0002079B File Offset: 0x0001E99B
	// (set) Token: 0x06000450 RID: 1104 RVA: 0x000207D8 File Offset: 0x0001E9D8
	public string sortingLayerName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mSortingLayerName))
			{
				return this.mSortingLayerName;
			}
			if (this.mRenderer == null)
			{
				return null;
			}
			this.mSortingLayerName = this.mRenderer.sortingLayerName;
			return this.mSortingLayerName;
		}
		set
		{
			if (this.mRenderer != null && this.mSortingLayerName != value)
			{
				this.mSortingLayerName = value;
				this.mRenderer.sortingLayerName = value;
			}
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000451 RID: 1105 RVA: 0x00020809 File Offset: 0x0001EA09
	public int finalRenderQueue
	{
		get
		{
			if (!(this.mDynamicMat != null))
			{
				return this.mRenderQueue;
			}
			return this.mDynamicMat.renderQueue;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000452 RID: 1106 RVA: 0x0002082B File Offset: 0x0001EA2B
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000453 RID: 1107 RVA: 0x0002084D File Offset: 0x0001EA4D
	// (set) Token: 0x06000454 RID: 1108 RVA: 0x00020855 File Offset: 0x0001EA55
	public Material baseMaterial
	{
		get
		{
			return this.mMaterial;
		}
		set
		{
			if (this.mMaterial != value)
			{
				this.mMaterial = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000455 RID: 1109 RVA: 0x00020873 File Offset: 0x0001EA73
	public Material dynamicMaterial
	{
		get
		{
			return this.mDynamicMat;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000456 RID: 1110 RVA: 0x0002087B File Offset: 0x0001EA7B
	// (set) Token: 0x06000457 RID: 1111 RVA: 0x00020883 File Offset: 0x0001EA83
	public Texture mainTexture
	{
		get
		{
			return this.mTexture;
		}
		set
		{
			this.mTexture = value;
			if (this.mBlock == null)
			{
				this.mBlock = new MaterialPropertyBlock();
			}
			this.mBlock.SetTexture("_MainTex", Utilities.IsNull(value) ? Texture2D.whiteTexture : value);
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000458 RID: 1112 RVA: 0x000208BF File Offset: 0x0001EABF
	// (set) Token: 0x06000459 RID: 1113 RVA: 0x000208C7 File Offset: 0x0001EAC7
	public Shader shader
	{
		get
		{
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				this.mShader = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600045A RID: 1114 RVA: 0x000208E5 File Offset: 0x0001EAE5
	// (set) Token: 0x0600045B RID: 1115 RVA: 0x000208F0 File Offset: 0x0001EAF0
	public UIDrawCall.ShadowMode shadowMode
	{
		get
		{
			return this.mShadowMode;
		}
		set
		{
			if (this.mShadowMode != value)
			{
				this.mShadowMode = value;
				if (this.mRenderer != null)
				{
					if (this.mShadowMode == UIDrawCall.ShadowMode.None)
					{
						this.mRenderer.shadowCastingMode = ShadowCastingMode.Off;
						this.mRenderer.receiveShadows = false;
						return;
					}
					if (this.mShadowMode == UIDrawCall.ShadowMode.Receive)
					{
						this.mRenderer.shadowCastingMode = ShadowCastingMode.Off;
						this.mRenderer.receiveShadows = true;
						return;
					}
					this.mRenderer.shadowCastingMode = ShadowCastingMode.On;
					this.mRenderer.receiveShadows = true;
				}
			}
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x00020976 File Offset: 0x0001EB76
	public int triangles
	{
		get
		{
			if (!(this.mMesh != null))
			{
				return 0;
			}
			return this.mTriangles;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600045D RID: 1117 RVA: 0x0002098E File Offset: 0x0001EB8E
	public bool isClipped
	{
		get
		{
			return this.mClipCount != 0;
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0002099C File Offset: 0x0001EB9C
	private void CreateMaterial()
	{
		this.mTextureClip = false;
		this.mLegacyShader = false;
		this.mClipCount = this.panel.clipCount;
		string text = (this.mShader != null) ? this.mShader.name : ((this.mMaterial != null) ? this.mMaterial.shader.name : "Unlit/Transparent Colored");
		text = text.Replace("GUI/Text Shader", "Unlit/Text");
		if (text.Length > 2 && text[text.Length - 2] == ' ')
		{
			int num = (int)text[text.Length - 1];
			if (num > 48 && num <= 57)
			{
				text = text.Substring(0, text.Length - 2);
			}
		}
		if (text.StartsWith("Hidden/"))
		{
			text = text.Substring(7);
		}
		text = text.Replace(" (SoftClip)", "");
		text = text.Replace(" (TextureClip)", "");
		if (this.panel != null && this.panel.clipping == UIDrawCall.Clipping.TextureMask)
		{
			this.mTextureClip = true;
			this.shader = Shader.Find("Hidden/" + text + " (TextureClip)");
		}
		else if (this.mClipCount != 0)
		{
			this.shader = Shader.Find(string.Concat(new object[]
			{
				"Hidden/",
				text,
				" ",
				this.mClipCount
			}));
			if (this.shader == null)
			{
				this.shader = Shader.Find(text + " " + this.mClipCount);
			}
			if (this.shader == null && this.mClipCount == 1)
			{
				this.mLegacyShader = true;
				this.shader = Shader.Find(text + " (SoftClip)");
			}
		}
		else
		{
			this.shader = Shader.Find(text);
		}
		if (this.shader == null)
		{
			this.shader = Shader.Find("Unlit/Transparent Colored");
		}
		if (this.mMaterial != null)
		{
			this.mDynamicMat = new Material(this.mMaterial);
			this.mDynamicMat.name = "[NGUI] " + this.mMaterial.name;
			this.mDynamicMat.hideFlags = (HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
			this.mDynamicMat.CopyPropertiesFromMaterial(this.mMaterial);
			string[] shaderKeywords = this.mMaterial.shaderKeywords;
			for (int i = 0; i < shaderKeywords.Length; i++)
			{
				this.mDynamicMat.EnableKeyword(shaderKeywords[i]);
			}
			if (this.shader != null)
			{
				this.mDynamicMat.shader = this.shader;
				return;
			}
			if (this.mClipCount != 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					text,
					" shader doesn't have a clipped shader version for ",
					this.mClipCount,
					" clip regions"
				}));
				return;
			}
		}
		else
		{
			this.mDynamicMat = new Material(this.shader);
			this.mDynamicMat.name = "[NGUI] " + this.shader.name;
			this.mDynamicMat.hideFlags = (HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x00020CCC File Offset: 0x0001EECC
	private Material RebuildMaterial()
	{
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.CreateMaterial();
		this.mDynamicMat.renderQueue = this.mRenderQueue;
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[]
			{
				this.mDynamicMat
			};
			this.mRenderer.sortingLayerName = this.mSortingLayerName;
			this.mRenderer.sortingOrder = this.mSortingOrder;
		}
		return this.mDynamicMat;
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00020D4C File Offset: 0x0001EF4C
	private void UpdateMaterials()
	{
		if (this.panel == null)
		{
			return;
		}
		if (this.mRebuildMat || this.mDynamicMat == null || this.mClipCount != this.panel.clipCount || this.mTextureClip != (this.panel.clipping == UIDrawCall.Clipping.TextureMask))
		{
			this.RebuildMaterial();
			this.mRebuildMat = false;
		}
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x00020DB8 File Offset: 0x0001EFB8
	public void UpdateGeometry(int widgetCount)
	{
		this.widgetCount = widgetCount;
		int count = this.verts.Count;
		if (count > 0 && count == this.uvs.Count && count == this.cols.Count && count % 4 == 0)
		{
			if (UIDrawCall.mColorSpace == ColorSpace.Uninitialized)
			{
				UIDrawCall.mColorSpace = QualitySettings.activeColorSpace;
			}
			if (UIDrawCall.mColorSpace == ColorSpace.Linear)
			{
				for (int i = 0; i < count; i++)
				{
					Color color = this.cols[i];
					color.r = Mathf.GammaToLinearSpace(color.r);
					color.g = Mathf.GammaToLinearSpace(color.g);
					color.b = Mathf.GammaToLinearSpace(color.b);
					color.a = Mathf.GammaToLinearSpace(color.a);
					this.cols[i] = color;
				}
			}
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.GetComponent<MeshFilter>();
			}
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (count < 65000)
			{
				int num = (count >> 1) * 3;
				bool flag = this.mIndices == null || this.mIndices.Length != num;
				if (this.mMesh == null)
				{
					this.mMesh = new Mesh();
					this.mMesh.hideFlags = HideFlags.DontSave;
					this.mMesh.name = ((this.mMaterial != null) ? ("[NGUI] " + this.mMaterial.name) : "[NGUI] Mesh");
					if (UIDrawCall.dx9BugWorkaround == 0)
					{
						this.mMesh.MarkDynamic();
					}
					flag = true;
				}
				bool flag2 = this.uvs.Count != count || this.cols.Count != count || this.uv2.Count != count || this.norms.Count != count || this.tans.Count != count;
				if (!flag2 && this.panel != null && this.panel.renderQueue != UIPanel.RenderQueue.Automatic)
				{
					flag2 = (this.mMesh == null || this.mMesh.vertexCount != this.verts.Count);
				}
				if (!flag2 && count << 1 < this.verts.Count)
				{
					flag2 = true;
				}
				this.mTriangles = count >> 1;
				if (this.mMesh.vertexCount != count)
				{
					this.mMesh.Clear();
					flag = true;
				}
				this.mMesh.SetVertices(this.verts);
				this.mMesh.SetUVs(0, this.uvs);
				this.mMesh.SetColors(this.cols);
				this.mMesh.SetUVs(1, (this.uv2.Count == count) ? this.uv2 : null);
				this.mMesh.SetNormals((this.norms.Count == count) ? this.norms : null);
				this.mMesh.SetTangents((this.tans.Count == count) ? this.tans : null);
				if (flag)
				{
					this.mIndices = this.GenerateCachedIndexBuffer(count, num);
					this.mMesh.triangles = this.mIndices;
				}
				if (flag2 || !this.alwaysOnScreen)
				{
					this.mMesh.RecalculateBounds();
				}
				this.mFilter.mesh = this.mMesh;
			}
			else
			{
				this.mTriangles = 0;
				if (this.mMesh != null)
				{
					this.mMesh.Clear();
				}
				Debug.LogError("Too many vertices on one panel: " + count);
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.AddComponent<MeshRenderer>();
				if (this.mShadowMode == UIDrawCall.ShadowMode.None)
				{
					this.mRenderer.shadowCastingMode = ShadowCastingMode.Off;
					this.mRenderer.receiveShadows = false;
				}
				else if (this.mShadowMode == UIDrawCall.ShadowMode.Receive)
				{
					this.mRenderer.shadowCastingMode = ShadowCastingMode.Off;
					this.mRenderer.receiveShadows = true;
				}
				else
				{
					this.mRenderer.shadowCastingMode = ShadowCastingMode.On;
					this.mRenderer.receiveShadows = true;
				}
			}
			if (this.mIsNew)
			{
				this.mIsNew = false;
				if (this.onCreateDrawCall != null)
				{
					this.onCreateDrawCall(this, this.mFilter, this.mRenderer);
				}
			}
			this.UpdateMaterials();
		}
		else
		{
			if (this.mFilter.mesh != null)
			{
				this.mFilter.mesh.Clear();
			}
			Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + count);
		}
		this.verts.Clear();
		this.uvs.Clear();
		this.uv2.Clear();
		this.cols.Clear();
		this.norms.Clear();
		this.tans.Clear();
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x000212AC File Offset: 0x0001F4AC
	private int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
	{
		int i = 0;
		int count = UIDrawCall.mCache.Count;
		while (i < count)
		{
			int[] array = UIDrawCall.mCache[i];
			if (array != null && array.Length == indexCount)
			{
				return array;
			}
			i++;
		}
		int[] array2 = new int[indexCount];
		int num = 0;
		for (int j = 0; j < vertexCount; j += 4)
		{
			array2[num++] = j;
			array2[num++] = j + 1;
			array2[num++] = j + 2;
			array2[num++] = j + 2;
			array2[num++] = j + 3;
			array2[num++] = j;
		}
		if (UIDrawCall.mCache.Count > 10)
		{
			UIDrawCall.mCache.RemoveAt(0);
		}
		UIDrawCall.mCache.Add(array2);
		return array2;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00021368 File Offset: 0x0001F568
	private void OnWillRenderObject()
	{
		this.UpdateMaterials();
		if (this.mBlock != null)
		{
			this.mRenderer.SetPropertyBlock(this.mBlock);
		}
		if (this.onRender != null)
		{
			this.onRender(this.mDynamicMat ?? this.mMaterial);
		}
		if (this.mDynamicMat == null || this.mClipCount == 0)
		{
			return;
		}
		if (this.mTextureClip)
		{
			Vector4 drawCallClipRange = this.panel.drawCallClipRange;
			Vector2 clipSoftness = this.panel.clipSoftness;
			Vector2 vector = new Vector2(1000f, 1000f);
			if (clipSoftness.x > 0f)
			{
				vector.x = drawCallClipRange.z / clipSoftness.x;
			}
			if (clipSoftness.y > 0f)
			{
				vector.y = drawCallClipRange.w / clipSoftness.y;
			}
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[0], new Vector4(-drawCallClipRange.x / drawCallClipRange.z, -drawCallClipRange.y / drawCallClipRange.w, 1f / drawCallClipRange.z, 1f / drawCallClipRange.w));
			this.mDynamicMat.SetTexture("_ClipTex", this.clipTexture);
			return;
		}
		if (!this.mLegacyShader)
		{
			UIPanel parentPanel = this.panel;
			int num = 0;
			while (parentPanel != null)
			{
				if (parentPanel.hasClipping)
				{
					float angle = 0f;
					Vector4 drawCallClipRange2 = parentPanel.drawCallClipRange;
					if (parentPanel != this.panel)
					{
						Vector3 vector2 = parentPanel.cachedTransform.InverseTransformPoint(this.panel.cachedTransform.position);
						drawCallClipRange2.x -= vector2.x;
						drawCallClipRange2.y -= vector2.y;
						Vector3 eulerAngles = this.panel.cachedTransform.rotation.eulerAngles;
						Vector3 vector3 = parentPanel.cachedTransform.rotation.eulerAngles - eulerAngles;
						vector3.x = NGUIMath.WrapAngle(vector3.x);
						vector3.y = NGUIMath.WrapAngle(vector3.y);
						vector3.z = NGUIMath.WrapAngle(vector3.z);
						if (Mathf.Abs(vector3.x) > 0.001f || Mathf.Abs(vector3.y) > 0.001f)
						{
							Debug.LogWarning("Panel can only be clipped properly if X and Y rotation is left at 0", this.panel);
						}
						angle = vector3.z;
					}
					this.SetClipping(num++, drawCallClipRange2, parentPanel.clipSoftness, angle);
				}
				parentPanel = parentPanel.parentPanel;
			}
			return;
		}
		Vector2 clipSoftness2 = this.panel.clipSoftness;
		Vector4 drawCallClipRange3 = this.panel.drawCallClipRange;
		Vector2 mainTextureOffset = new Vector2(-drawCallClipRange3.x / drawCallClipRange3.z, -drawCallClipRange3.y / drawCallClipRange3.w);
		Vector2 mainTextureScale = new Vector2(1f / drawCallClipRange3.z, 1f / drawCallClipRange3.w);
		Vector2 v = new Vector2(1000f, 1000f);
		if (clipSoftness2.x > 0f)
		{
			v.x = drawCallClipRange3.z / clipSoftness2.x;
		}
		if (clipSoftness2.y > 0f)
		{
			v.y = drawCallClipRange3.w / clipSoftness2.y;
		}
		this.mDynamicMat.mainTextureOffset = mainTextureOffset;
		this.mDynamicMat.mainTextureScale = mainTextureScale;
		this.mDynamicMat.SetVector("_ClipSharpness", v);
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x000216F4 File Offset: 0x0001F8F4
	private void SetClipping(int index, Vector4 cr, Vector2 soft, float angle)
	{
		angle *= -0.017453292f;
		Vector2 vector = new Vector2(1000f, 1000f);
		if (soft.x > 0f)
		{
			vector.x = cr.z / soft.x;
		}
		if (soft.y > 0f)
		{
			vector.y = cr.w / soft.y;
		}
		if (index < UIDrawCall.ClipRange.Length)
		{
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[index], new Vector4(-cr.x / cr.z, -cr.y / cr.w, 1f / cr.z, 1f / cr.w));
			this.mDynamicMat.SetVector(UIDrawCall.ClipArgs[index], new Vector4(vector.x, vector.y, Mathf.Sin(angle), Mathf.Cos(angle)));
		}
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x000217E4 File Offset: 0x0001F9E4
	private void Awake()
	{
		if (UIDrawCall.dx9BugWorkaround == -1)
		{
			UIDrawCall.dx9BugWorkaround = ((Application.platform == RuntimePlatform.WindowsPlayer && SystemInfo.graphicsShaderLevel < 40 && SystemInfo.graphicsDeviceVersion.Contains("Direct3D")) ? 1 : 0);
		}
		if (UIDrawCall.ClipRange == null)
		{
			UIDrawCall.ClipRange = new int[]
			{
				Shader.PropertyToID("_ClipRange0"),
				Shader.PropertyToID("_ClipRange1"),
				Shader.PropertyToID("_ClipRange2"),
				Shader.PropertyToID("_ClipRange4")
			};
		}
		if (UIDrawCall.ClipArgs == null)
		{
			UIDrawCall.ClipArgs = new int[]
			{
				Shader.PropertyToID("_ClipArgs0"),
				Shader.PropertyToID("_ClipArgs1"),
				Shader.PropertyToID("_ClipArgs2"),
				Shader.PropertyToID("_ClipArgs3")
			};
		}
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x000218B0 File Offset: 0x0001FAB0
	private void OnEnable()
	{
		this.mRebuildMat = true;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000218BC File Offset: 0x0001FABC
	private void OnDisable()
	{
		this.depthStart = int.MaxValue;
		this.depthEnd = int.MinValue;
		this.panel = null;
		this.manager = null;
		this.mMaterial = null;
		this.mTexture = null;
		this.clipTexture = null;
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[0];
		}
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.mDynamicMat = null;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00021933 File Offset: 0x0001FB33
	private void OnDestroy()
	{
		NGUITools.DestroyImmediate(this.mMesh);
		this.mMesh = null;
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00021947 File Offset: 0x0001FB47
	public static UIDrawCall Create(UIPanel panel, Material mat, Texture tex, Shader shader)
	{
		return UIDrawCall.Create(null, panel, mat, tex, shader);
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00021954 File Offset: 0x0001FB54
	private static UIDrawCall Create(string name, UIPanel pan, Material mat, Texture tex, Shader shader)
	{
		UIDrawCall uidrawCall = UIDrawCall.Create(name);
		uidrawCall.gameObject.layer = pan.cachedGameObject.layer;
		uidrawCall.baseMaterial = mat;
		uidrawCall.mainTexture = tex;
		uidrawCall.shader = shader;
		uidrawCall.renderQueue = pan.startingRenderQueue;
		uidrawCall.sortingOrder = pan.sortingOrder;
		uidrawCall.manager = pan;
		return uidrawCall;
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x000219B4 File Offset: 0x0001FBB4
	private static UIDrawCall Create(string name)
	{
		while (UIDrawCall.mInactiveList.size > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList.Pop();
			if (uidrawCall != null)
			{
				UIDrawCall.mActiveList.Add(uidrawCall);
				if (name != null)
				{
					uidrawCall.name = name;
				}
				NGUITools.SetActive(uidrawCall.gameObject, true);
				return uidrawCall;
			}
		}
		GameObject gameObject = new GameObject(name);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		UIDrawCall uidrawCall2 = gameObject.AddComponent<UIDrawCall>();
		UIDrawCall.mActiveList.Add(uidrawCall2);
		return uidrawCall2;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00021A28 File Offset: 0x0001FC28
	public static void ClearAll()
	{
		bool isPlaying = Application.isPlaying;
		int i = UIDrawCall.mActiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[--i];
			if (uidrawCall)
			{
				if (isPlaying)
				{
					NGUITools.SetActive(uidrawCall.gameObject, false);
				}
				else
				{
					NGUITools.DestroyImmediate(uidrawCall.gameObject);
				}
			}
		}
		UIDrawCall.mActiveList.Clear();
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00021A8A File Offset: 0x0001FC8A
	public static void ReleaseAll()
	{
		UIDrawCall.ClearAll();
		UIDrawCall.ReleaseInactive();
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00021A98 File Offset: 0x0001FC98
	public static void ReleaseInactive()
	{
		int i = UIDrawCall.mInactiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList[--i];
			if (uidrawCall)
			{
				NGUITools.DestroyImmediate(uidrawCall.gameObject);
			}
		}
		UIDrawCall.mInactiveList.Clear();
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00021AE4 File Offset: 0x0001FCE4
	public static int Count(UIPanel panel)
	{
		int num = 0;
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			if (UIDrawCall.mActiveList[i].manager == panel)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00021B28 File Offset: 0x0001FD28
	public static void Destroy(UIDrawCall dc)
	{
		if (dc)
		{
			if (dc.onCreateDrawCall != null)
			{
				NGUITools.Destroy(dc.gameObject);
				return;
			}
			dc.onRender = null;
			if (Application.isPlaying)
			{
				if (UIDrawCall.mActiveList.Remove(dc))
				{
					NGUITools.SetActive(dc.gameObject, false);
					UIDrawCall.mInactiveList.Add(dc);
					dc.mIsNew = true;
					return;
				}
			}
			else
			{
				UIDrawCall.mActiveList.Remove(dc);
				NGUITools.DestroyImmediate(dc.gameObject);
			}
		}
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00021BA4 File Offset: 0x0001FDA4
	public static void MoveToScene(Scene scene)
	{
		foreach (UIDrawCall uidrawCall in UIDrawCall.activeList)
		{
			SceneManager.MoveGameObjectToScene(uidrawCall.gameObject, scene);
		}
		foreach (UIDrawCall uidrawCall2 in UIDrawCall.inactiveList)
		{
			SceneManager.MoveGameObjectToScene(uidrawCall2.gameObject, scene);
		}
	}

	// Token: 0x040002FF RID: 767
	private static BetterList<UIDrawCall> mActiveList = new BetterList<UIDrawCall>();

	// Token: 0x04000300 RID: 768
	private static BetterList<UIDrawCall> mInactiveList = new BetterList<UIDrawCall>();

	// Token: 0x04000301 RID: 769
	[HideInInspector]
	[NonSerialized]
	public int widgetCount;

	// Token: 0x04000302 RID: 770
	[HideInInspector]
	[NonSerialized]
	public int depthStart = int.MaxValue;

	// Token: 0x04000303 RID: 771
	[HideInInspector]
	[NonSerialized]
	public int depthEnd = int.MinValue;

	// Token: 0x04000304 RID: 772
	[HideInInspector]
	[NonSerialized]
	public UIPanel manager;

	// Token: 0x04000305 RID: 773
	[HideInInspector]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x04000306 RID: 774
	[HideInInspector]
	[NonSerialized]
	public Texture2D clipTexture;

	// Token: 0x04000307 RID: 775
	[HideInInspector]
	[NonSerialized]
	public bool alwaysOnScreen;

	// Token: 0x04000308 RID: 776
	[HideInInspector]
	[NonSerialized]
	public List<Vector3> verts = new List<Vector3>();

	// Token: 0x04000309 RID: 777
	[HideInInspector]
	[NonSerialized]
	public List<Vector3> norms = new List<Vector3>();

	// Token: 0x0400030A RID: 778
	[HideInInspector]
	[NonSerialized]
	public List<Vector4> tans = new List<Vector4>();

	// Token: 0x0400030B RID: 779
	[HideInInspector]
	[NonSerialized]
	public List<Vector2> uvs = new List<Vector2>();

	// Token: 0x0400030C RID: 780
	[HideInInspector]
	[NonSerialized]
	public List<Vector4> uv2 = new List<Vector4>();

	// Token: 0x0400030D RID: 781
	[HideInInspector]
	[NonSerialized]
	public List<Color> cols = new List<Color>();

	// Token: 0x0400030E RID: 782
	[NonSerialized]
	private Material mMaterial;

	// Token: 0x0400030F RID: 783
	[NonSerialized]
	private Texture mTexture;

	// Token: 0x04000310 RID: 784
	[NonSerialized]
	private Shader mShader;

	// Token: 0x04000311 RID: 785
	[NonSerialized]
	private int mClipCount;

	// Token: 0x04000312 RID: 786
	[NonSerialized]
	private Transform mTrans;

	// Token: 0x04000313 RID: 787
	[NonSerialized]
	private Mesh mMesh;

	// Token: 0x04000314 RID: 788
	[NonSerialized]
	private MeshFilter mFilter;

	// Token: 0x04000315 RID: 789
	[NonSerialized]
	private MeshRenderer mRenderer;

	// Token: 0x04000316 RID: 790
	[NonSerialized]
	private Material mDynamicMat;

	// Token: 0x04000317 RID: 791
	[NonSerialized]
	private int[] mIndices;

	// Token: 0x04000318 RID: 792
	[NonSerialized]
	private UIDrawCall.ShadowMode mShadowMode;

	// Token: 0x04000319 RID: 793
	[NonSerialized]
	private bool mRebuildMat = true;

	// Token: 0x0400031A RID: 794
	[NonSerialized]
	private bool mLegacyShader;

	// Token: 0x0400031B RID: 795
	[NonSerialized]
	private int mRenderQueue = 3000;

	// Token: 0x0400031C RID: 796
	[NonSerialized]
	private int mTriangles;

	// Token: 0x0400031D RID: 797
	[NonSerialized]
	public bool isDirty;

	// Token: 0x0400031E RID: 798
	[NonSerialized]
	private bool mTextureClip;

	// Token: 0x0400031F RID: 799
	[NonSerialized]
	private bool mIsNew = true;

	// Token: 0x04000320 RID: 800
	public UIDrawCall.OnRenderCallback onRender;

	// Token: 0x04000321 RID: 801
	public UIDrawCall.OnCreateDrawCall onCreateDrawCall;

	// Token: 0x04000322 RID: 802
	[NonSerialized]
	private string mSortingLayerName;

	// Token: 0x04000323 RID: 803
	[NonSerialized]
	private int mSortingOrder;

	// Token: 0x04000324 RID: 804
	private static ColorSpace mColorSpace = ColorSpace.Uninitialized;

	// Token: 0x04000325 RID: 805
	private const int maxIndexBufferCache = 10;

	// Token: 0x04000326 RID: 806
	private static List<int[]> mCache = new List<int[]>(10);

	// Token: 0x04000327 RID: 807
	protected MaterialPropertyBlock mBlock;

	// Token: 0x04000328 RID: 808
	private static int[] ClipRange = null;

	// Token: 0x04000329 RID: 809
	private static int[] ClipArgs = null;

	// Token: 0x0400032A RID: 810
	private static int dx9BugWorkaround = -1;

	// Token: 0x02000534 RID: 1332
	public enum Clipping
	{
		// Token: 0x04002439 RID: 9273
		None,
		// Token: 0x0400243A RID: 9274
		TextureMask,
		// Token: 0x0400243B RID: 9275
		SoftClip = 3,
		// Token: 0x0400243C RID: 9276
		ConstrainButDontClip
	}

	// Token: 0x02000535 RID: 1333
	// (Invoke) Token: 0x0600379C RID: 14236
	public delegate void OnRenderCallback(Material mat);

	// Token: 0x02000536 RID: 1334
	// (Invoke) Token: 0x060037A0 RID: 14240
	public delegate void OnCreateDrawCall(UIDrawCall dc, MeshFilter filter, MeshRenderer ren);

	// Token: 0x02000537 RID: 1335
	public enum ShadowMode
	{
		// Token: 0x0400243E RID: 9278
		None,
		// Token: 0x0400243F RID: 9279
		Receive,
		// Token: 0x04002440 RID: 9280
		CastAndReceive
	}
}
