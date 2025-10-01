using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020004B9 RID: 1209
	public class JSONData : JSONNode
	{
		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060035C2 RID: 13762 RVA: 0x00165960 File Offset: 0x00163B60
		// (set) Token: 0x060035C3 RID: 13763 RVA: 0x00165968 File Offset: 0x00163B68
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x00165971 File Offset: 0x00163B71
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x00165980 File Offset: 0x00163B80
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x0016598F File Offset: 0x00163B8F
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0016599E File Offset: 0x00163B9E
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x001659AD File Offset: 0x00163BAD
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x001659BC File Offset: 0x00163BBC
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x001659BC File Offset: 0x00163BBC
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x001659D8 File Offset: 0x00163BD8
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData("");
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x0400220F RID: 8719
		private string m_Data;
	}
}
