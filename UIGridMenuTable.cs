using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002DF RID: 735
public class UIGridMenuTable : UIGridMenu
{
	// Token: 0x0600241C RID: 9244 RVA: 0x000FFB88 File Offset: 0x000FDD88
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
		base.Load<UIGridMenu.GridButtonTable>(this.TableButtons, 1, "Table", false, true);
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000FFBAC File Offset: 0x000FDDAC
	private void Init()
	{
		if (this.inited)
		{
			return;
		}
		this.inited = true;
		foreach (UIGridMenu.GridButtonTable gridButtonTable in this.TableButtons)
		{
			gridButtonTable.SpriteColor = ((gridButtonTable.Name == "None") ? Color.red : Color.white);
			gridButtonTable.Tags.Add("table");
			gridButtonTable.CloseMenu = base.gameObject;
		}
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000FFC48 File Offset: 0x000FDE48
	public List<UIGridMenu.GridButtonTable> GetTableButtons()
	{
		this.Init();
		return new List<UIGridMenu.GridButtonTable>(this.TableButtons);
	}

	// Token: 0x04001725 RID: 5925
	public List<UIGridMenu.GridButtonTable> TableButtons = new List<UIGridMenu.GridButtonTable>();

	// Token: 0x04001726 RID: 5926
	private bool inited;
}
