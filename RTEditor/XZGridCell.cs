using System;

namespace RTEditor
{
	// Token: 0x02000449 RID: 1097
	public class XZGridCell
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600323C RID: 12860 RVA: 0x001529A7 File Offset: 0x00150BA7
		public int CellIndexX
		{
			get
			{
				return this._cellIndexX;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600323D RID: 12861 RVA: 0x001529AF File Offset: 0x00150BAF
		public int CellIndexZ
		{
			get
			{
				return this._cellIndexZ;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x001529B7 File Offset: 0x00150BB7
		public XZGrid ParentGrid
		{
			get
			{
				return this._parentGrid;
			}
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x001529BF File Offset: 0x00150BBF
		public XZGridCell(int cellIndexX, int cellIndexZ, XZGrid parentGrid)
		{
			this._cellIndexX = cellIndexX;
			this._cellIndexZ = cellIndexZ;
			this._parentGrid = parentGrid;
		}

		// Token: 0x0400205D RID: 8285
		private int _cellIndexX;

		// Token: 0x0400205E RID: 8286
		private int _cellIndexZ;

		// Token: 0x0400205F RID: 8287
		private XZGrid _parentGrid;
	}
}
