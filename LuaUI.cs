using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

// Token: 0x02000194 RID: 404
public class LuaUI
{
	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060013DC RID: 5084 RVA: 0x000840FB File Offset: 0x000822FB
	// (set) Token: 0x060013DD RID: 5085 RVA: 0x00084103 File Offset: 0x00082303
	[MoonSharpHidden]
	public XmlUIScript xmlScript { get; private set; }

	// Token: 0x060013DE RID: 5086 RVA: 0x0008410C File Offset: 0x0008230C
	[MoonSharpHidden]
	public LuaUI(XmlUIScript xml)
	{
		this.xmlScript = xml;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x00084126 File Offset: 0x00082326
	public bool SetAttribute(string id, string name, string value)
	{
		return this.xmlScript.SetAttribute(id, name, value);
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x00084136 File Offset: 0x00082336
	public bool SetAttributes(string id, Dictionary<string, string> attributes)
	{
		return this.xmlScript.SetAttributes(id, attributes);
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x00084145 File Offset: 0x00082345
	public string GetAttribute(string id, string name)
	{
		return this.xmlScript.GetAttribute(id, name);
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x00084154 File Offset: 0x00082354
	public Dictionary<string, string> GetAttributes(string id)
	{
		return this.xmlScript.GetAttributes(id);
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x00084162 File Offset: 0x00082362
	public string GetValue(string id)
	{
		return this.xmlScript.GetValue(id);
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x00084170 File Offset: 0x00082370
	public bool SetValue(string id, string value)
	{
		return this.xmlScript.SetValue(id, value);
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x0008417F File Offset: 0x0008237F
	public string GetXml()
	{
		return this.xmlScript.GetXml();
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x0008418C File Offset: 0x0008238C
	public bool SetXml(string xml)
	{
		return this.xmlScript.SetXml(xml);
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x0008419A File Offset: 0x0008239A
	public bool SetXml(string xml, List<LuaUI.LuaCustomAsset> assets)
	{
		this.SetAssets(assets);
		return this.xmlScript.SetXml(xml);
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x000841AF File Offset: 0x000823AF
	public Table GetXmlTable(Script script)
	{
		return this.xmlScript.GetXmlTable(script);
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x000841BD File Offset: 0x000823BD
	public bool SetXmlTable(Script script, Table table)
	{
		return this.xmlScript.SetXmlTable(script, table);
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x000841CC File Offset: 0x000823CC
	public bool SetXmlTable(Script script, Table table, List<LuaUI.LuaCustomAsset> assets)
	{
		this.SetAssets(assets);
		return this.xmlScript.SetXmlTable(script, table);
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x000841E2 File Offset: 0x000823E2
	public bool Show(string id)
	{
		return this.xmlScript.Show(id);
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x000841F0 File Offset: 0x000823F0
	public bool Hide(string id)
	{
		return this.xmlScript.Hide(id);
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x000841FE File Offset: 0x000823FE
	public bool SetClass(string id, string name)
	{
		return this.xmlScript.SetClass(id, name);
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x00084210 File Offset: 0x00082410
	public List<LuaUI.LuaCustomAsset> GetCustomAssets()
	{
		List<LuaUI.LuaCustomAsset> list = new List<LuaUI.LuaCustomAsset>(this.xmlScript.CustomAssets.Count);
		foreach (CustomAssetState customAssetState in this.xmlScript.CustomAssets)
		{
			list.Add(new LuaUI.LuaCustomAsset(customAssetState));
		}
		return list;
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x00084284 File Offset: 0x00082484
	public bool SetCustomAssets(List<LuaUI.LuaCustomAsset> assets)
	{
		this.SetAssets(assets);
		this.SetXml(this.GetXml());
		return true;
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0008429C File Offset: 0x0008249C
	private void SetAssets(List<LuaUI.LuaCustomAsset> assets)
	{
		if (assets == null)
		{
			assets = new List<LuaUI.LuaCustomAsset>();
		}
		List<CustomAssetState> list = new List<CustomAssetState>(assets.Count);
		foreach (LuaUI.LuaCustomAsset luaCustomAsset in assets)
		{
			list.Add(luaCustomAsset.ToCustomAssetState());
		}
		this.xmlScript.CleanupCustomAssets();
		this.xmlScript.CustomAssets = list;
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060013F1 RID: 5105 RVA: 0x0008431C File Offset: 0x0008251C
	public bool loading
	{
		get
		{
			return this.xmlScript.Loading;
		}
	}

	// Token: 0x04000BBE RID: 3006
	public LuaUI.LuaCustomAssetType AssetType = new LuaUI.LuaCustomAssetType();

	// Token: 0x02000670 RID: 1648
	public class LuaCustomAsset
	{
		// Token: 0x06003B7F RID: 15231 RVA: 0x00002594 File Offset: 0x00000794
		public LuaCustomAsset()
		{
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x00177482 File Offset: 0x00175682
		public LuaCustomAsset(CustomAssetState customAssetState)
		{
			this.type = customAssetState.Type;
			this.name = customAssetState.Name;
			this.url = customAssetState.URL;
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x001774AE File Offset: 0x001756AE
		public CustomAssetState ToCustomAssetState()
		{
			return new CustomAssetState
			{
				Type = this.type,
				Name = this.name,
				URL = this.url
			};
		}

		// Token: 0x04002833 RID: 10291
		public CustomAssetType type;

		// Token: 0x04002834 RID: 10292
		public string name;

		// Token: 0x04002835 RID: 10293
		public string url;
	}

	// Token: 0x02000671 RID: 1649
	public class LuaCustomAssetType : LuaEnum
	{
		// Token: 0x06003B82 RID: 15234 RVA: 0x001774D9 File Offset: 0x001756D9
		internal LuaCustomAssetType() : base(typeof(CustomAssetType))
		{
		}
	}
}
