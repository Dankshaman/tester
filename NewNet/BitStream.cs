using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NewNet
{
	// Token: 0x0200039B RID: 923
	public class BitStream
	{
		// Token: 0x06002B4B RID: 11083 RVA: 0x00132374 File Offset: 0x00130574
		private void Init()
		{
			this.cachedWriteActions = new Dictionary<Type, object>
			{
				{
					typeof(uint),
					new Action<uint>(this.WriteUint)
				},
				{
					typeof(int),
					new Action<int>(this.WriteInt)
				},
				{
					typeof(ushort),
					new Action<ushort>(this.WriteUshort)
				},
				{
					typeof(float),
					new Action<float>(this.WriteFloat)
				},
				{
					typeof(ulong),
					new Action<ulong>(this.WriteUlong)
				},
				{
					typeof(long),
					new Action<long>(this.WriteLong)
				},
				{
					typeof(byte),
					new Action<byte>(this.WriteByte)
				},
				{
					typeof(bool),
					new Action<bool>(this.WriteBool)
				},
				{
					typeof(Vector3),
					new Action<Vector3>(this.WriteVector3)
				},
				{
					typeof(Quaternion),
					new Action<Quaternion>(this.WriteQuaternion)
				},
				{
					typeof(Color),
					new Action<Color>(this.WriteColor)
				},
				{
					typeof(Color32),
					new Action<Color32>(this.WriteColor32)
				},
				{
					typeof(NetworkPlayer),
					new Action<NetworkPlayer>(delegate(NetworkPlayer v)
					{
						this.WriteUshort(v.id);
					})
				}
			};
			this.cachedReadFuncs = new Dictionary<Type, object>
			{
				{
					typeof(uint),
					new Func<uint>(this.ReadUint)
				},
				{
					typeof(int),
					new Func<int>(this.ReadInt)
				},
				{
					typeof(ushort),
					new Func<ushort>(this.ReadUshort)
				},
				{
					typeof(float),
					new Func<float>(this.ReadFloat)
				},
				{
					typeof(ulong),
					new Func<ulong>(this.ReadUlong)
				},
				{
					typeof(long),
					new Func<long>(this.ReadLong)
				},
				{
					typeof(byte),
					new Func<byte>(this.ReadByte)
				},
				{
					typeof(bool),
					new Func<bool>(this.ReadBool)
				},
				{
					typeof(Vector3),
					new Func<Vector3>(this.ReadVector3)
				},
				{
					typeof(Quaternion),
					new Func<Quaternion>(this.ReadQuaternion)
				},
				{
					typeof(Color),
					new Func<Color>(this.ReadColor)
				},
				{
					typeof(Color32),
					new Func<Color32>(this.ReadColor32)
				},
				{
					typeof(NetworkPlayer),
					new Func<NetworkPlayer>(() => new NetworkPlayer(this.ReadUshort()))
				}
			};
			this.FuncReadInt = new Func<int>(this.ReadInt);
			this.FuncReadUshort = new Func<ushort>(this.ReadUshort);
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x00132694 File Offset: 0x00130894
		public static BitStream GetPooled(int sizeInBytes)
		{
			for (int i = 0; i < BitStream.streamPool.Count; i++)
			{
				BitStream.PoolStream poolStream = BitStream.streamPool[i];
				if (!poolStream.inUse && poolStream.stream.Buffer.Length == sizeInBytes)
				{
					poolStream.inUse = true;
					return poolStream.stream;
				}
			}
			BitStream.PoolStream poolStream2 = new BitStream.PoolStream(new BitStream(sizeInBytes));
			BitStream.streamPool.Add(poolStream2);
			long num = 0L;
			for (int j = 0; j < BitStream.streamPool.Count; j++)
			{
				num += (long)BitStream.streamPool[j].stream.Buffer.Length;
			}
			Debug.Log(string.Format("Created pooled stream. Pool Count: {0} Pool Size: {1}", BitStream.streamPool.Count, Utilities.BytesToFileSizeString(num)));
			return poolStream2.stream;
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x00132764 File Offset: 0x00130964
		public static void ReturnPooled(BitStream stream)
		{
			for (int i = 0; i < BitStream.streamPool.Count; i++)
			{
				BitStream.PoolStream poolStream = BitStream.streamPool[i];
				if (poolStream.stream == stream)
				{
					stream.Rewind();
					poolStream.inUse = false;
					return;
				}
			}
			Debug.LogError("Stream was not in pool.");
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x001327B3 File Offset: 0x001309B3
		public static List<BitStream.PoolStream> GetPool()
		{
			return BitStream.streamPool;
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06002B4F RID: 11087 RVA: 0x001327BA File Offset: 0x001309BA
		// (set) Token: 0x06002B50 RID: 11088 RVA: 0x001327C2 File Offset: 0x001309C2
		public byte[] Buffer { get; private set; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x001327CB File Offset: 0x001309CB
		// (set) Token: 0x06002B52 RID: 11090 RVA: 0x001327D3 File Offset: 0x001309D3
		public int ByteIndex { get; private set; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x001327DC File Offset: 0x001309DC
		// (set) Token: 0x06002B54 RID: 11092 RVA: 0x001327E4 File Offset: 0x001309E4
		public int BitIndex { get; private set; }

		// Token: 0x06002B55 RID: 11093 RVA: 0x001327ED File Offset: 0x001309ED
		public static BitStream CreateByBitSize(int sizeInBits)
		{
			return new BitStream(sizeInBits / 8 + 1);
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x001327F9 File Offset: 0x001309F9
		public static BitStream CreateByByteSize(int sizeInBytes)
		{
			return new BitStream(sizeInBytes);
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x00132801 File Offset: 0x00130A01
		public BitStream(int sizeInBytes)
		{
			this.Init();
			this.Buffer = new byte[sizeInBytes];
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x0013281B File Offset: 0x00130A1B
		public BitStream(byte[] data)
		{
			this.Init();
			this.Buffer = data;
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x00132830 File Offset: 0x00130A30
		public int GetWrittenSizeInBits()
		{
			return this.ByteIndex * 8 + this.BitIndex;
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x00132841 File Offset: 0x00130A41
		public int GetWrittenSizeInBytes()
		{
			if (this.BitIndex == 0)
			{
				return this.ByteIndex;
			}
			return this.ByteIndex + 1;
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x0013285C File Offset: 0x00130A5C
		public void Clear()
		{
			this.Buffer = null;
			this.ByteIndex = 0;
			this.BitIndex = 0;
			this.SavedByteIndex = 0;
			this.SavedBitIndex = 0;
			Dictionary<int, int> savedByteIndeces = this.SavedByteIndeces;
			if (savedByteIndeces != null)
			{
				savedByteIndeces.Clear();
			}
			Dictionary<int, int> savedBitIndeces = this.SavedBitIndeces;
			if (savedBitIndeces == null)
			{
				return;
			}
			savedBitIndeces.Clear();
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x001328AD File Offset: 0x00130AAD
		public void SetData(byte[] data)
		{
			this.Buffer = data;
			this.ByteIndex = 0;
			this.BitIndex = 0;
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x001328C4 File Offset: 0x00130AC4
		public byte[] CreateTrimmedArray()
		{
			byte[] array = new byte[this.GetWrittenSizeInBytes()];
			Array.Copy(this.Buffer, array, array.Length);
			return array;
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x001328ED File Offset: 0x00130AED
		public void Rewind()
		{
			this.ByteIndex = 0;
			this.BitIndex = 0;
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x001328FD File Offset: 0x00130AFD
		public void RewindBytes(int byteCount)
		{
			this.ByteIndex = Mathf.Max(0, this.ByteIndex - byteCount);
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x00132913 File Offset: 0x00130B13
		public void RestorePosition(int byteIndex, int bitIndex)
		{
			this.ByteIndex = byteIndex;
			this.BitIndex = bitIndex;
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x00132923 File Offset: 0x00130B23
		// (set) Token: 0x06002B62 RID: 11106 RVA: 0x0013292B File Offset: 0x00130B2B
		public int SavedByteIndex { get; private set; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x00132934 File Offset: 0x00130B34
		// (set) Token: 0x06002B64 RID: 11108 RVA: 0x0013293C File Offset: 0x00130B3C
		public int SavedBitIndex { get; private set; }

		// Token: 0x06002B65 RID: 11109 RVA: 0x00132945 File Offset: 0x00130B45
		public void SavePosition()
		{
			this.SavedByteIndex = this.ByteIndex;
			this.SavedBitIndex = this.BitIndex;
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x0013295F File Offset: 0x00130B5F
		public void RestoreSavedPosition()
		{
			this.ByteIndex = this.SavedByteIndex;
			this.BitIndex = this.SavedBitIndex;
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x0013297C File Offset: 0x00130B7C
		public void SavePosition(int saveSlot)
		{
			if (this.SavedBitIndeces == null)
			{
				this.SavedBitIndeces = new Dictionary<int, int>();
			}
			if (this.SavedByteIndeces == null)
			{
				this.SavedByteIndeces = new Dictionary<int, int>();
			}
			if (this.SavedBitIndeces.ContainsKey(saveSlot))
			{
				this.SavedBitIndeces[saveSlot] = this.BitIndex;
			}
			else
			{
				this.SavedBitIndeces.Add(saveSlot, this.BitIndex);
			}
			if (this.SavedByteIndeces.ContainsKey(saveSlot))
			{
				this.SavedByteIndeces[saveSlot] = this.ByteIndex;
				return;
			}
			this.SavedByteIndeces.Add(saveSlot, this.ByteIndex);
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x00132A18 File Offset: 0x00130C18
		public void RestoreSavedPosition(int saveSlot)
		{
			if (this.SavedBitIndeces.ContainsKey(saveSlot))
			{
				this.BitIndex = this.SavedBitIndeces[saveSlot];
			}
			if (this.SavedByteIndeces.ContainsKey(saveSlot))
			{
				this.ByteIndex = this.SavedByteIndeces[saveSlot];
			}
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x00132A68 File Offset: 0x00130C68
		public string BufferString()
		{
			string text = "";
			for (int i = 0; i < this.Buffer.Length; i++)
			{
				int num = (int)this.Buffer[i];
				string text2 = "";
				for (int j = 0; j < 8; j++)
				{
					text2 = (((num & 1 << j) == 0) ? 0 : 1) + text2;
				}
				text = text2 + " " + text;
			}
			return text;
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x00132ADC File Offset: 0x00130CDC
		public bool CanWrite(int bytesToWrite)
		{
			int num = this.ByteIndex;
			if (this.BitIndex != 0)
			{
				num++;
			}
			return num + bytesToWrite < this.Buffer.Length;
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x00132B0C File Offset: 0x00130D0C
		public void Write(object value, int nullValue = 2147483646)
		{
			UnityEngine.Object x;
			if (value == null || ((x = (value as UnityEngine.Object)) != null && x == null))
			{
				this.WriteInt(nullValue);
				return;
			}
			Type type = value.GetType();
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}
			if (type == typeof(object))
			{
				throw new Exception("Bitstream object type is not supported!");
			}
			if (type == typeof(byte[]))
			{
				this.WriteBytes((byte[])value);
				return;
			}
			if (type.IsArray)
			{
				Array array = (Array)value;
				this.WriteUint((uint)array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					this.Write(array.GetValue(i), 2147483646);
				}
				return;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
			{
				IList list = (IList)value;
				this.WriteUint((uint)list.Count);
				for (int j = 0; j < list.Count; j++)
				{
					this.Write(list[j], 2147483646);
				}
				return;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<, >)))
			{
				IDictionary dictionary = (IDictionary)value;
				this.WriteUint((uint)dictionary.Count);
				using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						this.Write(dictionaryEntry.Key, 2147483646);
						this.Write(dictionaryEntry.Value, 2147483646);
					}
					return;
				}
			}
			if (type == typeof(uint))
			{
				this.WriteUint((uint)value);
				return;
			}
			if (type == typeof(int))
			{
				this.WriteInt((int)value);
				return;
			}
			if (type == typeof(ushort))
			{
				this.WriteUshort((ushort)value);
				return;
			}
			if (type == typeof(float))
			{
				this.WriteFloat((float)value);
				return;
			}
			if (type == typeof(ulong))
			{
				this.WriteUlong((ulong)value);
				return;
			}
			if (type == typeof(long))
			{
				this.WriteLong((long)value);
				return;
			}
			if (type == typeof(byte))
			{
				this.WriteByte((byte)value);
				return;
			}
			if (type == typeof(bool))
			{
				this.WriteBool((bool)value);
				return;
			}
			if (type == typeof(Vector3))
			{
				this.WriteVector3((Vector3)value);
				return;
			}
			if (type == typeof(Quaternion))
			{
				this.WriteQuaternion((Quaternion)value);
				return;
			}
			if (type == typeof(Color))
			{
				this.WriteColor((Color)value);
				return;
			}
			if (type == typeof(Color32))
			{
				this.WriteColor32((Color32)value);
				return;
			}
			if (type == typeof(string))
			{
				this.WriteString((string)value);
				return;
			}
			if (type == typeof(NetworkView))
			{
				this.WriteUshort(((NetworkView)value).id);
				return;
			}
			if (type.IsSubclassOf(typeof(NetworkBehavior)))
			{
				this.WriteUshort(((NetworkBehavior)value).networkView.id);
				return;
			}
			if (type == typeof(NetworkPlayer))
			{
				this.WriteUshort(((NetworkPlayer)value).id);
				return;
			}
			if (type.IsValueType && type.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				this.Write(Convert.ChangeType(value, underlyingType), 2147483646);
				return;
			}
			if ((type.IsClass && !type.IsPrimitive && type != typeof(string)) || (type.IsValueType && !type.IsEnum))
			{
				foreach (VarInfo varInfo in type.GetVars())
				{
					if (BitStream.IsSerializable(varInfo))
					{
						object value2 = varInfo.GetValue(value);
						this.Write(value2, -2147483647);
					}
				}
				VarInfo[] vars;
				if (vars.Length == 0)
				{
					Debug.LogError("Bitstream write no members type: " + type);
					return;
				}
			}
			else
			{
				Debug.LogError("Unsupported bitstream write type: " + type);
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x00132FB4 File Offset: 0x001311B4
		public void Write<T>(T value, int nullValue = 2147483646)
		{
			object obj;
			if (this.cachedWriteActions.TryGetValue(typeof(!!0), out obj))
			{
				((Action<!!0>)obj)(value);
				return;
			}
			this.Write(value, nullValue);
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x00132FF4 File Offset: 0x001311F4
		public object Read(Type type, int nullValue = 2147483646)
		{
			if (type.IsNullable() && this.Peek<int>(this.FuncReadInt) == nullValue)
			{
				this.ReadInt();
				return null;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}
			if (type == typeof(byte[]))
			{
				return this.ReadBytes();
			}
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				int length = (int)this.ReadUint();
				Array array = Array.CreateInstance(elementType, length);
				for (int i = 0; i < array.Length; i++)
				{
					array.SetValue(this.Read(elementType, 2147483646), i);
				}
				return array;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
			{
				Type type2 = type.GetGenericArguments()[0];
				int num = (int)this.ReadUint();
				IList list = (IList)Activator.CreateInstance(type);
				for (int j = 0; j < num; j++)
				{
					list.Add(this.Read(type2, 2147483646));
				}
				return list;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<, >)))
			{
				Type[] genericArguments = type.GetGenericArguments();
				Type type3 = genericArguments[0];
				Type type4 = genericArguments[1];
				int num2 = (int)this.ReadUint();
				IDictionary dictionary = (IDictionary)Activator.CreateInstance(type);
				for (int k = 0; k < num2; k++)
				{
					dictionary.Add(this.Read(type3, 2147483646), this.Read(type4, 2147483646));
				}
				return dictionary;
			}
			if (type == typeof(uint))
			{
				return this.ReadUint();
			}
			if (type == typeof(int))
			{
				return this.ReadInt();
			}
			if (type == typeof(ushort))
			{
				return this.ReadUshort();
			}
			if (type == typeof(float))
			{
				return this.ReadFloat();
			}
			if (type == typeof(ulong))
			{
				return this.ReadUlong();
			}
			if (type == typeof(long))
			{
				return this.ReadLong();
			}
			if (type == typeof(byte))
			{
				return this.ReadByte();
			}
			if (type == typeof(bool))
			{
				return this.ReadBool();
			}
			if (type == typeof(Vector3))
			{
				return this.ReadVector3();
			}
			if (type == typeof(Quaternion))
			{
				return this.ReadQuaternion();
			}
			if (type == typeof(Color))
			{
				return this.ReadColor();
			}
			if (type == typeof(Color32))
			{
				return this.ReadColor32();
			}
			if (type == typeof(string))
			{
				return this.ReadString();
			}
			if (type == typeof(NetworkView))
			{
				return NetworkView.IdView[this.ReadUshort()];
			}
			if (type.IsSubclassOf(typeof(NetworkBehavior)))
			{
				return NetworkView.IdView[this.ReadUshort()].GetComponent(type);
			}
			if (type == typeof(NetworkPlayer))
			{
				return new NetworkPlayer(this.ReadUshort());
			}
			if (type.IsValueType && type.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				return this.Read(underlyingType, 2147483646);
			}
			if ((type.IsClass && !type.IsPrimitive && type != typeof(string)) || (type.IsValueType && !type.IsEnum))
			{
				object obj = Activator.CreateInstance(type);
				foreach (VarInfo varInfo in type.GetVars())
				{
					if (BitStream.IsSerializable(varInfo))
					{
						varInfo.SetValue(obj, this.Read(varInfo.VarType, -2147483647));
					}
				}
				return obj;
			}
			Debug.LogError("Unsupported bitstream read type: " + type);
			return null;
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x00133430 File Offset: 0x00131630
		public T Read<T>(int nullValue = 2147483646)
		{
			object obj;
			if (this.cachedReadFuncs.TryGetValue(typeof(!!0), out obj))
			{
				return ((Func<!!0>)obj)();
			}
			return (!!0)((object)this.Read(typeof(!!0), nullValue));
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x00133478 File Offset: 0x00131678
		public object Peek(Type type)
		{
			int bitIndex = this.BitIndex;
			int byteIndex = this.ByteIndex;
			object result = this.Read(type, 2147483646);
			this.BitIndex = bitIndex;
			this.ByteIndex = byteIndex;
			return result;
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x001334B0 File Offset: 0x001316B0
		public T Peek<T>()
		{
			int bitIndex = this.BitIndex;
			int byteIndex = this.ByteIndex;
			T result = this.Read<T>(2147483646);
			this.BitIndex = bitIndex;
			this.ByteIndex = byteIndex;
			return result;
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x001334E4 File Offset: 0x001316E4
		public T Peek<T>(Func<T> readFunc)
		{
			int bitIndex = this.BitIndex;
			int byteIndex = this.ByteIndex;
			T result = readFunc();
			this.BitIndex = bitIndex;
			this.ByteIndex = byteIndex;
			return result;
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x00133513 File Offset: 0x00131713
		private static bool IsSerializable(VarInfo memberInfo)
		{
			if (!(memberInfo.propertyInfo != null))
			{
				return BitStream.IsSerializable(memberInfo.fieldInfo);
			}
			return BitStream.IsSerializable(memberInfo.propertyInfo);
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x0013353A File Offset: 0x0013173A
		private static bool IsSerializable(FieldInfo fieldInfo)
		{
			return fieldInfo.IsPublic && !fieldInfo.IsStatic && !fieldInfo.IsLiteral && !fieldInfo.IsNotSerialized && !Attribute.IsDefined(fieldInfo, typeof(NonSerializedAttribute));
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x00133571 File Offset: 0x00131771
		private static bool IsSerializable(PropertyInfo propertyInfo)
		{
			return propertyInfo.CanRead && propertyInfo.CanWrite && Attribute.IsDefined(propertyInfo, typeof(SerializeField));
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x00133598 File Offset: 0x00131798
		public void FixByteIndex()
		{
			if (this.BitIndex != 0)
			{
				this.BitIndex = 0;
				int byteIndex = this.ByteIndex;
				this.ByteIndex = byteIndex + 1;
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x001335C4 File Offset: 0x001317C4
		private void CheckBufferBounds(int size)
		{
			if (this.Buffer.Length < this.ByteIndex + size)
			{
				throw new Exception("BitStream buffer bounds overflow");
			}
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x001335E4 File Offset: 0x001317E4
		public void WriteByte(byte value)
		{
			this.FixByteIndex();
			this.Buffer[this.ByteIndex] = value;
			int byteIndex = this.ByteIndex;
			this.ByteIndex = byteIndex + 1;
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x00133618 File Offset: 0x00131818
		public byte ReadByte()
		{
			this.FixByteIndex();
			byte result = this.Buffer[this.ByteIndex];
			int byteIndex = this.ByteIndex;
			this.ByteIndex = byteIndex + 1;
			return result;
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x00133648 File Offset: 0x00131848
		public void WriteBytes(byte[] bytes, int bytesToWrite)
		{
			this.FixByteIndex();
			Array.Copy(bytes, 0, this.Buffer, this.ByteIndex, bytesToWrite);
			this.ByteIndex += bytesToWrite;
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x00133672 File Offset: 0x00131872
		public void WriteBytes(byte[] bytes)
		{
			this.WriteUint((uint)bytes.Length);
			this.WriteBytes(bytes, bytes.Length);
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x00133687 File Offset: 0x00131887
		public void ReadBytes(byte[] output, int bytesToRead)
		{
			this.FixByteIndex();
			Array.Copy(this.Buffer, this.ByteIndex, output, 0, bytesToRead);
			this.ByteIndex += bytesToRead;
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x001336B4 File Offset: 0x001318B4
		public byte[] ReadBytes(int bytesToRead)
		{
			byte[] array = new byte[bytesToRead];
			this.ReadBytes(array, bytesToRead);
			return array;
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x001336D1 File Offset: 0x001318D1
		public byte[] ReadBytes()
		{
			return this.ReadBytes((int)this.ReadUint());
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x001336E0 File Offset: 0x001318E0
		public unsafe void WriteUlong(ulong value)
		{
			this.FixByteIndex();
			byte[] array;
			byte* ptr;
			if ((array = this.Buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			*(long*)(ptr + this.ByteIndex) = (long)value;
			array = null;
			this.ByteIndex += 8;
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x0013372C File Offset: 0x0013192C
		public unsafe ulong ReadUlong()
		{
			this.FixByteIndex();
			byte[] array;
			byte* ptr;
			if ((array = this.Buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			ulong result = (ulong)(*(long*)(ptr + this.ByteIndex));
			array = null;
			this.ByteIndex += 8;
			return result;
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x00133778 File Offset: 0x00131978
		public unsafe void WriteLong(long value)
		{
			this.FixByteIndex();
			byte[] array;
			byte* ptr;
			if ((array = this.Buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			*(long*)(ptr + this.ByteIndex) = value;
			array = null;
			this.ByteIndex += 8;
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x001337C4 File Offset: 0x001319C4
		public unsafe long ReadLong()
		{
			this.FixByteIndex();
			byte[] array;
			byte* ptr;
			if ((array = this.Buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			long result = *(long*)(ptr + this.ByteIndex);
			array = null;
			this.ByteIndex += 8;
			return result;
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x00133810 File Offset: 0x00131A10
		public unsafe void WriteUint(uint value)
		{
			if (this.BitIndex == 0)
			{
				byte[] array;
				byte* ptr;
				if ((array = this.Buffer) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				*(int*)(ptr + this.ByteIndex) = (int)value;
				array = null;
				this.ByteIndex += 4;
				return;
			}
			this.WriteUint(value, 32);
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x00133868 File Offset: 0x00131A68
		public unsafe uint ReadUint()
		{
			if (this.BitIndex == 0)
			{
				byte[] array;
				byte* ptr;
				if ((array = this.Buffer) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				uint result = *(uint*)(ptr + this.ByteIndex);
				array = null;
				this.ByteIndex += 4;
				return result;
			}
			return this.ReadUint(32);
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x001338BC File Offset: 0x00131ABC
		public void WriteUint(uint value, int numBits)
		{
			if (numBits == 32 && this.BitIndex == 0)
			{
				this.WriteUint(value);
				return;
			}
			uint num = (uint)this.Buffer[this.ByteIndex];
			for (int i = 0; i < numBits; i++)
			{
				if (((ulong)value & (ulong)(1L << (i & 31))) > 0UL)
				{
					num |= 1U << this.BitIndex;
				}
				else
				{
					num &= ~(1U << this.BitIndex);
				}
				int num2 = this.BitIndex;
				this.BitIndex = num2 + 1;
				if (this.BitIndex == 8)
				{
					this.BitIndex = 0;
					this.Buffer[this.ByteIndex] = (byte)num;
					num2 = this.ByteIndex;
					this.ByteIndex = num2 + 1;
					if (this.ByteIndex >= this.Buffer.Length)
					{
						break;
					}
					num = (uint)this.Buffer[this.ByteIndex];
				}
			}
			if (this.ByteIndex < this.Buffer.Length)
			{
				this.Buffer[this.ByteIndex] = (byte)num;
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x001339A8 File Offset: 0x00131BA8
		public uint ReadUint(int numBits)
		{
			if (numBits == 32 && this.BitIndex == 0)
			{
				return this.ReadUint();
			}
			uint num = 0U;
			uint num2 = (uint)this.Buffer[this.ByteIndex];
			for (int i = 0; i < numBits; i++)
			{
				uint num3 = ((num2 & 1U << this.BitIndex) > 0U) ? 1U : 0U;
				num |= num3 << i;
				int num4 = this.BitIndex;
				this.BitIndex = num4 + 1;
				if (this.BitIndex == 8)
				{
					this.BitIndex = 0;
					num4 = this.ByteIndex;
					this.ByteIndex = num4 + 1;
					if (this.ByteIndex >= this.Buffer.Length)
					{
						break;
					}
					num2 = (uint)this.Buffer[this.ByteIndex];
				}
			}
			return num;
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x00133A58 File Offset: 0x00131C58
		public unsafe void WriteInt(int value)
		{
			uint value2 = (uint)(*(&value));
			this.WriteUint(value2, 32);
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x00133A74 File Offset: 0x00131C74
		public unsafe int ReadInt()
		{
			uint num = this.ReadUint(32);
			return (int)(*(&num));
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x00133A8E File Offset: 0x00131C8E
		public void WriteInt(int value, int numBits)
		{
			if (numBits == 32)
			{
				this.WriteInt(value);
				return;
			}
			if (value < 0)
			{
				this.WriteBool(false);
				this.WriteUint((uint)(value * -1), numBits);
				return;
			}
			this.WriteBool(true);
			this.WriteUint((uint)value, numBits);
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x00133AC4 File Offset: 0x00131CC4
		public int ReadInt(int numBits)
		{
			if (numBits == 32)
			{
				return this.ReadInt();
			}
			bool flag = this.ReadBool();
			int num = (int)this.ReadUint(numBits);
			if (!flag)
			{
				return num * -1;
			}
			return num;
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x00133AF2 File Offset: 0x00131CF2
		public void WriteBool(bool value)
		{
			this.WriteUint(value ? 1U : 0U, 1);
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x00133B02 File Offset: 0x00131D02
		public bool ReadBool()
		{
			return this.ReadUint(1) == 1U;
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00133B10 File Offset: 0x00131D10
		public unsafe void WriteUshort(ushort value)
		{
			if (this.BitIndex == 0)
			{
				byte[] array;
				byte* ptr;
				if ((array = this.Buffer) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				*(short*)(ptr + this.ByteIndex) = (short)value;
				array = null;
				this.ByteIndex += 2;
				return;
			}
			this.WriteUint((uint)value, 16);
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x00133B68 File Offset: 0x00131D68
		public unsafe ushort ReadUshort()
		{
			if (this.BitIndex == 0)
			{
				byte[] array;
				byte* ptr;
				if ((array = this.Buffer) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				ushort result = *(ushort*)(ptr + this.ByteIndex);
				array = null;
				this.ByteIndex += 2;
				return result;
			}
			return (ushort)this.ReadUint(16);
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x00133BC0 File Offset: 0x00131DC0
		public unsafe void WriteFloat(float value)
		{
			uint value2 = *(uint*)(&value);
			this.WriteUint(value2, 32);
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x00133BDC File Offset: 0x00131DDC
		public unsafe float ReadFloat()
		{
			uint num = this.ReadUint(32);
			return *(float*)(&num);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x00133BF8 File Offset: 0x00131DF8
		public float PeekFloat()
		{
			int bitIndex = this.BitIndex;
			int byteIndex = this.ByteIndex;
			float result = this.ReadFloat();
			this.BitIndex = bitIndex;
			this.ByteIndex = byteIndex;
			return result;
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x00133C28 File Offset: 0x00131E28
		public void WriteString(string value)
		{
			this.FixByteIndex();
			int num = 2147483646;
			if (value != null)
			{
				num = Encoding.UTF8.GetBytes(value, 0, value.Length, this.Buffer, this.ByteIndex + 4);
			}
			this.WriteInt(num);
			this.ByteIndex += num;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x00133C7C File Offset: 0x00131E7C
		public string ReadString()
		{
			this.FixByteIndex();
			int num = this.ReadInt();
			string result = null;
			if (num != 2147483646)
			{
				result = Encoding.UTF8.GetString(this.Buffer, this.ByteIndex, num);
			}
			this.ByteIndex += num;
			return result;
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x00133CC8 File Offset: 0x00131EC8
		public string PeekString()
		{
			int bitIndex = this.BitIndex;
			int byteIndex = this.ByteIndex;
			string result = this.ReadString();
			this.BitIndex = bitIndex;
			this.ByteIndex = byteIndex;
			return result;
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x00133CF7 File Offset: 0x00131EF7
		public void WriteVector2(Vector2 value)
		{
			this.WriteFloat(value.x);
			this.WriteFloat(value.y);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x00133D11 File Offset: 0x00131F11
		public void WriteVector3(Vector3 value)
		{
			this.WriteFloat(value.x);
			this.WriteFloat(value.y);
			this.WriteFloat(value.z);
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x00133D37 File Offset: 0x00131F37
		public void WriteVector4(Vector4 value)
		{
			this.WriteFloat(value.x);
			this.WriteFloat(value.y);
			this.WriteFloat(value.z);
			this.WriteFloat(value.w);
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x00133D69 File Offset: 0x00131F69
		public Vector2 ReadVector2()
		{
			return new Vector2(this.ReadFloat(), this.ReadFloat());
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x00133D7C File Offset: 0x00131F7C
		public Vector3 ReadVector3()
		{
			return new Vector3(this.ReadFloat(), this.ReadFloat(), this.ReadFloat());
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x00133D95 File Offset: 0x00131F95
		public Vector4 ReadVector4()
		{
			return new Vector4(this.ReadFloat(), this.ReadFloat(), this.ReadFloat(), this.ReadFloat());
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x00133DB4 File Offset: 0x00131FB4
		public void WriteQuaternion(Quaternion v)
		{
			this.WriteFloat(v.x);
			this.WriteFloat(v.y);
			this.WriteFloat(v.z);
			this.WriteFloat(v.w);
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x00133DE6 File Offset: 0x00131FE6
		public Quaternion ReadQuaternion()
		{
			return new Quaternion(this.ReadFloat(), this.ReadFloat(), this.ReadFloat(), this.ReadFloat());
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x00133E05 File Offset: 0x00132005
		public void WriteColor32(Color32 color)
		{
			this.WriteByte(color.r);
			this.WriteByte(color.g);
			this.WriteByte(color.b);
			this.WriteByte(color.a);
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x00133E37 File Offset: 0x00132037
		public Color32 ReadColor32()
		{
			return new Color32(this.ReadByte(), this.ReadByte(), this.ReadByte(), this.ReadByte());
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x00133E56 File Offset: 0x00132056
		public void WriteColor(Color color)
		{
			this.WriteFloat(color.r);
			this.WriteFloat(color.g);
			this.WriteFloat(color.b);
			this.WriteFloat(color.a);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x00133E88 File Offset: 0x00132088
		public Color ReadColor()
		{
			return new Color(this.ReadFloat(), this.ReadFloat(), this.ReadFloat(), this.ReadFloat());
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x00133EA8 File Offset: 0x001320A8
		public void SkipBits(int bitCount)
		{
			int num = bitCount / 8;
			int num2 = bitCount - num * 8;
			this.ByteIndex += num;
			this.BitIndex += num2;
			if (this.BitIndex >= 8)
			{
				this.BitIndex -= 8;
				int byteIndex = this.ByteIndex;
				this.ByteIndex = byteIndex + 1;
			}
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x00133F02 File Offset: 0x00132102
		public void WritePositionLossy(Vector3 pos)
		{
			this.WritePositionLossyCoordinate(pos.x);
			this.WritePositionLossyCoordinate(pos.y);
			this.WritePositionLossyCoordinate(pos.z);
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x00133F28 File Offset: 0x00132128
		public void WritePositionLossyCoordinate(float pos)
		{
			uint value = (uint)((pos + 250f) / 500f * (float)BitStream.PositionBitMax);
			this.WriteUint(value, 17);
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x00133F54 File Offset: 0x00132154
		public Vector3 ReadPositionLossy()
		{
			return new Vector3(this.ReadPositionLossyCoordinate(), this.ReadPositionLossyCoordinate(), this.ReadPositionLossyCoordinate());
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x00133F6D File Offset: 0x0013216D
		public float ReadPositionLossyCoordinate()
		{
			return this.ReadUint(17) / (float)BitStream.PositionBitMax * 500f - 250f;
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x00133F8C File Offset: 0x0013218C
		public void WritePosition(Vector3 pos)
		{
			this.WritePositionCoordinate(pos.x);
			this.WritePositionCoordinate(pos.y);
			this.WritePositionCoordinate(pos.z);
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x00133FB2 File Offset: 0x001321B2
		public void WritePositionCoordinate(float pos)
		{
			this.WriteFloat(pos);
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x00133FBB File Offset: 0x001321BB
		public Vector3 ReadPosition()
		{
			return new Vector3(this.ReadPositionCoordinate(), this.ReadPositionCoordinate(), this.ReadPositionCoordinate());
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x00133FD4 File Offset: 0x001321D4
		public float ReadPositionCoordinate()
		{
			return this.ReadFloat();
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x00133FDC File Offset: 0x001321DC
		public void WriteRotationLossy(Vector3 rot)
		{
			this.WriteRotationLossyCoordinate(rot.x);
			this.WriteRotationLossyCoordinate(rot.y);
			this.WriteRotationLossyCoordinate(rot.z);
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x00134004 File Offset: 0x00132204
		public void WriteRotationLossyCoordinate(float value)
		{
			uint value2 = (uint)(value / 360f * (float)BitStream.RotationBitMax);
			this.WriteUint(value2, 12);
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x0013402A File Offset: 0x0013222A
		public Vector3 ReadRotationLossy()
		{
			return new Vector3(this.ReadRotationLossyCoordinate(), this.ReadRotationLossyCoordinate(), this.ReadRotationLossyCoordinate());
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x00134043 File Offset: 0x00132243
		public float ReadRotationLossyCoordinate()
		{
			return this.ReadUint(12) / (float)BitStream.RotationBitMax * 360f;
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x0013405C File Offset: 0x0013225C
		public void WriteRotation(Vector3 rot)
		{
			this.WriteRotationCoordinate(rot.x);
			this.WriteRotationCoordinate(rot.y);
			this.WriteRotationCoordinate(rot.z);
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x00133FB2 File Offset: 0x001321B2
		public void WriteRotationCoordinate(float value)
		{
			this.WriteFloat(value);
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x00134082 File Offset: 0x00132282
		public Vector3 ReadRotation()
		{
			return new Vector3(this.ReadRotationCoordinate(), this.ReadRotationCoordinate(), this.ReadRotationCoordinate());
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x00133FD4 File Offset: 0x001321D4
		public float ReadRotationCoordinate()
		{
			return this.ReadFloat();
		}

		// Token: 0x04001D61 RID: 7521
		public const int BIT_COUNT_POSITION = 17;

		// Token: 0x04001D62 RID: 7522
		public const int BIT_COUNT_ROTATION = 12;

		// Token: 0x04001D63 RID: 7523
		public const float SIZE_POSITION = 250f;

		// Token: 0x04001D64 RID: 7524
		public const float DOUBLE_SIZE_POSITION = 500f;

		// Token: 0x04001D65 RID: 7525
		public static readonly int PositionBitMax = (int)(Math.Pow(2.0, 17.0) - 1.0);

		// Token: 0x04001D66 RID: 7526
		public static readonly int RotationBitMax = (int)(Math.Pow(2.0, 12.0) - 1.0);

		// Token: 0x04001D67 RID: 7527
		private Dictionary<Type, object> cachedWriteActions;

		// Token: 0x04001D68 RID: 7528
		private Dictionary<Type, object> cachedReadFuncs;

		// Token: 0x04001D69 RID: 7529
		public Func<int> FuncReadInt;

		// Token: 0x04001D6A RID: 7530
		public Func<ushort> FuncReadUshort;

		// Token: 0x04001D6B RID: 7531
		private static List<BitStream.PoolStream> streamPool = new List<BitStream.PoolStream>();

		// Token: 0x04001D71 RID: 7537
		public Dictionary<int, int> SavedByteIndeces;

		// Token: 0x04001D72 RID: 7538
		public Dictionary<int, int> SavedBitIndeces;

		// Token: 0x04001D73 RID: 7539
		private const int nullMaxValue = 2147483646;

		// Token: 0x04001D74 RID: 7540
		private const int nullMinValue = -2147483647;

		// Token: 0x020007CC RID: 1996
		public class PoolStream
		{
			// Token: 0x1700083E RID: 2110
			// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x001825A4 File Offset: 0x001807A4
			// (set) Token: 0x06003FD8 RID: 16344 RVA: 0x001825AC File Offset: 0x001807AC
			public BitStream stream { get; private set; }

			// Token: 0x06003FD9 RID: 16345 RVA: 0x001825B5 File Offset: 0x001807B5
			public PoolStream(BitStream stream)
			{
				this.stream = stream;
				this.inUse = true;
			}

			// Token: 0x04002D6F RID: 11631
			public bool inUse;
		}
	}
}
