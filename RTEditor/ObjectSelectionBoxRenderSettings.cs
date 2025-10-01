using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003E3 RID: 995
	[Serializable]
	public class ObjectSelectionBoxRenderSettings
	{
		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002E14 RID: 11796 RVA: 0x000E4146 File Offset: 0x000E2346
		public static float MinSelectionBoxCornerLinePercentage
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06002E15 RID: 11797 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static float MaxSelectionBoxCornerLinePercentage
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002E16 RID: 11798 RVA: 0x0013DECE File Offset: 0x0013C0CE
		public static float MinSelectionBoxSizeAdd
		{
			get
			{
				return 0.001f;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002E17 RID: 11799 RVA: 0x0013DED5 File Offset: 0x0013C0D5
		// (set) Token: 0x06002E18 RID: 11800 RVA: 0x0013DEDD File Offset: 0x0013C0DD
		public ObjectSelectionBoxStyle SelectionBoxStyle
		{
			get
			{
				return this._selectionBoxStyle;
			}
			set
			{
				this._selectionBoxStyle = value;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002E19 RID: 11801 RVA: 0x0013DEE6 File Offset: 0x0013C0E6
		// (set) Token: 0x06002E1A RID: 11802 RVA: 0x0013DEEE File Offset: 0x0013C0EE
		public ObjectSelectionBoxRenderMode SelectionBoxRenderMode
		{
			get
			{
				return this._selectionBoxRenderMode;
			}
			set
			{
				this._selectionBoxRenderMode = value;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002E1B RID: 11803 RVA: 0x0013DEF7 File Offset: 0x0013C0F7
		// (set) Token: 0x06002E1C RID: 11804 RVA: 0x0013DEFF File Offset: 0x0013C0FF
		public float SelectionBoxCornerLinePercentage
		{
			get
			{
				return this._selectionBoxCornerLinePercentage;
			}
			set
			{
				this._selectionBoxCornerLinePercentage = Mathf.Clamp(value, ObjectSelectionBoxRenderSettings.MinSelectionBoxCornerLinePercentage, ObjectSelectionBoxRenderSettings.MaxSelectionBoxCornerLinePercentage);
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x0013DF17 File Offset: 0x0013C117
		// (set) Token: 0x06002E1E RID: 11806 RVA: 0x0013DF1F File Offset: 0x0013C11F
		public Color SelectionBoxLineColor
		{
			get
			{
				return this._selectionBoxLineColor;
			}
			set
			{
				this._selectionBoxLineColor = value;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002E1F RID: 11807 RVA: 0x0013DF28 File Offset: 0x0013C128
		// (set) Token: 0x06002E20 RID: 11808 RVA: 0x0013DF30 File Offset: 0x0013C130
		public float BoxSizeAdd
		{
			get
			{
				return this._boxSizeAdd;
			}
			set
			{
				this._boxSizeAdd = Mathf.Max(ObjectSelectionBoxRenderSettings.MinSelectionBoxSizeAdd, value);
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06002E21 RID: 11809 RVA: 0x0013DF43 File Offset: 0x0013C143
		// (set) Token: 0x06002E22 RID: 11810 RVA: 0x0013DF4B File Offset: 0x0013C14B
		public bool DrawBoxes
		{
			get
			{
				return this._drawBoxes;
			}
			set
			{
				this._drawBoxes = value;
			}
		}

		// Token: 0x04001EC5 RID: 7877
		[SerializeField]
		private ObjectSelectionBoxStyle _selectionBoxStyle;

		// Token: 0x04001EC6 RID: 7878
		[SerializeField]
		private ObjectSelectionBoxRenderMode _selectionBoxRenderMode;

		// Token: 0x04001EC7 RID: 7879
		[SerializeField]
		private float _selectionBoxCornerLinePercentage = 0.5f;

		// Token: 0x04001EC8 RID: 7880
		[SerializeField]
		private Color _selectionBoxLineColor = new Color(0f, 1f, 0f, 1f);

		// Token: 0x04001EC9 RID: 7881
		[SerializeField]
		private float _boxSizeAdd = 0.005f;

		// Token: 0x04001ECA RID: 7882
		[SerializeField]
		private bool _drawBoxes = true;
	}
}
