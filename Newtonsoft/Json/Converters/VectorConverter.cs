using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000462 RID: 1122
	public class VectorConverter : JsonConverter
	{
		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060032F2 RID: 13042 RVA: 0x00154D90 File Offset: 0x00152F90
		// (set) Token: 0x060032F3 RID: 13043 RVA: 0x00154D98 File Offset: 0x00152F98
		public bool EnableVector2 { get; set; }

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060032F4 RID: 13044 RVA: 0x00154DA1 File Offset: 0x00152FA1
		// (set) Token: 0x060032F5 RID: 13045 RVA: 0x00154DA9 File Offset: 0x00152FA9
		public bool EnableVector3 { get; set; }

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060032F6 RID: 13046 RVA: 0x00154DB2 File Offset: 0x00152FB2
		// (set) Token: 0x060032F7 RID: 13047 RVA: 0x00154DBA File Offset: 0x00152FBA
		public bool EnableVector4 { get; set; }

		// Token: 0x060032F8 RID: 13048 RVA: 0x00154DC3 File Offset: 0x00152FC3
		public VectorConverter()
		{
			this.EnableVector2 = true;
			this.EnableVector3 = true;
			this.EnableVector4 = true;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x00154DE0 File Offset: 0x00152FE0
		public VectorConverter(bool enableVector2, bool enableVector3, bool enableVector4) : this()
		{
			this.EnableVector2 = enableVector2;
			this.EnableVector3 = enableVector3;
			this.EnableVector4 = enableVector4;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x00154E00 File Offset: 0x00153000
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Type type = value.GetType();
			if (type == VectorConverter.V2)
			{
				Vector2 vector = (Vector2)value;
				VectorConverter.WriteVector(writer, vector.x, vector.y, null, null);
				return;
			}
			if (type == VectorConverter.V3)
			{
				Vector3 vector2 = (Vector3)value;
				VectorConverter.WriteVector(writer, vector2.x, vector2.y, new float?(vector2.z), null);
				return;
			}
			if (type == VectorConverter.V4)
			{
				Vector4 vector3 = (Vector4)value;
				VectorConverter.WriteVector(writer, vector3.x, vector3.y, new float?(vector3.z), new float?(vector3.w));
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x00154EDC File Offset: 0x001530DC
		private static void WriteVector(JsonWriter writer, float x, float y, float? z, float? w)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(x);
			writer.WritePropertyName("y");
			writer.WriteValue(y);
			if (z != null)
			{
				writer.WritePropertyName("z");
				writer.WriteValue(z.Value);
				if (w != null)
				{
					writer.WritePropertyName("w");
					writer.WriteValue(w.Value);
				}
			}
			writer.WriteEndObject();
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x00154F5B File Offset: 0x0015315B
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (objectType == VectorConverter.V2)
			{
				return VectorConverter.PopulateVector2(reader);
			}
			if (objectType == VectorConverter.V3)
			{
				return VectorConverter.PopulateVector3(reader);
			}
			return VectorConverter.PopulateVector4(reader);
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x00154F9C File Offset: 0x0015319C
		public override bool CanConvert(Type objectType)
		{
			return (this.EnableVector2 && objectType == VectorConverter.V2) || (this.EnableVector3 && objectType == VectorConverter.V3) || (this.EnableVector4 && objectType == VectorConverter.V4);
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x00154FEC File Offset: 0x001531EC
		private static Vector2 PopulateVector2(JsonReader reader)
		{
			Vector2 result = default(Vector2);
			if (reader.TokenType != JsonToken.Null)
			{
				JObject jobject = JObject.Load(reader);
				result.x = jobject["x"].Value<float>();
				result.y = jobject["y"].Value<float>();
			}
			return result;
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x00155044 File Offset: 0x00153244
		private static Vector3 PopulateVector3(JsonReader reader)
		{
			Vector3 result = default(Vector3);
			if (reader.TokenType != JsonToken.Null)
			{
				JObject jobject = JObject.Load(reader);
				result.x = jobject["x"].Value<float>();
				result.y = jobject["y"].Value<float>();
				result.z = jobject["z"].Value<float>();
			}
			return result;
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x001550B0 File Offset: 0x001532B0
		private static Vector4 PopulateVector4(JsonReader reader)
		{
			Vector4 result = default(Vector4);
			if (reader.TokenType != JsonToken.Null)
			{
				JObject jobject = JObject.Load(reader);
				result.x = jobject["x"].Value<float>();
				result.y = jobject["y"].Value<float>();
				result.z = jobject["z"].Value<float>();
				result.w = jobject["w"].Value<float>();
			}
			return result;
		}

		// Token: 0x040020C2 RID: 8386
		private static readonly Type V2 = typeof(Vector2);

		// Token: 0x040020C3 RID: 8387
		private static readonly Type V3 = typeof(Vector3);

		// Token: 0x040020C4 RID: 8388
		private static readonly Type V4 = typeof(Vector4);
	}
}
