using System;

namespace RTEditor
{
	// Token: 0x02000430 RID: 1072
	public struct FloatInterval
	{
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x00150584 File Offset: 0x0014E784
		// (set) Token: 0x06003178 RID: 12664 RVA: 0x0015058C File Offset: 0x0014E78C
		public float Min
		{
			get
			{
				return this._min;
			}
			set
			{
				if (value < this._max)
				{
					this._min = value;
				}
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x0015059E File Offset: 0x0014E79E
		// (set) Token: 0x0600317A RID: 12666 RVA: 0x001505A6 File Offset: 0x0014E7A6
		public float Max
		{
			get
			{
				return this._max;
			}
			set
			{
				if (value > this._min)
				{
					this._max = value;
				}
			}
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x001505B8 File Offset: 0x0014E7B8
		public FloatInterval(float min, float max)
		{
			this._min = min;
			this._max = max;
			if (this._min > this._max)
			{
				float min2 = this._min;
				this._min = this._max;
				this._max = min2;
			}
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x001505FB File Offset: 0x0014E7FB
		public bool Contains(float value)
		{
			return this._min <= value && value <= this._max;
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x00150614 File Offset: 0x0014E814
		public float Clamp(float value)
		{
			if (this.Contains(value))
			{
				return value;
			}
			if (value < this.Min)
			{
				return this.Min;
			}
			return this.Max;
		}

		// Token: 0x0400200E RID: 8206
		private float _min;

		// Token: 0x0400200F RID: 8207
		private float _max;
	}
}
