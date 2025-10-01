using System;
using System.Reflection;

// Token: 0x02000252 RID: 594
public class TagAttribute : Attribute
{
	// Token: 0x06001F6C RID: 8044 RVA: 0x000E0868 File Offset: 0x000DEA68
	public TagAttribute(TagType tag)
	{
		this.tag = tag;
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x000E0878 File Offset: 0x000DEA78
	public static bool IsTag(PropertyInfo property, TagType tag)
	{
		object[] customAttributes = property.GetCustomAttributes(true);
		for (int i = 0; i < customAttributes.Length; i++)
		{
			TagAttribute tagAttribute = customAttributes[i] as TagAttribute;
			if (tagAttribute != null && tagAttribute.tag == tag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000E08B4 File Offset: 0x000DEAB4
	public static bool IsTag(FieldInfo field, TagType tag)
	{
		object[] customAttributes = field.GetCustomAttributes(true);
		for (int i = 0; i < customAttributes.Length; i++)
		{
			TagAttribute tagAttribute = customAttributes[i] as TagAttribute;
			if (tagAttribute != null && tagAttribute.tag == tag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001346 RID: 4934
	public readonly TagType tag;
}
