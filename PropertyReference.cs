using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x02000064 RID: 100
[Serializable]
public class PropertyReference
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600040B RID: 1035 RVA: 0x0001D889 File Offset: 0x0001BA89
	// (set) Token: 0x0600040C RID: 1036 RVA: 0x0001D891 File Offset: 0x0001BA91
	public Component target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001D8A8 File Offset: 0x0001BAA8
	// (set) Token: 0x0600040E RID: 1038 RVA: 0x0001D8B0 File Offset: 0x0001BAB0
	public string name
	{
		get
		{
			return this.mName;
		}
		set
		{
			this.mName = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001D8C7 File Offset: 0x0001BAC7
	public bool isValid
	{
		get
		{
			return this.mTarget != null && !string.IsNullOrEmpty(this.mName);
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000410 RID: 1040 RVA: 0x0001D8E8 File Offset: 0x0001BAE8
	public bool isEnabled
	{
		get
		{
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget as MonoBehaviour;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00002594 File Offset: 0x00000794
	public PropertyReference()
	{
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001D922 File Offset: 0x0001BB22
	public PropertyReference(Component target, string fieldName)
	{
		this.mTarget = target;
		this.mName = fieldName;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0001D938 File Offset: 0x0001BB38
	public Type GetPropertyType()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			return this.mProperty.PropertyType;
		}
		if (this.mField != null)
		{
			return this.mField.FieldType;
		}
		return typeof(void);
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0001D9B0 File Offset: 0x0001BBB0
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is PropertyReference)
		{
			PropertyReference propertyReference = obj as PropertyReference;
			return this.mTarget == propertyReference.mTarget && string.Equals(this.mName, propertyReference.mName);
		}
		return false;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001DA01 File Offset: 0x0001BC01
	public override int GetHashCode()
	{
		return PropertyReference.s_Hash;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0001DA08 File Offset: 0x0001BC08
	public void Set(Component target, string methodName)
	{
		this.mTarget = target;
		this.mName = methodName;
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0001DA18 File Offset: 0x0001BC18
	public void Clear()
	{
		this.mTarget = null;
		this.mName = null;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0001DA28 File Offset: 0x0001BC28
	public void Reset()
	{
		this.mField = null;
		this.mProperty = null;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001DA38 File Offset: 0x0001BC38
	public override string ToString()
	{
		return PropertyReference.ToString(this.mTarget, this.name);
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001DA4C File Offset: 0x0001BC4C
	public static string ToString(Component comp, string property)
	{
		if (!(comp != null))
		{
			return null;
		}
		string text = comp.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(property))
		{
			return text + "." + property;
		}
		return text + ".[property]";
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001DAA8 File Offset: 0x0001BCA8
	[DebuggerHidden]
	[DebuggerStepThrough]
	public object Get()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			if (this.mProperty.CanRead)
			{
				return this.mProperty.GetValue(this.mTarget, null);
			}
		}
		else if (this.mField != null)
		{
			return this.mField.GetValue(this.mTarget);
		}
		return null;
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001DB30 File Offset: 0x0001BD30
	[DebuggerHidden]
	[DebuggerStepThrough]
	public bool Set(object value)
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty == null && this.mField == null)
		{
			return false;
		}
		if (value == null)
		{
			try
			{
				if (!(this.mProperty != null))
				{
					this.mField.SetValue(this.mTarget, null);
					return true;
				}
				if (this.mProperty.CanWrite)
				{
					this.mProperty.SetValue(this.mTarget, null, null);
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		if (!this.Convert(ref value))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Unable to convert ",
					value.GetType(),
					" to ",
					this.GetPropertyType()
				}));
			}
		}
		else
		{
			if (this.mField != null)
			{
				this.mField.SetValue(this.mTarget, value);
				return true;
			}
			if (this.mProperty.CanWrite)
			{
				this.mProperty.SetValue(this.mTarget, value, null);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001DC80 File Offset: 0x0001BE80
	[DebuggerHidden]
	[DebuggerStepThrough]
	private bool Cache()
	{
		if (this.mTarget != null && !string.IsNullOrEmpty(this.mName))
		{
			Type type = this.mTarget.GetType();
			this.mField = type.GetField(this.mName);
			this.mProperty = type.GetProperty(this.mName);
		}
		else
		{
			this.mField = null;
			this.mProperty = null;
		}
		return this.mField != null || this.mProperty != null;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001DD04 File Offset: 0x0001BF04
	private bool Convert(ref object value)
	{
		if (this.mTarget == null)
		{
			return false;
		}
		Type propertyType = this.GetPropertyType();
		Type from;
		if (value == null)
		{
			if (!propertyType.IsClass)
			{
				return false;
			}
			from = propertyType;
		}
		else
		{
			from = value.GetType();
		}
		return PropertyReference.Convert(ref value, from, propertyType);
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0001DD4C File Offset: 0x0001BF4C
	public static bool Convert(Type from, Type to)
	{
		object obj = null;
		return PropertyReference.Convert(ref obj, from, to);
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0001DD64 File Offset: 0x0001BF64
	public static bool Convert(object value, Type to)
	{
		if (value == null)
		{
			value = null;
			return PropertyReference.Convert(ref value, to, to);
		}
		return PropertyReference.Convert(ref value, value.GetType(), to);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0001DD84 File Offset: 0x0001BF84
	public static bool Convert(ref object value, Type from, Type to)
	{
		if (to.IsAssignableFrom(from))
		{
			return true;
		}
		if (to == typeof(string))
		{
			value = ((value != null) ? value.ToString() : "null");
			return true;
		}
		if (value == null)
		{
			return false;
		}
		if (to == typeof(int))
		{
			if (from == typeof(string))
			{
				int num;
				if (int.TryParse((string)value, out num))
				{
					value = num;
					return true;
				}
			}
			else
			{
				if (from == typeof(float))
				{
					value = Mathf.RoundToInt((float)value);
					return true;
				}
				if (from == typeof(double))
				{
					value = (int)Math.Round((double)value);
				}
			}
		}
		else if (to == typeof(float))
		{
			if (from == typeof(string))
			{
				float num2;
				if (float.TryParse((string)value, out num2))
				{
					value = num2;
					return true;
				}
			}
			else if (from == typeof(double))
			{
				value = (float)((double)value);
			}
			else if (from == typeof(int))
			{
				value = (float)((int)value);
			}
		}
		else if (to == typeof(double))
		{
			if (from == typeof(string))
			{
				double num3;
				if (double.TryParse((string)value, out num3))
				{
					value = num3;
					return true;
				}
			}
			else if (from == typeof(float))
			{
				value = (double)((float)value);
			}
			else if (from == typeof(int))
			{
				value = (double)((int)value);
			}
		}
		return false;
	}

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private Component mTarget;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	private string mName;

	// Token: 0x040002E4 RID: 740
	private FieldInfo mField;

	// Token: 0x040002E5 RID: 741
	private PropertyInfo mProperty;

	// Token: 0x040002E6 RID: 742
	private static int s_Hash = "PropertyBinding".GetHashCode();
}
