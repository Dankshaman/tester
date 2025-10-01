using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Xml.Tags
{
	// Token: 0x02000393 RID: 915
	public class TextTagHandler : ElementTagHandler
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x0012FDF8 File Offset: 0x0012DFF8
		public override MonoBehaviour primaryComponent
		{
			get
			{
				if (base.currentInstanceTransform == null)
				{
					return null;
				}
				return base.currentInstanceTransform.GetComponent<Text>();
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool UseParseChildElements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x0012FE18 File Offset: 0x0012E018
		public override void ParseChildElements(XmlNode xmlNode)
		{
			string text = xmlNode.InnerXml.Replace(" xmlns=\"XmlLayout\"", string.Empty).Replace(" xmlns=\"http://www.w3schools.com\"", string.Empty).Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty);
			text = TextTagHandler.ReplaceIgnoreCase(text, "<textcolor color=", "<color=");
			text = TextTagHandler.ReplaceIgnoreCase(text, "</textcolor", "</color");
			text = TextTagHandler.ReplaceIgnoreCase(text, "<textsize size=", "<size=");
			text = TextTagHandler.ReplaceIgnoreCase(text, "</textsize", "</size");
			text = text.Trim();
			text = TextTagHandler.ReplaceIgnoreCase(text, " {2,}", " ");
			text = text.Replace("<br/>", "\n").Replace("<br />", "\n");
			if (text.Contains("\n"))
			{
				text = text.Replace("\\n", "\n");
				text = string.Join("\n", (from s in text.Split(new char[]
				{
					'\n'
				})
				select s.Trim()).ToArray<string>());
			}
			text = StringExtensions.DecodeEncodedNonAsciiCharacters(text);
			(this.primaryComponent as Text).text = text;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x0012FF60 File Offset: 0x0012E160
		public override void ApplyAttributes(AttributeDictionary attributesToApply)
		{
			base.ApplyAttributes(attributesToApply);
			if (attributesToApply.ContainsKey("text"))
			{
				string value = attributesToApply["text"];
				TextCode.LocalizeUIText(ref value);
				(this.primaryComponent as Text).text = StringExtensions.DecodeEncodedNonAsciiCharacters(value);
			}
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x0012D03A File Offset: 0x0012B23A
		private static string ReplaceIgnoreCase(string source, string match, string replace)
		{
			return Regex.Replace(source, match, replace, RegexOptions.IgnoreCase);
		}

		// Token: 0x04001D1C RID: 7452
		public static List<string> TextAttributes = new List<string>
		{
			"text",
			"fontstyle",
			"font",
			"fontsize",
			"horizontalOverflow",
			"verticalOverflow",
			"resizeTextForBestFit",
			"resizeTextMinSize",
			"resizeTextMaxSize",
			"alignByGeometry"
		};
	}
}
