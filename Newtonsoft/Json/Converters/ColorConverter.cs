using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000461 RID: 1121
	public class ColorConverter : JsonConverter
	{
		// Token: 0x060032ED RID: 13037 RVA: 0x00154C04 File Offset: 0x00152E04
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Color color = (Color)value;
			writer.WriteStartObject();
			writer.WritePropertyName("a");
			writer.WriteValue(color.a);
			writer.WritePropertyName("r");
			writer.WriteValue(color.r);
			writer.WritePropertyName("g");
			writer.WriteValue(color.g);
			writer.WritePropertyName("b");
			writer.WriteValue(color.b);
			writer.WriteEndObject();
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x00154C8A File Offset: 0x00152E8A
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Color) || objectType == typeof(Color32);
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x00154CB0 File Offset: 0x00152EB0
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return default(Color);
			}
			JObject jobject = JObject.Load(reader);
			if (objectType == typeof(Color32))
			{
				return new Color32((byte)jobject["r"], (byte)jobject["g"], (byte)jobject["b"], (byte)jobject["a"]);
			}
			return new Color((float)jobject["r"], (float)jobject["g"], (float)jobject["b"], (float)jobject["a"]);
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060032F0 RID: 13040 RVA: 0x00014D66 File Offset: 0x00012F66
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}
	}
}
