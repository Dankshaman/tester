using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

// Token: 0x02000196 RID: 406
public class LuaWait
{
	// Token: 0x060013F9 RID: 5113 RVA: 0x000845B8 File Offset: 0x000827B8
	public uint Frames(Closure function, int numberFrames = 1)
	{
		if (function == null)
		{
			throw new ScriptRuntimeException("Error in Wait.frames: function is nil");
		}
		Wait.Identifier item = Wait.Frames(delegate
		{
			LuaWait.TryCall(function);
		}, numberFrames);
		this.startedWaits.Add(item);
		return item.id;
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x0008460C File Offset: 0x0008280C
	public uint Time(Closure function, float time, int repetitions = 1)
	{
		if (function == null)
		{
			throw new ScriptRuntimeException("Error in Wait.time: function is nil");
		}
		Wait.Identifier item = Wait.Time(delegate
		{
			LuaWait.TryCall(function);
		}, time, repetitions);
		this.startedWaits.Add(item);
		return item.id;
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x00084660 File Offset: 0x00082860
	public uint Condition(Closure function, Closure conditionFunction, float timeOutTime = float.PositiveInfinity, Closure timeOutFunction = null)
	{
		if (function == null)
		{
			throw new ScriptRuntimeException("Error in Wait.condition: function is nil");
		}
		if (conditionFunction == null)
		{
			throw new ScriptRuntimeException("Error in Wait.condition: condition_function is nil");
		}
		if (LuaWait.TryCallReturn(conditionFunction).Type != DataType.Boolean)
		{
			throw new ScriptRuntimeException("Error in Wait.condition: condition_function does not return bool");
		}
		Wait.Identifier item = Wait.Condition(delegate
		{
			LuaWait.TryCall(function);
		}, () => LuaWait.TryCallCondition(conditionFunction), timeOutTime, delegate
		{
			LuaWait.TryCall(timeOutFunction);
		});
		this.startedWaits.Add(item);
		return item.id;
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x0008470C File Offset: 0x0008290C
	public bool Stop(uint id)
	{
		Wait.Identifier identifier = new Wait.Identifier(id);
		if (this.startedWaits.Contains(identifier))
		{
			this.startedWaits.Remove(identifier);
		}
		return Wait.Stop(identifier);
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x00084744 File Offset: 0x00082944
	public void StopAll()
	{
		for (int i = 0; i < this.startedWaits.Count; i++)
		{
			Wait.Stop(this.startedWaits[i]);
		}
		this.startedWaits.Clear();
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x00084784 File Offset: 0x00082984
	private static void TryCall(Closure function)
	{
		LuaScript.TryCall(function);
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x00084790 File Offset: 0x00082990
	private static bool TryCallCondition(Closure conditionFunction)
	{
		DynValue dynValue = LuaWait.TryCallReturn(conditionFunction);
		return dynValue == null || (dynValue.Type == DataType.Boolean && dynValue.Boolean);
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x000847BA File Offset: 0x000829BA
	private static DynValue TryCallReturn(Closure function)
	{
		return LuaScript.TryCall(function);
	}

	// Token: 0x04000BBF RID: 3007
	private readonly List<Wait.Identifier> startedWaits = new List<Wait.Identifier>();

	// Token: 0x04000BC0 RID: 3008
	public const int COLLECT_DUPLICATE = 1;

	// Token: 0x04000BC1 RID: 3009
	public const int COLLECT_UNKNOWN = 2;

	// Token: 0x04000BC2 RID: 3010
	public DynValue Collect;

	// Token: 0x04000BC3 RID: 3011
	public const string CollectLua = "    \r\nreturn function (expected_ids, on_finished, on_add, on_error)\r\n    expected_table = {}\r\n    for k, v in ipairs(expected_ids) do\r\n        expected_table[v] = 0\r\n    end\r\n    return {\r\n        expected = expected_table,\r\n        results = {},\r\n        add = function(self, id, ...)\r\n            if self.expected[id] ~= nil then\r\n                self.expected[id] = self.expected[id] + 1\r\n                if self.expected[id] == 1 then\r\n                    self.results[id] = ...\r\n                    if on_add then on_add(...) end\r\n                    for k, v in pairs(self.expected) do\r\n                        if v == 0 then return end\r\n                    end\r\n                    on_finished(self.results)\r\n                else\r\n                    if on_error then on_error(Wait.COLLECT_DUPLICATE, id, ...) end\r\n                end\r\n            else\r\n                if on_error then on_error(Wait.COLLECT_UNKNOWN, id, ...) end\r\n            end\r\n        end ,\r\n        reset = function(self)\r\n            for k, v in pairs(self.expected) do\r\n                self.expected[k] = 0\r\n            end\r\n            results = {}\r\n        end\r\n    }\r\nend\r\n";
}
