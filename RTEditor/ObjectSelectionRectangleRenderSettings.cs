using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E4 RID: 996
	[Serializable]
	public class ObjectSelectionRectangleRenderSettings
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x0013DFA3 File Offset: 0x0013C1A3
		// (set) Token: 0x06002E25 RID: 11813 RVA: 0x0013DFAB File Offset: 0x0013C1AB
		public Color BorderLineColor
		{
			get
			{
				return this._borderLineColor;
			}
			set
			{
				this._borderLineColor = value;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002E26 RID: 11814 RVA: 0x0013DFB4 File Offset: 0x0013C1B4
		// (set) Token: 0x06002E27 RID: 11815 RVA: 0x0013DFBC File Offset: 0x0013C1BC
		public Color FillColor
		{
			get
			{
				return this._fillColor;
			}
			set
			{
				this._fillColor = value;
			}
		}

		// Token: 0x04001ECB RID: 7883
		[SerializeField]
		private Color _borderLineColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04001ECC RID: 7884
		[SerializeField]
		private Color _fillColor = new Color(0.37254903f, 0.42745098f, 0.50980395f, 0.5f);
	}
}
