using System;
using System.Collections.Generic;
using UI.Xml;
using UnityEngine.UI;

// Token: 0x0200038D RID: 909
public class PlaceholderAttribute : CustomXmlAttribute
{
	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool KeepOriginalTag
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x0012F604 File Offset: 0x0012D804
	public override bool UsesConvertMethod
	{
		get
		{
			return base.UsesConvertMethod;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool UsesApplyMethod
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06002AA3 RID: 10915 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool RestrictToPermittedElementsOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x0012F60C File Offset: 0x0012D80C
	public override List<string> PermittedElements
	{
		get
		{
			return new List<string>
			{
				"InputField"
			};
		}
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x0012F61E File Offset: 0x0012D81E
	public override AttributeDictionary Convert(string value, AttributeDictionary elementAttributes, XmlElement xmlElement)
	{
		return base.Convert(value, elementAttributes, xmlElement);
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x0012F629 File Offset: 0x0012D829
	public override void Apply(XmlElement xmlElement, string value, AttributeDictionary elementAttributes)
	{
		Wait.Frames(delegate
		{
			if (!xmlElement)
			{
				return;
			}
			InputField component = xmlElement.GetComponent<InputField>();
			if (!component)
			{
				return;
			}
			if (component.placeholder)
			{
				Text component2 = component.placeholder.GetComponent<Text>();
				if (component2)
				{
					component2.text = value;
				}
			}
		}, 1);
	}
}
