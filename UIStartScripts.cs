using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000341 RID: 833
public class UIStartScripts : MonoBehaviour
{
	// Token: 0x060027A1 RID: 10145 RVA: 0x00119679 File Offset: 0x00117879
	public void AddScript(LuaGlobalScriptManager script)
	{
		base.gameObject.SetActive(true);
		this.GlobalScript = script;
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x0011968E File Offset: 0x0011788E
	public void AddScript(LuaGameObjectScript script)
	{
		base.gameObject.SetActive(true);
		this.Scripts.Add(script);
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x001196A8 File Offset: 0x001178A8
	private void OnClick()
	{
		base.gameObject.SetActive(false);
		if (this.GlobalScript)
		{
			this.GlobalScript.DoString();
		}
		foreach (LuaGameObjectScript luaGameObjectScript in this.Scripts)
		{
			if (luaGameObjectScript)
			{
				luaGameObjectScript.DoString();
			}
		}
		this.Scripts.Clear();
		this.GlobalScript = null;
		LuaGlobalScriptManager.Instance.autoLoadOnce = true;
	}

	// Token: 0x04001A06 RID: 6662
	private LuaGlobalScriptManager GlobalScript;

	// Token: 0x04001A07 RID: 6663
	private List<LuaGameObjectScript> Scripts = new List<LuaGameObjectScript>();
}
