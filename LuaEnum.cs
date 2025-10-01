using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

// Token: 0x02000189 RID: 393
public class LuaEnum : IUserDataType
{
	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001332 RID: 4914 RVA: 0x000810FF File Offset: 0x0007F2FF
	[MoonSharpHidden]
	public DynValue Table { get; }

	// Token: 0x06001333 RID: 4915 RVA: 0x00081108 File Offset: 0x0007F308
	protected LuaEnum(Type enumType)
	{
		string[] names = Enum.GetNames(enumType);
		Array array = Enum.GetValues(enumType);
		List<DynValue> list = new List<DynValue>(names.Length);
		List<DynValue> list2 = new List<DynValue>(names.Length);
		Table table = new Table(null);
		this.values = new Dictionary<DynValue, DynValue>(array.Length * 2);
		for (int i = 0; i < names.Length; i++)
		{
			string text = names[i];
			DynValue dynValue = DynValue.NewString(text);
			DynValue dynValue2 = DynValue.NewString(char.ToLowerInvariant(text[0]).ToString() + text.Substring(1));
			list.Add(dynValue);
			list2.Add(dynValue2);
			DynValue value = DynValue.NewNumber((double)((int)array.GetValue(i)));
			table[dynValue] = value;
			this.values[dynValue] = value;
			this.values[dynValue2] = value;
		}
		this.nextValues = new Dictionary<DynValue, DynValue>(array.Length * 2 + 1);
		this.nextValues[DynValue.Nil] = DynValue.NewTuple(new DynValue[]
		{
			list[0],
			this.values[list[0]]
		});
		DynValue dynValue3 = list[0];
		DynValue dynValue4 = list2[0];
		for (int j = 0; j < names.Length - 1; j++)
		{
			DynValue key = dynValue3;
			DynValue key2 = dynValue4;
			dynValue3 = list[j + 1];
			dynValue4 = list2[j + 1];
			DynValue dynValue5 = this.values[dynValue3];
			this.nextValues[key] = DynValue.NewTuple(new DynValue[]
			{
				dynValue3,
				dynValue5
			});
			this.nextValues[key2] = DynValue.NewTuple(new DynValue[]
			{
				dynValue4,
				dynValue5
			});
		}
		this.nextValues[dynValue3] = DynValue.Nil;
		this.nextValues[dynValue4] = DynValue.Nil;
		this.Table = DynValue.NewTable(table);
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x00081314 File Offset: 0x0007F514
	public DynValue Index(Script script, DynValue index, bool isDirectIndexing)
	{
		DynValue result;
		if (!this.values.TryGetValue(index, out result))
		{
			return null;
		}
		return result;
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
	public bool SetIndex(Script script, DynValue index, DynValue value, bool isDirectIndexing)
	{
		return false;
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x00081334 File Offset: 0x0007F534
	public DynValue MetaIndex(Script script, string metaname)
	{
		if (metaname.Equals("__pairs"))
		{
			return LuaEnum.pairs;
		}
		return null;
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x0008134A File Offset: 0x0007F54A
	public static DynValue Pairs(ScriptExecutionContext executionContext, CallbackArguments args)
	{
		if (args.Count != 1)
		{
			throw new ScriptRuntimeException("wrong number of arguments to 'LuaEnum+pairs'");
		}
		return DynValue.NewTuple(new DynValue[]
		{
			LuaEnum.next,
			args[0],
			DynValue.Nil
		});
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x00081388 File Offset: 0x0007F588
	public static DynValue Next(ScriptExecutionContext executionContext, CallbackArguments args)
	{
		if (args.Count < 1 || args.Count > 2)
		{
			throw new ScriptRuntimeException("wrong number of arguments to 'LuaEnum+next'");
		}
		LuaEnum luaEnum = args.AsUserData<LuaEnum>(0, "LuaEnum+next", false);
		DynValue key = (args.Count > 1) ? args[1] : DynValue.Nil;
		DynValue result;
		if (luaEnum.nextValues.TryGetValue(key, out result))
		{
			return result;
		}
		throw new ScriptRuntimeException("invalid key to 'LuaEnum+next'");
	}

	// Token: 0x04000B9E RID: 2974
	private static readonly DynValue next = DynValue.NewCallback(new Func<ScriptExecutionContext, CallbackArguments, DynValue>(LuaEnum.Next), null);

	// Token: 0x04000B9F RID: 2975
	private static readonly DynValue pairs = DynValue.NewCallback(new Func<ScriptExecutionContext, CallbackArguments, DynValue>(LuaEnum.Pairs), null);

	// Token: 0x04000BA0 RID: 2976
	private readonly Dictionary<DynValue, DynValue> values;

	// Token: 0x04000BA1 RID: 2977
	private readonly Dictionary<DynValue, DynValue> nextValues;
}
