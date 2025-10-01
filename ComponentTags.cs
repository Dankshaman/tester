using System;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class ComponentTags : NetworkSingleton<ComponentTags>
{
	// Token: 0x060009A0 RID: 2464 RVA: 0x00044DB0 File Offset: 0x00042FB0
	private void Start()
	{
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00044DC3 File Offset: 0x00042FC3
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00044DD8 File Offset: 0x00042FD8
	public void SetTagsState(TagsState newTagsState)
	{
		this.tagsState = newTagsState;
		this.activeLabels.Clear();
		this.activeTags.Clear();
		this.tagFromLabel.Clear();
		for (int i = 0; i < this.tagsState.labels.Count; i++)
		{
			TagLabel tagLabel = this.tagsState.labels[i];
			if (tagLabel.normalized != "")
			{
				this.activeLabels[i] = tagLabel;
				this.tagFromLabel[tagLabel.normalized] = i;
				this.SetTagActive(i, true);
				this.LastTag = i;
			}
		}
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00044E7A File Offset: 0x0004307A
	public int AddTag(string label)
	{
		return this.AddTag(new TagLabel(label));
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00044E88 File Offset: 0x00043088
	public int AddTag(TagLabel tagLabel)
	{
		if (!Network.isServer)
		{
			return -1;
		}
		int num = this.tagsState.AddTag(tagLabel);
		if (num >= 0)
		{
			this.SetTagActive(num, true);
			this.activeLabels[num] = tagLabel;
			this.tagFromLabel[tagLabel.normalized] = num;
			if (num > this.LastTag)
			{
				this.LastTag = num;
			}
			base.networkView.RPC<TagLabel, int>(RPCTarget.Others, new Action<TagLabel, int>(this.RPCAddTag), tagLabel, num);
		}
		return num;
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00044F04 File Offset: 0x00043104
	private void OnPlayerConnect(NetworkPlayer player)
	{
		if (Network.isServer && (int)player.id != NetworkID.ID)
		{
			foreach (KeyValuePair<int, TagLabel> keyValuePair in this.activeLabels)
			{
				base.networkView.RPC<TagLabel, int>(player, new Action<TagLabel, int>(this.RPCAddTag), keyValuePair.Value, keyValuePair.Key);
			}
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00044F8C File Offset: 0x0004318C
	[Remote(Permission.Server)]
	private void RPCAddTag(TagLabel tagLabel, int tagIndex)
	{
		this.tagsState.SetTag(tagIndex, tagLabel);
		this.SetTagActive(tagIndex, true);
		this.activeLabels[tagIndex] = tagLabel;
		this.tagFromLabel[tagLabel.normalized] = tagIndex;
		if (tagIndex > this.LastTag)
		{
			this.LastTag = tagIndex;
		}
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x00044FE0 File Offset: 0x000431E0
	[Remote(Permission.Server)]
	private void RPCRemoveTag(int tagIndex)
	{
		this.tagsState.RemoveTag(tagIndex);
		this.tagFromLabel.Remove(this.activeLabels[tagIndex].normalized);
		this.activeLabels.Remove(tagIndex);
		this.SetTagActive(tagIndex, false);
		if (tagIndex == this.LastTag && tagIndex > 0)
		{
			this.LastTag = -1;
			for (int i = tagIndex - 1; i >= 0; i--)
			{
				if (this.IsTagActive(i))
				{
					this.LastTag = i;
					return;
				}
			}
		}
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00045060 File Offset: 0x00043260
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	public void RPCRequestTagForNPO(TagLabel tagLabel, NetworkPhysicsObject npo)
	{
		if (!Network.isServer)
		{
			return;
		}
		int num = this.TagIndexFromLabel(tagLabel);
		if (num == -1)
		{
			num = this.AddTag(tagLabel);
		}
		if (num >= 0)
		{
			npo.SetTag(num, true);
		}
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00045098 File Offset: 0x00043298
	public bool CheckRemoveTag(List<NetworkPhysicsObject> npos, int tagIndex)
	{
		if (!Network.isServer)
		{
			return false;
		}
		if (npos != null)
		{
			for (int i = 0; i < npos.Count; i++)
			{
				if (npos[i].TagIsSet(tagIndex))
				{
					return false;
				}
			}
		}
		if (NetworkSingleton<SnapPointManager>.Instance.TagIsSet(tagIndex))
		{
			return false;
		}
		if (this.tagsState.RemoveTag(tagIndex))
		{
			this.tagFromLabel.Remove(this.activeLabels[tagIndex].normalized);
			this.activeLabels.Remove(tagIndex);
			this.SetTagActive(tagIndex, false);
			if (tagIndex == this.LastTag && tagIndex > 0)
			{
				this.LastTag = -1;
				for (int j = tagIndex - 1; j >= 0; j--)
				{
					if (this.IsTagActive(j))
					{
						this.LastTag = j;
						break;
					}
				}
			}
			base.networkView.RPC<int>(RPCTarget.Others, new Action<int>(this.RPCRemoveTag), tagIndex);
			return true;
		}
		return false;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00045175 File Offset: 0x00043375
	public bool IsTagActive(int flagIndex)
	{
		return ComponentTags.GetFlag(this.activeTags, flagIndex);
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00045183 File Offset: 0x00043383
	public void SetTagActive(int flagIndex, bool active)
	{
		ComponentTags.SetFlag(ref this.activeTags, flagIndex, active);
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00045192 File Offset: 0x00043392
	public TagLabel TagLabelFromIndex(int flagIndex)
	{
		return this.activeLabels[flagIndex];
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x000451A0 File Offset: 0x000433A0
	public int TagIndexFromLabel(TagLabel label)
	{
		int result;
		if (this.tagFromLabel.TryGetValue(label.normalized, out result))
		{
			return result;
		}
		return -1;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x000451C8 File Offset: 0x000433C8
	public int TagIndexFromLabel(string normalizedLabel)
	{
		int result;
		if (this.tagFromLabel.TryGetValue(normalizedLabel, out result))
		{
			return result;
		}
		return -1;
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000451E8 File Offset: 0x000433E8
	public List<TagLabel> TagLabelsFromTags(List<ulong> flags)
	{
		List<TagLabel> list = new List<TagLabel>();
		for (int num = ComponentTags.NextSetFlag(flags, -1); num != -1; num = ComponentTags.NextSetFlag(flags, num))
		{
			list.Add(this.TagLabelFromIndex(num));
		}
		return list;
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00045220 File Offset: 0x00043420
	public List<string> DisplayedTagLabelsFromTags(List<ulong> flags)
	{
		List<string> list = new List<string>();
		for (int num = ComponentTags.NextSetFlag(flags, -1); num != -1; num = ComponentTags.NextSetFlag(flags, num))
		{
			list.Add(this.TagLabelFromIndex(num).displayed);
		}
		return list;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0004525C File Offset: 0x0004345C
	public List<ulong> FlagsFromTagLabels(List<TagLabel> tagLabels)
	{
		List<ulong> result = new List<ulong>();
		for (int i = 0; i < tagLabels.Count; i++)
		{
			int num = this.TagIndexFromLabel(tagLabels[i]);
			if (num == -1)
			{
				num = this.AddTag(tagLabels[i]);
			}
			if (num != -1)
			{
				ComponentTags.SetFlag(ref result, num, true);
			}
		}
		return result;
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x000452B0 File Offset: 0x000434B0
	public List<ulong> FlagsFromDisplayedTagLabels(List<string> displayedTagLabels)
	{
		List<ulong> result = new List<ulong>();
		if (displayedTagLabels != null)
		{
			for (int i = 0; i < displayedTagLabels.Count; i++)
			{
				TagLabel tagLabel = new TagLabel(displayedTagLabels[i]);
				int num = this.TagIndexFromLabel(tagLabel);
				if (num == -1)
				{
					num = this.AddTag(tagLabel);
				}
				if (num != -1)
				{
					ComponentTags.SetFlag(ref result, num, true);
				}
			}
		}
		return result;
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x00045308 File Offset: 0x00043508
	public static void SetFlag(ref List<ulong> flags, int flagIndex, bool enabled)
	{
		int i = flagIndex / 64;
		if (enabled)
		{
			while (i >= flags.Count)
			{
				flags.Add(0UL);
			}
		}
		else if (i >= flags.Count)
		{
			return;
		}
		List<ulong> list;
		int index;
		if (enabled)
		{
			list = flags;
			index = i;
			list[index] |= 1UL << flagIndex % 64;
			return;
		}
		list = flags;
		index = i;
		list[index] &= ~(1UL << flagIndex % 64);
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x00045380 File Offset: 0x00043580
	public static bool GetFlag(List<ulong> flags, int flagIndex)
	{
		int num = flagIndex / 64;
		return num < flags.Count && (flags[num] & 1UL << flagIndex % 64) > 0UL;
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x000453B4 File Offset: 0x000435B4
	public static int NextSetFlag(List<ulong> flags, int flagIndex = -1)
	{
		flagIndex++;
		for (int i = flagIndex / 64; i < flags.Count; i = flagIndex / 64)
		{
			if ((flags[i] & 1UL << flagIndex % 64) != 0UL)
			{
				return flagIndex;
			}
			flagIndex++;
		}
		return -1;
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x000453F8 File Offset: 0x000435F8
	public static void AndFlags(ref List<ulong> flags, List<ulong> otherFlags)
	{
		if (flags.Count > otherFlags.Count)
		{
			flags.RemoveRange(otherFlags.Count, flags.Count - otherFlags.Count);
		}
		for (int i = 0; i < flags.Count; i++)
		{
			List<ulong> list = flags;
			int index = i;
			list[index] &= otherFlags[i];
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0004545C File Offset: 0x0004365C
	public static void CopyFlags(ref List<ulong> flags, List<ulong> otherFlags)
	{
		for (int i = 0; i < flags.Count; i++)
		{
			if (i < otherFlags.Count)
			{
				flags[i] = otherFlags[i];
			}
		}
		for (int j = flags.Count; j < otherFlags.Count; j++)
		{
			flags.Add(otherFlags[j]);
		}
		if (flags.Count > otherFlags.Count)
		{
			flags.RemoveRange(otherFlags.Count, flags.Count - otherFlags.Count);
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x000454E4 File Offset: 0x000436E4
	public static void RemoveFlags(ref List<ulong> flags, List<ulong> otherFlags)
	{
		int num = 0;
		while (num < flags.Count && num < otherFlags.Count)
		{
			List<ulong> list = flags;
			int index = num;
			list[index] &= ~otherFlags[num];
			num++;
		}
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00045528 File Offset: 0x00043728
	public static List<ulong> NewCopyOfFlags(List<ulong> tagFlags)
	{
		List<ulong> list = new List<ulong>();
		if (tagFlags != null)
		{
			for (int i = 0; i < tagFlags.Count; i++)
			{
				list.Add(tagFlags[i]);
			}
		}
		return list;
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00045560 File Offset: 0x00043760
	public static int GetFlagCount(List<ulong> flags)
	{
		int num = 0;
		for (int i = 0; i < flags.Count; i++)
		{
			num += LibBytes.NumberOfSetBits(flags[i]);
		}
		return num;
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00045590 File Offset: 0x00043790
	public static bool HaveMatchingFlag(List<ulong> flagsA, List<ulong> flagsB)
	{
		for (int i = 0; i < Mathf.Min(flagsA.Count, flagsB.Count); i++)
		{
			if ((flagsA[i] & flagsB[i]) != 0UL)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x000455D0 File Offset: 0x000437D0
	public static bool FlagsAreIdentical(List<ulong> flagsA, List<ulong> flagsB)
	{
		if (flagsA.Count != flagsB.Count)
		{
			return false;
		}
		for (int i = 0; i < flagsA.Count; i++)
		{
			if (flagsA[i] != flagsB[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00045614 File Offset: 0x00043814
	public static bool HasAnyFlag(List<ulong> flags)
	{
		for (int i = 0; i < flags.Count; i++)
		{
			if (flags[i] != 0UL)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0004563E File Offset: 0x0004383E
	public void Reset()
	{
		this.SetTagsState(new TagsState());
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0004564C File Offset: 0x0004384C
	public static bool NormalizedLabelsAreEqual(List<string> a, List<string> b)
	{
		if (a == null && b == null)
		{
			return true;
		}
		if (a == null || b == null || a.Count != b.Count)
		{
			return false;
		}
		for (int i = 0; i < a.Count; i++)
		{
			if (LibString.NormalizedTag(a[i]) != LibString.NormalizedTag(b[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040006FB RID: 1787
	[NonSerialized]
	public TagsState tagsState = new TagsState();

	// Token: 0x040006FC RID: 1788
	[NonSerialized]
	public Dictionary<int, TagLabel> activeLabels = new Dictionary<int, TagLabel>();

	// Token: 0x040006FD RID: 1789
	private List<ulong> activeTags = new List<ulong>();

	// Token: 0x040006FE RID: 1790
	private Dictionary<string, int> tagFromLabel = new Dictionary<string, int>();

	// Token: 0x040006FF RID: 1791
	[NonSerialized]
	public int LastTag = -1;
}
