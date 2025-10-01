using System;

// Token: 0x020002C0 RID: 704
public class UIDebug : Singleton<UIDebug>
{
	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x060022CC RID: 8908 RVA: 0x000F8633 File Offset: 0x000F6833
	// (set) Token: 0x060022CD RID: 8909 RVA: 0x000F863B File Offset: 0x000F683B
	public UILabel Label { get; private set; }

	// Token: 0x060022CE RID: 8910 RVA: 0x000F8644 File Offset: 0x000F6844
	protected override void Awake()
	{
		base.Awake();
		this.Label = base.GetComponent<UILabel>();
	}
}
