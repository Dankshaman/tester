using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000446 RID: 1094
	public class ShaderPool : SingletonBase<ShaderPool>
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x00152369 File Offset: 0x00150569
		public Shader Geometry2D
		{
			get
			{
				if (this._geometry2D == null)
				{
					this._geometry2D = Shader.Find("Geometry2D");
				}
				return this._geometry2D;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600320F RID: 12815 RVA: 0x0015238F File Offset: 0x0015058F
		public Shader GizmoSolidComponent
		{
			get
			{
				if (this._gizmoSolidComponent == null)
				{
					this._gizmoSolidComponent = Shader.Find("Gizmo Solid Component");
				}
				return this._gizmoSolidComponent;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06003210 RID: 12816 RVA: 0x001523B5 File Offset: 0x001505B5
		public Shader GizmoLine
		{
			get
			{
				if (this._gizmoLine == null)
				{
					this._gizmoLine = Shader.Find("Gizmo Line");
				}
				return this._gizmoLine;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x001523DB File Offset: 0x001505DB
		public Shader GLLine
		{
			get
			{
				if (this._GLLine == null)
				{
					this._GLLine = Shader.Find("GLLine");
				}
				return this._GLLine;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x00152401 File Offset: 0x00150601
		public Shader GradientCameraBk
		{
			get
			{
				if (this._gradientCameraBk == null)
				{
					this._gradientCameraBk = Shader.Find("Gradient Camera Bk");
				}
				return this._gradientCameraBk;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06003213 RID: 12819 RVA: 0x00152427 File Offset: 0x00150627
		public Shader XZGrid
		{
			get
			{
				if (this._xzGrid == null)
				{
					this._xzGrid = Shader.Find("XZGrid");
				}
				return this._xzGrid;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06003214 RID: 12820 RVA: 0x0015244D File Offset: 0x0015064D
		public Shader TintedDiffuse
		{
			get
			{
				if (this._tintedDiffuse == null)
				{
					this._tintedDiffuse = Shader.Find("TintedDiffuse");
				}
				return this._tintedDiffuse;
			}
		}

		// Token: 0x0400204A RID: 8266
		private Shader _geometry2D;

		// Token: 0x0400204B RID: 8267
		private Shader _gizmoSolidComponent;

		// Token: 0x0400204C RID: 8268
		private Shader _gizmoLine;

		// Token: 0x0400204D RID: 8269
		private Shader _GLLine;

		// Token: 0x0400204E RID: 8270
		private Shader _gradientCameraBk;

		// Token: 0x0400204F RID: 8271
		private Shader _xzGrid;

		// Token: 0x04002050 RID: 8272
		private Shader _tintedDiffuse;
	}
}
