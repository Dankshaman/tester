using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E6 RID: 998
	[Serializable]
	public class ObjectSelectionSettings
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06002E29 RID: 11817 RVA: 0x0013E019 File Offset: 0x0013C219
		// (set) Token: 0x06002E2A RID: 11818 RVA: 0x0013E021 File Offset: 0x0013C221
		public ObjectSelectionRenderMode ObjectSelectionRenderMode
		{
			get
			{
				return this._objectSelectionRenderMode;
			}
			set
			{
				this._objectSelectionRenderMode = value;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06002E2B RID: 11819 RVA: 0x0013E02A File Offset: 0x0013C22A
		// (set) Token: 0x06002E2C RID: 11820 RVA: 0x0013E032 File Offset: 0x0013C232
		public int SelectableLayers
		{
			get
			{
				return this._selectableLayers;
			}
			set
			{
				this._selectableLayers = value;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06002E2D RID: 11821 RVA: 0x0013E03B File Offset: 0x0013C23B
		// (set) Token: 0x06002E2E RID: 11822 RVA: 0x0013E043 File Offset: 0x0013C243
		public bool CanSelectTerrainObjects
		{
			get
			{
				return this._canSelectTerrainObjects;
			}
			set
			{
				this._canSelectTerrainObjects = value;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06002E2F RID: 11823 RVA: 0x0013E04C File Offset: 0x0013C24C
		// (set) Token: 0x06002E30 RID: 11824 RVA: 0x0013E054 File Offset: 0x0013C254
		public bool CanSelectLightObjects
		{
			get
			{
				return this._canSelectLightObjects;
			}
			set
			{
				this._canSelectLightObjects = value;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06002E31 RID: 11825 RVA: 0x0013E05D File Offset: 0x0013C25D
		// (set) Token: 0x06002E32 RID: 11826 RVA: 0x0013E065 File Offset: 0x0013C265
		public bool CanSelectParticleSystemObjects
		{
			get
			{
				return this._canSelectParticleSystemObjects;
			}
			set
			{
				this._canSelectParticleSystemObjects = value;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002E33 RID: 11827 RVA: 0x0013E06E File Offset: 0x0013C26E
		// (set) Token: 0x06002E34 RID: 11828 RVA: 0x0013E076 File Offset: 0x0013C276
		public bool CanSelectSpriteObjects
		{
			get
			{
				return this._canSelectSpriteObjects;
			}
			set
			{
				this._canSelectSpriteObjects = value;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002E35 RID: 11829 RVA: 0x0013E07F File Offset: 0x0013C27F
		// (set) Token: 0x06002E36 RID: 11830 RVA: 0x0013E087 File Offset: 0x0013C287
		public bool CanSelectEmptyObjects
		{
			get
			{
				return this._canSelectEmptyObjects;
			}
			set
			{
				this._canSelectEmptyObjects = value;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002E37 RID: 11831 RVA: 0x0013E090 File Offset: 0x0013C290
		// (set) Token: 0x06002E38 RID: 11832 RVA: 0x0013E098 File Offset: 0x0013C298
		public bool CanClickSelect
		{
			get
			{
				return this._canClickSelect;
			}
			set
			{
				this._canClickSelect = value;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002E39 RID: 11833 RVA: 0x0013E0A1 File Offset: 0x0013C2A1
		// (set) Token: 0x06002E3A RID: 11834 RVA: 0x0013E0A9 File Offset: 0x0013C2A9
		public bool CanMultiSelect
		{
			get
			{
				return this._canMultiSelect;
			}
			set
			{
				this._canMultiSelect = value;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x0013E0B2 File Offset: 0x0013C2B2
		public ObjectSelectionBoxRenderSettings ObjectSelectionBoxRenderSettings
		{
			get
			{
				return this._objectSelectionBoxRenderSettings;
			}
		}

		// Token: 0x04001ECF RID: 7887
		[SerializeField]
		private ObjectSelectionRenderMode _objectSelectionRenderMode;

		// Token: 0x04001ED0 RID: 7888
		[SerializeField]
		private int _selectableLayers = -1;

		// Token: 0x04001ED1 RID: 7889
		[SerializeField]
		private bool _canSelectTerrainObjects;

		// Token: 0x04001ED2 RID: 7890
		[SerializeField]
		private bool _canSelectLightObjects;

		// Token: 0x04001ED3 RID: 7891
		[SerializeField]
		private bool _canSelectParticleSystemObjects;

		// Token: 0x04001ED4 RID: 7892
		[SerializeField]
		private bool _canSelectSpriteObjects = true;

		// Token: 0x04001ED5 RID: 7893
		[SerializeField]
		private bool _canSelectEmptyObjects;

		// Token: 0x04001ED6 RID: 7894
		[SerializeField]
		private bool _canClickSelect = true;

		// Token: 0x04001ED7 RID: 7895
		[SerializeField]
		private bool _canMultiSelect = true;

		// Token: 0x04001ED8 RID: 7896
		[SerializeField]
		private ObjectSelectionBoxRenderSettings _objectSelectionBoxRenderSettings = new ObjectSelectionBoxRenderSettings();
	}
}
