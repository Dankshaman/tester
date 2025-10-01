using System;
using NewNet;

// Token: 0x02000125 RID: 293
public class Hands : NetworkSingleton<Hands>
{
	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0006A3D2 File Offset: 0x000685D2
	// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0006A3DA File Offset: 0x000685DA
	[Sync(Permission.Admin, null, SerializationMethod.Default, false)]
	public HandsState handsState
	{
		get
		{
			return this._handsState;
		}
		set
		{
			this._handsState = value;
			HandZone.ResetHandZones();
			base.DirtySync("handsState");
		}
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x0006A3F4 File Offset: 0x000685F4
	public void Reset()
	{
		HandsState handsState = new HandsState();
		if (handsState != this.handsState)
		{
			this.handsState = handsState;
		}
	}

	// Token: 0x040009A5 RID: 2469
	private HandsState _handsState = new HandsState();
}
