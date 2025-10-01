using System;
using RTEditor;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class TTSObjectSelectionRectangle : ObjectSelectionRectangle
{
	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06001F2A RID: 7978 RVA: 0x000DE5B8 File Offset: 0x000DC7B8
	// (set) Token: 0x06001F2B RID: 7979 RVA: 0x000DE5C0 File Offset: 0x000DC7C0
	public Rect EnclosingRectangle
	{
		get
		{
			return this._enclosingRectangle;
		}
		set
		{
			this._enclosingRectangle = value;
		}
	}
}
