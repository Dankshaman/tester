using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000012 RID: 18
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Examples/Item Database")]
public class InvDatabase : MonoBehaviour
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600005F RID: 95 RVA: 0x00003CD0 File Offset: 0x00001ED0
	public static InvDatabase[] list
	{
		get
		{
			if (InvDatabase.mIsDirty)
			{
				InvDatabase.mIsDirty = false;
				InvDatabase.mList = NGUITools.FindActive<InvDatabase>();
			}
			return InvDatabase.mList;
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00003CEE File Offset: 0x00001EEE
	private void OnEnable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00003CEE File Offset: 0x00001EEE
	private void OnDisable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003CF8 File Offset: 0x00001EF8
	private InvBaseItem GetItem(int id16)
	{
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			InvBaseItem invBaseItem = this.items[i];
			if (invBaseItem.id16 == id16)
			{
				return invBaseItem;
			}
			i++;
		}
		return null;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00003D38 File Offset: 0x00001F38
	private static InvDatabase GetDatabase(int dbID)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.databaseID == dbID)
			{
				return invDatabase;
			}
			i++;
		}
		return null;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00003D70 File Offset: 0x00001F70
	public static InvBaseItem FindByID(int id32)
	{
		InvDatabase database = InvDatabase.GetDatabase(id32 >> 16);
		if (!(database != null))
		{
			return null;
		}
		return database.GetItem(id32 & 65535);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003DA0 File Offset: 0x00001FA0
	public static InvBaseItem FindByName(string exact)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			int j = 0;
			int count = invDatabase.items.Count;
			while (j < count)
			{
				InvBaseItem invBaseItem = invDatabase.items[j];
				if (invBaseItem.name == exact)
				{
					return invBaseItem;
				}
				j++;
			}
			i++;
		}
		return null;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003E04 File Offset: 0x00002004
	public static int FindItemID(InvBaseItem item)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.items.Contains(item))
			{
				return invDatabase.databaseID << 16 | item.id16;
			}
			i++;
		}
		return -1;
	}

	// Token: 0x04000041 RID: 65
	private static InvDatabase[] mList;

	// Token: 0x04000042 RID: 66
	private static bool mIsDirty = true;

	// Token: 0x04000043 RID: 67
	public int databaseID;

	// Token: 0x04000044 RID: 68
	public List<InvBaseItem> items = new List<InvBaseItem>();

	// Token: 0x04000045 RID: 69
	public UIAtlas iconAtlas;
}
