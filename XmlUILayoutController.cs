using System;
using System.Collections.Generic;
using UI.Xml;
using UnityEngine;

// Token: 0x02000391 RID: 913
[ExecuteInEditMode]
internal class XmlUILayoutController : XmlLayoutController
{
	// Token: 0x06002ABC RID: 10940 RVA: 0x0012F7B8 File Offset: 0x0012D9B8
	public override void ReceiveMessage(string methodName, string value, RectTransform source = null, List<string> parameters = null, int pointerId = 0)
	{
		if (this.SuppressEventHandling)
		{
			return;
		}
		this.xmlUI.ReceiveMessage(methodName, value, source);
	}

	// Token: 0x06002ABD RID: 10941 RVA: 0x0012F7D1 File Offset: 0x0012D9D1
	public override void PostLayoutRebuilt()
	{
		base.PostLayoutRebuilt();
		if (this.xmlUI)
		{
			this.xmlUI.PostLayoutRebuilt();
		}
	}

	// Token: 0x04001D0D RID: 7437
	public XmlUIScript xmlUI;
}
