using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003B8 RID: 952
	[Serializable]
	public class EditorCameraBk
	{
		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x0013AF7B File Offset: 0x0013917B
		public static float MinGradientOffset
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002D17 RID: 11543 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static float MaxGradientOffset
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x0013AF82 File Offset: 0x00139182
		// (set) Token: 0x06002D19 RID: 11545 RVA: 0x0013AF8A File Offset: 0x0013918A
		public Color TopColor
		{
			get
			{
				return this._topColor;
			}
			set
			{
				this._topColor = value;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x0013AF93 File Offset: 0x00139193
		// (set) Token: 0x06002D1B RID: 11547 RVA: 0x0013AF9B File Offset: 0x0013919B
		public Color BottomColor
		{
			get
			{
				return this._bottomColor;
			}
			set
			{
				this._bottomColor = value;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x0013AFA4 File Offset: 0x001391A4
		// (set) Token: 0x06002D1D RID: 11549 RVA: 0x0013AFAC File Offset: 0x001391AC
		public float GradientOffset
		{
			get
			{
				return this._gradientOffset;
			}
			set
			{
				this._gradientOffset = Mathf.Clamp(value, EditorCameraBk.MinGradientOffset, EditorCameraBk.MaxGradientOffset);
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x0013AFC4 File Offset: 0x001391C4
		// (set) Token: 0x06002D1F RID: 11551 RVA: 0x0013AFCC File Offset: 0x001391CC
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				this._isVisible = value;
				if (Application.isPlaying)
				{
					this.GetBkGameObject().SetActive(this._isVisible);
				}
			}
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x0013AFED File Offset: 0x001391ED
		public bool IsSameAs(GameObject gameObject)
		{
			return gameObject == this.GetBkGameObject();
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x0013AFFC File Offset: 0x001391FC
		public void OnCameraUpdate(Camera camera, bool isDoingPerspectiveSwitch)
		{
			Transform transform = camera.transform;
			Transform transform2 = this.GetBkGameObject().transform;
			transform2.position = transform.position + transform.forward * camera.farClipPlane * 0.98f;
			transform2.rotation = transform.rotation;
			transform2.parent = null;
			CameraViewVolume viewVolume = camera.GetViewVolume();
			transform2.localScale = (isDoingPerspectiveSwitch ? new Vector3(9999f, 9999f, 1f) : new Vector3(viewVolume.FarPlaneSize.x, viewVolume.FarPlaneSize.y, 1f));
			transform2.parent = transform;
			Material gradientCameraBk = SingletonBase<MaterialPool>.Instance.GradientCameraBk;
			gradientCameraBk.SetVector("_TopColor", this._topColor);
			gradientCameraBk.SetVector("_BottomColor", this._bottomColor);
			gradientCameraBk.SetFloat("_GradientOffset", this._gradientOffset);
			gradientCameraBk.SetFloat("_Height", transform2.localScale.y);
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x0013B104 File Offset: 0x00139304
		private GameObject GetBkGameObject()
		{
			if (this._bkObject == null)
			{
				this._bkObject = new GameObject("Editor Camera Bk Object");
				this._bkObject.AddComponent<MeshFilter>().sharedMesh = SingletonBase<MeshPool>.Instance.XYSquareMesh;
				this._bkObject.AddComponent<MeshRenderer>().sharedMaterial = SingletonBase<MaterialPool>.Instance.GradientCameraBk;
				this._bkObject.SetActive(this.IsVisible);
			}
			return this._bkObject;
		}

		// Token: 0x04001E3A RID: 7738
		[SerializeField]
		private bool _isVisible;

		// Token: 0x04001E3B RID: 7739
		[SerializeField]
		private Color _topColor = new Color(0.2784314f, 0.2784314f, 0.2784314f, 1f);

		// Token: 0x04001E3C RID: 7740
		[SerializeField]
		private Color _bottomColor = new Color(0f, 0f, 0f, 1f);

		// Token: 0x04001E3D RID: 7741
		[SerializeField]
		private float _gradientOffset;

		// Token: 0x04001E3E RID: 7742
		private GameObject _bkObject;
	}
}
