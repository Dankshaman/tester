using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

// Token: 0x0200022C RID: 556
public sealed class PhysicsStateDeserializationBinder : SerializationBinder
{
	// Token: 0x06001BA2 RID: 7074 RVA: 0x000BE89C File Offset: 0x000BCA9C
	public override Type BindToType(string assemblyName, string typeName)
	{
		if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(typeName))
		{
			return null;
		}
		assemblyName = Assembly.GetExecutingAssembly().FullName;
		Type type = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
		bool flag = true;
		if (PhysicsStateDeserializationBinder.WhiteListedNameType.Contains(typeName))
		{
			flag = false;
		}
		for (int i = 0; i < PhysicsStateDeserializationBinder.WhiteListedStartNameType.Count; i++)
		{
			if (typeName.StartsWith(PhysicsStateDeserializationBinder.WhiteListedStartNameType[i]))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			throw new Exception("Invalid type: " + typeName);
		}
		return type;
	}

	// Token: 0x04001171 RID: 4465
	private static List<string> WhiteListedStartNameType = new List<string>
	{
		"System.Collections.Generic.List`1[[PhysicsState, Assembly-CSharp,",
		"System.Collections.Generic.List`1[[System.String, mscorlib,",
		"System.Collections.Generic.List`1[[System.Int32, mscorlib,"
	};

	// Token: 0x04001172 RID: 4466
	private static List<string> WhiteListedNameType = new List<string>
	{
		"PhysicsState",
		"Mode"
	};
}
