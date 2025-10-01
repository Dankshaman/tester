using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoonSharp.Interpreter;
using UnityEngine;

namespace LuaFramework
{
	// Token: 0x020003B0 RID: 944
	public static class LuaReader
	{
		// Token: 0x06002CB1 RID: 11441 RVA: 0x00138BB0 File Offset: 0x00136DB0
		public static T Read<T>(Table luaTable)
		{
			return (!!0)((object)LuaReader.Convert(DynValue.NewTable(luaTable), typeof(!!0)));
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x00138BCC File Offset: 0x00136DCC
		public static T Read<T>(DynValue luaValue)
		{
			return (!!0)((object)LuaReader.Convert(luaValue, typeof(!!0)));
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x00138BE4 File Offset: 0x00136DE4
		private static void ReadProperty<T>(T obj, string propertyName, DynValue propertyValue)
		{
			try
			{
				PropertyInfo property = obj.GetType().GetProperty(propertyName);
				if (property != null && property.CanWrite && propertyValue != null)
				{
					Type propertyType = property.PropertyType;
					property.SetValue(obj, LuaReader.Convert(propertyValue, propertyType), null);
				}
			}
			catch
			{
				Debug.Log("LuaReader: could not define property \"" + propertyName + "\".");
				throw new Exception();
			}
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x00138C64 File Offset: 0x00136E64
		private static object Convert(DynValue luaValue, Type type)
		{
			if (LuaReader.customReaders.ContainsKey(type))
			{
				return LuaReader.customReaders[type](luaValue);
			}
			if (type == typeof(bool))
			{
				return luaValue.Boolean;
			}
			if (type == typeof(int))
			{
				return (int)luaValue.Number;
			}
			if (type == typeof(float))
			{
				return (float)luaValue.Number;
			}
			if (type == typeof(double))
			{
				return luaValue.Number;
			}
			if (type == typeof(string) && luaValue.String != null)
			{
				return luaValue.String;
			}
			if (type == typeof(byte))
			{
				return (byte)luaValue.Number;
			}
			if (type == typeof(decimal))
			{
				return (decimal)luaValue.Number;
			}
			if (luaValue.Type == DataType.Function)
			{
				return luaValue.Function;
			}
			if (type.IsEnum && luaValue.String != null)
			{
				return System.Convert.ChangeType(Enum.Parse(type, luaValue.String), type);
			}
			if (luaValue.Table == null)
			{
				return null;
			}
			if (type == typeof(Color))
			{
				return LuaReader.ReadColor(luaValue.Table);
			}
			if (type == typeof(Color32))
			{
				return LuaReader.ReadColor32(luaValue.Table);
			}
			if (type == typeof(Rect))
			{
				return LuaReader.ReadRect(luaValue.Table);
			}
			if (type == typeof(Vector2))
			{
				return LuaReader.ReadVector2(luaValue.Table);
			}
			if (type == typeof(Vector3))
			{
				return LuaReader.ReadVector3(luaValue.Table);
			}
			if (type == typeof(Vector4))
			{
				return LuaReader.ReadVector4(luaValue.Table);
			}
			if (type.IsArray)
			{
				return LuaReader.ReadArray(luaValue.Table, type);
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				return LuaReader.ReadList(luaValue.Table, type);
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >))
			{
				return LuaReader.ReadDictionary(luaValue.Table, type);
			}
			if (type.IsClass)
			{
				return LuaReader.ReadClass(luaValue.Table, type);
			}
			return null;
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x00138F00 File Offset: 0x00137100
		private static Color ReadColor(Table luaTable)
		{
			float r = (luaTable[1] == null) ? 0f : ((float)((double)luaTable[1]));
			float g = (luaTable[2] == null) ? 0f : ((float)((double)luaTable[2]));
			float b = (luaTable[3] == null) ? 0f : ((float)((double)luaTable[3]));
			float a = (luaTable[4] == null) ? 1f : ((float)((double)luaTable[4]));
			return new Color(r, g, b, a);
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x00138FB4 File Offset: 0x001371B4
		private static Color32 ReadColor32(Table luaTable)
		{
			byte r = (luaTable[1] == null) ? 0 : ((byte)((double)luaTable[1]));
			byte g = (luaTable[2] == null) ? 0 : ((byte)((double)luaTable[2]));
			byte b = (luaTable[3] == null) ? 0 : ((byte)((double)luaTable[3]));
			byte a = (luaTable[4] == null) ? byte.MaxValue : ((byte)((double)luaTable[4]));
			return new Color32(r, g, b, a);
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x0013905C File Offset: 0x0013725C
		private static Rect ReadRect(Table luaTable)
		{
			float x = (luaTable[1] == null) ? 0f : ((float)((double)luaTable[1]));
			float y = (luaTable[2] == null) ? 0f : ((float)((double)luaTable[2]));
			float width = (luaTable[3] == null) ? 0f : ((float)((double)luaTable[3]));
			float height = (luaTable[4] == null) ? 0f : ((float)((double)luaTable[4]));
			return new Rect(x, y, width, height);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x00139110 File Offset: 0x00137310
		private static Vector2 ReadVector2(Table luaTable)
		{
			float x = (luaTable[1] == null) ? 0f : ((float)((double)luaTable[1]));
			float y = (luaTable[2] == null) ? 0f : ((float)((double)luaTable[2]));
			return new Vector2(x, y);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x00139174 File Offset: 0x00137374
		private static Vector3 ReadVector3(Table luaTable)
		{
			float x = (luaTable[1] == null) ? 0f : ((float)((double)luaTable[1]));
			float y = (luaTable[2] == null) ? 0f : ((float)((double)luaTable[2]));
			float z = (luaTable[3] == null) ? 0f : ((float)((double)luaTable[3]));
			return new Vector3(x, y, z);
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x00139200 File Offset: 0x00137400
		private static Vector4 ReadVector4(Table luaTable)
		{
			float x = (luaTable[1] == null) ? 0f : ((float)((double)luaTable[1]));
			float y = (luaTable[2] == null) ? 0f : ((float)((double)luaTable[2]));
			float z = (luaTable[3] == null) ? 0f : ((float)((double)luaTable[3]));
			float w = (luaTable[4] == null) ? 0f : ((float)((double)luaTable[4]));
			return new Vector4(x, y, z, w);
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x001392B4 File Offset: 0x001374B4
		private static Array ReadArray(Table luaTable, Type type)
		{
			Type elementType = type.GetElementType();
			if (elementType == null)
			{
				return null;
			}
			if (type.GetArrayRank() == 1)
			{
				Array array = Array.CreateInstance(elementType, luaTable.Values.Count<DynValue>());
				int num = 0;
				foreach (DynValue luaValue in luaTable.Values)
				{
					array.SetValue(System.Convert.ChangeType(LuaReader.Convert(luaValue, elementType), elementType), num);
					num++;
				}
				return array;
			}
			if (type.GetArrayRank() == 2)
			{
				int length = (from dynValue in luaTable.Values
				where dynValue.Table != null
				select dynValue.Table.Values.Count<DynValue>()).Concat(new int[1]).Max();
				Array array2 = Array.CreateInstance(elementType, luaTable.Values.Count<DynValue>(), length);
				int num2 = 0;
				foreach (DynValue dynValue2 in luaTable.Values)
				{
					int num3 = 0;
					foreach (DynValue luaValue2 in dynValue2.Table.Values)
					{
						array2.SetValue(System.Convert.ChangeType(LuaReader.Convert(luaValue2, elementType), elementType), num2, num3);
						num3++;
					}
					num2++;
				}
				return array2;
			}
			return null;
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x00139468 File Offset: 0x00137668
		private static IList ReadList(Table luaTable, Type type)
		{
			Type type2 = type.GetGenericArguments()[0];
			IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
			{
				type2
			}));
			foreach (DynValue luaValue in luaTable.Values)
			{
				list.Add(System.Convert.ChangeType(LuaReader.Convert(luaValue, type2), type2));
			}
			return list;
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x001394F0 File Offset: 0x001376F0
		private static IDictionary ReadDictionary(Table luaTable, Type type)
		{
			Type type2 = type.GetGenericArguments()[0];
			Type type3 = type.GetGenericArguments()[1];
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(new Type[]
			{
				type2,
				type3
			}));
			foreach (TablePair tablePair in luaTable.Pairs)
			{
				if (!tablePair.Value.IsNil())
				{
					object obj = System.Convert.ChangeType(LuaReader.Convert(tablePair.Key, type2), type2);
					if (obj != null)
					{
						dictionary[obj] = System.Convert.ChangeType(LuaReader.Convert(tablePair.Value, type3), type3);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x001395B4 File Offset: 0x001377B4
		private static object ReadClass(Table luaTable, Type type)
		{
			object obj = Activator.CreateInstance(type);
			LuaReader.ReadClassData<object>(obj, luaTable);
			return obj;
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x001395C3 File Offset: 0x001377C3
		public static void ReadClassData<T>(T clrObject, DynValue luaValue)
		{
			LuaReader.ReadClassData<T>(clrObject, luaValue.Table);
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x001395D4 File Offset: 0x001377D4
		public static void ReadClassData<T>(T clrObject, Table luaTable)
		{
			if (!typeof(!!0).IsValueType && clrObject == null)
			{
				Debug.LogWarning("LuaReader: method ReadObjectData called with null object.");
				return;
			}
			if (luaTable == null)
			{
				Debug.LogWarning("LuaReader: method ReadObjectData called with null Lua table.");
				return;
			}
			foreach (TablePair tablePair in luaTable.Pairs)
			{
				if (!tablePair.Value.IsNil())
				{
					LuaReader.ReadProperty<T>(clrObject, tablePair.Key.String, tablePair.Value);
				}
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x00139674 File Offset: 0x00137874
		public static void ReadSingleProperty<T>(T clrObject, string propertyName, Table luaTable)
		{
			if (clrObject == null)
			{
				Debug.LogWarning("LuaReader: method ReadSingleProperty called with null object.");
				return;
			}
			if (luaTable == null)
			{
				Debug.LogWarning("LuaReader: method ReadSingleProperty called with null Lua table.");
				return;
			}
			LuaReader.ReadProperty<T>(clrObject, propertyName, luaTable.Get(propertyName));
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x001396A5 File Offset: 0x001378A5
		public static void AddCustomReader(Type type, Func<DynValue, object> reader)
		{
			if (type == null || reader == null)
			{
				return;
			}
			LuaReader.customReaders[type] = reader;
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x001396C0 File Offset: 0x001378C0
		public static void RemoveCustomReader(Type type)
		{
			if (type == null)
			{
				return;
			}
			LuaReader.customReaders.Remove(type);
		}

		// Token: 0x04001E14 RID: 7700
		private static readonly Dictionary<Type, Func<DynValue, object>> customReaders = new Dictionary<Type, Func<DynValue, object>>();
	}
}
