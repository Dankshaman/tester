using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020004B7 RID: 1207
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x1700072F RID: 1839
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x0016530D File Offset: 0x0016350D
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x001652FF File Offset: 0x001634FF
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x0016531A File Offset: 0x0016351A
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode result = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x00165348 File Offset: 0x00163548
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060035AD RID: 13741 RVA: 0x00165358 File Offset: 0x00163558
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x00165368 File Offset: 0x00163568
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode jsonnode in this.m_List)
			{
				yield return jsonnode;
			}
			List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x00165378 File Offset: 0x00163578
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x001653FC File Offset: 0x001635FC
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x001654A0 File Offset: 0x001636A0
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x0400220D RID: 8717
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
