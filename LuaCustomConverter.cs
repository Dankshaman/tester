using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x02000159 RID: 345
public static class LuaCustomConverter
{
	// Token: 0x06001137 RID: 4407 RVA: 0x00076A98 File Offset: 0x00074C98
	public static bool TryParse(Table table, string Entry, ref Vector3 vector)
	{
		if (table != null)
		{
			object obj = table[Entry];
			if (obj != null)
			{
				return LuaCustomConverter.TryParse(obj as Table, ref vector);
			}
		}
		return false;
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x00076AC4 File Offset: 0x00074CC4
	public static bool TryParse(Table table, string Entry, ref Color color)
	{
		if (table != null)
		{
			object obj = table[Entry];
			if (obj != null)
			{
				return LuaCustomConverter.TryParse(obj as Table, ref color);
			}
		}
		return false;
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x00076AF0 File Offset: 0x00074CF0
	public static bool TryParse(Table table, ref Vector2 vector)
	{
		if (table != null && table["x"] != null && table["y"] != null)
		{
			vector = new Vector2(Convert.ToSingle(table["x"]), Convert.ToSingle(table["y"]));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null)
		{
			vector = new Vector2(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]));
			return true;
		}
		return false;
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x00076B98 File Offset: 0x00074D98
	public static bool TryParse(Table table, ref Vector2? vector)
	{
		if (table != null && table["x"] != null && table["y"] != null)
		{
			vector = new Vector2?(new Vector2(Convert.ToSingle(table["x"]), Convert.ToSingle(table["y"])));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null)
		{
			vector = new Vector2?(new Vector2(Convert.ToSingle(table[1]), Convert.ToSingle(table[2])));
			return true;
		}
		return false;
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x00076C58 File Offset: 0x00074E58
	public static bool TryParse(Table table, ref Vector3 vector)
	{
		if (table != null && table["x"] != null && table["y"] != null && table["z"] != null)
		{
			vector = new Vector3(Convert.ToSingle(table["x"]), Convert.ToSingle(table["y"]), Convert.ToSingle(table["z"]));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null)
		{
			vector = new Vector3(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3]));
			return true;
		}
		Vector2 v = default(Vector2);
		if (LuaCustomConverter.TryParse(table, ref v))
		{
			vector = v;
			return true;
		}
		return false;
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00076D5C File Offset: 0x00074F5C
	public static bool TryParse(Table table, ref Vector3? vector)
	{
		if (table != null && table["x"] != null && table["y"] != null && table["z"] != null)
		{
			vector = new Vector3?(new Vector3(Convert.ToSingle(table["x"]), Convert.ToSingle(table["y"]), Convert.ToSingle(table["z"])));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null)
		{
			vector = new Vector3?(new Vector3(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3])));
			return true;
		}
		Vector2? vector2 = null;
		if (LuaCustomConverter.TryParse(table, ref vector2))
		{
			Vector2? vector3 = vector2;
			vector = ((vector3 != null) ? new Vector3?(vector3.GetValueOrDefault()) : null);
			return true;
		}
		return false;
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00076E8C File Offset: 0x0007508C
	public static bool TryParse(Table table, ref Vector4 vector)
	{
		if (table != null && table["x"] != null && table["y"] != null && table["z"] != null && table["w"] != null)
		{
			vector = new Vector4(Convert.ToSingle(table["x"]), Convert.ToSingle(table["y"]), Convert.ToSingle(table["z"]), Convert.ToSingle(table["w"]));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null && table[4] != null)
		{
			vector = new Vector4(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3]), Convert.ToSingle(table[4]));
			return true;
		}
		Vector3 v = default(Vector3);
		if (LuaCustomConverter.TryParse(table, ref v))
		{
			vector = v;
			return true;
		}
		return false;
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x00076FD0 File Offset: 0x000751D0
	public static bool TryParse(Table table, ref Vector4? vector)
	{
		if (table != null && table["x"] != null && table["y"] != null && table["z"] != null && table["w"] != null)
		{
			vector = new Vector4?(new Vector4(Convert.ToSingle(table["x"]), Convert.ToSingle(table["y"]), Convert.ToSingle(table["z"]), Convert.ToSingle(table["w"])));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null && table[4] != null)
		{
			vector = new Vector4?(new Vector4(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3]), Convert.ToSingle(table[4])));
			return true;
		}
		Vector3? vector2 = null;
		if (LuaCustomConverter.TryParse(table, ref vector2))
		{
			Vector3? vector3 = vector2;
			vector = ((vector3 != null) ? new Vector4?(vector3.GetValueOrDefault()) : null);
			return true;
		}
		return false;
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x00077144 File Offset: 0x00075344
	public static bool TryParse(Table table, ref Color color)
	{
		if (table != null && table["r"] != null && table["g"] != null && table["b"] != null && table["a"] != null)
		{
			color = new Color(Convert.ToSingle(table["r"]), Convert.ToSingle(table["g"]), Convert.ToSingle(table["b"]), Convert.ToSingle(table["a"]));
			return true;
		}
		if (table != null && table["r"] != null && table["g"] != null && table["b"] != null)
		{
			color = new Color(Convert.ToSingle(table["r"]), Convert.ToSingle(table["g"]), Convert.ToSingle(table["b"]));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null && table[4] != null)
		{
			color = new Color(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3]), Convert.ToSingle(table[4]));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null)
		{
			color = new Color(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3]));
			return true;
		}
		return false;
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x0007733C File Offset: 0x0007553C
	public static bool TryParse(Table table, ref Color? color)
	{
		if (table != null && table["r"] != null && table["g"] != null && table["b"] != null && table["a"] != null)
		{
			color = new Color?(new Color(Convert.ToSingle(table["r"]), Convert.ToSingle(table["g"]), Convert.ToSingle(table["b"]), Convert.ToSingle(table["a"])));
			return true;
		}
		if (table != null && table["r"] != null && table["g"] != null && table["b"] != null)
		{
			color = new Color?(new Color(Convert.ToSingle(table["r"]), Convert.ToSingle(table["g"]), Convert.ToSingle(table["b"])));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null && table[4] != null)
		{
			color = new Color?(new Color(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3]), Convert.ToSingle(table[4])));
			return true;
		}
		if (table != null && table[1] != null && table[2] != null && table[3] != null)
		{
			color = new Color?(new Color(Convert.ToSingle(table[1]), Convert.ToSingle(table[2]), Convert.ToSingle(table[3])));
			return true;
		}
		return false;
	}

	// Token: 0x06001141 RID: 4417 RVA: 0x0007754B File Offset: 0x0007574B
	public static bool IsVectorOrColor(Type type)
	{
		return LuaCustomConverter.IsVector(type) || LuaCustomConverter.IsColor(type);
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x00077560 File Offset: 0x00075760
	public static bool IsVector(Type type)
	{
		return type == typeof(Vector2) || type == typeof(Vector2?) || type == typeof(Vector3) || type == typeof(Vector3?) || type == typeof(Vector4) || type == typeof(Vector4?) || type == typeof(VectorState) || type == typeof(VectorState?);
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x00077600 File Offset: 0x00075800
	public static bool IsColor(Type type)
	{
		return type == typeof(Color) || type == typeof(Color?) || type == typeof(ColourState) || type == typeof(ColourState?);
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x00077655 File Offset: 0x00075855
	public static T Parse<T>(Table table)
	{
		return (!!0)((object)LuaCustomConverter.Parse(typeof(!!0), table));
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x0007766C File Offset: 0x0007586C
	public static object Parse(Type type, object value)
	{
		string value2;
		if ((value2 = (value as string)) != null)
		{
			return LuaCustomConverter.Parse(type, value2);
		}
		return LuaCustomConverter.Parse(type, (Table)value);
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x00077698 File Offset: 0x00075898
	public static object Parse(Type type, string value)
	{
		if (type == typeof(Color))
		{
			Colour colour;
			if (Colour.TryColourFromLabel(value, out colour))
			{
				return colour;
			}
			throw new ScriptRuntimeException("Error converting String to Color. (White, Red, Blue, etc.)");
		}
		else
		{
			if (type == typeof(Color?))
			{
				return Colour.NullableColorFromLabel(value);
			}
			throw new Exception("Failed to type check LuaCustomConverter.Parse().");
		}
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x00077700 File Offset: 0x00075900
	public static object Parse(Type type, Table table)
	{
		if (type == typeof(Vector2))
		{
			Vector2 zero = Vector2.zero;
			if (!LuaCustomConverter.TryParse(table, ref zero))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y or 1, 2)");
			}
			return zero;
		}
		else if (type == typeof(Vector2?))
		{
			Vector2? vector = null;
			if (!LuaCustomConverter.TryParse(table, ref vector))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y or 1, 2)");
			}
			return vector;
		}
		else if (type == typeof(Vector3))
		{
			Vector3 zero2 = Vector3.zero;
			if (!LuaCustomConverter.TryParse(table, ref zero2))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y, z or 1, 2, 3)");
			}
			return zero2;
		}
		else if (type == typeof(Vector3?))
		{
			Vector3? vector2 = null;
			if (!LuaCustomConverter.TryParse(table, ref vector2))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y, z or 1, 2, 3)");
			}
			return vector2;
		}
		else if (type == typeof(Vector4))
		{
			Vector4 zero3 = Vector4.zero;
			if (!LuaCustomConverter.TryParse(table, ref zero3))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y, z, w or 1, 2, 3, 4)");
			}
			return zero3;
		}
		else if (type == typeof(Vector4?))
		{
			Vector4? vector3 = null;
			if (!LuaCustomConverter.TryParse(table, ref vector3))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y, z, w or 1, 2, 3, 4)");
			}
			return vector3;
		}
		else if (type == typeof(Color))
		{
			Color white = Color.white;
			if (!LuaCustomConverter.TryParse(table, ref white))
			{
				throw new ScriptRuntimeException("Error converting Table to Color. (r, g, b or 1, 2, 3)");
			}
			return white;
		}
		else if (type == typeof(Color?))
		{
			Color? color = null;
			if (!LuaCustomConverter.TryParse(table, ref color))
			{
				throw new ScriptRuntimeException("Error converting Table to Color. (r, g, b or 1, 2, 3)");
			}
			return color;
		}
		else if (type == typeof(VectorState))
		{
			Vector3 zero4 = Vector3.zero;
			if (!LuaCustomConverter.TryParse(table, ref zero4))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y, z or 1, 2, 3)");
			}
			return new VectorState(zero4);
		}
		else if (type == typeof(VectorState?))
		{
			Vector3? vector4 = null;
			if (!LuaCustomConverter.TryParse(table, ref vector4))
			{
				throw new ScriptRuntimeException("Error converting Table to Vector. (x, y, z or 1, 2, 3)");
			}
			if (vector4 != null)
			{
				return new VectorState(vector4.Value);
			}
			return null;
		}
		else if (type == typeof(ColourState))
		{
			Color white2 = Color.white;
			if (!LuaCustomConverter.TryParse(table, ref white2))
			{
				throw new ScriptRuntimeException("Error converting Table to Color. (r, g, b or 1, 2, 3)");
			}
			return new ColourState(white2);
		}
		else
		{
			if (!(type == typeof(ColourState?)))
			{
				throw new Exception("Failed to type check LuaCustomConverter.Parse().");
			}
			Color? color2 = null;
			if (!LuaCustomConverter.TryParse(table, ref color2))
			{
				throw new ScriptRuntimeException("Error converting Table to Color. (r, g, b or 1, 2, 3)");
			}
			if (color2 != null)
			{
				return new ColourState(color2.Value);
			}
			return null;
		}
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x000779E0 File Offset: 0x00075BE0
	public static Table GetTable(Vector2 vector, Script script)
	{
		Table table = new Table(script);
		table.MetaTable = (Table)script.Globals["Vector"];
		table["x"] = vector.x;
		table["y"] = vector.y;
		table["z"] = 0;
		return table;
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x00077A4C File Offset: 0x00075C4C
	public static Table GetTable(Vector3 vector, Script script)
	{
		Table table = new Table(script);
		table.MetaTable = (Table)script.Globals["Vector"];
		table["x"] = vector.x;
		table["y"] = vector.y;
		table["z"] = vector.z;
		return table;
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x00077ABC File Offset: 0x00075CBC
	public static Table GetTable(Vector4 vector, Script script)
	{
		Table table = new Table(script);
		table.MetaTable = (Table)script.Globals["Vector"];
		table["x"] = vector.x;
		table["y"] = vector.y;
		table["z"] = vector.z;
		table["w"] = vector.w;
		return table;
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x00077B44 File Offset: 0x00075D44
	public static Table GetTable(Color color, Script script)
	{
		Table table = new Table(script);
		table.MetaTable = (Table)script.Globals["Color"];
		table["r"] = color.r;
		table["g"] = color.g;
		table["b"] = color.b;
		table["a"] = color.a;
		return table;
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x00077BCC File Offset: 0x00075DCC
	public static Table GetTable(Colour colour, Script script)
	{
		Table table = new Table(script);
		table.MetaTable = (Table)script.Globals["Color"];
		table["r"] = colour.r;
		table["g"] = colour.g;
		table["b"] = colour.b;
		table["a"] = colour.a;
		return table;
	}

	// Token: 0x0600114D RID: 4429 RVA: 0x00077C54 File Offset: 0x00075E54
	public static Table GetTable(object obj, Script script)
	{
		if (obj == null)
		{
			return null;
		}
		Table table = new Table(script);
		Type type = obj.GetType();
		if (type == typeof(Vector2) || type == typeof(Vector2?))
		{
			return LuaCustomConverter.GetTable((Vector2)obj, script);
		}
		if (type == typeof(Vector3) || type == typeof(Vector3?))
		{
			return LuaCustomConverter.GetTable((Vector3)obj, script);
		}
		if (type == typeof(Vector4) || type == typeof(Vector4?))
		{
			return LuaCustomConverter.GetTable((Vector4)obj, script);
		}
		if (type == typeof(Color) || type == typeof(Color?))
		{
			return LuaCustomConverter.GetTable((Color)obj, script);
		}
		if (type == typeof(VectorState) || type == typeof(VectorState?))
		{
			return LuaCustomConverter.GetTable(((VectorState)obj).ToVector(), script);
		}
		if (type == typeof(ColourState) || type == typeof(ColourState?))
		{
			return LuaCustomConverter.GetTable(((ColourState)obj).ToColour(), script);
		}
		foreach (VarInfo varInfo in type.GetVars())
		{
			try
			{
				object value = varInfo.GetValue(obj);
				if (value == null)
				{
					if (!LuaCustomConverter.SkipNullTypes.Contains(type))
					{
						table[varInfo.Name] = null;
					}
				}
				else if (varInfo.VarType == typeof(Vector2) || varInfo.VarType == typeof(Vector2?))
				{
					table[varInfo.Name] = LuaCustomConverter.GetTable((Vector2)value, script);
				}
				else if (varInfo.VarType == typeof(Vector3) || varInfo.VarType == typeof(Vector3?))
				{
					table[varInfo.Name] = LuaCustomConverter.GetTable((Vector3)value, script);
				}
				else if (varInfo.VarType == typeof(Vector4) || varInfo.VarType == typeof(Vector4?))
				{
					table[varInfo.Name] = LuaCustomConverter.GetTable((Vector4)value, script);
				}
				else if (varInfo.VarType == typeof(Color) || varInfo.VarType == typeof(Color?))
				{
					table[varInfo.Name] = LuaCustomConverter.GetTable((Color)value, script);
				}
				else if (varInfo.VarType == typeof(VectorState) || varInfo.VarType == typeof(VectorState?))
				{
					table[varInfo.Name] = LuaCustomConverter.GetTable(((VectorState)value).ToVector(), script);
				}
				else if (varInfo.VarType == typeof(ColourState) || varInfo.VarType == typeof(ColourState?))
				{
					table[varInfo.Name] = LuaCustomConverter.GetTable(((ColourState)value).ToColour(), script);
				}
				else
				{
					table[varInfo.Name] = value;
				}
			}
			catch (Exception ex)
			{
				Chat.LogError(string.Concat(new object[]
				{
					"Error getting Table from Object: ",
					ex.Message,
					" class: ",
					type,
					" type: ",
					varInfo.VarType
				}), true);
				Debug.LogException(ex);
			}
		}
		return table;
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x00078064 File Offset: 0x00076264
	public static Table GetTable(object[] objs, Script script)
	{
		Table table = new Table(script);
		for (int i = 0; i < objs.Length; i++)
		{
			table[i + 1] = LuaCustomConverter.GetTable(objs[i], script);
		}
		return table;
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x0007809E File Offset: 0x0007629E
	public static T GetObject<T>(Table table, object initObject = null)
	{
		return (!!0)((object)LuaCustomConverter.GetObject(typeof(!!0), table, initObject));
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x000780B8 File Offset: 0x000762B8
	public static object GetObject(Type type, Table table, object initObject = null)
	{
		if (LuaCustomConverter.IsVectorOrColor(type))
		{
			return LuaCustomConverter.Parse(type, table);
		}
		object obj;
		if (initObject == null)
		{
			obj = Activator.CreateInstance(type);
		}
		else
		{
			obj = initObject;
		}
		foreach (TablePair tablePair in table.Pairs)
		{
			string @string = tablePair.Key.String;
			object value = tablePair.Value.ToObject();
			LuaCustomConverter.SetValue(obj, @string, value);
		}
		return obj;
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x00078144 File Offset: 0x00076344
	public static T GetObject<T>(Dictionary<string, object> dict, object initObject = null)
	{
		Type typeFromHandle = typeof(!!0);
		object obj;
		if (initObject == null)
		{
			obj = Activator.CreateInstance(typeFromHandle);
		}
		else
		{
			obj = initObject;
		}
		foreach (KeyValuePair<string, object> keyValuePair in dict)
		{
			LuaCustomConverter.SetValue(obj, keyValuePair.Key, keyValuePair.Value);
		}
		return (!!0)((object)obj);
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x000781C0 File Offset: 0x000763C0
	public static bool SetValue(object obj, string memberName, object value)
	{
		Type type = obj.GetType();
		VarInfo var = type.GetVar(memberName);
		if (var != null)
		{
			try
			{
				var.SetValue(obj, LuaCustomConverter.GetValue(var.VarType, value));
				return true;
			}
			catch (Exception ex)
			{
				Chat.LogError(string.Format("Error converting Lua Table entry {0} to Type {1} on Class {2}. Value = {3}. ({4})", new object[]
				{
					memberName,
					var.VarType,
					type,
					value,
					ex.Message
				}), true);
				Debug.Log(value.GetType());
				Debug.LogError(ex);
				return false;
			}
		}
		Debug.Log(string.Format("Error Lua table entry <{0}> does not exist on this class <{1}>.", memberName, obj));
		return true;
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x00078264 File Offset: 0x00076464
	private static object GetValue(Type type, object value)
	{
		if (value == null)
		{
			return null;
		}
		if (type.IsArray)
		{
			Type elementType = type.GetElementType();
			Table table = (Table)value;
			int length = table.Length;
			Array array = Array.CreateInstance(elementType, length);
			for (int i = 1; i <= length; i++)
			{
				array.SetValue(LuaCustomConverter.GetValue(elementType, table.Get(i).ToObject()), i);
			}
			return array;
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
		{
			Type type2 = type.GetGenericArguments()[0];
			Table table2 = (Table)value;
			int length2 = table2.Length;
			IList list = (IList)Activator.CreateInstance(type);
			for (int j = 1; j <= length2; j++)
			{
				list.Add(LuaCustomConverter.GetValue(type2, table2.Get(j).ToObject()));
			}
			return list;
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<, >)))
		{
			Type[] genericArguments = type.GetGenericArguments();
			Table table3 = (Table)value;
			Type type3 = genericArguments[0];
			Type type4 = genericArguments[1];
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(type);
			foreach (TablePair tablePair in table3.Pairs)
			{
				if (!tablePair.Value.IsNil())
				{
					dictionary.Add(LuaCustomConverter.GetValue(type3, tablePair.Key.ToObject()), LuaCustomConverter.GetValue(type4, tablePair.Value.ToObject()));
				}
			}
			return dictionary;
		}
		if (type.IsValueType && type.IsEnum)
		{
			object obj = Convert.ChangeType(Convert.ToInt32(value), Enum.GetUnderlyingType(type));
			if (!Enum.IsDefined(type, obj))
			{
				throw new ScriptRuntimeException(string.Format("Error converting {0} to {1}.", value, type.Name));
			}
			return obj;
		}
		else
		{
			if (LuaCustomConverter.IsVectorOrColor(type))
			{
				return LuaCustomConverter.Parse(type, value);
			}
			if (type == typeof(int) || type == typeof(int?))
			{
				return Convert.ToInt32(value);
			}
			if (type == typeof(float) || type == typeof(float?))
			{
				return Convert.ToSingle(value);
			}
			if (type == typeof(string) && value is double)
			{
				return ((double)value).ToString();
			}
			Table table4;
			if ((table4 = (value as Table)) != null && type != typeof(Table))
			{
				object obj2 = Activator.CreateInstance(type);
				foreach (TablePair tablePair2 in table4.Pairs)
				{
					if (!tablePair2.Value.IsNil())
					{
						string @string = tablePair2.Key.String;
						object value2 = tablePair2.Value.ToObject();
						VarInfo var = type.GetVar(@string);
						if (var != null)
						{
							var.SetValue(obj2, LuaCustomConverter.GetValue(var.VarType, value2));
						}
					}
				}
				return obj2;
			}
			return value;
		}
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x000785A8 File Offset: 0x000767A8
	public static void RegisterConvertionTypeToTable<T>(bool skipNull = false, bool includeChildVarTypes = false)
	{
		Type typeFromHandle = typeof(!!0);
		if (LuaCustomConverter.RegisteredTypes.Contains(typeFromHandle))
		{
			return;
		}
		LuaCustomConverter.RegisteredTypes.Add(typeFromHandle);
		if (skipNull)
		{
			LuaCustomConverter.SkipNullTypes.Add(typeFromHandle);
		}
		if (includeChildVarTypes)
		{
			VarInfo[] vars = typeFromHandle.GetVars();
			for (int i = 0; i < vars.Length; i++)
			{
				LuaCustomConverter.RegisterChildVarType(vars[i].VarType, skipNull);
			}
		}
		UserData.RegisterType<T>(InteropAccessMode.Default, null);
		Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeFromHandle, new Func<DynValue, object>(LuaCustomConverter.TableToObject<T>));
		Script.GlobalOptions.CustomConverters.SetClrToScriptCustomConversion(typeFromHandle, new Func<Script, object, DynValue>(LuaCustomConverter.ObjectToTable));
	}

	// Token: 0x06001155 RID: 4437 RVA: 0x00078650 File Offset: 0x00076850
	private static void RegisterChildVarType(Type type, bool skipNull)
	{
		if (LuaCustomConverter.RegisteredTypes.Contains(type))
		{
			return;
		}
		if (type.IsArray)
		{
			LuaCustomConverter.RegisterChildVarType(type.GetElementType(), skipNull);
			return;
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
		{
			LuaCustomConverter.RegisterChildVarType(type.GetGenericArguments()[0], skipNull);
			return;
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<, >)))
		{
			Type[] genericArguments = type.GetGenericArguments();
			LuaCustomConverter.RegisterChildVarType(genericArguments[0], skipNull);
			LuaCustomConverter.RegisterChildVarType(genericArguments[1], skipNull);
			return;
		}
		if ((type.IsClass && !type.IsPrimitive && type != typeof(string)) || (type.IsValueType && !type.IsEnum && !type.IsPrimitive))
		{
			VarInfo[] vars = type.GetVars();
			if (vars.Length != 0)
			{
				LuaCustomConverter.RegisterConvertionTypeToTable(type, skipNull);
			}
			for (int i = 0; i < vars.Length; i++)
			{
				LuaCustomConverter.RegisterChildVarType(vars[i].VarType, skipNull);
			}
		}
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x00078750 File Offset: 0x00076950
	private static void RegisterConvertionTypeToTable(Type type, bool skipNull)
	{
		if (type.FullName.Contains("["))
		{
			return;
		}
		if (LuaCustomConverter.RegisteredTypes.Contains(type))
		{
			return;
		}
		LuaCustomConverter.RegisteredTypes.Add(type);
		if (skipNull)
		{
			LuaCustomConverter.SkipNullTypes.Add(type);
		}
		UserData.RegisterType(type, InteropAccessMode.Default, null);
		Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, type, (DynValue x) => LuaCustomConverter.GetObject(type, x.Table, null));
		Script.GlobalOptions.CustomConverters.SetClrToScriptCustomConversion(type, new Func<Script, object, DynValue>(LuaCustomConverter.ObjectToTable));
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x00078809 File Offset: 0x00076A09
	private static object TableToObject<T>(DynValue dynVal)
	{
		return LuaCustomConverter.GetObject<T>(dynVal.Table, null);
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x0007881C File Offset: 0x00076A1C
	private static DynValue ObjectToTable(Script script, object obj)
	{
		return DynValue.NewTable(LuaCustomConverter.GetTable(obj, script));
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x0007882A File Offset: 0x00076A2A
	public static void RegisterConvertionStringToType<T>()
	{
		Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.String, typeof(!!0), new Func<DynValue, object>(LuaCustomConverter.StringToObject<T>));
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x00078852 File Offset: 0x00076A52
	private static object StringToObject<T>(DynValue dynVal)
	{
		return LuaCustomConverter.Parse(typeof(!!0), dynVal.String);
	}

	// Token: 0x04000B0E RID: 2830
	private const string errorMsgVector2 = "Error converting Table to Vector. (x, y or 1, 2)";

	// Token: 0x04000B0F RID: 2831
	private const string errorMsgVector3 = "Error converting Table to Vector. (x, y, z or 1, 2, 3)";

	// Token: 0x04000B10 RID: 2832
	private const string errorMsgVector4 = "Error converting Table to Vector. (x, y, z, w or 1, 2, 3, 4)";

	// Token: 0x04000B11 RID: 2833
	private const string errorMsgTableColor = "Error converting Table to Color. (r, g, b or 1, 2, 3)";

	// Token: 0x04000B12 RID: 2834
	private const string errorMsgStringColor = "Error converting String to Color. (White, Red, Blue, etc.)";

	// Token: 0x04000B13 RID: 2835
	private static List<Type> RegisteredTypes = new List<Type>();

	// Token: 0x04000B14 RID: 2836
	private static List<Type> SkipNullTypes = new List<Type>();
}
