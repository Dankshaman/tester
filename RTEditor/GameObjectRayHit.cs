using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000455 RID: 1109
	public class GameObjectRayHit
	{
		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060032BA RID: 12986 RVA: 0x00154634 File Offset: 0x00152834
		public Ray Ray
		{
			get
			{
				return this._ray;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x0015463C File Offset: 0x0015283C
		public GameObject HitObject
		{
			get
			{
				return this._hitObject;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060032BC RID: 12988 RVA: 0x00154644 File Offset: 0x00152844
		public OrientedBoxRayHit ObjectBoxHit
		{
			get
			{
				return this._objectBoxHit;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x0015464C File Offset: 0x0015284C
		public MeshRayHit ObjectMeshHit
		{
			get
			{
				return this._objectMeshHit;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060032BE RID: 12990 RVA: 0x00154654 File Offset: 0x00152854
		public TerrainRayHit ObjectTerrainHit
		{
			get
			{
				return this._objectTerrainHit;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x0015465C File Offset: 0x0015285C
		public SpriteRayHit ObjectSpriteHit
		{
			get
			{
				return this._objectSpriteHit;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060032C0 RID: 12992 RVA: 0x00154664 File Offset: 0x00152864
		public bool WasBoxHit
		{
			get
			{
				return this._objectBoxHit != null;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060032C1 RID: 12993 RVA: 0x0015466F File Offset: 0x0015286F
		public bool WasMeshHit
		{
			get
			{
				return this._objectMeshHit != null && this._hitObject != null;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060032C2 RID: 12994 RVA: 0x00154687 File Offset: 0x00152887
		public bool WasTerrainHit
		{
			get
			{
				return this._objectTerrainHit != null && this._hitObject != null;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x0015469F File Offset: 0x0015289F
		public bool WasSpriteHit
		{
			get
			{
				return this._objectSpriteHit != null && this._hitObject != null;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060032C4 RID: 12996 RVA: 0x001546B8 File Offset: 0x001528B8
		public Vector3 HitPoint
		{
			get
			{
				if (this.WasBoxHit)
				{
					return this._objectBoxHit.HitPoint;
				}
				if (this.WasMeshHit)
				{
					return this._objectMeshHit.HitPoint;
				}
				if (this.WasTerrainHit)
				{
					return this._objectTerrainHit.HitPoint;
				}
				if (this.WasSpriteHit)
				{
					return this._objectSpriteHit.HitPoint;
				}
				return Vector3.zero;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x0015471C File Offset: 0x0015291C
		public Vector3 HitNormal
		{
			get
			{
				if (this.WasBoxHit)
				{
					return this._objectBoxHit.HitNormal;
				}
				if (this.WasMeshHit)
				{
					return this._objectMeshHit.HitNormal;
				}
				if (this.WasTerrainHit)
				{
					return this._objectTerrainHit.HitNormal;
				}
				if (this.WasSpriteHit)
				{
					return this._objectSpriteHit.HitNormal;
				}
				return Vector3.zero;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060032C6 RID: 12998 RVA: 0x00154780 File Offset: 0x00152980
		public float HitEnter
		{
			get
			{
				if (this.WasBoxHit)
				{
					return this._objectBoxHit.HitEnter;
				}
				if (this.WasMeshHit)
				{
					return this._objectMeshHit.HitEnter;
				}
				if (this.WasTerrainHit)
				{
					return this._objectTerrainHit.HitEnter;
				}
				if (this.WasSpriteHit)
				{
					return this._objectSpriteHit.HitEnter;
				}
				return 0f;
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x001547E4 File Offset: 0x001529E4
		public GameObjectRayHit(Ray ray, GameObject hitObject, OrientedBoxRayHit objectBoxHit, MeshRayHit objectMeshHit, TerrainRayHit objectTerrainHit, SpriteRayHit objectSpriteHit)
		{
			this._ray = ray;
			this._hitObject = hitObject;
			if (objectTerrainHit != null)
			{
				this._objectTerrainHit = objectTerrainHit;
				this._objectBoxHit = null;
				this._objectMeshHit = null;
				this._objectSpriteHit = null;
			}
			else if (objectMeshHit != null)
			{
				this._objectTerrainHit = null;
				this._objectBoxHit = null;
				this._objectMeshHit = objectMeshHit;
				this._objectSpriteHit = null;
			}
			else if (objectSpriteHit != null)
			{
				this._objectTerrainHit = null;
				this._objectBoxHit = null;
				this._objectMeshHit = null;
				this._objectSpriteHit = objectSpriteHit;
			}
			if (objectBoxHit != null)
			{
				this._objectTerrainHit = null;
				this._objectBoxHit = objectBoxHit;
				this._objectMeshHit = null;
				this._objectSpriteHit = null;
			}
		}

		// Token: 0x04002089 RID: 8329
		private Ray _ray;

		// Token: 0x0400208A RID: 8330
		private GameObject _hitObject;

		// Token: 0x0400208B RID: 8331
		private OrientedBoxRayHit _objectBoxHit;

		// Token: 0x0400208C RID: 8332
		private MeshRayHit _objectMeshHit;

		// Token: 0x0400208D RID: 8333
		private TerrainRayHit _objectTerrainHit;

		// Token: 0x0400208E RID: 8334
		private SpriteRayHit _objectSpriteHit;
	}
}
