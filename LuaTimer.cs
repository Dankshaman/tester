using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class LuaTimer
{
	// Token: 0x06001386 RID: 4998 RVA: 0x00082987 File Offset: 0x00080B87
	[MoonSharpHidden]
	public LuaTimer()
	{
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x0008299C File Offset: 0x00080B9C
	public bool Create(Script script, Table Parameters)
	{
		string text = null;
		if (Parameters["identifier"] != null)
		{
			text = Parameters["identifier"].ToString();
		}
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		string text2 = null;
		if (Parameters["function_name"] != null)
		{
			text2 = Parameters["function_name"].ToString();
		}
		if (string.IsNullOrEmpty(text2))
		{
			return false;
		}
		Script functionOwner = script;
		if (Parameters["function_owner"] != null)
		{
			functionOwner = ((LuaGameObjectScript)Parameters["function_owner"]).lua;
		}
		Table parameters = null;
		if (Parameters["parameters"] != null)
		{
			parameters = (Table)Parameters["parameters"];
		}
		float delay = 0f;
		if (Parameters["delay"] != null)
		{
			float.TryParse(Parameters["delay"].ToString(), out delay);
		}
		int repetitions = 1;
		if (Parameters["repetitions"] != null)
		{
			int.TryParse(Parameters["repetitions"].ToString(), out repetitions);
		}
		if (this.Timers.ContainsKey(text))
		{
			Chat.LogError("Timer already contains identifier: " + text, true);
			return false;
		}
		if (!LuaGlobalScriptManager.Instance.BlockEvents)
		{
			this.Timers.Add(text, LuaGlobalScriptManager.Instance.StartCoroutine(this.TimerCoroutine(text, text2, functionOwner, parameters, delay, repetitions)));
			return true;
		}
		return false;
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00082AEA File Offset: 0x00080CEA
	private IEnumerator TimerCoroutine(string Identifier, string FunctionName, Script FunctionOwner, Table Parameters, float Delay, int Repetitions)
	{
		if (!LuaGlobalScriptManager.Instance.loaded)
		{
			yield break;
		}
		int numberOfReps = Repetitions;
		if (numberOfReps == 0)
		{
			numberOfReps = -1;
		}
		if (FunctionOwner == null)
		{
			yield break;
		}
		Table copyTable = null;
		if (Parameters != null)
		{
			copyTable = LuaGlobalScriptManager.CopyLuaTable(Parameters, FunctionOwner);
		}
		WaitForSeconds waitDelay = new WaitForSeconds(Delay);
		while (numberOfReps != 0)
		{
			yield return waitDelay;
			if (numberOfReps > -1)
			{
				int num = numberOfReps;
				numberOfReps = num - 1;
			}
			if (FunctionOwner != null && FunctionOwner.Globals[FunctionName] != null)
			{
				try
				{
					if (numberOfReps == 0 && this.Timers.ContainsKey(Identifier))
					{
						this.Timers.Remove(Identifier);
					}
					if (copyTable != null)
					{
						FunctionOwner.Call(FunctionOwner.Globals[FunctionName], new object[]
						{
							copyTable
						});
					}
					else
					{
						FunctionOwner.Call(FunctionOwner.Globals[FunctionName]);
					}
				}
				catch (Exception e)
				{
					Chat.LogError("Error calling Lua function, " + FunctionName + ", in Timer: " + LuaScript.ExceptionToMessage(e), true);
					LuaGlobalScriptManager.Instance.PushLuaErrorMessage(LuaScript.ExceptionToMessage(e), "-2", "Error calling Lua function, " + FunctionName + ", in Timer: ");
				}
			}
		}
		yield break;
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x00082B26 File Offset: 0x00080D26
	public bool Destroy(string Identifier)
	{
		if (this.Timers.ContainsKey(Identifier))
		{
			LuaGlobalScriptManager.Instance.StopCoroutine(this.Timers[Identifier]);
			this.Timers.Remove(Identifier);
			return true;
		}
		return false;
	}

	// Token: 0x04000BAA RID: 2986
	[MoonSharpHidden]
	public Dictionary<string, UnityEngine.Coroutine> Timers = new Dictionary<string, UnityEngine.Coroutine>();
}
