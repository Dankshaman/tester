using System;
using UI.Xml;

// Token: 0x0200038E RID: 910
public class PositionAttribute : CustomXmlAttribute
{
	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool KeepOriginalTag
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06002AA9 RID: 10921 RVA: 0x0012F604 File Offset: 0x0012D804
	public override bool UsesConvertMethod
	{
		get
		{
			return base.UsesConvertMethod;
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06002AAA RID: 10922 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool UsesApplyMethod
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x0012F61E File Offset: 0x0012D81E
	public override AttributeDictionary Convert(string value, AttributeDictionary elementAttributes, XmlElement xmlElement)
	{
		return base.Convert(value, elementAttributes, xmlElement);
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x0012F658 File Offset: 0x0012D858
	public override void Apply(XmlElement xmlElement, string value, AttributeDictionary elementAttributes)
	{
		xmlElement.transform.localPosition = value.ToVector3();
	}
}
