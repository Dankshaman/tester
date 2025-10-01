using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class LuaNotes
{
	// Token: 0x060013F2 RID: 5106 RVA: 0x0008432C File Offset: 0x0008252C
	public List<LuaNotes.LuaNotebookParameters> GetNotebookTabs()
	{
		List<UITab> allTabs = NetworkSingleton<NetworkUI>.Instance.GUINotepad.GetComponent<UINotebook>().GetAllTabs();
		List<LuaNotes.LuaNotebookParameters> list = new List<LuaNotes.LuaNotebookParameters>();
		for (int i = 0; i < allTabs.Count; i++)
		{
			UITab uitab = allTabs[i];
			LuaNotes.LuaNotebookParameters item = new LuaNotes.LuaNotebookParameters
			{
				index = new int?(i),
				title = uitab.title,
				body = uitab.body,
				color = Colour.LabelFromColour(uitab.VisibleColor)
			};
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x000843B8 File Offset: 0x000825B8
	public bool EditNotebookTab(LuaNotes.LuaNotebookParameters parameters)
	{
		UINotebook component = NetworkSingleton<NetworkUI>.Instance.GUINotepad.GetComponent<UINotebook>();
		if (parameters.index != null)
		{
			int? index = parameters.index;
			int num = component.GetNumTabs() - 1;
			if (!(index.GetValueOrDefault() > num & index != null))
			{
				UITab tab = component.GetTab(parameters.index.Value);
				if (tab == null)
				{
					return false;
				}
				TabState tabState = new TabState(tab);
				if (parameters.title != null)
				{
					tabState.title = parameters.title;
				}
				if (parameters.body != null)
				{
					tabState.body = parameters.body;
				}
				if (parameters.color != null)
				{
					tabState.visibleColor = new ColourState(Colour.ColourFromLabel(parameters.color));
				}
				Dictionary<int, TabState> singleChange = new Dictionary<int, TabState>
				{
					{
						parameters.index.Value,
						tabState
					}
				};
				component.NotepadTabChange(singleChange);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x00084498 File Offset: 0x00082698
	public int AddNotebookTab(LuaNotes.LuaNotebookParameters parameters)
	{
		string text = "";
		if (parameters.title != null)
		{
			text = parameters.title;
		}
		if (string.IsNullOrEmpty(text))
		{
			return -1;
		}
		string body = "";
		if (parameters.title != null)
		{
			body = parameters.body;
		}
		Color visibleColor = Colour.Grey;
		if (parameters.color != null)
		{
			visibleColor = Colour.ColourFromLabel(parameters.color);
		}
		UITab to = new UITab
		{
			title = text,
			body = body,
			VisibleColor = visibleColor
		};
		UINotebook component = NetworkSingleton<NetworkUI>.Instance.GUINotepad.GetComponent<UINotebook>();
		component.AddNewTabServer(to);
		return component.GetAllTabs().Count;
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x00084538 File Offset: 0x00082738
	public bool RemoveNotebookTab(int IndexToRemove)
	{
		UINotebook component = NetworkSingleton<NetworkUI>.Instance.GUINotepad.GetComponent<UINotebook>();
		List<UITab> allTabs = component.GetAllTabs();
		if (IndexToRemove < 0 || IndexToRemove > allTabs.Count - 1)
		{
			return false;
		}
		component.NotepadTabDelete(new TabState(allTabs[IndexToRemove]));
		component.DeleteTabOthers(allTabs[IndexToRemove]);
		return true;
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x0008458D File Offset: 0x0008278D
	public string GetNotes()
	{
		return NetworkSingleton<NetworkUI>.Instance.notepadstring;
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00084599 File Offset: 0x00082799
	public bool SetNotes(string notes)
	{
		NetworkUI instance = NetworkSingleton<NetworkUI>.Instance;
		if (notes == null)
		{
			throw new ScriptRuntimeException("Error: missing parameter or parameter was null for setNotes.");
		}
		instance.notepadstring = notes;
		return true;
	}

	// Token: 0x02000672 RID: 1650
	public class LuaNotebookParameters
	{
		// Token: 0x04002836 RID: 10294
		public int? index;

		// Token: 0x04002837 RID: 10295
		public string title;

		// Token: 0x04002838 RID: 10296
		public string body;

		// Token: 0x04002839 RID: 10297
		public string color;
	}
}
