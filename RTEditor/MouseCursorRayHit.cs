using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x02000458 RID: 1112
	public class MouseCursorRayHit
	{
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x00154968 File Offset: 0x00152B68
		public GridCellRayHit GridCellRayHit
		{
			get
			{
				return this._gridCellRayHit;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x00154970 File Offset: 0x00152B70
		public List<GameObjectRayHit> SortedObjectRayHits
		{
			get
			{
				return this._sortedObjectRayHits;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x00154978 File Offset: 0x00152B78
		public GameObjectRayHit ClosestObjectRayHit
		{
			get
			{
				if (this._sortedObjectRayHits.Count == 0)
				{
					return null;
				}
				return this._sortedObjectRayHits[0];
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060032D7 RID: 13015 RVA: 0x00154995 File Offset: 0x00152B95
		public bool WasACellHit
		{
			get
			{
				return this._gridCellRayHit != null;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x001549A0 File Offset: 0x00152BA0
		public bool WasAnObjectHit
		{
			get
			{
				this._sortedObjectRayHits.RemoveAll((GameObjectRayHit item) => item.HitObject == null);
				return this._sortedObjectRayHits.Count != 0;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x001549DB File Offset: 0x00152BDB
		public bool WasAnythingHit
		{
			get
			{
				return this.WasACellHit || this.WasAnObjectHit;
			}
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x001549ED File Offset: 0x00152BED
		public MouseCursorRayHit(GridCellRayHit gridCellRayHit, List<GameObjectRayHit> sortedObjectRayHits)
		{
			this._gridCellRayHit = gridCellRayHit;
			this._sortedObjectRayHits = ((sortedObjectRayHits != null) ? new List<GameObjectRayHit>(sortedObjectRayHits) : new List<GameObjectRayHit>());
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x00154A14 File Offset: 0x00152C14
		public List<GameObject> GetAllObjectsSortedByHitDistance()
		{
			if (!this.WasAnObjectHit)
			{
				return new List<GameObject>();
			}
			List<GameObject> list = new List<GameObject>();
			foreach (GameObjectRayHit gameObjectRayHit in this._sortedObjectRayHits)
			{
				list.Add(gameObjectRayHit.HitObject);
			}
			return list;
		}

		// Token: 0x04002099 RID: 8345
		private GridCellRayHit _gridCellRayHit;

		// Token: 0x0400209A RID: 8346
		private List<GameObjectRayHit> _sortedObjectRayHits;
	}
}
