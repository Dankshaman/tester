using System;
using UI.Xml;

// Token: 0x02000390 RID: 912
public class VisibilityAttribute : CustomXmlAttribute
{
	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool KeepOriginalTag
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x0012F604 File Offset: 0x0012D804
	public override bool UsesConvertMethod
	{
		get
		{
			return base.UsesConvertMethod;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x00014D66 File Offset: 0x00012F66
	public override bool UsesApplyMethod
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x0012F61E File Offset: 0x0012D81E
	public override AttributeDictionary Convert(string value, AttributeDictionary elementAttributes, XmlElement xmlElement)
	{
		return base.Convert(value, elementAttributes, xmlElement);
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x0012F67E File Offset: 0x0012D87E
	public override void Apply(XmlElement xmlElement, string value, AttributeDictionary elementAttributes)
	{
		VisibilityAttribute.UpdateVisibility(xmlElement);
		base.Apply(xmlElement, value, elementAttributes);
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x0012F690 File Offset: 0x0012D890
	public static void UpdateVisibility(XmlElement xmlElement)
	{
		string attribute = xmlElement.GetAttribute("visibility", null);
		bool flag = false;
		if (attribute.Contains("|"))
		{
			string[] array = attribute.Split(new char[]
			{
				'|'
			});
			for (int i = 0; i < array.Length; i++)
			{
				flag = VisibilityAttribute.IsMatch(array[i]);
				if (flag)
				{
					break;
				}
			}
		}
		else
		{
			flag = VisibilityAttribute.IsMatch(attribute);
		}
		bool flag2 = true;
		if (xmlElement.HasAttribute("active"))
		{
			flag2 = xmlElement.GetAttribute("active", null).ToBoolean();
		}
		xmlElement.gameObject.SetActive(flag2 && flag);
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x0012F724 File Offset: 0x0012D924
	public static bool IsMatch(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return true;
		}
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
		if (string.Equals(value, playerState.team.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		bool result;
		if (string.Equals(value, "admin", StringComparison.OrdinalIgnoreCase))
		{
			result = NetworkSingleton<PlayerManager>.Instance.IsAdmin(playerState.id);
		}
		else if (string.Equals(value, "host", StringComparison.OrdinalIgnoreCase))
		{
			result = NetworkSingleton<PlayerManager>.Instance.IsHost(playerState.id);
		}
		else
		{
			result = string.Equals(value, playerState.stringColor, StringComparison.OrdinalIgnoreCase);
		}
		return result;
	}
}
