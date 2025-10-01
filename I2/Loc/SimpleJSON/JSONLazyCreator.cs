using System;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020004BA RID: 1210
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x060035CC RID: 13772 RVA: 0x00165ACF File Offset: 0x00163CCF
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x00165AE5 File Offset: 0x00163CE5
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x00165AFB File Offset: 0x00163CFB
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x17000738 RID: 1848
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray
				{
					value
				});
			}
		}

		// Token: 0x17000739 RID: 1849
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass
				{
					{
						aKey,
						value
					}
				});
			}
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x00165B7C File Offset: 0x00163D7C
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray
			{
				aItem
			});
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x00165BA0 File Offset: 0x00163DA0
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass
			{
				{
					aKey,
					aItem
				}
			});
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x00165BC2 File Offset: 0x00163DC2
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x00165BCD File Offset: 0x00163DCD
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x00165BC2 File Offset: 0x00163DC2
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x00165BD9 File Offset: 0x00163DD9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x001649B5 File Offset: 0x00162BB5
		public override string ToString()
		{
			return "";
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x001649B5 File Offset: 0x00162BB5
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060035DB RID: 13787 RVA: 0x00165BE4 File Offset: 0x00163DE4
		// (set) Token: 0x060035DC RID: 13788 RVA: 0x00165C00 File Offset: 0x00163E00
		public override int AsInt
		{
			get
			{
				JSONData aVal = new JSONData(0);
				this.Set(aVal);
				return 0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x00165C1C File Offset: 0x00163E1C
		// (set) Token: 0x060035DE RID: 13790 RVA: 0x00165C40 File Offset: 0x00163E40
		public override float AsFloat
		{
			get
			{
				JSONData aVal = new JSONData(0f);
				this.Set(aVal);
				return 0f;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060035DF RID: 13791 RVA: 0x00165C5C File Offset: 0x00163E5C
		// (set) Token: 0x060035E0 RID: 13792 RVA: 0x00165C88 File Offset: 0x00163E88
		public override double AsDouble
		{
			get
			{
				JSONData aVal = new JSONData(0.0);
				this.Set(aVal);
				return 0.0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060035E1 RID: 13793 RVA: 0x00165CA4 File Offset: 0x00163EA4
		// (set) Token: 0x060035E2 RID: 13794 RVA: 0x00165CC0 File Offset: 0x00163EC0
		public override bool AsBool
		{
			get
			{
				JSONData aVal = new JSONData(false);
				this.Set(aVal);
				return false;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060035E3 RID: 13795 RVA: 0x00165CDC File Offset: 0x00163EDC
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060035E4 RID: 13796 RVA: 0x00165CF8 File Offset: 0x00163EF8
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x04002210 RID: 8720
		private JSONNode m_Node;

		// Token: 0x04002211 RID: 8721
		private string m_Key;
	}
}
