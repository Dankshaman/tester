using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000448 RID: 1096
	[Serializable]
	public class XZGrid
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinLineThickness
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x0014264A File Offset: 0x0014084A
		public static float MinCellSize
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x001525DF File Offset: 0x001507DF
		public static float MinLineFadeZoomFactor
		{
			get
			{
				return 0.0001f;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x00024D16 File Offset: 0x00022F16
		public static int MinColorFadeCellCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x001525E6 File Offset: 0x001507E6
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x001525EE File Offset: 0x001507EE
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				this._isVisible = value;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x001525F7 File Offset: 0x001507F7
		// (set) Token: 0x06003230 RID: 12848 RVA: 0x001525FF File Offset: 0x001507FF
		public float CellSizeX
		{
			get
			{
				return this._cellSizeX;
			}
			set
			{
				this._cellSizeX = Mathf.Max(value, XZGrid.MinCellSize);
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x00152612 File Offset: 0x00150812
		// (set) Token: 0x06003232 RID: 12850 RVA: 0x0015261A File Offset: 0x0015081A
		public float CellSizeZ
		{
			get
			{
				return this._cellSizeZ;
			}
			set
			{
				this._cellSizeZ = Mathf.Max(value, XZGrid.MinCellSize);
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x0015262D File Offset: 0x0015082D
		// (set) Token: 0x06003234 RID: 12852 RVA: 0x00152635 File Offset: 0x00150835
		public Color LineColor
		{
			get
			{
				return this._lineColor;
			}
			set
			{
				this._lineColor = value;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x0015263E File Offset: 0x0015083E
		public Plane Plane
		{
			get
			{
				return new Plane(Vector3.up, Vector3.zero);
			}
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x00152650 File Offset: 0x00150850
		public void Render()
		{
			if (!this._isVisible || Camera.current != MonoSingletonBase<EditorCamera>.Instance.Camera)
			{
				return;
			}
			Camera camera = MonoSingletonBase<EditorCamera>.Instance.Camera;
			float num = Mathf.Abs(camera.transform.position.y);
			int num2 = MathHelper.GetNumberOfDigits((int)num) - 1;
			int num3 = num2 + 1;
			float num4 = Mathf.Pow(10f, (float)num2);
			float num5 = Mathf.Pow(10f, (float)num3);
			Material xzgrid = SingletonBase<MaterialPool>.Instance.XZGrid;
			xzgrid.SetFloat("_CamFarPlane", camera.farClipPlane);
			xzgrid.SetVector("_CamLook", camera.transform.forward);
			xzgrid.SetFloat("_FadeScale", num / 10f);
			float num6 = Mathf.Clamp(1f - (num - num4) / (num5 - num4), 0f, 1f);
			GLPrimitives.DrawGridLines(this._cellSizeX * num4, this._cellSizeZ * num4, camera, xzgrid, new Color(this._lineColor.r, this._lineColor.g, this._lineColor.b, this._lineColor.a * num6));
			GLPrimitives.DrawGridLines(this._cellSizeX * num5, this._cellSizeZ * num5, camera, xzgrid, new Color(this._lineColor.r, this._lineColor.g, this._lineColor.b, this._lineColor.a - this._lineColor.a * num6));
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x001527E0 File Offset: 0x001509E0
		public XZGridCell GetCellFromWorldPoint(Vector3 worldPoint)
		{
			Vector3 vector = this.Plane.ProjectPoint(worldPoint);
			return this.GetCellFromWorldXZ(vector.x, vector.z);
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x0015280C File Offset: 0x00150A0C
		public XZGridCell GetCellFromWorldXZ(float worldX, float worldZ)
		{
			int cellIndexX = Mathf.FloorToInt((worldX + 0.5f * this._cellSizeX) / this._cellSizeX);
			int cellIndexZ = Mathf.FloorToInt((worldZ + 0.5f * this._cellSizeZ) / this._cellSizeZ);
			return new XZGridCell(cellIndexX, cellIndexZ, this);
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x00152858 File Offset: 0x00150A58
		public List<Vector3> GetCellCornerPoints(XZGridCell gridCell)
		{
			float num = (float)gridCell.CellIndexX * this._cellSizeX;
			float num2 = (float)gridCell.CellIndexZ * this._cellSizeX;
			return new List<Vector3>
			{
				new Vector3(num, 0f, num2),
				new Vector3(num + this._cellSizeX, 0f, num2),
				new Vector3(num + this._cellSizeX, 0f, num2 + this._cellSizeZ),
				new Vector3(num, 0f, num2 + this._cellSizeZ)
			};
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x001528EC File Offset: 0x00150AEC
		public Vector3 GetCellCornerPointClosestToInputDevPos()
		{
			Ray ray;
			if (!MonoSingletonBase<InputDevice>.Instance.GetPickRay(MonoSingletonBase<EditorCamera>.Instance.Camera, out ray))
			{
				return Vector3.zero;
			}
			float distance;
			if (this.Plane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				return Vector3Extensions.GetPointClosestToPoint(this.GetCellCornerPoints(this.GetCellFromWorldXZ(point.x, point.z)), point);
			}
			return Vector3.zero;
		}

		// Token: 0x04002059 RID: 8281
		[SerializeField]
		private bool _isVisible = true;

		// Token: 0x0400205A RID: 8282
		[SerializeField]
		private float _cellSizeX = 1f;

		// Token: 0x0400205B RID: 8283
		[SerializeField]
		private float _cellSizeZ = 1f;

		// Token: 0x0400205C RID: 8284
		[SerializeField]
		private Color _lineColor = new Color(0.5f, 0.5f, 0.5f, 0.4f);
	}
}
