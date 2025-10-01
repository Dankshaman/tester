using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000444 RID: 1092
	public class MaterialPool : SingletonBase<MaterialPool>
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x00152117 File Offset: 0x00150317
		public Material Geometry2D
		{
			get
			{
				if (this._geometry2D == null)
				{
					this._geometry2D = new Material(SingletonBase<ShaderPool>.Instance.Geometry2D);
				}
				return this._geometry2D;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06003200 RID: 12800 RVA: 0x00152142 File Offset: 0x00150342
		public Material GizmoSolidComponent
		{
			get
			{
				if (this._gizmoSolidComponent == null)
				{
					this._gizmoSolidComponent = new Material(SingletonBase<ShaderPool>.Instance.GizmoSolidComponent);
				}
				return this._gizmoSolidComponent;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x0015216D File Offset: 0x0015036D
		public Material GizmoLine
		{
			get
			{
				if (this._gizmoLine == null)
				{
					this._gizmoLine = new Material(SingletonBase<ShaderPool>.Instance.GizmoLine);
				}
				return this._gizmoLine;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x00152198 File Offset: 0x00150398
		public Material GLLine
		{
			get
			{
				if (this._GLLine == null)
				{
					this._GLLine = new Material(SingletonBase<ShaderPool>.Instance.GLLine);
				}
				return this._GLLine;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x001521C3 File Offset: 0x001503C3
		public Material GradientCameraBk
		{
			get
			{
				if (this._gradientCameraBk == null)
				{
					this._gradientCameraBk = new Material(SingletonBase<ShaderPool>.Instance.GradientCameraBk);
				}
				return this._gradientCameraBk;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06003204 RID: 12804 RVA: 0x001521EE File Offset: 0x001503EE
		public Material XZGrid
		{
			get
			{
				if (this._xzGrid == null)
				{
					this._xzGrid = new Material(SingletonBase<ShaderPool>.Instance.XZGrid);
				}
				return this._xzGrid;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x00152219 File Offset: 0x00150419
		public Material TintedDiffuse
		{
			get
			{
				if (this._tintedDiffuse == null)
				{
					this._tintedDiffuse = new Material(SingletonBase<ShaderPool>.Instance.TintedDiffuse);
				}
				return this._tintedDiffuse;
			}
		}

		// Token: 0x0400203D RID: 8253
		private Material _geometry2D;

		// Token: 0x0400203E RID: 8254
		private Material _gizmoSolidComponent;

		// Token: 0x0400203F RID: 8255
		private Material _gizmoLine;

		// Token: 0x04002040 RID: 8256
		private Material _GLLine;

		// Token: 0x04002041 RID: 8257
		private Material _gradientCameraBk;

		// Token: 0x04002042 RID: 8258
		private Material _xzGrid;

		// Token: 0x04002043 RID: 8259
		private Material _tintedDiffuse;
	}
}
