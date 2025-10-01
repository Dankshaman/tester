using System;
using System.Collections.Generic;

// Token: 0x020001FB RID: 507
public class TagsState : IEquatable<TagsState>
{
	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06001A44 RID: 6724 RVA: 0x000B93AC File Offset: 0x000B75AC
	public List<TagLabel> labels { get; } = new List<TagLabel>();

	// Token: 0x06001A45 RID: 6725 RVA: 0x000B93B4 File Offset: 0x000B75B4
	public void SetTag(int tagIndex, TagLabel tagLabel)
	{
		while (this.labels.Count <= tagIndex)
		{
			this.labels.Add(new TagLabel(""));
		}
		this.labels[tagIndex] = tagLabel;
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x000B93E8 File Offset: 0x000B75E8
	public int AddTag(TagLabel label)
	{
		if (label.normalized == "")
		{
			return -1;
		}
		int num = -1;
		for (int i = 0; i < this.labels.Count; i++)
		{
			if (this.labels[i].normalized == "" || this.labels[i].normalized == label.normalized)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			this.labels[num] = label;
			return num;
		}
		this.labels.Add(label);
		return this.labels.Count - 1;
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x000B948C File Offset: 0x000B768C
	public int RemoveTag(string label)
	{
		if (label == "")
		{
			return -1;
		}
		string b = LibString.NormalizedTag(label);
		for (int i = 0; i < this.labels.Count; i++)
		{
			if (this.labels[i].normalized == b)
			{
				this.labels[i] = new TagLabel("");
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x000B94F7 File Offset: 0x000B76F7
	public bool RemoveTag(int flagIndex)
	{
		if (flagIndex < this.labels.Count)
		{
			this.labels[flagIndex] = new TagLabel("");
			return true;
		}
		return false;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x000B9520 File Offset: 0x000B7720
	public bool Equals(TagsState other)
	{
		return other != null && (this == other || this.labels.SequenceEqualNullable(other.labels));
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x000B953E File Offset: 0x000B773E
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((TagsState)obj)));
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x000B956C File Offset: 0x000B776C
	public override int GetHashCode()
	{
		if (this.labels == null)
		{
			return 0;
		}
		return this.labels.GetHashCode();
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(TagsState left, TagsState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(TagsState left, TagsState right)
	{
		return !object.Equals(left, right);
	}
}
