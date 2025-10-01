using System;
using UI.Xml;

// Token: 0x0200038F RID: 911
public class RotationAttribute : CustomXmlAttribute
{
	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06002AAE RID: 10926 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool KeepOriginalTag
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06002AAF RID: 10927 RVA: 0x0012F604 File Offset: 0x0012D804
	public override bool UsesConvertMethod
	{
		get
		{
			return base.UsesConvertMethod;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06002AB0 RID: 10928 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool UsesApplyMethod
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x0012F61E File Offset: 0x0012D81E
	public override AttributeDictionary Convert(string value, AttributeDictionary elementAttributes, XmlElement xmlElement)
	{
		return base.Convert(value, elementAttributes, xmlElement);
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x0012F66B File Offset: 0x0012D86B
	public override void Apply(XmlElement xmlElement, string value, AttributeDictionary elementAttributes)
	{
		xmlElement.transform.localEulerAngles = value.ToVector3();
	}
}
