using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x020004B6 RID: 1206
	public class JSONNode
	{
		// Token: 0x06003574 RID: 13684 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x17000723 RID: 1827
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000724 RID: 1828
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06003579 RID: 13689 RVA: 0x001649B5 File Offset: 0x00162BB5
		// (set) Token: 0x0600357A RID: 13690 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x0600357B RID: 13691 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x001649BC File Offset: 0x00162BBC
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x00079594 File Offset: 0x00077794
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x00079594 File Offset: 0x00077794
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x001649CA File Offset: 0x00162BCA
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06003580 RID: 13696 RVA: 0x001649CD File Offset: 0x00162BCD
		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06003581 RID: 13697 RVA: 0x001649D6 File Offset: 0x00162BD6
		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode jsonnode in this.Childs)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChilds)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x001649E6 File Offset: 0x00162BE6
		public override string ToString()
		{
			return "JSONNode";
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x001649E6 File Offset: 0x00162BE6
		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06003584 RID: 13700 RVA: 0x001649F0 File Offset: 0x00162BF0
		// (set) Token: 0x06003585 RID: 13701 RVA: 0x00164A11 File Offset: 0x00162C11
		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06003586 RID: 13702 RVA: 0x00164A20 File Offset: 0x00162C20
		// (set) Token: 0x06003587 RID: 13703 RVA: 0x00164A49 File Offset: 0x00162C49
		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x00164A58 File Offset: 0x00162C58
		// (set) Token: 0x06003589 RID: 13705 RVA: 0x00164A89 File Offset: 0x00162C89
		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600358A RID: 13706 RVA: 0x00164A98 File Offset: 0x00162C98
		// (set) Token: 0x0600358B RID: 13707 RVA: 0x00164AC6 File Offset: 0x00162CC6
		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600358C RID: 13708 RVA: 0x00164ADD File Offset: 0x00162CDD
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x0600358D RID: 13709 RVA: 0x00164AE5 File Offset: 0x00162CE5
		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x00164AED File Offset: 0x00162CED
		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x00164AF5 File Offset: 0x00162CF5
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x00164B08 File Offset: 0x00162D08
		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || a == b;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x00164B1B File Offset: 0x00162D1B
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x00164B27 File Offset: 0x00162D27
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x00164B2D File Offset: 0x00162D2D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x00164B38 File Offset: 0x00162D38
		internal static string Escape(string aText)
		{
			string text = "";
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				case '\v':
					goto IL_A3;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_A3;
						}
						text += "\\\\";
					}
					else
					{
						text += "\\\"";
					}
					break;
				}
				IL_B1:
				i++;
				continue;
				IL_A3:
				text += c.ToString();
				goto IL_B1;
			}
			return text;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x00164C08 File Offset: 0x00162E08
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = "";
			string text2 = "";
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_429;
						case '\v':
						case '\f':
							goto IL_412;
						default:
							if (c != ' ')
							{
								goto IL_412;
							}
							break;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_412;
						}
						if (flag)
						{
							text += aJSON[i].ToString();
						}
						else
						{
							if (text != "")
							{
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(text);
								}
								else if (text2 != "")
								{
									jsonnode.Add(text2, text);
								}
							}
							text2 = "";
							text = "";
						}
					}
					else
					{
						flag = !flag;
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									text += aJSON[i].ToString();
									goto IL_429;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != "")
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = "";
								text = "";
								jsonnode = stack.Peek();
								goto IL_429;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											text += "\b";
											goto IL_429;
										}
										if (c2 == 'f')
										{
											text += "\f";
											goto IL_429;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											text += "\n";
											goto IL_429;
										}
										switch (c2)
										{
										case 'r':
											text += "\r";
											goto IL_429;
										case 't':
											text += "\t";
											goto IL_429;
										case 'u':
										{
											string s = aJSON.Substring(i + 1, 4);
											text += ((char)int.Parse(s, NumberStyles.AllowHexSpecifier)).ToString();
											i += 4;
											goto IL_429;
										}
										}
									}
									text += c2.ToString();
									goto IL_429;
								}
								goto IL_429;
							case ']':
								break;
							default:
								goto IL_412;
							}
						}
						else
						{
							if (flag)
							{
								text += aJSON[i].ToString();
								goto IL_429;
							}
							text2 = text;
							text = "";
							goto IL_429;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_412;
						}
					}
					else
					{
						if (flag)
						{
							text += aJSON[i].ToString();
							goto IL_429;
						}
						stack.Push(new JSONClass());
						if (jsonnode != null)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(stack.Peek());
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, stack.Peek());
							}
						}
						text2 = "";
						text = "";
						jsonnode = stack.Peek();
						goto IL_429;
					}
					if (flag)
					{
						text += aJSON[i].ToString();
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != "")
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.Add(text);
							}
							else if (text2 != "")
							{
								jsonnode.Add(text2, text);
							}
						}
						text2 = "";
						text = "";
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_429:
				i++;
				continue;
				IL_412:
				text += aJSON[i].ToString();
				goto IL_429;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x00165060 File Offset: 0x00163260
		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x0016507B File Offset: 0x0016327B
		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x0016507B File Offset: 0x0016327B
		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0016507B File Offset: 0x0016327B
		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x00165088 File Offset: 0x00163288
		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x001650D8 File Offset: 0x001632D8
		public string SaveToBase64()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = 0L;
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x00165124 File Offset: 0x00163324
		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string aKey = aReader.ReadString();
					JSONNode aItem = JSONNode.Deserialize(aReader);
					jsonclass.Add(aKey, aItem);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x0016507B File Offset: 0x0016327B
		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x0016507B File Offset: 0x0016327B
		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x0016507B File Offset: 0x0016327B
		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x00165218 File Offset: 0x00163418
		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x00165250 File Offset: 0x00163450
		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode result;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				result = JSONNode.LoadFromStream(fileStream);
			}
			return result;
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x00165288 File Offset: 0x00163488
		public static JSONNode LoadFromBase64(string aBase64)
		{
			return JSONNode.LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = 0L
			});
		}
	}
}
