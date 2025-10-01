using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000447 RID: 1095
	[ExecuteInEditMode]
	[Serializable]
	public class RuntimeEditorApplication : MonoSingletonBase<RuntimeEditorApplication>
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x0015247B File Offset: 0x0015067B
		public static Vector3 MinObjectVolumeSize
		{
			get
			{
				return new Vector3(0.001f, 0.001f, 0.001f);
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06003217 RID: 12823 RVA: 0x00152491 File Offset: 0x00150691
		// (set) Token: 0x06003218 RID: 12824 RVA: 0x00152499 File Offset: 0x00150699
		public bool UseUnityColliders
		{
			get
			{
				return this._useUnityColliders;
			}
			set
			{
				this._useUnityColliders = value;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06003219 RID: 12825 RVA: 0x001524A2 File Offset: 0x001506A2
		// (set) Token: 0x0600321A RID: 12826 RVA: 0x000025B8 File Offset: 0x000007B8
		public bool EnableUndoRedo
		{
			get
			{
				return this._enableUndoRedo;
			}
			set
			{
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600321B RID: 12827 RVA: 0x001524AA File Offset: 0x001506AA
		// (set) Token: 0x0600321C RID: 12828 RVA: 0x001524B2 File Offset: 0x001506B2
		public bool UseCustomCamera
		{
			get
			{
				return this._useCustomCamera;
			}
			set
			{
				if (Application.isEditor && !Application.isPlaying && this._useCustomCamera != value)
				{
					this._useCustomCamera = value;
				}
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x0600321D RID: 12829 RVA: 0x001524D2 File Offset: 0x001506D2
		// (set) Token: 0x0600321E RID: 12830 RVA: 0x000025B8 File Offset: 0x000007B8
		public Camera CustomCamera
		{
			get
			{
				return this._customCamera;
			}
			set
			{
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600321F RID: 12831 RVA: 0x001524DA File Offset: 0x001506DA
		// (set) Token: 0x06003220 RID: 12832 RVA: 0x001524E2 File Offset: 0x001506E2
		public Vector3 VolumeSizeForLightObjects
		{
			get
			{
				return this._volumeSizeForLightObjects;
			}
			set
			{
				this._volumeSizeForLightObjects = Vector3.Max(RuntimeEditorApplication.MinObjectVolumeSize, value.GetVectorWithAbsComponents());
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x001524FA File Offset: 0x001506FA
		// (set) Token: 0x06003222 RID: 12834 RVA: 0x00152502 File Offset: 0x00150702
		public Vector3 VolumeSizeForParticleSystemObjects
		{
			get
			{
				return this._volumeSizeForParticleSystemObjects;
			}
			set
			{
				this._volumeSizeForParticleSystemObjects = Vector3.Max(RuntimeEditorApplication.MinObjectVolumeSize, value.GetVectorWithAbsComponents());
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06003223 RID: 12835 RVA: 0x0015251A File Offset: 0x0015071A
		// (set) Token: 0x06003224 RID: 12836 RVA: 0x00152522 File Offset: 0x00150722
		public Vector3 VolumeSizeForEmptyObjects
		{
			get
			{
				return this._volumeSizeForEmptyObjects;
			}
			set
			{
				this._volumeSizeForEmptyObjects = Vector3.Max(RuntimeEditorApplication.MinObjectVolumeSize, value.GetVectorWithAbsComponents());
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x0015253A File Offset: 0x0015073A
		public XZGrid XZGrid
		{
			get
			{
				return this._xzGrid;
			}
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x00152542 File Offset: 0x00150742
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			SingletonBase<EditorScene>.Instance.Update();
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x00152556 File Offset: 0x00150756
		private void OnRenderObject()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this._xzGrid.Render();
		}

		// Token: 0x04002051 RID: 8273
		[SerializeField]
		private bool _useUnityColliders;

		// Token: 0x04002052 RID: 8274
		[SerializeField]
		private bool _enableUndoRedo = true;

		// Token: 0x04002053 RID: 8275
		[SerializeField]
		private bool _useCustomCamera;

		// Token: 0x04002054 RID: 8276
		[SerializeField]
		private Camera _customCamera;

		// Token: 0x04002055 RID: 8277
		[SerializeField]
		private Vector3 _volumeSizeForLightObjects = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x04002056 RID: 8278
		[SerializeField]
		private Vector3 _volumeSizeForParticleSystemObjects = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x04002057 RID: 8279
		[SerializeField]
		private Vector3 _volumeSizeForEmptyObjects = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x04002058 RID: 8280
		[SerializeField]
		private XZGrid _xzGrid = new XZGrid();
	}
}
