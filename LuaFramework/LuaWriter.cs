using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LuaFramework
{
	// Token: 0x020003B1 RID: 945
	public class LuaWriter : StringWriter
	{
		// Token: 0x06002CC5 RID: 11461 RVA: 0x001396E4 File Offset: 0x001378E4
		public void AddTab()
		{
			this._tabs++;
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x001396F4 File Offset: 0x001378F4
		public void RemoveTab()
		{
			this._tabs--;
			if (this._tabs < 0)
			{
				this._tabs = 0;
			}
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x00139714 File Offset: 0x00137914
		public override void Write(string str)
		{
			if (this._waitingForTabs)
			{
				this.WriteTabs();
				this._waitingForTabs = false;
			}
			base.Write(str);
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x00139732 File Offset: 0x00137932
		public override void WriteLine()
		{
			if (this._waitingForTabs)
			{
				this.WriteTabs();
				this._waitingForTabs = false;
			}
			base.WriteLine();
			this._waitingForTabs = true;
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x00139758 File Offset: 0x00137958
		private void WriteTabs()
		{
			for (int i = 0; i < this._tabs; i++)
			{
				base.Write("\t");
			}
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x00139781 File Offset: 0x00137981
		public void SetRequiredAttribute(Type attributeType)
		{
			this._requiredAttributeType = attributeType;
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x0013978C File Offset: 0x0013798C
		public void WriteProperty<T>(T obj, string propertyName, bool multiline = true, bool trailingComma = false)
		{
			PropertyInfo property = obj.GetType().GetProperty(propertyName);
			if (property == null)
			{
				Debug.Log("ERROR " + propertyName);
			}
			this.WriteProperty<T>(obj, property, multiline, trailingComma);
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x001397D4 File Offset: 0x001379D4
		private void WriteProperty<T>(T obj, PropertyInfo propertyInfo, bool multiline = true, bool trailingComma = false)
		{
			object value = propertyInfo.GetValue(obj, null);
			Type propertyType = propertyInfo.PropertyType;
			if (value != null)
			{
				this.Write(propertyInfo.Name + " = ");
				this.WriteObject(value, propertyType, multiline, trailingComma);
			}
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x0013981A File Offset: 0x00137A1A
		public void WriteObject<T>(T obj, bool multiline = true, bool trailingComma = false)
		{
			this.WriteObject(obj, typeof(!!0), multiline, trailingComma);
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x00139834 File Offset: 0x00137A34
		private void WriteObject(object obj, Type type, bool multiline = true, bool trailingComma = false)
		{
			if (type == typeof(bool))
			{
				this.Write(((bool)obj).ToString().ToLower());
			}
			else if (type == typeof(int))
			{
				this.Write(((int)obj).ToString(CultureInfo.InvariantCulture).ToLower());
			}
			else if (type == typeof(float))
			{
				this.Write(((float)obj).ToString(CultureInfo.InvariantCulture).ToLower());
			}
			else if (type == typeof(double))
			{
				this.Write(((double)obj).ToString(CultureInfo.InvariantCulture).ToLower());
			}
			else if (type == typeof(string))
			{
				this.Write(((string)obj).ToLiteral());
			}
			else if (type == typeof(byte))
			{
				this.Write(((byte)obj).ToString(CultureInfo.InvariantCulture).ToLower());
			}
			else if (type == typeof(decimal))
			{
				this.Write(((decimal)obj).ToString(CultureInfo.InvariantCulture).ToLower());
			}
			else if (type.IsEnum)
			{
				this.Write(obj.ToString().ToLiteral());
			}
			else if (type == typeof(Color))
			{
				this.WriteColor((Color)obj);
			}
			else if (type == typeof(Color32))
			{
				this.WriteColor32((Color32)obj);
			}
			else if (type == typeof(Rect))
			{
				this.WriteRect((Rect)obj);
			}
			else if (type == typeof(Vector2))
			{
				this.WriteVector2((Vector2)obj);
			}
			else if (type == typeof(Vector3))
			{
				this.WriteVector3((Vector3)obj);
			}
			else if (type == typeof(Vector4))
			{
				this.WriteVector4((Vector4)obj);
			}
			else if (type.IsArray)
			{
				this.WriteArray((Array)obj, multiline);
			}
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				this.WriteList((IList)obj, multiline);
			}
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >))
			{
				this.WriteDictionary((IDictionary)obj, multiline);
			}
			else if (type.IsClass)
			{
				this.WriteClass<object>(obj, multiline, false);
			}
			if (trailingComma)
			{
				this.Write(", ");
				if (multiline)
				{
					this.WriteLine();
				}
			}
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x00139B30 File Offset: 0x00137D30
		private void WriteColor(Color color)
		{
			this.WriteArray(new float[]
			{
				color.r,
				color.g,
				color.b,
				color.a
			}, false);
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x00139B63 File Offset: 0x00137D63
		private void WriteColor32(Color32 color32)
		{
			this.WriteArray(new byte[]
			{
				color32.r,
				color32.g,
				color32.b,
				color32.a
			}, false);
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x00139B96 File Offset: 0x00137D96
		private void WriteRect(Rect rect)
		{
			this.WriteArray(new float[]
			{
				rect.x,
				rect.y,
				rect.width,
				rect.height
			}, false);
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x00139BCD File Offset: 0x00137DCD
		private void WriteVector2(Vector2 vector2)
		{
			this.WriteArray(new float[]
			{
				vector2.x,
				vector2.y
			}, false);
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x00139BEE File Offset: 0x00137DEE
		private void WriteVector3(Vector3 vector3)
		{
			this.WriteArray(new float[]
			{
				vector3.x,
				vector3.y,
				vector3.z
			}, false);
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x00139C18 File Offset: 0x00137E18
		private void WriteVector4(Vector4 vector4)
		{
			this.WriteArray(new float[]
			{
				vector4.x,
				vector4.y,
				vector4.z,
				vector4.w
			}, false);
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x00139C4C File Offset: 0x00137E4C
		private void WriteDictionary(IDictionary dictionary, bool multiline = true)
		{
			this.Write("{");
			if (multiline)
			{
				this.WriteLine();
			}
			this.AddTab();
			foreach (object obj in dictionary.Keys)
			{
				string text = obj as string;
				if (text != null)
				{
					if (this.IsValidTableString(text))
					{
						this.Write(text + " = ");
					}
					else
					{
						this.Write("[\"" + text + "\"] = ");
					}
				}
				else if (obj is int)
				{
					this.Write("[" + (int)obj + "] = ");
				}
				this.WriteObject(dictionary[obj], dictionary.GetType().GetGenericArguments()[1], multiline, true);
			}
			this.RemoveTab();
			this.Write("}");
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x00139D4C File Offset: 0x00137F4C
		private bool IsValidTableString(string key)
		{
			return Regex.IsMatch(key, "^\\D\\w*$");
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x00139D5C File Offset: 0x00137F5C
		private void WriteArray(Array array, bool multiline = true)
		{
			this.Write("{");
			if (multiline)
			{
				this.WriteLine();
			}
			this.AddTab();
			if (array.Rank == 1)
			{
				int length = array.GetLength(0);
				for (int i = 0; i < length; i++)
				{
					this.WriteObject(array.GetValue(i), array.GetType().GetElementType(), multiline, i < length - 1);
				}
			}
			else if (array.Rank == 2)
			{
				int length2 = array.GetLength(0);
				int length3 = array.GetLength(1);
				for (int j = 0; j < length2; j++)
				{
					this.WriteLine("{");
					this.AddTab();
					for (int k = 0; k < length3; k++)
					{
						this.WriteObject(array.GetValue(j, k), array.GetType().GetElementType(), multiline, k < length3 - 1);
					}
					this.RemoveTab();
					this.Write("}");
					if (j < length2 - 1)
					{
						this.WriteLine(",");
					}
					else
					{
						this.WriteLine();
					}
				}
			}
			this.RemoveTab();
			this.Write("}");
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x00139E70 File Offset: 0x00138070
		private void WriteList(IList list, bool multiline = true)
		{
			this.Write("{");
			if (multiline)
			{
				this.WriteLine();
			}
			this.AddTab();
			foreach (object obj in list)
			{
				this.WriteObject(obj, list.GetType().GetGenericArguments()[0], multiline, true);
			}
			this.RemoveTab();
			this.Write("}");
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x00139EFC File Offset: 0x001380FC
		private void WriteClass<T>(T obj, bool multiline = true, bool trailingComma = false)
		{
			this.Write("{");
			if (multiline)
			{
				this.WriteLine();
			}
			this.AddTab();
			ICustomLuaSerializer customLuaSerializer = obj as ICustomLuaSerializer;
			if (customLuaSerializer != null)
			{
				customLuaSerializer.Serialize(this);
			}
			else
			{
				foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
				{
					if (!(this._requiredAttributeType != null) || propertyInfo.GetCustomAttributes(this._requiredAttributeType, false).Length != 0)
					{
						this.WriteProperty<T>(obj, propertyInfo, multiline, true);
					}
				}
			}
			this.RemoveTab();
			this.Write("}");
			if (trailingComma)
			{
				this.Write(", ");
				if (multiline)
				{
					this.WriteLine();
				}
			}
		}

		// Token: 0x04001E15 RID: 7701
		private Type _requiredAttributeType;

		// Token: 0x04001E16 RID: 7702
		private int _tabs;

		// Token: 0x04001E17 RID: 7703
		private bool _waitingForTabs = true;
	}
}
