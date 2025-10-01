using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

// Token: 0x020001A4 RID: 420
public class LuaTags : IUserDataDescriptor
{
	// Token: 0x06001505 RID: 5381 RVA: 0x00089BF6 File Offset: 0x00087DF6
	public LuaTags(NetworkPhysicsObject npo)
	{
		this.NPO = npo;
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06001506 RID: 5382 RVA: 0x00089C05 File Offset: 0x00087E05
	public string Name
	{
		get
		{
			return "LuaTags";
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06001507 RID: 5383 RVA: 0x00089C0C File Offset: 0x00087E0C
	public Type Type
	{
		get
		{
			return typeof(LuaTags);
		}
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x00089C18 File Offset: 0x00087E18
	public DynValue Index(Script script, object obj, DynValue index, bool dummy)
	{
		if (this.NPO.TagIsSet(index.String))
		{
			return DynValue.True;
		}
		return DynValue.False;
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x00089C38 File Offset: 0x00087E38
	public bool SetIndex(Script script, object obj, DynValue index, DynValue value, bool dummy)
	{
		TagLabel tagLabel = new TagLabel(index.String);
		int num = NetworkSingleton<ComponentTags>.Instance.TagIndexFromLabel(tagLabel);
		if (num == -1)
		{
			num = NetworkSingleton<ComponentTags>.Instance.AddTag(tagLabel);
		}
		if (num >= 0)
		{
			this.NPO.SetTag(num, value.CastToBool());
			return true;
		}
		return false;
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x00079594 File Offset: 0x00077794
	public string AsString(object obj)
	{
		return null;
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x00079594 File Offset: 0x00077794
	public DynValue MetaIndex(Script script, object obj, string metaname)
	{
		return null;
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x00089C88 File Offset: 0x00087E88
	public bool IsTypeCompatible(Type type, object obj)
	{
		return type.IsInstanceOfType(obj);
	}

	// Token: 0x04000BF3 RID: 3059
	private NetworkPhysicsObject NPO;
}
