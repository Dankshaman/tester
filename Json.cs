using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine;

// Token: 0x02000136 RID: 310
public static class Json
{
	// Token: 0x06001039 RID: 4153 RVA: 0x0006F525 File Offset: 0x0006D725
	public static string GetJson(object Obj, bool Indented = true)
	{
		return JsonConvert.SerializeObject(Obj, Indented ? Formatting.Indented : Formatting.None, Json.Settings);
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x0006F53C File Offset: 0x0006D73C
	public static byte[] GetBson(object Obj)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BsonWriter bsonWriter = new BsonWriter(memoryStream))
			{
				new JsonSerializer
				{
					NullValueHandling = NullValueHandling.Ignore,
					Converters = 
					{
						new VectorConverter(),
						new ColorConverter()
					}
				}.Serialize(bsonWriter, Obj);
			}
			result = memoryStream.ToArray();
		}
		return result;
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0006F5C8 File Offset: 0x0006D7C8
	public static T Load<T>(string Json)
	{
		return JsonConvert.DeserializeObject<T>(Json);
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0006F5D0 File Offset: 0x0006D7D0
	public static T Load<T>(byte[] Bson, bool ReadRootValueAsArray = false)
	{
		return Json.Load<T>(Bson, 0, Bson.Length, ReadRootValueAsArray);
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0006F5DD File Offset: 0x0006D7DD
	public static T Load<T>(byte[] Bson, int size, bool ReadRootValueAsArray = false)
	{
		return Json.Load<T>(Bson, 0, size, ReadRootValueAsArray);
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x0006F5E8 File Offset: 0x0006D7E8
	public static T Load<T>(byte[] Bson, int index, int size, bool ReadRootValueAsArray = false)
	{
		T result;
		using (MemoryStream memoryStream = new MemoryStream(Bson, index, size))
		{
			using (BsonReader bsonReader = new BsonReader(memoryStream))
			{
				bsonReader.ReadRootValueAsArray = ReadRootValueAsArray;
				JsonSerializer jsonSerializer = new JsonSerializer();
				jsonSerializer.Error += Json.HandleBsonNullError;
				result = jsonSerializer.Deserialize<T>(bsonReader);
			}
		}
		return result;
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x0006F660 File Offset: 0x0006D860
	private static void HandleBsonNullError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
	{
		string message = args.ErrorContext.Error.Message;
		if (message.Contains("Error converting value {null} to"))
		{
			args.ErrorContext.Handled = true;
			Debug.LogWarning(message);
		}
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x0006F69D File Offset: 0x0006D89D
	public static object Load(string Json, Type type)
	{
		return JsonConvert.DeserializeObject(Json, type);
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x0006F6A8 File Offset: 0x0006D8A8
	public static object Load(byte[] Bson, Type type, int index, int size, bool ReadRootValueAsArray = false)
	{
		object result;
		using (MemoryStream memoryStream = new MemoryStream(Bson, index, size))
		{
			using (BsonReader bsonReader = new BsonReader(memoryStream))
			{
				bsonReader.ReadRootValueAsArray = ReadRootValueAsArray;
				result = new JsonSerializer().Deserialize(bsonReader, type);
			}
		}
		return result;
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0006F710 File Offset: 0x0006D910
	public static T Clone<T>(T Obj)
	{
		return Json.Load<T>(Json.GetJson(Obj, false));
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0006F723 File Offset: 0x0006D923
	public static void Log(object Obj)
	{
		Debug.Log(Json.GetJson(Obj, true));
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0006F734 File Offset: 0x0006D934
	public static T GetValueFromJson<T>(string Json, string ValueName, Json.SearchType searchType = Json.SearchType.Exact)
	{
		bool flag = false;
		JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(Json));
		while (jsonTextReader.Read())
		{
			if (jsonTextReader.Value != null)
			{
				if (flag)
				{
					if (jsonTextReader.Value is !!0)
					{
						return (!!0)((object)jsonTextReader.Value);
					}
					try
					{
						return (!!0)((object)Convert.ChangeType(jsonTextReader.Value, typeof(!!0)));
					}
					catch (InvalidCastException)
					{
						Debug.LogError("Data found but of wrong type!");
						flag = false;
					}
				}
				string text = jsonTextReader.Value as string;
				switch (searchType)
				{
				case Json.SearchType.Exact:
					if (text == ValueName)
					{
						flag = true;
					}
					break;
				case Json.SearchType.Contains:
					if (text.Contains(ValueName))
					{
						flag = true;
					}
					break;
				case Json.SearchType.Starts:
					if (text.StartsWith(ValueName))
					{
						flag = true;
					}
					break;
				case Json.SearchType.Ends:
					if (text.EndsWith(ValueName))
					{
						flag = true;
					}
					break;
				}
			}
			else
			{
				flag = false;
			}
		}
		return default(!!0);
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0006F82C File Offset: 0x0006DA2C
	public static List<T> GetValuesFromJson<T>(string Json, string ValueName, Json.SearchType searchType = Json.SearchType.Exact)
	{
		List<T> list = new List<!!0>();
		bool flag = false;
		JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(Json));
		while (jsonTextReader.Read())
		{
			if (jsonTextReader.Value != null)
			{
				if (flag)
				{
					flag = false;
					if (jsonTextReader.Value is !!0)
					{
						list.Add((!!0)((object)jsonTextReader.Value));
					}
					else
					{
						try
						{
							list.Add((!!0)((object)Convert.ChangeType(jsonTextReader.Value, typeof(!!0))));
						}
						catch (InvalidCastException)
						{
							Debug.LogError("Data found but of wrong type!");
						}
					}
				}
				string text = jsonTextReader.Value as string;
				if (text != null)
				{
					switch (searchType)
					{
					case Json.SearchType.Exact:
						if (text == ValueName)
						{
							flag = true;
						}
						break;
					case Json.SearchType.Contains:
						if (text.Contains(ValueName))
						{
							flag = true;
						}
						break;
					case Json.SearchType.Starts:
						if (text.StartsWith(ValueName))
						{
							flag = true;
						}
						break;
					case Json.SearchType.Ends:
						if (text.EndsWith(ValueName))
						{
							flag = true;
						}
						break;
					}
				}
			}
			else
			{
				flag = false;
			}
		}
		return list;
	}

	// Token: 0x04000A51 RID: 2641
	private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
	{
		NullValueHandling = NullValueHandling.Ignore,
		Converters = new List<JsonConverter>
		{
			new VectorConverter(),
			new ColorConverter()
		}
	};

	// Token: 0x02000647 RID: 1607
	public enum SearchType
	{
		// Token: 0x0400278D RID: 10125
		Exact,
		// Token: 0x0400278E RID: 10126
		Contains,
		// Token: 0x0400278F RID: 10127
		Starts,
		// Token: 0x04002790 RID: 10128
		Ends
	}
}
