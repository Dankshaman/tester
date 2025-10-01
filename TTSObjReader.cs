using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class TTSObjReader
{
	// Token: 0x06001F1F RID: 7967 RVA: 0x000DCEF2 File Offset: 0x000DB0F2
	public TTSGameObject ConvertString(string objString)
	{
		return this.CreateObjects(ref objString, null, "");
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x000DCF04 File Offset: 0x000DB104
	private TTSGameObject CreateObjects(ref string objFile, TTSObjReader.ObjData objData, string fileName)
	{
		string[] array = objFile.Split(new char[]
		{
			'\n'
		});
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = false;
		bool flag2 = false;
		int item = 0;
		int num4 = 0;
		List<int> list = new List<int>();
		int num5 = 0;
		this.maxPoints = this.Clamp(this.maxPoints, 0, 65534);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Length >= 2)
			{
				char c = char.ToLower(array[i][0]);
				char c2 = char.ToLower(array[i][1]);
				if (c == 'f' && c2 == ' ')
				{
					if (!flag)
					{
						item = num5;
					}
					num5++;
					if (flag2 && !flag)
					{
						num4++;
						list.Add(item);
						flag2 = false;
					}
					flag = true;
				}
				else if ((c == 'o' && c2 == ' ') || (c == 'g' && c2 == ' ') || (c == 'u' && c2 == 's'))
				{
					flag2 = true;
					flag = false;
				}
				else if (c == 'v' && c2 == ' ')
				{
					num++;
				}
				else if (c == 'v' && c2 == 't')
				{
					num2++;
				}
				else if (this.useSuppliedNormals && c == 'v' && c2 == 'n')
				{
					num3++;
				}
			}
		}
		if (num4 == 0)
		{
			list.Add(item);
			num4 = 1;
		}
		if (num == 0)
		{
			return new TTSGameObject(true);
		}
		if (num5 == 0)
		{
			return new TTSGameObject(true);
		}
		list.Add(-1);
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		Vector3[] array2 = new Vector3[num];
		Vector2[] array3 = new Vector2[num2];
		Vector3[] array4 = new Vector3[num3];
		List<string> list2 = new List<string>();
		bool flag3 = false;
		bool flag4 = false;
		string[] array5 = new string[num4];
		int j = 0;
		string[] array6 = new string[0];
		int[] array7 = new int[num4 + 1];
		num5 = 0;
		num4 = 0;
		int num9 = 0;
		try
		{
			while (j < array.Length)
			{
				string text = array[j++];
				if (text.Length >= 3 && text[0] != '#')
				{
					this.CleanLine(ref text);
					if (text.Length >= 3)
					{
						while (text[text.Length - 1] == '\\' && j < array.Length)
						{
							text = text.Substring(0, text.Length - 1) + " " + array[j++].TrimEnd(Array.Empty<char>());
							this.CleanLine(ref text);
						}
						char c3 = char.ToLower(text[0]);
						char c4 = char.ToLower(text[1]);
						if (((c3 == 'o' && c4 == ' ') || (c3 == 'g' && c4 == ' ')) && num9++ == 0)
						{
							if (this.useFileNameAsObjectName && fileName != "")
							{
								array5[num4] = fileName;
							}
							else
							{
								array5[num4] = text.Substring(2, text.Length - 2);
							}
						}
						else if (c3 == 'v' && c4 == ' ')
						{
							array6 = text.Split(new char[]
							{
								' '
							});
							if (array6.Length != 4)
							{
								throw new Exception("Incorrect number of points while trying to read vertices:\n" + text + "\n");
							}
							array2[num6++] = new Vector3(-float.Parse(array6[1], CultureInfo.InvariantCulture), float.Parse(array6[2], CultureInfo.InvariantCulture), float.Parse(array6[3], CultureInfo.InvariantCulture));
						}
						else if (c3 == 'v' && c4 == 't')
						{
							array6 = text.Split(new char[]
							{
								' '
							});
							if (array6.Length > 4 || array6.Length < 3)
							{
								throw new Exception("Incorrect number of points while trying to read UV data:\n" + text + "\n");
							}
							array3[num7++] = new Vector2(float.Parse(array6[1], CultureInfo.InvariantCulture), float.Parse(array6[2], CultureInfo.InvariantCulture));
						}
						else if (this.useSuppliedNormals && c3 == 'v' && c4 == 'n')
						{
							array6 = text.Split(new char[]
							{
								' '
							});
							if (array6.Length != 4)
							{
								throw new Exception("Incorrect number of points while trying to read normals:\n" + text + "\n");
							}
							array4[num8++] = new Vector3(-float.Parse(array6[1], CultureInfo.InvariantCulture), float.Parse(array6[2], CultureInfo.InvariantCulture), float.Parse(array6[3], CultureInfo.InvariantCulture));
						}
						else if (c3 == 'f' && c4 == ' ')
						{
							array6 = text.Split(new char[]
							{
								' '
							});
							if (array6.Length >= 4 && array6.Length <= 5)
							{
								if (array6[1].Substring(0, 1) == "-")
								{
									for (int k = 1; k < array6.Length; k++)
									{
										string[] array8 = array6[k].Split(new char[]
										{
											'/'
										});
										array8[0] = (num6 - -int.Parse(array8[0]) + 1).ToString();
										if (array8.Length > 1)
										{
											if (array8[1] != "")
											{
												array8[1] = (num7 - -int.Parse(array8[1]) + 1).ToString();
											}
											if (array8.Length == 3)
											{
												array8[2] = (num8 - -int.Parse(array8[2]) + 1).ToString();
											}
										}
										array6[k] = string.Join("/", array8);
									}
								}
								for (int l = 1; l < 4; l++)
								{
									list2.Add(array6[l]);
								}
								if (array6.Length == 5)
								{
									flag3 = true;
									list2.Add(array6[1]);
									list2.Add(array6[3]);
									list2.Add(array6[4]);
								}
							}
							else
							{
								flag4 = true;
							}
							if (++num5 == list[num4 + 1])
							{
								array7[++num4] = list2.Count;
								num9 = 0;
							}
						}
					}
				}
			}
		}
		catch (Exception)
		{
			return new TTSGameObject(true);
		}
		int num10;
		if (this.combineMultipleGroups && !this.useSubmeshesWhenCombining)
		{
			num10 = 1;
			array7[1] = list2.Count;
		}
		else
		{
			array7[num4 + 1] = list2.Count;
			num10 = array7.Length - 1;
		}
		int[] array9 = new int[list2.Count];
		int[] array10 = new int[list2.Count];
		int[] array11 = new int[list2.Count];
		int num11 = 3;
		for (int m = 0; m < list2.Count; m++)
		{
			array6 = list2[m].Split(new char[]
			{
				'/'
			});
			array9[m] = int.Parse(array6[0]) - 1;
			if (array6.Length > 1)
			{
				if (array6[1] != "")
				{
					array10[m] = int.Parse(array6[1]) - 1;
				}
				if (array6.Length == num11 && this.useSuppliedNormals)
				{
					array11[m] = int.Parse(array6[2]) - 1;
				}
			}
		}
		List<Vector3> list3 = new List<Vector3>(array2);
		if (num2 > 0)
		{
			this.SplitOnUVs(list2, array9, array10, list3, array3, array2, ref num6);
		}
		if (flag3)
		{
			bool flag5 = this.suppressWarnings;
		}
		if (flag4)
		{
			bool flag6 = this.suppressWarnings;
		}
		if (num2 == 0)
		{
			bool flag7 = this.suppressWarnings;
		}
		if (num3 == 0)
		{
			bool flag8 = this.suppressWarnings;
		}
		if (num == 0 && list2.Count == 0)
		{
			return new TTSGameObject(true);
		}
		if (num == 0)
		{
			return new TTSGameObject(true);
		}
		if (list2.Count == 0)
		{
			return new TTSGameObject(true);
		}
		TTSGameObject ttsgameObject = new TTSGameObject(false);
		TTSMesh ttsmesh = default(TTSMesh);
		Vector3[] array12 = null;
		Vector2[] array13 = null;
		Vector3[] array14 = null;
		int[] array15 = null;
		bool flag9 = this.combineMultipleGroups && this.useSubmeshesWhenCombining && num10 > 1;
		for (int n = 0; n < num10; n++)
		{
			ttsmesh = default(TTSMesh);
			if (!flag9 || (flag9 && n == 0))
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				List<Vector3> list4 = new List<Vector3>();
				int num12 = 0;
				int num13 = 0;
				int num14 = array7[n];
				int num15 = array7[n + 1];
				if (flag9)
				{
					num14 = array7[0];
					num15 = array7[num10];
				}
				for (int num16 = num14; num16 < num15; num16++)
				{
					if (!dictionary.TryGetValue(array9[num16], out num13))
					{
						dictionary[array9[num16]] = num12++;
						list4.Add(list3[array9[num16]]);
					}
				}
				if (list4.Count > this.maxPoints)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"The number of vertices in the object ",
						array5[n],
						" exceeds the maximum allowable limit of ",
						this.maxPoints
					}));
					Debug.LogError("Number of vertices: " + list4.Count);
					return new TTSGameObject(true);
				}
				array12 = new Vector3[list4.Count];
				array13 = new Vector2[list4.Count];
				if (num8 > 0)
				{
					array14 = new Vector3[list4.Count];
				}
				else
				{
					array14 = null;
				}
				array15 = new int[num15 - num14];
				if (this.scaleFactor == Vector3.one && this.objRotation == Vector3.zero && this.objPosition == Vector3.zero)
				{
					for (int num17 = 0; num17 < list4.Count; num17++)
					{
						array12[num17] = list4[num17];
					}
				}
				if (num7 > 0 && num8 > 0 && this.useSuppliedNormals)
				{
					for (int num18 = num14; num18 < num15; num18++)
					{
						array13[dictionary[array9[num18]]] = array3[array10[num18]];
						array14[dictionary[array9[num18]]] = array4[array11[num18]].normalized;
					}
				}
				else
				{
					if (num7 > 0)
					{
						for (int num19 = num14; num19 < num15; num19++)
						{
							array13[dictionary[array9[num19]]] = array3[array10[num19]];
						}
					}
					else
					{
						array13 = null;
					}
					if (num8 > 0 && this.useSuppliedNormals)
					{
						for (int num20 = num14; num20 < num15; num20++)
						{
							array14[dictionary[array9[num20]]] = array4[array11[num20]];
						}
					}
				}
				num12 = 0;
				for (int num21 = num14; num21 < num15; num21 += 3)
				{
					array15[num12] = dictionary[array9[num21]];
					array15[num12 + 1] = dictionary[array9[num21 + 2]];
					array15[num12 + 2] = dictionary[array9[num21 + 1]];
					num12 += 3;
				}
				ttsmesh.vertices = array12;
				ttsmesh.uv = array13;
				if (this.useSuppliedNormals)
				{
					ttsmesh.normals = array14;
				}
			}
			if (flag9)
			{
				int num22 = array7[n + 1] - array7[n];
				int[] array16 = new int[num22];
				Array.Copy(array15, array7[n], array16, 0, num22);
				ttsmesh.triangles = array16;
			}
			else
			{
				ttsmesh.triangles = array15;
			}
			if (!flag9 || (flag9 && n == num10 - 1))
			{
				if (num8 == 0 || !this.useSuppliedNormals)
				{
					ttsmesh.normals = null;
					array14 = null;
				}
				if (this.computeTangents && array12 != null && array14 != null && array13 != null && array15 != null)
				{
					Vector4[] tangents = new Vector4[array12.Length];
					TTSObjReader.CalculateTangents(array12, array14, array13, array15, tangents);
					ttsmesh.tangents = tangents;
				}
			}
			if (n == 0 && ttsmesh.normals != null)
			{
				for (int num23 = 0; num23 < ttsmesh.normals.Length; num23++)
				{
				}
			}
			if (n == 0 && ttsmesh.normals != null)
			{
				for (int num24 = 0; num24 < ttsmesh.normals.Length; num24++)
				{
					if (float.IsNaN(ttsmesh.normals[num24].x))
					{
						ttsmesh.normals[num24] = new Vector3(0f, ttsmesh.normals[num24].y, ttsmesh.normals[num24].z);
					}
					if (float.IsNaN(ttsmesh.normals[num24].y))
					{
						ttsmesh.normals[num24] = new Vector3(ttsmesh.normals[num24].x, 0f, ttsmesh.normals[num24].z);
					}
					if (float.IsNaN(ttsmesh.normals[num24].z))
					{
						ttsmesh.normals[num24] = new Vector3(ttsmesh.normals[num24].x, ttsmesh.normals[num24].y, 0f);
					}
				}
			}
			ttsgameObject.meshes.Add(ttsmesh);
		}
		if (objData != null)
		{
			objData.SetDone();
			return new TTSGameObject(true);
		}
		return ttsgameObject;
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x000DDBE4 File Offset: 0x000DBDE4
	private void CleanLine(ref string line)
	{
		while (line.IndexOf("  ") != -1)
		{
			line = line.Replace("  ", " ");
		}
		line = line.Trim();
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x000DDC14 File Offset: 0x000DBE14
	public static void CalculateTangents(Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles, Vector4[] tangents)
	{
		Vector3[] array = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			array[i] = default(Vector3);
		}
		Vector3[] array2 = new Vector3[vertices.Length];
		for (int j = 0; j < vertices.Length; j++)
		{
			array2[j] = default(Vector3);
		}
		int num = triangles.Length;
		int num2 = tangents.Length;
		Vector3 b = default(Vector3);
		Vector3 b2 = default(Vector3);
		for (int k = 0; k < num; k += 3)
		{
			int num3 = triangles[k];
			int num4 = triangles[k + 1];
			int num5 = triangles[k + 2];
			Vector3 vector = vertices[num3];
			Vector3 vector2 = vertices[num4];
			Vector3 vector3 = vertices[num5];
			Vector2 vector4 = uv[num3];
			Vector2 vector5 = uv[num4];
			Vector2 vector6 = uv[num5];
			float num6 = vector2.x - vector.x;
			float num7 = vector3.x - vector.x;
			float num8 = vector2.y - vector.y;
			float num9 = vector3.y - vector.y;
			float num10 = vector2.z - vector.z;
			float num11 = vector3.z - vector.z;
			float num12 = vector5.x - vector4.x;
			float num13 = vector6.x - vector4.x;
			float num14 = vector5.y - vector4.y;
			float num15 = vector6.y - vector4.y;
			float num16 = num12 * num15 - num13 * num14;
			float num17 = (num16 == 0f) ? 0f : (1f / num16);
			b.x = (num15 * num6 - num14 * num7) * num17;
			b.y = (num15 * num8 - num14 * num9) * num17;
			b.z = (num15 * num10 - num14 * num11) * num17;
			b2.x = (num12 * num7 - num13 * num6) * num17;
			b2.y = (num12 * num9 - num13 * num8) * num17;
			b2.z = (num12 * num11 - num13 * num10) * num17;
			array[num3] += b;
			array[num4] += b;
			array[num5] += b;
			array2[num3] += b2;
			array2[num4] += b2;
			array2[num5] += b2;
		}
		for (int l = 0; l < num2; l++)
		{
			Vector3 vector7 = normals[l];
			Vector3 vector8 = array[l];
			Vector3 normalized = (vector8 - vector7 * Vector3.Dot(vector7, vector8)).normalized;
			tangents[l] = new Vector4(normalized.x, normalized.y, normalized.z);
			tangents[l].w = ((Vector3.Dot(Vector3.Cross(vector7, vector8), array2[l]) < 0f) ? -1f : 1f);
		}
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x000DDF84 File Offset: 0x000DC184
	public static Vector4[] CalculateTangents(Vector3[] vertices, Vector3[] normals, Vector2[] uv, int[] triangles)
	{
		Vector4[] array = new Vector4[vertices.Length];
		Vector3[] array2 = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			array2[i] = default(Vector3);
		}
		Vector3[] array3 = new Vector3[vertices.Length];
		for (int j = 0; j < vertices.Length; j++)
		{
			array3[j] = default(Vector3);
		}
		int num = triangles.Length;
		int num2 = vertices.Length;
		Vector3 b = default(Vector3);
		Vector3 b2 = default(Vector3);
		for (int k = 0; k < num; k += 3)
		{
			int num3 = triangles[k];
			int num4 = triangles[k + 1];
			int num5 = triangles[k + 2];
			Vector3 vector = vertices[num3];
			Vector3 vector2 = vertices[num4];
			Vector3 vector3 = vertices[num5];
			Vector2 vector4 = uv[num3];
			Vector2 vector5 = uv[num4];
			Vector2 vector6 = uv[num5];
			float num6 = vector2.x - vector.x;
			float num7 = vector3.x - vector.x;
			float num8 = vector2.y - vector.y;
			float num9 = vector3.y - vector.y;
			float num10 = vector2.z - vector.z;
			float num11 = vector3.z - vector.z;
			float num12 = vector5.x - vector4.x;
			float num13 = vector6.x - vector4.x;
			float num14 = vector5.y - vector4.y;
			float num15 = vector6.y - vector4.y;
			float num16 = num12 * num15 - num13 * num14;
			float num17 = (num16 == 0f) ? 0f : (1f / num16);
			b.x = (num15 * num6 - num14 * num7) * num17;
			b.y = (num15 * num8 - num14 * num9) * num17;
			b.z = (num15 * num10 - num14 * num11) * num17;
			b2.x = (num12 * num7 - num13 * num6) * num17;
			b2.y = (num12 * num9 - num13 * num8) * num17;
			b2.z = (num12 * num11 - num13 * num10) * num17;
			array2[num3] += b;
			array2[num4] += b;
			array2[num5] += b;
			array3[num3] += b2;
			array3[num4] += b2;
			array3[num5] += b2;
		}
		for (int l = 0; l < num2; l++)
		{
			Vector3 vector7 = normals[l];
			Vector3 vector8 = array2[l];
			Vector3 normalized = (vector8 - vector7 * Vector3.Dot(vector7, vector8)).normalized;
			array[l] = new Vector4(normalized.x, normalized.y, normalized.z);
			array[l].w = ((Vector3.Dot(Vector3.Cross(vector7, vector8), array3[l]) < 0f) ? -1f : 1f);
		}
		return array;
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x000DE2FC File Offset: 0x000DC4FC
	private void SplitOnUVs(List<string> triData, int[] triVerts, int[] triUVs, List<Vector3> objVertList, Vector2[] objUVs, Vector3[] objVertices, ref int verticesCount)
	{
		Dictionary<int, Vector2> dictionary = new Dictionary<int, Vector2>();
		Dictionary<TTSObjReader.Int2, int> dictionary2 = new Dictionary<TTSObjReader.Int2, int>();
		TTSObjReader.Int2 key = new TTSObjReader.Int2();
		Vector2 zero = Vector2.zero;
		int num = 0;
		for (int i = 0; i < triData.Count; i++)
		{
			if (!dictionary.TryGetValue(triVerts[i], out zero))
			{
				dictionary[triVerts[i]] = objUVs[triUVs[i]];
			}
			else if (dictionary[triVerts[i]] != objUVs[triUVs[i]])
			{
				key = new TTSObjReader.Int2(triVerts[i], triUVs[i]);
				if (dictionary2.TryGetValue(key, out num))
				{
					triVerts[i] = num;
				}
				else
				{
					objVertList.Add(objVertices[triVerts[i]]);
					int num2 = i;
					int num3 = verticesCount;
					verticesCount = num3 + 1;
					triVerts[num2] = num3;
					dictionary[triVerts[i]] = objUVs[triUVs[i]];
					dictionary2[key] = triUVs[i];
				}
			}
		}
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x000DE3EC File Offset: 0x000DC5EC
	private string GetFileName(string line, string token)
	{
		string result = "";
		if (line.Length > token.Length + 2)
		{
			int num = token.Length + 1;
			result = line.Substring(num, line.Length - num).Replace("\r", "");
		}
		return result;
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x000DE438 File Offset: 0x000DC638
	private string GetToken(string line)
	{
		string result = "map_Kd";
		if (line.StartsWith("map_bump"))
		{
			result = "map_bump";
		}
		else if (line.StartsWith("bump"))
		{
			result = "bump";
		}
		return result;
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x000DE474 File Offset: 0x000DC674
	private void ParseKLine(ref string line, ref float r, ref float g, ref float b)
	{
		if (line.Contains(".rfl") && !this.suppressWarnings)
		{
			return;
		}
		if (line.Contains("xyz") && !this.suppressWarnings)
		{
			return;
		}
		try
		{
			string[] array = line.Split(new char[]
			{
				' '
			});
			if (array.Length > 1)
			{
				r = float.Parse(array[1], CultureInfo.InvariantCulture);
			}
			if (array.Length > 3)
			{
				g = float.Parse(array[2], CultureInfo.InvariantCulture);
				b = float.Parse(array[3], CultureInfo.InvariantCulture);
			}
			else
			{
				g = r;
				b = r;
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x000DE520 File Offset: 0x000DC720
	public int Clamp(int x, int y, int z)
	{
		if (x < y)
		{
			return y;
		}
		if (x > z)
		{
			return z;
		}
		return x;
	}

	// Token: 0x040012FF RID: 4863
	public int maxPoints = 30000;

	// Token: 0x04001300 RID: 4864
	public bool combineMultipleGroups = true;

	// Token: 0x04001301 RID: 4865
	public bool useSubmeshesWhenCombining = true;

	// Token: 0x04001302 RID: 4866
	public bool useFileNameAsObjectName;

	// Token: 0x04001303 RID: 4867
	public bool computeTangents = true;

	// Token: 0x04001304 RID: 4868
	public bool useSuppliedNormals = true;

	// Token: 0x04001305 RID: 4869
	public bool overrideDiffuse;

	// Token: 0x04001306 RID: 4870
	public bool overrideSpecular;

	// Token: 0x04001307 RID: 4871
	public bool overrideAmbient;

	// Token: 0x04001308 RID: 4872
	public bool suppressWarnings;

	// Token: 0x04001309 RID: 4873
	public bool useMTLFallback;

	// Token: 0x0400130A RID: 4874
	public bool autoCenterOnOrigin;

	// Token: 0x0400130B RID: 4875
	public Vector3 scaleFactor = new Vector3(1f, 1f, 1f);

	// Token: 0x0400130C RID: 4876
	public Vector3 objRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x0400130D RID: 4877
	public Vector3 objPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x020006F5 RID: 1781
	public class Int2
	{
		// Token: 0x06003D4A RID: 15690 RVA: 0x0017B6E0 File Offset: 0x001798E0
		public Int2()
		{
			this.a = 0;
			this.b = 0;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x0017B6F6 File Offset: 0x001798F6
		public Int2(int a, int b)
		{
			this.a = a;
			this.b = b;
		}

		// Token: 0x04002A31 RID: 10801
		public int a;

		// Token: 0x04002A32 RID: 10802
		public int b;
	}

	// Token: 0x020006F6 RID: 1782
	public class ObjData
	{
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06003D4C RID: 15692 RVA: 0x0017B70C File Offset: 0x0017990C
		public bool isDone
		{
			get
			{
				return this._isDone;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x0017B714 File Offset: 0x00179914
		public float progress
		{
			get
			{
				return this._progress;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06003D4E RID: 15694 RVA: 0x0017B71C File Offset: 0x0017991C
		public bool cancel
		{
			get
			{
				return this._cancel;
			}
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x0017B724 File Offset: 0x00179924
		public void SetDone()
		{
			this._isDone = true;
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x0017B72D File Offset: 0x0017992D
		public void SetProgress(float p)
		{
			this._progress = p;
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x0017B736 File Offset: 0x00179936
		public void Cancel()
		{
			this._cancel = true;
		}

		// Token: 0x04002A33 RID: 10803
		private bool _isDone;

		// Token: 0x04002A34 RID: 10804
		private float _progress;

		// Token: 0x04002A35 RID: 10805
		private bool _cancel;
	}

	// Token: 0x020006F7 RID: 1783
	public class LinesRef
	{
		// Token: 0x04002A36 RID: 10806
		public string[] lines;
	}

	// Token: 0x020006F8 RID: 1784
	public class BoolRef
	{
		// Token: 0x06003D54 RID: 15700 RVA: 0x0017B73F File Offset: 0x0017993F
		public BoolRef(bool b)
		{
			this.b = b;
		}

		// Token: 0x04002A37 RID: 10807
		public bool b;
	}
}
